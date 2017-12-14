using System;
using System.Linq;
using Grid.Tests.TestContext.Inits;
using Grid.Tests.TestContext.Models;
using Grid.Tests.Tools;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Grid.Tests.Tests.Managers
{
    public class GridManagerTest
    {
        [Fact(DisplayName = "Grid.Managers.GridManager.Constructor Run")]
        public void GridManagerConstructorTest01_Run()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                Assert.NotNull(manager);
            }
        }
        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridRowNewModel Is type")]
        public void GridManagerGetGridRowNewModelTest01_IsType()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var tableModel = manager.GetGridRowNewModel();
                Assert.IsType<TableModel>(tableModel);
            }
        }
        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridRowFindModel Is type")]
        public void GridManagerGetGridRowFindModelTest01_IsType()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);

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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Skip(number - 1).Take(1).FirstOrDefault();

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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Skip(number - 1).Take(1).FirstOrDefault();

                var tableModel = await manager.GetGridRowModelAsync(model.Id, true);
                Assert.NotEqual(0, tableModel.Id);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Skip(number - 1).Take(1).FirstOrDefault();

                var tableModel = await manager.GetGridRowCopyModelAsync(model.Id);
                Assert.Equal(0, tableModel.Id);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var lastindex = context.Set<TableModel>().Max(m => m.Id);
                var actualexception = await TestTools.ThrowsAsync<Exception>(async () => await manager.GetGridRowCopyModelAsync(lastindex + 1));
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var lastindex = context.Set<TableModel>().Max(m => m.Id);
                var actualexception = await TestTools.ThrowsAsync<Exception>(async () => await manager.GetGridRowViewModelAsync(lastindex + 1));
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

                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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

                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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

                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var actualexception = await TestTools.ThrowsAsync<Exception>(async () => await manager.SaveGridRowModelAsync(model));

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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var temp = context.Set<TableModel>().FirstOrDefault(m => m.Id == model.Id);

                var actualexception = await TestTools.ThrowsAsync<DbUpdateConcurrencyException>(async () => await manager.SaveGridRowModelAsync(model));

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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var actualexception = await TestTools.ThrowsAsync<Exception>(async () => await manager.SaveModelInContextAsync(model));

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

                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                model = await manager.SaveGridRowModelAsync(model);
                var copymodel = await manager.GetGridRowCopyModelAsync(model.Id);
                var actualmodel = await manager.SaveGridRowModelAsync(copymodel);

                Assert.True(actualmodel.Id > 0);
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

                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);

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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualList = manager.GetGridSelectList(randKeyId, term);
                if (term == null)
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
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);

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
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var randKeyId = TestTools.rInt(1024, -1024);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);

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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);

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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);

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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);

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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);

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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);

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
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
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

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync is type")]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_IsType()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualResponseModel = await manager.GetGridResponseModelAsync();
                Assert.IsType<Models.ResponseModel<ViewModel>>(actualResponseModel);
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync totalRowCount = 0")]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_TotalRowCount0()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var model = context.Set<TableModel>().Last();

                var actualResponseModel = await manager.GetGridResponseModelAsync();
                Assert.Equal(0, actualResponseModel.TotalRowCount);
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync run null")]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_RunNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var actualResponseModel = await manager.GetGridResponseModelAsync();
                Assert.NotNull(actualResponseModel);
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync the table is not has rows")]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_TableisNotHasRows()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var actualResponseModel = await manager.GetGridResponseModelAsync();
                Assert.NotNull(actualResponseModel);
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync run without orderby & findmodel")]
        [InlineData(0, 0, null)]
        [InlineData(0, 25, null)]
        [InlineData(1, 25, null)]
        [InlineData(1, 25, 256)]
        [InlineData(1, 25, -256)]
        [InlineData(512, 25, 512)]
        [InlineData(-512, 25, -512)]
        [InlineData(-512, 25, -512)]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_RunWithoutOrderbyAndFindmodel(int currentPage, int pageSize, int? keyId)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {

                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var requestModel = new Models.RequestModel<FindModel> { CurrentPage = currentPage, PageSize = pageSize, KeyId = keyId };
                var actualResponseModel = await manager.GetGridResponseModelAsync(requestModel);

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

        [Fact(DisplayName = "Grid.Managers.GridManager.QueryableFilter can be null")]
        public void GridManagerQueryableFilter_IsNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var excpectQueryable = context.Set<TableModel>().AsQueryable();
                var actualQueryable = manager.QueryableFilter(excpectQueryable, null);

                Assert.Equal(excpectQueryable, actualQueryable);
                Assert.Equal(excpectQueryable.ToList(), actualQueryable.ToList());
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.QueryableFilter filtered id")]
        [InlineData(0)]
        [InlineData(2)]
        [InlineData(256)]
        [InlineData(-512)]
        public void GridManagerQueryableFilter_filteredById(int Id)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(Id).CreateTestUsers(Id).CreateTableModels(Id);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var fm = new FindModel { Id = Id };

                var excpectQueryable = context.Set<TableModel>().AsQueryable();
                if (Id != default(int))
                {
                    excpectQueryable = excpectQueryable.Where(m => m.Id == Id);
                }
                var actualQueryable = manager.QueryableFilter(excpectQueryable, fm);

                Assert.Equal(excpectQueryable, actualQueryable);
                Assert.Equal(excpectQueryable.ToList(), actualQueryable.ToList());
            }
        }
        
        [Theory(DisplayName = "Grid.Managers.GridManager.QueryableFilter filtered CreatedBy")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("name")]
        [InlineData("Test name")]
        [InlineData("Test")]
        public void GridManagerQueryableFilter_filteredByCreatedBy(string CreatedBy)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var fm = new FindModel { CreatedBy = CreatedBy };
        
                var excpectQueryable = context.Set<TableModel>().AsQueryable();
                if (!string.IsNullOrEmpty(CreatedBy))
                {
                    excpectQueryable = excpectQueryable.Where(m => m.CreatedBy.Contains(CreatedBy));
                }
                var actualQueryable = manager.QueryableFilter(excpectQueryable, fm);
        
                Assert.Equal(excpectQueryable, actualQueryable);
                Assert.Equal(excpectQueryable.ToList(), actualQueryable.ToList());
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.QueryableFilter filtered LastUpdatedBy")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("name")]
        [InlineData("Test name")]
        [InlineData("Test")]
        public void GridManagerQueryableFilter_filteredByLastUpdatedBy(string LastUpdatedBy)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var fm = new FindModel { LastUpdatedBy = LastUpdatedBy };

                var excpectQueryable = context.Set<TableModel>().AsQueryable();
                if (!string.IsNullOrEmpty(LastUpdatedBy))
                {
                    excpectQueryable = excpectQueryable.Where(m => m.LastUpdatedBy.Contains(LastUpdatedBy));
                }
                var actualQueryable = manager.QueryableFilter(excpectQueryable, fm);

                Assert.Equal(excpectQueryable, actualQueryable);
                Assert.Equal(excpectQueryable.ToList(), actualQueryable.ToList());
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.QueryableFilter filtered CreatedDateStart is null")]
        public void GridManagerQueryableFilter_filteredByCreatedDateStartIsNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var CreatedDateStart = (DateTime?)null;
                var fm = new FindModel { CreatedDateStart = CreatedDateStart };

                var excpectQueryable = context.Set<TableModel>().AsQueryable();

                if (CreatedDateStart.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.CreatedDate >= CreatedDateStart);
                }
                var actualQueryable = manager.QueryableFilter(excpectQueryable, fm);

                Assert.Equal(excpectQueryable, actualQueryable);
                Assert.Equal(excpectQueryable.ToList(), actualQueryable.ToList());
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.QueryableFilter filtered CreatedDateStart")]
        [InlineData(2017, 11, 23)]
        public void GridManagerQueryableFilter_filteredByCreatedDateStart(int? year, int? mounth, int? day)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                DateTime? CreatedDateStart = new DateTime(year.Value, mounth.Value, day.Value);
                var fm = new FindModel { CreatedDateStart = CreatedDateStart };

                var excpectQueryable = context.Set<TableModel>().AsQueryable();

                if (CreatedDateStart.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.CreatedDate >= CreatedDateStart);
                }
                var actualQueryable = manager.QueryableFilter(excpectQueryable, fm);

                Assert.Equal(excpectQueryable, actualQueryable);
                Assert.Equal(excpectQueryable.ToList(), actualQueryable.ToList());
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.QueryableFilter filtered CreatedDateStart first row")]
        public void GridManagerQueryableFilter_filteredByCreatedDateStartFirstRow()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var CreatedDateStart = context.Set<TableModel>()?.FirstOrDefault()?.CreatedDate;
                var fm = new FindModel { CreatedDateStart = CreatedDateStart };

                var excpectQueryable = context.Set<TableModel>().AsQueryable();

                if (CreatedDateStart.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.CreatedDate >= CreatedDateStart);
                }
                var actualQueryable = manager.QueryableFilter(excpectQueryable, fm);

                Assert.Equal(excpectQueryable, actualQueryable);
                Assert.Equal(excpectQueryable.ToList(), actualQueryable.ToList());
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.QueryableFilter filtered LastUpdatedDateStart is null")]
        public void GridManagerQueryableFilter_filteredByLastUpdatedDateStartIsNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var LastUpdatedDateStart = (DateTime?)null;
                var fm = new FindModel { LastUpdatedDateStart = LastUpdatedDateStart };

                var excpectQueryable = context.Set<TableModel>().AsQueryable();

                if (LastUpdatedDateStart.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.LastUpdatedDate >= LastUpdatedDateStart);
                }
                var actualQueryable = manager.QueryableFilter(excpectQueryable, fm);

                Assert.Equal(excpectQueryable, actualQueryable);
                Assert.Equal(excpectQueryable.ToList(), actualQueryable.ToList());
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.QueryableFilter filtered LastUpdatedDateStart")]
        [InlineData(2017, 11, 23)]
        public void GridManagerQueryableFilter_filteredByLastUpdatedDateStart(int? year, int? mounth, int? day)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                DateTime? LastUpdatedDateStart = new DateTime(year.Value, mounth.Value, day.Value);
                var fm = new FindModel { LastUpdatedDateStart = LastUpdatedDateStart };

                var excpectQueryable = context.Set<TableModel>().AsQueryable();

                if (LastUpdatedDateStart.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.LastUpdatedDate >= LastUpdatedDateStart);
                }
                var actualQueryable = manager.QueryableFilter(excpectQueryable, fm);

                Assert.Equal(excpectQueryable, actualQueryable);
                Assert.Equal(excpectQueryable.ToList(), actualQueryable.ToList());
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.QueryableFilter filtered LastUpdatedDateStart first row")]
        public void GridManagerQueryableFilter_filteredByLastUpdatedDateStartFirstRow()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var LastUpdatedDateStart = context.Set<TableModel>()?.FirstOrDefault()?.LastUpdatedDate;
                var fm = new FindModel { LastUpdatedDateStart = LastUpdatedDateStart };

                var excpectQueryable = context.Set<TableModel>().AsQueryable();

                if (LastUpdatedDateStart.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.LastUpdatedDate >= LastUpdatedDateStart);
                }
                var actualQueryable = manager.QueryableFilter(excpectQueryable, fm);

                Assert.Equal(excpectQueryable, actualQueryable);
                Assert.Equal(excpectQueryable.ToList(), actualQueryable.ToList());
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.QueryableFilter filtered CreatedDateEnd is null")]
        public void GridManagerQueryableFilter_filteredByCreatedDateEndIsNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var CreatedDateEnd = (DateTime?)null;
                var fm = new FindModel { CreatedDateEnd = CreatedDateEnd };

                var excpectQueryable = context.Set<TableModel>().AsQueryable();

                if (CreatedDateEnd.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.CreatedDate <= CreatedDateEnd);
                }
                var actualQueryable = manager.QueryableFilter(excpectQueryable, fm);

                Assert.Equal(excpectQueryable, actualQueryable);
                Assert.Equal(excpectQueryable.ToList(), actualQueryable.ToList());
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.QueryableFilter filtered CreatedDateEnd")]
        [InlineData(2017, 11, 23)]
        public void GridManagerQueryableFilter_filteredByCreatedDateEnd(int? year, int? mounth, int? day)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                DateTime? CreatedDateEnd = new DateTime(year.Value, mounth.Value, day.Value);
                var fm = new FindModel { CreatedDateEnd = CreatedDateEnd };

                var excpectQueryable = context.Set<TableModel>().AsQueryable();

                if (CreatedDateEnd.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.CreatedDate <= CreatedDateEnd);
                }
                var actualQueryable = manager.QueryableFilter(excpectQueryable, fm);

                Assert.Equal(excpectQueryable, actualQueryable);
                Assert.Equal(excpectQueryable.ToList(), actualQueryable.ToList());
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.QueryableFilter filtered CreatedDateEnd first row")]
        public void GridManagerQueryableFilter_filteredByCreatedDateEndFirstRow()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var CreatedDateEnd = context.Set<TableModel>()?.FirstOrDefault()?.CreatedDate;
                var fm = new FindModel { CreatedDateEnd = CreatedDateEnd };

                var excpectQueryable = context.Set<TableModel>().AsQueryable();

                if (CreatedDateEnd.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.CreatedDate <= CreatedDateEnd);
                }
                var actualQueryable = manager.QueryableFilter(excpectQueryable, fm);

                Assert.Equal(excpectQueryable, actualQueryable);
                Assert.Equal(excpectQueryable.ToList(), actualQueryable.ToList());
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.QueryableFilter filtered LastUpdatedDateEnd is null")]
        public void GridManagerQueryableFilter_filteredByLastUpdatedDateEndIsNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var LastUpdatedDateEnd = (DateTime?)null;
                var fm = new FindModel { LastUpdatedDateEnd = LastUpdatedDateEnd };

                var excpectQueryable = context.Set<TableModel>().AsQueryable();

                if (LastUpdatedDateEnd.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.LastUpdatedDate <= LastUpdatedDateEnd);
                }
                var actualQueryable = manager.QueryableFilter(excpectQueryable, fm);

                Assert.Equal(excpectQueryable, actualQueryable);
                Assert.Equal(excpectQueryable.ToList(), actualQueryable.ToList());
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.QueryableFilter filtered LastUpdatedDateEnd")]
        [InlineData(2017, 11, 23)]
        public void GridManagerQueryableFilter_filteredByLastUpdatedDateEnd(int? year, int? mounth, int? day)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                DateTime? LastUpdatedDateEnd = new DateTime(year.Value, mounth.Value, day.Value);
                var fm = new FindModel { LastUpdatedDateEnd = LastUpdatedDateEnd };

                var excpectQueryable = context.Set<TableModel>().AsQueryable();

                if (LastUpdatedDateEnd.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.LastUpdatedDate <= LastUpdatedDateEnd);
                }
                var actualQueryable = manager.QueryableFilter(excpectQueryable, fm);

                Assert.Equal(excpectQueryable, actualQueryable);
                Assert.Equal(excpectQueryable.ToList(), actualQueryable.ToList());
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.QueryableFilter filtered LastUpdatedDateEnd first row")]
        public void GridManagerQueryableFilter_filteredByLastUpdatedDateEndFirstRow()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var LastUpdatedDateEnd = context.Set<TableModel>()?.FirstOrDefault()?.LastUpdatedDate;
                var fm = new FindModel { LastUpdatedDateEnd = LastUpdatedDateEnd };

                var excpectQueryable = context.Set<TableModel>().AsQueryable();

                if (LastUpdatedDateEnd.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.LastUpdatedDate <= LastUpdatedDateEnd);
                }
                var actualQueryable = manager.QueryableFilter(excpectQueryable, fm);

                Assert.Equal(excpectQueryable, actualQueryable);
                Assert.Equal(excpectQueryable.ToList(), actualQueryable.ToList());
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridList can be null")]
        public void GridManagerGetGridList_IsNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();
                var actualQueryable = manager.GetGridList(null, null);
                
                Assert.Equal(excpectQueryable.Count(), actualQueryable.Count());
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridList filtered id")]
        [InlineData(0)]
        [InlineData(2)]
        [InlineData(256)]
        [InlineData(-512)]
        public void GridManagerGetGridList_filteredById(int Id)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(Id).CreateTestUsers(Id).CreateTableModels(Id);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var fm = new FindModel { Id = Id };

                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();
                if (Id != default(int))
                {
                    excpectQueryable = excpectQueryable.Where(m => m.Id == Id);
                }
                var actualQueryable = manager.GetGridList(null, fm);
                
                Assert.Equal(excpectQueryable.Count(), actualQueryable.Count());
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridList filtered CreatedBy")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("name")]
        [InlineData("Test name")]
        [InlineData("Test")]
        public void GridManagerGetGridList_filteredByCreatedBy(string CreatedBy)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var fm = new FindModel { CreatedBy = CreatedBy };

                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();
                if (!string.IsNullOrEmpty(CreatedBy))
                {
                    excpectQueryable = excpectQueryable.Where(m => m.CreatedBy.Contains(CreatedBy));
                }
                var actualQueryable = manager.GetGridList(null, fm);

                
                Assert.Equal(excpectQueryable.Count(), actualQueryable.Count());
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridList filtered LastUpdatedBy")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("name")]
        [InlineData("Test name")]
        [InlineData("Test")]
        public void GridManagerGetGridList_filteredByLastUpdatedBy(string LastUpdatedBy)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var fm = new FindModel { LastUpdatedBy = LastUpdatedBy };

                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();
                if (!string.IsNullOrEmpty(LastUpdatedBy))
                {
                    excpectQueryable = excpectQueryable.Where(m => m.LastUpdatedBy.Contains(LastUpdatedBy));
                }
                var actualQueryable = manager.GetGridList(null, fm);

                
                Assert.Equal(excpectQueryable.Count(), actualQueryable.Count());
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridList filtered CreatedDateStart is null")]
        public void GridManagerGetGridList_filteredByCreatedDateStartIsNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var CreatedDateStart = (DateTime?)null;
                var fm = new FindModel { CreatedDateStart = CreatedDateStart };

                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();

                if (CreatedDateStart.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.CreatedDate >= CreatedDateStart);
                }
                var actualQueryable = manager.GetGridList(null, fm);

                
                Assert.Equal(excpectQueryable.Count(), actualQueryable.Count());
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridList filtered CreatedDateStart")]
        [InlineData(2017, 11, 23)]
        public void GridManagerGetGridList_filteredByCreatedDateStart(int? year, int? mounth, int? day)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                DateTime? CreatedDateStart = new DateTime(year.Value, mounth.Value, day.Value);
                var fm = new FindModel { CreatedDateStart = CreatedDateStart };

                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();

                if (CreatedDateStart.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.CreatedDate >= CreatedDateStart);
                }
                var actualQueryable = manager.GetGridList(null, fm);

                
                Assert.Equal(excpectQueryable.Count(), actualQueryable.Count());
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridList filtered CreatedDateStart first row")]
        public void GridManagerGetGridList_filteredByCreatedDateStartFirstRow()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var CreatedDateStart = context.Set<TableModel>()?.FirstOrDefault()?.CreatedDate;
                var fm = new FindModel { CreatedDateStart = CreatedDateStart };

                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();

                if (CreatedDateStart.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.CreatedDate >= CreatedDateStart);
                }
                var actualQueryable = manager.GetGridList(null, fm);

                
                Assert.Equal(excpectQueryable.Count(), actualQueryable.Count());
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridList filtered LastUpdatedDateStart is null")]
        public void GridManagerGetGridList_filteredByLastUpdatedDateStartIsNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var LastUpdatedDateStart = (DateTime?)null;
                var fm = new FindModel { LastUpdatedDateStart = LastUpdatedDateStart };

                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();

                if (LastUpdatedDateStart.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.LastUpdatedDate >= LastUpdatedDateStart);
                }
                var actualQueryable = manager.GetGridList(null, fm);

                
                Assert.Equal(excpectQueryable.Count(), actualQueryable.Count());
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridList filtered LastUpdatedDateStart")]
        [InlineData(2017, 11, 23)]
        public void GridManagerGetGridList_filteredByLastUpdatedDateStart(int? year, int? mounth, int? day)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                DateTime? LastUpdatedDateStart = new DateTime(year.Value, mounth.Value, day.Value);
                var fm = new FindModel { LastUpdatedDateStart = LastUpdatedDateStart };

                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();

                if (LastUpdatedDateStart.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.LastUpdatedDate >= LastUpdatedDateStart);
                }
                var actualQueryable = manager.GetGridList(null, fm);

                
                Assert.Equal(excpectQueryable.Count(), actualQueryable.Count());
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridList filtered LastUpdatedDateStart first row")]
        public void GridManagerGetGridList_filteredByLastUpdatedDateStartFirstRow()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var LastUpdatedDateStart = context.Set<TableModel>()?.FirstOrDefault()?.LastUpdatedDate;
                var fm = new FindModel { LastUpdatedDateStart = LastUpdatedDateStart };

                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();

                if (LastUpdatedDateStart.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.LastUpdatedDate >= LastUpdatedDateStart);
                }
                var actualQueryable = manager.GetGridList(null, fm);

                
                Assert.Equal(excpectQueryable.Count(), actualQueryable.Count());
            }
        }
        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridList filtered CreatedDateEnd is null")]
        public void GridManagerGetGridList_filteredByCreatedDateEndIsNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var CreatedDateEnd = (DateTime?)null;
                var fm = new FindModel { CreatedDateEnd = CreatedDateEnd };

                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();

                if (CreatedDateEnd.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.CreatedDate <= CreatedDateEnd);
                }
                var actualQueryable = manager.GetGridList(null, fm);

                
                Assert.Equal(excpectQueryable.Count(), actualQueryable.Count());
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridList filtered CreatedDateEnd")]
        [InlineData(2017, 11, 23)]
        public void GridManagerGetGridList_filteredByCreatedDateEnd(int? year, int? mounth, int? day)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                DateTime? CreatedDateEnd = new DateTime(year.Value, mounth.Value, day.Value);
                var fm = new FindModel { CreatedDateEnd = CreatedDateEnd };

                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();

                if (CreatedDateEnd.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.CreatedDate <= CreatedDateEnd);
                }
                var actualQueryable = manager.GetGridList(null, fm);

                
                Assert.Equal(excpectQueryable.Count(), actualQueryable.Count());
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridList filtered CreatedDateEnd first row")]
        public void GridManagerGetGridList_filteredByCreatedDateEndFirstRow()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var CreatedDateEnd = context.Set<TableModel>()?.FirstOrDefault()?.CreatedDate;
                var fm = new FindModel { CreatedDateEnd = CreatedDateEnd };

                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();

                if (CreatedDateEnd.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.CreatedDate <= CreatedDateEnd);
                }
                var actualQueryable = manager.GetGridList(null, fm);

                
                Assert.Equal(excpectQueryable.Count(), actualQueryable.Count());
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridList filtered LastUpdatedDateEnd is null")]
        public void GridManagerGetGridList_filteredByLastUpdatedDateEndIsNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var LastUpdatedDateEnd = (DateTime?)null;
                var fm = new FindModel { LastUpdatedDateEnd = LastUpdatedDateEnd };

                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();

                if (LastUpdatedDateEnd.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.LastUpdatedDate <= LastUpdatedDateEnd);
                }
                var actualQueryable = manager.GetGridList(null, fm);

                
                Assert.Equal(excpectQueryable.Count(), actualQueryable.Count());
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridList filtered LastUpdatedDateEnd")]
        [InlineData(2017, 11, 23)]
        public void GridManagerGetGridList_filteredByLastUpdatedDateEnd(int? year, int? mounth, int? day)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                DateTime? LastUpdatedDateEnd = new DateTime(year.Value, mounth.Value, day.Value);
                var fm = new FindModel { LastUpdatedDateEnd = LastUpdatedDateEnd };

                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();

                if (LastUpdatedDateEnd.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.LastUpdatedDate <= LastUpdatedDateEnd);
                }
                var actualQueryable = manager.GetGridList(null, fm);

                
                Assert.Equal(excpectQueryable.Count(), actualQueryable.Count());
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridList filtered LastUpdatedDateEnd first row")]
        public void GridManagerGetGridList_filteredByLastUpdatedDateEndFirstRow()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var LastUpdatedDateEnd = context.Set<TableModel>()?.FirstOrDefault()?.LastUpdatedDate;
                var fm = new FindModel { LastUpdatedDateEnd = LastUpdatedDateEnd };

                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();

                if (LastUpdatedDateEnd.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.LastUpdatedDate <= LastUpdatedDateEnd);
                }
                var actualQueryable = manager.GetGridList(null, fm);

                
                Assert.Equal(excpectQueryable.Count(), actualQueryable.Count());
            }
        }

        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync list filter can be null")]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_IsNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);

                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();
                var actualQueryable = await manager.GetGridResponseModelAsync(null);

                Assert.Null(actualQueryable.List);
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync list filtered id")]
        [InlineData(0)]
        [InlineData(2)]
        [InlineData(256)]
        [InlineData(-512)]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_filteredById(int Id)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(Id).CreateTestUsers(Id).CreateTableModels(Id);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var fm = new FindModel { Id = Id };
        
                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();
                if (Id != default(int))
                {
                    excpectQueryable = excpectQueryable.Where(m => m.Id == Id);
                }
                var requestModel = new Models.RequestModel<FindModel>
                {
                    CurrentPage = 0,
                    PageSize = 25,
                    FindModel = fm
                };
                var actualQueryable = await manager.GetGridResponseModelAsync(requestModel);
                var excpectedCount = excpectQueryable.Skip(requestModel.PageSize * requestModel.CurrentPage).Take(requestModel.PageSize).Count();
                Assert.Equal(excpectedCount, actualQueryable.List.Count());
            }
        }
        
        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync list filtered CreatedBy")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("name")]
        [InlineData("Test name")]
        [InlineData("Test")]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_filteredByCreatedBy(string CreatedBy)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var fm = new FindModel { CreatedBy = CreatedBy };
        
                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();
                if (!string.IsNullOrEmpty(CreatedBy))
                {
                    excpectQueryable = excpectQueryable.Where(m => m.CreatedBy.Contains(CreatedBy));
                }
                var requestModel = new Models.RequestModel<FindModel>
                {
                    CurrentPage = 0,
                    PageSize = 25,
                    FindModel = fm
                };
                var actualQueryable = await manager.GetGridResponseModelAsync(requestModel);
                var excpectedCount = excpectQueryable.Skip(requestModel.PageSize * requestModel.CurrentPage).Take(requestModel.PageSize).Count();
                Assert.Equal(excpectedCount, actualQueryable.List.Count());
            }
        }
        
        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync list filtered LastUpdatedBy")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("name")]
        [InlineData("Test name")]
        [InlineData("Test")]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_filteredByLastUpdatedBy(string LastUpdatedBy)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var fm = new FindModel { LastUpdatedBy = LastUpdatedBy };
        
                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();
                if (!string.IsNullOrEmpty(LastUpdatedBy))
                {
                    excpectQueryable = excpectQueryable.Where(m => m.LastUpdatedBy.Contains(LastUpdatedBy));
                }
                var requestModel = new Models.RequestModel<FindModel>
                {
                    CurrentPage = 0,
                    PageSize = 25,
                    FindModel = fm
                };
                var actualQueryable = await manager.GetGridResponseModelAsync(requestModel);
                var excpectedCount = excpectQueryable.Skip(requestModel.PageSize * requestModel.CurrentPage).Take(requestModel.PageSize).Count();
                Assert.Equal(excpectedCount, actualQueryable.List.Count());
            }
        }
        
        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync list filtered CreatedDateStart is null")]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_filteredByCreatedDateStartIsNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var CreatedDateStart = (DateTime?)null;
                var fm = new FindModel { CreatedDateStart = CreatedDateStart };
        
                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();
        
                if (CreatedDateStart.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.CreatedDate >= CreatedDateStart);
                }
                var requestModel = new Models.RequestModel<FindModel>
                {
                    CurrentPage = 0,
                    PageSize = 25,
                    FindModel = fm
                };
                var actualQueryable = await manager.GetGridResponseModelAsync(requestModel);
                var excpectedCount = excpectQueryable.Skip(requestModel.PageSize * requestModel.CurrentPage).Take(requestModel.PageSize).Count();
                Assert.Equal(excpectedCount, actualQueryable.List.Count());
            }
        }
        
        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync list filtered CreatedDateStart")]
        [InlineData(2017, 11, 23)]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_filteredByCreatedDateStart(int? year, int? mounth, int? day)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                DateTime? CreatedDateStart = new DateTime(year.Value, mounth.Value, day.Value);
                var fm = new FindModel { CreatedDateStart = CreatedDateStart };
        
                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();
        
                if (CreatedDateStart.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.CreatedDate >= CreatedDateStart);
                }
                var requestModel = new Models.RequestModel<FindModel>
                {
                    CurrentPage = 0,
                    PageSize = 25,
                    FindModel = fm
                };
                var actualQueryable = await manager.GetGridResponseModelAsync(requestModel);
                var excpectedCount = excpectQueryable.Skip(requestModel.PageSize * requestModel.CurrentPage).Take(requestModel.PageSize).Count();
                Assert.Equal(excpectedCount, actualQueryable.List.Count());
            }
        }
        
        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync list filtered CreatedDateStart first row")]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_filteredByCreatedDateStartFirstRow()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var CreatedDateStart = context.Set<TableModel>()?.FirstOrDefault()?.CreatedDate;
                var fm = new FindModel { CreatedDateStart = CreatedDateStart };
        
                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();
        
                if (CreatedDateStart.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.CreatedDate >= CreatedDateStart);
                }
                var requestModel = new Models.RequestModel<FindModel>
                {
                    CurrentPage = 0,
                    PageSize = 25,
                    FindModel = fm
                };
                var actualQueryable = await manager.GetGridResponseModelAsync(requestModel);
                var excpectedCount = excpectQueryable.Skip(requestModel.PageSize * requestModel.CurrentPage).Take(requestModel.PageSize).Count();
                Assert.Equal(excpectedCount, actualQueryable.List.Count());
            }
        }
        
        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync list filtered LastUpdatedDateStart is null")]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_filteredByLastUpdatedDateStartIsNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var LastUpdatedDateStart = (DateTime?)null;
                var fm = new FindModel { LastUpdatedDateStart = LastUpdatedDateStart };
        
                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();
        
                if (LastUpdatedDateStart.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.LastUpdatedDate >= LastUpdatedDateStart);
                }
                var requestModel = new Models.RequestModel<FindModel>
                {
                    CurrentPage = 0,
                    PageSize = 25,
                    FindModel = fm
                };
                var actualQueryable = await manager.GetGridResponseModelAsync(requestModel);
                var excpectedCount = excpectQueryable.Skip(requestModel.PageSize * requestModel.CurrentPage).Take(requestModel.PageSize).Count();
                Assert.Equal(excpectedCount, actualQueryable.List.Count());
            }
        }
        
        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync list filtered LastUpdatedDateStart")]
        [InlineData(2017, 11, 23)]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_filteredByLastUpdatedDateStart(int? year, int? mounth, int? day)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                DateTime? LastUpdatedDateStart = new DateTime(year.Value, mounth.Value, day.Value);
                var fm = new FindModel { LastUpdatedDateStart = LastUpdatedDateStart };
        
                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();
        
                if (LastUpdatedDateStart.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.LastUpdatedDate >= LastUpdatedDateStart);
                }
                var requestModel = new Models.RequestModel<FindModel>
                {
                    CurrentPage = 0,
                    PageSize = 25,
                    FindModel = fm
                };
                var actualQueryable = await manager.GetGridResponseModelAsync(requestModel);
                var excpectedCount = excpectQueryable.Skip(requestModel.PageSize * requestModel.CurrentPage).Take(requestModel.PageSize).Count();
                Assert.Equal(excpectedCount, actualQueryable.List.Count());
            }
        }
        
        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync list filtered LastUpdatedDateStart first row")]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_filteredByLastUpdatedDateStartFirstRow()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var LastUpdatedDateStart = context.Set<TableModel>()?.FirstOrDefault()?.LastUpdatedDate;
                var fm = new FindModel { LastUpdatedDateStart = LastUpdatedDateStart };
        
                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();
        
                if (LastUpdatedDateStart.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.LastUpdatedDate >= LastUpdatedDateStart);
                }
                var requestModel = new Models.RequestModel<FindModel>
                {
                    CurrentPage = 0,
                    PageSize = 25,
                    FindModel = fm
                };
                var actualQueryable = await manager.GetGridResponseModelAsync(requestModel);
                var excpectedCount = excpectQueryable.Skip(requestModel.PageSize * requestModel.CurrentPage).Take(requestModel.PageSize).Count();
                Assert.Equal(excpectedCount, actualQueryable.List.Count());
            }
        }
        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync list filtered CreatedDateEnd is null")]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_filteredByCreatedDateEndIsNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var CreatedDateEnd = (DateTime?)null;
                var fm = new FindModel { CreatedDateEnd = CreatedDateEnd };
        
                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();
        
                if (CreatedDateEnd.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.CreatedDate <= CreatedDateEnd);
                }
                var requestModel = new Models.RequestModel<FindModel>
                {
                    CurrentPage = 0,
                    PageSize = 25,
                    FindModel = fm
                };
                var actualQueryable = await manager.GetGridResponseModelAsync(requestModel);
                var excpectedCount = excpectQueryable.Skip(requestModel.PageSize * requestModel.CurrentPage).Take(requestModel.PageSize).Count();
                Assert.Equal(excpectedCount, actualQueryable.List.Count());
            }
        }
        
        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync list filtered CreatedDateEnd")]
        [InlineData(2017, 11, 23)]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_filteredByCreatedDateEnd(int? year, int? mounth, int? day)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                DateTime? CreatedDateEnd = new DateTime(year.Value, mounth.Value, day.Value);
                var fm = new FindModel { CreatedDateEnd = CreatedDateEnd };
        
                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();
        
                if (CreatedDateEnd.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.CreatedDate <= CreatedDateEnd);
                }
                var requestModel = new Models.RequestModel<FindModel>
                {
                    CurrentPage = 0,
                    PageSize = 25,
                    FindModel = fm
                };
                var actualQueryable = await manager.GetGridResponseModelAsync(requestModel);
                var excpectedCount = excpectQueryable.Skip(requestModel.PageSize * requestModel.CurrentPage).Take(requestModel.PageSize).Count();
                Assert.Equal(excpectedCount, actualQueryable.List.Count());
            }
        }
        
        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync list filtered CreatedDateEnd first row")]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_filteredByCreatedDateEndFirstRow()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var CreatedDateEnd = context.Set<TableModel>()?.FirstOrDefault()?.CreatedDate;
                var fm = new FindModel { CreatedDateEnd = CreatedDateEnd };
        
                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();
        
                if (CreatedDateEnd.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.CreatedDate <= CreatedDateEnd);
                }
                var requestModel = new Models.RequestModel<FindModel>
                {
                    CurrentPage = 0,
                    PageSize = 25,
                    FindModel = fm
                };
                var actualQueryable = await manager.GetGridResponseModelAsync(requestModel);
                var excpectedCount = excpectQueryable.Skip(requestModel.PageSize * requestModel.CurrentPage).Take(requestModel.PageSize).Count();
                Assert.Equal(excpectedCount, actualQueryable.List.Count());
            }
        }
        
        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync list filtered LastUpdatedDateEnd is null")]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_filteredByLastUpdatedDateEndIsNull()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var LastUpdatedDateEnd = (DateTime?)null;
                var fm = new FindModel { LastUpdatedDateEnd = LastUpdatedDateEnd };
        
                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();
        
                if (LastUpdatedDateEnd.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.LastUpdatedDate <= LastUpdatedDateEnd);
                }
                var requestModel = new Models.RequestModel<FindModel>
                {
                    CurrentPage = 0,
                    PageSize = 25,
                    FindModel = fm
                };
                var actualQueryable = await manager.GetGridResponseModelAsync(requestModel);
                var excpectedCount = excpectQueryable.Skip(requestModel.PageSize * requestModel.CurrentPage).Take(requestModel.PageSize).Count();
                Assert.Equal(excpectedCount, actualQueryable.List.Count());
            }
        }
        
        [Theory(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync list filtered LastUpdatedDateEnd")]
        [InlineData(2017, 11, 23)]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_filteredByLastUpdatedDateEnd(int? year, int? mounth, int? day)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                DateTime? LastUpdatedDateEnd = new DateTime(year.Value, mounth.Value, day.Value);
                var fm = new FindModel { LastUpdatedDateEnd = LastUpdatedDateEnd };
        
                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();
        
                if (LastUpdatedDateEnd.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.LastUpdatedDate <= LastUpdatedDateEnd);
                }
                var requestModel = new Models.RequestModel<FindModel>
                {
                    CurrentPage = 0,
                    PageSize = 25,
                    FindModel = fm
                };
                var actualQueryable = await manager.GetGridResponseModelAsync(requestModel);
                var excpectedCount = excpectQueryable.Skip(requestModel.PageSize * requestModel.CurrentPage).Take(requestModel.PageSize).Count();
                Assert.Equal(excpectedCount, actualQueryable.List.Count());
            }
        }
        
        [Fact(DisplayName = "Grid.Managers.GridManager.GetGridResponseModelAsync list filtered LastUpdatedDateEnd first row")]
        public async System.Threading.Tasks.Task GridManagerGetGridResponseModelAsync_filteredByLastUpdatedDateEndFirstRow()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var LastUpdatedDateEnd = context.Set<TableModel>()?.FirstOrDefault()?.LastUpdatedDate;
                var fm = new FindModel { LastUpdatedDateEnd = LastUpdatedDateEnd };
        
                var excpectQueryable = manager.GetGridListWithOneLevelIncludes();
        
                if (LastUpdatedDateEnd.HasValue)
                {
                    excpectQueryable = excpectQueryable.Where(m => m.LastUpdatedDate <= LastUpdatedDateEnd);
                }
                var requestModel = new Models.RequestModel<FindModel>
                {
                    CurrentPage = 0,
                    PageSize = 25,
                    FindModel = fm
                };
                var actualQueryable = await manager.GetGridResponseModelAsync(requestModel);
                var excpectedCount = excpectQueryable.Skip(requestModel.PageSize * requestModel.CurrentPage).Take(requestModel.PageSize).Count();
                Assert.Equal(excpectedCount, actualQueryable.List.Count());
            }
        }

        [Theory(DisplayName = "Grid.Managers.GridManager.SaveGridListAsync Is type")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerSaveGridListAsync_IsType(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number).CreateTableModels(number);
                var listmodels = context.Set<TableModel>().ToList();
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var tableModel = await manager.SaveGridListAsync(listmodels);

                Assert.IsType<System.Collections.Generic.List<ViewModel>>(tableModel);
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.SaveGridListAsync id > 0")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerSaveGridListAsync_IdNot0(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number).CreateTableModels(number);
                var listmodels = context.Set<TableModel>().ToList();
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var tableModel = await manager.SaveGridListAsync(listmodels);
                listmodels.ForEach((item) => {
                    Assert.NotEqual(0, item.Id);
                });
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.SaveGridListAsync Update Model")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerSaveGridListAsync_updatemodel(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number).CreateTableModels(number);
                var listmodels = context.Set<TableModel>().ToList();
                listmodels.ForEach((item) => {
                    item.Test = "test";
                });
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var actual = await manager.SaveGridListAsync(listmodels);
                actual.ToList().ForEach((item) => {
                    item.Test = "test";
                    Assert.Equal("test", item.Test);
                });
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.SaveGridListAsync Exception Id < 0")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerSaveGridListAsync_ExceptionNegativeId(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number).CreateTableModels(number);
                var listmodels = context.Set<TableModel>().ToList();
                listmodels.ForEach((item) => {
                    item.Id = item.Id * (-1);
                });
                
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
                var actualexception = await TestTools.ThrowsAsync<Exception>(async () => await manager.SaveGridListAsync(listmodels));
        
                Assert.NotNull(actualexception.Message);
                Assert.Matches(@"Данные не сохранены! Ошибка сохранения записи ([^\d])", actualexception.Message);
            }
        }
        [Theory(DisplayName = "Grid.Managers.GridManager.SaveGridListAsync DbUpdateConcurrencyException Id > 0")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerSaveGridListAsync_DbUpdateConcurrencyExceptionPositiveId(int number)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(number).CreateTestUsers(number);
        
                var model = InitTableModel.DbLoad(number, 5);
                model.Id = number;
                var listmodels = context.Set<TableModel>().ToList();
                listmodels.Add(model);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
        
                var actualexception = await TestTools.ThrowsAsync<DbUpdateConcurrencyException>(async () => await manager.SaveGridListAsync(listmodels));
                Assert.NotNull(actualexception.Message);
            }
        }
        
        [Theory(DisplayName = "Grid.Managers.GridManager.SaveGridListAsync Save New ModelsList")]
        [InlineData(2)]
        [InlineData(64)]
        [InlineData(256)]
        public async System.Threading.Tasks.Task GridManagerSaveGridListAsync_SaveNewModelsList(int count)
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(1).CreateTestUsers(1);
                var manager = new Grid.Managers.GridManager<TableModel, ViewModel, FindModel>(context);
        
                var listmodels = new System.Collections.Generic.List<TableModel>();
                var userId = context.Set<User>().FirstOrDefault().Id;
                var employeeId = context.Set<Employee>().FirstOrDefault().Id;
                for (var i=0; i<count; i++)
                {
                    var newmodel = manager.GetGridRowNewModel();
                    newmodel.UserId = userId;
                    newmodel.EmployeeId = employeeId;
                    newmodel.Test = "test";
                    newmodel.IsBool = true;
                    newmodel.Decimal = 1.01m;
                    listmodels.Add(newmodel);
                }
        
                var actual = await manager.SaveGridListAsync(listmodels);

                Assert.IsType<System.Collections.Generic.List<ViewModel>>(actual);
                Assert.Equal(count, actual.Count());
                actual.ToList().ForEach((item) => {
                    Assert.True(item.Id > 0);
                    Assert.Equal("test", item.Test);
                    Assert.Equal(userId, item.UserId);
                    Assert.Equal(employeeId, item.EmployeeId);
                    Assert.Equal(true, item.IsBool);
                    Assert.Equal(1.01m, item.Decimal);
                });
            }
        }

    }
}
