namespace LVMiniApi.Models
{
    /// <summary>
    /// Model for updating and existing product group. All fields are optional.
    /// </summary>
    public class UpdateProductGroupDto
    {
        /// <summary>
        /// The name of the group.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Weather the group is still active or not.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
