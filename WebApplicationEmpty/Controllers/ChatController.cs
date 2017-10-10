using TEST.Data.Models;
using TEST.Managers;
using System;
using System.Linq;

namespace TEST.Controllers
{
    public class ChatController : GridController<Chat, ChatViewModel>, IGridController<Chat>
    {
        public ChatController(  IGridManager<Chat> objManager) : base(objManager)
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
                return BadRequest(ex.Message);
            }
        }

    }
    public class ChatViewModel : Chat
    {
        public int? TableId { get; set; }
        public int? AttachmentCount { get; set; }
        public string AuthorName { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeePhotoFileName { get; set; }
        public string PhotoSrc { get; set; }
    }
}
