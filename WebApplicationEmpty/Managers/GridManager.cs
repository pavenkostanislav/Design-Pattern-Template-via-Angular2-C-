using KPMA.Data;
using KPMA.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KPMA.Managers
{
    public class GridManager<T> : IGridManager<T> where T : class, Data.Interfaces.IIdModel
    {
        protected readonly CoreDbContext db;

        public GridManager(CoreDbContext db)
        {
            this.db = db;
        }

        virtual public User CurrentUser
        {
            get
            {
                return db.CurrentUser;
            }
        }
        virtual public IQueryable<T> GetGridList(int? parentlId)
        {
            return db.Set<T>().AsQueryable();
        }

        virtual public T GetGridRowModel(int id)
        {
            return db.Set<T>().Find(id);
        }

        virtual public T SaveGridRowModel(T model)
        {
            if (model.Id == 0)
            {
                this.ClearGridRowProperties(model);
                var e = db.Set<T>().Add(model).Entity;
                db.SaveChanges();
                return e;
            }
            else
            {
                this.ClearGridRowProperties(model);
                var e = db.Set<T>().Update(model).Entity;
                db.SaveChanges();
                return e;
            }
        }

        virtual public void DeleteGridRowModel(int id)
        {
            var model = db.Set<T>().Find(id);
            if (model == null)
            {
                throw new Exception($"Запись не найдена. ({id})");
            }

            db.Set<T>().Remove(model);
            db.SaveChanges();
        }

        virtual public void ClearGridRowProperties(T model)
        {
            //throw new NotImplementedException();
        }
    }
}
