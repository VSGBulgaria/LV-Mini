namespace LVMiniApi.Helpers
{
    /// <summary>
    /// Parameters used for manipulating product group data
    /// through the query string like filtering, data shaping, etc.
    /// </summary>
    public class ProductGroupResourceParameters
    {
        /// <summary>
        /// Through this you can specify what fields you want
        /// from the given resource.
        /// </summary>
        public string Fields { get; set; }
    }
}
