using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AzureImageUploadAppService.Models
{
    public class FormUploadModel
    {
        public string Title { get; set; }
        public IFormFile Image { get; set; }
    }
}
