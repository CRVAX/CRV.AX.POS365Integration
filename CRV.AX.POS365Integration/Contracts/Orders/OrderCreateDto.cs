using CRV.AX.POS365Integration.Contracts.OrderDetails;
using CRV.AX.POS365Integration.Contracts.PaymentMethods;
using System;
using System.Collections.Generic;

namespace CRV.AX.POS365Integration.Contracts.Orders
{
    public class OrderCreateDto: BaseRequestDto
    {
        public OrderCreateDto(BaseParams _base_params) : base(_base_params)
        {
            Order = new OrderInformationCreateDto(_base_params);
        }

        public OrderInformationCreateDto Order { get; set; }
    }

    public class OrderInformationCreateDto
    {
        private BaseParams base_params = new BaseParams();

        public OrderInformationCreateDto(BaseParams _base_params) 
        {
            base_params = _base_params;

            // Hardcode
            Status = 2;

            OrderDetails = new List<OrderDetailCreateDto>();
        }

        /// <summary>
        /// TransaciontId
        /// </summary>
        public string Code { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// RetailTransactionTable.PaymentAmount
        /// </summary>
        public double AmountReceived { get; set; }

        public double Discount { get; set; }

        public double ExcessCash { get; set; }

        public DateTime PurchaseDate { get; set; }

        public double ShippingCost { get; set; }

        public long? SoldById { get; set; }

        public int Status { get; set; }

        public double Total { get; set; }

        public double TotalAdditionalServices { get; set; }

        public double TotalPayment { get; set; }

        public double VAT { get; set; }

        public string VATRates { get; set; }

        public long? AccountId { get; set; }

        // Don't sync Customer information
        //public long? PartnerId { get; set; }

        public List<OrderDetailCreateDto> OrderDetails { get; set; }

        public void AddOrderDetail(OrderDetailCSVDto _orderDetailCSV)
        {
            BaseParams od_params = new BaseParams()
            {
                AXId = _orderDetailCSV.AXId,
                FileName = _orderDetailCSV.FileName,
                Id = _orderDetailCSV.Id,
                Session = base_params.Session,
                StoreNumber = _orderDetailCSV.StoreNumber
            };

            OrderDetails.Add(new OrderDetailCreateDto(od_params)
            {
                ProductId = _orderDetailCSV.ProductId,
                Code = _orderDetailCSV.Code,
                Name = _orderDetailCSV.Name,
                Price = _orderDetailCSV.Price,
                Quantity = _orderDetailCSV.Quantity
            });
        }

        public PaymentMethodCreateDto MoreAttributes { get; set; }

        public void AddPaymentMethods(PaymentMethodCSVDto _pm)
        {
            BaseParams pm_params = new BaseParams()
            {
                AXId = _pm.AXId,
                FileName = _pm.FileName,
                Id = _pm.Id,
                Session = base_params.Session,
                StoreNumber = _pm.StoreNumber
            };

            // For pushing to POS365 API JSON
            MoreAttributes = MoreAttributes ?? new PaymentMethodCreateDto();
            MoreAttributes.PaymentMethods = MoreAttributes.PaymentMethods ?? new List<PaymentMethodInformationCreateDto>();
            MoreAttributes.PaymentMethods.Add(new PaymentMethodInformationCreateDto()
            {
                AccountId = _pm.AccountId,
                Value = _pm.Value
            });

            // For exporting to CSV to update into DB
            TempPaymentMethodInformation = TempPaymentMethodInformation ?? new List<TempPaymentMethodInformationCreateDto>();
            TempPaymentMethodInformation.Add(new TempPaymentMethodInformationCreateDto(pm_params)
            {
                AccountId = _pm.AccountId,
                Value = _pm.Value
            });
        }

        public List<TempPaymentMethodInformationCreateDto> TempPaymentMethodInformation { get; set; }
    }
}
