using System;

namespace CRV.AX.POS365Integration.Contracts.Orders
{
    public class OrderCSVDto: BaseDto
    {
        public OrderCSVDto() : base() { }
        public OrderCSVDto(BaseParams _base_params) : base(_base_params) { }

        public string CustAccount { get; set; }

        /// <summary>
        /// TransaciontId
        /// </summary>
        public string Code { get; set; }

        public string Description { get; set; }

        public long? AccountId { get; set; }

        /// <summary>
        /// RetailTransactionTable.PaymentAmount
        /// </summary>
        public double AmountReceived { get; set; }

        public double Discount { get; set; }

        public long? PartnerId { get; set; }

        public DateTime PurchaseDate { get; set; }

        public long? SoldById { get; set; }

        public double Total { get; set; }

        public double TotalPayment { get; set; }

        public double VAT { get; set; }

        public string VATRates { get; set; }        
    }
}
