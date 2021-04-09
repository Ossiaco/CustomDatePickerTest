// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.iOS
{
    using Custom.DatePickerTest.Hosting;
    using FFImageLoading.Forms.Platform;
    using FFImageLoading.Svg.Forms;
    using Foundation;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Identity.Client;
    using UIKit;
    using Xamarin.Essentials;
    using Xamarin.Forms.Platform.iOS;

    /// <summary>
    /// The UIApplicationDelegate for the application. This class is responsible for launching the
    /// User Interface of the application, as well as listening (and optionally responding) to
    /// application events from iOS.
    /// </summary>
    ///
    /// <seealso cref="FormsApplicationDelegate"/>
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        /// <summary>
        /// This method is invoked when the application has loaded and is ready to run. In this method
        /// you should instantiate the window, load the UI into it and then make the window visible.
        /// You have 17 seconds to return from this method, or iOS will terminate your application.
        /// </summary>
        ///
        /// <param name="app">      Reference to the UIApplication that invoked this delegate method. </param>
        /// <param name="options">  An NSDictionary with the launch options, can be null.   Possible key
        ///                         values are UIApplication's LaunchOption static properties. </param>
        /// <returns>
        /// True if it succeeds, false if it fails.
        /// </returns>
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Forms.Forms.Init();
            CachedImageRenderer.Init();
            var ignore = typeof(SvgCachedImage);

            MainThread.BeginInvokeOnMainThread(() => this.LoadApplication(Startup.Init(this.ConfigureServices)));

            return base.FinishedLaunching(app, options);
        }

        /// <inheritdoc/>
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
            return true;
        }

        private void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
        }
    }
}
