using CRV.AX.POS365Integration.Contracts.OrderDetails;
using CRV.AX.POS365Integration.Contracts.PaymentMethods;
using System.Collections.Generic;

namespace CRV.AX.POS365Integration.Contracts.Orders
{
    /// <summary>
    /// This is Retail Store Transactions.
    /// <para>Response class, which receive information from POS365</para>
    /// <para>POS365 = Order</para>
    /// <para>GTF = Transactions</para>
    /// </summary
    public class OrderSuccessDto: BaseDto
    {
        public OrderSuccessDto (BaseParams _base_params) : base (_base_params) 
        {
            OrderDetails = new List<OrderDetailSuccessDto>();
            PaymentMethods = new List<PaymentMethodsSuccessDto>();
        }

        public string Message { get; set; }

        public string Code { get; set; }

        public List<OrderDetailSuccessDto> OrderDetails { get; set; }

        public List<PaymentMethodsSuccessDto> PaymentMethods { get; set; }
    }
}
