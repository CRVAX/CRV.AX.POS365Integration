using CsvHelper.Configuration.Attributes;
using System;

namespace CRV.AX.POS365Integration.Contracts.OrderDetails
{
    /// <summary>
    /// This is Retail Store Transactions Sales.
    /// <para>Response class, which receive information from POS365</para>
    /// <para>POS365 = Order detail</para>
    /// <para>GTF = Transactions Sales</para>
    /// </summary
    public class OrderDetailCSVDto: BaseDto
    {
        public OrderDetailCSVDto() : base() { }

        public OrderDetailCSVDto(BaseParams _base_params) : base(_base_params) { }

        [Ignore]
        public override long? Id { get; set; }

        public string TransactionId { get; set; }

        public long? ProductId { get; set; }

        /// <summary>
        /// ItemId
        /// </summary>
        public string Code { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public double Quantity { get; set; }
    }
}
