using TEST.Managers;
using System;

namespace TEST.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class GridController<T,U> : Controller, IGridController<T> where T : class,
                                                                                Data.Interfaces.IIdModel,
                                                                                Data.Interfaces.IDisplayName,
                                                                                Data.Interfaces.IClearVirtualMethodsModel
                                                                                , new()
    {
        protected readonly IGridManager<T> objManager;
        protected readonly ICoreManager coreManager;
        protected readonly IMetaObjectManager moManager;
        protected readonly IAttachmentManager attManager;
        protected readonly IConstantManager constManager;
        protected readonly IEmployeeManager employeeManager;

        public readonly MetaObject tableModel;

        public GridController(  IGridManager<T> objManager
                                        )
        {
            this.objManager = objManager;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("list/{keyId:int?}")]
        virtual public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetGridListAsync(int? keyId)
        {
            try
            {
                var list = await objManager.GetGridListAsync(keyId);

                var resultjson = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<U>>(Newtonsoft.Json.JsonConvert.SerializeObject(list));

                return Json(resultjson);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}/{mode?}")]
        virtual public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetGridRowModelAsync(int id, TEST.Enums.ModeEnum? mode)
        {
            try
            {
                if (mode == null)
                {
                    mode = TEST.Enums.ModeEnum.@default;
                }

                T model = default(T);

                switch (mode)
                {
                    case TEST.Enums.ModeEnum.viewonly:
                    case TEST.Enums.ModeEnum.@default:
                        {
                            model = await objManager.GetGridRowModelAsync(id);

                            if (model == null)
                            {
                                throw new Exception($"Ошибка получения записи из базы ({id})");
                            }

                        }
                        break;
                    case TEST.Enums.ModeEnum.@new:

                        model = new T();
                        break;
                    case TEST.Enums.ModeEnum.copy:
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

        [HttpPost()]
        virtual public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> SaveGridRowModelAsync([FromBody] T model)
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
                    return Json(new ViewModels.SelectItemViewModel { id = model.Id, text = model.DisplayName });
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
