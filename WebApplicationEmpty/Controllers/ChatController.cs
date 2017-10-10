namespace TEST.Controllers
{
    public class ChatController : GridController<TEST.Data.Models.Chat, ChatViewModel>, TEST.Controllers.IGridController<TEST.Data.Models.Chat>
    {
        public ChatController(TEST.Managers.IGridManager<TEST.Data.Models.Chat> objManager) : base(objManager)
        {
        }
    }
    public class ChatViewModel : TEST.Data.Models.Chat
    {
        public string AuthorName {
            get
            {
                return this.Author.DisplayName;
            }
        }
    }
}
