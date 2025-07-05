﻿using Microsoft.Extensions.Configuration;
using System.IO;

namespace TraficViolation
{
    public static class ConfigHelper
    {
        public static IConfigurationRoot Configuration { get; }

        static ConfigHelper()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public static string GetConnectionString(string name = "DefaultConnection")
        {
            return Configuration.GetConnectionString(name);
        }
    }
}
