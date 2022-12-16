using CRV.AX.POS365Integration.Contracts.Stores;
using System.Threading.Tasks;

namespace CRV.AX.POS365Integration.Interfaces.Stores
{
    public interface IStoreBusiness
    {
        Task<StoreDto> LoginAsync(StoreInputDto input);
    }
}
    