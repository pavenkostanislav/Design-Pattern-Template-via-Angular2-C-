using Grid.Models;
using Grid.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Grid.Managers
{
    public class GridManager<GridTableModel, GridViewModel, GridFindModel> : IGridManager<GridTableModel, GridViewModel, GridFindModel> 
                                                where GridTableModel :   class,
                                                            Grid.Interfaces.IIdModel,
                                                            Grid.Interfaces.ILogModel,
                                                            Grid.Interfaces.IDisplayName,
                                                            Grid.Interfaces.IClearVirtualPropertiesModel,
                                                            new()

                                                where GridViewModel : class,
                                                            Grid.Interfaces.IIdModel,
                                                            Grid.Interfaces.ILogModel,
                                                            Grid.Interfaces.IDisplayName,
                                                            Grid.Interfaces.IClearVirtualPropertiesModel,
                                                            new()

                                                where GridFindModel : class,
                                                            Grid.Interfaces.IIdModel,
                                                            Grid.Interfaces.IDisplayName,
                                                            Grid.Interfaces.IClearVirtualPropertiesModel,
                                                            Grid.Interfaces.ILogFindModel,
                                                            new()
    {
        public readonly DbContext db;
        private readonly string _TableName;
        public string TableName
        {
            get
            {
                return this._TableName;
            }
        }

        public GridManager(DbContext db)
        {
            this.db = db;
            this._TableName = (this.GetGridRowNewModel()).GetType().Name;
        }

        #region List

        /// <summary>
        /// Async: get task list classes with 1) includes first level (2) filtered (3) sorted
        /// 
        /// Execute 1:  used this.GetGridListViewModel with params: requestModel.KeyId and requestModel.FindModel
        ///             list = await this.GetGridListAsync(requestModel.KeyId, requestModel.FindModel)
        /// Execute 2:  convert your list classes to list view model classes and get IQueryable
        /// Execute 3:  if (requestModel.OrderNamesList != null) 
        ///             do myQueryable = myQueryable.OrderByList(requestModel.OrderNamesList)
        /// </summary>
        /// <param name="requestModel">request model by your class model</param>
        /// <returns> new response model your view model class with default params 
        /// Param TotalRowCount: list.Count() 
        /// Param CurrentPage: GridTools.GetCarouselCurrentPage(requestModel.PageSize, requestModel.CurrentPage, list.Count()) 
        /// Param List: myQueryable.Skip(requestModel.PageSize * requestModel.CurrentPage).Take(requestModel.PageSize).ToList()
        /// </returns>
        virtual public async Task<Grid.Models.ResponseModel<GridViewModel>> GetGridResponseModelAsync(RequestModel<GridFindModel> requestModel = null)
        {
            if(requestModel == null)
            {
                requestModel = new RequestModel<GridFindModel>
                {
                    CurrentPage = 0,
                    PageSize = 0,
                    KeyId = null,
                    OrderNamesList = new System.Collections.Generic.List<string>(),
                    FindModel = new GridFindModel()
                };
            }

            var list = await this.GetGridListViewModelAsync(requestModel.KeyId, requestModel.FindModel);

            var myQueryable = list.AsQueryable();

            if (requestModel.OrderNamesList != null)
            {
                myQueryable = myQueryable.OrderByList(requestModel.OrderNamesList);
            }
            
            return new ResponseModel<GridViewModel>
            {
                TotalRowCount = list.Count(),
                List = myQueryable.Skip(requestModel.PageSize * requestModel.CurrentPage).Take(requestModel.PageSize).ToList()
            };
        }
        /// <summary>
        /// Get list classes includes first level with filtered
        /// 
        /// Execute 1: list = this.GetGridListWithOneLevelIncludes();
        /// Execute 2: list = this.QueryableFilter(list, findModel)
        /// </summary>
        /// <param name="predicate">lambda-expressions: input-parameters: your class parameter, operator: true/false</param>
        /// <returns>Get list classes includes first level with filtered</returns>
        virtual public IQueryable<GridTableModel> GetGridList(int? keyId = null, GridFindModel findModel = null)
        {
            var list = this.GetGridListWithOneLevelIncludes();

            list = this.QueryableFilter(list, findModel);

            return list;
        }

        /// <summary>
        /// Get queryable list by table model with filtered
        /// 
        /// Execute 1:  filtered Id by Id
        /// Execute 2:  filtered CreatedBy by CreatedBy
        /// Execute 3:  filtered CreatedDate by CreatedDateStart - CreatedDateEnd
        /// Execute 4:  filtered LastUpdatedBy by LastUpdatedBy
        /// Execute 5:  filtered LastUpdatedDate by LastUpdatedDateStart - LastUpdatedDateEnd
        /// 
        /// </summary>
        /// <param name="list">queryable list by table model</param>
        /// <param name="findModel">find model implement by table model</param>
        /// <returns>Get queryable list by table model with filtered</returns>
        virtual public IQueryable<GridTableModel> QueryableFilter(IQueryable<GridTableModel> list, GridFindModel findModel)
        {
            if (findModel != null)
            {
                if (findModel.Id != default(int))
                {
                    list = list.Where(m => m.Id == findModel.Id);
                }

                if (!string.IsNullOrEmpty(findModel.CreatedBy))
                {
                    list = list.Where(m => m.CreatedBy.Contains(findModel.CreatedBy));
                }

                if (findModel.CreatedDateStart.HasValue)
                {
                    list = list.Where(m => m.CreatedDate >= findModel.CreatedDateStart);
                }
                if (findModel.CreatedDateEnd.HasValue)
                {
                    list = list.Where(m => m.CreatedDate <= findModel.CreatedDateEnd);
                }

                //LastUpdatedBy
                if (!string.IsNullOrEmpty(findModel.LastUpdatedBy))
                {
                    list = list.Where(m => m.LastUpdatedBy.Contains(findModel.LastUpdatedBy));
                }

                //LastUpdatedDate
                if (findModel.LastUpdatedDateStart.HasValue)
                {
                    list = list.Where(m => m.LastUpdatedDate >= findModel.LastUpdatedDateStart);
                }
                if (findModel.LastUpdatedDateEnd.HasValue)
                {
                    list = list.Where(m => m.LastUpdatedDate <= findModel.LastUpdatedDateEnd);
                }
            }
            return list;
        }

        /// <summary>
        /// Async: get task list classes with includes first level
        /// 
        /// Execute 1:  return this.GetGridList(keyId, findModel).ToListAsync();
        /// </summary>
        /// <param name="keyId">not used</param>
        /// <param name="findModel"></param>
        /// <returns>Async: get task list classes with includes first level</returns>

        virtual public System.Threading.Tasks.Task<System.Collections.Generic.List<GridTableModel>> GetGridListAsync(int? keyId = null, GridFindModel findModel = null)
        {
            return this.GetGridList(keyId, findModel).ToListAsync();
        }


        /// <summary>
        /// Get list classes includes first level
        /// 
        /// Execute 1:  query = db.Set<GridTableModel>().AsNoTracking().AsQueryable()
        /// Execute 2:  foreach all includes in db.Model.FindEntityType(typeof(GridTableModel)).GetNavigations() 
        ///             and do query = query.Include(property.Name);
        /// Execute 3:  if (predicate != null) query = query.Where(predicate);
        /// </summary>
        /// <param name="predicate">lambda-expressions: input-parameters: your class parameter, operator: true/false</param>
        /// <returns>Get list classes includes first level</returns>
        virtual public IQueryable<GridTableModel> GetGridListWithOneLevelIncludes(System.Linq.Expressions.Expression<Func<GridTableModel, bool>> predicate = null)
        {
            var query = db.Set<GridTableModel>().AsNoTracking().AsQueryable();
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

        /// <summary>
        /// Get list classes with includes first level async
        /// 
        /// Execute 1:  return this.GetGridListWithOneLevelIncludes(predicate).ToListAsync()
        /// </summary>
        /// <param name="predicate">lambda-expressions: input-parameters: your class parameter, operator: true/false</param>
        /// <returns>Get list classes includes first level</returns>
        virtual public System.Threading.Tasks.Task<System.Collections.Generic.List<GridTableModel>> GetGridListWithOneLevelIncludesAsync(System.Linq.Expressions.Expression<Func<GridTableModel, bool>> predicate = null)
        {
            return this.GetGridListWithOneLevelIncludes(predicate).ToListAsync();
        }

        virtual public async System.Threading.Tasks.Task<System.Collections.Generic.IList<GridViewModel>> GetGridListViewModelAsync(int? keyId = null, GridFindModel findModel = null)
        {
            var list = await this.GetGridListAsync(keyId, findModel);
            var viewModelList = GridTools.ConvertList<GridTableModel, GridViewModel>(list);
            return viewModelList;
        }
        /// <summary>
        /// Can saved list classes implement IList GridTableModel 
        /// </summary>
        /// <param name="ilist">list classes implement IList GridTableModel</param>
        /// <returns></returns>

        virtual public async System.Threading.Tasks.Task<System.Collections.Generic.IList<GridViewModel>> SaveGridListAsync(System.Collections.Generic.IList<GridTableModel> ilist)
        {
            var list = new System.Collections.Generic.List<GridTableModel>();
            foreach (var item in ilist)
            {
                list.Add(await SaveModelInContextAsync(item));
            }
            await db.SaveChangesAsync();
            return Tools.GridTools.ConvertList<GridTableModel, GridViewModel>(list);
        }
        #endregion

        #region Model
        virtual public GridTableModel GetGridRowNewModel()
        {
            return new GridTableModel();
        }
        virtual public GridFindModel GetGridRowFindModel()
        {
            return new GridFindModel();
        }
        virtual public async System.Threading.Tasks.Task<GridTableModel> GetGridRowModelAsync(int id, bool withOneLevelIncludes = false)
        {
            GridTableModel model;
            if (withOneLevelIncludes)
            {
                model = await this.GetGridListWithOneLevelIncludes(m => m.Id == id).FirstOrDefaultAsync();
            }
            else
            {
                model = await db.Set<GridTableModel>().FindAsync(id);
            }
            if (model == null)
            {
                throw new Exception($"Ошибка получения записи из базы ({id})");
            }
            return model;
        }
        virtual public async System.Threading.Tasks.Task<GridTableModel> GetGridRowCopyModelAsync(int id)
        {
            var _copy = await this.GetGridRowModelAsync(id);
            _copy.Id = 0;
            return _copy;
        }

        virtual public async System.Threading.Tasks.Task<GridViewModel> SaveGridRowModelAsync(GridTableModel model)
        {
            GridTableModel ResultModel = await SaveModelInContextAsync(model);
            await db.SaveChangesAsync();

            return Tools.GridTools.Convert<GridTableModel, GridViewModel>(ResultModel);
        }

        virtual public async Task<GridTableModel> SaveModelInContextAsync(GridTableModel model)
        {
            model.DisplayName = null;

            if (model.Id == 0)
            {
                model.ClearVirtualProperties();
                model = (await db.Set<GridTableModel>().AddAsync(model)).Entity;
            }
            else
            {
                if (model.Id > 0)
                {
                    model.ClearVirtualProperties();
                    model = db.Set<GridTableModel>().Update(model).Entity;
                }
                else
                {
                    throw new Exception($"Данные не сохранены! Ошибка сохранения записи {model.Id}");
                }
            }

            return model;
        }

        virtual public async System.Threading.Tasks.Task<GridViewModel> GetGridRowViewModelAsync(int id)
        {
            var model = await this.GetGridRowModelAsync(id, true);
            return Tools.GridTools.Convert<GridTableModel, GridViewModel>(model);
        }

        /// <summary>
        /// Async: find and delete model in database
        /// if (model == null) 
        /// do throw new Exception($"Запись не найдена. ({id})");
        /// 
        /// </summary>
        /// <param name="id">key in table at database</param>
        /// <returns>System.Threading.Tasks.Task</returns>
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

        #endregion

        #region GetSelectViewModel

        /// <summary>
        /// Get queryable list classes
        /// 
        /// Execute 1:  if (!string.IsNullOrWhiteSpace(term)) 
        ///             do list = list.Where(u => u.DisplayName.Contains(term))
        /// </summary>
        /// <param name="keyId">not used</param>
        /// <param name="term">filter by DisplayName property</param>
        /// <returns>Get queryable list classes</returns>
        virtual public IQueryable<GridTableModel> GetGridSelectList(int? keyId, string term)
        {
            var list = db.Set<GridTableModel>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                list = list.Where(u => u.DisplayName.Contains(term));
            }

            return list;
        }
        virtual public System.Threading.Tasks.Task<System.Collections.Generic.List<Grid.Models.SelectItemViewModel>> GetGridSelectListAsync(int? keyId, string term)
        {
            return this.GetGridSelectList(keyId, term)
                .Select(u => new Grid.Models.SelectItemViewModel { id = u.Id, text = u.DisplayName })
                .OrderBy(m => m.text)
                .ToListAsync();
        }

        virtual async public System.Threading.Tasks.Task<Grid.Models.SelectItemViewModel> GetSelectItemViewModelAsync(int id)
        {
            return  await db.Set<GridTableModel>()
                            .Select(s => new Grid.Models.SelectItemViewModel { id = s.Id, text = s.DisplayName, sort = s.Id })
                            .FirstOrDefaultAsync(m => m.id == id);
        }

        #endregion
    }
}
