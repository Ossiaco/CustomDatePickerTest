// Copyright (c) PlaceholderCompany. All rights reserved.

using System;
using Custom.DatePickerTest.Controls;
using Custom.DatePickerTest.iOS.Extensions;
using Custom.DatePickerTest.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomDatePicker), typeof(CustomDatePickerRenderer))]

namespace Custom.DatePickerTest.iOS.Renderers
{
    /// <summary>
    /// An extended date picker renderer.
    /// </summary>
    ///
    /// <seealso cref="T:Xamarin.Forms.Platform.iOS.DatePickerRenderer"/>
    public class CustomDatePickerRenderer : DatePickerRenderer
    {
        /// <summary>
        /// Executes the element changed action.
        /// </summary>
        ///
        /// <param name="e"> An ElementChangedEventArgs{DatePicker} to process. </param>
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (this.Control != null && e?.NewElement is CustomDatePicker datePicker)
            {
                this.Control.BorderStyle = UITextBorderStyle.None;

                if (this.Control.InputAccessoryView is UIToolbar toolbar)
                {
                    var buttonTextAttributes = new UITextAttributes()
                    {
                        Font = UIFont.FromName("SFStrong", 15),
                        TextColor = UIColorExtensions.FromHex("5A5E62"),
                    };

                    using var doneButton = new UIBarButtonItem(datePicker.DoneButtonText, UIBarButtonItemStyle.Done, this.DoneButtonAction);
                    using var cancelButton = new UIBarButtonItem(datePicker.CancelButtonText, UIBarButtonItemStyle.Done, this.CancelButtonAction);
                    using var title = new UIBarButtonItem(datePicker.TitleText, UIBarButtonItemStyle.Plain, null) { Enabled = false };
                    using var space = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);

                    doneButton.SetTitleTextAttributes(buttonTextAttributes, UIControlState.Normal);
                    title.SetTitleTextAttributes(buttonTextAttributes, UIControlState.Normal);
                    title.SetTitleTextAttributes(buttonTextAttributes, UIControlState.Disabled);
                    cancelButton.SetTitleTextAttributes(buttonTextAttributes, UIControlState.Normal);
                    toolbar.SetItems(new UIBarButtonItem[] { cancelButton, space, title, space, doneButton }, true);
                }

                if (this.Control.InputView is UIDatePicker uiDatePicker)
                {
                    uiDatePicker.Mode = UIDatePickerMode.DateAndTime;
                }
            }
        }

        private void CancelButtonAction(object sender, EventArgs eventArgs)
        {
            this.Control.ResignFirstResponder();
            this.Element.Unfocus();
        }

        private void DoneButtonAction(object sender, EventArgs eventArgs)
        {
            this.Control.ResignFirstResponder();
        }
    }
}