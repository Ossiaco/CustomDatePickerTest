// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Droid
{
    using System.Threading.Tasks;
    using Android.App;
    using Android.Content;
    using Android.OS;
    using Android.Util;

    /// <summary>
    /// A SplashScreen activity.
    /// </summary>
    [Activity(Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        private static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        /// <inheritdoc/>
        public override void OnBackPressed()
        {
        }

        /// <inheritdoc/>
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Log.Debug(TAG, "SplashActivity.OnCreate");
        }

        /// <summary>
        /// Launches the startup task.
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            var startupWork = new Task(() => { this.SimulateStartupAsync().ConfigureAwait(false); });
            startupWork.Start();
        }

        /// <summary>
        /// Simulates background work that happens behind the splash screen.
        /// </summary>
        private async Task SimulateStartupAsync()
        {
            Log.Debug(TAG, "Performing some startup work that takes a bit of time.");
            await Task.Delay(500).ConfigureAwait(false); // Simulate a bit of startup work.
            Log.Debug(TAG, "Startup work is finished - starting MainActivity.");
            using var intent = new Intent(Application.Context, typeof(MainActivity));
            this.StartActivity(intent);
        }
    }
}