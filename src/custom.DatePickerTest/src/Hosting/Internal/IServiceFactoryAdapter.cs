// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Hosting.Internal
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    internal interface IServiceFactoryAdapter
    {
        object CreateBuilder(IServiceCollection services);

        IServiceProvider CreateServiceProvider(object containerBuilder);
    }
}