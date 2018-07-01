using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using VGIS.Azure;
using VGIS.Downloader.Domain;
using VGIS.Downloader.Repositories;
using VGIS.Downloader.Settings;


namespace VGIS.Downloader
{
    partial class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var settings = GetSettings();

                var blobStorageService = new BlobStorageService(settings.StorageAccountCs, settings.ContainerName);
                var tableStorageService = new TableStorageService(settings.StorageAccountCs, settings.TableName);
                var syncStatusRepository = new SyncStatusRepository();

                var downloadLogic = new DownloaderLogic(blobStorageService, tableStorageService, syncStatusRepository);
                var t = downloadLogic.RunAsync();
                t.Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("Job done!");
            Console.ReadLine();
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
