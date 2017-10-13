using KPMA.Data.Models;
using KPMA.Managers;
using KPMA.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KPMA.Controllers
{
    public class ChatController : GridController<Chat, ChatViewModel, ChatFindModel>, IGridController<Chat>
    {
        public ChatController(IGridManager<Chat, ChatFindModel> objManager,
                                        ICoreManager coreManager,
                                        IAttachmentManager attManager,
                                        IMetaObjectManager moManager,
                                        IConstantManager constManager,
                                        IEmployeeManager employeeManager) : base(objManager, coreManager, attManager, moManager, constManager, employeeManager)
        {
        }

        [HttpGet("list/{authorId:int?}")]
        override public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetGridListAsync(int? authorId)
        {
            try
            {
                var perm = await coreManager.GetPermissions("Chat", User, false);
                if (!perm.CanRead)
                {
                    throw new Exception("У вас нет прав на эту страницу!");
                }
                var ret = await objManager.GetGridListAsync(authorId);
                return Json(ret.Select(m => new ChatViewModel
                {
                    TableId = this.tableModel.Id,
                    Id = m.Id,
                    AttachmentCount = attManager.GetAttachmentCount(this.tableModel.Id, m.Id),
                    AuthorId = m.AuthorId,
                    EmployeeId = employeeManager.GetEmployeeModelByUser(m.AuthorId)?.Id,
                    EmployeePhotoFileName = employeeManager.GetEmployeeModelByUser(m.AuthorId)?.PhotoFileName,
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

    }
}
