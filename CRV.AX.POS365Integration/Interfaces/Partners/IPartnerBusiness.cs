using CRV.AX.POS365Integration.Contracts.Partners;
using CRV.AX.POS365Integration.Contracts.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRV.AX.POS365Integration.Interfaces.Partners
{
    public interface IPartnerBusiness
    {
        Task<(PartnerSuccessDto, PartnerFailDto, int)> CreateAsync(PartnerCreateDto input);

        Task<(PartnerSuccessDto, PartnerFailDto, int)> UpdateAsync(PartnerUpdateDto input);

        Task<List<SuccessedPartner>> AllInOneAsync(StoreDto _storeSession, string _csvFolder);

        Task<(PartnerDeleteSuccessDto, PartnerFailDto, int)> DeleteAsync(PartnerDeleteDto input);

        Task DeleteAllAsync(StoreDto _storeSession, int countToDelete = 100);

        Task SaveToFileAsync(List<SuccessedPartner> inputs);
    }
}
