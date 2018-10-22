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

namespace ServiceTest.Controller
{
    public class CatalogControllerTest
    {
        private CatalogController controller;
        private Mock<ICatalogService> mockCatalogService;
        private IList<CatalogItem> mockItems = new List<CatalogItem>();
        private IList<CatalogType> mockTypes = new List<CatalogType>();
        private IList<CatalogBrand> mockBrands = new List<CatalogBrand>();

        public CatalogControllerTest()
        {
            SetUpMockData();
            SetUpMock();
            controller = new CatalogController(mockCatalogService.Object);
        }

        private void SetUpMockData()
        {
            mockTypes.Add(new CatalogType() { Id = Guid.NewGuid(), Type = "Mug", Icon = "local_cafe", IsSelected = true });
            mockTypes.Add(new CatalogType() { Id = Guid.NewGuid(), Type = "T-Shirt", Icon = "person", IsSelected = true });
            mockTypes.Add(new CatalogType() { Id = Guid.NewGuid(), Type = "Sheet", Icon = "label", IsSelected = true });
            mockTypes.Add(new CatalogType() { Id = Guid.NewGuid(), Type = "USB Memory Stick", Icon = "usb", IsSelected = true });

            mockBrands.Add(new CatalogBrand() { Id = Guid.NewGuid(), Brand = "Azure", Icon = "cloud", IsSelected = true });
            mockBrands.Add(new CatalogBrand() { Id = Guid.NewGuid(), Brand = ".NET", Icon = "code", IsSelected = true });
            mockBrands.Add(new CatalogBrand() { Id = Guid.NewGuid(), Brand = "Visual Studio", Icon = "polymer", IsSelected = true });
            mockBrands.Add(new CatalogBrand() { Id = Guid.NewGuid(), Brand = "SQL Server", Icon = "storage", IsSelected = true });
            mockBrands.Add(new CatalogBrand() { Id = Guid.NewGuid(), Brand = "Other", Icon = "category", IsSelected = true });

            mockItems.Add(new CatalogItem() { Id = Guid.NewGuid(), CatalogTypeId = mockTypes.Where(c => c.Type == "T-Shirt").Select(c => c.Id).First(), CatalogBrandId = mockBrands.Where(c => c.Brand == ".NET").Select(c => c.Id).First(), Description = ".NET Bot Black Sweatshirt", Name = ".NET Bot Black Sweatshirt", Price = 19.5M, FileName = "Item 1.png", CreatedDate = DateTime.Today });
            mockItems.Add(new CatalogItem() { Id = Guid.NewGuid(), CatalogTypeId = mockTypes.Where(c => c.Type == "Mug").Select(c => c.Id).First(), CatalogBrandId = mockBrands.Where(c => c.Brand == ".NET").Select(c => c.Id).First(), Description = ".NET Black & White Mug", Name = ".NET Black & White Mug", Price= 8.50M, FileName = "Item 2.png", CreatedDate = DateTime.Today });
            mockItems.Add(new CatalogItem() { Id = Guid.NewGuid(), CatalogTypeId = mockTypes.Where(c => c.Type == "T-Shirt").Select(c => c.Id).First(), CatalogBrandId = mockBrands.Where(c => c.Brand == "Other").Select(c => c.Id).First(), Description = "Prism White T-Shirt", Name = "Prism White T-Shirt", Price = 12, FileName = "Item 3.png", CreatedDate = DateTime.Today });
            mockItems.Add(new CatalogItem() { Id = Guid.NewGuid(), CatalogTypeId = mockTypes.Where(c => c.Type == "T-Shirt").Select(c => c.Id).First(), CatalogBrandId = mockBrands.Where(c => c.Brand == ".NET").Select(c => c.Id).First(), Description = ".NET Foundation Sweatshirt", Name = ".NET Foundation Sweatshirt", Price = 12, FileName = "Item 4.png", CreatedDate = DateTime.Today });
            mockItems.Add(new CatalogItem() { Id = Guid.NewGuid(), CatalogTypeId = mockTypes.Where(c => c.Type == "Sheet").Select(c => c.Id).First(), CatalogBrandId = mockBrands.Where(c => c.Brand == "Other").Select(c => c.Id).First(), Description = "Roslyn Red Sheet", Name = "Roslyn Red Sheet", Price = 8.5M, FileName = "Item 5.png", CreatedDate = DateTime.Today });
            mockItems.Add(new CatalogItem() { Id = Guid.NewGuid(), CatalogTypeId = mockTypes.Where(c => c.Type == "T-Shirt").Select(c => c.Id).First(), CatalogBrandId = mockBrands.Where(c => c.Brand == ".NET").Select(c => c.Id).First(), Description = ".NET Blue Sweatshirt", Name = ".NET Blue Sweatshirt", Price = 12, FileName = "Item 6.png", CreatedDate = DateTime.Today });
            mockItems.Add(new CatalogItem() { Id = Guid.NewGuid(), CatalogTypeId = mockTypes.Where(c => c.Type == "T-Shirt").Select(c => c.Id).First(), CatalogBrandId = mockBrands.Where(c => c.Brand == "Other").Select(c => c.Id).First(), Description = "Roslyn Red T-Shirt", Name = "Roslyn Red T-Shirt", Price = 12, FileName = "Item 7.png", CreatedDate = DateTime.Today });
            mockItems.Add(new CatalogItem() { Id = Guid.NewGuid(), CatalogTypeId = mockTypes.Where(c => c.Type == "T-Shirt").Select(c => c.Id).First(), CatalogBrandId = mockBrands.Where(c => c.Brand == "Other").Select(c => c.Id).First(), Description = "Kudu Purple Sweatshirt", Name = "Kudu Purple Sweatshirt", Price = 8.5M, FileName = "Item 8.png", CreatedDate = DateTime.Today });
            mockItems.Add(new CatalogItem() { Id = Guid.NewGuid(), CatalogTypeId = mockTypes.Where(c => c.Type == "Mug").Select(c => c.Id).First(), CatalogBrandId = mockBrands.Where(c => c.Brand == "Other").Select(c => c.Id).First(), Description = "Cup<T> White Mug", Name = "Cup<T> White Mug", Price = 12, FileName = "Item 9.png", CreatedDate = DateTime.Today });
            mockItems.Add(new CatalogItem() { Id = Guid.NewGuid(), CatalogTypeId = mockTypes.Where(c => c.Type == "Sheet").Select(c => c.Id).First(), CatalogBrandId = mockBrands.Where(c => c.Brand == ".NET").Select(c => c.Id).First(), Description = ".NET Foundation Sheet", Name = ".NET Foundation Sheet", Price = 12, FileName = "Item 10.png", CreatedDate = DateTime.Today });
            mockItems.Add(new CatalogItem() { Id = Guid.NewGuid(), CatalogTypeId = mockTypes.Where(c => c.Type == "Sheet").Select(c => c.Id).First(), CatalogBrandId = mockBrands.Where(c => c.Brand == ".NET").Select(c => c.Id).First(), Description = "Cup<T> Sheet", Name = "Cup<T> Sheet", Price = 8.5M, FileName = "Item 11.png", CreatedDate = DateTime.Today });
            mockItems.Add(new CatalogItem() { Id = Guid.NewGuid(), CatalogTypeId = mockTypes.Where(c => c.Type == "T-Shirt").Select(c => c.Id).First(), CatalogBrandId = mockBrands.Where(c => c.Brand == "Other").Select(c => c.Id).First(), Description = "Prism White TShirt", Name = "Prism White TShirt", Price = 12, FileName = "Item 12.png", CreatedDate = DateTime.Today });
        }
        private void SetUpMock()
        {
            var mock = new Mock<ICatalogService>();
            mock.Setup(m => m.GetItems()).Returns(() => mockItems);
            mock.Setup(m => m.GetTypes()).Returns(() => mockTypes);
            mock.Setup(m => m.GetBrands()).Returns(() => mockBrands);
            this.mockCatalogService = mock;
        }

