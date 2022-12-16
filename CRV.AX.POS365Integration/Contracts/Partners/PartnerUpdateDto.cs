using System;

namespace CRV.AX.POS365Integration.Contracts.Partners
{
    /// <summary>
    /// Update partner information
    /// <para>GTF = Customer</para>
    /// </summary>
    public class PartnerUpdateDto: BaseRequestDto
    {
        public PartnerUpdateDto(BaseParams _base_params) : base(_base_params)
        {
            Partner = new PartnerInformationUpdateDto();
        }

        public PartnerInformationUpdateDto Partner { get; set; }
    }

    public class PartnerInformationUpdateDto
    {
        public long? Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }
    }
}
