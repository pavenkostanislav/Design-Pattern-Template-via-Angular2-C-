using Grid.Managers;
using Grid.Tools;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Grid.Controllers
{
    [Route("api/[controller]")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class GridController<GridTableModel, GridViewModel, GridFindModel> : Controller, IGridController<GridTableModel, GridViewModel, GridFindModel>
    {
        protected readonly IGridManager<GridTableModel, GridViewModel, GridFindModel> objManager;

        public GridController(  IGridManager<GridTableModel, GridViewModel, GridFindModel> objManager )
        {
            this.objManager = objManager;
            
        }

        [Microsoft.AspNetCore.Mvc.HttpPost("list")]
        virtual public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetGridRequestModelAsync([FromBody] Grid.Models.RequestModel<GridFindModel> requestModel)
        {
            try
            {
                return Json(await this.objManager.GetGridResponseModelAsync(requestModel));

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
                return Json(await this.objManager.GetGridListViewModelAsync(keyId));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("{id:int}")]
        virtual public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetGridModelAsync(int id)
        {
            try
            {
                return Json(await this.objManager.GetGridRowViewModelAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("{id:int}/new")]
        virtual public Microsoft.AspNetCore.Mvc.IActionResult GetGridNewModel(int id)
        {
            try
            {
                var model = this.objManager.GetGridRowNewModel();
                return Json(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("{id:int}/copy")]
        virtual public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetGridCopyModelAsync(int id)
        {
            try
            {
                return Json(await this.objManager.GetGridRowCopyModelAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("{id:int}/find")]
        virtual public Microsoft.AspNetCore.Mvc.IActionResult GetGridFindModel(int id)
        {
            try
            {
                return Json(this.objManager.GetGridRowFindModel());
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
                    return Json(await this.objManager.SaveGridRowModelAsync(model));
                }
                else
                {
                    throw new Exception($"Ошибка валидация модели в контроллере");
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
                await this.objManager.DeleteGridRowModelAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
 
        #region Select

        [HttpGet("select/{parentId:int?}/{term?}")]
        virtual public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> SelectGridFindAsync(int? parentId, string term)
        {
            try
            {
                return Json(await this.objManager.GetGridSelectListAsync(parentId, term));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("select/{id:int}")]
        virtual public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetSelectGridRowModelAsync(int id)
        {
            try
            {
                return Json(await this.objManager.GetSelectItemViewModelAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}