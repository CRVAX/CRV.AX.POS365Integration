using AXLogExtension.Common;
using CRV.AX.POS365Integration.Contracts.Stores;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CRV.AX.POS365Integration.Business
{
    public class CRVBase
    {
        #region API
        /// <summary>
        /// "application/json" string
        /// </summary>
        private string Application_Json = "application/json";

        public async Task<(object, int)> CallAPI<S, F>(string uri, HttpMethod method, StoreInputDto input)
        {
            try
            {
                await AxWriteLineAndLog.LogInformationAsync($"Start calling {nameof(CRVBase)}.{nameof(CallAPI)}(uri: {uri}, method: {method}");
                uri = $"{uri}&Username={input.User}&Password={input.Password}";

                var client = new HttpClient();
                var request = new HttpRequestMessage()
                {
                    Method = method,
                    RequestUri = new Uri(uri)
                };

                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    int successStatusCode = (int)response.StatusCode;
                    if (successStatusCode / AxConstants.AX_API_RESULT_SUCCESS_STATUS_GROUP == 1)
                    {
                        await AxWriteLineAndLog.LogInformationAsync($"Calling {nameof(CRVBase)}.{nameof(CallAPI)}(uri: {uri}, method: {method}");
                        try
                        {
                            var result = JsonConvert.DeserializeObject<S>(body);
                            result.GetType().GetProperty("StoreNumber")?.SetValue(result, input.StoreNumber);
                            return (result, (int)response.StatusCode);
                        }
                        catch
                        {
                            return (null, (int)response.StatusCode);
                        }
                    }
                    else
                    {
                        await AxWriteLineAndLog.LogErrorAsync($"Calling {nameof(CRVBase)}.{nameof(CallAPI)}(uri: {uri}, method: {method}");
                        try
                        {
                            var result = JsonConvert.DeserializeObject<F>(body);
                            return (result, (int)response.StatusCode);
                        }
                        catch
                        {
                            return (null, (int)response.StatusCode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await AxWriteLineAndLog.WriteException(nameof(CRVBase), nameof(CallAPI), $"uri: {uri}, method: {method}", ex);
                return (null, AxConstants.AX_API_RESULT_EXCEPTION_STATUS_CODE);
            }
        }

        //public async Task<(object, int)> CallAPIDelete(string uri, string session, long id)
        //{
        //    try
        //    {
        //        await AxWriteLineAndLog.LogInformationAsync($"Start calling {nameof(CRVBase)}.{nameof(CallAPI)}(uri: {uri}, method: {method}, session: {session}, request_model: {JsonConvert.SerializeObject(request_model)})");
        //        uri = $"{uri}/{id}";

        //        var client = new HttpClient(new HttpClientHandler { UseCookies = false });
        //        var request = new HttpRequestMessage()
        //        {
        //            Method = HttpMethod.Delete,
        //            RequestUri = new Uri(uri),
        //            Headers =
        //            {
        //                { "Accept", Application_Json },
        //                { "Cookie", $"ss-id={session}" }
        //            }
        //        };

        //        using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
        //        {
        //            var body = await response.Content.ReadAsStringAsync();
        //            int successStatusCode = (int)response.StatusCode;
        //            if (successStatusCode / AxConstants.AX_API_RESULT_SUCCESS_STATUS_GROUP == 1)
        //            {
        //                await AxWriteLineAndLog.LogInformationAsync($"Calling {nameof(CRVBase)}.{nameof(CallAPI)}(uri: {uri}, method: {HttpMethod.Delete}, session: {session}, id: {id}) return success: {body ?? "[Empty body]"}");
        //                try
        //                {
        //                    return (null, (int)response.StatusCode);
        //                }
        //                catch
        //                {
        //                    return (null, (int)response.StatusCode);
        //                }
        //            }
        //            else
        //            {
        //                await AxWriteLineAndLog.LogErrorAsync($"Calling {nameof(CRVBase)}.{nameof(CallAPI)}(uri: {uri}, method: {HttpMethod.Delete}, toksessionen: {session}, request_model: {JsonConvert.SerializeObject(request_model)}) return fail: {body ?? "[Empty body]"}");
        //                try
        //                {
        //                    return (null, (int)response.StatusCode);
        //                }
        //                catch
        //                {
        //                    return (null, (int)response.StatusCode);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await AxWriteLineAndLog.WriteException(nameof(CRVBase), nameof(CallAPI), $"uri: {uri}, method: {HttpMethod.Delete}, session: {session}, request_model: {JsonConvert.SerializeObject(request_model)}", ex);
        //        return (null, AxConstants.AX_API_RESULT_EXCEPTION_STATUS_CODE);
        //    }
        //}

        /// <summary>m
        /// Call API via URI
        /// </summary>
        /// <typeparam name="S">Success generic class</typeparam>
        /// <typeparam name="F">Fail generic class</typeparam>
        /// <typeparam name="T">Input/Request generic class</typeparam>
        /// <param name="uri">API URL</param>
        /// <param name="method">HttpMethod: Get/Post/Put/Patch/Delete...</param>
        /// <param name="session">Access Token, which return from Authenticate API</param>
        /// <param name="request_model">Input/Request model</param>
        /// <returns></returns>
        public async Task<(object, int)> CallAPI<S, F, T>(string uri, HttpMethod method, string session, T request_model = default)
        {
            try
            {
                AxWriteLineAndLog.IsWriteToConsole = true;
                await AxWriteLineAndLog.LogInformationAsync($"Start calling {nameof(CRVBase)}.{nameof(CallAPI)}(uri: {uri}, method: {method}, session: {session}, request_model: {JsonConvert.SerializeObject(request_model)})");

                var client = new HttpClient(new HttpClientHandler { UseCookies = false });
                var request = new HttpRequestMessage()
                {
                    Method = method,
                    RequestUri = new Uri(uri),
                    Headers =
                    {
                        { "Accept", Application_Json },
                        { "Cookie", $"ss-id={session}" }
                    }
                };
                
                if (!EqualityComparer<T>.Default.Equals(request_model, default(T)))
                {
                    string request_json = JsonConvert.SerializeObject(request_model);
                    request.Content = new StringContent(request_json)
                    {
                        Headers =
                        {
                            ContentType = new MediaTypeHeaderValue(Application_Json),
                        }
                    };
                }

                
                using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    //response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    int successStatusCode = (int)response.StatusCode;
                    if (successStatusCode / AxConstants.AX_API_RESULT_SUCCESS_STATUS_GROUP == 1)
                    {
                        await AxWriteLineAndLog.LogInformationAsync($"Calling {nameof(CRVBase)}.{nameof(CallAPI)}(uri: {uri}, method: {method}, session: {session}, request_model: {JsonConvert.SerializeObject(request_model)}) return success: {body ?? "[Empty body]"}");
                        try
                        {
                            var result = JsonConvert.DeserializeObject<S>(body);
                            result.GetType().GetProperty("AXId")?.SetValue(result, request_model.GetType().GetProperty("AXId").GetValue(request_model));
                            result.GetType().GetProperty("StoreNumber")?.SetValue(result, request_model.GetType().GetProperty("StoreNumber").GetValue(request_model));
                            result.GetType().GetProperty("FileName")?.SetValue(result, request_model.GetType().GetProperty("FileName").GetValue(request_model));
                            return (result, (int)response.StatusCode);
                        }
                        catch
                        {
                            return (null, (int)response.StatusCode);
                        }
                    }
                    else
                    {
                        await AxWriteLineAndLog.LogErrorAsync($"Calling {nameof(CRVBase)}.{nameof(CallAPI)}(uri: {uri}, method: {method}, toksessionen: {session}, request_model: {JsonConvert.SerializeObject(request_model)}) return fail: {body ?? "[Empty body]"}");
                        try
                        {
                            var result = JsonConvert.DeserializeObject<F>(body);
                            result.GetType().GetProperty("AXId")?.SetValue(result, request_model.GetType().GetProperty("AXId").GetValue(request_model));
                            result.GetType().GetProperty("StoreNumber")?.SetValue(result, request_model.GetType().GetProperty("StoreNumber").GetValue(request_model));
                            result.GetType().GetProperty("FileName")?.SetValue(result, request_model.GetType().GetProperty("FileName").GetValue(request_model));

                            return (result, (int)response.StatusCode);
                        }
                        catch
                        {
                            return (null, (int)response.StatusCode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await AxWriteLineAndLog.WriteException(nameof(CRVBase), nameof(CallAPI), $"uri: {uri}, method: {method}, session: {session}, request_model: {JsonConvert.SerializeObject(request_model)}", ex);
                return (null, AxConstants.AX_API_RESULT_EXCEPTION_STATUS_CODE);
            }
        }

        public async Task<(object, int)> CallAPI<S, F, T>(string uri, HttpMethod method, string session, List<T> request_model = null)
        {
            try
            {
                await AxWriteLineAndLog.LogInformationAsync($"Start calling {nameof(CRVBase)}.{nameof(CallAPI)}(uri: {uri}, method: {method}, session: {session}, request_model: {JsonConvert.SerializeObject(request_model)})");
                if (request_model != null && request_model.Count == 0)
                {
                    await AxWriteLineAndLog.LogInformationAsync("Length of Request model is zero.");
                    return (null, 999);
                }

                var client = new HttpClient(new HttpClientHandler { UseCookies = false });
                var request = new HttpRequestMessage()
                {
                    Method = method,
                    RequestUri = new Uri(uri),
                    Headers =
                    {
                        { "Accept", Application_Json },
                        { "Cookie", $"ss-id={session}" }
                    }
                };

                string json = JsonConvert.SerializeObject(request_model);
                if (request_model != null)
                {
                    request.Content = new StringContent(json)
                    {
                        Headers =
                        {
                            ContentType = new MediaTypeHeaderValue(Application_Json)
                        }
                    };
                }

                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    int successStatusCode = (int)response.StatusCode;
                    if (successStatusCode / AxConstants.AX_API_RESULT_SUCCESS_STATUS_CODE == 1)
                    {
                        await AxWriteLineAndLog.LogInformationAsync($"Calling {nameof(CRVBase)}.{nameof(CallAPI)}(uri: {uri}, method: {method}, session: {session}, request_model: {JsonConvert.SerializeObject(request_model)}) return success: {body ?? "[Empty body]"}");
                        try
                        {
                            var result = JsonConvert.DeserializeObject<S>(body);
                            result.GetType().GetProperty("AXId")?.SetValue(result, request_model.GetType().GetProperty("AXId").GetValue(request_model));
                            result.GetType().GetProperty("StoreNumber")?.SetValue(result, request_model.GetType().GetProperty("StoreNumber").GetValue(request_model));
                            result.GetType().GetProperty("FileName")?.SetValue(result, request_model.GetType().GetProperty("FileName").GetValue(request_model));
                            return (result, (int)response.StatusCode);
                        }
                        catch
                        {
                            return (null, (int)response.StatusCode);
                        }
                    }
                    else
                    {
                        await AxWriteLineAndLog.LogInformationAsync($"Calling {nameof(CRVBase)}.{nameof(CallAPI)}(uri: {uri}, method: {method}, session: {session}, request_model: {JsonConvert.SerializeObject(request_model)}) return fail: {body ?? "[Empty body]"}");
                        try
                        {
                            var result = JsonConvert.DeserializeObject<F>(body);
                            result.GetType().GetProperty("AXId")?.SetValue(result, request_model.GetType().GetProperty("AXId").GetValue(request_model));
                            result.GetType().GetProperty("StoreNumber")?.SetValue(result, request_model.GetType().GetProperty("StoreNumber").GetValue(request_model));
                            result.GetType().GetProperty("FileName")?.SetValue(result, request_model.GetType().GetProperty("FileName").GetValue(request_model));
                            return (result, (int)response.StatusCode);
                        }
                        catch
                        {
                            return (null, (int)response.StatusCode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await AxWriteLineAndLog.WriteException(nameof(CRVBase), nameof(CallAPI), $"uri: {uri}, method: {method}, session: {session}, request_model: {JsonConvert.SerializeObject(request_model)}", ex);
                return (null, AxConstants.AX_API_RESULT_EXCEPTION_STATUS_CODE);
            }
        }

        #endregion API
    }
}
