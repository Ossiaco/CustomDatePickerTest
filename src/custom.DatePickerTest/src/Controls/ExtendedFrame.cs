// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Controls
{
    using Xamarin.Forms;

    /// <summary>
    /// An extended frame.
    /// </summary>
    ///
    /// <seealso cref="T:Xamarin.Forms.Frame"/>
    public class ExtendedFrame : Frame
    {
        /// <summary>
        /// The border width property.
        /// </summary>
        public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(nameof(BorderWidth), typeof(float), typeof(ExtendedFrame));

        /// <summary>
        /// Gets or sets the width of the border.
        /// </summary>
        ///
        /// <value>
        /// The width of the border.
        /// </value>
        public float BorderWidth
        {
            get => (float)this.GetValue(BorderWidthProperty);
            set => this.SetValue(BorderWidthProperty, value);
        }
    }
}
