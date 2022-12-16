namespace CRV.AX.POS365Integration.Contracts.Accounts
{
    public class AccountGetDto : BaseRequestDto
    {
        public AccountGetDto(BaseParams _base_params) : base(_base_params) { }

        public int Type { get; set; }

        public int Top { get; set; }
    }
}
