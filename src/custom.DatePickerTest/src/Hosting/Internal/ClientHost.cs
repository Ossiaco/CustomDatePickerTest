// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Hosting.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Custom.DatePickerTest.Exceptions;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    internal sealed class ClientHost : IHost, IDisposable
    {
        private readonly ApplicationLifetime applicationLifetime;
        private readonly IEnumerable<IHostedService> hostedServices;
        private readonly IHostLifetime hostLifetime;
        private readonly ILogger<ClientHost> logger;
        private readonly HostOptions options;

        public ClientHost(IServiceProvider services, IHostApplicationLifetime applicationLifetime, ILogger<ClientHost> logger, IHostLifetime hostLifetime, IOptions<HostOptions> options)
        {
            Guard.Throw.ArgumentNullException(services == null, nameof(services));
            Guard.Throw.ArgumentNullException(applicationLifetime == null, nameof(applicationLifetime));
            Guard.Throw.ArgumentNullException(logger == null, nameof(logger));
            Guard.Throw.ArgumentNullException(hostLifetime == null, nameof(hostLifetime));
            this.Services = services;
            this.applicationLifetime = (ApplicationLifetime)applicationLifetime;
            this.logger = logger;
            this.hostLifetime = hostLifetime;
            this.hostedServices = this.Services.GetService<IEnumerable<IHostedService>>() ?? new Collection<IHostedService>();
            Guard.Throw.ArgumentNullException(options?.Value == null, nameof(options));
            this.options = options!.Value;
        }

        public IServiceProvider Services
        {
            get;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP007:Don't dispose injected.", Justification = "Host owns it")]
        public void Dispose()
        {
            AppDomain.CurrentDomain.ProcessExit -= this.OnProcessExit;
            (this.Services as IDisposable)?.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            this.logger.Starting();
            AppDomain.CurrentDomain.ProcessExit += this.OnProcessExit;
            await this.hostLifetime.WaitForStartAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            foreach (var hostedService in this.hostedServices)
            {
                await hostedService.StartAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            }

            this.applicationLifetime?.NotifyStarted();
            this.logger.Started();
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            this.logger.Stopping();
            using (var cts = new CancellationTokenSource(this.options.ShutdownTimeout))
            {
                using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, cancellationToken))
                {
                    var token = linkedCts.Token;
                    this.applicationLifetime?.StopApplication();
                    IList<Exception> exceptions = new List<Exception>();
                    if (this.hostedServices != null)
                    {
                        foreach (var item2 in this.hostedServices.Reverse())
                        {
                            token.ThrowIfCancellationRequested();
                            try
                            {
                                await item2.StopAsync(token).ConfigureAwait(continueOnCapturedContext: false);
                            }
                            catch (Exception item)
                            {
                                exceptions.Add(item);
                            }
                        }
                    }

                    token.ThrowIfCancellationRequested();
                    await this.hostLifetime.StopAsync(token);
                    this.applicationLifetime?.NotifyStopped();
                    if (exceptions.Count > 0)
                    {
                        var ex = new AggregateException("One or more hosted services failed to stop.", exceptions);
                        this.logger.StoppedWithException(ex);
                        throw ex;
                    }
                }
            }

            this.logger.Stopped();
        }

#if NETCOREAPP3_0
        private void OnProcessExit(object? sender, EventArgs e)
        {
            StopAsync().Wait();
        }
#else
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD002:Avoid problematic synchronous waits", Justification = "No choice")]
        private void OnProcessExit(object? sender, EventArgs e)
        {
            this.StopAsync().Wait();
        }
#endif

    }
}