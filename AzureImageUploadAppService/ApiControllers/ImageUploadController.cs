using System.Threading.Tasks;
using AzureImageUploadAppService.Interfaces;
using AzureImageUploadAppService.Models;
using Microsoft.AspNetCore.Mvc;

namespace AzureImageUploadAppService.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
        private readonly IImageStore imageStore;

        public ImageUploadController(IImageStore imageStore)
        {
            this.imageStore = imageStore;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm]FormUploadModel model)
        {
            if(string.IsNullOrEmpty(model.Title) || model.Image == null)
            {
                return this.BadRequest(new Response { Error = "Title And Image must be provided" });
            }

            try
            {
                using (var stream = model.Image.OpenReadStream())
                {
                    await this.imageStore.SaveImage(model.Title, stream);
                }
            }
            catch (System.Exception ex)
            {
                return this.BadRequest(new Response { Error = "Ooops something went wrong!" });
            }

            return this.Ok(new Response { Success = true });
        }
    }
}