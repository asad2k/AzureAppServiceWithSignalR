using System.IO;
using System.Threading.Tasks;
using AzureImageUploadAppService.Interfaces;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureImageUploadAppService.Services
{
    public class ImageStore : IImageStore
    {
        private readonly CloudBlobClient cloudBlobClient;

        private readonly string baseUri = "https://asadazurestorageaccount.blob.core.windows.net/";

        public ImageStore(ICloudStorageWrapper cloudStorageWrapper)
        {
            this.cloudBlobClient = cloudStorageWrapper.GetCloudBlobClient();
        }

        public async Task SaveImage(string title, Stream imageStream)
        {
            var container = this.cloudBlobClient.GetContainerReference("images");

            var blob = container.GetBlockBlobReference(title);
            await blob.UploadFromStreamAsync(imageStream);
        }
    }
}