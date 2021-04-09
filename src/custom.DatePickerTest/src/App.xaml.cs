// Copyright (c) PlaceholderCompany. All rights reserved.

using Xamarin.Forms;

[assembly: ExportFont("SFProTextRegular.ttf", Alias = "SFRegular")]
[assembly: ExportFont("SFProTextStrong.ttf", Alias = "SFStrong")]

namespace Custom.DatePickerTest
{
    using System;
    using Xamarin.Forms;

    /// <content>
    /// An application.
    /// </content>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Resources = new ();
            this.Resources.Add("FontFamily", "SFStrong");

            Device.SetFlags(new string[] { "RadioButton_Experimental" });

            new AppBootstrapper();
            this.MainPage = AppBootstrapper.CreateMainPage();
        }

        /// <summary>
        /// Gets or sets the parent activity or window.
        /// </summary>
        ///
        /// <value>
        /// The parent activity or window.
        /// </value>
        public static object? ParentActivityOrWindow { get; set; }

        /// <summary>
        /// Gets or sets the service provider.
        /// </summary>
        public static IServiceProvider? ServiceProvider { get; set; }

        /// <summary>
        /// Application developers override this method to perform actions when the application resumes
        /// from a sleeping state.
        /// </summary>
        ///
        /// <seealso cref="M:Xamarin.Forms.Application.OnResume()"/>
        protected override void OnResume()
        {
        }

        /// <summary>
        /// Application developers override this method to perform actions when the application enters
        /// the sleeping state.
        /// </summary>
        ///
        /// <seealso cref="M:Xamarin.Forms.Application.OnSleep()"/>
        protected override void OnSleep()
        {
        }

        /// <summary>
        /// Executes the start action.
        /// </summary>
        ///
        /// <seealso cref="M:Xamarin.Forms.Application.OnStart()"/>
        protected override void OnStart()
        {
        }
    }
}
