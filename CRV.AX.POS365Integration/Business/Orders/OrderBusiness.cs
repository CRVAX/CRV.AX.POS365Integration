using CRV.AX.POS365Integration.Common;
using CRV.AX.POS365Integration.Contracts;
using CRV.AX.POS365Integration.Contracts.Accounts;
using CRV.AX.POS365Integration.Contracts.OrderDetails;
using CRV.AX.POS365Integration.Contracts.Orders;
using CRV.AX.POS365Integration.Contracts.Partners;
using CRV.AX.POS365Integration.Contracts.PaymentMethods;
using CRV.AX.POS365Integration.Contracts.Products;
using CRV.AX.POS365Integration.Contracts.Stores;
using CRV.AX.POS365Integration.Interfaces.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CRV.AX.POS365Integration.Business.Orders
{
    public class OrderBusiness: CRVBase, IOrderBusiness
    {
        public async Task<(OrderSuccessDto, OrderFailDto, int)> CreateAsync(OrderCreateDto input)
        {
            var result = await CallAPI<OrderSuccessDto, OrderFailDto, OrderCreateDto>(POS365URL.Order.Create, HttpMethod.Post, input.Session, input);

            if (result.Item2 == AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE)
            {
                OrderSuccessDto order = result.Item1 as OrderSuccessDto;
                order.AXId = input.AXId;
                order.StoreNumber = input.StoreNumber;

                input.Order.OrderDetails.ForEach(d =>
                {
                    order.OrderDetails.Add
                    (
                        new OrderDetailSuccessDto(new BaseParams(input.Session, d.AXId, d.StoreNumber, d.FileName))
                        { 
                            ProductId = d.ProductId 
                        }
                    );
                });

                input.Order.TempPaymentMethodInformation?.ForEach(p =>
                {
                    order.PaymentMethods.Add
                    (
                        new PaymentMethodsSuccessDto(new BaseParams(input.Session, p.AXId, p.StoreNumber, p.FileName))
                        {
                            AccountId = p.AccountId
                        }
                    );
                });

                return (order, null, result.Item2);
            }
            else
            {
                OrderFailDto failAccount = (result.Item1 as OrderFailDto);
                failAccount.AXId = input.AXId;
                failAccount.StoreNumber = input.StoreNumber;

                return (null, failAccount, result.Item2);
            }
        }

        public async Task<List<SuccessedOrder>> AllInOneAsync
        (
            StoreDto _storeSession, 
            string _csvFolder, 
            List<SuccessedProduct> successedProducts, 
            List<SuccessedAccount> successedAccounts, 
            List<SuccessedPartner> successedPartners
        )
        {
            List<SuccessedOrder> ordersSuccessed = new List<SuccessedOrder>();
            List<OrderCSVDto> orders = new List<OrderCSVDto>();
            List<OrderDetailCSVDto> orderDetails = new List<OrderDetailCSVDto>();
            List<PaymentMethodCSVDto> paymentMethods = new List<PaymentMethodCSVDto>();

            List<string> orderFiles = AxFolder.GetFiles(_csvFolder, AxEnum.AxPOS365ExportType.Transactions, _storeSession.StoreNumber);
            List<string> orderDetailFiles = AxFolder.GetFiles(_csvFolder, AxEnum.AxPOS365ExportType.TransactionSales, _storeSession.StoreNumber);
            List<string> paymentMethodFiles = AxFolder.GetFiles(_csvFolder, AxEnum.AxPOS365ExportType.PaymentTrans, _storeSession.StoreNumber);

            orderFiles.ForEach(file =>
                orders.AddRange(AxCSVHelper.Convert<OrderCSVDto>(file))
            );

            orderDetailFiles.ForEach(file =>
                orderDetails.AddRange(AxCSVHelper.Convert<OrderDetailCSVDto>(file))
            );

            paymentMethodFiles.ForEach(file => 
                paymentMethods.AddRange(AxCSVHelper.Convert<PaymentMethodCSVDto>(file))
            );

            foreach (OrderCSVDto order in orders)
            {
                if (order.Id == 0)
                {
                    OrderCreateDto orderCreateInput = new OrderCreateDto(new BaseParams(_storeSession.SessionId, order.AXId, order.StoreNumber, order.FileName));
                    orderCreateInput.Order.AmountReceived = order.AmountReceived;
                    orderCreateInput.Order.Code = order.Code;
                    orderCreateInput.Order.Description = order.Description;
                    orderCreateInput.Order.Discount = order.Discount;
                    orderCreateInput.Order.PurchaseDate = order.PurchaseDate.ToUniversalTime();
                    orderCreateInput.Order.SoldById = order.SoldById;
                    orderCreateInput.Order.Total = order.Total;
                    orderCreateInput.Order.TotalPayment = order.TotalPayment;
                    orderCreateInput.Order.VAT = order.VAT;
                    orderCreateInput.Order.VATRates = order.VATRates;

                    #region Order details
                    orderDetails.Where(x => x.TransactionId == order.Code)
                                .ToList()
                                .ForEach(od =>
                                {
                                    od.ProductId = successedProducts.Where(prod => prod.ItemId == od.Code).FirstOrDefault()?.Id ?? od.ProductId;
                                    orderCreateInput.Order.AddOrderDetail(od);
                                });
                    #endregion Order details

                    // GTF - QuangLP - Don't Sync Customer information - 2022/12/14 - DELETE - START
                    //#region Partner aka CustAccount
                    //order.PartnerId = order.PartnerId == 0
                    //                    ? (successedPartners.Where(cust => cust.CustAccount == order.CustAccount)
                    //                                        .FirstOrDefault()?.Id ?? 0
                    //                      )
                    //                    : order.PartnerId;

                    //orderCreateInput.Order.PartnerId = order.PartnerId;
                    //#endregion Parner aka CustAccount
                    // GTF - QuangLP - Don't Sync Customer information - 2022/12/14 - DELETE - END

                    #region Account aka PaymentMethods
                    paymentMethods.Where(pm => pm.TransactionId == order.Code)
                                  .ToList()?
                                  .ForEach(pm =>
                                  {
                                      pm.AccountId = successedAccounts.FirstOrDefault(acc => acc.TenderTypeId == pm.TenderTypeId)?.Id ?? pm.AccountId;
                                      orderCreateInput.Order.AddPaymentMethods(pm);
                                  });

                    List<PaymentMethodCSVDto> pmByTransactions = paymentMethods.Where(pm => pm.TransactionId == order.Code).ToList();

                    order.AccountId = pmByTransactions?.Count > 1 ?
                                        pmByTransactions.FirstOrDefault(f => f.Value == Convert.ToInt64(pmByTransactions.Max(m => m.Value))).AccountId
                                        : order.AccountId;

                    orderCreateInput.Order.AccountId = order.AccountId;
                    #endregion Account aka PaymentMethods

                    var result = await CreateAsync(orderCreateInput);
                    if (result.Item3 == AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE)
                    {
                        OrderSuccessDto okOrder = result.Item1;
                        ordersSuccessed.Add(
                            new SuccessedOrder(
                                new BaseParams(_storeSession.SessionId, okOrder.AXId, okOrder.StoreNumber, okOrder.Id, okOrder.FileName),
                                    okOrder.OrderDetails,
                                    okOrder.PaymentMethods
                            )
                            {
                                TransactionId = okOrder.Code,
                                AccountId = orderCreateInput.Order.AccountId,
                                // GTF - QuangLP - Don't Sync Customer information - 2022/12/14 - DELETE - START
                                // PartnerId = orderCreateInput.Order.PartnerId
                                // GTF - QuangLP - Don't Sync Customer information - 2022/12/14 - DELETE - END
                            }
                        );                        
                    }
                }
            }

            await SaveToFileAsync(ordersSuccessed);
            return ordersSuccessed;
        }

        public async Task SaveToFileAsync(List<SuccessedOrder> inputs)
        {
            List<string> orderFileNames = inputs.GroupBy(x => x.FileName).Select(g => g.Key).Distinct().ToList();
            foreach(string s in orderFileNames)
            {
                List<SuccessedOrder> filterdOrders = inputs.Where(x => x.FileName == s).ToList();
                AxCSVHelper.SaveToFile(filterdOrders, $"{(AxFolder.GetSucceededDirectoryAsync(AxFolder.GetAxFolder()).Result)}\\{s}");
                await TextFileHelper.AxFastRenameAsync($"{AxFolder.GetAxFolder()}\\{s}");
            }

            List<string> orderDetailFileNames = new List<string>();
            inputs.Select(x => x.OrderDetails).ToList().ForEach(x => orderDetailFileNames.AddRange(x.GroupBy(p => p.FileName).Select(g => g.Key).ToList()));
            orderDetailFileNames = orderDetailFileNames.Distinct().ToList();
            foreach (string s in orderDetailFileNames)
            {
                List<SuccessedOrderDetail> filterdOrderDetails = new List<SuccessedOrderDetail>();
                inputs.Select(x => x.OrderDetails).ToList().ForEach(d => filterdOrderDetails.AddRange(d.Where(f => f.FileName == s).ToList()));
                AxCSVHelper.SaveToFile(filterdOrderDetails, $"{AxFolder.GetSucceededDirectoryAsync(AxFolder.GetAxFolder()).Result}\\{s}");
                await TextFileHelper.AxFastRenameAsync($"{AxFolder.GetAxFolder()}\\{s}");
            }

            List<string> paymentMethodsFileNames = new List<string>();
            inputs.Select(x => x.PaymentMethods).ToList().ForEach(x => paymentMethodsFileNames.AddRange(x.GroupBy(g => g.FileName).Select(g => g.Key).ToList()));
            paymentMethodsFileNames = paymentMethodsFileNames.Distinct().ToList();
            foreach(string s in paymentMethodsFileNames)
            {
                List<SuccessedPaymentMethod> filterdPaymentMethods = new List<SuccessedPaymentMethod>();
                inputs.Select(x => x.PaymentMethods).ToList().ForEach(p => filterdPaymentMethods.AddRange(p.Where(f => f.FileName == s).ToList()));
                AxCSVHelper.SaveToFile(filterdPaymentMethods, $"{AxFolder.GetSucceededDirectoryAsync(AxFolder.GetAxFolder()).Result}\\{s}");
                await TextFileHelper.AxFastRenameAsync($"{AxFolder.GetAxFolder()}\\{s}");
            }
        }
    }
}
