using KPMA.Data.Models;
using System.Linq;

namespace KPMA.Managers
{
    public interface IGridManager<T, GridFindModel>
    {
        User CurrentUser { get; }
        IQueryable<T> GetGridList(int? keyId = null);
        System.Threading.Tasks.Task<System.Collections.Generic.List<T>> GetGridListAsync(int? keyId = null);
        IQueryable<T> GetGridSelectList(int? keyId, string term);
        System.Threading.Tasks.Task<System.Collections.Generic.List<ViewModels.SelectItemViewModel>> GetGridSelectListAsync(int? keyId, string term);
        System.Threading.Tasks.Task<T> GetGridRowModelAsync(int id);
        System.Threading.Tasks.Task<T> SaveGridRowModelAsync(T model);
        System.Threading.Tasks.Task DeleteGridRowModelAsync(int id);
        void ChangeModelSaveGridRowModel(T model);
        ResponseModel<T> GetGridResponseModel(RequestModel<GridFindModel> requestModel);
        //System.Threading.Tasks.Task<ResponseModel<T>> GetGridResponseModelAsync(RequestModel requestModel);
    }
}

