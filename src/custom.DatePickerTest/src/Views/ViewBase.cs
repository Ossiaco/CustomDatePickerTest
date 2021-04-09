// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Views
{
    using System;
    using System.Reactive.Disposables;
    using ReactiveUI.XamForms;

    /// <summary>
    /// A view base.
    /// </summary>
    ///
    /// <typeparam name="TViewModel">   Type of the view model. </typeparam>
    ///
    /// <seealso cref="T:ReactiveUI.XamForms.ReactiveContentPage{TViewModel}"/>
    public class ViewBase<TViewModel> : ReactiveContentPage<TViewModel>, IDisposable
        where TViewModel : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewBase{TViewModel}"/> class.
        /// </summary>
        public ViewBase()
        {
            this.Disposables = new CompositeDisposable();
        }

        /// <summary>
        /// Gets the disposables.
        /// </summary>
        ///
        /// <value>
        /// The disposables.
        /// </value>
        public CompositeDisposable Disposables { get; }

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
