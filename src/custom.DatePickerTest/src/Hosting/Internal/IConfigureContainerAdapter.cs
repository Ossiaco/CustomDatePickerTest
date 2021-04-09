// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Hosting.Internal
{
    using Microsoft.Extensions.Hosting;

    internal interface IConfigureContainerAdapter
    {
        void ConfigureContainer(HostBuilderContext hostContext, object containerBuilder);
    }
}