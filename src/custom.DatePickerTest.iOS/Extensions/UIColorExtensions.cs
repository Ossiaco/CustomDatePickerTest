// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.iOS.Extensions
{
    using System;
    using System.Globalization;
    using UIKit;

    /// <summary>
    /// UIColor extensions.
    /// </summary>
    public static class UIColorExtensions
    {
        /// <summary>
        /// An UIColor extension method that returns UIColor from hexadecimal.
        /// </summary>
        ///
        /// <param name="colorValue"> The color value. </param>
        ///
        /// <returns>
        /// An UIColor.
        /// </returns>
        public static UIColor FromHex(string? colorValue)
        {
            if (colorValue != null)
            {
                var hexArray = colorValue.ToCharArray();
                var red = int.Parse($"{hexArray[0]}{hexArray[1]}", NumberStyles.HexNumber, CultureInfo.CurrentCulture);
                var green = int.Parse($"{hexArray[2]}{hexArray[3]}", NumberStyles.HexNumber, CultureInfo.CurrentCulture);
                var blue = int.Parse($"{hexArray[4]}{hexArray[5]}", NumberStyles.HexNumber, CultureInfo.CurrentCulture);
                return UIColor.FromRGB(red, green, blue);
            }

            throw new ArgumentNullException(nameof(colorValue));
        }
    }
}