using AzureImageUploadAppService.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureImageUploadAppService.Services
{
    public class CloudStorageWrapper : ICloudStorageWrapper
    {
        private readonly CloudBlobClient cloudBlobClient;

        public CloudStorageWrapper(IConfiguration configuration)
        {
            string strorageconn = configuration["StorageConnectionString"];
            CloudStorageAccount storageacc = CloudStorageAccount.Parse(strorageconn);

            cloudBlobClient = storageacc.CreateCloudBlobClient();
        }

        public CloudBlobClient GetCloudBlobClient()
        {
            return cloudBlobClient;
        }
    }
}