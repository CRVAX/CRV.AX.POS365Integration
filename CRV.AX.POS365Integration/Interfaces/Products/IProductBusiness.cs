using CRV.AX.POS365Integration.Contracts.Products;
using CRV.AX.POS365Integration.Contracts.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRV.AX.POS365Integration.Interfaces.Products
{
    public interface IProductBusiness
    {
        Task<(ProductSuccessDto, ProductFailDto, int)> CreateAsync(ProductCreateDto input);

        Task<(ProductSuccessDto, ProductFailDto, int)> UpdateAsync(ProductUpdateDto input);

        Task<List<SuccessedProduct>> AllInOneAsync(StoreDto _storeSession, string _csvFolder);

        Task<(ProductDeleteSuccessDto, ProductFailDto, int)> DeleteAsync(ProductDeleteDto input);

        Task DeleteAllAsync(StoreDto _storeSession, int countToDelete = 100);

        Task SaveToFileAsync(List<SuccessedProduct> inputs);
    }
}
