namespace CRV.AX.POS365Integration.Contracts.PaymentMethods
{
    public class SuccessedPaymentMethod : BaseDto
    {
        public SuccessedPaymentMethod(BaseParams _base_params) : base(_base_params) { }

        public long? AccountId { get; set; }
    }
}
