using BI.Utilities;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.TCF;
using Entities.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BI
{
    public class TCFManager : ITCFManager
    {
        private readonly ITCFRepository _tcfRepository = null;
        private readonly TCFSettings _tcfSettings = null;
        private readonly ILogger _logger = null;
        private readonly IHelper _helper = null;
        private readonly IEventBookingLogManager _eventLog = null;
        private static IConfiguration _configuration = null;

        public TCFManager(ITCFRepository tcfRepository, IOptions<TCFSettings> tcfSettings, ILogger<TCFManager> logger, IHelper helper,
                                                                            IEventBookingLogManager eventLog, IConfiguration configuration)
        {
            _tcfRepository = tcfRepository;
            _tcfSettings = tcfSettings.Value;
            _logger = logger;
            _helper = helper;
            _eventLog = eventLog;
            _configuration = configuration;
        }

        /// <summary>
        /// Get the list of ids available in the TCF from the list of given ids
        /// </summary>
        /// <param name="searchIdList"></param>
        /// <param name="masterDataType"></param>
        /// <param name="tcfToken"></param>
        /// <returns></returns>
        private async Task<List<int>> GetDataListExistsinTCF(List<int> searchIdList, MasterDataType masterDataType, string tcfToken)
        {
            #region Declaration
            var tcfBase = _tcfSettings.BaseUrl;
            string tcfRequest = "";
            List<int> matchedDataList = new List<int>();
            HttpResponseMessage httpResponse = null;
            var isBearerToken = false;
            #endregion

            switch (masterDataType)
            {
                //if master type is suppliercontactcreation then fetch the available supplier contact ids
                case MasterDataType.SupplierContactCreation:
                    tcfRequest = _tcfSettings.SupplierContacsByIdListUrl;
                    TCFSupplierContactIdListRequest supplierContactIds = new TCFSupplierContactIdListRequest();
                    supplierContactIds.apiSupplierContactIds = searchIdList;
                    httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, tcfRequest, supplierContactIds, tcfBase, tcfToken, isBearerToken);
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                        matchedDataList = await getDataListFromResponse(httpResponse, masterDataType);
                    break;
                //if master type is buyercreation then fetch the available buyer ids
                case MasterDataType.BuyerCreation:
                    tcfRequest = _tcfSettings.BuyerByIdListUrl;
                    TCFBuyerIdListRequest buyerIdsRequest = new TCFBuyerIdListRequest();
                    buyerIdsRequest.buyerIds = searchIdList;
                    httpResponse = await _helper.SendRequestToPartnerAPI(Method.Post, tcfRequest, buyerIdsRequest, tcfBase, tcfToken, isBearerToken);
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                        matchedDataList = await getDataListFromResponse(httpResponse, masterDataType);
                    break;
            }

            return matchedDataList;
        }

        /// <summary>
        /// Returns the list of ids from the http response
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <param name="masterDataType"></param>
        /// <returns></returns>
        private async Task<List<int>> getDataListFromResponse(HttpResponseMessage httpResponse, MasterDataType masterDataType)
        {
            List<int> matchedDataList = new List<int>();
            if (httpResponse != null)
            {
                var userData = await httpResponse.Content.ReadAsStringAsync();
                JObject userResultJson = JObject.Parse(userData);
                if (userResultJson != null && userResultJson.GetValue("result") != null)
                {
                    JObject userDataJson = JObject.Parse(userResultJson.GetValue("result").ToString());
                    if (userDataJson.GetValue("data").Any())
                    {
                        foreach (var item in userDataJson.GetValue("data").ToArray())
                        {
                            if (masterDataType == MasterDataType.SupplierContactCreation)
                            {
                                matchedDataList.Add(Convert.ToInt32(item["apiSupplierContactId"]));
                            }
                            if (masterDataType == MasterDataType.BuyerCreation)
                            {
                                matchedDataList.Add(Convert.ToInt32(item["apiBuyerId"]));
                            }
                        }
                        return matchedDataList;
                    }
                }
            }

            return matchedDataList;
        }

        /// <summary>
        /// Check the given master data available in tcf by id
        /// </summary>
        /// <param name="searchId"></param>
        /// <param name="masterDataType"></param>
        /// <param name="tcfToken"></param>
        /// <returns></returns>
        private async Task<bool> CheckDataExistsinTCF(int searchId, MasterDataType masterDataType, string tcfToken)
        {
            #region Declaration
            var tcfBase = _tcfSettings.BaseUrl;
            string tcfRequest = string.Empty;
            var isDataExists = false;
            HttpResponseMessage httpResponse = null;
            #endregion

            //depends on the master data type it will check the data exists in the TCF and return true if it is available
            switch (masterDataType)
            {
                case MasterDataType.CustomerContactCreation:
                    tcfRequest = string.Format(_tcfSettings.CustomerContactByIdtUrl, searchId);
                    break;
                case MasterDataType.SupplierCreation:
                    tcfRequest = string.Format(_tcfSettings.SupplierByIdtUrl, searchId);
                    break;
                case MasterDataType.UserCreation:
                    tcfRequest = string.Format(_tcfSettings.UserByIdUrl, searchId);
                    break;
                case MasterDataType.ProductCreation:
                    tcfRequest = string.Format(_tcfSettings.ProductIdByUrl, searchId);
                    break;
            }

            httpResponse = await _helper.SendRequestToPartnerAPI(Method.Get, tcfRequest, null, tcfBase, tcfToken, false);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
                isDataExists = await getDataFromResponse(httpResponse);

            return isDataExists;
        }

        /// <summary>
        /// Check the data available or not from the http response
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <returns></returns>
        private async Task<bool> getDataFromResponse(HttpResponseMessage httpResponse)
        {
            var isExists = false;
            var userData = await httpResponse.Content.ReadAsStringAsync();
            JObject userDataJson = JObject.Parse(userData);
            if (userDataJson != null && userDataJson.GetValue("data") != null && userDataJson.GetValue("data").Any())
            {
                isExists = true;
            }
            return isExists;
        }

        /// <summary>
        /// Get the success status result from the response when we create/update the master data
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <returns></returns>
        private bool getSuccessResultFromResponse(HttpResponseMessage httpResponse)
        {
            bool isSuccess = false;
            //read the http response content
            var userData = httpResponse.Content.ReadAsStringAsync();
            //convert to json object
            JObject userDataJson = JObject.Parse(userData.Result);
            //read the result and check the status
            if (userDataJson != null && userDataJson.GetValue("result") != null)
            {
                var userResultJson = JObject.Parse(userDataJson.GetValue("result").ToString());
                if (userResultJson != null && userResultJson.GetValue("status") != null)
                {
                    if (Convert.ToString(userResultJson.GetValue("status")) == "1")
                    {
                        isSuccess = true;
                    }
                }

            }
            return isSuccess;
        }

        /// <summary>
        /// Save/Update the customer contacts to TCF
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="fbToken"></param>
        /// <param name="masterDataAction"></param>
        /// <returns></returns>
        public async Task<TCFResponseMessage> SaveCustomerContactToTCF(int contactId)
        {
            #region VariableDeclaration
            TCFResponseMessage response = new TCFResponseMessage();
            string tcfRequest = _tcfSettings.CustomerContactRequestUrl;
            Method httpMethod = Method.Post;
            var tcfToken = _tcfSettings.MasterToken;
            var tcfBase = _tcfSettings.BaseUrl;
            var isBearerToken = false;
            #endregion

            try
            {

                //Get the customer contact data needs to push to tcf
                var customerContact = await _tcfRepository.GetTCFCustomerContact(contactId);

                if (customerContact != null)
                {
                    //check the customer contact already exists in the TCF
                    var isDataExists = await CheckDataExistsinTCF(contactId, MasterDataType.CustomerContactCreation, tcfToken);

                    //if contact exists in the TCF then update the details
                    if (isDataExists)
                    {
                        tcfRequest = string.Format(_tcfSettings.CustomerContactUpdateUrl, contactId);
                        httpMethod = Method.JSONPut;
                    }

                    _logger.LogInformation("API-TCF : TCF Customer contact request start ");
                    _logger.LogInformation(JsonConvert.SerializeObject(customerContact));

                    //save the request log before calling the TCF api
                    await _eventLog.SaveTCFMasterRequestLog(new TCFMasterRequestLogInfo()
                    {
                        AccountId = contactId,
                        DataType = (int)TCFDataType.Customercontact,
                        RequestUrl = tcfRequest,
                        LogInformation = JsonConvert.SerializeObject(customerContact),
                    });

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(httpMethod, tcfRequest, customerContact, tcfBase, tcfToken, isBearerToken);

                    _logger.LogInformation("API-TCF : TCF Customer contact request end ");
                    _logger.LogInformation(httpResponse.StatusCode.ToString());

                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        //if status is ok and status is success
                        response.IsSuccess = await getResponseData(httpResponse);
                    }
                    else
                        response.IsSuccess = false;

                    //set the response message
                    if (!response.IsSuccess && httpResponse.Content != null)
                    {
                        //save the request log before calling the TCF api
                        await _eventLog.SaveTCFMasterRequestLog(new TCFMasterRequestLogInfo()
                        {
                            AccountId = contactId,
                            DataType = (int)TCFDataType.Customercontact,
                            RequestUrl = tcfRequest,
                            LogInformation = JsonConvert.SerializeObject(customerContact),
                            ResponseMessage = await httpResponse.Content.ReadAsStringAsync()
                        });
                    }
                }

                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save/Update the supplier data to TCF
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="fbToken"></param>
        /// <param name="masterDataAction"></param>
        /// <returns></returns>
        public async Task<TCFResponseMessage> SaveSupplierToTCF(int supplierId)
        {
            #region VariableDeclaration
            TCFResponseMessage response = new TCFResponseMessage();
            string tcfRequest = _tcfSettings.SupplierRequestUrl;
            Method httpMethod = Method.Post;
            var tcfToken = _tcfSettings.MasterToken;
            var tcfBase = _tcfSettings.BaseUrl;
            var isBearerToken = false;
            #endregion
            try
            {
                //Get the supplier details needs to be pushed to TCF
                var supplier = await _tcfRepository.GetTCFSupplier(supplierId);
                //get the customer glcodes
                var customerGLCodes = await _tcfRepository.GetCustomerGLCodesBySupplier(supplierId);
                //assign the glcodes for the supplier
                supplier.glCode = customerGLCodes.Select(x=>x.GlCode).ToList();

                if (supplier != null)
                {
                    //check the supplier already exists in the TCF
                    var isDataExists = await CheckDataExistsinTCF(supplierId, MasterDataType.SupplierCreation, tcfToken);

                    //if data exists in the TCF then update the details
                    if (isDataExists)
                    {
                        tcfRequest = string.Format(_tcfSettings.SupplierUpdateUrl, supplierId);
                        httpMethod = Method.JSONPut;
                    }

                    _logger.LogInformation("API-TCF : TCF supplier request start ");
                    _logger.LogInformation(JsonConvert.SerializeObject(supplier));

                    //save the request log before calling the TCF api
                    await _eventLog.SaveTCFMasterRequestLog(new TCFMasterRequestLogInfo()
                    {
                        AccountId = supplier.apiSupplierId,
                        DataType = (int)TCFDataType.Supplier,
                        RequestUrl = tcfRequest,
                        LogInformation = JsonConvert.SerializeObject(supplier),
                    });

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(httpMethod, tcfRequest, supplier, tcfBase, tcfToken, isBearerToken);

                    _logger.LogInformation("API-TCF : TCF supplier request end ");
                    _logger.LogInformation(httpResponse.StatusCode.ToString());

                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        response.IsSuccess = await getResponseData(httpResponse);
                    }
                    else
                        response.IsSuccess = false;

                    //if supplier is created then add the supplier contacts
                    if (response.IsSuccess)
                    {
                        var contactResponse = await SaveSupplierContactsToTCF(supplierId, tcfToken);
                        return contactResponse;
                    }
                    //if supplier is not created created the log with the actual response message
                    else if (!response.IsSuccess && httpResponse.Content != null)
                    {
                        await _eventLog.SaveTCFMasterRequestLog(new TCFMasterRequestLogInfo()
                        {
                            AccountId = supplier.apiSupplierId,
                            DataType = (int)TCFDataType.Supplier,
                            RequestUrl = tcfRequest,
                            LogInformation = JsonConvert.SerializeObject(supplier),
                            ResponseMessage = await httpResponse.Content.ReadAsStringAsync()
                        });

                    }
                }

                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Map the supplier contacts as per the TCF data
        /// </summary>
        /// <param name="contactList"></param>
        /// <returns></returns>
        private TCFSupplierContact MapTCFSupplierContacts(List<Suppliercontact> contactList)
        {
            TCFSupplierContact supplierContact = new TCFSupplierContact();
            supplierContact.supplierContacts = contactList;
            return supplierContact;
        }


        /// <summary>
        /// Save/Update the supplier contact data to TCF
        /// </summary>
        /// <param name="supplierContacts"></param>
        /// <param name="tcfRequest"></param>
        /// <param name="tcfToken"></param>
        /// <param name="httpMethod"></param>
        /// <returns></returns>
        private async Task<TCFResponseMessage> UpdateSupplierContacts(List<Suppliercontact> supplierContacts, string tcfRequest, string tcfToken, Method httpMethod)
        {
            #region Declaration
            TCFResponseMessage response = new TCFResponseMessage();
            var tcfBase = _tcfSettings.BaseUrl;
            var isBearerToken = false;
            #endregion

            try
            {
                //make the supplier contact get request
                var supplierContactRequest = MapTCFSupplierContacts(supplierContacts);

                _logger.LogInformation("API-TCF : TCF supplier contacts request start ");
                _logger.LogInformation(JsonConvert.SerializeObject(supplierContacts));

                //create the request log
                foreach (var contact in supplierContacts)
                {
                    var requestLog = await _eventLog.SaveTCFMasterRequestLog(new TCFMasterRequestLogInfo()
                    {
                        AccountId = contact.apiSupplierContactId,
                        DataType = (int)TCFDataType.Supplier,
                        RequestUrl = tcfRequest,
                        LogInformation = JsonConvert.SerializeObject(supplierContacts.Where(x => x.apiSupplierContactId == contact.apiSupplierContactId))
                    });
                }

                HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(httpMethod, tcfRequest, supplierContactRequest, tcfBase, tcfToken, isBearerToken);

                _logger.LogInformation("API-TCF : TCF supplier contacts request end ");
                _logger.LogInformation(httpResponse.StatusCode.ToString());

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var status = await getResponseData(httpResponse);
                    if (status)
                        response.IsSuccess = true;
                    else
                        response.IsSuccess = false;
                }
                else
                    response.IsSuccess = false;

                //create the response log
                if (!response.IsSuccess && httpResponse.Content != null)
                {
                    foreach (var contact in supplierContacts)
                    {
                        var responseLog = await _eventLog.SaveTCFMasterRequestLog(new TCFMasterRequestLogInfo()
                        {
                            AccountId = contact.apiSupplierContactId,
                            DataType = (int)TCFDataType.Supplier,
                            RequestUrl = tcfRequest,
                            ResponseMessage = await httpResponse.Content.ReadAsStringAsync(),
                            LogInformation = JsonConvert.SerializeObject(supplierContacts.Where(x => x.apiSupplierContactId == contact.apiSupplierContactId))
                        });
                    }
                }

                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        /// <summary>
        /// Save/Update the supplier contacts to TCF
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="fbToken"></param>
        /// <param name="masterDataAction"></param>
        /// <returns></returns>
        public async Task<TCFResponseMessage> SaveSupplierContactsToTCF(int supplierId, string tcfToken)
        {
            #region Declaration
            TCFResponseMessage response = new TCFResponseMessage();
            TCFResponseMessage createContactResponse = null;
            TCFResponseMessage updateContactResponse = null;
            #endregion

            try
            {
                //get the supplier contacts needs to pushed to TCF
                var supplierContacts = await _tcfRepository.GetTCFSupplierContacts(supplierId);

                if (supplierContacts != null && supplierContacts.Any())
                {
                    //get the supplier contact ids
                    var supplierContactIds = supplierContacts.Select(x => x.apiSupplierContactId).ToList();

                    //get the supplier contacts exists in the TCF already
                    var availableContactIds = await GetDataListExistsinTCF(supplierContactIds, MasterDataType.SupplierContactCreation, tcfToken);

                    var contactListToBeCreated = supplierContacts.Where(x => !availableContactIds.Contains(x.apiSupplierContactId)).ToList();
                    var contactListToBeUpdated = supplierContacts.Where(x => availableContactIds.Contains(x.apiSupplierContactId)).ToList();

                    //create the supplier contacts
                    if (contactListToBeCreated != null && contactListToBeCreated.Any())
                    {
                        createContactResponse = await UpdateSupplierContacts(contactListToBeCreated, _tcfSettings.SupplierContactRequestUrl, tcfToken, Method.Post);
                    }
                    //update the supplier contacts
                    if (contactListToBeUpdated != null && contactListToBeUpdated.Any())
                    {
                        updateContactResponse = await UpdateSupplierContacts(contactListToBeUpdated, _tcfSettings.SupplierContactUpdateUrl, tcfToken, Method.JSONPut);
                    }

                    if (createContactResponse != null && updateContactResponse != null)
                    {
                        if (createContactResponse.IsSuccess && updateContactResponse.IsSuccess)
                        {
                            response.IsSuccess = true;
                        }
                        else
                        {
                            response.IsSuccess = false;
                        }
                    }
                    else if (createContactResponse != null)
                    {
                        if (createContactResponse.IsSuccess)
                        {
                            response.IsSuccess = true;
                        }
                        else
                        {
                            response.IsSuccess = false;
                        }
                    }
                    else if (updateContactResponse != null)
                    {
                        if (updateContactResponse.IsSuccess)
                        {
                            response.IsSuccess = true;
                        }
                        else
                        {
                            response.IsSuccess = false;
                        }
                    }
                }

                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// Map user repo to tcf user data
        /// </summary>
        /// <param name="userRepo"></param>
        /// <returns></returns>
        public TCFUser MapTCFUser(TCFUserRepo userRepo)
        {
            TCFUser user = new TCFUser();
            user.apiLinkUserId = userRepo.apiLinkUserId;
            user.userName = userRepo.userName;
            user.email = !string.IsNullOrEmpty(userRepo.email) ? userRepo.email : "";
            user.glCode = !string.IsNullOrEmpty(userRepo.glCode) ? userRepo.glCode : "";
            if (userRepo.userTypeId == (int)UserTypeEnum.Customer && userRepo.customerId != null)
            {
                user.apiUserTypeId = userRepo.customerId.GetValueOrDefault();
                user.userType = UserTypeEnum.Customer.ToString();
            }
            else if (userRepo.userTypeId == (int)UserTypeEnum.Supplier && userRepo.supplierId != null)
            {
                user.apiUserTypeId = userRepo.supplierId.GetValueOrDefault();
                user.userType = UserTypeEnum.Supplier.ToString();
            }
            else
            {
                user.apiUserTypeId = 0;
                user.userType = UserTypeEnum.InternalUser.ToString();
            }

            return user;
        }

        /// <summary>
        /// Save/Update the user information to TCF
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="masterDataAction"></param>
        /// <returns></returns>
        public async Task<TCFResponseMessage> SaveUserToTCF(int userId)
        {

            #region VariableDeclaration
            TCFResponseMessage response = new TCFResponseMessage();
            string tcfRequest = _tcfSettings.UserRequestUrl;
            Method httpMethod = Method.Post;
            var tcfToken = _tcfSettings.MasterToken;
            var tcfBase = _tcfSettings.BaseUrl;
            var isBearerToken = false;
            #endregion

            try
            {
                //get the user information needs to be pushed to TCF
                var userRepo = await _tcfRepository.GetTCFUser(userId);

                if (userRepo != null)
                {
                    //map the user repo info to user data
                    var user = MapTCFUser(userRepo);

                    //check the user already exists in the TCF
                    var isDataExists = await CheckDataExistsinTCF(userId, MasterDataType.UserCreation, tcfToken);

                    //if user exists in the TCF then update the details
                    if (isDataExists)
                    {
                        tcfRequest = string.Format(_tcfSettings.UserUpdateUrl, userId);
                        httpMethod = Method.JSONPut;
                    }

                    _logger.LogInformation("API-TCF : TCF user request start ");
                    _logger.LogInformation(JsonConvert.SerializeObject(user));

                    //save the request before call the TCF Api
                    await _eventLog.SaveTCFMasterRequestLog(new TCFMasterRequestLogInfo()
                    {
                        AccountId = user.apiLinkUserId,
                        DataType = (int)TCFDataType.User,
                        RequestUrl = tcfRequest,
                        LogInformation = JsonConvert.SerializeObject(user),
                    });


                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(httpMethod, tcfRequest, user, tcfBase, tcfToken, isBearerToken);

                    _logger.LogInformation("API-TCF : TCF user request end ");
                    _logger.LogInformation(httpResponse.StatusCode.ToString());

                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        var status = await getResponseData(httpResponse);
                        if (status)
                            response.IsSuccess = true;
                        else
                            response.IsSuccess = false;
                    }
                    else
                        response.IsSuccess = false;

                    //save the response log if the data updation is not success
                    if (!response.IsSuccess && httpResponse.Content != null)
                    {
                        //save the request before call the TCF Api
                        await _eventLog.SaveTCFMasterRequestLog(new TCFMasterRequestLogInfo()
                        {
                            AccountId = user.apiLinkUserId,
                            DataType = (int)TCFDataType.User,
                            RequestUrl = tcfRequest,
                            LogInformation = JsonConvert.SerializeObject(user),
                            ResponseMessage = await httpResponse.Content.ReadAsStringAsync()
                        });
                    }

                }

                return response;
            }
            catch (Exception ex)
            {
                return new TCFResponseMessage { IsSuccess = true, ResponseMessage = ex.Message.ToString() };
            }
        }

        private async Task<TCFResponseMessage> UpdateBuyerList(List<TCFBuyer> tcfBuyerList, string tcfRequest, string tcfToken, Method httpMethod)
        {
            #region Declarations
            TCFResponseMessage response = new TCFResponseMessage();
            var tcfBase = _tcfSettings.BaseUrl;
            TCFBuyerRequest buyerRequest = new TCFBuyerRequest();
            var isBearerToken = false;
            #endregion

            try
            {

                _logger.LogInformation("API-TCF : TCF buyer list request start ");
                _logger.LogInformation(JsonConvert.SerializeObject(tcfBuyerList));

                //assign the buyer list to the request
                buyerRequest.buyers = tcfBuyerList;

                //save the request log
                foreach (var buyer in tcfBuyerList)
                {
                    await _eventLog.SaveTCFMasterRequestLog(new TCFMasterRequestLogInfo()
                    {
                        AccountId = buyer.apiBuyerId,
                        DataType = (int)TCFDataType.Supplier,
                        RequestUrl = tcfRequest,
                        LogInformation = JsonConvert.SerializeObject(tcfBuyerList.Where(x => x.apiBuyerId == buyer.apiBuyerId)),
                    });
                }

                HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(httpMethod, tcfRequest, buyerRequest, tcfBase, tcfToken, isBearerToken);

                _logger.LogInformation("API-TCF : TCF buyer list request end ");
                _logger.LogInformation(httpResponse.StatusCode.ToString());

                //if the buyer is created
                if (httpResponse.StatusCode == HttpStatusCode.Created)
                {
                    response.IsSuccess = true;
                }
                //if the http status is ok then check the status 
                else if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    if (getSuccessResultFromResponse(httpResponse))
                        response.IsSuccess = true;
                    else
                        response.IsSuccess = false;
                }
                //any status other than ok or created
                else
                    response.IsSuccess = false;

                //save the response log
                if (!response.IsSuccess && httpResponse.Content != null)
                {
                    //save the response log
                    foreach (var buyer in tcfBuyerList)
                    {
                        await _eventLog.SaveTCFMasterRequestLog(new TCFMasterRequestLogInfo()
                        {
                            AccountId = buyer.apiBuyerId,
                            DataType = (int)TCFDataType.Supplier,
                            RequestUrl = tcfRequest,
                            LogInformation = JsonConvert.SerializeObject(tcfBuyerList.Where(x => x.apiBuyerId == buyer.apiBuyerId)),
                            ResponseMessage = await httpResponse.Content.ReadAsStringAsync()
                        });
                    }
                }

                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        /// <summary>
        /// Save/Update the buyer list to TCF
        /// </summary>
        /// <param name="buyerList"></param>
        /// <param name="tcfToken"></param>
        /// <returns></returns>
        public async Task<TCFResponseMessage> SaveBuyerListToTCF(int customerId, int? entityId)
        {
            #region VariableDeclaration
            TCFResponseMessage response = new TCFResponseMessage();
            TCFResponseMessage createBuyerResponse = null;
            TCFResponseMessage updateBuyerResponse = null;
            var tcfToken = _tcfSettings.MasterToken;
            #endregion

            try
            {
                //get the buyer list needs to be push to TCF
                var tcfBuyerList = await _tcfRepository.GetTCFBuyerList(customerId,entityId);

                if (tcfBuyerList != null && tcfBuyerList.Any())
                {
                    //get the buyer ids from the list
                    var buyerIds = tcfBuyerList.Select(x => x.apiBuyerId).ToList();

                    //get the available buyerids in the tcf list from the list of buyerids
                    var availableBuyerIds = await GetDataListExistsinTCF(buyerIds, MasterDataType.BuyerCreation, tcfToken);

                    //filter the buyer list needs to created to TCF
                    var buyerListToBeCreated = tcfBuyerList.Where(x => !availableBuyerIds.Contains(x.apiBuyerId)).ToList();
                    //filter the buyer list needs to updated to TCF
                    var buyerListToBeUpdated = tcfBuyerList.Where(x => availableBuyerIds.Contains(x.apiBuyerId)).ToList();

                    //create the buyer list
                    if (buyerListToBeCreated != null && buyerListToBeCreated.Any())
                    {
                        createBuyerResponse = await UpdateBuyerList(buyerListToBeCreated, _tcfSettings.BuyerRequestUrl, tcfToken, Method.Post);
                    }

                    //update the buyer list
                    if (buyerListToBeUpdated != null && buyerListToBeUpdated.Any())
                    {
                        updateBuyerResponse = await UpdateBuyerList(buyerListToBeUpdated, _tcfSettings.BuyerUpdateUrl, tcfToken, Method.JSONPut);
                    }

                    //add both create buyer and update buyer in response message if it is failed
                    if (createBuyerResponse != null && updateBuyerResponse != null)
                    {
                        if (createBuyerResponse.IsSuccess && updateBuyerResponse.IsSuccess)
                        {
                            response.IsSuccess = true;
                        }
                        else
                        {
                            response.IsSuccess = false;
                        }
                    }
                    //add the create buyer response message if it is failed
                    else if (createBuyerResponse != null)
                    {
                        if (createBuyerResponse.IsSuccess)
                        {
                            response.IsSuccess = true;
                        }
                        else
                        {
                            response.IsSuccess = false;
                        }
                    }
                    //add the update buyer response message if it is failed
                    else if (updateBuyerResponse != null)
                    {
                        if (updateBuyerResponse.IsSuccess)
                        {
                            response.IsSuccess = true;
                        }
                        else
                        {
                            response.IsSuccess = false;
                        }
                    }
                }

                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save/Update the product to TCF
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="tcfToken"></param>
        /// <param name="masterDataAction"></param>
        /// <returns></returns>
        public async Task<TCFResponseMessage> SaveProductToTCF(int productId,int? entityId)
        {
            #region VariableDeclaration
            TCFResponseMessage response = new TCFResponseMessage();
            string tcfRequest = _tcfSettings.ProductRequestUrl;
            Method httpMethod = Method.Post;
            var tcfBase = _tcfSettings.BaseUrl;
            var tcfToken = _tcfSettings.MasterToken;
            var isBearerToken = false;
            #endregion
            try
            {
                //get the product details needs to be pushed to TCF
                var product = await _tcfRepository.GetTCFProduct(productId, entityId);

                if (product != null)
                {
                    //check the product already exists in the TCF
                    var isDataExists = await CheckDataExistsinTCF(productId, MasterDataType.ProductCreation, tcfToken);

                    //if product exists in the TCF then update the details
                    if (isDataExists)
                    {
                        tcfRequest = string.Format(_tcfSettings.ProductUpdateUrl, productId);
                        httpMethod = Method.JSONPut;
                    }

                    _logger.LogInformation("API-TCF : TCF Products request start ");
                    _logger.LogInformation(JsonConvert.SerializeObject(product));

                    await _eventLog.SaveTCFMasterRequestLog(new TCFMasterRequestLogInfo()
                    {
                        AccountId = productId,
                        DataType = (int)TCFDataType.Product,
                        RequestUrl = tcfRequest,
                        LogInformation = JsonConvert.SerializeObject(product),
                    });

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(httpMethod, tcfRequest, product, tcfBase, tcfToken, isBearerToken);

                    _logger.LogInformation("API-TCF : TCF Products data request end ");
                    _logger.LogInformation(httpResponse.StatusCode.ToString());

                    //if response status is created then product creation is success
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        var status = await getResponseData(httpResponse);
                        if (status)
                            response.IsSuccess = true;
                        else
                            response.IsSuccess = false;
                    }
                    else
                        response.IsSuccess = false;

                    //if the response status is not success and then log the response message
                    if (!response.IsSuccess && httpResponse.Content != null)
                    {
                        await _eventLog.SaveTCFMasterRequestLog(new TCFMasterRequestLogInfo()
                        {
                            AccountId = productId,
                            DataType = (int)TCFDataType.Product,
                            RequestUrl = tcfRequest,
                            LogInformation = JsonConvert.SerializeObject(product),
                            ResponseMessage = await httpResponse.Content.ReadAsStringAsync()
                        });

                    }
                }

                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Get the success status result from the response when we create/update the master data
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <returns></returns>
        private async Task<bool> getResponseData(HttpResponseMessage httpResponse)
        {
            bool isSuccess = false;
            //read the http response content
            var userData = await httpResponse.Content.ReadAsStringAsync();
            //convert to json object
            JObject userDataJson = JObject.Parse(userData);
            //read the result and check the status
            if (userDataJson != null && userDataJson.GetValue("result") != null)
            {
                var userResultJson = JObject.Parse(userDataJson.GetValue("result").ToString());
                if (userResultJson != null && userResultJson.GetValue("status") != null && Convert.ToString(userResultJson.GetValue("status")) == "1")
                {
                    isSuccess = true;
                }

            }
            return isSuccess;
        }

        public async Task<TCFResponseMessage> SaveCustomerToTCF(int customerId)
        {
            #region VariableDeclaration
            TCFResponseMessage response = new TCFResponseMessage();
            string tcfRequest = _tcfSettings.CustomerRequestUrl;
            Method httpMethod = Method.Post;
            var tcfToken = _tcfSettings.MasterToken;
            var tcfBase = _tcfSettings.BaseUrl;
            var isBearerToken = false;
            #endregion

            try
            {

                //Get the customer details needs to be pushed to TCF
                var customer = await _tcfRepository.GetTCFCustomer(customerId);

                if (customer != null)
                {
                   

                    //check the customer already exists in the TCF
                    var isDataExists = await CheckCustomerExistsinTCF(customer.glCode, tcfToken);

                    //if contact exists in the TCF then update the details
                    if (isDataExists)
                    {
                        tcfRequest = _tcfSettings.CustomerUpdateUrl;
                        httpMethod = Method.JSONPut;
                    }

                    _logger.LogInformation("API-TCF : TCF Customer request start ");
                    _logger.LogInformation(JsonConvert.SerializeObject(customer));

                    //save the request log before calling the TCF api
                    await _eventLog.SaveTCFMasterRequestLog(new TCFMasterRequestLogInfo()
                    {
                        AccountId = customerId,
                        DataType = (int)TCFDataType.Customer,
                        RequestUrl = tcfRequest,
                        LogInformation = JsonConvert.SerializeObject(customer),
                    });

                    HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(httpMethod, tcfRequest, customer, tcfBase, tcfToken, isBearerToken);

                    _logger.LogInformation("API-TCF : TCF Customer request end ");
                    _logger.LogInformation(httpResponse.StatusCode.ToString());

                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        //if status is ok and status is success
                        response.IsSuccess = await getResponseData(httpResponse);
                    }
                    else
                        response.IsSuccess = false;

                    //set the response message
                    if (!response.IsSuccess && httpResponse.Content != null)
                    {
                        //save the request log before calling the TCF api
                        await _eventLog.SaveTCFMasterRequestLog(new TCFMasterRequestLogInfo()
                        {
                            AccountId = customerId,
                            DataType = (int)TCFDataType.Customer,
                            RequestUrl = tcfRequest,
                            LogInformation = JsonConvert.SerializeObject(customer),
                            ResponseMessage = await httpResponse.Content.ReadAsStringAsync()
                        });
                    }
                }
                else
                {
                    await _eventLog.SaveTCFMasterRequestLog(new TCFMasterRequestLogInfo()
                    {
                        AccountId = customerId,
                        DataType = (int)TCFDataType.Customer,
                        ResponseMessage = "After reached the tcf manager and customer not exists"
                    });
                }

                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> CheckCustomerExistsinTCF(string glCode, string tcfToken)
        {
            #region Declaration
            var tcfBase = _tcfSettings.BaseUrl;
            string tcfRequest = string.Empty;
            var isDataExists = false;
            HttpResponseMessage httpResponse = null;
            #endregion


            tcfRequest = string.Format(_tcfSettings.CustomerByGLCode, glCode);

            httpResponse = await _helper.SendRequestToPartnerAPI(Method.Get, tcfRequest, null, tcfBase, tcfToken, false);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
                isDataExists = await getDataFromResponse(httpResponse);

            return isDataExists;
        }

    }
}