        [Fact]
        public void WhenCallGetItems_ShouldExist()
        {
            AssertMethodExists("GetItems");
        }

        [Fact]
        public void WhenCallGetItemsByIds_ShouldExist()
        {
            AssertMethodExists("GetItemsByIds");
        }

        [Fact]
        public void WhenCallGetItemByName_ShouldExist()
        {
            AssertMethodExists("GetItemByName");
        }

        [Fact]
        public void WhenCallGetItemsByTypesAndBrands_ShouldExist()
        {
            AssertMethodExists("GetItemsByTypesAndBrands");
        }

        [Fact]
        public void WhenCallGetItemsByTypes_ShouldExist()
        {
            AssertMethodExists("GetItemsByTypes");
        }

        [Fact]
        public void WhenCallGetItemsByBrands_ShouldExist()
        {
            AssertMethodExists("GetItemsByBrands");
        }

        [Fact]
        public void WhenCallGetCatalogTypes_ShouldExist()
        {
            AssertMethodExists("GetCatalogTypes");
        }

        [Fact]
        public void WhenCallGetCatalogBrands_ShouldExist()
        {
            AssertMethodExists("GetCatalogBrands");
        }

        [Fact]
        public async void WhenCallGetItems_ShouldGetAllItems()
        {
            var result = await controller.GetItems() as OkObjectResult;
            AssertResultOk(result);
            Assert.Equal(mockItems, result.Value);
        }

