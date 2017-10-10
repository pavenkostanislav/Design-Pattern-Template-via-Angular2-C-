using TEST.Data.Models;
using System.Linq;

namespace TEST.Managers
{
    public class ChatManager : GridManager<Chat>, IGridManager<Chat>
    {
        public ChatManager(DbContext db) : base(db)
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
