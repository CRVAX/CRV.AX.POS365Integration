using AXLogExtension.Common;
using CRV.AX.POS365Integration.Business.Accounts;
using CRV.AX.POS365Integration.Business.Orders;
using CRV.AX.POS365Integration.Business.Partners;
using CRV.AX.POS365Integration.Business.Products;
using CRV.AX.POS365Integration.Business.Stores;
using CRV.AX.POS365Integration.Common;
using CRV.AX.POS365Integration.Contracts;
using CRV.AX.POS365Integration.Contracts.Accounts;
using CRV.AX.POS365Integration.Contracts.OrderDetails;
using CRV.AX.POS365Integration.Contracts.Orders;
using CRV.AX.POS365Integration.Contracts.Partners;
using CRV.AX.POS365Integration.Contracts.PaymentMethods;
using CRV.AX.POS365Integration.Contracts.Products;
using CRV.AX.POS365Integration.Contracts.Stores;
using CRV.AX.POS365Integration.Interfaces.Accounts;
using CRV.AX.POS365Integration.Interfaces.Orders;
using CRV.AX.POS365Integration.Interfaces.Partners;
using CRV.AX.POS365Integration.Interfaces.Products;
using CRV.AX.POS365Integration.Interfaces.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AxFolder = CRV.AX.POS365Integration.Common.AxFolder;

namespace CRV.AX.POS365Integration
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Commented pushing Customer information code: RISK

            AxInit.Init(args);
            await AxWriteLineAndLog.LogInformationAsync($"Environment: {AxInit.AxEnvironment}");

            string csvFolder = AxFolder.GetAxFolder();
            string csvProcessedFolter = await AxFolder.GetProcessedDirectoryAsync(csvFolder);
            List<string> storeFiles = AxFolder.GetFiles(csvFolder, AxEnum.AxPOS365ExportType.Stores, "");

            #region Store
            List<StoreCSVDto> storeCSVs = new List<StoreCSVDto>();
            List<StoreInputDto> storeInputs = new List<StoreInputDto>();
            storeFiles.ForEach(x => storeCSVs.AddRange(AxCSVHelper.Convert<StoreCSVDto>(x)));
            storeCSVs.ForEach(x => storeInputs.Add(new StoreInputDto()
            {
                StoreNumber = x.StoreNumber,
                Password = x.Password,
                URL = x.URL,
                User = x.User
            }));
            #endregion Store

            IStoreBusiness storeBiz = new StoreBusiness();
            // GTF - QuangLP - Don't Sync Customer information - 2022/12/14 - DELETE - START
            // IPartnerBusiness partnerBiz = new PartnerBusiness();
            // GTF - QuangLP - Don't Sync Customer information - 2022/12/14 - DELETE - END
            IProductBusiness productBiz = new ProductBusiness();
            IAccountBusiness accountBiz = new AccountBusiness();
            IOrderBusiness orderBiz = new OrderBusiness();

            List<SuccessedPartner> partnersSuccessed = new List<SuccessedPartner>();
            List<SuccessedProduct> productsSuccessed = new List<SuccessedProduct>();
            List<SuccessedAccount> accountsSuccessed = new List<SuccessedAccount>();
            List<SuccessedOrder> ordersSuccessed = new List<SuccessedOrder>();

            foreach (StoreInputDto store in storeInputs)
            {
                POS365URL.BaseURL = store.URL;
                StoreDto storeSession = await storeBiz.LoginAsync(store);

                if (storeSession != null)
                {
                    #region Delete all
                    ////////await partnerBiz.DeleteAllAsync(storeSession);
                    ////////await productBiz.DeleteAllAsync(storeSession);
                    ////////await accountBiz.DeleteAllAsync(storeSession);
                    #endregion Delete all

                    // GTF - QuangLP - Don't Sync Customer information - 2022/12/14 - DELETE - START
                    // partnersSuccessed.AddRange(await partnerBiz.AllInOneAsync(storeSession, csvFolder));
                    // GTF - QuangLP - Don't Sync Customer information - 2022/12/14 - DELETE - END
                    productsSuccessed.AddRange(await productBiz.AllInOneAsync(storeSession, csvFolder));
                    accountsSuccessed.AddRange(await accountBiz.AllInOneAsync(storeSession, csvFolder));
                    ordersSuccessed.AddRange(await orderBiz.AllInOneAsync(storeSession, csvFolder, productsSuccessed, accountsSuccessed, partnersSuccessed));
                }
            }

            await AxWriteLineAndLog.LogInformationAsync("Done all!");
        }
    }
}