        [Fact]
        public async void WhenCallGetItemsByIds_ShouldGetItemsFilteredByIds()
        {
            var ids = mockItems.Select(c => c.Id).ToArray();
            var result = await controller.GetItemsByIds(String.Join(',', ids)) as OkObjectResult;
            var expected = mockItems.Where(i => ids.Contains(i.Id)).ToList();
            AssertResultOk(result);
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public async void WhenCallGetItemsById_ShouldGetItemFilteredById()
        {
            var id = mockItems.First().Id;
            var result = await controller.GetItemsByIds(id.ToString()) as OkObjectResult;
            var expected = mockItems.Where(i => i.Id == id).ToList();
            AssertResultOk(result);
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public async void WhenCallGetItemsByName_ShouldGetItemFilteredByName()
        {
            var name = "Item 2";
            var result = await controller.GetItemByName(name) as OkObjectResult;
            var expected = mockItems.Where(i => i.Name == name).ToList();
            AssertResultOk(result);
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public async void WhenCallGetItemsByTypes_ShouldGetItemFilteredByTypes()
        {
            var ids = mockTypes.Select(c => c.Id).Take(3);
            var result = await controller.GetItemsByTypes(String.Join(',', ids)) as OkObjectResult;
            var expected = mockItems.Where(i => ids.Contains(i.CatalogTypeId)).ToList();
            AssertResultOk(result);
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public async void WhenCallGetItemsByBrands_ShouldGetItemFilteredByTypes()
        {
            var ids = mockBrands.Select(c => c.Id).Take(3);
            var result = await controller.GetItemsByBrands(String.Join(',', ids)) as OkObjectResult;
            var expected = mockItems.Where(i => ids.Contains(i.CatalogBrandId)).ToList();
            AssertResultOk(result);
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public async void WhenCallGetItemsByTypesAndBrands_ShouldGetItemFilteredByTypesAndBrands()
        {
            var typeIds = mockTypes.Select(c => c.Id).Take(2);
            var brandIds = mockBrands.Select(c => c.Id).Take(2);
            var result = await controller.GetItemsByTypesAndBrands(String.Join(',', typeIds), String.Join(',', brandIds)) as OkObjectResult;
            var expected = mockItems.Where(i => typeIds.Contains(i.CatalogTypeId) && brandIds.Contains(i.CatalogBrandId)).ToList();
            AssertResultOk(result);
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public async void WhenCallGetCatalogTypes_ShouldGetCatalogTypes()
        {
            var result = await controller.GetCatalogTypes() as OkObjectResult;
            AssertResultOk(result);
            Assert.Equal(mockTypes, result.Value);
        }

        [Fact]
        public async void WhenCallGetCatalogBrands_ShouldGetCatalogBrands()
        {
            var result = await controller.GetCatalogBrands() as OkObjectResult;
            AssertResultOk(result);
            Assert.Equal(mockBrands, result.Value);
        }

        private void AssertResultOk(OkObjectResult result)
        {
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        private void AssertMethodExists(string methodName)
        {
            var method = controller.GetType().GetMethod(methodName);
            Assert.NotNull(method);
        }
    }
}
