using CRV.AX.POS365Integration.Contracts.Accounts;
using CRV.AX.POS365Integration.Contracts.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRV.AX.POS365Integration.Interfaces.Accounts
{
    public interface IAccountBusiness
    {
        Task<(AccountSuccessDto, AccountFailDto, int)> CreateAsync(AccountCreateDto input);

        Task<(AccountSuccessDto, AccountFailDto, int)> UpdateAsync(AccountUpdateDto input);

        Task<List<SuccessedAccount>> AllInOneAsync(StoreDto _storeSession, string _csvFolder);

        Task<(AccountDeleteSuccessDto, AccountFailDto, int)> DeleteAsync(AccountDeleteDto input);

        Task DeleteAllAsync(StoreDto _storeSession, int countToDelete = 100);

        Task SaveToFileAsync(List<SuccessedAccount> inputs);
    }
}
