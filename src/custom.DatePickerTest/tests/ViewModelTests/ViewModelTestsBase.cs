// ------------------------------------------------------------
// Copyright (c) Ossiaco Inc. All rights reserved.
// ------------------------------------------------------------

namespace dcbel.Mobile.Tests.ViewModelTests
{
    using System;
    using System.Reactive.Disposables;
    using System.Reflection;
    using NSubstitute;
    using ReactiveUI;
    using Splat;
    using Xunit.Sdk;

    /// <summary>
    /// A view model tests base.
    /// </summary>
    public class ViewModelTestsBase : IDisposable
    {
        public ViewModelTestsBase()
        {
            this.Disposables = new ();
            this.HostScreen = Substitute.For<IScreen>();

            this.HostScreen.Router.Returns(new RoutingState());
            Locator.CurrentMutable.RegisterConstant(this.HostScreen, typeof(IScreen));
        }

        public IScreen HostScreen { get; }

        public CompositeDisposable Disposables { get; }

        public virtual void Dispose() => this.Disposables.Dispose();
    }
}
