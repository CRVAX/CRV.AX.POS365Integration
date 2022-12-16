using CsvHelper.Configuration.Attributes;
using System;

namespace CRV.AX.POS365Integration.Contracts.PaymentMethods
{
    public class PaymentMethodCSVDto : BaseDto
    {
        public PaymentMethodCSVDto() : base() { }

        public PaymentMethodCSVDto(BaseParams _base_params) : base(_base_params) { }

        [Ignore]
        public override long? Id { get; set; }

        public string TransactionId { get; set; }

        public string TenderTypeId { get; set; }

        public long? AccountId { get; set; }

        public double Value { get; set; }
    }

}
