using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TEST.Models;
using TEST.Managers;
using TEST.Tools;

namespace TEST.Controllers
{
    public class ChatController : GridController<Chat, ChatViewModel, ChatFindModel>, IGridController<Chat>
    {
        public ChatController(IGridManager<Chat, ChatFindModel> objManager) : base(objManager)
        {
        }

        [HttpGet("list/{authorId:int?}")]
        override public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetGridListAsync(int? authorId)
        {
            try
            {
                var ret = await objManager.GetGridListAsync(authorId);
                return Json(ret.Select(m => new Models.ChatViewModel
                {
                    TableId = this.tableName,
                    Id = m.Id,
                    AttachmentCount = 0,
                    AuthorId = m.AuthorId,
                    EmployeeId = 0,
                    EmployeePhotoFileName = string.Empty,
                    PhotoSrc = string.Empty,
                    AuthorName = "DisplayName",
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
                return BadRequest(ex.Message);
            }
        }

    }
}
