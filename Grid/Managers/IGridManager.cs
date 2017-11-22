using Grid.Models;
using System.Linq;

namespace Grid.Managers
{
    public interface IGridManager<GridTableModel, GridViewModel, GridFindModel>
    {
        string TableName { get; }
        IQueryable<GridTableModel> GetGridList(int? keyId = default(int?), GridFindModel findModel = default(GridFindModel));
        System.Threading.Tasks.Task<System.Collections.Generic.List<GridTableModel>> GetGridListAsync(int? keyId = default(int?), GridFindModel findModel = default(GridFindModel));
        IQueryable<GridTableModel> GetGridListWithOneLevelIncludes(System.Linq.Expressions.Expression<System.Func<GridTableModel, bool>> predicate = null);
        IQueryable<GridTableModel> QueryableFilter(IQueryable<GridTableModel> list, GridFindModel findModel);

        System.Threading.Tasks.Task<System.Collections.Generic.List<GridTableModel>> GetGridListWithOneLevelIncludesAsync(System.Linq.Expressions.Expression<System.Func<GridTableModel, bool>> predicate = null);

        IQueryable<GridTableModel> GetGridSelectList(int? keyId, string term);
        System.Threading.Tasks.Task<System.Collections.Generic.List<Grid.Models.SelectItemViewModel>> GetGridSelectListAsync(int? keyId, string term);
        System.Threading.Tasks.Task<System.Collections.Generic.IList<GridViewModel>> GetGridListViewModelAsync(int? keyId = default(int?), GridFindModel findModel = default(GridFindModel));
        GridTableModel GetGridRowNewModel();
        GridFindModel GetGridRowFindModel();
        System.Threading.Tasks.Task<GridTableModel> GetGridRowModelAsync(int id, bool withOneLevelIncludes = false);
        System.Threading.Tasks.Task<GridTableModel> GetGridRowCopyModelAsync(int id);
        System.Threading.Tasks.Task<GridViewModel> GetGridRowViewModelAsync(int id);
        System.Threading.Tasks.Task<Grid.Models.SelectItemViewModel> GetSelectItemViewModelAsync(int id);
        System.Threading.Tasks.Task<GridViewModel> SaveGridRowModelAsync(GridTableModel model);

        System.Threading.Tasks.Task DeleteGridRowModelAsync(int id);

        System.Threading.Tasks.Task<ResponseModel<GridViewModel>> GetGridResponseModelAsync(RequestModel<GridFindModel> requestModel = null);
        System.Threading.Tasks.Task<System.Collections.Generic.IList<GridViewModel>> SaveGridListAsync(System.Collections.Generic.IList<GridTableModel> listmodel);
        System.Threading.Tasks.Task<GridTableModel> SaveModelInContextAsync(GridTableModel model);
    }
}

