namespace CRV.AX.POS365Integration.Contracts.Products
{
    public class SuccessedProduct: BaseDto
    {
        public SuccessedProduct(BaseParams _base_params) : base(_base_params) { }

        public string ItemId { get; set; }
    }
}
