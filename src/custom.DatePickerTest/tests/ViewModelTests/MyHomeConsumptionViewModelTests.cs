// ------------------------------------------------------------
// Copyright (c) Ossiaco Inc. All rights reserved.
// ------------------------------------------------------------

namespace dcbel.Mobile.Tests.ViewModelTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Text;
    using dcbel.Mobile.Services;
    using dcbel.Mobile.ViewModels;
    using Microsoft.Reactive.Testing;
    using NSubstitute;
    using ReactiveUI;
    using ReactiveUI.Testing;
    using Splat;
    using Xunit;

    public class MyHomeConsumptionViewModelTests : ViewModelTestsBase
    {
        [Fact]
        public void ShouldObserveChangesToLiveConsumption() => new TestScheduler().With(scheduler =>
        {
            var dcbelDataServiceProvider = Substitute.For<IDcbelDataServiceProvider>();
            dcbelDataServiceProvider.GetDcbelDataServiceAsync().Returns(new MockDcbelDataService());
            var myHomeConsumptionViewModel = new MyHomeConsumptionViewModel(dcbelDataServiceProvider).DisposeWith(this.Disposables);

            Assert.Equal(0.00, myHomeConsumptionViewModel.MyHomeConsumption.LiveConsumption);
            scheduler.AdvanceBy(TimeSpan.FromSeconds(6).Ticks);
            Assert.Equal(1.00, myHomeConsumptionViewModel.MyHomeConsumption.LiveConsumption);
            scheduler.AdvanceBy(TimeSpan.FromSeconds(6).Ticks);
            Assert.Equal(2.00, myHomeConsumptionViewModel.MyHomeConsumption.LiveConsumption);
        });

        [Fact]
        public void ShouldGetHourlyConsumptionAfterCommandExecution() => new TestScheduler().With(scheduler =>
        {
            var dcbelDataServiceProvider = Substitute.For<IDcbelDataServiceProvider>();
            dcbelDataServiceProvider.GetDcbelDataServiceAsync().Returns(new MockDcbelDataService());
            var myHomeConsumptionViewModel = new MyHomeConsumptionViewModel(dcbelDataServiceProvider).DisposeWith(this.Disposables);

            Observable.Return(Unit.Default).InvokeCommand(myHomeConsumptionViewModel.GetAvgHourlyConsumption).DisposeWith(this.Disposables);
            scheduler.AdvanceBy(TimeSpan.FromTicks(1).Ticks);
            Assert.Equal(4, myHomeConsumptionViewModel.MyHomeConsumption.AvgConsumptions.Count);
            Assert.Equal(4.06, myHomeConsumptionViewModel.MyHomeConsumption.AvgConsumptions.FirstOrDefault().Value);
        });
    }
}
