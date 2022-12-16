using AXLogExtension.Common;
using CRV.AX.POS365Integration.Common;
using CRV.AX.POS365Integration.Contracts;
using CRV.AX.POS365Integration.Contracts.Products;
using CRV.AX.POS365Integration.Contracts.Stores;
using CRV.AX.POS365Integration.Interfaces.Products;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AxFolder = CRV.AX.POS365Integration.Common.AxFolder;
using TextFileHelper = CRV.AX.POS365Integration.Common.TextFileHelper;

namespace CRV.AX.POS365Integration.Business.Products
{
    public class ProductBusiness: CRVBase, IProductBusiness
    {
        public async Task<(ProductSuccessDto, ProductFailDto, int)> CreateAsync(ProductCreateDto input)
        {
            try
            {
                var result = await CallAPI<ProductSuccessDto, ProductFailDto, ProductCreateDto>(POS365URL.Product.Create, HttpMethod.Post, input.Session, input);

                if (result.Item2 == AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE)
                {
                    ProductSuccessDto product = result.Item1 as ProductSuccessDto;
                    product.AXId = input.AXId;
                    product.StoreNumber = input.StoreNumber;

                    return (product, null, result.Item2);
                }
                else
                {
                    ProductFailDto failProduct = (result.Item1 as ProductFailDto);
                    failProduct.AXId = input.AXId;
                    failProduct.StoreNumber = input.StoreNumber;

                    return (null, failProduct, result.Item2);
                }
            }
            catch (Exception ex)
            {
                await AxWriteLineAndLog.WriteException(nameof(ProductBusiness), nameof(CreateAsync), JsonConvert.SerializeObject(input), ex);
                return (null, null, AxConstants.AX_API_RESULT_EXCEPTION_STATUS_CODE);
            }
        }

        public async Task<(ProductSuccessDto, ProductFailDto, int)> UpdateAsync(ProductUpdateDto input)
        {
            try
            {
                var result = await CallAPI<ProductSuccessDto, ProductFailDto, ProductUpdateDto>(POS365URL.Product.Update, HttpMethod.Post, input.Session, input);

                if (result.Item2 == AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE)
                {
                    ProductSuccessDto product = result.Item1 as ProductSuccessDto;
                    product.AXId = input.AXId;
                    product.StoreNumber = input.StoreNumber;

                    return (product, null, result.Item2);
                }
                else
                {
                    ProductFailDto failProduct = (result.Item1 as ProductFailDto);
                    failProduct.AXId = input.AXId;
                    failProduct.StoreNumber = input.StoreNumber;

                    return (null, failProduct, result.Item2);
                }
            }
            catch (Exception ex)
            {
                await AxWriteLineAndLog.WriteException(nameof(ProductBusiness), nameof(UpdateAsync), JsonConvert.SerializeObject(input), ex);
                return (null, null, AxConstants.AX_API_RESULT_EXCEPTION_STATUS_CODE);
            }
        }

