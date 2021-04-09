// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Hosting.Internal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.Options;

    // Summary:
    internal class AppDomainLifetime : IHostLifetime, IDisposable
    {
        private readonly IHostApplicationLifetime applicationLifetime;
        private readonly IHostEnvironment environment;
        private readonly ILogger logger;
        private readonly ConsoleLifetimeOptions options;
        private CancellationTokenRegistration? applicationStartedRegistration;
        private CancellationTokenRegistration? applicationStoppedRegistration;
        private CancellationTokenRegistration? applicationStoppingRegistration;

        public AppDomainLifetime(IOptions<ConsoleLifetimeOptions> options, IHostEnvironment environment, IHostApplicationLifetime applicationLifetime)
            : this(options, environment, applicationLifetime, NullLoggerFactory.Instance)
        {
        }

        public AppDomainLifetime(IOptions<ConsoleLifetimeOptions> options, IHostEnvironment environment, IHostApplicationLifetime applicationLifetime, ILoggerFactory loggerFactory)
        {
            var obj = options?.Value;
            this.options = obj ?? throw new ArgumentNullException(nameof(options));
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
            this.applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
            this.logger = loggerFactory.CreateLogger<AppDomainLifetime>();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.applicationLifetime.StopApplication();
            return Task.CompletedTask;
        }

        public async Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            if (!this.options.SuppressStatusMessages)
            {
#if NETSTANDARD2_0
                this.applicationStartedRegistration?.Dispose();
                this.applicationStoppingRegistration?.Dispose();
                this.applicationStoppedRegistration?.Dispose();
#else
                await (this.applicationStartedRegistration?.DisposeAsync() ?? default);
                await (this.applicationStoppingRegistration?.DisposeAsync() ?? default);
                await (this.applicationStoppedRegistration?.DisposeAsync() ?? default);
#endif
                this.applicationStartedRegistration = this.applicationLifetime.ApplicationStarted.Register(this.OnApplicationStarted, false);
                this.applicationStoppingRegistration = this.applicationLifetime.ApplicationStopping.Register(this.OnApplicationStopping, false);
                this.applicationStoppedRegistration = this.applicationLifetime.ApplicationStopped.Register(this.OnApplicationStopped, false);
            }

#if NETSTANDARD2_0
            await Task.CompletedTask;
#endif
        }

        protected virtual void Dispose(bool disposing)
        {
            this.applicationStartedRegistration?.Dispose();
            this.applicationStoppingRegistration?.Dispose();
            this.applicationStoppedRegistration?.Dispose();
        }

        private void OnApplicationStarted()
        {
            this.logger.LogInformation("Application started. Press Ctrl+C to shut down.");
            this.logger.LogInformation("Hosting environment: {envName}", this.environment.EnvironmentName);
            this.logger.LogInformation("Content root path: {contentRoot}", this.environment.ContentRootPath);
        }

        private void OnApplicationStopped()
        {
            this.logger.LogInformation("Application has stopped...");
        }

        private void OnApplicationStopping()
        {
            this.logger.LogInformation("Application is shutting down...");
        }
    }
}
