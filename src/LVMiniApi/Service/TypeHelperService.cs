using System.Reflection;

namespace LVMiniApi.Service
{
    internal class TypeHelperService : ITypeHelperService
    {
        public bool TypeHasProperties<T>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            // the fields are separated by ",", so we split it
            var fieldsAfterSplit = fields.Split(',');

            // check if the requested fields exit on the source
            foreach (var field in fieldsAfterSplit)
            {
                // trim each field as it might contain leading or trailing spaces
                var propertyName = field.Trim();

                // check if the property can be found on T
                var propertyInfo = typeof(T)
                    .GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                // it can't be found, return false
                if (propertyInfo == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
