// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Views.ChargingSchedule
{
    using System;
    using System.Reactive.Disposables;
    using Custom.DatePickerTest.ViewModels.ChargingSchedule;
    using ReactiveUI;
    using Xamarin.Forms.Xaml;

    /// <content>
    /// A list charging schedule view.
    /// </content>
    public partial class ListChargingScheduleView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListChargingScheduleView"/> class.
        /// </summary>
        public ListChargingScheduleView()
        {
            this.ViewModel = App.ServiceProvider.GetService<ListChargingScheduleViewModel>();

            this.InitializeComponent();

            this.WhenActivated(
                disposables =>
                {
                    this.DatePicker.MinimumDate = DateTime.Today.AddDays(-1);

                    this.OneWayBind(this.ViewModel, vm => vm.GoBack, view => view.TitleBar.Command).DisposeWith(disposables);
                    this.Bind(this.ViewModel, vm => vm.DateSelected, view => view.DatePicker.Date).DisposeWith(disposables);
                    this.ScheduleNewCharge.Clicked += (object sender, EventArgs eventArgs) => this.DatePicker.Focus();
                })
                .DisposeWith(this.Disposables);

            this.OneWayBind(this.ViewModel, vm => vm.Schedules, view => view.ChargingSchedules.ItemsSource).DisposeWith(this.Disposables);
            this.OneWayBind(this.ViewModel, vm => vm.IsChargingScheduleListVisible, view => view.ChargingScheduleLayout.IsVisible).DisposeWith(this.Disposables);
            this.OneWayBind(this.ViewModel, vm => vm.IsChargingScheduleListVisible, view => view.NewChargeLayout.IsVisible, isVisible => !isVisible).DisposeWith(this.Disposables);
        }
    }
}