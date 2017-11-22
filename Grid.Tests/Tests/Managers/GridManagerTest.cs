using System;
using Xunit;
using System.Linq;
using Grid.Test.TestContext.Models;
using Grid.Test.TestContext.Inits;
using Microsoft.EntityFrameworkCore;
using Grid.Test.Tools;

namespace Grid.Test.Tests.Tools
{
    public class GridManagerTest
    {
        [Fact(DisplayName = "Grid.Managers.GridManager.Constructor Run")]
        public void GridManagerConstructorTest01_Run()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                Assert.NotNull(manager);
            }
        }
        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridRowNewModel Is type")]
        public void GridManagerGetGridRowNewModelTest01_IsType()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var tableModel = manager.GetGridRowNewModel();
                Assert.IsType<TableModel>(tableModel);
            }
        }
        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridRowFindModel Is type")]
        public void GridManagerGetGridRowFindModelTest01_IsType()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var findModel = manager.GetGridRowFindModel();
                Assert.IsType<FindModel>(findModel);
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridRowModelAsync Is type")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerGetGridRowModelAsync_IsType(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number).CreateTableModels(number);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Skip(number-1).Take(1).FirstOrDefault();

                var tableModel = await manager.GetGridRowModelAsync(model.Id);
                Assert.IsType<TableModel>(tableModel);
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridRowModelAsync [withOneLevelIncludes=true] Is type")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerGetGridRowModelAsyncWithOneLevelIncludes_IsType(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number).CreateTableModels(number);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Skip(number - 1).Take(1).FirstOrDefault();

                var tableModel = await manager.GetGridRowModelAsync(model.Id, true);
                Assert.IsType<TableModel>(tableModel);
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridRowModelAsync id <> 0")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerGetGridRowModelAsync_IdNot0(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number).CreateTableModels(number);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Skip(number - 1).Take(1).FirstOrDefault();

                var tableModel = await manager.GetGridRowModelAsync(model.Id);
                Assert.NotEqual(0, tableModel.Id);
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridRowModelAsync [withOneLevelIncludes=true] id <> 0")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerGetGridRowModelAsyncWithOneLevelIncludes_IdNot0(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number).CreateTableModels(number);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Skip(number - 1).Take(1).FirstOrDefault();

                var tableModel = await manager.GetGridRowModelAsync(model.Id, true);
                Assert.NotEqual(0,tableModel.Id);
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridRowModelAsync [withOneLevelIncludes=true] is includes")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerGetGridRowModelAsyncWithOneLevelIncludes_IsIncludes(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number).CreateTableModels(number);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>()
                                    .Skip(number - 1)
                                    .Take(1)
                                    .FirstOrDefault();

                var tableModel = await manager.GetGridRowModelAsync(model.Id, true);

                Assert.NotNull(tableModel.User);
                Assert.Null(tableModel.UserNull);
                Assert.NotNull(tableModel.Employee);
                Assert.Null(tableModel.EmployeeNull);

                Assert.IsType<User>(tableModel.User);
                Assert.IsType<Employee>(tableModel.Employee);
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridRowCopyModelAsync Is type")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerGetGridRowCopyModelAsync_IsType(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number).CreateTableModels(number);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Skip(number - 1).Take(1).FirstOrDefault();

                var tableModel = await manager.GetGridRowCopyModelAsync(model.Id);
                Assert.IsType<TableModel>(tableModel);
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridRowCopyModelAsync Id=0")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerGetGridRowCopyModelAsync_Id_0(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number).CreateTableModels(number);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Skip(number - 1).Take(1).FirstOrDefault();

                var tableModel = await manager.GetGridRowCopyModelAsync(model.Id);
                Assert.Equal(0,tableModel.Id);
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridRowCopyModelAsync Exception")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerGetGridRowCopyModelAsync_Exception(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number).CreateTableModels(number);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var lastindex = context.Set<TableModel>().Max(m => m.Id);
                var actualexception = await Test.Tools.TestTools.ThrowsAsync<Exception>(async () => await manager.GetGridRowCopyModelAsync(lastindex + 1));
                Assert.NotNull(actualexception.Message);
                Assert.Matches(@"Ошибка получения записи из базы ([^\d])", actualexception.Message);
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridRowViewModelAsync Is type")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerGetGridRowViewModelAsync_IsType(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number).CreateTableModels(number);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Skip(number - 1).Take(1).FirstOrDefault();

                var tableModel = await manager.GetGridRowViewModelAsync(model.Id);
                Assert.IsType<ViewModel>(tableModel);
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridRowViewModelAsync id <> 0")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerGetGridRowViewModelAsync_IdNot0(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number).CreateTableModels(number);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Skip(number - 1).Take(1).FirstOrDefault();

                var tableModel = await manager.GetGridRowViewModelAsync(model.Id);
                Assert.NotEqual(0, tableModel.Id);
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridRowViewModelAsync Exception")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerGetGridRowViewModelAsync_Exception(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number).CreateTableModels(number);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var lastindex = context.Set<TableModel>().Max(m => m.Id);
                var actualexception = await Test.Tools.TestTools.ThrowsAsync<Exception>(async () => await manager.GetGridRowViewModelAsync(lastindex + 1));
                Assert.NotNull(actualexception.Message);
                Assert.Matches(@"Ошибка получения записи из базы ([^\d])", actualexception.Message);
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.SaveGridRowModelAsync Is type")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerSaveGridRowModelAsync_IsType(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number);

                var model = InitTableModel.DbLoad(number, 5);

                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var tableModel = await manager.SaveGridRowModelAsync(model);

                Assert.IsType<ViewModel>(tableModel);
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.SaveGridRowModelAsync id <> 0")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerSaveGridRowModelAsync_IdNot0(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number);

                var model = InitTableModel.DbLoad(number, 5);

                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var tableModel = await manager.SaveGridRowModelAsync(model);

                Assert.NotEqual(0, tableModel.Id);
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.SaveGridRowModelAsync Update Model")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerSaveGridRowModelAsync_updatemodel(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number);

                var model = InitTableModel.DbLoad(number, 5);

                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                model = await manager.SaveGridRowModelAsync(model);
                var testmodel = context.Set<TableModel>().Find(model.Id);
                testmodel.Test = "test";
                var actualmodel = await manager.SaveGridRowModelAsync(testmodel);

                Assert.Equal("test", actualmodel.Test);
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.SaveGridRowModelAsync Exception Id < 0")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerSaveGridRowModelAsync_ExceptionNegativeId(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number);

                var model = InitTableModel.DbLoad(number, 5);
                model.Id = number * (-1);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var actualexception = await Test.Tools.TestTools.ThrowsAsync<Exception>(async () => await manager.SaveGridRowModelAsync(model));

                Assert.NotNull(actualexception.Message);
                Assert.Matches(@"Данные не сохранены! Ошибка сохранения записи ([^\d])", actualexception.Message);
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.SaveGridRowModelAsync DbUpdateConcurrencyException Id > 0")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerSaveGridRowModelAsync_DbUpdateConcurrencyExceptionPositiveId(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number);

                var model = InitTableModel.DbLoad(number, 5);
                model.Id = number;
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var temp = context.Set<TableModel>().FirstOrDefault(m => m.Id == model.Id);

                var actualexception = await Test.Tools.TestTools.ThrowsAsync<DbUpdateConcurrencyException> (async () => await manager.SaveGridRowModelAsync(model));

                Assert.NotNull(actualexception.Message);
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.SaveModelInContextAsync Exception Id < 0")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerSaveModelInContextAsync_ExceptionNegativeId(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number);

                var model = InitTableModel.DbLoad(number, 5);
                model.Id = number * (-1);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var actualexception = await Test.Tools.TestTools.ThrowsAsync<Exception>(async () => await manager.SaveModelInContextAsync(model));

                Assert.NotNull(actualexception.Message);
                Assert.Matches(@"Данные не сохранены! Ошибка сохранения записи ([^\d])", actualexception.Message);
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.SaveGridRowModelAsync Update Copy Model")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerSaveGridRowModelAsync_UpdateCopyModel(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number);

                var model = InitTableModel.DbLoad(number, 5);

                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                model = await manager.SaveGridRowModelAsync(model);
                var copymodel = await manager.GetGridRowCopyModelAsync(model.Id);
                var actualmodel = await manager.SaveGridRowModelAsync(copymodel);

                Assert.True(actualmodel.Id>0);
                Assert.NotEqual(model.Id, actualmodel.Id);
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.SaveGridRowModelAsync Save New Model")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerSaveGridRowModelAsync_SaveNewModel(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number);

                var model = InitTableModel.DbLoad(number, 5);

                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var newmodel = manager.GetGridRowNewModel();
                newmodel.UserId = context.Set<User>().FirstOrDefault().Id;
                newmodel.EmployeeId = context.Set<Employee>().FirstOrDefault().Id;
                newmodel.Test = "test";
                newmodel.IsBool = true;
                newmodel.Decimal = 1.01m;

                var actualmodel = await manager.SaveGridRowModelAsync(newmodel);

                Assert.True(actualmodel.Id > 0);
                Assert.Equal(newmodel.Test, actualmodel.Test);
                Assert.Equal(newmodel.UserId, actualmodel.UserId);
                Assert.Equal(newmodel.EmployeeId, actualmodel.EmployeeId);
                Assert.Equal(newmodel.IsBool, actualmodel.IsBool);
                Assert.Equal(newmodel.Decimal, actualmodel.Decimal);
                Assert.Equal(newmodel.UserId, actualmodel.UserId);
            }
        }
        
        [Theory(DisplayName = "Grid.Managers.GridManager.GetSelectItemViewModelAsync Is type")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerGetSelectItemViewModelAsync_IsType(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number).CreateTableModels(number);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Skip(number - 1).Take(1).FirstOrDefault();

                var tableModel = await manager.GetSelectItemViewModelAsync(model.Id);
                Assert.IsType<Grid.Models.SelectItemViewModel>(tableModel);
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.GetSelectItemViewModelAsync id > 0")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerGetSelectItemViewModelAsync_PositiveId(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number).CreateTableModels(number);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Skip(number - 1).Take(1).FirstOrDefault();
        
                var tableModel = await manager.GetSelectItemViewModelAsync(model.Id);
                Assert.NotEqual(0, tableModel.id);
                Assert.Equal(model.Id, tableModel.id);
                Assert.Equal(model.Id, tableModel.sort);
                Assert.Equal(model.DisplayName, tableModel.text);
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.GetSelectItemViewModelAsync Exception")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerGetSelectItemViewModelAsync_Exception(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number).CreateTableModels(number);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var lastindex = context.Set<TableModel>().Max(m => m.Id);
                var tableModel = await manager.GetSelectItemViewModelAsync(lastindex + 1);
                Assert.Null(tableModel);
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridSelectList Is type")]
        [InlineData("Краказябра")]
        [InlineData("table test text 1")]
        [InlineData("table")]
        [InlineData(null)]
        public void GridManagerGetGridSelectList_IsType(string term)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024,-1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = manager.GetGridSelectList(randKeyId, term);
                if(term == null)
                    Assert.IsType<Microsoft.EntityFrameworkCore.Internal.InternalDbSet<TableModel>>(actualList);
                else
                    Assert.IsType<Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable<TableModel>>(actualList);
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridSelectList Not null")]
        [InlineData("Краказябра")]
        [InlineData("table test text 1")]
        [InlineData("table")]
        [InlineData(null)]
        public void GridManagerGetGridSelectList_NotNull(string term)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024,-1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = manager.GetGridSelectList(randKeyId, term).ToList();
                Assert.NotNull(actualList);
                actualList.ForEach((item) => {
                    Assert.True(item.Id > 0);
                    Assert.NotEmpty(item.DisplayName);
                });
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridSelectList Count >= 0")]
        [InlineData("Краказябра")]
        [InlineData("table test text 1")]
        [InlineData("table")]
        [InlineData(null)]
        public void GridManagerGetGridSelectList_PositiveCount(string term)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024,-1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = manager.GetGridSelectList(randKeyId, term).ToList();
                Assert.True(actualList.Count >= 0);
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridSelectList The table ns not found")]
        public void GridManagerGetGridSelectList_TableisNotFound()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                var randKeyId = TestTools.rInt(1024,-1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var actualList = manager.GetGridSelectList(randKeyId, null).ToList();
                Assert.NotNull(actualList);
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridSelectListAsync Is type")]
        [InlineData("Краказябра")]
        [InlineData("table test text 1")]
        [InlineData("table")]
        [InlineData(null)]
        public async System.Threading.Tasks.Task GridManagerGetGridSelectListAsync_IsType(string term)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024,-1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = await manager.GetGridSelectListAsync(randKeyId, term);
                Assert.IsType<System.Collections.Generic.List<Grid.Models.SelectItemViewModel>>(actualList);
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridSelectListAsync Not null")]
        [InlineData("Краказябра")]
        [InlineData("table test text 1")]
        [InlineData("table")]
        [InlineData(null)]
        public async System.Threading.Tasks.Task GridManagerGetGridSelectListAsync_NotNull(string term)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024,-1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = (await manager.GetGridSelectListAsync(randKeyId, term)).ToList();
                Assert.NotNull(actualList);
                actualList.ForEach((item) => {
                    Assert.True(item.id > 0);
                    Assert.NotEmpty(item.text);
                });
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridSelectListAsync Not null sort")]
        [InlineData("Краказябра")]
        [InlineData("table test text 1")]
        [InlineData("table")]
        [InlineData(null)]
        public async System.Threading.Tasks.Task GridManagerGetGridSelectListAsync_NotNullSort(string term)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = (await manager.GetGridSelectListAsync(randKeyId, term)).ToList();
                Assert.NotNull(actualList);
                actualList.ForEach((item) => {
                    Assert.True(item.sort >= 0);
                });
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridSelectListAsync Count >= 0")]
        [InlineData("Краказябра")]
        [InlineData("table test text 1")]
        [InlineData("table")]
        [InlineData(null)]
        public async System.Threading.Tasks.Task GridManagerGetGridSelectListAsync_PositiveCount(string term)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024,-1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = await manager.GetGridSelectListAsync(randKeyId, term);
                Assert.True(actualList.Count >= 0);
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridSelectListAsync The table ns not found")]
        public async System.Threading.Tasks.Task GridManagerGetGridSelectListAsync_TableisNotFound()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                var randKeyId = TestTools.rInt(1024,-1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var actualList = await manager.GetGridSelectListAsync(randKeyId, null);
                Assert.NotNull(actualList);
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridListWithOneLevelIncludes Is type")]
        public void GridManagerGetGridListWithOneLevelIncludes_IsType()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = manager.GetGridListWithOneLevelIncludes();
                Assert.IsType<Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable<TableModel>>(actualList);
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridListWithOneLevelIncludes Not null")]
        public void GridManagerGetGridListWithOneLevelIncludes_NotNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = manager.GetGridListWithOneLevelIncludes().ToList();
                Assert.NotNull(actualList);
                actualList.ForEach((item) => {
                    Assert.True(item.Id > 0);
                    Assert.NotEmpty(item.DisplayName);
                });
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridListWithOneLevelIncludes Count >= 0")]
        public void GridManagerGetGridListWithOneLevelIncludes_PositiveCount()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = manager.GetGridListWithOneLevelIncludes().ToList();
                Assert.True(actualList.Count >= 0);
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridListWithOneLevelIncludes The table is not has rows")]
        public void GridManagerGetGridListWithOneLevelIncludes_TableisNotHasRows()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var actualList = manager.GetGridListWithOneLevelIncludes().ToList();
                Assert.NotNull(actualList);
            }
        }
        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridListWithOneLevelIncludes is includes")]
        public void GridManagerGetGridListWithOneLevelIncludes_IsIncludes()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {

                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = manager.GetGridListWithOneLevelIncludes().ToList();

                var excpectedlist = context.Set<TableModel>()
                                        .Include(m => m.User)
                                        .Include(m => m.UserNull)
                                        .Include(m => m.Employee)
                                        .Include(m => m.EmployeeNull)
                                        .ToList();


                actualList.ForEach((item) =>
                {
                    Assert.NotNull(item.User);
                    Assert.Null(item.UserNull);
                    Assert.NotNull(item.Employee);
                    Assert.Null(item.EmployeeNull);
                });
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridListWithOneLevelIncludesAsync Is type")]
        public async System.Threading.Tasks.Task GridManagerGetGridListWithOneLevelIncludesAsync_IsType()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = await manager.GetGridListWithOneLevelIncludesAsync();
                Assert.IsType<System.Collections.Generic.List<TableModel>>(actualList);
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridListWithOneLevelIncludesAsync Not null")]
        public async System.Threading.Tasks.Task GridManagerGetGridListWithOneLevelIncludesAsync_NotNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = (await manager.GetGridListWithOneLevelIncludesAsync()).ToList();
                Assert.NotNull(actualList);
                actualList.ForEach((item) => {
                    Assert.True(item.Id > 0);
                    Assert.NotEmpty(item.DisplayName);
                });
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridListWithOneLevelIncludesAsync Count >= 0")]
        public async System.Threading.Tasks.Task GridManagerGetGridListWithOneLevelIncludesAsync_PositiveCount()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = (await manager.GetGridListWithOneLevelIncludesAsync()).ToList();
                Assert.True(actualList.Count >= 0);
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridListWithOneLevelIncludesAsync The table is not has rows")]
        public async System.Threading.Tasks.Task GridManagerGetGridListWithOneLevelIncludesAsync_TableisNotHasRows()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var actualList = (await manager.GetGridListWithOneLevelIncludesAsync()).ToList();
                Assert.NotNull(actualList);
            }
        }
        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridListWithOneLevelIncludesAsync is includes")]
        public async System.Threading.Tasks.Task GridManagerGetGridListWithOneLevelIncludesAsync_IsIncludes()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {

                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = (await manager.GetGridListWithOneLevelIncludesAsync()).ToList();

                var excpectedlist = context.Set<TableModel>()
                                        .Include(m => m.User)
                                        .Include(m => m.UserNull)
                                        .Include(m => m.Employee)
                                        .Include(m => m.EmployeeNull)
                                        .ToList();


                actualList.ForEach((item) =>
                {
                    Assert.NotNull(item.User);
                    Assert.Null(item.UserNull);
                    Assert.NotNull(item.Employee);
                    Assert.Null(item.EmployeeNull);
                });
            }
        }








        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridList Is type")]
        public void GridManagerGetGridList_IsType()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = manager.GetGridList();
                Assert.IsType<Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable<TableModel>>(actualList);
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridList Not null")]
        public void GridManagerGetGridList_NotNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = manager.GetGridList().ToList();
                Assert.NotNull(actualList);
                actualList.ForEach((item) => {
                    Assert.True(item.Id > 0);
                    Assert.NotEmpty(item.DisplayName);
                });
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridList Count >= 0")]
        public void GridManagerGetGridList_PositiveCount()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = manager.GetGridList().ToList();
                Assert.True(actualList.Count >= 0);
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridList The table is not has rows")]
        public void GridManagerGetGridList_TableisNotHasRows()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var actualList = manager.GetGridList().ToList();
                Assert.NotNull(actualList);
            }
        }
        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridList is includes")]
        public void GridManagerGetGridList_IsIncludes()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {

                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = manager.GetGridList().ToList();

                var excpectedlist = context.Set<TableModel>()
                                        .Include(m => m.User)
                                        .Include(m => m.UserNull)
                                        .Include(m => m.Employee)
                                        .Include(m => m.EmployeeNull)
                                        .ToList();


                actualList.ForEach((item) =>
                {
                    Assert.NotNull(item.User);
                    Assert.Null(item.UserNull);
                    Assert.NotNull(item.Employee);
                    Assert.Null(item.EmployeeNull);
                });
            }
        }















        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridListAsync Is type")]
        public async System.Threading.Tasks.Task GridManagerGetGridListAsync_IsType()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = await manager.GetGridListAsync();
                Assert.IsType<System.Collections.Generic.List<TableModel>>(actualList);
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridListAsync Not null")]
        public async System.Threading.Tasks.Task GridManagerGetGridListAsync_NotNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = (await manager.GetGridListAsync()).ToList();
                Assert.NotNull(actualList);
                actualList.ForEach((item) => {
                    Assert.True(item.Id > 0);
                    Assert.NotEmpty(item.DisplayName);
                });
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridListAsync Count >= 0")]
        public async System.Threading.Tasks.Task GridManagerGetGridListAsync_PositiveCount()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = (await manager.GetGridListAsync()).ToList();
                Assert.True(actualList.Count >= 0);
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridListAsync The table is not has rows")]
        public async System.Threading.Tasks.Task GridManagerGetGridListAsync_TableisNotHasRows()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var actualList = (await manager.GetGridListAsync()).ToList();
                Assert.NotNull(actualList);
            }
        }
        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridListAsync is includes")]
        public async System.Threading.Tasks.Task GridManagerGetGridListAsync_IsIncludes()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {

                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = (await manager.GetGridListAsync()).ToList();

                var excpectedlist = context.Set<TableModel>()
                                        .Include(m => m.User)
                                        .Include(m => m.UserNull)
                                        .Include(m => m.Employee)
                                        .Include(m => m.EmployeeNull)
                                        .ToList();


                actualList.ForEach((item) =>
                {
                    Assert.NotNull(item.User);
                    Assert.Null(item.UserNull);
                    Assert.NotNull(item.Employee);
                    Assert.Null(item.EmployeeNull);
                });
            }
        }














        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridListViewModelAsync Is type")]
        public async System.Threading.Tasks.Task GridManagerGetGridListViewModelAsync_IsType()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = await manager.GetGridListViewModelAsync();
                Assert.IsType<System.Collections.Generic.List<ViewModel>>(actualList);
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridListViewModelAsync Not null")]
        public async System.Threading.Tasks.Task GridManagerGetGridListViewModelAsync_NotNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = (await manager.GetGridListViewModelAsync()).ToList();
                Assert.NotNull(actualList);
                actualList.ForEach((item) => {
                    Assert.True(item.Id > 0);
                    Assert.NotEmpty(item.DisplayName);
                });
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridListViewModelAsync Count >= 0")]
        public async System.Threading.Tasks.Task GridManagerGetGridListViewModelAsync_PositiveCount()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = (await manager.GetGridListViewModelAsync()).ToList();
                Assert.True(actualList.Count >= 0);
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridListViewModelAsync The table is not has rows")]
        public async System.Threading.Tasks.Task GridManagerGetGridListViewModelAsync_TableisNotHasRows()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var actualList = (await manager.GetGridListViewModelAsync()).ToList();
                Assert.NotNull(actualList);
            }
        }
        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridListViewModelAsync with fields name & includes is null")]
        public async System.Threading.Tasks.Task GridManagerGetGridListViewModelAsync_WithFieldsNameAndIncludesIsNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {

                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = (await manager.GetGridListViewModelAsync()).ToList();

                var excpectedlist = context.Set<TableModel>()
                                        .Include(m => m.User)
                                        .Include(m => m.UserNull)
                                        .Include(m => m.Employee)
                                        .Include(m => m.EmployeeNull)
                                        .ToList();


                actualList.ForEach((item) =>
                {
                    Assert.Null(item.User);
                    Assert.Null(item.UserNull);
                    Assert.Null(item.Employee);
                    Assert.Null(item.EmployeeNull);

                    Assert.NotEmpty(item.UserName);
                    Assert.NotEmpty(item.EmployeeName);
                    Assert.Matches(string.Empty, item.UserNullName);
                    Assert.Matches(string.Empty, item.EmployeeNullName);
                });
            }
        }














        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync Is type")]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_IsType()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualResponseModel = await manager.GetGridResponseModelAsync();
                Assert.IsType<Models.ResponseModel<ViewModel>>(actualResponseModel);
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync is null")]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_IsNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualResponseModel = await manager.GetGridResponseModelAsync(null);
                Assert.NotNull(actualResponseModel);
                Assert.Equal(6,actualResponseModel.TotalRowCount);
                Assert.True(actualResponseModel.List.Count == 0);
                Assert.IsType<System.Collections.Generic.List<ViewModel>>(actualResponseModel.List);
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync is empty")]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_IsEmpty()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualResponseModel = await manager.GetGridResponseModelAsync();
                Assert.NotNull(actualResponseModel);
                Assert.Equal(6, actualResponseModel.TotalRowCount);
                Assert.True(actualResponseModel.List.Count == 0);
                Assert.IsType<System.Collections.Generic.List<ViewModel>>(actualResponseModel.List);
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync ")]
        [InlineData(0, 0, null, null, null)]
        [InlineData(6, 6, null, null, null)]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_IsNull(int currentPage, int pageSize, int? keyId, System.Collections.Generic.List<string> orderNamesList, FindModel findModel)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var requestModel = new Models.RequestModel<FindModel> { CurrentPage = currentPage, PageSize = pageSize, KeyId = keyId, OrderNamesList = orderNamesList, FindModel = findModel};


                var actualResponseModel = await manager.GetGridResponseModelAsync(requestModel);
                Assert.NotNull(actualResponseModel);
                Assert.Equal(6, actualResponseModel.TotalRowCount);
                Assert.Equal(0, actualResponseModel.List.Count);
                Assert.IsType<System.Collections.Generic.List<ViewModel>>(actualResponseModel.List);
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync Count >= 0")]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_PositiveCount()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualResponseModel = await manager.GetGridResponseModelAsync();
                Assert.True(actualResponseModel.List.Count >= 0);
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync The table is not has rows")]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_TableisNotHasRows()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var actualResponseModel = await manager.GetGridResponseModelAsync();
                Assert.NotNull(actualResponseModel);
            }
        }
        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync with fields name & includes is null")]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_WithFieldsNameAndIncludesIsNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {

                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualResponseModel = await manager.GetGridResponseModelAsync();

                var excpectedlist = context.Set<TableModel>()
                                        .Include(m => m.User)
                                        .Include(m => m.UserNull)
                                        .Include(m => m.Employee)
                                        .Include(m => m.EmployeeNull)
                                        .ToList();


                foreach (var item in actualResponseModel.List)
                {
                    Assert.Null(item.User);
                    Assert.Null(item.UserNull);
                    Assert.Null(item.Employee);
                    Assert.Null(item.EmployeeNull);

                    Assert.NotEmpty(item.UserName);
                    Assert.NotEmpty(item.EmployeeName);
                    Assert.Matches(string.Empty, item.UserNullName);
                    Assert.Matches(string.Empty, item.EmployeeNullName);
                }
            }
        }
    }
}	
