// Copyright (c) PlaceholderCompany. All rights reserved.

using Custom.DatePickerTest.Controls;
using Custom.DatePickerTest.Droid.Renderers;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(CustomDatePicker), typeof(CustomDatePickerRenderer))]

namespace Custom.DatePickerTest.Droid.Renderers
{
    using Android.App;
    using Android.Content;
    using Android.Graphics.Drawables;
    using Android.Text.Format;
    using Xamarin.Forms.Platform.Android;

    /// <summary>
    /// An extended date time picker renderer.
    /// </summary>
    public class CustomDatePickerRenderer : DatePickerRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDatePickerRenderer"/> class.
        /// </summary>
        ///
        /// <param name="context"> The context. </param>
        public CustomDatePickerRenderer(Context context)
            : base(context)
        {
        }

        /// <summary>
        /// Executes the element changed action.
        /// </summary>
        ///
        /// <param name="e"> An ElementChangedEventArgs{DatePicker} to process. </param>
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (this.Control != null)
            {
                using var gd = new GradientDrawable();
                gd.SetStroke(0, Android.Graphics.Color.Transparent);
                this.Control.SetBackground(gd);
            }
        }

        /// <summary>
        /// Creates date picker dialog.
        /// </summary>
        ///
        /// <param name="year">  The year. </param>
        /// <param name="month"> The month. </param>
        /// <param name="day">   The day. </param>
        ///
        /// <returns>
        /// The new date picker dialog.
        /// </returns>
        protected override DatePickerDialog CreateDatePickerDialog(int year, int month, int day)
        {
            if (this.Context != null)
            {
                var datePickerDialog = new DatePickerDialog(
                    this.Context,
                    this.OnDateSet,
                    year,
                    month,
                    day);

                return datePickerDialog;
            }

            return base.CreateDatePickerDialog(year, month, day);
        }

        private void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs dateSetEventArgs)
        {
            if (this.Element != null && sender is DatePickerDialog datePickerDialog && this.Context != null)
            {
                using var timePickerDialog = new TimePickerDialog(
                    this.Context,
                    (o, e) => this.OnTimeSet(o, e, dateSetEventArgs),
                    0,
                    0,
                    DateFormat.Is24HourFormat(this.Context));
                timePickerDialog.Show();
                System.Console.WriteLine("DateSet");
            }
        }

        private void OnTimeSet(object sender, TimePickerDialog.TimeSetEventArgs timeSetEventArgs, DatePickerDialog.DateSetEventArgs dateSetEventArgs)
        {
            if (sender is TimePickerDialog timePickerDialog)
            {
                this.Element.Date = dateSetEventArgs.Date.AddHours(timeSetEventArgs.HourOfDay).AddMinutes(timeSetEventArgs.Minute);
                this.Element.Unfocus();
                System.Console.WriteLine("TimeSet");
            }
        }
    }
}