// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Hosting.Internal
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    internal class ServiceFactoryAdapter<TContainerBuilder> : IServiceFactoryAdapter
        where TContainerBuilder : notnull
    {
        private readonly Func<HostBuilderContext>? contextResolver;
        private readonly Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>>? factoryResolver;
        private IServiceProviderFactory<TContainerBuilder>? serviceProviderFactory;

        public ServiceFactoryAdapter(IServiceProviderFactory<TContainerBuilder> serviceProviderFactory)
        {
            this.serviceProviderFactory = serviceProviderFactory ?? throw new ArgumentNullException(nameof(serviceProviderFactory));
        }

        public ServiceFactoryAdapter(Func<HostBuilderContext> contextResolver, Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factoryResolver)
        {
            this.contextResolver = contextResolver ?? throw new ArgumentNullException(nameof(contextResolver));
            this.factoryResolver = factoryResolver ?? throw new ArgumentNullException(nameof(factoryResolver));
        }

        public object CreateBuilder(IServiceCollection services)
        {
            if (this.serviceProviderFactory == null)
            {
                this.serviceProviderFactory = this.factoryResolver!(this.contextResolver!());
                if (this.serviceProviderFactory == null)
                {
                    throw new InvalidOperationException("The resolver returned a null IServiceProviderFactory");
                }
            }

            return this.serviceProviderFactory.CreateBuilder(services)!;
        }

        public IServiceProvider CreateServiceProvider(object containerBuilder)
        {
            if (this.serviceProviderFactory == null)
            {
                throw new InvalidOperationException("CreateBuilder must be called before CreateServiceProvider");
            }

            return this.serviceProviderFactory.CreateServiceProvider((TContainerBuilder)containerBuilder);
        }
    }
}