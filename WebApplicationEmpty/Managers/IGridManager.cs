using KPMA.Data.Models;
using System.Linq;

namespace KPMA.Managers
{
    public interface IGridManager<T>
    {
        User CurrentUser { get; }

        IQueryable<T> GetGridList(int? parentlId);
        T GetGridRowModel(int id);
        T SaveGridRowModel(T model);
        void DeleteGridRowModel(int id);
        void ClearGridRowProperties(T model);
    }
}

