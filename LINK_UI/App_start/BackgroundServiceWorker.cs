using Components.Core.contracts;
using Components.Core.entities.Emails;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.EmailLog;
using DTO.EventBookingLog;
using DTO.File;
using DTO.FullBridge;
using DTO.Inspection;
using DTO.Master;
using DTO.TCF;
using Entities;
using Entities.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LINK_UI.App_start
{
    /// <summary>
    /// Initialize background service method for email queue
    /// </summary>
    public class BackgroundServiceWorker : BackgroundService
    {
        private readonly ILogger<BackgroundServiceWorker> _logger;
        private readonly IRabbitMQGenericClient _rabbitMqClient;
        private readonly IEmailManager _emailManager;
        private static IConfiguration _configuration = null;
        //  private readonly ITenantProvider _tenantProvider = null;

        public BackgroundServiceWorker(IServiceProvider services,
            ILogger<BackgroundServiceWorker> logger, IRabbitMQGenericClient rabbitMqClient,
            IEmailManager emailManager, IConfiguration configuration)
        {
            Services = services;
            _logger = logger;
            _rabbitMqClient = rabbitMqClient;
            _emailManager = emailManager;
            _configuration = configuration;
            // _tenantProvider = tenantProvider;
        }

        public IServiceProvider Services { get; }

        /// <summary>
        /// This method will trigger after starting the application
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service running.");

            await DoWork(stoppingToken);
        }
        /// <summary>
        ///  Subscribe Email Queue
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is working.");

            _rabbitMqClient.Listen<EmailDataRequest>(_configuration["EmailQueue"], ProcessMessage);
            _rabbitMqClient.Listen<EmailDataRequest>(_configuration["EmailErrorQueue"], ProcessErrorMessage);
            _rabbitMqClient.Listen<FbReportFetchRequest>(_configuration["FbReportQueue"], ProcessFbReportMessage);
            _rabbitMqClient.Listen<MasterDataRequest>(_configuration["AccountQueue"], ProcessAccountData);
            _rabbitMqClient.Listen<FileAttachmentToZipRequest>(_configuration["ProcessZipFileQueue"], ProcessFileAttachments);
            _rabbitMqClient.Listen<FastReportRequest>(_configuration["FastReportQueue"], ProcessFastReportQueue);
            _rabbitMqClient.Listen<FbBookingRequest>(_configuration["FBBookingSyncQueue"], ProcessFbBookingRequest);


            await Task.CompletedTask;
        }

        /// <summary>
        /// Process Queue message one by one.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task ProcessMessage(MessageModel message)
        {
            using (var scope = Services.CreateScope())
            {
                Thread.Sleep(10000);
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IEmailLogQueueManager>();

                var scopedEmailSendService =
                    scope.ServiceProvider
                   .GetRequiredService<IEmailSendManager>();

                var scopedFBReportManager =
                    scope.ServiceProvider
                        .GetRequiredService<IFBReportManager>();

                // send email
                var data = message.Payload as EmailDataRequest;
                var emailDataFromDB = await scopedProcessingService.GetEmailLogById(data.EmailQueueId);

                if (emailDataFromDB != null)
                {
                    try
                    {
                        var email = new EmailInfoRequest()
                        {
                            Body = emailDataFromDB.Body,
                            CCList = (emailDataFromDB.Cclist != null && emailDataFromDB.Cclist.Any()) ? emailDataFromDB.Cclist.Split(";").ToArray() : null,
                            Subject = emailDataFromDB.Subject,
                            Id = data.Id,
                            TryCount = data.TryCount,
                            FileList = emailDataFromDB.LogEmailQueueAttachments.Where(x => x.Active).Select(x => new FileResponse()
                            {
                                Content = x.File,
                                Name = x.FileName,
                                FileLink = x.FileLink,
                                FileUniqueId = x.FileUniqueId,
                                FileStorageType = x.FileStorageType.GetValueOrDefault(),
                                Result = FileResult.Success
                            }),
                            EmailQueueId = data.EmailQueueId,
                            Recepients = (emailDataFromDB.ToList != null && emailDataFromDB.ToList.Any()) ? emailDataFromDB.ToList.Split(";").ToArray() : null
                        };

                        var isSendEmailSuccess = _emailManager.SendEmail(email, emailDataFromDB.EntityId.GetValueOrDefault());
                        if (isSendEmailSuccess)
                        {
                            emailDataFromDB.TryCount = data.TryCount;
                            emailDataFromDB.SendOn = DateTime.UtcNow;
                            emailDataFromDB.Status = (int)EmailStatus.Success;
                            await scopedProcessingService.UpdateEmailLog(emailDataFromDB);

                            // check email type and update the booking status once all the report sent 
                            if (emailDataFromDB.LogBookingReportEmailQueues.Any(x => x.EsTypeId == (int)EmailSendingType.ReportSend))
                            {
                                var bookingIds = emailDataFromDB.LogBookingReportEmailQueues.
                                    Where(x => x.EsTypeId == (int)EmailSendingType.ReportSend).Select(x => x.InspectionId.GetValueOrDefault()).Distinct().ToList();
                                await scopedEmailSendService.UpdateBookingStatusbyReportSent(bookingIds, emailDataFromDB.EntityId.GetValueOrDefault(), emailDataFromDB.CreatedBy);

                                string fbToken = getFbToken();
                                await scopedFBReportManager.UpdateSentReportDateAndTime(bookingIds, fbToken);
                            }
                            else if (emailDataFromDB.LogBookingReportEmailQueues.Any(x => x.EsTypeId == (int)EmailSendingType.InvoiceStatus))
                            {
                                var bookingIds = emailDataFromDB.LogBookingReportEmailQueues.
                                    Where(x => x.EsTypeId == (int)EmailSendingType.InvoiceStatus).Select(x => x.InspectionId.GetValueOrDefault()).Distinct().ToList();
                                await scopedEmailSendService.UpdateInvoiceStatusbyInvoiceSend(bookingIds, emailDataFromDB.EntityId.GetValueOrDefault(), emailDataFromDB.CreatedBy);

                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation("Email Sending Error:" + ex.Message.ToString() + "");
                        if (data.TryCount <= Int32.Parse(_configuration["EmailTryCount"]))
                        {
                            emailDataFromDB.SendOn = DateTime.UtcNow;
                            emailDataFromDB.Status = (int)EmailStatus.Failure;
                            emailDataFromDB.TryCount = data.TryCount;
                            await scopedProcessingService.UpdateEmailLog(emailDataFromDB);

                            data.TryCount = data.TryCount + 1;
                            await _rabbitMqClient.Publish<EmailDataRequest>(_configuration["EmailErrorQueue"], data);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Process Error Queue message one by one.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task ProcessErrorMessage(MessageModel message)
        {
            using (var scope = Services.CreateScope())
            {
                Thread.Sleep(10000);
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IEmailLogQueueManager>();

                var scopedEmailSendService =
                        scope.ServiceProvider
                       .GetRequiredService<IEmailSendManager>();

                var data = message.Payload as EmailDataRequest;

                var emailDataFromDB = await scopedProcessingService.GetEmailLogById(data.EmailQueueId);

                if (data.TryCount <= Int32.Parse(_configuration["EmailTryCount"]))
                {
                    emailDataFromDB.TryCount = data.TryCount;
                    emailDataFromDB.SendOn = DateTime.UtcNow;
                    emailDataFromDB.Status = (int)EmailStatus.Failure;
                    await scopedProcessingService.UpdateEmailLog(emailDataFromDB);
                }

                try
                {
                    var email = new EmailInfoRequest()
                    {
                        Body = emailDataFromDB.Body,
                        CCList = (emailDataFromDB.Cclist != null && emailDataFromDB.Cclist.Any()) ? emailDataFromDB.Cclist.Split(";").ToArray() : null,
                        Subject = emailDataFromDB.Subject,
                        TryCount = data.TryCount,
                        FileList = emailDataFromDB.LogEmailQueueAttachments.Select(x => new FileResponse()
                        {
                            Content = x.File,
                            Name = x.FileName,
                            FileLink = x.FileLink,
                            FileUniqueId = x.FileUniqueId,
                            FileStorageType = x.FileStorageType.GetValueOrDefault(),
                            Result = FileResult.Success
                        }),
                        EmailQueueId = data.EmailQueueId,
                        Id = data.Id,
                        Recepients = (emailDataFromDB.ToList != null && emailDataFromDB.ToList.Any()) ? emailDataFromDB.ToList.Split(";").ToArray() : null
                    };

                    var isSendEmailSuccess = _emailManager.SendEmail(email, emailDataFromDB.EntityId.GetValueOrDefault());

                    if (isSendEmailSuccess)
                    {
                        if (data.TryCount <= Int32.Parse(_configuration["EmailTryCount"]))
                        {
                            emailDataFromDB.TryCount = data.TryCount;
                        }
                        emailDataFromDB.SendOn = DateTime.UtcNow;
                        emailDataFromDB.Status = (int)EmailStatus.Success;
                        await scopedProcessingService.UpdateEmailLog(emailDataFromDB);

                        // check email type and update the booking status once all the report sent 
                        if (emailDataFromDB.LogBookingReportEmailQueues.Any(x => x.EsTypeId == (int)EmailSendingType.ReportSend))
                        {
                            var bookingIds = emailDataFromDB.LogBookingReportEmailQueues.
                                Where(x => x.EsTypeId == (int)EmailSendingType.ReportSend).Select(x => x.InspectionId.GetValueOrDefault()).Distinct().ToList();
                            await scopedEmailSendService.UpdateBookingStatusbyReportSent(bookingIds, emailDataFromDB.EntityId.GetValueOrDefault(), emailDataFromDB.CreatedBy);
                        }
                        else if (emailDataFromDB.LogBookingReportEmailQueues.Any(x => x.EsTypeId == (int)EmailSendingType.InvoiceStatus))
                        {
                            var bookingIds = emailDataFromDB.LogBookingReportEmailQueues.
                                Where(x => x.EsTypeId == (int)EmailSendingType.InvoiceStatus).Select(x => x.InspectionId.GetValueOrDefault()).Distinct().ToList();
                            await scopedEmailSendService.UpdateInvoiceStatusbyInvoiceSend(bookingIds, emailDataFromDB.EntityId.GetValueOrDefault(), emailDataFromDB.CreatedBy);

                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Email Sending Error:" + ex.Message.ToString() + "");
                    // if email process failure again push to error queue then process the email max no of times
                    if (data.TryCount <= Int32.Parse(_configuration["EmailTryCount"]))
                    {
                        data.TryCount = data.TryCount + 1;
                        emailDataFromDB.TryCount = data.TryCount;
                        emailDataFromDB.SendOn = DateTime.UtcNow;
                        emailDataFromDB.Status = (int)EmailStatus.Failure;
                        await scopedProcessingService.UpdateEmailLog(emailDataFromDB);
                        await _rabbitMqClient.Publish<EmailDataRequest>(_configuration["EmailErrorQueue"], data);
                    }
                }
            }

        }

        /// <summary>
        ///  Process Fb Report Message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task ProcessFbReportMessage(MessageModel message)
        {
            using (var scope = Services.CreateScope())
            {

                Thread.Sleep(10000);
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IFBReportManager>();

                var data = message.Payload as FbReportFetchRequest;

                try
                {
                    var fbToken = getFbToken();

                    if (data != null && data.ReportId > 0 && !string.IsNullOrEmpty(fbToken))
                    {
                        var result = await scopedProcessingService.FetchBulkFBReport(data.FbReportId, data.ReportId, fbToken);
                        if (result.IsNewReportFormatCheckPoint)
                        {
                            await _rabbitMqClient.Publish<FastReportRequest>(_configuration["FastReportQueue"], new FastReportRequest() { ReportIds = new List<int>() { data.ReportId }, BookingId = result.InspectionId, EntityId = result.EntityId });
                        }
                    }

                }
                catch (Exception ex)
                {

                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            await Task.CompletedTask;
        }

        /// <summary>
        ///  Push the master data update to FB
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task ProcessAccountData(MessageModel message)
        {
            try
            {
                var data = message.Payload as MasterDataRequest;

                using (var scope = Services.CreateScope())
                {
                    var scopedEventLog =
                           scope.ServiceProvider
                               .GetRequiredService<IEventBookingLogManager>();
                    await scopedEventLog.SaveTCFMasterRequestLog(new TCFMasterRequestLogInfo()
                    {
                        AccountId = data.SearchId,
                        DataType = (int)TCFDataType.Customer,
                        LogInformation = JsonConvert.SerializeObject(data),
                        ResponseMessage = "After reached the queue(Process Account Data)"
                    });
                }

                if (data.ExternalClient == ExternalClient.FullBridge)
                    await ProcessFBAccount(data);
                else if (data.ExternalClient == ExternalClient.TCF)
                    await ProcessTCFACcount(data);

            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// process the fb master data creation
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task ProcessFBAccount(MasterDataRequest data)
        {
            var fbToken = getFbToken();

            switch (data.MasterDataType)
            {
                case MasterDataType.CustomerCreation:
                    //process the fb customer creation/updation
                    await ProcessFBCustomerAccount(data.SearchId, fbToken, data.EntityId);
                    break;
                case MasterDataType.SupplierCreation:
                    //process the fb supplier creation/updation
                    await ProcessFBSupplierAccount(data.SearchId, fbToken, data.EntityId);
                    break;
                case MasterDataType.FactoryCreation:
                    //process factory account creation or updation
                    await ProcessFBFactoryAccount(data.SearchId, fbToken, data.EntityId);
                    break;
                case MasterDataType.FactoryUpdation:
                    {
                        //process supplier account creation or updation
                        await ProcessFBFactoryAccount(data.SearchId, fbToken, data.EntityId);
                    }
                    break;
                case MasterDataType.ProductCreation:
                    //process product creation or updation
                    await ProcessProductAccount(data.SearchId, data.EntityId, fbToken);
                    break;
            }

        }

        /// <summary>
        /// Process the TCF Master Data Creation/Updation
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task ProcessTCFACcount(MasterDataRequest data)
        {
            switch (data.MasterDataType)
            {
                case MasterDataType.SupplierCreation:
                    //process the tcf supplier creation/updation
                    await ProcessTCFSupplierAccount(data.SearchId);
                    break;
                case MasterDataType.FactoryCreation:
                    //process the tcf supplier creation/updation
                    await ProcessTCFSupplierAccount(data.SearchId);
                    break;
                case MasterDataType.ProductCreation:
                    //process the tcf product creation/updation
                    await ProcessTCFProduct(data.SearchId, data.EntityId);
                    break;
                case MasterDataType.CustomerContactCreation:
                    //process customer contact creation or updation
                    await ProcessTCFCustomerContact(data.SearchId);
                    break;
                case MasterDataType.UserCreation:
                    //process user creation or updation
                    await ProcessTCFUser(data.SearchId);
                    break;
                case MasterDataType.BuyerCreation:
                    //process buyer creation or updation
                    await ProcessTCFBuyer(data.SearchId, data.EntityId);
                    break;
                case MasterDataType.CustomerCreation:
                    {
                        using (var scope = Services.CreateScope())
                        {
                            var scopedEventLog =
                                   scope.ServiceProvider
                                       .GetRequiredService<IEventBookingLogManager>();
                            await scopedEventLog.SaveTCFMasterRequestLog(new TCFMasterRequestLogInfo()
                            {
                                AccountId = data.SearchId,
                                DataType = (int)TCFDataType.Customer,
                                LogInformation = JsonConvert.SerializeObject(data),
                                ResponseMessage = "Before calling the tcf manager to push customer"
                            });
                        }
                        //process the tcf supplier creation/updation
                        await ProcessTCFCustomerAccount(data.SearchId);
                    }
                    break;
            }
        }

        private async Task ProcessTCFCustomerAccount(int customerId)
        {
            using (var scope = Services.CreateScope())
            {
                #region ScopeVariableDeclaration
                var scopedTCFService =
                scope.ServiceProvider
                    .GetRequiredService<ITCFManager>();

                #endregion ScopeVariableDeclaration

                var response = await scopedTCFService.SaveCustomerToTCF(customerId);

                //updates the customer details to fb
                if (!response.IsSuccess)
                {
                    //create the task if supplier failed to update in fb.
                    await CreateTask(TaskType.UpdateCustomerToTCF, customerId, response.ResponseMessage);
                }
                else
                {
                    //update the task to done if 
                    await UpdateTask(TaskType.UpdateCustomerToTCF, customerId);
                }
            }
        }

        /// <summary>
        /// Process the customer account
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="fbToken"></param>
        /// <param name="masterDataMap"></param>
        /// <returns></returns>
        private async Task ProcessFBCustomerAccount(int customerId, string fbToken, int entityId)
        {
            using (var scope = Services.CreateScope())
            {
                #region ScopeVariableDeclartion
                var scopedFBService =
                    scope.ServiceProvider
                        .GetRequiredService<IFBReportManager>();

                var scopedFBLogService =
                    scope.ServiceProvider
                        .GetRequiredService<IEventBookingLogManager>();

                #endregion ScopeVariableDeclartion

                try
                {

                    if (!string.IsNullOrEmpty(fbToken))
                    {
                        //save customer data to FB
                        if (!await scopedFBService.SaveCustomerMasterDataToFB(customerId, fbToken, entityId))
                        {
                            //create the task if customer failed to update in fb
                            await CreateTask(TaskType.UpdateCustomerToFB, customerId);
                        }
                        else
                        {
                            //update the task is done if it is created in fb
                            await UpdateTask(TaskType.UpdateCustomerToFB, customerId);
                        }
                    }

                }
                catch (Exception ex)
                {
                    await scopedFBLogService.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        RequestUrl = "FB Customer Update Issue in Queue",
                        LogInformation = ex.Message.ToString()
                    });
                    //create the task if customer failed to update in fb
                    await CreateTask(TaskType.UpdateCustomerToFB, customerId);
                }
            }
        }

        /// <summary>
        /// update the supplier data to TCF
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="tcfToken"></param>
        /// <param name="masterDataAction"></param>
        /// <returns></returns>
        private async Task ProcessTCFSupplierAccount(int supplierId)
        {
            using (var scope = Services.CreateScope())
            {
                #region ScopeVariableDeclaration
                var scopedFBService =
                scope.ServiceProvider
                    .GetRequiredService<ITCFManager>();

                #endregion ScopeVariableDeclaration

                var response = await scopedFBService.SaveSupplierToTCF(supplierId);

                //updates the supplier details to fb
                if (!response.IsSuccess)
                {
                    //create the task if supplier failed to update in fb.
                    await CreateTask(TaskType.UpdateSupplierToTCF, supplierId, response.ResponseMessage);
                }
                else
                {
                    //update the task to done if 
                    await UpdateTask(TaskType.UpdateSupplierToTCF, supplierId);
                }
            }
        }

        /// <summary>
        /// update the supplier data to FB
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        private async Task ProcessFBSupplierAccount(int supplierId, string fbToken, int entityId)
        {
            using (var scope = Services.CreateScope())
            {
                #region ScopeVariableDeclaration
                var scopedFBService =
                scope.ServiceProvider
                    .GetRequiredService<IFBReportManager>();

                var scopedFBLogService =
                    scope.ServiceProvider
                        .GetRequiredService<IEventBookingLogManager>();

                #endregion ScopeVariableDeclaration

                try
                {
                    if (!string.IsNullOrEmpty(fbToken))
                    {
                        //updates the supplier details to fb
                        if (!await scopedFBService.SaveSupplierMasterDataToFB(supplierId, fbToken, entityId))
                        {
                            //create the task if supplier failed to update in fb.
                            await CreateTask(TaskType.UpdateSupplierToFB, supplierId);
                        }
                        else
                        {
                            //update the task to done if 
                            await UpdateTask(TaskType.UpdateSupplierToFB, supplierId);
                        }
                    }

                }
                catch (Exception ex)
                {
                    await scopedFBLogService.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        RequestUrl = "FB Supplier Update Issue in Queue",
                        LogInformation = ex.Message.ToString()
                    });
                    await CreateTask(TaskType.UpdateSupplierToFB, supplierId);
                }
            }
        }

        private async Task<int> GetFactoryBySupplierId(int supplierId)
        {
            using (var scope = Services.CreateScope())
            {
                var scopedSupplierService =
                    scope.ServiceProvider
                        .GetRequiredService<ISupplierManager>();
                return await scopedSupplierService.GetFactoryIdBySupplierId(supplierId);
            }
        }

        private async Task ProcessFBFactoryAccount(int factoryId, string fbToken, int entityId)
        {
            using (var scope = Services.CreateScope())
            {

                var scopedFBService =
                    scope.ServiceProvider
                        .GetRequiredService<IFBReportManager>();

                var scopedFBLogService =
                    scope.ServiceProvider
                        .GetRequiredService<IEventBookingLogManager>();

                try
                {

                    if (!string.IsNullOrEmpty(fbToken))
                    {
                        if (!await scopedFBService.SaveFactoryMasterDataToFB(factoryId, fbToken, entityId))
                        {
                            await CreateTask(TaskType.UpdateFactoryToFB, factoryId);
                        }
                        else
                        {
                            await UpdateTask(TaskType.UpdateFactoryToFB, factoryId);
                        }
                    }

                }
                catch (Exception ex)
                {
                    await scopedFBLogService.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        RequestUrl = "FB Factory Update Issue in Queue",
                        LogInformation = ex.Message.ToString()
                    });
                    await CreateTask(TaskType.UpdateFactoryToFB, factoryId);
                }
            }
        }

        /// <summary>
        /// Update product data to FB
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="fbToken"></param>
        /// <param name="masterDataMap"></param>
        /// <returns></returns>
        private async Task ProcessProductAccount(int productId, int? entityId, string fbToken)
        {
            using (var scope = Services.CreateScope())
            {

                var scopedFBService =
                    scope.ServiceProvider
                        .GetRequiredService<IFBReportManager>();

                var scopedFBLogService =
                  scope.ServiceProvider
                         .GetRequiredService<IEventBookingLogManager>();

                await scopedFBLogService.SaveFbBookingRequestLog(new FBBookingLogInfo()
                {
                    RequestUrl = "Before FB SaveProductMasterDataToFB " + entityId.ToString(),
                    LogInformation = "Before FB SaveProductMasterDataToFB " + productId.ToString()
                });

                try
                {

                    if (!string.IsNullOrEmpty(fbToken))
                    {
                        if (!await scopedFBService.SaveProductMasterDataToFB(productId, entityId, fbToken))
                        {
                            await CreateTask(TaskType.UpdateFactoryToFB, productId);
                        }
                        else
                        {
                            await UpdateTask(TaskType.UpdateFactoryToFB, productId);
                        }
                    }

                }
                catch (Exception)
                {
                    await CreateTask(TaskType.UpdateFactoryToFB, productId);
                }
            }
        }

        private async Task CreateTask(TaskType taskType, int linkId, string taskReason = "")
        {
            using (var scope = Services.CreateScope())
            {
                var scopedUserRightsService =
                  scope.ServiceProvider
                      .GetRequiredService<IUserRightsManager>();

                var taskList = await scopedUserRightsService.GetTask(linkId, new[] { (int)taskType }, false);

                if (taskList == null || taskList.Count() == 0)
                {
                    var userData = Convert.ToString(_configuration["FailedTaskITUserIds"]);
                    List<int> userList = userData.Split(',').Select(int.Parse).ToList();
                    var customUserID = Convert.ToInt32(_configuration["ExternalAccessorUserId"]);
                    await scopedUserRightsService.AddTask(taskType, linkId, userList, customUserID);
                }

            }

        }

        private async Task UpdateTask(TaskType taskType, int linkId)
        {
            using (var scope = Services.CreateScope())
            {
                var scopedUserRightsService =
                  scope.ServiceProvider
                      .GetRequiredService<IUserRightsManager>();

                await scopedUserRightsService.UpdateTask(linkId, new[] { (int)taskType }, false);

            }

        }

        /// <summary>
        /// updates the customer contact to TCF
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="tcfToken"></param>
        /// <returns></returns>
        private async Task ProcessTCFCustomerContact(int contactId)
        {
            using (var scope = Services.CreateScope())
            {
                #region ScopeVariableDeclartion
                var scopedTCFService =
                    scope.ServiceProvider
                        .GetRequiredService<ITCFManager>();

                #endregion ScopeVariableDeclartion

                try
                {

                    var response = await scopedTCFService.SaveCustomerContactToTCF(contactId);
                    //save customer data to FB
                    if (!response.IsSuccess)
                    {
                        //create the task if customer failed to update in fb
                        await CreateTask(TaskType.UpdateCustomerContactToTCF, contactId, response.ResponseMessage);
                    }
                    else
                    {
                        //update the task is done if it is created in fb
                        await UpdateTask(TaskType.UpdateCustomerContactToTCF, contactId);
                    }


                }
                catch (Exception)
                {
                    //create the task if customer failed to update in fb
                    await CreateTask(TaskType.UpdateCustomerToFB, contactId);
                }
            }
        }

        /// <summary>
        /// Process user info pushed to TCF
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="masterDataAction"></param>
        /// <returns></returns>
        private async Task ProcessTCFUser(int userId)
        {
            using (var scope = Services.CreateScope())
            {
                #region ScopeVariableDeclartion
                var scopedTCFService =
                    scope.ServiceProvider
                        .GetRequiredService<ITCFManager>();

                #endregion ScopeVariableDeclartion

                try
                {
                    var response = await scopedTCFService.SaveUserToTCF(userId);

                    //save customer data to FB
                    if (response != null && !response.IsSuccess)
                    {
                        //create the task if customer failed to update in fb
                        await CreateTask(TaskType.UpdateUserToTCF, userId, response.ResponseMessage);
                    }
                    else
                    {
                        //update the task is done if it is created in fb
                        await UpdateTask(TaskType.UpdateUserToTCF, userId);
                    }

                }
                catch (Exception)
                {
                    //create the task if customer failed to update in fb
                    await CreateTask(TaskType.UpdateUserToTCF, userId);
                }
            }
        }

        /// <summary>
        /// Updates the tcf info to buyer
        /// </summary>
        /// <param name="buyerList"></param>
        /// <param name="tcfToken"></param>
        /// <returns></returns>
        private async Task ProcessTCFBuyer(int customerId, int? entityId)
        {
            using (var scope = Services.CreateScope())
            {
                #region ScopeVariableDeclaration
                var scopedFBService =
                scope.ServiceProvider
                    .GetRequiredService<ITCFManager>();

                #endregion ScopeVariableDeclaration

                var response = await scopedFBService.SaveBuyerListToTCF(customerId, entityId);

                //updates the supplier details to fb
                if (!response.IsSuccess)
                {
                    //create the task if supplier failed to update in fb.
                    await CreateTask(TaskType.UpdateBuyerToTCF, customerId, response.ResponseMessage);

                }
                else
                {
                    //update the task to done if 
                    await UpdateTask(TaskType.UpdateBuyerToTCF, customerId);

                }
            }
        }

        /// <summary>
        /// update the product information to TCF
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="tcfToken"></param>
        /// <returns></returns>
        private async Task ProcessTCFProduct(int productId, int? entityId)
        {
            using (var scope = Services.CreateScope())
            {
                #region ScopeVariableDeclaration
                var scopedFBService =
                scope.ServiceProvider
                    .GetRequiredService<ITCFManager>();

                #endregion ScopeVariableDeclaration

                var response = await scopedFBService.SaveProductToTCF(productId, entityId);

                //updates the supplier details to fb
                if (!response.IsSuccess)
                {
                    //create the task if supplier failed to update in fb.
                    await CreateTask(TaskType.UpdateProductToTCF, productId, response.ResponseMessage);

                }
                else
                {
                    //update the task to done if 
                    await UpdateTask(TaskType.UpdateProductToTCF, productId);

                }
            }
        }

        /// <summary>
        /// process the file attachments
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task ProcessFileAttachments(MessageModel message)
        {
            try
            {
                _logger.LogInformation("Process the file attachments to zip starts");
                var data = message.Payload as FileAttachmentToZipRequest;

                using (var scope = Services.CreateScope())
                {
                    #region ScopeVariableDeclartion
                    var scopedScheduleJobManager =
                        scope.ServiceProvider
                            .GetRequiredService<IScheduleJobManager>();

                    #endregion ScopeVariableDeclartion

                    try
                    {
                        await scopedScheduleJobManager.UploadInspectionAttachementsAsZipToCloud(data);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation("Process the file attachments to zip failed message", ex.Message.ToString());
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

        private async Task ProcessFastReportQueue(MessageModel message)
        {
            try
            {
                _logger.LogInformation("Process the fast report starts");
                var data = message.Payload as FastReportRequest;

                using (var scope = Services.CreateScope())
                {
                    #region ScopeVariableDeclartion
                    var scopedScheduleJobManager =
                        scope.ServiceProvider
                            .GetRequiredService<IScheduleJobManager>();



                    #endregion ScopeVariableDeclartion

                    try
                    {
                        var token = AuthentificationService.GenerateAPIToken(_configuration);
                        var fbToken = getFbToken();
                        await scopedScheduleJobManager.ProcessInspectionFastReport(data, fbToken, token);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation("Process the file attachments to zip failed message", ex.Message.ToString());
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

        }
        private string getFbToken()
        {
            var Fbclaims = new List<Claim>
            {
                new Claim("email",_configuration["FbAdminEmail"]),
                new Claim("firstname", _configuration["FbAdminUserName"]),
                new Claim("lastname", ""),
                new Claim("role", "admin"),
                new Claim("redirect", "")
            };
            return AuthentificationService.CreateFBToken(Fbclaims, _configuration["FBKey"]);
        }
        private async Task ProcessFbBookingRequest(MessageModel message)
        {
            var data = message.Payload as FbBookingRequest;

            using (var scope = Services.CreateScope())
            {
                Thread.Sleep(10000);

                var Fbclaims = new List<Claim>
                    {
                        new Claim("email",_configuration["FbAdminEmail"]),
                        new Claim("firstname", _configuration["FbAdminUserName"]),
                        new Claim("lastname", ""),
                        new Claim("role", "admin"),
                        new Claim("redirect", "")
                    };

                var fbToken = AuthentificationService.CreateFBToken(Fbclaims, _configuration["FBKey"]);

                var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IBookingEmailLogQueueManager>();

                var bookingFbLog = await scopedProcessingService.GetBookingFbQueueLogById(data.ResultId);

                if (bookingFbLog != null)
                {
                    try
                    {
                        //foreach
                        switch (bookingFbLog.FbBookingSyncType)
                        {
                            case (int)FbBookingSyncType.AuditCreation:
                                await AuditCreationForFB(bookingFbLog.BookingId, bookingFbLog.EntityId, fbToken);
                                break;
                            case (int)FbBookingSyncType.AuditUpdation:
                                await AuditUpdationForFB(bookingFbLog, fbToken);
                                break;
                            case (int)FbBookingSyncType.AuditBookingCancellation:
                                await AuditCancelToFb(bookingFbLog, fbToken, false);
                                break;
                            case (int)FbBookingSyncType.InspectionUpdation:
                                await BookingUpdationForFB(bookingFbLog, fbToken);
                                break;
                        }

                        bookingFbLog.TryCount = data.TryCount;
                        bookingFbLog.Status = (int)FbBookingQueueStatus.Successs;
                        await scopedProcessingService.UpdateBookingFbQueueLog(bookingFbLog);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation("Email Sending Error:" + ex.Message.ToString() + "");
                        if (data.TryCount <= Int32.Parse(_configuration["FbBookingQueueTryCount"]))
                        {
                            bookingFbLog.Status = (int)FbBookingQueueStatus.Failure;
                            bookingFbLog.TryCount = data.TryCount;
                            await scopedProcessingService.UpdateBookingFbQueueLog(bookingFbLog);
                            //Thread.Sleep(300000);

                            data.TryCount = data.TryCount + 1;
                            await _rabbitMqClient.Publish<FbBookingRequest>(_configuration["FBBookingSyncQueue"], data);
                        }
                        else
                        {

                        }
                    }

                }
            }
        }

        /// <summary>
        /// generate fb token with new rsa key
        /// </summary>
        /// <param name="rsa"></param>
        /// <returns></returns>
        private string getFbTokenByRsa(string rsa)
        {
            var Fbclaims = new List<Claim>
            {
                new Claim("email",_configuration["FbAdminEmail"]),
                new Claim("firstname", _configuration["FbAdminUserName"]),
                new Claim("lastname", ""),
                new Claim("role", "admin"),
                new Claim("redirect", "")
            };
            return AuthentificationService.CreateFBToken(Fbclaims, rsa);
        }

        private async Task AuditCreationForFB(int bookingId, int entityId, string fbToken)
        {
            using (var scope = Services.CreateScope())
            {
                #region ScopeVariableDeclaration
                var scopedFBService =
                scope.ServiceProvider
                    .GetRequiredService<IFBReportManager>();

                var scopedFBLogService =
                    scope.ServiceProvider
                        .GetRequiredService<IEventBookingLogManager>();

                #endregion ScopeVariableDeclaration

                try
                {
                    if (!string.IsNullOrEmpty(fbToken))
                    {
                        await scopedFBService.SaveAuditMasterDataToFB(bookingId, entityId, fbToken); // null = entityId                        
                    }

                }
                catch (Exception ex)
                {
                    await scopedFBLogService.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        RequestUrl = "FB Audit Data Issue in Queue",
                        ServiceId = (int)Service.AuditId,
                        LogInformation = ex.Message.ToString()
                    });
                    throw ex;
                }
            }
        }

        private async Task AuditUpdationForFB(LogBookingFbQueue logBookingFb, string fbToken)
        {
            using (var scope = Services.CreateScope())
            {
                #region ScopeVariableDeclaration
                var scopedFBService =
                scope.ServiceProvider
                    .GetRequiredService<IFBReportManager>();

                var scopedFBLogService =
                    scope.ServiceProvider
                        .GetRequiredService<IEventBookingLogManager>();


                var scopedAuditRepository =
                  scope.ServiceProvider
                      .GetRequiredService<IAuditRepository>();

                #endregion ScopeVariableDeclaration
                StringBuilder failureResult = new StringBuilder();
                try
                {
                    if (!string.IsNullOrEmpty(fbToken))
                    {
                        var audit = await scopedAuditRepository.GetAuditData(logBookingFb.BookingId);
                        var auditTranFiles = await scopedAuditRepository.GetAuditTranFiles(audit.Id);
                        if (audit.FbmissionId == null || logBookingFb.IsMissionUpdated == true)
                        {
                            bool isMissionNotCreated = audit.FbmissionId == null;

                            var auditFbReportDetails = await scopedAuditRepository.GetAuditForFbReportDetails(audit.Id);
                            var auditCustomerContactDetails = await scopedAuditRepository.GetAuditCustomerContacts(audit.Id);
                            var auditSupplierContactDetails = await scopedAuditRepository.GetAuditSupplierContacts(audit.Id);

                            if (!await scopedFBService.SaveAuditMissionDataToFB(audit, auditFbReportDetails, auditCustomerContactDetails, auditSupplierContactDetails, fbToken))
                            {
                                failureResult.Append(FBFailure.MissionSave + "--failure" + "\n");
                                throw new Exception();
                            }
                            if (isMissionNotCreated)
                            {
                                if (!await scopedFBService.CreateAuditReportRequest(audit, fbToken))
                                {
                                    failureResult.Append(FBFailure.MissionSave + "--done" + "\n");
                                    failureResult.Append(FBFailure.MissionUrl + "--done" + "\n");
                                    failureResult.Append(FBFailure.ReportSave + "--failure" + "\n");
                                    throw new Exception();
                                }

                            }
                            else
                            {
                                if (audit.CuProductCategoryNavigation != null)
                                {
                                    if (!await scopedFBService.UpdateReportProductCategory(audit, fbToken))
                                    {
                                        failureResult.Append(FBFailure.MissionSave + "--done" + "\n");
                                        failureResult.Append(FBFailure.MissionUrl + "--done" + "\n");
                                        failureResult.Append(FBFailure.ReportSave + "--failure" + "\n");
                                        throw new Exception();
                                    }
                                }
                            }
                        }

                        if (!await scopedFBService.SaveAuditMissionUrlsDataToFb(audit, auditTranFiles, fbToken))
                        {
                            failureResult.Append(FBFailure.MissionSave + "--done" + "\n");
                            failureResult.Append(FBFailure.MissionUrl + "--failure" + "\n");
                            throw new Exception();
                        }
                        if (audit.StatusId == (int)AuditStatus.Received || audit.StatusId == (int)AuditStatus.Confirmed || audit.StatusId == (int)AuditStatus.Scheduled || audit.StatusId == (int)AuditStatus.Rescheduled)
                        {
                            if (!await scopedFBService.CreateUserToFBMissionForAudit(audit, fbToken))
                            {
                                failureResult.Append(FBFailure.UserSave + "--failure" + "\n");
                                throw new Exception();
                            }
                        }

                    }

                }
                catch (Exception ex)
                {
                    await scopedFBLogService.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        RequestUrl = "FB Audit Data Issue in Queue",
                        BookingId = logBookingFb.BookingId,
                        ServiceId = (int)Service.AuditId,
                        LogInformation = failureResult.ToString()
                    });
                    ex.Source = failureResult.ToString();
                    throw ex;
                }
            }
        }


        private async Task AuditCancelToFb(LogBookingFbQueue logBookingFbQueue, string fbToken, bool isInspectesd)
        {
            using (var scope = Services.CreateScope())
            {
                #region ScopeVariableDeclaration
                var scopedFBService =
                scope.ServiceProvider
                    .GetRequiredService<IFBReportManager>();

                var scopedFBLogService =
                    scope.ServiceProvider
                        .GetRequiredService<IEventBookingLogManager>();

                var scopedAuditRepository =
                    scope.ServiceProvider
                        .GetRequiredService<IAuditRepository>();

                #endregion ScopeVariableDeclaration
                StringBuilder failureResult = new StringBuilder();
                try
                {
                    if (!string.IsNullOrEmpty(fbToken))
                    {
                        var audit = await scopedAuditRepository.GetAuditData(logBookingFbQueue.BookingId);
                        var fbDeleteFbMissionResponse = await scopedFBService.DeleteAuditMission(audit, fbToken);
                        if (!fbDeleteFbMissionResponse)
                        {
                            failureResult.Append(FBFailure.DeleteMission + "--failure" + "\n");
                            throw new Exception();
                        }
                    }
                    else
                    {
                        failureResult.Append(FBFailure.DeleteMission + "--failure" + "\n");
                        throw new Exception();
                    }

                }
                catch (Exception ex)
                {
                    await scopedFBLogService.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        RequestUrl = "FB Audit Data Issue in Queue",
                        ServiceId = (int)Service.AuditId,
                        LogInformation = failureResult.ToString()
                    });
                    throw ex;
                }
            }
        }

        private async Task BookingUpdationForFB(LogBookingFbQueue logBookingFb, string fbToken)
        {
            using (var scope = Services.CreateScope())
            {
                #region ScopeVariableDeclaration
                var scopedFBService =
                scope.ServiceProvider
                    .GetRequiredService<IFBReportManager>();

                var scopedFBLogService =
                    scope.ServiceProvider
                        .GetRequiredService<IEventBookingLogManager>();


                var scopedInspectionBookingRepository =
                  scope.ServiceProvider
                      .GetRequiredService<IInspectionBookingRepository>();

                #endregion ScopeVariableDeclaration
                StringBuilder failureResult = new StringBuilder();
                try
                {
                    if (!string.IsNullOrEmpty(fbToken))
                    {
                        var inspection = await scopedInspectionBookingRepository.GetInspectionWithFileAttachment(logBookingFb.BookingId);
                        if (inspection == null)
                        {
                            failureResult.Append(FBFailure.InspectionDataNotFound + "--failure" + "\n");
                            throw new Exception();
                        }
                        if (inspection.FbMissionId.HasValue && inspection.FbMissionId.Value > 0)
                        {
                            if (!await scopedFBService.SaveMissionUrlsDataToFb(inspection, fbToken))
                            {
                                failureResult.Append(FBFailure.MissionUrl + "--failure" + "\n");
                                throw new Exception();
                            }
                        }

                    }

                }
                catch (Exception ex)
                {
                    await scopedFBLogService.SaveFbBookingRequestLog(new FBBookingLogInfo()
                    {
                        RequestUrl = "FB Audit Data Issue in Queue",
                        BookingId = logBookingFb.BookingId,
                        ServiceId = (int)Service.AuditId,
                        LogInformation = failureResult.ToString()
                    });
                    ex.Source = failureResult.ToString();
                    throw ex;
                }
            }
        }
    }
}
