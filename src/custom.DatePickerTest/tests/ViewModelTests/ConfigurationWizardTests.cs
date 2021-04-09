// ------------------------------------------------------------
// Copyright (c) Ossiaco Inc. All rights reserved.
// ------------------------------------------------------------

namespace dcbel.Mobile.Tests.ViewModelTests
{
    using System;
    using System.Collections.Generic;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using Chorus.Actors.Sites.Assets;
    using dcbel.Mobile.Services;
    using dcbel.Mobile.ViewModels.ConfigurationWizard;
    using Microsoft.Reactive.Testing;
    using NSubstitute;
    using ReactiveUI;
    using ReactiveUI.Testing;
    using Splat;
    using Xunit;

    public class ConfigurationWizardTests : ViewModelTestsBase
    {
        [Fact]
        public void ShouldNavigateToNextTab() => new TestScheduler().With(scheduler =>
        {
            var dcbelDataServiceProvider = Substitute.For<IDcbelDataServiceProvider>();
            dcbelDataServiceProvider.GetDcbelDataServiceAsync().Returns(new MockDcbelDataService());
            var mockConfigurationWizardRouter = Substitute.For<IConfigurationWizardRouter>();

            var configureEVViewModel = new ConfigureFirstElectricVehicleViewModel(mockConfigurationWizardRouter, dcbelDataServiceProvider).DisposeWith(this.Disposables);
            mockConfigurationWizardRouter.GetStep(1).Returns(configureEVViewModel);

            var configurationBaseViewModel = new BeginConfigurationViewModel(mockConfigurationWizardRouter).DisposeWith(this.Disposables);

            Locator.CurrentMutable.Register(() => configurationBaseViewModel, typeof(IRoutableViewModel), nameof(BeginConfigurationViewModel));
            Locator.CurrentMutable.Register(() => configureEVViewModel, typeof(IRoutableViewModel), nameof(ConfigureFirstElectricVehicleViewModel));

            using var disposable = Observable.Return(Unit.Default).InvokeCommand(configurationBaseViewModel.SkipCurrentConfiguration);
            scheduler.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);

            Assert.Equal(1, configurationBaseViewModel.Router.CurrentActiveTab);
        });

