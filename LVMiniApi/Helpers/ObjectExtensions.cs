using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace LVMiniApi.Helpers
{
    internal static class ObjectExtensions
    {
        public static ExpandoObject ShapeData<TSource>(this TSource source, string fields)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }

            // create a list to hold ExpandoObjects
            var dataShapedObject = new ExpandoObject();

            // create a list with PropertyInfo object on TSource.
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                // all public properties should be in the ExpandoObject
                var propertyInfos = typeof(TSource)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var propertyInfo in propertyInfos)
                {
                    // get the value of the property on the source object
                    var propertyValue = propertyInfo.GetValue(source);

                    (dataShapedObject as IDictionary<string, object>).Add(propertyInfo.Name, propertyValue);
                }

                return dataShapedObject;
            }

            var fieldsAfterSplit = fields.Split(',');
            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();
                var propertyInfo = typeof(TSource)
                    .GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo == null)
                {
                    throw new Exception($"Property {propertyName} wasn't found on {typeof(TSource)}");
                }

                var propertyValue = propertyInfo.GetValue(source);

                (dataShapedObject as IDictionary<string, object>).Add(propertyInfo.Name, propertyValue);
            }

            return dataShapedObject;
        }
    }
}