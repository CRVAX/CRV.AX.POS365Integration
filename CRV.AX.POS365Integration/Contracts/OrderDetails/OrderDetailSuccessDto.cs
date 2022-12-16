using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRV.AX.POS365Integration.Contracts.OrderDetails
{
    public class OrderDetailSuccessDto: BaseDto
    {
        public OrderDetailSuccessDto(BaseParams _base_params) : base(_base_params) { }

        public long? ProductId { get; set; }
    }
}
