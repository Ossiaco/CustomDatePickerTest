// Copyright (c) PlaceholderCompany. All rights reserved.

using Custom.DatePickerTest.Controls;
using Custom.DatePickerTest.Droid.Renderers;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(ExtendedFrame), typeof(ExtendedFrameRenderer))]

namespace Custom.DatePickerTest.Droid.Renderers
{
    using Android.Content;
    using Android.Graphics;
    using Xamarin.Forms.Platform.Android;

    /// <summary>
    /// An extended frame renderer.
    /// </summary>
    ///
    /// <seealso cref="T:Xamarin.Forms.Platform.Android.FrameRenderer"/>
    public class ExtendedFrameRenderer : FrameRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedFrameRenderer"/> class.
        /// </summary>
        ///
        /// <param name="context"> The context. </param>
        public ExtendedFrameRenderer(Context context)
            : base(context)
        {
        }

        /// <summary>
        /// Executes the draw action.
        /// </summary>
        ///
        /// <param name="canvas"> The canvas. </param>
        protected override void OnDraw(Canvas? canvas)
        {
            var frame = (ExtendedFrame)this.Element;

            using var firstPaint = new Paint();
            using var secondPaint = new Paint();
            using var backgroundPaint = new Paint();

            firstPaint.AntiAlias = true;
            firstPaint.SetStyle(Paint.Style.Stroke);
            firstPaint.StrokeWidth = frame.BorderWidth + 2;
            firstPaint.Color = frame.BorderColor.ToAndroid();

            secondPaint.AntiAlias = true;
            secondPaint.SetStyle(Paint.Style.Stroke);
            secondPaint.StrokeWidth = frame.BorderWidth;
            secondPaint.Color = frame.BackgroundColor.ToAndroid();

            backgroundPaint.SetStyle(Paint.Style.Stroke);
            backgroundPaint.StrokeWidth = 4;
            backgroundPaint.Color = frame.BackgroundColor.ToAndroid();

            using Rect oldBounds = new ();
            if (canvas != null)
            {
                canvas.GetClipBounds(oldBounds);

                using RectF oldOutlineBounds = new ();
                oldOutlineBounds.Set(oldBounds);

                using RectF myOutlineBounds = new ();
                myOutlineBounds.Set(oldBounds);
                myOutlineBounds.Top += (int)secondPaint.StrokeWidth + 3;
                myOutlineBounds.Bottom -= (int)secondPaint.StrokeWidth + 3;
                myOutlineBounds.Left += (int)secondPaint.StrokeWidth + 3;
                myOutlineBounds.Right -= (int)secondPaint.StrokeWidth + 3;

                canvas.DrawRoundRect(oldOutlineBounds, 10, 10, backgroundPaint); // to "hide" old outline
                canvas.DrawRoundRect(myOutlineBounds, frame.CornerRadius, frame.CornerRadius, firstPaint);
                canvas.DrawRoundRect(myOutlineBounds, frame.CornerRadius, frame.CornerRadius, secondPaint);

                base.OnDraw(canvas);
            }
        }
    }
}