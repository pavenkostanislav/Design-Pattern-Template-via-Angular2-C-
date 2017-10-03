using KPMA.Data.Models;
using KPMA.Managers;
using KPMA.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KPMA.Controllers
{
    [Route("api/Chat")]
    [Authorize]
    public class EmployeeChatController : Controller, IGridController<Chat>
    {
        private readonly EmployeeChatManager empchatManager;
        private readonly ICoreManager coreManager;

        public EmployeeChatController(EmployeeChatManager empchatManager,
                                        ICoreManager coreManager)
        {
            this.empchatManager = empchatManager;
            this.coreManager = coreManager;
        }

        [HttpGet("list/{authorId:int?}")]
        public IActionResult GetGridList(int? authorId)
        {
            try
            {
                var perm = coreManager.GetPermissions("Chat", User, false).Result;
                if (!perm.CanRead)
                {
                    throw new Exception("У вас нет прав на эту страницу!");
                }
                var list = empchatManager.GetGridList(authorId).ToList();
                return Json(list.Select(m => new EmployeeChatViewModel
                {
                    Id = m.Id,
                    AuthorId = m.AuthorId,
                    EmployeeId = empchatManager.GetFindEmployee(m.AuthorId)?.Id,
                    EmployeePhotoFileName = empchatManager.GetFindEmployee(m.AuthorId)?.PhotoFileName,
                    PhotoSrc = string.Empty,
                    AuthorName = m.Author?.DisplayName,
                    Message = m.Message,
                    Rating = m.Rating,
                    RatedUsers = m.RatedUsers,
                    CreatedBy = m.CreatedBy,
                    CreatedDate = m.CreatedDate,
                    LastUpdatedBy = m.LastUpdatedBy,
                    LastUpdatedDate = m.LastUpdatedDate
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionTools.GetExceptionMessage(ex));
            }
        }

        [HttpGet("{id:int}/{mode?}")]
        public IActionResult GetGridRowModel(int id, Enums.ModeEnum? mode)
        {
            try
            {
                var perm = coreManager.GetPermissions("Chat", User, false).Result;

                if (!perm.CanRead)
                {
                    throw new Exception("У вас нет прав на просмотр страницы!");
                }

                if (mode == null)
                {
                    mode = Enums.ModeEnum.@default;
                }

                Chat model = null;

                switch (mode)
                {
                    case Enums.ModeEnum.viewonly:
                    case Enums.ModeEnum.@default:
                        {
                            model = empchatManager.GetGridRowModel(id);

                            if (model == null)
                            {
                                throw new Exception($"Ошибка получения записи из базы ({id})");
                            }

                        }
                        break;
                    case Enums.ModeEnum.@new:

                        model = new Chat();
                        break;
                    case Enums.ModeEnum.copy:
                        {
                            var _copy = empchatManager.GetGridRowModel(id);
                            if (_copy == null)
                            {
                                throw new Exception($"Ошибка получения записи из базы ({id})");
                            }

                            model = new Chat
                            {
                                Id = 0
                            };
                        }
                        break;
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

        [HttpPost()]
        public IActionResult SaveGridRowModel([FromBody] Chat model)
        {
            try
            {
                var perm = coreManager.GetPermissions("Chat", User, false).Result;

                if (model.Id == 0 && !perm.CanAdd)
                {
                    throw new Exception("У вас нет прав на добавление новой записи!");
                }
                if (model.Id != 0 && !perm.CanEdit)
                {
                    throw new Exception("У вас нет прав на редактирование записи!");
                }

                if (ModelState.IsValid)
                {
                    return Json(empchatManager.SaveGridRowModel(model));
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
        public IActionResult DeleteGridRowModel(int id)
        {
            try
            {
                var perm = coreManager.GetPermissions("Chat", User, false).Result;

                if (!perm.CanDelete)
                {
                    throw new Exception("У вас нет прав на удаление записи!");
                }

                empchatManager.DeleteGridRowModel(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ExceptionTools.GetExceptionMessage(ex));
            }
        }
    }
    public class EmployeeChatViewModel : Chat
    {
        public string AuthorName { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeePhotoFileName { get; set; }
        public string PhotoSrc { get; set; }
    }
}
