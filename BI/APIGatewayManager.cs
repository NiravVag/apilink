using BI.Utilities;
using Contracts.Managers;
using DTO.Common;
using DTO.GenericAPI;
using DTO.TCF;
using Entities;
using Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace BI
{
    public class APIGatewayManager : ApiCommonData,IAPIGatewayManager
    {
        private readonly IHelper _helper = null;
        private readonly IEventBookingLogManager _eventLog = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly TCFSettings _tcfSettings = null;

        public APIGatewayManager(IHelper helper, IEventBookingLogManager eventLog, IAPIUserContext ApplicationContext, IOptions<TCFSettings> tcfSettings)
        {
            _helper = helper;
            _eventLog = eventLog;
            _ApplicationContext = ApplicationContext;
            _tcfSettings = tcfSettings.Value;
        }

        //htpps://prepor

        /// <summary>
        /// Common get request function
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<dynamic> GetRequest(APIGatewayGETRequest request)
        {
            bool isBearer = false;
            string baseRequestUrl = string.Empty;
            string clientToken = string.Empty;


            if (!string.IsNullOrEmpty(request.RequestUrl))
            {
                switch (request.Client)
                {
                    case (int)ClientEnum.TCF:
                        baseRequestUrl = _tcfSettings.BaseUrl;
                        clientToken = request.IsGenericToken ? _tcfSettings.MasterToken : request.Token;
                        break;
                }

                //insert error log
                await _eventLog.SaveAPIGatewayLog(new ApigatewayLog()
                {
                    RequestUrl = request.RequestUrl,
                    RequestBaseUrl = baseRequestUrl,
                    CreatedBy = _ApplicationContext.UserId,
                    CreatedOn = DateTime.Now
                });

                try
                {
                    //call the external api
                    using (HttpResponseMessage response = await _helper.SendRequestToPartnerAPI(Method.Get, request.RequestUrl, null, baseRequestUrl, clientToken, isBearer))
                    {
                        try
                        {
                            //throw error if response is false
                            response.EnsureSuccessStatusCode();
                            //get the response data
                            string responseBody = await response.Content.ReadAsStringAsync();

                            return JsonConvert.DeserializeObject<dynamic>(responseBody);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            await _eventLog.SaveAPIGatewayLog(new ApigatewayLog()
                            {
                                RequestUrl = request.RequestUrl,
                                RequestBaseUrl = baseRequestUrl,
                                CreatedBy = _ApplicationContext.UserId,
                                ResponseMessage = (response.StatusCode == HttpStatusCode.OK) ? await response.Content.ReadAsStringAsync() : response.StatusCode.ToString(),
                                CreatedOn = DateTime.Now
                            });
                        }

                    }
                }
                catch (Exception ex)
                {
                    await _eventLog.SaveAPIGatewayLog(new ApigatewayLog()
                    {
                        RequestUrl = request.RequestUrl,
                        RequestBaseUrl = baseRequestUrl,
                        //LogInformation = request.RequestData,
                        ResponseMessage = ex.Message.ToString(),
                        CreatedBy = _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    });
                }

            }

            return default(dynamic);
        }

        /// <summary>
        /// Generic Post Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<dynamic> PostRequest(APIGatewayPOSTRequest request)
        {
            string baseRequestUrl = string.Empty;
            string clientToken = string.Empty;

            if (!string.IsNullOrEmpty(request.RequestUrl))
            {
                switch (request.Client)
                {
                    case (int)ClientEnum.TCF:
                        baseRequestUrl = _tcfSettings.BaseUrl;
                        clientToken = request.IsGenericToken ? _tcfSettings.MasterToken : request.Token;
                        break;
                }

                //insert error log
                await _eventLog.SaveAPIGatewayLog(new ApigatewayLog()
                {
                    RequestUrl = request.RequestUrl,
                    RequestBaseUrl = baseRequestUrl,
                    CreatedBy = _ApplicationContext.UserId,
                    CreatedOn = DateTime.Now
                });

                try
                {
                    //call the external api
                    using (HttpResponseMessage response = await _helper.SendRequestToPartnerAPI(Method.Post, request.RequestUrl, request.RequestData, baseRequestUrl, clientToken, false))
                    {
                        try
                        {
                            //throw error if response is false
                            response.EnsureSuccessStatusCode();
                            //get the response data
                            string responseBody = await response.Content.ReadAsStringAsync();

                            return JsonConvert.DeserializeObject<dynamic>(responseBody);
                        }
                        catch (Exception ex)
                        {
                            //insert response error log
                            await _eventLog.SaveAPIGatewayLog(new ApigatewayLog()
                            {
                                RequestUrl = request.RequestUrl,
                                RequestBaseUrl = baseRequestUrl,
                                LogInformation = request.RequestData,
                                ResponseMessage = (response.StatusCode == HttpStatusCode.OK) ? await response.Content.ReadAsStringAsync() : response.StatusCode.ToString(),
                                CreatedBy = _ApplicationContext.UserId,
                                CreatedOn = DateTime.Now
                            });
                        }

                    }
                }
                catch (Exception ex)
                {
                    await _eventLog.SaveAPIGatewayLog(new ApigatewayLog()
                    {
                        RequestUrl = request.RequestUrl,
                        RequestBaseUrl = baseRequestUrl,
                        //LogInformation = request.RequestData,
                        ResponseMessage = ex.Message.ToString(),
                        CreatedBy = _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    });
                }

            }

            return default(dynamic);

            //string token = "access_token_c7295a82193ae0260f9902e6db44b749723d8db1";
            //return await _genericAPI.PostRequest(uri, content, token);
        }

        /// <summary>
        /// Generic Put Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<dynamic> PutRequest(APIGatewayPUTRequest request)
        {
            string baseRequestUrl = string.Empty;
            string clientToken = string.Empty;

            switch (request.Client)
            {
                case (int)ClientEnum.TCF:
                    baseRequestUrl = _tcfSettings.BaseUrl;
                    clientToken = request.IsGenericToken ? _tcfSettings.MasterToken : request.Token;
                    break;
            }

            //insert the log
            await _eventLog.SaveAPIGatewayLog(new ApigatewayLog()
            {
                RequestUrl = request.RequestUrl,
                RequestBaseUrl = baseRequestUrl,
                LogInformation = request.RequestUrl,
                CreatedBy = _ApplicationContext.UserId,
                CreatedOn = DateTime.Now
            });

            //call the external api
            using (HttpResponseMessage response = await _helper.SendRequestToPartnerAPI(Method.JSONPut, request.RequestUrl, null, baseRequestUrl, clientToken, false))
            {
                try
                {
                    //throw error if response is false
                    response.EnsureSuccessStatusCode();
                    //get the response data
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<dynamic>(responseBody);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    //insert response error log
                    await _eventLog.SaveAPIGatewayLog(new ApigatewayLog()
                    {
                        RequestUrl = request.RequestUrl,
                        RequestBaseUrl = baseRequestUrl,
                        LogInformation = request.RequestUrl,
                        ResponseMessage = (response.StatusCode == HttpStatusCode.OK) ? await response.Content.ReadAsStringAsync() : response.StatusCode.ToString(),
                        CreatedBy = _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    });
                }

            }

            return default(dynamic);

        }

        /// <summary>
        /// Generic Delete Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<dynamic> DeleteRequest(GenericAPIDELETERequest request)
        {
            //insert the log
            await _eventLog.SaveAPIGatewayLog(new ApigatewayLog()
            {
                RequestUrl = request.RequestUrl,
                RequestBaseUrl = request.RequestBase,
                CreatedBy = _ApplicationContext.UserId,
                CreatedOn = DateTime.Now
            });

            //call the external api
            using (HttpResponseMessage response = await _helper.SendRequestToPartnerAPI(Method.Delete, request.RequestUrl, null, request.RequestBase, request.Token, false))
            {
                try
                {
                    //throw error if response is false
                    response.EnsureSuccessStatusCode();
                    //get the response data
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<dynamic>(responseBody);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    //insert response error log
                    await _eventLog.SaveAPIGatewayLog(new ApigatewayLog()
                    {
                        RequestUrl = request.RequestUrl,
                        RequestBaseUrl = request.RequestBase,
                        ResponseMessage = (response.StatusCode == HttpStatusCode.OK) ? await response.Content.ReadAsStringAsync() : response.StatusCode.ToString(),
                        CreatedBy = _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    });
                }

            }

            return default(dynamic);

            //string token = "access_token_c7295a82193ae0260f9902e6db44b749723d8db1";
            //return await _genericAPI.PostRequest(uri, content, token);
        }

        /// <summary>
        /// Upload the document
        /// </summary>
        /// <param name="fileToUpload"></param>
        /// <param name="requestFormValues"></param>
        /// <returns></returns>
        public async Task<dynamic> UploadDocument(IFormFile fileToUpload, IDictionary<string, string> requestFormValues)
        {
            if (requestFormValues.ContainsKey("apiService") && Convert.ToInt32(requestFormValues["apiService"]) == (int)APIServiceEnum.TCF)
            {
                return await UploadTCFDocument(fileToUpload, requestFormValues);
            }
            return default(dynamic);
        }

        /// <summary>
        /// Upload the TCF Document
        /// </summary>
        /// <param name="fileToUpload"></param>
        /// <param name="requestFormValues"></param>
        /// <returns></returns>
        private async Task<dynamic> UploadTCFDocument(IFormFile fileToUpload, IDictionary<string, string> requestFormValues)
        {
            MultipartFormDataContent form;
            string baseUrl = _tcfSettings.BaseUrl;
            string requestUrl = string.Empty;
            string userToken = string.Empty;

            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseUrl);
                requestUrl = _tcfSettings.UploadTCFDocumentUrl + requestFormValues["tcfId"];

                //populate the form data input
                form = GetTCFDocumentFormData(fileToUpload, requestFormValues);

                //assign the user token
                if (requestFormValues.ContainsKey("userToken"))
                    userToken = Convert.ToString(requestFormValues["userToken"]);

                try
                {
                    response = await _helper.UploadDocument(form, baseUrl, requestUrl, userToken);
                    //throw error if response is false
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<dynamic>(responseBody);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    //insert response error log
                    await _eventLog.SaveAPIGatewayLog(new ApigatewayLog()
                    {
                        RequestUrl = requestUrl,
                        RequestBaseUrl = baseUrl,
                        ResponseMessage = (response.StatusCode == HttpStatusCode.OK) ? await response.Content.ReadAsStringAsync() : response.StatusCode.ToString(),
                        CreatedBy = _ApplicationContext.UserId,
                        CreatedOn = DateTime.Now
                    });
                }

                return default(dynamic);

            }
            catch (Exception)
            {
                return default(dynamic);

            }
        }

        private MultipartFormDataContent GetTCFDocumentFormData(IFormFile fileToUpload, IDictionary<string, string> requestFormValues)
        {
            MultipartFormDataContent form = new MultipartFormDataContent();

            using (var ms = new MemoryStream())
            {
                fileToUpload.CopyTo(ms);

                var dataFiles = ms.ToArray();

                var fileContent = new ByteArrayContent(dataFiles);

                string fileName = ReplaceWhitespace(fileToUpload.FileName, " ");

                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "ufile",
                    FileName = fileName
                };

                form.Add(fileContent);

                if (requestFormValues.ContainsKey("documentName"))
                    form.Add(new StringContent(requestFormValues["documentName"]), "doc_name");

                if (requestFormValues.ContainsKey("standardIds"))
                    form.Add(new StringContent("[" + requestFormValues["standardIds"] + "]"), "standard_ids");

                if (requestFormValues.ContainsKey("typeId"))
                    form.Add(new StringContent(requestFormValues["typeId"]), "type_id");

                if (requestFormValues.ContainsKey("issuerId"))
                    form.Add(new StringContent(requestFormValues["issuerId"]), "issuer_id");

                if (requestFormValues.ContainsKey("documentIssueDate"))
                    form.Add(new StringContent(requestFormValues["documentIssueDate"]), "date_document_issue");

            }

            return form;
        }


        public async Task<dynamic> UploadFiles(List<IFormFile> files, IDictionary<string, string> requestFormValues)
        {
            MultipartFormDataContent form;
            string baseUrl = string.Empty;
            string requestUrl = string.Empty;
            string userToken = string.Empty;
            int client = 0;
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                if (requestFormValues.ContainsKey("client"))
                    client = Convert.ToInt32(requestFormValues["client"]);

                switch (client)
                {
                    case (int)APIServiceEnum.TCF:
                        {
                            baseUrl = _tcfSettings.BaseUrl;
                            if (requestFormValues.ContainsKey("requestUrl"))
                                requestUrl = Convert.ToString(requestFormValues["requestUrl"]);
                            if (requestFormValues.ContainsKey("token"))
                                userToken = Convert.ToString(requestFormValues["token"]);
                        }

                        break;
                }

                //populate the form data input
                form = GetFileFormData(files);

                response = await _helper.UploadDocument(form, baseUrl, requestUrl, userToken);
                //throw error if response is false
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<dynamic>(responseBody);

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //insert response error log
                await _eventLog.SaveAPIGatewayLog(new ApigatewayLog()
                {
                    RequestUrl = requestUrl,
                    RequestBaseUrl = baseUrl,
                    ResponseMessage = (response.StatusCode == HttpStatusCode.OK) ? await response.Content.ReadAsStringAsync() : response.StatusCode.ToString(),
                    CreatedBy = _ApplicationContext.UserId,
                    CreatedOn = DateTime.Now
                });
            }

            return default(dynamic);


        }


        private MultipartFormDataContent GetFileFormData(List<IFormFile> files)
        {
            MultipartFormDataContent form = new MultipartFormDataContent();

            foreach (var fileToUpload in files)
            {
                using (var ms = new MemoryStream())
                {
                    fileToUpload.CopyTo(ms);
                    var dataFiles = ms.ToArray();
                    form.Add(new ByteArrayContent(dataFiles), "ufile", fileToUpload.FileName);
                }
            }

            return form;
        }

    }
}
