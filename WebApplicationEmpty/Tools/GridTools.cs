using System.Linq;
using System.Reflection;

namespace KPMA.Tools
{
    public static class GridTools
    {
        public static System.Collections.Generic.IList<To> ConvertList<From, To>(System.Collections.Generic.IList<From> source)
            where From : Data.Interfaces.IClearVirtualPropertiesModel
            where To : Data.Interfaces.IIdModel, new()
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
    }
}