namespace LVMiniApi.Models
{
    public class ProductDto
    {
        public string ProductCode { get; set; }

        public string ProductDescription { get; set; }

        public byte ProductType { get; set; }

        public bool IsActive { get; set; }

        public bool IsHidden { get; set; }
    }
}
