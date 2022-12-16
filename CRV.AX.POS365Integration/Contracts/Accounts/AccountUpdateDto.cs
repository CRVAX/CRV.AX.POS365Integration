using System;

namespace CRV.AX.POS365Integration.Contracts.Accounts
{
    /// <summary>
    /// Update account information
    /// <para>GTF = TenderType</para>
    /// </summary>
    public class AccountUpdateDto: BaseRequestDto
    {
        public AccountUpdateDto(BaseParams _base_params) : base(_base_params)
        {
            Account = new AccountInformationUpdateDto();
        }

        public AccountInformationUpdateDto Account { get; set; }
    }

    public class AccountInformationUpdateDto
    {
        public long? Id { get; set; }

        public string Name { get; set; }
    }
}
