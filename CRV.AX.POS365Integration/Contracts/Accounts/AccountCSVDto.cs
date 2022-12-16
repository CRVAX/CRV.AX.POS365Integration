namespace CRV.AX.POS365Integration.Contracts.Accounts
{
    public class AccountCSVDto: BaseDto
    {
        public AccountCSVDto() : base() { }

        public AccountCSVDto(BaseParams _base_params) : base(_base_params) { }

        public string TenderTypeId { get; set; }

        public string Name { get; set; }
    }
}
