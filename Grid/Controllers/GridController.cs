using Grid.Managers;
using Grid.Tools;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Grid.Controllers
{
    [Route("api/[controller]")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class GridController<GridTableModel,GridViewModel,GridFindModel> : Controller

                                                            where GridTableModel :  class,
                                                                                    Grid.Interfaces.IIdModel,
                                                                                    Grid.Interfaces.IDisplayName,
                                                                                    Grid.Interfaces.IClearVirtualPropertiesModel,
                                                                                    new()

                                                            where GridViewModel :   GridTableModel,
                                                                                    new()

                                                            where GridFindModel :   GridTableModel,
                                                                                    new()
    {
        protected readonly IGridManager<GridTableModel, GridFindModel> objManager;
        public readonly int tableName;

        public GridController(  IGridManager<GridTableModel, GridFindModel> objManager  )
        {
            this.objManager = objManager;
            tableName = 0;
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("list")]
        public Microsoft.AspNetCore.Mvc.IActionResult GetGridRequestModel([FromBody] Grid.Models.RequestModel<GridFindModel> requestModel)
        {
            try
            {
                var ret = objManager.GetGridResponseModel(requestModel);
                
                var responseViewModel = new Models.ResponseModel<GridViewModel>();
                responseViewModel.TableId = this.tableName;
                responseViewModel.CurrentPage = ret.CurrentPage;
                responseViewModel.TotalRowCount = ret.TotalRowCount;
                responseViewModel.List = GridTools.ConvertList<GridTableModel,GridViewModel>(ret.List);
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
        virtual public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetGridRowModelAsync(int id, Grid.Enums.Cmd? mode)
        {
            try
            {
                if (mode == null)
                {
                    mode = Grid.Enums.Cmd.@default;
                }

                GridTableModel model = default(GridTableModel);

                switch (mode)
                {
                    case Grid.Enums.Cmd.viewonly:
                    case Grid.Enums.Cmd.@default:
                        {
                            model = await objManager.GetGridRowModelAsync(id);

                            if (model == null)
                            {
                                throw new Exception($"Ошибка получения записи из базы ({id})");
                            }

                        }
                        break;
                    case Grid.Enums.Cmd.@new:

                        model = new GridTableModel();
                        break;
                    case Grid.Enums.Cmd.copy:
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
                    case Grid.Enums.Cmd.find:

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
                    return Json(new Models.SelectItemViewModel { id = model.Id, text = model.DisplayName });
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