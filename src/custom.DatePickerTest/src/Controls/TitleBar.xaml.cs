// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Controls
{
    using System.Windows.Input;
    using Xamarin.Forms;

    /// <content>
    /// A title bar.
    /// </content>
    public partial class TitleBar
    {
        /// <summary>
        /// The title property.
        /// </summary>
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(TitleBar), propertyChanged: OnTitleTextChanged);

        /// <summary>
        /// The text color property.
        /// </summary>
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TitleBar), propertyChanged: OnTextColorChanged);

        /// <summary>
        /// The image property.
        /// </summary>
        public static readonly BindableProperty BackButtonImageSourceProperty = BindableProperty.Create(nameof(BackButtonImageSource), typeof(string), typeof(TitleBar), propertyChanged: OnBackButtonImageSourceChanged);

        /// <summary>
        /// The back button label text property.
        /// </summary>
        public static readonly BindableProperty BackButtonLabelTextProperty = BindableProperty.Create(nameof(BackButtonLabelText), typeof(string), typeof(TitleBar), propertyChanged: OnBackButtonLabelTextChanged);

        /// <summary>
        /// The command property.
        /// </summary>
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(TitleBar), propertyChanged: OnCommandSourceChanged);

        /// <summary>
        /// Initializes a new instance of the <see cref="TitleBar"/> class.
        /// </summary>
        public TitleBar()
        {
            this.InitializeComponent();

            this.CommandSource = new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
            };

            this.BackButtonImage.GestureRecognizers.Add(this.CommandSource);
            this.BackButtonLabel.GestureRecognizers.Add(this.CommandSource);
        }

        /// <summary>
        /// Gets or sets the title text.
        /// </summary>
        ///
        /// <value>
        /// The title text.
        /// </value>
        public string Title
        {
            get => (string)this.GetValue(TitleProperty);
            set => this.SetValue(TitleProperty, value);
        }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        ///
        /// <value>
        /// The color of the text.
        /// </value>
        public Color TextColor
        {
            get => (Color)this.GetValue(TextColorProperty);
            set => this.SetValue(TextColorProperty, value);
        }

        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        ///
        /// <value>
        /// The image source.
        /// </value>
        public string BackButtonImageSource
        {
            get => (string)this.GetValue(BackButtonImageSourceProperty);
            set => this.SetValue(BackButtonImageSourceProperty, value);
        }

        /// <summary>
        /// Gets or sets the back button label text.
        /// </summary>
        ///
        /// <value>
        /// The back button label text.
        /// </value>
        public string BackButtonLabelText
        {
            get => (string)this.GetValue(BackButtonImageSourceProperty);
            set => this.SetValue(BackButtonImageSourceProperty, value);
        }

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        ///
        /// <value>
        /// The command.
        /// </value>
        public ICommand Command
        {
            get => (ICommand)this.GetValue(CommandProperty);
            set => this.SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the command source.
        /// </summary>
        ///
        /// <value>
        /// The command source.
        /// </value>
        private TapGestureRecognizer CommandSource { get; set; }

        /// <summary>
        /// Executes the title text changed action.
        /// </summary>
        ///
        /// <param name="bindable"> The bindable. </param>
        /// <param name="oldValue"> The old value. </param>
        /// <param name="newValue"> The new value. </param>
        private static void OnTitleTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (TitleBar)bindable;
            if (view != null)
            {
                view.TitleText.Text = (string)newValue;
            }
        }

        /// <summary>
        /// Executes the text color changed action.
        /// </summary>
        ///
        /// <param name="bindable"> The bindable. </param>
        /// <param name="oldValue"> The old value. </param>
        /// <param name="newValue"> The new value. </param>
        private static void OnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (TitleBar)bindable;
            if (view != null)
            {
                view.TitleText.TextColor = (Color)newValue;
            }
        }

        /// <summary>
        /// Executes the back button image source changed action.
        /// </summary>
        ///
        /// <param name="bindable"> The bindable. </param>
        /// <param name="oldValue"> The old value. </param>
        /// <param name="newValue"> The new value. </param>
        private static void OnBackButtonImageSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (TitleBar)bindable;
            if (view != null)
            {
                view.BackButtonImage.Source = (string)newValue;
                view.BackButtonImage.IsVisible = true;
            }
        }

        /// <summary>
        /// Executes the back button label text changed action.
        /// </summary>
        ///
        /// <param name="bindable"> The bindable. </param>
        /// <param name="oldValue"> The old value. </param>
        /// <param name="newValue"> The new value. </param>
        private static void OnBackButtonLabelTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (TitleBar)bindable;
            if (view != null)
            {
                view.BackButtonLabel.Text = (string)newValue;
                view.BackButtonLabel.IsVisible = true;
            }
        }

        private static void OnCommandSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (TitleBar)bindable;
            if (view != null)
            {
                view.CommandSource.Command = (ICommand)newValue;
            }
        }
    }
}