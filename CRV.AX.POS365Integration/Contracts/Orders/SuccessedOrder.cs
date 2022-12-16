using CRV.AX.POS365Integration.Contracts.OrderDetails;
using CRV.AX.POS365Integration.Contracts.PaymentMethods;
using System.Collections.Generic;

namespace CRV.AX.POS365Integration.Contracts.Orders
{
    public class SuccessedOrder: BaseDto
    {
        private readonly BaseParams base_params;
        public SuccessedOrder(BaseParams _base_params) : base(_base_params) 
        {
            base_params = _base_params;
            OrderDetails = new List<SuccessedOrderDetail>();
            PaymentMethods = new List<SuccessedPaymentMethod>();
        }

        public SuccessedOrder(BaseParams _base_params, List<OrderDetailSuccessDto> _ordeDetails, List<PaymentMethodsSuccessDto> _pm) : base(_base_params)
        {
            base_params = _base_params;
            OrderDetails = new List<SuccessedOrderDetail>();
            PaymentMethods = new List<SuccessedPaymentMethod>();

            AddOrderDetails(_ordeDetails);
            AddPaymentMethods(_pm);
        }

        public string TransactionId { get; set; }

        public long? AccountId { get; set; }

        // GTF - QuangLP - Don't Sync Customer information - 2022/12/14 - DELETE - START
        // public long? PartnerId { get; set; }
        // GTF - QuangLP - Don't Sync Customer information - 2022/12/14 - DELETE - END

        public List<SuccessedOrderDetail> OrderDetails { get; set; }

        public List<SuccessedPaymentMethod> PaymentMethods { get; set; }

        private void AddOrderDetails(List<OrderDetailSuccessDto> _ordeDetails)
        {
            _ordeDetails.ForEach(x => OrderDetails.Add(new SuccessedOrderDetail(new BaseParams(base_params.Session, x.AXId, x.StoreNumber, x.FileName)) { ProductId = x.ProductId }));
        }

        private void AddPaymentMethods(List<PaymentMethodsSuccessDto> _pm)
        {
            _pm.ForEach(x => PaymentMethods.Add(new SuccessedPaymentMethod(new BaseParams(base_params.Session, x.AXId, x.StoreNumber, x.FileName)) { AccountId = x.AccountId }));
        }
    }
}
