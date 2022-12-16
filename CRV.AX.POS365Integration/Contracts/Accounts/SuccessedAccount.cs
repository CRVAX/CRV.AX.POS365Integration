namespace CRV.AX.POS365Integration.Contracts.Accounts
{
    public class SuccessedAccount: BaseDto
    {
        public SuccessedAccount(BaseParams _base_params) : base (_base_params) { }

        public string TenderTypeId { get; set; }
    }
}
