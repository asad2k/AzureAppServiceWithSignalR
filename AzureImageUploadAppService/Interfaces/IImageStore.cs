using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AzureImageUploadAppService.Interfaces
{
    public interface IImageStore
    {
        Task SaveImage(string title, Stream imageStream);
    }
}
