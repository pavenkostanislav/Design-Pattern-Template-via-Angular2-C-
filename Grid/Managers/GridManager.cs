using Grid.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Grid.Managers
{
    public class GridManager<GridTableModel, GridFindModel> : IGridManager<GridTableModel,GridFindModel> 
                                                where GridTableModel :   class,
                                                            Grid.Interfaces.IIdModel,
                                                            Grid.Interfaces.IDisplayName,
                                                            Grid.Interfaces.IClearVirtualPropertiesModel
                                                where GridFindModel : class,
                                                            Grid.Interfaces.IIdModel,
                                                            Grid.Interfaces.IDisplayName,
                                                            Grid.Interfaces.IClearVirtualPropertiesModel
    {
        public readonly DbContext db;

        public GridManager(DbContext db)
        {
            this.db = db;
        }


        public ResponseModel<GridTableModel> GetGridResponseModel(RequestModel<GridFindModel> requestModel)
        {
            var list = this.GetGridList(requestModel.KeyId, requestModel.FindModel).ToList();

            var currentPage = 0;
            var totalRowCount = list.Count();

            if (requestModel.CurrentPage > totalRowCount / requestModel.PageSize + 1)
            {
                currentPage = 0;
            }
            else
            {
                currentPage = requestModel.CurrentPage;
            }

            int skip = requestModel.PageSize * (requestModel.CurrentPage);

            int take = requestModel.PageSize;

            var listPage = list.Skip(skip).Take(take);

            var responseModel = new ResponseModel<GridTableModel>
            {
                TotalRowCount = totalRowCount,
                CurrentPage = currentPage,
                List = listPage.ToList()
            };

            return responseModel;
        }

        virtual public IQueryable<GridTableModel> GetGridList(int? keyId = default(int?), GridFindModel findModel = default(GridFindModel))
        {
            return this.GetGridAllList();
        }

        virtual public IQueryable<GridTableModel> GetGridAllList(System.Linq.Expressions.Expression<Func<GridTableModel, bool>> predicate = null)
        {
            var query = db.Set<GridTableModel>().AsQueryable();
            foreach (var property in db.Model.FindEntityType(typeof(GridTableModel)).GetNavigations())
            {
                query = query.Include(property.Name);
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return query;
        }

        virtual public System.Threading.Tasks.Task<System.Collections.Generic.List<GridTableModel>> GetGridAllListAsync(System.Linq.Expressions.Expression<Func<GridTableModel, bool>> predicate = null)
        {
            return this.GetGridAllList(predicate).ToListAsync();
        }

        virtual public System.Threading.Tasks.Task<System.Collections.Generic.List<GridTableModel>> GetGridListAsync(int? keyId = null)
        {
            return this.GetGridList(keyId).ToListAsync();
        }

        virtual public IQueryable<GridTableModel> GetGridSelectList(int? keyId, string term)
        {
            var list = db.Set<GridTableModel>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                list = list.Where(u => u.DisplayName.Contains(term));
            }

            return list;
        }

        virtual public System.Threading.Tasks.Task<System.Collections.Generic.List<SelectItemViewModel>> GetGridSelectListAsync(int? keyId, string term)
        {
            return this.GetGridSelectList(keyId, term)
                .Select(u => new SelectItemViewModel { id = u.Id, text = u.DisplayName })
                .OrderBy(m => m.text)
                .ToListAsync();
        }

        virtual public System.Threading.Tasks.Task<GridTableModel> GetGridRowModelAsync(int id)
        {
            return db.Set<GridTableModel>().FindAsync(id);
        }

        virtual public void ChangeModelSaveGridRowModel(GridTableModel model)
        {
            //throw new NotImplementedException();
        }

        virtual public async System.Threading.Tasks.Task<GridTableModel> SaveGridRowModelAsync(GridTableModel model)
        {
            this.ChangeModelSaveGridRowModel(model);

            GridTableModel e;
            if (model.Id == 0)
            {
                model.ClearVirtualProperties();
                e = db.Set<GridTableModel>().Add(model).Entity;
            }
            else
            {
                model.ClearVirtualProperties();
                e = db.Set<GridTableModel>().Update(model).Entity;
            }

            await db.SaveChangesAsync();

            return e;
        }

        virtual public async System.Threading.Tasks.Task DeleteGridRowModelAsync(int id)
        {
            var model = await db.Set<GridTableModel>().FindAsync(id);
            if (model == null)
            {
                throw new Exception($"Запись не найдена. ({id})");
            }

            db.Set<GridTableModel>().Remove(model);
            await db.SaveChangesAsync();
        }
    }
}
