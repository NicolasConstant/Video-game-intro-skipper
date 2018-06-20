using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using VGIS.Downloader.Settings;


namespace VGIS.Downloader
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var settings = GetSettings();


            Console.WriteLine(settings.StorageAccountCs);
            Console.ReadKey();
        }

        private static StorageValues GetSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.prod.json", optional: true);

            var configuration = builder.Build();
            var settings = new StorageValues();
            configuration.GetSection("Storage").Bind(settings);
            return settings;
        }
    }
}
