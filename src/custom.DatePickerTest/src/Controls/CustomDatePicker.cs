// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Controls
{
    using Xamarin.Forms;

    /// <summary>
    /// A custom date picker.
    /// </summary>
    ///
    /// <seealso cref="T:Xamarin.Forms.DatePicker"/>
    public class CustomDatePicker : DatePicker
    {
        /// <summary>
        /// The done button text propery.
        /// </summary>
        public static readonly BindableProperty DoneButtonTextProperty = BindableProperty.Create(nameof(DoneButtonText), typeof(string), typeof(CustomDatePicker));

        /// <summary>
        /// The cancel button text property.
        /// </summary>
        public static readonly BindableProperty CancelButtonTextProperty = BindableProperty.Create(nameof(CancelButtonText), typeof(string), typeof(CustomDatePicker));

        /// <summary>
        /// The title text property.
        /// </summary>
        public static readonly BindableProperty TitleTextProperty = BindableProperty.Create(nameof(TitleText), typeof(string), typeof(CustomDatePicker));

        /// <summary>
        /// Gets or sets the done button text.
        /// </summary>
        ///
        /// <value>
        /// The done button text.
        /// </value>
        public string DoneButtonText
        {
            get => (string)this.GetValue(DoneButtonTextProperty);
            set => this.SetValue(DoneButtonTextProperty, value);
        }

        /// <summary>
        /// Gets or sets the cancel button text.
        /// </summary>
        ///
        /// <value>
        /// The cancel button text.
        /// </value>
        public string CancelButtonText
        {
            get => (string)this.GetValue(CancelButtonTextProperty);
            set => this.SetValue(CancelButtonTextProperty, value);
        }

        /// <summary>
        /// Gets or sets the title text.
        /// </summary>
        ///
        /// <value>
        /// The title text.
        /// </value>
        public string TitleText
        {
            get => (string)this.GetValue(TitleTextProperty);
            set => this.SetValue(TitleTextProperty, value);
        }
    }
}
