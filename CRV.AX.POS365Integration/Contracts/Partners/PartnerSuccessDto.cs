using System;

namespace CRV.AX.POS365Integration.Contracts.Partners
{

    public class PartnerSuccessDto: BaseDto
    {
        public PartnerSuccessDto(BaseParams _base_params) : base(_base_params) { }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }
    }
}
