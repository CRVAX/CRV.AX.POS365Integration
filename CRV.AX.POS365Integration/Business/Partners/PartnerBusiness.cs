using AXLogExtension.Common;
using CRV.AX.POS365Integration.Common;
using CRV.AX.POS365Integration.Contracts;
using CRV.AX.POS365Integration.Contracts.Partners;
using CRV.AX.POS365Integration.Contracts.Stores;
using CRV.AX.POS365Integration.Interfaces.Partners;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AxFolder = CRV.AX.POS365Integration.Common.AxFolder;
using TextFileHelper = CRV.AX.POS365Integration.Common.TextFileHelper;

namespace CRV.AX.POS365Integration.Business.Partners
{
    public class PartnerBusiness : CRVBase, IPartnerBusiness
    {
        public async Task<(PartnerSuccessDto, PartnerFailDto, int)> CreateAsync(PartnerCreateDto input)
        {
            try
            {
                var result = await CallAPI<PartnerSuccessDto, PartnerFailDto, PartnerCreateDto>(POS365URL.Partner.Create, HttpMethod.Post, input.Session, input);

                if (result.Item2 == AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE)
                {
                    PartnerSuccessDto partner = result.Item1 as PartnerSuccessDto;
                    return (partner, null, result.Item2);
                }
                else
                {
                    PartnerFailDto failPartner = (result.Item1 as PartnerFailDto);
                    return (null, failPartner, result.Item2);
                }
            }
            catch (Exception ex)
            {
                await AxWriteLineAndLog.WriteException(nameof(PartnerBusiness), nameof(CreateAsync), JsonConvert.SerializeObject(input), ex);
                return (null, null, AxConstants.AX_API_RESULT_EXCEPTION_STATUS_CODE);
            }
        }

        public async Task<(PartnerSuccessDto, PartnerFailDto, int)> UpdateAsync(PartnerUpdateDto input)
        {
            try
            {
                var result = await CallAPI<PartnerSuccessDto, PartnerFailDto, PartnerUpdateDto>(POS365URL.Partner.Update, HttpMethod.Post, input.Session, input);

                if (result.Item2 == AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE)
                {
                    PartnerSuccessDto partner = result.Item1 as PartnerSuccessDto;
                    partner.AXId = input.AXId;
                    partner.StoreNumber = input.StoreNumber;

                    return (partner, null, result.Item2);
                }
                else
                {
                    PartnerFailDto failPartner = (result.Item1 as PartnerFailDto);
                    failPartner.AXId = input.AXId;
                    failPartner.StoreNumber = input.StoreNumber;

                    return (null, failPartner, result.Item2);
                }
            }
            catch (Exception ex)
            {
                await AxWriteLineAndLog.WriteException(nameof(PartnerBusiness), nameof(UpdateAsync), JsonConvert.SerializeObject(input), ex);
                return (null, null, AxConstants.AX_API_RESULT_EXCEPTION_STATUS_CODE);
            }
        }

