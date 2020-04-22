using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureImageUploadAppService.Interfaces
{
    public interface ICloudStorageWrapper
    {
        CloudBlobClient GetCloudBlobClient();
    }
}
