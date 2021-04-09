// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Views.ChargingSchedule
{
    using System;
    using System.Reactive.Disposables;
    using Custom.DatePickerTest.ViewModels.ChargingSchedule;
    using ReactiveUI;
    using Xamarin.Forms.Xaml;

    /// <content>
    /// An edit charging schedule view.
    /// </content>
    public partial class EditChargingScheduleView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditChargingScheduleView"/> class.
        /// </summary>
        public EditChargingScheduleView()
        {
            this.ViewModel = App.ServiceProvider.GetService<EditChargingScheduleViewModel>();

            this.InitializeComponent();

            this.DepartureDateTimePicker.MinimumDate = DateTime.Today.AddDays(-1.0);

            this.WhenActivated(
                disposables =>
                {
                    this.Bind(this.ViewModel, vm => vm.ChargingSchedule.DateTime, view => view.DepartureDateTimePicker.Date).DisposeWith(disposables);
                    this.Bind(this.ViewModel, vm => vm.ChargingSchedule.DateTime, view => view.DepartureTimePicker.Time, dateTime => dateTime.GetValueOrDefault().TimeOfDay, timeSpan => default(DateTime) + timeSpan).DisposeWith(disposables);
                    this.Bind(this.ViewModel, vm => vm.ChargingSchedule.IsRecurring, view => view.RecuringSwitch.IsToggled).DisposeWith(disposables);
                    this.Bind(this.ViewModel, vm => vm.ChargingSchedule.Range, view => view.RangeText.Text).DisposeWith(disposables);
                    this.Bind(this.ViewModel, vm => vm.ChargingSchedule.Range, view => view.RangeSlider.Value, vmToViewConverter: StringToDouble, viewToVmConverter: DoubleToString).DisposeWith(disposables);

                    this.OneWayBind(this.ViewModel, vm => vm.RecurringScheduleDaysOfWeek, view => view.RecurringDaysView.ItemsSource).DisposeWith(disposables);
                    this.OneWayBind(this.ViewModel, vm => vm.ChargingSchedule.IsRecurring, view => view.RecurringDaysView.IsVisible).DisposeWith(disposables);
                    this.OneWayBind(this.ViewModel, vm => vm.ChargingSchedule.IsRecurring, view => view.DepartureTimePicker.IsVisible).DisposeWith(disposables);
                    this.OneWayBind(this.ViewModel, vm => vm.ChargingSchedule.IsRecurring, view => view.DepartureDateTimePicker.IsVisible, isVisible => !isVisible).DisposeWith(disposables);
                    this.OneWayBind(this.ViewModel, vm => vm.GoBack, view => view.TitleBar.Command).DisposeWith(disposables);
                })
                .DisposeWith(this.Disposables);
        }

        private static string DoubleToString(double i)
        {
            return i.ToString();
        }

        private static double StringToDouble(string? entry)
        {
            if (!string.IsNullOrEmpty(entry))
            {
                return double.Parse(entry);
            }

            return 0;
        }
    }
}