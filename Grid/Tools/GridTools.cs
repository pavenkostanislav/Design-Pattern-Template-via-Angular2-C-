using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Grid.Tools
{
    public static class GridTools
    {
        public static System.Collections.Generic.IList<To> ConvertList<From, To>(System.Collections.Generic.IList<From>  source = null)
            where From : Interfaces.IClearVirtualPropertiesModel
            where To : Grid.Interfaces.IIdModel, new()
        {
            if (source == null) { return null; }
            var target = new System.Collections.Generic.List<To>();
            foreach (var item in source)
            {
                To itemView = ConvertTo<From, To>(item);
                target.Add(itemView);
            }
            return target;
        }

        public static To ConvertTo<From, To>(From source)
            where From : Interfaces.IClearVirtualPropertiesModel
            where To : Grid.Interfaces.IIdModel, new()
        {
            var itemView = (To)System.Activator.CreateInstance(typeof(To), source);
            source.ClearVirtualProperties();

            foreach (var sourceProperty in source.GetType().GetProperties())
            {
                var targetProperty = itemView.GetType().GetProperty(sourceProperty.Name, BindingFlags.Public | BindingFlags.Instance);
                if (targetProperty != null && targetProperty.CanWrite)
                {
                    var GetPropValue = source.GetType().GetRuntimeProperties().FirstOrDefault(p => string.Equals(p.Name, sourceProperty.Name, System.StringComparison.OrdinalIgnoreCase))?.GetValue(source);
                    targetProperty.SetValue(itemView, GetPropValue);
                }
            }

            return itemView;
        }

        public static IQueryable<TEntity> MultipleIncludes<TEntity>(this IQueryable<TEntity> query, params string[] includes) 
            where TEntity : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }

        public static string ToUpperFirstChar(this string param)
        {
            var str2 = new System.Text.StringBuilder(param);
            int x = param[0];//получаем код ASCI
            int X = x - 32;//Разница между маленькой буквой и большой 32 символа.У заглавных букв код ASCI меньше
            str2.Replace((char)x, (char)X, 0, 1);//casting на int преобразует его в символ по таблице ASCI
            param = str2.ToString();
            return param;
        }

        public static int GetCarouselCurrentPage(int CurrentPage, int PageSize, int TotalCount)
        {
            if (CurrentPage <= 0 || TotalCount <= 0 || PageSize <= 0 || CurrentPage > TotalCount / (PageSize + 1))
            {
                return 0;
            }
            else
            {
                return CurrentPage;
            }
        }


        public static IQueryable<ViewEntity> OrderByList<ViewEntity>(this IQueryable<ViewEntity> queryable, System.Collections.Generic.List<string> OrderNamesList)
        {
            for (var i = 0; i < OrderNamesList.Count(); i++)
            {
                var isAsc = true;
                var param = OrderNamesList[i];
                var param_split = param.Split(' ');
                if (param_split.Count() == 2)
                {
                    if (param_split[1].ToUpper() == "ASC")
                    {
                        isAsc = true;
                    }
                    if (param_split[1].ToUpper() == "DESC")
                    {
                        isAsc = false;
                    }
                }
                param = param_split[0];

                var propertyInfo = typeof(ViewEntity).GetProperty(param);
                if (propertyInfo == null)
                {
                    param = param.ToUpperFirstChar();
                    propertyInfo = typeof(ViewEntity).GetProperty(param);
                }
                if (i == 0)
                {
                    if (isAsc)
                    {
                        queryable = queryable.OrderBy(x => propertyInfo.GetValue(x, null));
                    }
                    else
                    {
                        queryable = queryable.OrderByDescending(x => propertyInfo.GetValue(x, null));
                    }
                }
                else
                {
                    if (isAsc)
                    {
                        queryable = ((IOrderedQueryable<ViewEntity>)queryable).ThenBy(x => propertyInfo.GetValue(x, null)).AsQueryable();
                    }
                    else
                    {
                        queryable = ((IOrderedQueryable<ViewEntity>)queryable).ThenByDescending(x => propertyInfo.GetValue(x, null)).AsQueryable();
                    }
                }
            }

            return queryable;
        }
    }
}