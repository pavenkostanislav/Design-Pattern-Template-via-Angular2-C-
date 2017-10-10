using TEST.Data;
using TEST.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace TEST.Managers
{
    public class GridManager<T> : IGridManager<T> where T : class, 
                                                            Data.Interfaces.IIdModel,
                                                            Data.Interfaces.IDisplayName,
                                                            Data.Interfaces.IClearVirtualMethodsModel
    {
        public readonly CoreDbContext db;

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

        virtual public IQueryable<T> GetGridList(int? keyId = null)
        {
            return db.Set<T>().AsQueryable();
        }

        virtual public System.Threading.Tasks.Task<System.Collections.Generic.List<T>> GetGridListAsync(int? keyId = null)
        {
            return this.GetGridList(keyId).ToListAsync();
        }

        virtual public IQueryable<T> GetGridSelectList(int? keyId, string term)
        {
            var list = db.Set<T>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                list = list.Where(u => u.DisplayName.Contains(term));
            }

            return list;
        }

        virtual public System.Threading.Tasks.Task<System.Collections.Generic.List<ViewModels.SelectItemViewModel>> GetGridSelectListAsync(int? keyId, string term)
        {
            return this.GetGridSelectList(keyId, term)
                .Select(u => new ViewModels.SelectItemViewModel { id = u.Id, text = u.DisplayName })
                .OrderBy(m => m.text)
                .ToListAsync();
        }

        virtual public System.Threading.Tasks.Task<T> GetGridRowModelAsync(int id)
        {
            return db.Set<T>().FindAsync(id);
        }

        virtual public void ChangeModelSaveGridRowModel(T model)
        {
            //throw new NotImplementedException();
        }

        virtual public async System.Threading.Tasks.Task<T> SaveGridRowModelAsync(T model)
        {
            this.ChangeModelSaveGridRowModel(model);

            T e;
            if (model.Id == 0)
            {
                model.ClearVirtualMethods();
                e = db.Set<T>().Add(model).Entity;
            }
            else
            {
                model.ClearVirtualMethods();
                e = db.Set<T>().Update(model).Entity;
            }

            await db.SaveChangesAsync();

            return e;
        }

        virtual public async System.Threading.Tasks.Task DeleteGridRowModelAsync(int id)
        {
            var model = await db.Set<T>().FindAsync(id);
            if (model == null)
            {
                throw new Exception($"Запись не найдена. ({id})");
            }

            db.Set<T>().Remove(model);
            await db.SaveChangesAsync();
        }
    }
}
