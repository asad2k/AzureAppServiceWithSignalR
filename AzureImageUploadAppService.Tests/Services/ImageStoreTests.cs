using System.IO;
using System.Threading.Tasks;
using AzureImageUploadAppService.Interfaces;
using AzureImageUploadAppService.Models;
using AzureImageUploadAppService.Services;
using Microsoft.WindowsAzure.Storage.Blob;
using Moq;
using Xunit;

namespace AzureImageUploadAppService.Tests.Services
{
    public class ImageStoreTests
    {
        [Fact]
        public async Task Should_Save_Image()
        {
            var uri = new System.Uri("http://azure.com/somefile/");

            var mockedCloudStorageWrapper = new Mock<ICloudStorageWrapper>();
            var mockedCloudBlobClient = new Mock<CloudBlobClient>(uri);
            var mockedCloudBlobContainer = new Mock<CloudBlobContainer>(uri);
            var mockedCloudBlockBlob = new Mock<CloudBlockBlob>(uri);

            mockedCloudBlockBlob
                .Setup(r => r.UploadFromStreamAsync(It.IsAny<Stream>()))
                .Returns(Task.CompletedTask);

            mockedCloudBlobContainer
                .Setup(r => r.GetBlockBlobReference(It.IsAny<string>()))
                .Returns(mockedCloudBlockBlob.Object);

            mockedCloudBlobClient
                .Setup(r => r.GetContainerReference(It.IsAny<string>()))
                .Returns(mockedCloudBlobContainer.Object);

            mockedCloudStorageWrapper
                .Setup(r => r.GetCloudBlobClient())
                .Returns(mockedCloudBlobClient.Object);

            var mockedResponse = new Mock<Response>();
            var imageStore = new ImageStore(mockedCloudStorageWrapper.Object);
            await imageStore.SaveImage("", null);

            mockedCloudBlobClient.Verify(r => r.GetContainerReference(It.IsAny<string>()), Times.AtLeastOnce);

            mockedCloudBlobContainer.Verify(r => r.GetBlockBlobReference(It.IsAny<string>()), Times.AtLeastOnce);

            mockedCloudBlockBlob.Verify(r => r.UploadFromStreamAsync(It.IsAny<Stream>()), Times.AtLeastOnce);
        }
    }
}