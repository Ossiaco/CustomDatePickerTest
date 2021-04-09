// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Models
{
    using System;
    using System.Collections.ObjectModel;
    using ReactiveUI.Fody.Helpers;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChargingSchedule"/> class.
    /// </summary>
    public class ChargingSchedule : ModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChargingSchedule"/> class.
        /// </summary>
        public ChargingSchedule()
        {
            this.Dates = new ();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this is recurring.
        /// </summary>
        ///
        /// <value>
        /// True if this is recurring, false if not.
        /// </value>
        [Reactive]
        public bool IsRecurring { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is active.
        /// </summary>
        ///
        /// <value>
        /// True if this is active, false if not.
        /// </value>
        [Reactive]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets the dates.
        /// </summary>
        ///
        /// <value>
        /// The dates.
        /// </value>
        public ObservableCollection<DayOfWeek> Dates { get; }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        ///
        /// <value>
        /// The time.
        /// </value>
        [Reactive]
        public DateTime? DateTime { get; set; }

        /// <summary>
        /// Gets or sets the range.
        /// </summary>
        ///
        /// <value>
        /// The range.
        /// </value>
        [Reactive]
        public string? Range { get; set; }
    }
}
