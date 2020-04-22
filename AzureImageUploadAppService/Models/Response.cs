using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureImageUploadAppService.Models
{
    public class Response
    {
        public string Error { get; set; }
        public bool Success { get; set; }
    }

    public class Response<T> : Response where T : class
    {
        public T Data { get; set; }
    }
}
