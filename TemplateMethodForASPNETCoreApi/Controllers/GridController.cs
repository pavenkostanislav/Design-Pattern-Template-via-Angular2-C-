
using Microsoft.AspNetCore.Mvc;
using System;
using TEST.Managers;
using TEST.Models;
using TEST.Tools;

namespace TEST.Controllers
{
    [Route("api/[controller]")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class GridController<GridTableModel,GridViewModel,GridFindModel> : Controller

                                                            where GridTableModel :  class,
                                                                                    Interfaces.IIdModel,
                                                                                    Interfaces.IDisplayName,
                                                                                    Interfaces.IClearVirtualPropertiesModel,
                                                                                    new()

                                                            where GridViewModel :   GridTableModel,
                                                                                    new()

                                                            where GridFindModel :   GridTableModel,
                                                                                    new()
    {
        protected readonly IGridManager<GridTableModel, GridFindModel> objManager;
        public readonly int tableName;

        public GridController(  IGridManager<GridTableModel, GridFindModel> objManager )
        {
            this.objManager = objManager;
            tableName = 0;
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("list")]
        public IActionResult GetGridRequestModel([FromBody] TEST.Managers.RequestModel<GridFindModel> requestModel)
        {
            try
            {
                //var permissions = ???; //Полчение прав по таблице
                //if (!permissions.CanRead)
                //{
                //    throw new Exception("У вас нет прав на эту страницу!");
                //}

                var ret = objManager.GetGridResponseModel(requestModel);

                var responseViewModel = new ResponseModel<GridViewModel>();
                responseViewModel.TableId = this.tableName;
                responseViewModel.CurrentPage = ret.CurrentPage;
                responseViewModel.TotalRowCount = ret.TotalRowCount;
                responseViewModel.List = GridTools.ConvertList<GridTableModel, GridViewModel>(ret.List);
                return Json(responseViewModel);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("list/{keyId:int?}")]
        virtual public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetGridListAsync(int? keyId)
        {
            try
            {
                //var permissions = ???; //Полчение прав по таблице
                //if (!permissions.CanRead)
                //{
                //    throw new Exception("У вас нет прав на эту страницу!");
                //}

                var list = await objManager.GetGridListAsync(keyId);
                var viewModelList = GridTools.ConvertList<GridTableModel, GridViewModel>(list);
                return Json(viewModelList);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("{id:int}/{mode?}")]
        virtual public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetGridRowModelAsync(int id, Enums.ModeEnum? mode)
        {
            try
            {
                //var permissions = ???; //Полчение прав по таблице
                //if (!permissions.CanRead)
                //{
                //    throw new Exception("У вас нет прав на просмотр страницы!");
                //}

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
                return BadRequest(ex.Message);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpPost()]
        virtual public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> SaveGridRowModelAsync([FromBody] GridTableModel model)
        {
            try
            {
                //var permissions = ???; //Полчение прав по таблице
                //if (model.Id == 0 && !permissions.CanAdd)
                //{
                //    throw new Exception("У вас нет прав на добавление новой записи!");
                //}
                //if (model.Id != 0 && !permissions.CanEdit)
                //{
                //    throw new Exception("У вас нет прав на редактирование записи!");
                //}

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
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        virtual public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> DeleteGridRowModelAsync(int id)
        {
            try
            {
                //var permissions = ???; //Полчение прав по таблице
                //if (!permissions.CanDelete)
                //{
                //    throw new Exception("У вас нет прав на удаление записи!");
                //}

                await objManager.DeleteGridRowModelAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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

                return BadRequest(ex.Message);
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
                    return Json(new SelectItemViewModel { id = model.Id, text = model.DisplayName });
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}