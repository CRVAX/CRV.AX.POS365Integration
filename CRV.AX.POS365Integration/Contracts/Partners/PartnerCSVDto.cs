namespace CRV.AX.POS365Integration.Contracts.Partners
{
    public class PartnerCSVDto: BaseDto
    {
        public PartnerCSVDto() : base() { }

        public PartnerCSVDto(BaseParams _base_params) : base(_base_params) { }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }
    }
}
