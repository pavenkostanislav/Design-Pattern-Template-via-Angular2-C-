using Xunit;
using Grid.Tools;
using System.Linq;

using Grid.Test.TestContext.Models;
using Grid.Test.TestContext.Inits;
using Microsoft.EntityFrameworkCore;
using Grid.Test.Tools;

namespace Grid.Test.Tests.Tools
{
    public class GridToolsTest
    {
        [Fact(DisplayName = "Grid.Tools.GridTools.ToUpperFirstIndexStringBuilder Run")]
        public void ToUpperFirstIndexStringBuilderTest01_RunEqualStatusIdd()
        {
            var str1 = "statusId";
            var str2 = "StatusId";

            Assert.Equal(str1.ToUpperFirstIndexStringBuilder(), str2);
        }
        [Fact(DisplayName = "Grid.Tools.GridTools.ToUpperFirstIndexStringBuilder Is type")]
        public void ToUpperFirstIndexStringBuilderTest02_IsType()
        {
            var str1 = "statusId";
            var actual = str1.ToUpperFirstIndexStringBuilder();
            Assert.IsType<string>(actual);
        }
        [Fact(DisplayName = "Grid.Tools.GridTools.ToUpperFirstIndexStringBuilder Performance 01")]
        public void ToUpperFirstIndexStringBuilder_PerformanceTest01()
        {
            var expectedstr = "statusId";
            var expectedwatch = System.Diagnostics.Stopwatch.StartNew();
            var _chararray = expectedstr.ToCharArray();
            _chararray[0] = System.Convert.ToChar(System.Convert.ToInt32(expectedstr[0]) - 32);
            var expected = new string(_chararray);
            expectedwatch.Stop();

            var actualstr = "statusId";
            var actualwatch = System.Diagnostics.Stopwatch.StartNew();
            var _stringBuilder = new System.Text.StringBuilder(actualstr);
            int x = actualstr[0];//получаем код ASCI
            int X = x - 32;//Разница между маленькой буквой и большой 32 символа.У заглавных букв код ASCI меньше
            _stringBuilder.Replace((char)x, (char)X, 0, 1);//casting на int преобразует его в символ по таблице ASCI
            var actual = _stringBuilder.ToString();
            actualwatch.Stop();
            
            Assert.Equal(expected, actual);
            Assert.True(expectedwatch.ElapsedTicks >= actualwatch.ElapsedTicks);
        }
        [Fact(DisplayName = "Grid.Tools.GridTools.ToUpperFirstIndexStringBuilder Performance 02")]
        public void ToUpperFirstIndexStringBuilder_PerformanceTest02()
        {
            var str = "statusId";
            var newtestwatch3 = System.Diagnostics.Stopwatch.StartNew();
            var strResult3 = (string)(str.Remove(1, str.Length - 1)).ToUpper() + str.Remove(0, 1);
            newtestwatch3.Stop();

            var actualstr = "statusId";
            var actualwatch = System.Diagnostics.Stopwatch.StartNew();
            var _stringBuilder = new System.Text.StringBuilder(actualstr);
            int x = actualstr[0];//получаем код ASCI
            int X = x - 32;//Разница между маленькой буквой и большой 32 символа.У заглавных букв код ASCI меньше
            _stringBuilder.Replace((char)x, (char)X, 0, 1);//casting на int преобразует его в символ по таблице ASCI
            var actual = _stringBuilder.ToString();
            actualwatch.Stop();

            Assert.Equal(actual, strResult3);
            Assert.True(actualwatch.ElapsedTicks <= newtestwatch3.ElapsedTicks);
        }
        [Fact(DisplayName = "Grid.Tools.GridTools.ConvertList Run (NULL)")]
        public void ConvertListTest01_RunNull()
        {
            System.Collections.Generic.IList<TableModel> listTableModelTest = null;
            Assert.Equal(GridTools.ConvertList<TableModel, ViewModel>(listTableModelTest), null);
        }
        [Fact(DisplayName = "Grid.Tools.GridTools.ConvertList Count")]
        public void ConvertListTest02_Count()
        {
            var listTableModelTest = InitTableModel.CreateTableModels(10);
            var listViewModelTest = GridTools.ConvertList<TableModel, ViewModel>(listTableModelTest);
            
            Assert.Equal(listViewModelTest.Count, listTableModelTest.Count);
        }
        [Fact(DisplayName = "Grid.Tools.GridTools.ConvertList Run")]
        public void ConvertListTest03_Run()
        {
            var count = TestTools.rInt(10,0);
            var listTableModelTest = InitTableModel.CreateTableModels(count);
            var listViewModelTest = GridTools.ConvertList<TableModel, ViewModel>(listTableModelTest);
            
            for (var i = 0; i < count; i++)
            {
                Assert.Null(listTableModelTest[i].User);
                Assert.Null(listTableModelTest[i].UserNull);
                Assert.NotNull(listViewModelTest[i].UserNullName);
                Assert.NotNull(listViewModelTest[i].UserName);
            }
        }
        [Fact(DisplayName = "Grid.Tools.GridTools.Convert Run")]
        public void ConvertTest01_Run()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestEmployees(5).CreateTestUsers(5).CreateTableModels(5);
                
