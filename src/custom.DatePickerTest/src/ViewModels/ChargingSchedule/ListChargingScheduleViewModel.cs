// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.ViewModels.ChargingSchedule
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using Custom.DatePickerTest.Models;
    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;
    using Splat;

    /// <summary>
    /// A ViewModel for the charging schedule list.
    /// </summary>
    ///
    /// <seealso cref="T:Custom.DatePickerTest.ViewModels.ViewModelBase"/>
    public class ListChargingScheduleViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListChargingScheduleViewModel"/> class.
        /// </summary>
        ///
        /// <param name="dataStore">                     The data store. </param>
        public ListChargingScheduleViewModel(ChargingScheduleDataStore dataStore)
            : base("Charging Schedule List View Model")
        {
            this.IsChargingScheduleListVisible = true;

            this.ChargingSchedules = new ObservableCollection<ChargingSchedule>();

            this.DataStore = dataStore;

            this.RefreshChargingSchedules = ReactiveCommand.CreateFromTask(this.GetChargingSchedulesAsync).DisposeWith(this.Disposables);

            this.NavigateToEditChargingScheduleViewModel = ReactiveCommand.CreateFromObservable<Guid, IRoutableViewModel>(this.EditChargingSchedule).DisposeWith(this.Disposables);

            this.Schedules = new ();
            for (var i = 0; i < 5; i++)
            {
                this.Schedules.Add(new Models.ChargingSchedule { DateTime = DateTime.Today, Range = "150 mi" });
            }

            this.WhenAnyValue(vm => vm.Schedules)
                .Select(schedules => schedules.Any())
                .ToPropertyEx(this, vm => vm.IsChargingScheduleListVisible)
                .DisposeWith(this.Disposables);

            this.WhenAnyValue(vm => vm.DateSelected)
                .Subscribe(dateSelected =>
                {
                    if (dateSelected.CompareTo(DateTime.Today) == 1)
                    {
                        var chargingSchedule = new ChargingSchedule
                        {
                            DateTime = dateSelected,
                        };
                        if (this.DataStore.TryAddData<ChargingSchedule>(chargingSchedule))
                        {
                            this.HostScreen.Router.Navigate
                                .Execute(Locator.Current.GetService<IRoutableViewModel>(nameof(EditChargingScheduleViewModel)))
                                .Subscribe()
                                .DisposeWith(this.Disposables);
                        }
                    }
                })
                .DisposeWith(this.Disposables);
        }

        /// <summary>
        /// Gets or sets the schedules.
        /// </summary>
        ///
        /// <value>
        /// The schedules.
        /// </value>
        public ObservableCollection<Models.ChargingSchedule> Schedules { get; set; }

        /// <summary>
        /// Gets or sets the Date selected.
        /// </summary>
        ///
        /// <value>
        /// The date selected.
        /// </value>
        [Reactive]
        public DateTime DateSelected { get; set; }

        /// <summary>
        /// Gets a value indicating whether this is charging schedule list visible.
        /// </summary>
        ///
        /// <value>
        /// True if this is charging schedule list visible, false if not.
        /// </value>
        [ObservableAsProperty]
        public bool IsChargingScheduleListVisible { get; }

        /// <summary>
        /// Gets the 'navigate to edit charging schedule view model' command.
        /// </summary>
        ///
        /// <value>
        /// The 'navigate to edit charging schedule view model' command.
        /// </value>
        public ReactiveCommand<Guid, IRoutableViewModel> NavigateToEditChargingScheduleViewModel { get; }

        /// <summary>
        /// Gets the charging schedule.
        /// </summary>
        ///
        /// <value>
        /// The charging schedule.
        /// </value>
        public ChargingScheduleDataStore DataStore { get; }

        /// <summary>
        /// Gets the charging schedules.
        /// </summary>
        ///
        /// <value>
        /// The charging schedules.
        /// </value>
        public ObservableCollection<ChargingSchedule> ChargingSchedules { get; }

        /// <summary>
        /// Gets the 'refresh charging schedules' command.
        /// </summary>
        ///
        /// <value>
        /// The 'refresh charging schedules' command.
        /// </value>
        public ReactiveCommand<Unit, Unit> RefreshChargingSchedules { get; }

        /// <summary>
        /// Gets charging schedules.
        /// </summary>
        ///
        /// <returns>
        /// An asynchronous result.
        /// </returns>
        private async Task GetChargingSchedulesAsync()
        {
            await Task.FromResult("done");
        }

        private IObservable<IRoutableViewModel> EditChargingSchedule(Guid chargingScheduleId)
        {
            var editChargingSchedule = (EditChargingScheduleViewModel)Locator.Current.GetService<IRoutableViewModel>(nameof(EditChargingScheduleViewModel));
            if (chargingScheduleId == Guid.Empty)
            {
                editChargingSchedule.ViewTitle = "EVDC Charging Schedule";
            }
            else
            {
                editChargingSchedule.ViewTitle = "Edit EVDC Charging Schedule";
            }

            return this.HostScreen.Router.Navigate.Execute(editChargingSchedule);
        }
    }
}
