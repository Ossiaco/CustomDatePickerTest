// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.ViewModels.ChargingSchedule
{
    using System.Collections.Generic;
    using Custom.DatePickerTest.Models;

    /// <summary>
    /// A charging schedule data store.
    /// </summary>
    ///
    /// <seealso cref="T:Custom.DatePickerTest.ViewModels.IViewModelDataStore{Custom.DatePickerTest.Models.ModelBase}"/>
    public class ChargingScheduleDataStore : IViewModelDataStore<ModelBase>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChargingScheduleDataStore"/> class.
        /// </summary>
        public ChargingScheduleDataStore()
        {
            this.DataStore = new Dictionary<string, ModelBase>();
        }

        /// <summary>
        /// Gets the data store.
        /// </summary>
        ///
        /// <value>
        /// The data store.
        /// </value>
        ///
        /// <seealso cref="P:Custom.DatePickerTest.ViewModels.IViewModelDataStore{Custom.DatePickerTest.Models.ModelBase}.DataStore"/>
        public Dictionary<string, ModelBase> DataStore { get; }

        /// <summary>
        /// Gets a data.
        /// </summary>
        ///
        /// <param name="property"> Name of the property. </param>
        ///
        /// <returns>
        /// The data.
        /// </returns>
        ///
        /// <seealso cref="M:IViewModelDataStore.GetData(string)"/>
        public ModelBase? GetData(string property)
        {
            if (this.DataStore.TryGetValue(property, out var dataElement))
            {
                return dataElement;
            }

            return null;
        }

        /// <summary>
        /// Attempts to add data.
        /// </summary>
        ///
        /// <typeparam name="TData"> Type of the data. </typeparam>
        /// <param name="dataToAdd"> The data to add. </param>
        ///
        /// <returns>
        /// True if it succeeds, false if it fails.
        /// </returns>
        ///
        /// <seealso cref="M:IViewModelDataStore.TryAddData{TData}(ModelBase)"/>
        public bool TryAddData<TData>(ModelBase dataToAdd)
        {
            return this.DataStore.TryAdd(typeof(TData).Name, dataToAdd);
        }

        /// <summary>
        /// Attempts to get data.
        /// </summary>
        ///
        /// <typeparam name="TData"> Type of the data. </typeparam>
        /// <param name="property"> [out] The property. </param>
        ///
        /// <returns>
        /// True if it succeeds, false if it fails.
        /// </returns>
        ///
        /// <seealso cref="M:IViewModelDataStore.TryGetData{TData}(out ModelBase)"/>
        public bool TryGetData<TData>(out ModelBase property)
        {
            return this.DataStore.TryGetValue(typeof(TData).Name, out property);
        }
    }
}
