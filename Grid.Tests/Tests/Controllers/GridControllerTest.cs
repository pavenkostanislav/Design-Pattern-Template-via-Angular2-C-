using Xunit;
using Grid.Tests.TestContext.Models;
using Grid.Tests.TestContext.Inits;
using System.Linq;
using Grid.Controllers;
using Grid.Managers;
using Grid.Models;
using Grid.Tests.Tools;


namespace Grid.Tests.Tests.Controllers
{
    public class GridControllerTest
    {
        [Fact(DisplayName = "KPMA.Managers.GridController<TableModel, ViewModel, FindModel> Run Constructor")]
        public void GridController_Constructor_Run()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                
                var manager = new GridManager<TableModel, ViewModel, FindModel>(context);
                var controller = new GridController<TableModel, ViewModel, FindModel>(manager);
                Assert.NotNull(controller);
            }
        }

        
        
        [Theory(DisplayName = "KPMA.Managers.GridController.GetGridNewModelAsync return JsonResult.Value=typeOf(TableModel)")]
        [InlineData(0)]
        [InlineData(2)]
        [InlineData(-64)]
        public async void GridController_GetGridNewModelAsync_IsTypeTableModelInJsonResultValue(int id)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestUsers(2).CreateTestEmployees(2).CreateTableModels(2);
                
                var manager = new GridManager<TableModel, ViewModel, FindModel>(context);
                var controller = new GridController<TableModel, ViewModel, FindModel>(manager);
                var result = controller.GetGridNewModel(id);

                Assert.IsType<TableModel>(((Microsoft.AspNetCore.Mvc.JsonResult)result).Value);
            }
        }


        [Theory(DisplayName = "KPMA.Managers.GridController.GetGridCopyModelAsync is type JsonResult")]
        [InlineData(2)]
        [InlineData(64)]
        public async void GridController_GetGridCopyModelAsync_RunIsTypeJsonResult(int id)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestUsers(id).CreateTestEmployees(id).CreateTableModels(id);
                
                var manager = new GridManager<TableModel, ViewModel, FindModel>(context);
                var controller = new GridController<TableModel, ViewModel, FindModel>(manager);
                var result = await controller.GetGridCopyModelAsync(id);

                Assert.IsType<Microsoft.AspNetCore.Mvc.JsonResult>(result);
            }
        }
        [Theory(DisplayName = "KPMA.Managers.GridController.GetGridCopyModelAsync return JsonResult.Value=typeOf(TableModel)")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async void GridController_GetGridCopyModelAsync_ReturnIsTypeCopyModel(int id)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestUsers(id).CreateTestEmployees(id).CreateTableModels(id);
                
                var manager = new GridManager<TableModel, ViewModel, FindModel>(context);
                var controller = new GridController<TableModel, ViewModel, FindModel>(manager);
                var result = await controller.GetGridCopyModelAsync(id);

                Assert.IsType<TableModel>(((Microsoft.AspNetCore.Mvc.JsonResult)result).Value);
            }
        }


        [Theory(DisplayName = "KPMA.Managers.GridController.GetGridModelAsync is type JsonResult")]
        [InlineData(2)]
        [InlineData(64)]
        public async void GridController_GetGridModelAsync_RunIsTypeJsonResult(int id)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestUsers(id).CreateTestEmployees(id).CreateTableModels(id);
                
                var manager = new GridManager<TableModel, ViewModel, FindModel>(context);
                var controller = new GridController<TableModel, ViewModel, FindModel>(manager);
                var result = await controller.GetGridModelAsync(id);

                Assert.IsType<Microsoft.AspNetCore.Mvc.JsonResult>(result);
            }
        }
        [Theory(DisplayName = "KPMA.Managers.GridController.GetGridModelAsync return JsonResult.Value=typeOf(ViewModel)")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async void GridController_GetGridModelAsync_ReturnIsTypeViewModel(int id)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestUsers(id).CreateTestEmployees(id).CreateTableModels(id);
                
                var manager = new GridManager<TableModel, ViewModel, FindModel>(context);
                var controller = new GridController<TableModel, ViewModel, FindModel>(manager);
                var result = await controller.GetGridModelAsync(id);

                Assert.IsType<ViewModel>(((Microsoft.AspNetCore.Mvc.JsonResult)result).Value);
            }
        }


        [Theory(DisplayName = "KPMA.Managers.GridController.GetSelectGridRowModelAsync is type JsonResult")]
        [InlineData(2)]
        [InlineData(64)]
        public async void GridController_GetSelectGridRowModelAsync_RunIsTypeJsonResult(int id)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestUsers(id).CreateTestEmployees(id).CreateTableModels(id);
                
                var manager = new GridManager<TableModel, ViewModel, FindModel>(context);
                var controller = new GridController<TableModel, ViewModel, FindModel>(manager);
                var result = await controller.GetSelectGridRowModelAsync(id);

                Assert.IsType<Microsoft.AspNetCore.Mvc.JsonResult>(result);
            }
        }
        [Theory(DisplayName = "KPMA.Managers.GridController.GetSelectGridRowModelAsync return JsonResult.Value=typeOf(SelectItemViewModel)")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async void GridController_GetSelectGridRowModelAsync_ReturnIsTypeSelectItemViewModel(int id)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestUsers(id).CreateTestEmployees(id).CreateTableModels(id);
                
                var manager = new GridManager<TableModel, ViewModel, FindModel>(context);
                var controller = new GridController<TableModel, ViewModel, FindModel>(manager);
                var result = await controller.GetSelectGridRowModelAsync(id);

                Assert.IsType<SelectItemViewModel>(((Microsoft.AspNetCore.Mvc.JsonResult)result).Value);
            }
        }

        [Theory(DisplayName = "KPMA.Managers.GridController.SelectGridFindAsync is type JsonResult")]
        [InlineData(null, null)]
        [InlineData(2, null)]
        [InlineData(64, "test")]
        public async void GridController_SelectGridFindAsync_RunIsTypeJsonResult(int? keyId, string term)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestUsers(64).CreateTestEmployees(64).CreateTableModels(64);
                
                var manager = new GridManager<TableModel, ViewModel, FindModel>(context);
                var controller = new GridController<TableModel, ViewModel, FindModel>(manager);
                var result = await controller.SelectGridFindAsync(keyId, term);

                Assert.IsType<Microsoft.AspNetCore.Mvc.JsonResult>(result);
            }
        }
        [Theory(DisplayName = "KPMA.Managers.GridController.SelectGridListAsync return JsonResult.Value=typeOf(SelectItemViewModel)")]
        [InlineData(null, null)]
        [InlineData(2, null)]
        [InlineData(-64, "")]
        [InlineData(256, "test")]
        public async void GridController_SelectGridListAsync_ReturnIsTypeSelectItemViewModel(int? keyId, string term)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestUsers(256).CreateTestEmployees(256).CreateTableModels(256);
                
                var manager = new GridManager<TableModel, ViewModel, FindModel>(context);
                var controller = new GridController<TableModel, ViewModel, FindModel>(manager);
                var result = await controller.SelectGridFindAsync(keyId, term);

                Assert.IsType<System.Collections.Generic.List<SelectItemViewModel>>(((Microsoft.AspNetCore.Mvc.JsonResult)result).Value);
            }
        }
        [Theory(DisplayName = "KPMA.Managers.GridController.SaveGridRowModelAsync Edit JsonResult Saved type is ViewModel & Value")]
        [InlineData(5)]
        [InlineData(25)]
        public async void GridController_SaveGridRowModelAsync_SavedTypeIsViewModel(int id)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestUsers(id).CreateTestEmployees(id).CreateTableModels(id);
                
                var manager = new GridManager<TableModel, ViewModel, FindModel>(context);
                var controller = new GridController<TableModel, ViewModel, FindModel>(manager);
                var model = context.Set<TableModel>().Find(id);
                model.Test = "Test";
                var result = await controller.SaveGridRowModelAsync(model);

                Assert.IsType<ViewModel>(((Microsoft.AspNetCore.Mvc.JsonResult)result).Value);
                Assert.Equal("Test", ((ViewModel)((Microsoft.AspNetCore.Mvc.JsonResult)result).Value).Test);
            }
        }
        [Theory(DisplayName = "KPMA.Managers.GridController.SaveGridListAsync Edit JsonResult Saved type is ViewModel & Value")]
        [InlineData(5)]
        [InlineData(25)]
        public async void GridController_SaveGridListAsync_SavedTypeIsViewModel(int id)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestUsers(id).CreateTestEmployees(id).CreateTableModels(id);
                
                var manager = new GridManager<TableModel, ViewModel, FindModel>(context);
                var controller = new GridController<TableModel, ViewModel, FindModel>(manager);
                var list = context.Set<TableModel>().ToList();
                foreach (var item in list)
                {
                    item.Test = "Test";
                }
                var result = await controller.SaveGridListAsync(list);

                Assert.IsType<System.Collections.Generic.List<ViewModel>>(((Microsoft.AspNetCore.Mvc.JsonResult)result).Value);
                foreach (var item in (System.Collections.Generic.List<ViewModel>)((Microsoft.AspNetCore.Mvc.JsonResult)result).Value)
                {
                    Assert.Equal("Test", item.Test);
                }
            }
        }
        [Theory(DisplayName = "KPMA.Managers.GridController.GetGridListAsync is type JsonResult")]
        [InlineData(null)]
        [InlineData(64)]
        [InlineData(2)]
        [InlineData(0)]
        [InlineData(-2)]
        [InlineData(-64)]
        public async void GridController_GetGridListAsync_RunIsTypeJsonResult(int? keyId)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestUsers(5).CreateTestEmployees(5).CreateTableModels(5);
                
                var manager = new GridManager<TableModel, ViewModel, FindModel>(context);
                var controller = new GridController<TableModel, ViewModel, FindModel>(manager);
                var result = await controller.GetGridListAsync(keyId);

                Assert.IsType<Microsoft.AspNetCore.Mvc.JsonResult>(result);
            }
        }
        [Theory(DisplayName = "KPMA.Managers.GridController.GetGridListAsync return JsonResult.Value=typeOf(List<ViewModel>)")]
        [InlineData(null)]
        [InlineData(64)]
        [InlineData(2)]
        [InlineData(0)]
        [InlineData(-2)]
        [InlineData(-64)]
        public async void GridController_GetGridListAsync_ReturnIsTypeSelectItemViewModel(int? keyId)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestUsers(256).CreateTestEmployees(256).CreateTableModels(256);
                
                var manager = new GridManager<TableModel, ViewModel, FindModel>(context);
                var controller = new GridController<TableModel, ViewModel, FindModel>(manager);
                var result = await controller.GetGridListAsync(keyId);

                Assert.IsType<System.Collections.Generic.List<ViewModel>>(((Microsoft.AspNetCore.Mvc.JsonResult)result).Value);
            }
        }

        [Fact(DisplayName = "KPMA.Managers.GridController.GetGridResponseModelAsync is type JsonResult")]
        public async void GridController_GetGridResponseModelAsync_RunIsTypeJsonResult()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                
                var manager = new GridManager<TableModel, ViewModel, FindModel>(context);
                var controller = new GridController<TableModel, ViewModel, FindModel>(manager);
                var result = await controller.GetGridResponseModelAsync(new RequestModel<FindModel>());

                Assert.IsType<Microsoft.AspNetCore.Mvc.JsonResult>(result);
            }
        }
        [Theory(DisplayName = "KPMA.Managers.GridController.GetGridResponseModelAsync return JsonResult.Value=typeOf(ResponseModel<ViewModel>)")]
        [InlineData(0, 25, null)]
        [InlineData(0, -65, 65)]
        [InlineData(-65, 25, 0)]
        public async void GridController_GetGridResponseModelAsync_ReturnIsTypeSelectItemViewModel(int currentPage, int pageSize, int? keyId)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestUsers(3).CreateTestEmployees(3).CreateTableModels(3);
                
                var manager = new GridManager<TableModel, ViewModel, FindModel>(context);
                var controller = new GridController<TableModel, ViewModel, FindModel>(manager);

                var rm = new RequestModel<FindModel>();
                rm.CurrentPage = currentPage;
                rm.PageSize = pageSize;
                rm.KeyId = keyId;

                var result = await controller.GetGridResponseModelAsync(rm);

                Assert.IsType<Models.ResponseModel<ViewModel>>(((Microsoft.AspNetCore.Mvc.JsonResult)result).Value);
            }
        }
        [Fact(DisplayName = "KPMA.Managers.GridController.GetGridResponseModelAsync return JsonResult.Value=typeOf(ResponseModel<ViewModel>) for null")]
        public async void GridController_GetGridResponseModelAsync_ReturnIsTypeSelectItemViewModel()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestUsers(3).CreateTestEmployees(3).CreateTableModels(3);
                
                var manager = new GridManager<TableModel, ViewModel, FindModel>(context);
                var controller = new GridController<TableModel, ViewModel, FindModel>(manager);
                var result = await controller.GetGridResponseModelAsync(null);

                Assert.IsType<Models.ResponseModel<ViewModel>>(((Microsoft.AspNetCore.Mvc.JsonResult)result).Value);
            }
        }
    }
}