        public async Task<List<SuccessedProduct>> AllInOneAsync(StoreDto _storeSession, string _csvFolder)
        {
            List<SuccessedProduct> productsSuccessed = new List<SuccessedProduct>();
            List<ProductCSVDto> products = new List<ProductCSVDto>();
            List<string> productFiles = AxFolder.GetFiles(_csvFolder, AxEnum.AxPOS365ExportType.Products, _storeSession.StoreNumber);
            productFiles.ForEach(file => products.AddRange(AxCSVHelper.Convert<ProductCSVDto>(file)));

            foreach (ProductCSVDto product in products)
            {
                if (product.Id == 0)
                {
                    ProductCreateDto productCreateInput = new ProductCreateDto(new BaseParams(_storeSession.SessionId, product.AXId, product.StoreNumber, product.FileName));
                    productCreateInput.Product.Code = product.Code;
                    productCreateInput.Product.Name = product.Name;
                    productCreateInput.Product.Unit = product.Unit;
                    productCreateInput.Product.Price = product.Price;

                    var result = await CreateAsync(productCreateInput);
                    if (result.Item3 == AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE)
                    {
                        ProductSuccessDto okProduct = result.Item1;
                        productsSuccessed.Add(new SuccessedProduct(new BaseParams(_storeSession.SessionId, okProduct.AXId, okProduct.StoreNumber, okProduct.Id, okProduct.FileName))
                        {
                            ItemId = product.Code
                        });
                    }
                }
                else
                {
                    ProductUpdateDto productUpdateInput = new ProductUpdateDto(new BaseParams(_storeSession.SessionId, product.AXId, product.StoreNumber, product.FileName));
                    productUpdateInput.Product.Id = product.Id;
                    productUpdateInput.Product.Code = product.Code;
                    productUpdateInput.Product.Name = product.Name;
                    productUpdateInput.Product.Unit = product.Unit;
                    productUpdateInput.Product.Price = product.Price;

                    var result = await UpdateAsync(productUpdateInput);
                    if (result.Item3 == AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE)
                    {
                        ProductSuccessDto okProduct = result.Item1;
                        productsSuccessed.Add(new SuccessedProduct(new BaseParams(_storeSession.SessionId, okProduct.AXId, okProduct.StoreNumber, okProduct.Id, okProduct.FileName))
                        {
                            ItemId = product.Code
                        });
                    }
                }
            }

            await SaveToFileAsync(productsSuccessed);
            return productsSuccessed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns>A tuple value.
        /// <para>First is ProductDeleteSuccessDto class</para>
        /// <para>First is ProductFailDto class</para>
        /// <para>First is status value</para>
        /// </returns>
        public async Task<(ProductDeleteSuccessDto, ProductFailDto, int)> DeleteAsync(ProductDeleteDto input)
        {
            string newURI = string.Format(POS365URL.Product.Delete, input.Id);
            var result = await CallAPI<ProductDeleteSuccessDto, ProductFailDto, ProductDeleteDto>(newURI, HttpMethod.Delete, input.Session, input);

            if (result.Item2 == AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE)
            {
                ProductDeleteSuccessDto Product = result.Item1 as ProductDeleteSuccessDto;
                return (Product, null, result.Item2);
            }
            else
            {
                ProductFailDto failProduct = (result.Item1 as ProductFailDto);
                return (null, failProduct, result.Item2);
            }
        }

        public async Task DeleteAllAsync(StoreDto _storeSession, int countToDelete = 100)
        {
            int deletedCount = 0;
            ProductGetDto input = null; // Empty input for fixing ambiguous between class and List of Class

            string newURI = string.Format(POS365URL.Product.Get, countToDelete);
            var result = await CallAPI<ProductGetSuccessDto, ProductFailDto, ProductGetDto>(newURI, HttpMethod.Get, _storeSession.SessionId, input);

            if (result.Item2 == AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE)
            {
                ProductGetSuccessDto ProductResults = (result.Item1 as ProductGetSuccessDto);
                if (ProductResults != null && ProductResults.__Count > 0)
                {
                    foreach (ProductGetResultDto Product in ProductResults.Results)
                    {
                        ProductDeleteDto deleteProduct = new ProductDeleteDto(new BaseParams())
                        {
                            Id = Product.Id,
                            Session = _storeSession.SessionId
                        };
                        var deletedResult = await DeleteAsync(deleteProduct);
                        deletedCount += deletedResult.Item3 == AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE ? 1 : 0;
                    }

                    countToDelete = ProductResults.__Count > countToDelete ? countToDelete : ProductResults.__Count;
                    if (countToDelete > 0 && deletedCount > 0)
                    {
                        await DeleteAllAsync(_storeSession, countToDelete);
                    }
                }
            }
        }

        public async Task SaveToFileAsync(List<SuccessedProduct> inputs)
        {
            await Task.Run(() =>
            {
                var fileNames = inputs.GroupBy(x => x.FileName).Select(g => new { FileName = g.Key }).ToList();
                fileNames.ForEach(async s =>
                {
                    List<SuccessedProduct> filterdProducts = inputs.Where(x => x.FileName == s.FileName).ToList();
                    AxCSVHelper.SaveToFile(filterdProducts, $"{(await AxFolder.GetSucceededDirectoryAsync(AxFolder.GetAxFolder()))}\\{s.FileName}");
                    await TextFileHelper.AxFastRenameAsync($"{AxFolder.GetAxFolder()}\\{s.FileName}");
                });
            });
        }
    }
}
