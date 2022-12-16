using CRV.AX.POS365Integration.Contracts.Accounts;
using CRV.AX.POS365Integration.Contracts.Orders;
using CRV.AX.POS365Integration.Contracts.Partners;
using CRV.AX.POS365Integration.Contracts.Products;
using CRV.AX.POS365Integration.Contracts.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRV.AX.POS365Integration.Interfaces.Orders
{
    public interface IOrderBusiness
    {
        Task<(OrderSuccessDto, OrderFailDto, int)> CreateAsync(OrderCreateDto input);

        Task<List<SuccessedOrder>> AllInOneAsync(StoreDto _storeSession, string _csvFolder, List<SuccessedProduct> successedProducts, List<SuccessedAccount> successedAccounts, List<SuccessedPartner> successedPartners);

        Task SaveToFileAsync(List<SuccessedOrder> inputs);
    }
}
