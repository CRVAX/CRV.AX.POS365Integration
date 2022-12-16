using System;

namespace CRV.AX.POS365Integration.Contracts.OrderDetails
{
    public class OrderDetailCreateDto : BaseRequestDto
    {

        public OrderDetailCreateDto(BaseParams _base_params) : base(_base_params) { }

        /// <summary>
        /// POS365 ProductId. This filed mapped when call API of POS365 returned.
        /// </summary>
        public long? ProductId { get; set; }

        /// <summary>
        /// Barcode
        /// </summary>
        public string Code { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public double Quantity { get; set; }
    }
}
