using System.Collections.Generic;

namespace CRV.AX.POS365Integration.Contracts.PaymentMethods
{
    public class PaymentMethodCreateDto
    {
        public List<PaymentMethodInformationCreateDto> PaymentMethods { get; set; }

        public PaymentMethodCreateDto()
        {
            PaymentMethods = new List<PaymentMethodInformationCreateDto>();
        }
    }

    public class PaymentMethodInformationCreateDto
    {
        public long? AccountId { get; set; }

        public double Value { get; set; }
    }

    public class TempPaymentMethodInformationCreateDto : BaseRequestDto
    {
        public TempPaymentMethodInformationCreateDto(BaseParams _base_params) : base(_base_params) { }

        public long? AccountId { get; set; }

        public double Value { get; set; }
    }
}
