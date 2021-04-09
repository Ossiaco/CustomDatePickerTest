// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.ViewModels.ChargingSchedule
{
    using System;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Threading.Tasks;
    using Custom.DatePickerTest.Models;
    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    /// <summary>
    /// A ViewModel for the edit.
    /// </summary>
    ///
    /// <seealso cref="T:Custom.DatePickerTest.ViewModels.ViewModelBase"/>
    public class EditChargingScheduleViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditChargingScheduleViewModel"/> class.
        /// </summary>
        ///
        /// <param name="dataStore"> The data store. </param>
        public EditChargingScheduleViewModel(ChargingScheduleDataStore dataStore)
            : base("Edit Charging Schedule")
        {
            this.DataStore = dataStore;

            this.ViewTitle = "EVDC Charging Schedule";

            this.SaveChargingSchedule = ReactiveCommand.CreateFromTask(this.SaveChargingScheduleAsync).DisposeWith(this.Disposables);
            this.DeleteChargingSchedule = ReactiveCommand.CreateFromTask<Guid>(this.DeleteChargingScheduleAsync).DisposeWith(this.Disposables);
            this.RefreshChargingSchedule = ReactiveCommand.Create(() =>
            {
                if (this.DataStore.TryGetData<ChargingSchedule>(out var chargingSchedule))
                {
                    this.ChargingSchedule = (ChargingSchedule)chargingSchedule;
                    return true;
                }

                return false;
            })
            .DisposeWith(this.Disposables);

            this.RecurringScheduleDaysOfWeek = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;

            if (this.DataStore.TryGetData<ChargingSchedule>(out var chargingSchedule))
            {
                this.ChargingSchedule = (ChargingSchedule)chargingSchedule;
            }
            else
            {
                this.ChargingSchedule = new ChargingSchedule();
            }
        }

        /// <summary>
        /// Gets the recurring schedule days of week.
        /// </summary>
        ///
        /// <value>
        /// The recurring schedule days of week.
        /// </value>
        public string[] RecurringScheduleDaysOfWeek { get; }

        /// <summary>
        /// Gets or sets the charging schedule.
        /// </summary>
        ///
        /// <value>
        /// The charging schedule.
        /// </value>
        [Reactive]
        public ChargingSchedule ChargingSchedule { get; set; }

        /// <summary>
        /// Gets the charging schedule.
        /// </summary>
        ///
        /// <value>
        /// The charging schedule.
        /// </value>
        public ChargingScheduleDataStore DataStore { get; }

        /// <summary>
        /// Gets or sets the view title.
        /// </summary>
        ///
        /// <value>
        /// The view title.
        /// </value>
        public string ViewTitle { get; set; }

        /// <summary>
        /// Gets the 'refresh charging schedule' command.
        /// </summary>
        ///
        /// <value>
        /// The 'refresh charging schedule' command.
        /// </value>
        public ReactiveCommand<Unit, bool> RefreshChargingSchedule { get; }

        /// <summary>
        /// Gets the 'save charging schedule' command.
        /// </summary>
        ///
        /// <value>
        /// The 'save charging schedule' command.
        /// </value>
        public ReactiveCommand<Unit, Unit> SaveChargingSchedule { get; }

        /// <summary>
        /// Gets the 'delete charging schedule' command.
        /// </summary>
        ///
        /// <value>
        /// The 'delete charging schedule' command.
        /// </value>
        public ReactiveCommand<Guid, Unit> DeleteChargingSchedule { get; }

        /// <summary>
        /// Saves the charging schedule asynchronous.
        /// </summary>
        ///
        /// <returns>
        /// An asynchronous result.
        /// </returns>
        private async Task SaveChargingScheduleAsync()
        {
            await Task.FromResult("The charging schedule has been saved");
        }

        /// <summary>
        /// Deletes the charging schedule asynchronous described by chargingScheduleId.
        /// </summary>
        ///
        /// <param name="chargingScheduleId"> Identifier for the charging schedule. </param>
        ///
        /// <returns>
        /// An asynchronous result.
        /// </returns>
        private async Task DeleteChargingScheduleAsync(Guid chargingScheduleId)
        {
            await Task.FromResult($"The charging schedule with guid: {chargingScheduleId} has been deleted");
        }
    }
}
