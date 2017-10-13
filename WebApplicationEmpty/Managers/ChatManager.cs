using KPMA.Data;
using KPMA.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KPMA.Managers
{
    public class ChatManager : GridManager<Chat, ChatFindModel>
    {
        public ChatManager(CoreDbContext db) : base(db)
        { }

        override public IQueryable<Chat> GetGridList(int? authorId)
        {
            var ret = db.Set<Chat>()
                .Include(m => m.Author)
                .Where(o => o.OwnerId == CurrentUser.OwnerId)
                .AsQueryable();
            return ret;
        }

        override public void ChangeModelSaveGridRowModel(Chat model)
        {
            model.OwnerId = model.OwnerId == 0 ? this.CurrentUser.OwnerId : model.OwnerId;
        }
    }
}
