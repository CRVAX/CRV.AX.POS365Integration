namespace CRV.AX.POS365Integration.Contracts.OrderDetails
{
    public class SuccessedOrderDetail : BaseDto
    {
        public SuccessedOrderDetail(BaseParams _base_params) : base(_base_params) { }

        public long? ProductId { get; set; }
    }
}
