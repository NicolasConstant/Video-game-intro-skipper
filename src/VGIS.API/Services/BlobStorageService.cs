using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Vgis_crowdsourcing_api.Services
{
    public class BlobStorageService
    {
        private readonly string _storageCs;
        private readonly string _containerName;

        public BlobStorageService(string storageCs, string containerName)
        {
            _storageCs = storageCs;
            _containerName = containerName;
        }

        public async Task<string> UploadFileToBlob(byte[] fileBytes, string fileName)
        {
            if (!CloudStorageAccount.TryParse(_storageCs, out var storageAccount))
                throw new ArgumentException("Wrong Azure Blob CS");

            var uniqueFileName = Guid.NewGuid() + "--" + fileName;
            var cloudBlobClient = storageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(_containerName);

            //Create blob if needed
            if (!await cloudBlobContainer.ExistsAsync())
                await cloudBlobContainer.CreateAsync();

            //Upload file
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(Guid.NewGuid() + "--" + fileName);
            await cloudBlockBlob.UploadFromByteArrayAsync(fileBytes, 0, fileBytes.Length);

            return uniqueFileName;

            //// List the blobs in the container.
            //BlobContinuationToken blobContinuationToken = null;
            //do
            //{
            //    var results = await cloudBlobContainer.ListBlobsSegmentedAsync(null, blobContinuationToken);
            //    // Get the value of the continuation token returned by the listing call.
            //    blobContinuationToken = results.ContinuationToken;
            //    foreach (IListBlobItem item in results.Results)
            //    {
            //        Console.WriteLine(item.Uri);
            //    }
            //} while (blobContinuationToken != null); // Loop while the continuation token is not null.

            //await cloudBlockBlob.DownloadToFileAsync(destinationFile, FileMode.Create);

        }
    }
}