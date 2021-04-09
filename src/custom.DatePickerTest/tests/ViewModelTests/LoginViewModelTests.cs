// ------------------------------------------------------------
// Copyright (c) Ossiaco Inc. All rights reserved.
// ------------------------------------------------------------

namespace dcbel.Mobile.Tests.ViewModelTests
{
    using Chorus.Client.Native.Configuration;
    using dcbel.Mobile.Services;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Identity.Client;
    using Microsoft.Reactive.Testing;
    using NSubstitute;
    using ReactiveUI.Testing;
    using Xunit;

    public class LoginViewModelTests : ViewModelTestsBase
    {
        [Fact]
        public void ShouldThrowExceptionIfNotAuthenticated() => new TestScheduler().With(scheduler =>
        {
            var publicClientApplication = Substitute.For<IPublicClientApplication>();
            var loggerFactory = Substitute.For<ILoggerFactory>();
            var optionsMonitor = Substitute.For<IOptionsMonitor<B2CAuthenticationOptions>>();

            var dcbelDataServiceProvider = Substitute.For<IDcbelDataServiceProvider>();
            var dcbelDataService = new MockDcbelDataService();
            var authenticationService = Substitute.For<AuthenticationService>(loggerFactory, optionsMonitor, publicClientApplication);

            dcbelDataServiceProvider.GetDcbelDataServiceAsync().Returns(dcbelDataService);

            // TODO: Finish test
        });
    }
}
