using CRV.AX.POS365Integration.Common;
using CRV.AX.POS365Integration.Contracts.Stores;
using CRV.AX.POS365Integration.Interfaces.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CRV.AX.POS365Integration.Business.Stores
{
    public class StoreBusiness: CRVBase, IStoreBusiness
    {
        public async Task<StoreDto> LoginAsync(StoreInputDto input)
        {
            var result = await CallAPI<StoreDto, StoreFailDto>(POS365URL.Store.Login, HttpMethod.Get, input);

            return (result.Item1 as StoreDto);
        }
    }
}
