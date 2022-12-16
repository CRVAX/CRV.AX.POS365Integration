namespace CRV.AX.POS365Integration.Contracts.Accounts
{
    /// <summary>
    /// This is Payment method.
    /// <para>Creation of class, passed to API push to POS365</para>
    /// <para>POS365 = Account</para>
    /// <para>GTF = Tender</para>
    /// </summary
    public class AccountCreateDto : BaseRequestDto
    {
        public AccountCreateDto(BaseParams _base_params) : base(_base_params)
        {
            Account = new AccountInfomationCreateDto();
        }

        public AccountInfomationCreateDto Account { get; set; }
    }

    public class AccountInfomationCreateDto
    {
        public string Name { get; set; }
    }
}
