// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest
{
    using System;
    using System.Threading.Tasks;
    using Custom.DatePickerTest.ViewModels.ChargingSchedule;
    using Custom.DatePickerTest.Views.ChargingSchedule;
    using ReactiveUI;
    using ReactiveUI.XamForms;
    using Splat;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    /// An application bootstrapper.
    /// </summary>
    ///
    /// <seealso cref="T:ReactiveUI.ReactiveObject"/>
    /// <seealso cref="T:ReactiveUI.IScreen"/>
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppBootstrapper"/> class.
        /// </summary>
        public AppBootstrapper()
        {
            this.Router = new RoutingState();
            Locator.CurrentMutable.RegisterConstant(this, typeof(IScreen));
            RegisterViews();
            RegisterViewModels();

            MainThread.BeginInvokeOnMainThread(async () => await NavigateToFirstViewModelAsync());
        }

        /// <summary>
        /// Gets the Router associated with this Screen.
        /// </summary>
        ///
        /// <value>
        /// The router.
        /// </value>
        ///
        /// <seealso cref="P:ReactiveUI.IScreen.Router"/>
        public RoutingState Router { get; }

        /// <summary>
        /// Creates main page.
        /// </summary>
        ///
        /// <returns>
        /// The new main page.
        /// </returns>
        public static Page CreateMainPage() => new RoutedViewHost();

        /// <summary>
        /// Registers the views.
        /// </summary>
        private static void RegisterViews()
        {
            Locator.CurrentMutable.Register(() => App.ServiceProvider.GetService<ListChargingScheduleView>(), typeof(IViewFor<ListChargingScheduleViewModel>));
            Locator.CurrentMutable.Register(() => App.ServiceProvider.GetService<EditChargingScheduleView>(), typeof(IViewFor<EditChargingScheduleViewModel>));
        }

        /// <summary>
        /// Registers the view models.
        /// </summary>
        private static void RegisterViewModels()
        {
            Locator.CurrentMutable.Register(() => App.ServiceProvider.GetService<ListChargingScheduleViewModel>(), typeof(IRoutableViewModel), nameof(ListChargingScheduleViewModel));
            Locator.CurrentMutable.Register(() => App.ServiceProvider.GetService<EditChargingScheduleViewModel>(), typeof(IRoutableViewModel), nameof(EditChargingScheduleViewModel));
        }

        private static Task NavigateToFirstViewModelAsync()
        {
            var hostScreen = Locator.Current.GetService<IScreen>();
            try
            {
                hostScreen.Router.NavigateAndReset.Execute(Locator.Current.GetService<IRoutableViewModel>(nameof(ListChargingScheduleViewModel))).Subscribe().Dispose();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.StackTrace);
            }

            return Task.CompletedTask;
        }
    }
}
