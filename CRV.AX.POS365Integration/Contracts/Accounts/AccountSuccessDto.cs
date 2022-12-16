namespace CRV.AX.POS365Integration.Contracts.Accounts
{
    /// <summary>
    /// This is Payment method.
    /// <para>Response class, which receive information from POS365</para>
    /// <para>POS365 = Account</para>
    /// <para>GTF = Tender</para>
    /// </summary>
    public class AccountSuccessDto: BaseDto
    {
        public AccountSuccessDto(BaseParams _base_params) : base(_base_params) { }

        public string Name { get; set; }
    }
}
