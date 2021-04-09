// Copyright (c) PlaceholderCompany. All rights reserved.

using Custom.DatePickerTest.Controls;
using Custom.DatePickerTest.iOS.Renderers;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(ExtendedFrame), typeof(ExtendedFrameRenderer))]

namespace Custom.DatePickerTest.iOS.Renderers
{
    using Xamarin.Forms.Platform.iOS;

    /// <summary>
    /// An extended frame renderer.
    /// </summary>
    ///
    /// <seealso cref="T:Xamarin.Forms.Platform.iOS.FrameRenderer"/>
    public class ExtendedFrameRenderer : FrameRenderer
    {
        /// <summary>
        /// Executes the element changed action.
        /// </summary>
        ///
        /// <param name="e"> An ElementChangedEventArgs{Frame} to process. </param>
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            var frame = (ExtendedFrame)this.Element;
            if (frame != null)
            {
                if (frame.BorderWidth > 0)
                {
                    this.Layer.BorderColor = frame.BorderColor.ToCGColor();
                    this.Layer.BorderWidth = frame.BorderWidth;
                    this.Layer.CornerRadius = frame.CornerRadius;
                }
            }
        }
    }
}