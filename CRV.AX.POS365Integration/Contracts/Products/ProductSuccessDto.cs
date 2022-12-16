namespace CRV.AX.POS365Integration.Contracts.Products
{
    public class ProductSuccessDto : BaseDto
    {
        public ProductSuccessDto(BaseParams _base_params) : base(_base_params) { }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Unit { get; set; }

        public double Price { get; set; }
    }
}
