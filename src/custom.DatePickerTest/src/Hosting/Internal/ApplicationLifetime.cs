// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Hosting.Internal
{
    using System;
    using System.Threading;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    internal sealed class ApplicationLifetime : IHostApplicationLifetime, IDisposable
    {
        private readonly CancellationTokenSource startedSource = new CancellationTokenSource();
        private readonly CancellationTokenSource stoppingSource = new CancellationTokenSource();
        private readonly CancellationTokenSource stoppedSource = new CancellationTokenSource();
        private readonly ILogger<ApplicationLifetime> logger;
        private bool disposed;

        public ApplicationLifetime(ILogger<ApplicationLifetime> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Gets triggered when the application host has fully started and is about to wait
        /// for a graceful shutdown.
        /// </summary>
        public CancellationToken ApplicationStarted => this.startedSource.Token;

        /// <summary>
        /// Gets triggered when the application host is performing a graceful shutdown.
        /// Request may still be in flight. Shutdown will block until this event completes.
        /// </summary>
        public CancellationToken ApplicationStopping => this.stoppingSource.Token;

        /// <summary>
        /// Gets triggered when the application host is performing a graceful shutdown.
        /// All requests should be complete at this point. Shutdown will block
        /// until this event completes.
        /// </summary>
        public CancellationToken ApplicationStopped => this.stoppedSource.Token;

        /// <summary>
        /// Signals the ApplicationStopping event and blocks until it completes.
        /// </summary>
        public void StopApplication()
        {
            // Lock on CTS to synchronize multiple calls to StopApplication. This guarantees that the first call
            // to StopApplication and its callbacks run to completion before subsequent calls to StopApplication,
            // which will no-op since the first call already requested cancellation, get a chance to execute.
            lock (this.stoppingSource)
            {
                try
                {
                    this.ExecuteHandlers(this.stoppingSource);
                }
                catch (Exception ex)
                {
                    this.logger.ApplicationError(LoggerEventIds.APPLICATIONSTOPPINGEXCEPTION, "An error occurred stopping the application", ex);
                }
            }
        }

        /// <summary>
        /// Signals the ApplicationStarted event and blocks until it completes.
        /// </summary>
        public void NotifyStarted()
        {
            try
            {
                this.ExecuteHandlers(this.startedSource);
            }
            catch (Exception ex)
            {
                this.logger.ApplicationError(LoggerEventIds.APPLICATIONSTARTUPEXCEPTION, "An error occurred starting the application", ex);
            }
        }

        /// <summary>
        /// Signals the ApplicationStopped event and blocks until it completes.
        /// </summary>
        public void NotifyStopped()
        {
            try
            {
                this.ExecuteHandlers(this.stoppedSource);
            }
            catch (Exception ex)
            {
                this.logger.ApplicationError(LoggerEventIds.APPLICATIONSTOPPEDEXCEPTION, "An error occurred stopping the application", ex);
            }
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            this.startedSource.Dispose();
            this.stoppingSource.Dispose();
            this.stoppedSource.Dispose();
        }

        private void ExecuteHandlers(CancellationTokenSource cancel)
        {
            // Noop if this is already cancelled
            if (cancel.IsCancellationRequested)
            {
                return;
            }

            // Run the cancellation token callbacks
            cancel.Cancel(throwOnFirstException: false);
        }

        private void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }
    }
}
