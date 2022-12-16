namespace CRV.AX.POS365Integration.Contracts.Partners
{
    public class PartnerGetDto: BaseRequestDto
    {
        public PartnerGetDto(BaseParams _base_params) : base(_base_params) { }

        public int Type { get; set; }

        public int Top { get; set; }
    }
}
