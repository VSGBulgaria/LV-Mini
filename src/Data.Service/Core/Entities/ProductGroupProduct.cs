namespace Data.Service.Core.Entities
{
    public class ProductGroupProduct
    {
        public int IDProduct { get; set; }
        public Product Product { get; set; }

        public int IDProductGroup { get; set; }
        public ProductGroup ProductGroup { get; set; }
    }
}
