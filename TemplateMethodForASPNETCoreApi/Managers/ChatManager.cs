using Microsoft.EntityFrameworkCore;
using System.Linq;
using TEST.Models;

namespace TEST.Managers
{
    public class ChatManager : GridManager<Chat, ChatFindModel>
    {
        public ChatManager(DbContext db) : base(db)
        { }

        override public IQueryable<Chat> GetGridList(int? authorId, ChatFindModel findModel)
        {
            var ret = db.Set<Chat>()
                .AsQueryable();
            return ret;
        }
    }
}
