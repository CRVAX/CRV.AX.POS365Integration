namespace CRV.AX.POS365Integration.Contracts.Partners
{
    /// <summary>
    /// Create new Partner
    /// <para>GTF = Customer</para>
    /// </summary>
    public class PartnerCreateDto: BaseRequestDto
    {
        public PartnerCreateDto(BaseParams _base_params) : base(_base_params)
        {
            Partner = new PartnerInfomationCreateDto();
        }

        public PartnerInfomationCreateDto Partner { get; set; }
    }

    public class PartnerInfomationCreateDto
    {
        /// <summary>
        /// This properties create as default Value is 1
        /// </summary>
        public int Type { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public PartnerInfomationCreateDto()
        {
            Type = 1;
        }
    }
}
