using AXLogExtension.Common;
using CRV.AX.POS365Integration.Common;
using CRV.AX.POS365Integration.Contracts;
using CRV.AX.POS365Integration.Contracts.Accounts;
using CRV.AX.POS365Integration.Contracts.Stores;
using CRV.AX.POS365Integration.Interfaces.Accounts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AxFolder = CRV.AX.POS365Integration.Common.AxFolder;
using TextFileHelper = CRV.AX.POS365Integration.Common.TextFileHelper;

namespace CRV.AX.POS365Integration.Business.Accounts
{
    public class AccountBusiness: CRVBase, IAccountBusiness
    {
        public async Task<(AccountSuccessDto, AccountFailDto, int)> CreateAsync(AccountCreateDto input)
        {
            try
            {
                var result = await CallAPI<AccountSuccessDto, AccountFailDto, AccountCreateDto>(POS365URL.Account.Create, HttpMethod.Post, input.Session, input);

                if (result.Item2 == AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE)
                {
                    AccountSuccessDto account = result.Item1 as AccountSuccessDto;
                    account.AXId = input.AXId;
                    account.StoreNumber = input.StoreNumber;

                    return (account, null, result.Item2);
                }
                else
                {
                    AccountFailDto failAccount = (result.Item1 as AccountFailDto);
                    failAccount.Id = 0;
                    failAccount.AXId = input.AXId;
                    failAccount.StoreNumber = input.StoreNumber;

                    return (null, failAccount, result.Item2);
                }
            }
            catch (Exception ex)
            {
                await AxWriteLineAndLog.WriteException(nameof(AccountBusiness), nameof(CreateAsync), JsonConvert.SerializeObject(input), ex);
                return (null, null, AxConstants.AX_API_RESULT_EXCEPTION_STATUS_CODE);
            }
        }

        public async Task<(AccountSuccessDto, AccountFailDto, int)> UpdateAsync(AccountUpdateDto input)
        {
            try
            {
                var result = await CallAPI<AccountSuccessDto, AccountFailDto, AccountUpdateDto>(POS365URL.Account.Update, HttpMethod.Post, input.Session, input);

                if (result.Item2 == AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE)
                {
                    AccountSuccessDto account = result.Item1 as AccountSuccessDto;
                    account.AXId = input.AXId;
                    account.StoreNumber = input.StoreNumber;

                    return (account, null, result.Item2);
                }
                else
                {
                    AccountFailDto failAccount = (result.Item1 as AccountFailDto);
                    failAccount.AXId = input.AXId;
                    failAccount.StoreNumber = input.StoreNumber;

                    return (null, failAccount, result.Item2);
                }
            }
            catch (Exception ex)
            {
                await AxWriteLineAndLog.WriteException(nameof(AccountBusiness), nameof(UpdateAsync), JsonConvert.SerializeObject(input), ex);
                return (null, null, AxConstants.AX_API_RESULT_EXCEPTION_STATUS_CODE);
            }
        }

        public async Task<List<SuccessedAccount>> AllInOneAsync(StoreDto _storeSession, string _csvFolder)
        {
            List<SuccessedAccount> accountsSuccessed = new List<SuccessedAccount>();
            List<AccountCSVDto> accounts = new List<AccountCSVDto>();
            List<string> accountFiles = AxFolder.GetFiles(_csvFolder, AxEnum.AxPOS365ExportType.Tenders, _storeSession.StoreNumber);
            accountFiles.ForEach(file => accounts.AddRange(AxCSVHelper.Convert<AccountCSVDto>(file)));

            foreach (AccountCSVDto account in accounts)
            {
                if (account.Id == 0)
                {
                    AccountCreateDto accountCreateInput = new AccountCreateDto(new BaseParams(_storeSession.SessionId, account.AXId, account.StoreNumber, account.FileName));
                    accountCreateInput.Account.Name = account.Name;

                    var result = await CreateAsync(accountCreateInput);
                    if (result.Item3 == AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE)
                    {
                        AccountSuccessDto okAccount = result.Item1;
                        accountsSuccessed.Add(new SuccessedAccount(new BaseParams(_storeSession.SessionId, okAccount.AXId, okAccount.StoreNumber, okAccount.Id, okAccount.FileName))
                        {
                            TenderTypeId = account.TenderTypeId
                        });
                    }
                }
                else
                {
                    AccountUpdateDto accountUpdateInput = new AccountUpdateDto(new BaseParams(_storeSession.SessionId, account.AXId, account.StoreNumber, account.FileName));
                    accountUpdateInput.Account.Id = account.Id;
                    accountUpdateInput.Account.Name = account.Name;

                    var result = await UpdateAsync(accountUpdateInput);
                    if (result.Item3 == AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE)
                    {
                        AccountSuccessDto okAccount = result.Item1;
                        accountsSuccessed.Add(new SuccessedAccount(new BaseParams(_storeSession.SessionId, okAccount.AXId, okAccount.StoreNumber, okAccount.Id, okAccount.FileName))
                        {
                            TenderTypeId = account.TenderTypeId
                        });
                    }
                }
            }

            await SaveToFileAsync(accountsSuccessed);
            return accountsSuccessed;
        }

        public async Task<(AccountDeleteSuccessDto, AccountFailDto, int)> DeleteAsync(AccountDeleteDto input)
        {
            string newURI = string.Format(POS365URL.Account.Delete, input.Id);
            var result = await CallAPI<AccountDeleteSuccessDto, AccountFailDto, AccountDeleteDto>(newURI, HttpMethod.Delete, input.Session, input);

            if (result.Item2 == AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE)
            {
                AccountDeleteSuccessDto Account = result.Item1 as AccountDeleteSuccessDto;
                return (Account, null, result.Item2);
            }
            else
            {
                AccountFailDto failAccount = (result.Item1 as AccountFailDto);
                return (null, failAccount, result.Item2);
            }
        }

        public async Task DeleteAllAsync(StoreDto _storeSession, int countToDelete = 100)
        {
            AccountGetDto input = null; // Empty input for fixing ambiguous between class and List of Class

            string newURI = string.Format(POS365URL.Account.Get, countToDelete);
            var result = await CallAPI<List<AccountGetSuccessDto>, AccountFailDto, AccountGetDto>(newURI, HttpMethod.Get, _storeSession.SessionId, input);

            if (result.Item2 == AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE)
            {
                List<AccountGetSuccessDto> AccountResults = (result.Item1 as List<AccountGetSuccessDto>);
                if (AccountResults != null && AccountResults.Count > 0)
                {
                    foreach (AccountGetSuccessDto Account in AccountResults)
                    {
                        AccountDeleteDto deleteAccount = new AccountDeleteDto(new BaseParams())
                        {
                            Id = Account.Id,
                            Session = _storeSession.SessionId
                        };
                        await DeleteAsync(deleteAccount);
                    }
                }
            }
        }

        public async Task SaveToFileAsync(List<SuccessedAccount> inputs)
        {
            await Task.Run(() =>
            {
                var fileNames = inputs.GroupBy(x => x.FileName).Select(g => new { FileName = g.Key }).ToList();
                fileNames.ForEach(async s =>
                {
                    List<SuccessedAccount> filterdAccounts = inputs.Where(x => x.FileName == s.FileName).ToList();
                    AxCSVHelper.SaveToFile(filterdAccounts, $"{(await AxFolder.GetSucceededDirectoryAsync(AxFolder.GetAxFolder()))}\\{s.FileName}");
                    await TextFileHelper.AxFastRenameAsync($"{AxFolder.GetAxFolder()}\\{s.FileName}");
                });
            });
        }
    }
}
