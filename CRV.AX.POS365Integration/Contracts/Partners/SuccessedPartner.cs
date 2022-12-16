namespace CRV.AX.POS365Integration.Contracts.Partners
{
    public class SuccessedPartner: BaseDto
    {
        public SuccessedPartner(BaseParams _base_params) : base(_base_params) { }

        /// <summary>
        /// POS365 Code
        /// </summary>
        public string CustAccount { get; set; }
    }
}