        [Fact]
        public void ShouldNavigateBack() => new TestScheduler().With(scheduler =>
        {
            var dcbelDataServiceProvider = Substitute.For<IDcbelDataServiceProvider>();
            dcbelDataServiceProvider.GetDcbelDataServiceAsync().Returns(new MockDcbelDataService());
            var mockConfigurationWizardRouter = Substitute.For<IConfigurationWizardRouter>();

            var configureElectricVehicleViewModel = new ConfigureFirstElectricVehicleViewModel(mockConfigurationWizardRouter, dcbelDataServiceProvider).DisposeWith(this.Disposables);
            var configureElectricVehicle2ViewModel = new ConfigureSecondElectricVehicleViewModel(mockConfigurationWizardRouter, dcbelDataServiceProvider).DisposeWith(this.Disposables);
            var configurationBaseViewModel = new BeginConfigurationViewModel(mockConfigurationWizardRouter).DisposeWith(this.Disposables);

            mockConfigurationWizardRouter.GetStep(0).Returns(configurationBaseViewModel);
            mockConfigurationWizardRouter.GetStep(1).Returns(configureElectricVehicleViewModel);
            mockConfigurationWizardRouter.GetStep(2).Returns(configureElectricVehicle2ViewModel);

            Locator.CurrentMutable.Register(() => configurationBaseViewModel, typeof(IRoutableViewModel), nameof(BeginConfigurationViewModel));
            Locator.CurrentMutable.Register(() => configureElectricVehicleViewModel, typeof(IRoutableViewModel), nameof(ConfigureFirstElectricVehicleViewModel));
            Locator.CurrentMutable.Register(() => configureElectricVehicle2ViewModel, typeof(IRoutableViewModel), nameof(ConfigureSecondElectricVehicleViewModel));

            var goToNextViewModelObservable = configurationBaseViewModel.SkipCurrentConfiguration.Execute().ObserveOn(scheduler);
            var skipSubscription = goToNextViewModelObservable.Subscribe((vm) =>
            {
                Assert.Equal(1, ((ConfigureFirstElectricVehicleViewModel)vm).Router.CurrentActiveTab);
                Assert.Equal(nameof(ConfigureFirstElectricVehicleViewModel), vm.GetType().Name);
            })
            .DisposeWith(this.Disposables);
            scheduler.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);

            Observable.Return(Unit.Default).InvokeCommand(configureElectricVehicleViewModel.GoBack).DisposeWith(this.Disposables);
            scheduler.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
        });

        [Fact]
        public void ShouldGrantAccessToCommand() => new TestScheduler().With(scheduler =>
        {
            var dcbelDataServiceProvider = Substitute.For<IDcbelDataServiceProvider>();
            var mockConfigurationWizardRouter = Substitute.For<IConfigurationWizardRouter>();

            var dataService = Substitute.For<IDcbelDataService>();
            dcbelDataServiceProvider.GetDcbelDataServiceAsync().Returns(new MockDcbelDataService());

            var configureEVViewModel = new ConfigureFirstElectricVehicleViewModel(mockConfigurationWizardRouter, dcbelDataServiceProvider).DisposeWith(this.Disposables);

            configureEVViewModel.EV1.BatterySize = 1;
            var navigateToEv2CommandObservable = configureEVViewModel.NavigateToNextTab!.Execute().ObserveOn(scheduler);
            var isExecutingObservable = configureEVViewModel.NavigateToNextTab.IsExecuting.ObserveOn(scheduler);
            navigateToEv2CommandObservable.Subscribe().DisposeWith(this.Disposables);
            scheduler.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
            isExecutingObservable.Subscribe(isExecuting => Assert.False(isExecuting)).DisposeWith(this.Disposables);

            Assert.False(configureEVViewModel.CanDisplayModelPicker);

            configureEVViewModel.EV1.Brand = configureEVViewModel.Brands[0];
            scheduler.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);

            Assert.True(configureEVViewModel.CanDisplayModelPicker);

            configureEVViewModel.EV1.Model = "Model";
            configureEVViewModel.EV1.ChargingProtocol = ElectricVehicleChargingProtocol.CCS;

            navigateToEv2CommandObservable = configureEVViewModel.NavigateToNextTab!.Execute().ObserveOn(scheduler);
            isExecutingObservable = configureEVViewModel.NavigateToNextTab.IsExecuting.ObserveOn(scheduler);
            scheduler.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
            isExecutingObservable.Subscribe(isExecuting => Assert.True(isExecuting)).DisposeWith(this.Disposables);

            Assert.Equal(2, mockConfigurationWizardRouter.CurrentActiveTab);
        });

        [Fact]
        public void ShouldGrantAccessToCommandOnHouseViewModel() => new TestScheduler().With(scheduler =>
        {
            var dcbelDataServiceProvider = Substitute.For<IDcbelDataServiceProvider>();
            var mockConfigurationWizardRouter = Substitute.For<IConfigurationWizardRouter>();

            var dataService = Substitute.For<IDcbelDataService>();
            dcbelDataServiceProvider.GetDcbelDataServiceAsync().Returns(new MockDcbelDataService());

            var configureHouseViewModel = new ConfigureHouseViewModel(mockConfigurationWizardRouter).DisposeWith(this.Disposables);

            configureHouseViewModel.House.Area.Value = 100;
            var navigateToUtilityCommandObservable = configureHouseViewModel.NavigateToNextTab!.Execute().ObserveOn(scheduler);
            var isExecutingObservable = configureHouseViewModel.NavigateToNextTab.IsExecuting.ObserveOn(scheduler);
            navigateToUtilityCommandObservable.Subscribe().DisposeWith(this.Disposables);
            scheduler.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
            isExecutingObservable.Subscribe(isExecuting => Assert.False(isExecuting)).DisposeWith(this.Disposables);

            configureHouseViewModel.House.Area.UnitOfMeasure = "m\u00B2";

            navigateToUtilityCommandObservable = configureHouseViewModel.NavigateToNextTab!.Execute().ObserveOn(scheduler);
            isExecutingObservable = configureHouseViewModel.NavigateToNextTab.IsExecuting.ObserveOn(scheduler);
            scheduler.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);
            isExecutingObservable.Subscribe(isExecuting => Assert.True(isExecuting)).DisposeWith(this.Disposables);

            Assert.Equal(4, mockConfigurationWizardRouter.CurrentActiveTab);
        });
    }
}
