using System;
using System.IO;
using System.Threading.Tasks;
using AzureImageUploadAppService.ApiControllers;
using AzureImageUploadAppService.Interfaces;
using AzureImageUploadAppService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AzureImageUploadAppService.Tests.ApiControllers
{
    public class ImageUploadControllerTests
    {
        [Fact]
        public async Task Should_Upload_Image_To_Storage()
        {
            Mock<IImageStore> mockedImageStore = new Mock<IImageStore>();

            mockedImageStore.Setup(r => r.SaveImage(It.IsAny<string>(), It.IsAny<Stream>())).Returns(Task.CompletedTask);

            ImageUploadController controller = new ImageUploadController(mockedImageStore.Object);

            var fileMock = new Mock<IFormFile>();
            var content = "some file content";
            var fileName = "test.jpg";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(r => r.OpenReadStream()).Returns(ms);
            fileMock.Setup(r => r.FileName).Returns(fileName);
            fileMock.Setup(r => r.Length).Returns(ms.Length);

            var model = new FormUploadModel { Title = "Asad", Image = fileMock.Object };

            var result = await controller.Upload(model) as OkObjectResult;

            var response = result.Value as Response;

            Assert.Null(response.Error);
            Assert.True(response.Success);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Should_Not_Upload_Image_And_Return_Missing_Error_When_Title_Missing()
        {
            Mock<IImageStore> mockedImageStore = new Mock<IImageStore>();

            mockedImageStore.Setup(r => r.SaveImage(It.IsAny<string>(), It.IsAny<Stream>())).Returns(Task.CompletedTask);

            ImageUploadController controller = new ImageUploadController(mockedImageStore.Object);

            var fileMock = new Mock<IFormFile>();
            var content = "some file content";
            var fileName = "test.jpg";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(r => r.OpenReadStream()).Returns(ms);
            fileMock.Setup(r => r.FileName).Returns(fileName);
            fileMock.Setup(r => r.Length).Returns(ms.Length);

            var model = new FormUploadModel { Image = fileMock.Object };

            var result = await controller.Upload(model) as BadRequestObjectResult;

            var response = result.Value as Response;

            Assert.NotNull(response.Error);
            Assert.False(response.Success);
            Assert.Equal(400, result.StatusCode);
        }


        [Fact]
        public async Task Should_Not_Upload_Image_And_Return_Missing_Error_When_Image_Missing()
        {
            Mock<IImageStore> mockedImageStore = new Mock<IImageStore>();

            mockedImageStore.Setup(r => r.SaveImage(It.IsAny<string>(), It.IsAny<Stream>())).Returns(Task.CompletedTask);

            ImageUploadController controller = new ImageUploadController(mockedImageStore.Object);

            var model = new FormUploadModel { Title = "MyTitle"};

            var result = await controller.Upload(model) as BadRequestObjectResult;

            var response = result.Value as Response;

            Assert.NotNull(response.Error);
            Assert.False(response.Success);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task Should_Not_Upload_Image_And_Return_Error_When_Exception_Raised_Inside_SaveImage_Method()
        {
            Mock<IImageStore> mockedImageStore = new Mock<IImageStore>();

            mockedImageStore.Setup(r => r.SaveImage(It.IsAny<string>(), It.IsAny<Stream>()))
                .Returns(Task.FromException(new ApplicationException("Sample exception")));

            ImageUploadController controller = new ImageUploadController(mockedImageStore.Object);

            var fileMock = new Mock<IFormFile>();
            var content = "some file content";
            var fileName = "test.jpg";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(r => r.OpenReadStream()).Returns(ms);
            fileMock.Setup(r => r.FileName).Returns(fileName);
            fileMock.Setup(r => r.Length).Returns(ms.Length);

            var model = new FormUploadModel { Title = "MyTitle", Image = fileMock.Object };

            var result = await controller.Upload(model) as BadRequestObjectResult;

            var response = result.Value as Response;

            Assert.NotNull(response.Error);
            Assert.False(response.Success);
            Assert.Equal(400, result.StatusCode);
        }
    }
}