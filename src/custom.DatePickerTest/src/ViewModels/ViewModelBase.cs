// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.ViewModels
{
    using System;
    using System.Reactive;
    using System.Reactive.Concurrency;
    using System.Reactive.Disposables;
    using ReactiveUI;
    using Splat;

    /// <summary>
    /// A view model base.
    /// </summary>
    ///
    /// <seealso cref="T:ReactiveUI.ReactiveObject"/>
    /// <seealso cref="T:ReactiveUI.IRoutableViewModel"/>
    public class ViewModelBase : ReactiveObject, IRoutableViewModel, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        ///
        /// <param name="title">                    The title. </param>
        /// <param name="mainThreadScheduler">      (Optional) Gets the scheduler for scheduling
        ///                                         operations on the main thread. </param>
        /// <param name="taskPoolScheduler">        (Optional) Gets the scheduler for scheduling
        ///                                         operations on the task pool. </param>
        /// <param name="hostScreen">               (Optional) Gets the IScreen that this ViewModel is
        ///                                         currently being shown in. This is usually passed into
        ///                                         the ViewModel in the Constructor and saved as a
        ///                                         ReadOnly Property. </param>
        public ViewModelBase(string title, IScheduler? mainThreadScheduler = null, IScheduler? taskPoolScheduler = null, IScreen? hostScreen = null)
        {
            this.UrlPathSegment = title;
            this.HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();

            this.MainThreadScheduler = mainThreadScheduler ?? RxApp.MainThreadScheduler;
            this.TaskPoolScheduler = taskPoolScheduler ?? RxApp.TaskpoolScheduler;

            this.Disposables = new CompositeDisposable();

            this.GoBack = ReactiveCommand.CreateFromObservable(() => this.HostScreen.Router.NavigateBack.Execute()).DisposeWith(this.Disposables);
        }

        /// <summary>
        /// Gets a string token representing the current ViewModel, such as 'login' or 'user'.
        /// </summary>
        ///
        /// <value>
        /// The URL path segment.
        /// </value>
        ///
        /// <seealso cref="P:ReactiveUI.IRoutableViewModel.UrlPathSegment"/>
        public string UrlPathSegment { get; }

        /// <summary>
        /// Gets the IScreen that this ViewModel is currently being shown in. This is usually passed into
        /// the ViewModel in the Constructor and saved as a ReadOnly Property.
        /// </summary>
        ///
        /// <value>
        /// The host screen.
        /// </value>
        ///
        /// <seealso cref="P:ReactiveUI.IRoutableViewModel.HostScreen"/>
        public IScreen HostScreen { get; }

        /// <summary>
        /// Gets or sets the 'go back' command.
        /// </summary>
        ///
        /// <value>
        /// The 'go back' command.
        /// </value>
        public ReactiveCommand<Unit, Unit> GoBack { get; protected set; }

        /// <summary>
        /// Gets the disposables.
        /// </summary>
        ///
        /// <value>
        /// The disposables.
        /// </value>
        public CompositeDisposable Disposables { get; }

        /// <summary>
        /// Gets the scheduler for scheduling operations on the main thread.
        /// </summary>
        protected IScheduler MainThreadScheduler { get; }

        /// <summary>
        /// Gets the scheduler for scheduling operations on the task pool.
        /// </summary>
        protected IScheduler TaskPoolScheduler { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the disposed.
        /// </summary>
        ///
        /// <value>
        /// True if disposed, false if not.
        /// </value>
        private bool Disposed { get; set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        ///
        /// <seealso cref="M:IDisposable.Dispose()"/>
        public virtual void Dispose()
        {
            if (this.Disposed)
            {
                return;
            }

            this.Disposed = true;
            this.Disposables.Dispose();
        }
    }
}
