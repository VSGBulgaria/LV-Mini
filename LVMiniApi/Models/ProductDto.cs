namespace LVMiniApi.Models
{
    /// <summary>
    /// Model for representing the information of a product.
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// The product code.
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Description of the product.
        /// </summary>
        public string ProductDescription { get; set; }

        /// <summary>
        /// Type of the product.
        /// </summary>
        public byte ProductType { get; set; }

        /// <summary>
        /// Shows if the product is active or not.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Shows if the product is hidden.
        /// </summary>
        public bool IsHidden { get; set; }
    }
}
