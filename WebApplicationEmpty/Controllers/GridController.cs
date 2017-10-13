using KPMA.Data.Models;
using KPMA.Managers;
using KPMA.Tools;
using Microsoft.AspNetCore.Mvc;
using System;

namespace KPMA.Controllers
{
    [Route("api/[controller]")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class GridController<GridTableModel,GridViewModel,GridFindModel> : Controller

                                                            where GridTableModel :  class,
                                                                                    Data.Interfaces.IIdModel,
                                                                                    Data.Interfaces.IDisplayName,
                                                                                    Data.Interfaces.IClearVirtualPropertiesModel,
                                                                                    new()

                                                            where GridViewModel :   GridTableModel,
                                                                                    new()

                                                            where GridFindModel :   GridTableModel,
                                                                                    new()
    {
        protected readonly IGridManager<GridTableModel, GridFindModel> objManager;
        protected readonly ICoreManager coreManager;
        protected readonly IMetaObjectManager moManager;
        protected readonly IAttachmentManager attManager;
        protected readonly IConstantManager constManager;
        protected readonly IEmployeeManager employeeManager;

        public readonly MetaObject tableModel;

        public GridController(  IGridManager<GridTableModel, GridFindModel> objManager,
                                ICoreManager coreManager,
                                IAttachmentManager attManager,
                                IMetaObjectManager moManager,
                                IConstantManager constManager,
                                IEmployeeManager employeeManager
                                        )
        {
            this.objManager = objManager;
            this.coreManager = coreManager;
            this.attManager = attManager;
            this.moManager = moManager;
            this.constManager = constManager;
            this.employeeManager = employeeManager;


            MetaObject tNode = moManager.GetMetaObjectModelByName("Table");
            if (tNode == null)
            {
                throw new Exception("Нарушена базовая структура хранения данных!");
            }

            tableModel = moManager.GetMetaObjectModelByName(typeof(GridTableModel).Name, tNode.Id);
            if (tableModel == null)
            {
                throw new Exception("Нет таблицы в базе!");
            }

        }

        [Microsoft.AspNetCore.Mvc.HttpPost("list")]
        public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetGridRequestModelAsync([FromBody] KPMA.Managers.RequestModel<GridFindModel> requestModel)
        {
            try
            {
                var permissions = await coreManager.GetPermissions(tableModel.Name, User, false);
                if (!permissions.CanRead)
                {
                    throw new Exception("У вас нет прав на эту страницу!");
                }

                var ret = objManager.GetGridResponseModel(requestModel);
                
                var responseViewModel = new ResponseModel<GridViewModel>();
                responseViewModel.TableId = this.tableModel.Id;
                responseViewModel.CurrentPage = ret.CurrentPage;
                responseViewModel.TotalRowCount = ret.TotalRowCount;
                responseViewModel.List = GridTools.ConvertList<GridTableModel,GridViewModel>(ret.List);
                return Json(responseViewModel);

            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionTools.GetExceptionMessage(ex));
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("list/{keyId:int?}")]
        virtual public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetGridListAsync(int? keyId)
        {
            try
            {
                var permissions = await coreManager.GetPermissions(tableModel.Name, User, false);
                if (!permissions.CanRead)
                {
                    throw new Exception("У вас нет прав на эту страницу!");
                }

                var list = await objManager.GetGridListAsync(keyId);
                var viewModelList = GridTools.ConvertList<GridTableModel, GridViewModel>(list);
                return Json(viewModelList);

            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionTools.GetExceptionMessage(ex));
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("{id:int}/{mode?}")]
        virtual public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetGridRowModelAsync(int id, Enums.ModeEnum? mode)
        {
            try
            {
                var permissions = await coreManager.GetPermissions(tableModel.Name, User, false);
                if (!permissions.CanRead)
                {
                    throw new Exception("У вас нет прав на просмотр страницы!");
                }

                if (mode == null)
                {
                    mode = Enums.ModeEnum.@default;
                }

                GridTableModel model = default(GridTableModel);

                switch (mode)
                {
                    case Enums.ModeEnum.viewonly:
                    case Enums.ModeEnum.@default:
                        {
                            model = await objManager.GetGridRowModelAsync(id);

                            if (model == null)
                            {
                                throw new Exception($"Ошибка получения записи из базы ({id})");
                            }

                        }
                        break;
                    case Enums.ModeEnum.@new:

                        model = new GridTableModel();
                        break;
                    case Enums.ModeEnum.copy:
                        {
                            var _copy = await objManager.GetGridRowModelAsync(id);
                            if (_copy == null)
                            {
                                throw new Exception($"Ошибка получения записи из базы ({id})");
                            }

                            model = _copy;

                            model.Id = 0;

                        }
                        break;
                    case Enums.ModeEnum.find:

                        var findModel = new GridFindModel();
                        return Json(findModel);
                    default:
                        throw new Exception($"Ошибка получения записи Chat ({id})");
                }

                return Json(model);

            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionTools.GetExceptionMessage(ex));
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPost()]
        virtual public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> SaveGridRowModelAsync([FromBody] GridTableModel model)
        {
            try
            {
                var permissions = await coreManager.GetPermissions(tableModel.Name, User, false);
                if (model.Id == 0 && !permissions.CanAdd)
                {
                    throw new Exception("У вас нет прав на добавление новой записи!");
                }
                if (model.Id != 0 && !permissions.CanEdit)
                {
                    throw new Exception("У вас нет прав на редактирование записи!");
                }

                if (ModelState.IsValid)
                {
                    return Json(await objManager.SaveGridRowModelAsync(model));
                }
                else
                {
                    throw new Exception($"Ошибка");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionTools.GetExceptionMessage(ex));
            }
        }

        [HttpDelete("{id:int}")]
        virtual public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> DeleteGridRowModelAsync(int id)
        {
            try
            {
                var permissions = await coreManager.GetPermissions(tableModel.Name, User, false);
                if (!permissions.CanDelete)
                {
                    throw new Exception("У вас нет прав на удаление записи!");
                }

                await objManager.DeleteGridRowModelAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionTools.GetExceptionMessage(ex));
            }
        }



        #region Select

        [HttpGet("select/{parentId:int?}/{term?}")]
        public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> SelectGridFindAsync(int? parentId, string term)
        {
            try
            {
                var list = await objManager.GetGridSelectListAsync(parentId, term);
                return Json(list);

            }
            catch (Exception ex)
            {

                return BadRequest(ExceptionTools.GetExceptionMessage(ex));
            }
        }

        [HttpGet("select/{id:int}")]
        public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetSelectGridRowModelAsync(int id)
        {
            try
            {
                var model = await objManager.GetGridRowModelAsync(id);
                if (model != null)
                {
                    return Json(new ViewModels.SelectItemViewModel { id = model.Id, text = model.DisplayName });
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {

                return BadRequest(ExceptionTools.GetExceptionMessage(ex));
            }
        }

        #endregion
    }
}