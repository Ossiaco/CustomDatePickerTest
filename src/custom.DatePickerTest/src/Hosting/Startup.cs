// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Hosting
{
    using System;
    using Custom.DatePickerTest;
    using Custom.DatePickerTest.Hosting.Internal;
    using Custom.DatePickerTest.ViewModels.ChargingSchedule;
    using Custom.DatePickerTest.Views.ChargingSchedule;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Splat;
    using Xamarin.Essentials;

    internal static class Startup
    {
        private static IHost? host;

        public static IServiceProvider? ServiceProvider { get; set; }

        public static App? Init(Action<HostBuilderContext, IServiceCollection> nativeConfigureServices, object? parentActivityOrWindow = null)
        {
            var configuration = new ConfigurationBuilder().Build();

            var hostBuilder = new ClientHostBuilder().ConfigureHost(configuration, nativeConfigureServices);

            host?.Dispose();
            host = hostBuilder.Build();
            host.Start();

            // Save our service provider so we can use it later.
            App.ServiceProvider = host.Services;
            App.ParentActivityOrWindow = parentActivityOrWindow;
            return App.ServiceProvider.GetService<App>();
        }

        private static IHostBuilder ConfigureHost(this IHostBuilder builder, IConfigurationRoot configuration, Action<HostBuilderContext, IServiceCollection> nativeConfigureServices)
        {
            return builder.UseContentRoot(FileSystem.AppDataDirectory) // Tell the host configuration where to file the file (this is required for Xamarin apps)
                .ConfigureHostConfiguration(builder => builder.AddConfiguration(configuration))
                .ConfigureAppConfiguration((hostContext, builder) => builder.AddConfiguration(configuration)).ConfigureServices((hostBuilderContext, services) =>
                {
                    // Configure our local services and access the host configuration
                    nativeConfigureServices(hostBuilderContext, services);
                    ConfigureServices(hostBuilderContext, services);
                });
        }

        private static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            services.AddSingleton<ChargingScheduleDataStore>();
            services.AddSingleton<ListChargingScheduleViewModel>();
            services.AddSingleton<EditChargingScheduleViewModel>();

            services.AddTransient<ListChargingScheduleView>();
            services.AddTransient<EditChargingScheduleView>();

            services.AddSingleton<App>();
        }
    }
}
