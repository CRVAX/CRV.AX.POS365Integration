namespace CRV.AX.POS365Integration.Contracts.Products
{
    /// <summary>
    /// Post product to POS365 to create new
    /// </summary>
    public class ProductCreateDto: BaseRequestDto
    {
        public ProductCreateDto(BaseParams _base_params) : base(_base_params)
        {
            Product = new ProductInfomationCreateDto();
        }

        public ProductInfomationCreateDto Product { get; set; }
    }

    public class ProductInfomationCreateDto
    {
        /// <summary>
        /// This properties create as default Value is 1
        /// </summary>
        public int ProductType { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string Unit { get; set; }

        public int OnHand { get; set; }

        public ProductInfomationCreateDto()
        {
            ProductType = 1;
            OnHand = 999999;
        }
    }
}
