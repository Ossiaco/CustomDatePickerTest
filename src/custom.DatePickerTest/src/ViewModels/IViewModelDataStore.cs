// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.ViewModels
{
    using System.Collections.Generic;
    using Custom.DatePickerTest.Models;

    /// <summary>
    /// Interface for view model data store.
    /// </summary>
    ///
    /// <typeparam name="TModel"> Type of the model. </typeparam>
    public interface IViewModelDataStore<TModel>
        where TModel : ModelBase
    {
        /// <summary>
        /// Gets the data store.
        /// </summary>
        ///
        /// <value>
        /// The data store.
        /// </value>
        public Dictionary<string, TModel> DataStore { get; }

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
        public bool TryAddData<TData>(TModel dataToAdd);

        /// <summary>
        /// Gets a data.
        /// </summary>
        ///
        /// <param name="propertyName"> Name of the property. </param>
        ///
        /// <returns>
        /// The data.
        /// </returns>
        public TModel? GetData(string propertyName);

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
        public bool TryGetData<TData>(out TModel property);
    }
}
