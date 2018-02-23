namespace LVMiniApi.Service
{
    /// <summary>
    /// Contains methods for manipulating data for a given type.
    /// </summary>
    public interface ITypeHelperService
    {
        /// <summary>
        /// Checks if a given type contains the properties which were specified.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="fields">The properties/fields that were inputted.</param>
        /// <returns>True of false.</returns>
        bool TypeHasProperties<T>(string fields);
    }
}
