using System;
using Xunit;
using Catalog.API.Controllers;
using Moq;
using Catalog.API.Services;
using System.Collections.Generic;
using Catalog.API.Models;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.IO;

namespace ServiceTest.Controller
{
    public class ImageControllerTest
    {
        private ImageController controller;
        private IDictionary<string, string> config;
        private Mock<ICatalogService> mockCatalogService;
        private IList<CatalogItem> mockItems = new List<CatalogItem>()
        {
            new CatalogItem() { Id = 1, CatalogTypeId = 2, CatalogBrandId = 2, Description = ".NET Bot Black Sweatshirt", Name = ".NET Bot Black Sweatshirt", Price = 19.5M, FileName = "Item 1.png", CreateDate = DateTime.Today },
            new CatalogItem() { Id = 2, CatalogTypeId = 1, CatalogBrandId = 2, Description = ".NET Black & White Mug", Name = ".NET Black & White Mug", Price= 8.50M, FileName = "Item 2.png", CreateDate = DateTime.Today },
            new CatalogItem() { Id = 3, CatalogTypeId = 2, CatalogBrandId = 5, Description = "Prism White T-Shirt", Name = "Prism White T-Shirt", Price = 12, FileName = "Item 3.png", CreateDate = DateTime.Today },
            new CatalogItem() { Id = 4, CatalogTypeId = 2, CatalogBrandId = 2, Description = ".NET Foundation Sweatshirt", Name = ".NET Foundation Sweatshirt", Price = 12, FileName = "Item 4.png", CreateDate = DateTime.Today },
            new CatalogItem() { Id = 5, CatalogTypeId = 3, CatalogBrandId = 5, Description = "Roslyn Red Sheet", Name = "Roslyn Red Sheet", Price = 8.5M, FileName = "Item 5.png", CreateDate = DateTime.Today },
            new CatalogItem() { Id = 6, CatalogTypeId = 2, CatalogBrandId = 2, Description = ".NET Blue Sweatshirt", Name = ".NET Blue Sweatshirt", Price = 12, FileName = "Item 6.png", CreateDate = DateTime.Today },
            new CatalogItem() { Id = 7, CatalogTypeId = 2, CatalogBrandId = 5, Description = "Roslyn Red T-Shirt", Name = "Roslyn Red T-Shirt", Price = 12, FileName = "Item 7.png", CreateDate = DateTime.Today },
            new CatalogItem() { Id = 8, CatalogTypeId = 2, CatalogBrandId = 5, Description = "Kudu Purple Sweatshirt", Name = "Kudu Purple Sweatshirt", Price = 8.5M, FileName = "Item 8.png", CreateDate = DateTime.Today },
            new CatalogItem() { Id = 9, CatalogTypeId = 1, CatalogBrandId = 5, Description = "Cup<T> White Mug", Name = "Cup<T> White Mug", Price = 12, FileName = "Item 9.png", CreateDate = DateTime.Today },
            new CatalogItem() { Id = 10, CatalogTypeId = 3, CatalogBrandId = 2, Description = ".NET Foundation Sheet", Name = ".NET Foundation Sheet", Price = 12, FileName = "Item 10.png", CreateDate = DateTime.Today },
            new CatalogItem() { Id = 11, CatalogTypeId = 3, CatalogBrandId = 2, Description = "Cup<T> Sheet", Name = "Cup<T> Sheet", Price = 8.5M, FileName = "Item 11.png", CreateDate = DateTime.Today },
            new CatalogItem() { Id = 12, CatalogTypeId = 2, CatalogBrandId = 5, Description = "Prism White TShirt", Name = "Prism White TShirt", Price = 12, FileName = "Item 12.png", CreateDate = DateTime.Today }
        };

        public ImageControllerTest()
        {
            SetUpMock();
            config = new Dictionary<string, string>()
            {
                { "image-path", "../../../../service/Images" }
            };
            controller = new ImageController(mockCatalogService.Object, config);
        }

        private void SetUpMock()
        {
            var mock = new Mock<ICatalogService>();
            mock.Setup(m => m.GetItems()).Returns(() => mockItems);
            this.mockCatalogService = mock;
        }

        [Fact]
        public void WhenCallGetImage_ShouldExist()
        {
            var method = controller.GetType().GetMethod("GetImage");
            Assert.NotNull(method);
        }

        [Fact]
        public async void WhenGetImage_ShouldGetImageFileFromId()
        {
            var id = 3;
            var result = await controller.GetImage(id) as FileContentResult;

            Assert.NotNull(result);
            
            var item = mockItems.Where(i => i.Id == id).SingleOrDefault();
            var path = Path.Combine(config["image-path"], item.FileName);
            var imageFileExtension = Path.GetExtension(item.FileName);
            var mimetype = "image/png";
            var buffer = System.IO.File.ReadAllBytes(path);
            var expected = controller.File(buffer, mimetype);

            Assert.Equal(expected.FileDownloadName, result.FileDownloadName);
            Assert.Equal(expected.LastModified, result.LastModified);
            Assert.Equal(expected.ContentType, result.ContentType);
        }
    }
}