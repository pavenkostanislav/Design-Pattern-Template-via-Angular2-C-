using TEST.Data;
using TEST.Managers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TEST.Managers
{
    public class GridManager<T, GridFindModel> : IGridManager<T,GridFindModel> 
                                                where T :   class, 
                                                            Interfaces.IIdModel,
                                                            Interfaces.IDisplayName,
                                                            Interfaces.IClearVirtualPropertiesModel
                                                where GridFindModel : class,
                                                            Interfaces.IIdModel,
                                                            Interfaces.IDisplayName,
                                                            Interfaces.IClearVirtualPropertiesModel
    {
        public readonly DbContext db;

        public GridManager(DbContext db)
        {
            this.db = db;
        }


        public ResponseModel<T> GetGridResponseModel(RequestModel<GridFindModel> requestModel)
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

            var responseModel = new ResponseModel<T>
            {
                TotalRowCount = totalRowCount,
                CurrentPage = currentPage,
                List = listPage.ToList()
            };

            return responseModel;
        }


        //public System.Threading.Tasks.Task<ResponseModel<T>> GetGridResponseModelAsync(RequestModel requestModel)
        //{
        //    throw new NotImplementedException();
        //}

        virtual public IQueryable<T> GetGridList(int? keyId = default(int?), GridFindModel findModel = default(GridFindModel))
        {
            return this.GetGridAllList();
        }

        virtual public IQueryable<T> GetGridAllList(System.Linq.Expressions.Expression<Func<T, bool>> predicate = null)
        {
            var query = db.Set<T>().AsQueryable();
            foreach (var property in db.Model.FindEntityType(typeof(T)).GetNavigations())
            {
                query = query.Include(property.Name);
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return query;
        }

        virtual public System.Threading.Tasks.Task<System.Collections.Generic.List<T>> GetGridAllListAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return this.GetGridAllList(predicate).ToListAsync();
        }

        virtual public System.Threading.Tasks.Task<System.Collections.Generic.List<T>> GetGridListAsync(int? keyId = null)
        {
            return this.GetGridList(keyId).ToListAsync();
        }

        virtual public IQueryable<T> GetGridSelectList(int? keyId, string term)
        {
            var list = db.Set<T>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                list = list.Where(u => u.DisplayName.Contains(term));
            }

            return list;
        }

        virtual public System.Threading.Tasks.Task<System.Collections.Generic.List<ViewModels.SelectItemViewModel>> GetGridSelectListAsync(int? keyId, string term)
        {
            return this.GetGridSelectList(keyId, term)
                .Select(u => new ViewModels.SelectItemViewModel { id = u.Id, text = u.DisplayName })
                .OrderBy(m => m.text)
                .ToListAsync();
        }

        virtual public System.Threading.Tasks.Task<T> GetGridRowModelAsync(int id)
        {
            return db.Set<T>().FindAsync(id);
        }

        virtual public void ChangeModelSaveGridRowModel(T model)
        {
            //throw new NotImplementedException();
        }

        virtual public async System.Threading.Tasks.Task<T> SaveGridRowModelAsync(T model)
        {
            this.ChangeModelSaveGridRowModel(model);

            T e;
            if (model.Id == 0)
            {
                model.ClearVirtualProperties();
                e = db.Set<T>().Add(model).Entity;
            }
            else
            {
                model.ClearVirtualProperties();
                e = db.Set<T>().Update(model).Entity;
            }

            await db.SaveChangesAsync();

            return e;
        }

        virtual public async System.Threading.Tasks.Task DeleteGridRowModelAsync(int id)
        {
            var model = await db.Set<T>().FindAsync(id);
            if (model == null)
            {
                throw new Exception($"Запись не найдена. ({id})");
            }

            db.Set<T>().Remove(model);
            await db.SaveChangesAsync();
        }
    }
    public class ResponseModel<T>
    {
        public int TableId { get; set; }
        public int TotalRowCount { get; set; }
        public int CurrentPage { get; set; }
        public System.Collections.Generic.IList<T> List { get; set; }
    }
    public class RequestModel<GridFindModel>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int? KeyId { get; set; }
        public GridFindModel FindModel { get; set; }
    }
}
