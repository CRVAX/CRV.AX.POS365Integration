namespace CRV.AX.POS365Integration.Contracts.Products
{
    public class ProductGetDto: BaseRequestDto
    {
        public ProductGetDto(BaseParams _base_params) : base(_base_params) { }

        public int Type { get; set; }

        public int Top { get; set; }
    }
}
