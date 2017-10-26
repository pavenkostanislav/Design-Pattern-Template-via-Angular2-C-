using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;

namespace KPMA.Tools
{
    public static class GridTools
    {
        public static System.Collections.Generic.IList<To> ConvertList<From, To>(System.Collections.Generic.IList<From> source)
            where From : Interfaces.IClearVirtualPropertiesModel
            where To : Grid.Interfaces.IIdModel, new()
        {
            var target = new System.Collections.Generic.List<To>();
            foreach (var item in source)
            {
                var itemView = (To)System.Activator.CreateInstance(typeof(To), item);
                item.ClearVirtualProperties();

                foreach (var sourceProperty in item.GetType().GetProperties())
                {
                    var targetProperty = itemView.GetType().GetProperty(sourceProperty.Name, BindingFlags.Public | BindingFlags.Instance);
                    if (targetProperty != null && targetProperty.CanWrite)
                    {
                        var GetPropValue = item.GetType().GetRuntimeProperties().FirstOrDefault(p => string.Equals(p.Name, sourceProperty.Name, System.StringComparison.OrdinalIgnoreCase))?.GetValue(item);
                        targetProperty.SetValue(itemView, GetPropValue);
                    }
                }
                target.Add(itemView);
            }
            return target;
        }

        public static IQueryable<TEntity> IncludeMultiple<TEntity>(this IQueryable<TEntity> query, params string[] includes) 
            where TEntity : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }
    }
}