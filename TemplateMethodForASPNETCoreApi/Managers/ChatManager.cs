using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace TEST.Managers
{
    public class ChatManager : GridManager<Chat, ChatFindModel>
    {
        public ChatManager(CoreDbContext db) : base(db)
        { }

        override public IQueryable<Chat> GetGridList(int? authorId)
        {
            var ret = db.Set<Chat>()
                .Include(m => m.Author)
                .AsQueryable();
            return ret;
        }
    }
}
