// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Hosting
{
    using System;
    using System.IO;
    using System.Reflection;
    using Custom.DatePickerTest;
    using Custom.DatePickerTest.ViewModels.ChargingSchedule;
    using Custom.DatePickerTest.Views.ChargingSchedule;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Console;
    using Splat;
    using Xamarin.Essentials;

    internal static class Startup
    {
        private static IHost? host;

        public static IServiceProvider? ServiceProvider { get; set; }

        public static App? Init(Action<HostBuilderContext, IServiceCollection> nativeConfigureServices, object? parentActivityOrWindow = null)
        {
            var configuration = new ConfigurationBuilder()
                .SetupConfiguration()
                .Build();

            var hostBuilder = new HostBuilder().ConfigureHost(configuration, nativeConfigureServices);

            host?.Dispose();
            host = hostBuilder.Build();
            host.Start();

            // Save our service provider so we can use it later.
            App.ServiceProvider = host.Services;
            App.ParentActivityOrWindow = parentActivityOrWindow;
            return App.ServiceProvider.GetService<App>();
        }

        private static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            // Register View Model
            services.AddSingleton<ListChargingScheduleViewModel>();
            services.AddSingleton<EditChargingScheduleViewModel>();

            // Register Views
            services.AddTransient<ListChargingScheduleView>();
            services.AddTransient<EditChargingScheduleView>();

            services.AddSingleton<App>();
        }

        private static IHostBuilder ConfigureHost(this IHostBuilder builder, IConfigurationRoot configuration, Action<HostBuilderContext, IServiceCollection> nativeConfigureServices)
        {
            return builder.UseContentRoot(FileSystem.AppDataDirectory) // Tell the host configuration where to file the file (this is required for Xamarin apps)
                .ConfigureHostConfiguration(builder => builder.AddConfiguration(configuration))
                .ConfigureAppConfiguration((hostContext, builder) => builder.AddConfiguration(configuration))
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    // Configure our local services and access the host configuration
                    nativeConfigureServices(hostBuilderContext, services);
                    ConfigureServices(hostBuilderContext, services);
                })
                .ConfigureLogging((hostingContext, loggingBuilder) =>
                {
                    loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    loggingBuilder.AddSimpleConsole(o =>
                    {
                        o.ColorBehavior = LoggerColorBehavior.Disabled;
                    });
                });
        }

        private static IConfigurationBuilder SetupConfiguration(this IConfigurationBuilder builder, string[]? args = default)
        {
            if (args != null)
            {
                builder.AddCommandLine(args);
            }

            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            var systemDir = FileSystem.CacheDirectory;
            var jsonPath = ExtractSaveResource(systemDir, $"{assemblyName}.appsettings.json");

            builder.SetBasePath(systemDir);

            // read in the configuration file!
            builder.AddJsonFile(jsonPath);

            var environmentName = builder.GetEnvironmentName();
            if (!string.IsNullOrEmpty(environmentName))
            {
                jsonPath = ExtractSaveResource(systemDir, $"{assemblyName}.appsettings.{environmentName}.json");
                builder.AddJsonFile(jsonPath);
            }

            return builder;
        }

        private static string ExtractSaveResource(string location, string filename)
        {
            var a = Assembly.GetExecutingAssembly();
            using var resFilestream = a.GetManifestResourceStream(filename);
            var full = string.Empty;
            if (resFilestream != null)
            {
                full = Path.Combine(location, filename);

                using var stream = File.Create(full);
                resFilestream.CopyTo(stream);
            }

            return full;
        }

        private static string? GetEnvironmentName(this IConfigurationBuilder builder)
        {
            var envConfig = builder.AddEnvironmentVariables("DOTNET_").Build();
            return envConfig[HostDefaults.EnvironmentKey] ?? "development";
        }
    }
}
