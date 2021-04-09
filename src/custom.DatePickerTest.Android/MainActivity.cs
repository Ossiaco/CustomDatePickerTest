// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Droid
{
    using Android.App;
    using Android.Content;
    using Android.Content.PM;
    using Android.OS;
    using Android.Runtime;
    using Android.Views;
    using Custom.DatePickerTest.Hosting;
    using FFImageLoading.Forms.Platform;
    using FFImageLoading.Svg.Forms;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Identity.Client;
    using Plugin.CurrentActivity;
    using Xamarin.Essentials;
    using Xamarin.Forms.Platform.Android;

    /// <summary>
    /// A main activity.
    /// </summary>
    ///
    /// <seealso cref="FormsAppCompatActivity"/>
    [Activity(Label = "Custom.DatePickerTest.Android", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : FormsAppCompatActivity
    {
        /// <inheritdoc/>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <inheritdoc/>
        public override void OnBackPressed()
        {
        }

        /// <inheritdoc/>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CachedImageRenderer.Init(true);
            var ignore = typeof(SvgCachedImage);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat && this.Window != null)
            {
                this.Window.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);
            }

            MainThread.BeginInvokeOnMainThread(() => this.LoadApplication(Startup.Init(this.ConfigureServices, CrossCurrentActivity.Current.Activity)));
        }

        /// <inheritdoc/>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
        }

        /// <summary>
        /// Configure services.
        /// </summary>
        ///
        /// <param name="ctx">      The context. </param>
        /// <param name="services"> The services. </param>
        private void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
        }
    }
}