namespace CRV.AX.POS365Integration.Contracts.Products
{
    public class ProductCSVDto: BaseDto
    {
        public ProductCSVDto() : base() { }

        public ProductCSVDto(BaseParams _base_params) : base (_base_params) { }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Unit { get; set; }

        public double Price { get; set; }
    }
}
