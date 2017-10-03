using KPMA.Data;
using KPMA.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KPMA.Managers
{
    public class EmployeeChatManager : GridManager<Chat>, IGridManager<Chat>
    {
        public EmployeeChatManager(CoreDbContext db) : base(db)
        { }
        
        override public IQueryable<Chat> GetGridList(int? parentlId)
        {
            var ret = db.Set<Chat>().Include(m => m.Author).AsQueryable();
            return ret;
        }
        override public void ClearGridRowProperties(Chat model)
        {
            model.Author = null;
        }
    }
}
