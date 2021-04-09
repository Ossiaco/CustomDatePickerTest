// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Hosting.Internal
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Hosting.Internal;
    using Microsoft.Extensions.Logging;

    internal class ClientHostBuilder : IHostBuilder
    {
        private readonly List<Action<HostBuilderContext, IConfigurationBuilder>> configureAppConfigActions = new List<Action<HostBuilderContext, IConfigurationBuilder>>();
        private readonly List<IConfigureContainerAdapter> configureContainerActions = new List<IConfigureContainerAdapter>();
        private readonly List<Action<IConfigurationBuilder>> configureHostConfigActions = new List<Action<IConfigurationBuilder>>();
        private readonly List<Action<HostBuilderContext, IServiceCollection>> configureServicesActions = new List<Action<HostBuilderContext, IServiceCollection>>();
        private IConfiguration? appConfiguration;
        private IServiceProvider? appServices;
        private HostBuilderContext? hostBuilderContext;
        private bool hostBuilt;
        private IConfiguration? hostConfiguration;
        private HostingEnvironment? hostingEnvironment;
        private IServiceFactoryAdapter serviceProviderFactory = new ServiceFactoryAdapter<IServiceCollection>(new DefaultServiceProviderFactory());

        public IDictionary<object, object> Properties { get; } = new Dictionary<object, object>();

        public IHost Build()
        {
            if (this.hostBuilt)
            {
                throw new InvalidOperationException("Build can only be called once.");
            }

            this.hostBuilt = true;
            this.BuildHostConfiguration();
            this.CreateHostingEnvironment();
            this.CreateHostBuilderContext();
            this.BuildAppConfiguration();
            this.CreateServiceProvider();
            if (this.appServices != null)
            {
                return this.appServices.GetRequiredService<IHost>();
            }

            return new HostBuilder().Build();
        }

        public IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            var configureAppConfigActions = this.configureAppConfigActions;
            if (configureDelegate == null)
            {
                throw new ArgumentNullException(nameof(configureDelegate));
            }

            configureAppConfigActions.Add(configureDelegate);
            return this;
        }

        public IHostBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
        {
            var configureContainerActions = this.configureContainerActions;
            if (configureDelegate == null)
            {
                throw new ArgumentNullException(nameof(configureDelegate));
            }

            configureContainerActions.Add(new ConfigureContainerAdapter<TContainerBuilder>(configureDelegate));
            return this;
        }

        public IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            var configureHostConfigActions = this.configureHostConfigActions;
            if (configureDelegate == null)
            {
                throw new ArgumentNullException(nameof(configureDelegate));
            }

            configureHostConfigActions.Add(configureDelegate);
            return this;
        }

        public IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            var configureServicesActions = this.configureServicesActions;
            if (configureDelegate == null)
            {
                throw new ArgumentNullException(nameof(configureDelegate));
            }

            configureServicesActions.Add(configureDelegate);
            return this;
        }

        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
            where TContainerBuilder : notnull
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            this.serviceProviderFactory = new ServiceFactoryAdapter<TContainerBuilder>(factory);
            return this;
        }

        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory)
            where TContainerBuilder : notnull
        {
            Func<HostBuilderContext> contextResolver = () => this.hostBuilderContext!;
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            this.serviceProviderFactory = new ServiceFactoryAdapter<TContainerBuilder>(contextResolver, factory);
            return this;
        }

        private void BuildAppConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder().SetBasePath(this.hostingEnvironment!.ContentRootPath).AddConfiguration(this.hostConfiguration);
            foreach (var configureAppConfigAction in this.configureAppConfigActions)
            {
                configureAppConfigAction(this.hostBuilderContext!, configurationBuilder);
            }

            this.appConfiguration = configurationBuilder.Build();
            this.hostBuilderContext!.Configuration = this.appConfiguration;
        }

        private void BuildHostConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder().AddInMemoryCollection();
            var envConfig = configurationBuilder.AddEnvironmentVariables("DOTNET_").Build();
            if (string.IsNullOrEmpty(envConfig[HostDefaults.EnvironmentKey]))
            {
                envConfig[HostDefaults.EnvironmentKey] = Environment.GetEnvironmentVariable("Hosting:Environment") ?? Environment.GetEnvironmentVariable("DOTNET_ENV") ?? "production";
            }

            foreach (var configureHostConfigAction in this.configureHostConfigActions)
            {
                configureHostConfigAction(configurationBuilder);
            }

            this.hostConfiguration = configurationBuilder.Build();
        }

        private void CreateHostBuilderContext()
        {
            this.hostBuilderContext = new HostBuilderContext(this.Properties)
            {
                HostingEnvironment = this.hostingEnvironment,
                Configuration = this.hostConfiguration,
            };
        }

        private void CreateHostingEnvironment()
        {
            this.hostingEnvironment = new HostingEnvironment
            {
                ApplicationName = this.hostConfiguration![HostDefaults.ApplicationKey],
                EnvironmentName = this.hostConfiguration[HostDefaults.EnvironmentKey] ?? Environments.Production,
                ContentRootPath = this.ResolveContentRootPath(this.hostConfiguration[HostDefaults.ContentRootKey], AppContext.BaseDirectory),
            };
            this.hostingEnvironment.ContentRootFileProvider = new PhysicalFileProvider(this.hostingEnvironment.ContentRootPath);
        }

        private void CreateServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            ((IServiceCollection)serviceCollection).AddSingleton((IHostEnvironment)this.hostingEnvironment!);
            if (this.hostBuilderContext != null)
            {
                serviceCollection.AddSingleton(this.hostBuilderContext);
            }

            if (this.appConfiguration != null)
            {
                serviceCollection.AddSingleton(this.appConfiguration);
            }

            serviceCollection.AddSingleton<IHostApplicationLifetime, ApplicationLifetime>();
            serviceCollection.AddSingleton<IHostLifetime, AppDomainLifetime>();
            serviceCollection.AddSingleton<IHost, ClientHost>();
            serviceCollection.AddOptions();
            serviceCollection.AddLogging();
            foreach (var configureServicesAction in this.configureServicesActions)
            {
                configureServicesAction(this.hostBuilderContext!, serviceCollection);
            }

            var containerBuilder = this.serviceProviderFactory.CreateBuilder(serviceCollection);
            foreach (var configureContainerAction in this.configureContainerActions)
            {
                configureContainerAction.ConfigureContainer(this.hostBuilderContext!, containerBuilder);
            }

            this.appServices = this.serviceProviderFactory.CreateServiceProvider(containerBuilder);
            if (this.appServices == null)
            {
                throw new InvalidOperationException("The IServiceProviderFactory returned a null IServiceProvider.");
            }
        }

        private string ResolveContentRootPath(string contentRootPath, string basePath)
        {
            if (string.IsNullOrEmpty(contentRootPath))
            {
                return basePath;
            }

            if (Path.IsPathRooted(contentRootPath))
            {
                return contentRootPath;
            }

            return Path.Combine(Path.GetFullPath(basePath), contentRootPath);
        }
    }
}