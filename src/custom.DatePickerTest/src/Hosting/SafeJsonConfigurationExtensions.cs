// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Hosting
{
    using System;
    using System.IO;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.Json;
    using Microsoft.Extensions.FileProviders;

    internal static class SafeJsonConfigurationExtensions
    {
        public static IConfigurationBuilder SafeAddJsonFile(this IConfigurationBuilder builder, string path)
        {
            return builder.AddJsonFile(null, path, optional: false, reloadOnChange: false);
        }

        public static IConfigurationBuilder SafeAddJsonFile(this IConfigurationBuilder builder, string path, bool optional)
        {
            return builder.SafeAddJsonFile(null, path, optional, reloadOnChange: false);
        }

        public static IConfigurationBuilder SafeAddJsonFile(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange)
        {
            return builder.SafeAddJsonFile(null, path, optional, reloadOnChange);
        }

        public static IConfigurationBuilder SafeAddJsonFile(this IConfigurationBuilder builder, IFileProvider? provider, string path, bool optional, bool reloadOnChange)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                return builder;
            }

            return builder.AddJsonFile(delegate(JsonConfigurationSource s)
            {
                s.FileProvider = provider;
                s.Path = path;
                s.Optional = optional;
                s.ReloadOnChange = reloadOnChange;
                s.ResolveFileProvider();
            });
        }
    }
}
