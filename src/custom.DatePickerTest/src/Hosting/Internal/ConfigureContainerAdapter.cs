// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Hosting.Internal
{
    using System;
    using Microsoft.Extensions.Hosting;

    internal class ConfigureContainerAdapter<TContainerBuilder> : IConfigureContainerAdapter
    {
        private readonly Action<HostBuilderContext, TContainerBuilder> action;

        public ConfigureContainerAdapter(Action<HostBuilderContext, TContainerBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            this.action = action;
        }

        public void ConfigureContainer(HostBuilderContext hostContext, object containerBuilder)
        {
            this.action(hostContext, (TContainerBuilder)containerBuilder);
        }
    }
}