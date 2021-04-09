// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Droid.Renderers
{
    using Android.Content;
    using Android.Util;

    /// <summary>
    /// Extension to convert Float to Pixels.
    /// </summary>
    public static class FloatToDpExtension
    {
        /// <summary>
        /// Convert float to pixel.
        /// </summary>
        /// <param name="valueInDp"> float value. </param>
        /// <param name="context"> context. </param>
        /// <returns> pixels.</returns>
        public static float DpToPixels(this float valueInDp, Context? context)
        {
            if (context?.Resources?.DisplayMetrics != null)
            {
                var metrics = context.Resources.DisplayMetrics;
                return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
            }
            else
            {
                return 0;
            }
        }
    }
}