                var modelTest = context.Set<TableModel>().Include(m => m.User).Include(m => m.UserNull).Last();
                var viewModelTest = GridTools.Convert<TableModel, ViewModel>(modelTest);

                Assert.Equal(viewModelTest.Id, modelTest.Id);
                Assert.Equal(viewModelTest.Test, modelTest.Test);
                Assert.Equal(viewModelTest.Date, modelTest.Date);
                Assert.Equal(viewModelTest.DateNull, modelTest.DateNull);
                Assert.Equal(viewModelTest.UserId, modelTest.UserId);
                Assert.Equal(viewModelTest.UserNullId, modelTest.UserNullId);
                Assert.Equal(viewModelTest.IsBool, modelTest.IsBool);
                Assert.Equal(viewModelTest.IsBoolNull, modelTest.IsBoolNull);
                Assert.Equal(viewModelTest.Decimal, modelTest.Decimal);
                Assert.Equal(viewModelTest.DecimalNull, modelTest.DecimalNull);
                Assert.Equal(viewModelTest.User, modelTest.User);
                Assert.Equal(viewModelTest.UserNull, modelTest.UserNull);
                Assert.Equal(viewModelTest.DisplayName, modelTest.DisplayName);
                Assert.Equal(viewModelTest.CreatedDate, modelTest.CreatedDate);
                Assert.Equal(viewModelTest.LastUpdatedBy, modelTest.LastUpdatedBy);
                Assert.Equal(viewModelTest.LastUpdatedDate, modelTest.LastUpdatedDate);


                Assert.NotNull(viewModelTest.UserName);
                Assert.Null(viewModelTest.UserNull);
                Assert.NotNull(viewModelTest.UserNullName);
            }
        }
        [Fact(DisplayName = "Grid.Tools.GridTools.OrderByList Memory testing")]
        public void OrderByListTest01_MemoryTesting()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestUsers(5);
                
                var str = new System.Collections.Generic.List<string>() { "CreatedDate", "DisplayName desc" };
                var actual = context.Set<User>().AsQueryable().OrderByList(str);
                var expected = context.Set<User>().AsQueryable().OrderBy(o => o.CreatedDate).ThenByDescending(t => t.DisplayName);

                Assert.Equal(expected, actual);
                Assert.Equal(expected.ToList(), actual.ToList());
            }
        }
        [Fact(DisplayName = "Grid.Tools.GridTools.MultipleIncludes Memory testing")]
        public void MultipleIncludesTest01_MemoryTesting()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestUsers(10).CreateTableModels(5);
                var actual = context.Set<TableModel>().MultipleIncludes("User", "EmployeeNull").ToList();
                var expected = context.Set<TableModel>().Include(i => i.User).Include(i => i.EmployeeNull).ToList();
                
                Assert.Equal(expected, actual);
            }
        }
        [Fact(DisplayName = "Grid.Tools.GridTools.MultipleIncludes Performance")]
        public void MultipleIncludesTest01_PerformanceTest()
        {
            using (var context = new TestContext.TestDbContext(TestTools.CreateNewContextOptions()))
            {
                context.CreateTestUsers(10).CreateTableModels(5);

                var expectedwatch = System.Diagnostics.Stopwatch.StartNew();
                var expected = context.Set<TableModel>().Include(i => i.User).Include(i => i.EmployeeNull).ToList();
                expectedwatch.Stop();

                var actualwatch = System.Diagnostics.Stopwatch.StartNew();
                var actual = context.Set<TableModel>().AsQueryable();
                string[] includes = { "User", "EmployeeNull" };
                var actuallist = includes.Aggregate(actual, (current, include) => current.Include(include)).ToList();
                actualwatch.Stop();

                Assert.True(actualwatch.ElapsedTicks <= expectedwatch.ElapsedTicks);
            }
        }
    }
}
