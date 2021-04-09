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
    using System.Text;
    using dcbel.Mobile.Models;
    using dcbel.Mobile.ViewModels.ChargingSchedule;
    using Microsoft.Reactive.Testing;
    using ReactiveUI;
    using ReactiveUI.Testing;
    using Splat;
    using Xunit;

    public class ChargingScheduleTests : ViewModelTestsBase
    {
        public ChargingScheduleTests()
        {
            var chargingScheduleDataStore = new ChargingScheduleDataStore();
            var editChargingScheduleViewModel = new EditChargingScheduleViewModel(chargingScheduleDataStore);
            var listChargingScheduleViewModel = new ListChargingScheduleViewModel(chargingScheduleDataStore);
            Locator.CurrentMutable.Register(() => editChargingScheduleViewModel.DisposeWith(this.Disposables), typeof(IRoutableViewModel), nameof(EditChargingScheduleViewModel));
            Locator.CurrentMutable.Register(() => listChargingScheduleViewModel.DisposeWith(this.Disposables), typeof(IRoutableViewModel), nameof(ListChargingScheduleViewModel));
        }

        [Fact]
        public void ShouldMakeCalendarPickerVisible() => new TestScheduler().With(scheduler =>
        {
            var listChargingScheduleViewModel = (ListChargingScheduleViewModel)Locator.Current.GetService<IRoutableViewModel>(nameof(ListChargingScheduleViewModel));
            Assert.True(listChargingScheduleViewModel.IsChargingScheduleListVisible);

            Observable.Return(Unit.Default).InvokeCommand(listChargingScheduleViewModel.ScheduleNewCharge).DisposeWith(this.Disposables);
            Assert.False(listChargingScheduleViewModel.IsChargingScheduleListVisible);

            Observable.Return(Unit.Default).InvokeCommand(listChargingScheduleViewModel.CancelNewScheduleDatePick).DisposeWith(this.Disposables);
            Assert.True(listChargingScheduleViewModel.IsChargingScheduleListVisible);
        });

        [Fact]
        public void ShouldSetProperEditViewTitle() => new TestScheduler().With(scheduler =>
        {
            var listChargingScheduleViewModel = (ListChargingScheduleViewModel)Locator.Current.GetService<IRoutableViewModel>(nameof(ListChargingScheduleViewModel));

            Observable.Return(Guid.NewGuid()).InvokeCommand(listChargingScheduleViewModel.NavigateToEditChargingScheduleViewModel).DisposeWith(this.Disposables);

            var editChargingScheduleViewModel = (EditChargingScheduleViewModel)Locator.Current.GetService<IRoutableViewModel>(nameof(EditChargingScheduleViewModel));
            Assert.Equal("Edit EVDC Charging Schedule", editChargingScheduleViewModel.ViewTitle);

            Observable.Return(Guid.Empty).InvokeCommand(listChargingScheduleViewModel.NavigateToEditChargingScheduleViewModel).DisposeWith(this.Disposables);
            Assert.Equal("EVDC Charging Schedule", editChargingScheduleViewModel.ViewTitle);
        });

        [Fact]
        public void ShouldHaveProperTimeSetAfterStartingEdit() => new TestScheduler().With(scheduler =>
        {
            var listChargingScheduleViewModel = (ListChargingScheduleViewModel)Locator.Current.GetService<IRoutableViewModel>(nameof(ListChargingScheduleViewModel));

            if (listChargingScheduleViewModel.DataStore.TryGetData<ChargingSchedule>(out var chargingSchedule))
            {
                ((ChargingSchedule)chargingSchedule).DateTime = DateTime.Parse("January 27, 2021 10:00 AM");
            }

            var editChargingScheduleViewModel = (EditChargingScheduleViewModel)Locator.Current.GetService<IRoutableViewModel>(nameof(EditChargingScheduleViewModel));
            Observable.Return(Unit.Default).InvokeCommand(editChargingScheduleViewModel.RefreshChargingSchedule).DisposeWith(this.Disposables);
            Assert.Equal(DateTime.Parse("January 27, 2021 10:00 AM"), editChargingScheduleViewModel.ChargingSchedule!.DateTime);
        });
    }
}