        public async Task<List<SuccessedPartner>> AllInOneAsync(StoreDto _storeSession, string _csvFolder)
        {
            List<SuccessedPartner> partnersSuccessed = new List<SuccessedPartner>();
            List<PartnerCSVDto> partners = new List<PartnerCSVDto>();
            List<string> partnerFiles = AxFolder.GetFiles(_csvFolder, AxEnum.AxPOS365ExportType.Customers, _storeSession.StoreNumber);
            partnerFiles.ForEach(file => partners.AddRange(AxCSVHelper.Convert<PartnerCSVDto>(file)));

            foreach (PartnerCSVDto partner in partners)
            {
                if (partner.Id == 0)
                {
                    PartnerCreateDto partnerCreateInput = new PartnerCreateDto(new BaseParams(_storeSession.SessionId, partner.AXId, partner.StoreNumber, partner.FileName));
                    partnerCreateInput.Partner.Code = partner.Code;
                    partnerCreateInput.Partner.Name = partner.Name;
                    partnerCreateInput.Partner.Phone = partner.Phone;

                    var result = await CreateAsync(partnerCreateInput);
                    if (result.Item3 == AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE)
                    {
                        PartnerSuccessDto okPartner = result.Item1;
                        partnersSuccessed.Add(new SuccessedPartner(new BaseParams(_storeSession.SessionId, okPartner.AXId, okPartner.StoreNumber, okPartner.Id, okPartner.FileName))
                        {
                            CustAccount = partner.Code
                        });
                    }
                }
                else
                {
                    PartnerUpdateDto partnerUpdateInput = new PartnerUpdateDto(new BaseParams(_storeSession.SessionId, partner.AXId, partner.StoreNumber, partner.FileName));
                    partnerUpdateInput.Partner.Id = partner.Id;
                    partnerUpdateInput.Partner.Code = partner.Code;
                    partnerUpdateInput.Partner.Name = partner.Name;
                    partnerUpdateInput.Partner.Phone = partner.Phone;

                    var result = await UpdateAsync(partnerUpdateInput);
                    if (result.Item3 == AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE)
                    {
                        PartnerSuccessDto okPartner = result.Item1;
                        partnersSuccessed.Add(new SuccessedPartner(new BaseParams(_storeSession.SessionId, okPartner.AXId, okPartner.StoreNumber, okPartner.Id, okPartner.FileName))
                        {
                            CustAccount = partner.Code
                        });
                    }
                }
            }

            await SaveToFileAsync(partnersSuccessed);
            return partnersSuccessed;
        }

        public async Task<(PartnerDeleteSuccessDto, PartnerFailDto, int)> DeleteAsync(PartnerDeleteDto input)
        {
            string newURI = string.Format(POS365URL.Partner.Delete, input.Id);
            var result = await CallAPI<PartnerDeleteSuccessDto, PartnerFailDto, PartnerDeleteDto>(newURI, HttpMethod.Delete, input.Session, input);

            if (result.Item2 == AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE)
            {
                PartnerDeleteSuccessDto partner = result.Item1 as PartnerDeleteSuccessDto;
                return (partner, null, result.Item2);
            }
            else
            {
                PartnerFailDto failPartner = (result.Item1 as PartnerFailDto);
                return (null, failPartner, result.Item2);
            }
        }

        public async Task DeleteAllAsync(StoreDto _storeSession, int countToDelete = 100)
        {
            int deletedCount = 0;
            PartnerGetDto input = null; // Empty input for fixing ambiguous between class and List of Class

            string newURI = string.Format(POS365URL.Partner.Get, countToDelete);
            var result = await CallAPI<PartnerGetSuccessDto, PartnerFailDto, PartnerGetDto>(newURI, HttpMethod.Get, _storeSession.SessionId, input);

            if (result.Item2 == AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE)
            {
                PartnerGetSuccessDto partnerResults = (result.Item1 as PartnerGetSuccessDto);
                if (partnerResults != null && partnerResults.__Count > 0)
                {
                    foreach(PartnerGetResultDto partner in partnerResults.Results)
                    {
                        PartnerDeleteDto deletePartner = new PartnerDeleteDto(new BaseParams())
                        {
                            Id = partner.Id,
                            Session = _storeSession.SessionId
                        };
                        var deletedResult = await DeleteAsync(deletePartner);
                        deletedCount += deletedResult.Item3 == AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE ? 1 : 0;
                    }

                    countToDelete = partnerResults.__Count > countToDelete ? countToDelete : partnerResults.__Count;
                    if (countToDelete > 0 && deletedCount > 0)
                    {
                        await DeleteAllAsync(_storeSession, countToDelete);
                    }
                }
            }
        }

        public async Task SaveToFileAsync(List<SuccessedPartner> inputs)
        {
            await Task.Run(() =>
            {
                var fileNames = inputs?.GroupBy(x => x.FileName).Select(g => new { FileName = g.Key }).ToList();
                fileNames?.ForEach(async s =>
                {
                    List<SuccessedPartner> filterdPartners = inputs.Where(x => x.FileName == s.FileName).ToList();
                    AxCSVHelper.SaveToFile(filterdPartners, $"{(await AxFolder.GetSucceededDirectoryAsync(AxFolder.GetAxFolder()))}\\{s.FileName}");
                    await TextFileHelper.AxFastRenameAsync($"{AxFolder.GetAxFolder()}\\{s.FileName}");
                });
            });
        }

    }
}
