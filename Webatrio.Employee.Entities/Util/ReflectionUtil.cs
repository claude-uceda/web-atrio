using System.Collections;
using System.Reflection;

namespace Webatrio.Employee.Entities.Util
{
    public class ReflectionUtil
    {
        public static object Clone(object item)
        {
            var type = item.GetType();
            var copy = Activator.CreateInstance(item.GetType(), null);

            var properties = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.CanWrite && !x.PropertyType.IsInterface && !x.PropertyType.IsPointer)
            ;

            foreach (var propertyInfo in properties)
            {
                object value = propertyInfo.GetValue(item);
                if (value == null)
                    continue;

                bool isBasicType = propertyInfo.PropertyType.IsPrimitive || value is string;
                if (isBasicType)
                    propertyInfo.SetValue(copy, value);
                else
                {
                    if (propertyInfo.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
                    {
                        //todo replicate collection and all items in it
                        propertyInfo.SetValue(copy, value);
                    }
                    else
                    {
                        propertyInfo.SetValue(copy, Clone(value));
                    }
                }
            }

            return copy;
        }
    }
}
