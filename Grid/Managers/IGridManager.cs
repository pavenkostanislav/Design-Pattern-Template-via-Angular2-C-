using KPMA.Models;
using System.Linq;

namespace KPMA.Managers
{
    public interface IGridManager<GridTableModel, GridFindModel>
    {
        IQueryable<GridTableModel> GetGridList(int? keyId = default(int?), GridFindModel findModel = default(GridFindModel));
        IQueryable<GridTableModel> GetGridAllList(System.Linq.Expressions.Expression<System.Func<GridTableModel, bool>> predicate);
        System.Threading.Tasks.Task<System.Collections.Generic.List<GridTableModel>> GetGridAllListAsync(System.Linq.Expressions.Expression<System.Func<GridTableModel, bool>> predicate);
        System.Threading.Tasks.Task<System.Collections.Generic.List<GridTableModel>> GetGridListAsync(int? keyId = null);
        IQueryable<GridTableModel> GetGridSelectList(int? keyId, string term);
        System.Threading.Tasks.Task<System.Collections.Generic.List<SelectItemViewModel>> GetGridSelectListAsync(int? keyId, string term);
        System.Threading.Tasks.Task<GridTableModel> GetGridRowModelAsync(int id);
        System.Threading.Tasks.Task<GridTableModel> SaveGridRowModelAsync(GridTableModel model);
        System.Threading.Tasks.Task DeleteGridRowModelAsync(int id);
        void ChangeModelSaveGridRowModel(GridTableModel model);
        ResponseModel<GridTableModel> GetGridResponseModel(RequestModel<GridFindModel> requestModel);
    }
}

