using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace EmailSchedulerService
{                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
    public class SchedulerDataAccess : ISchedulerDataAccess
    {

        public void ScheduleProcess(string[] scheduleOptions)
        {

            //Getting the configuration data
            IConfigurationRoot configuration = GetConfigurationData();

            ILoggerFactory loggerfactory = new LoggerFactory();
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            loggerfactory.AddSerilog();

            Log.Information("Api Call Started");

            try
            {
                Log.Information("Getting token for API application started");
                AuthorizationServerAnswer authorizationServerToken;
                authorizationServerToken = GenerateAPIToken(configuration);
                Log.Information("Getting token for API application ended");

                Log.Information("Schedule Started");

                // first option - schedule type
                string scheduleOption = scheduleOptions.Length > 0 ? scheduleOptions[0] : "0";

                switch (Int32.Parse(scheduleOption))
                {
                    case (int)ScheduleOptions.ScheduleQcEmail:
                        // first option - schedule type, second option - office ids, third - entity id
                        if (scheduleQCEmailAPI(authorizationServerToken, configuration, scheduleOptions))
                        {
                            Log.Information("ScheduleQCEmail Processed successfully");
                        }
                        break;

                    case (int)ScheduleOptions.ScheduleFbReport:
                        // first option - schedule type, second option - entity id
                        if (scheduleFbReportFetch(authorizationServerToken, configuration, scheduleOptions))
                        {
                            Log.Information("Schedule Report fecth Processed successfully");
                        }
                        break;

                    case (int)ScheduleOptions.ScheduleCulturaPackingInfo:
                        // first option - schedule type, second option - entity id
                        if (scheduleCulturaPackingInfo(authorizationServerToken, configuration, scheduleOptions))
                        {
                            Log.Information("Schedule job Processed successfully");
                        }
                        break;

                    case (int)ScheduleOptions.ScheduleTravelTariffEmail:
                        // first option - schedule type, second option - entity id
                        if (scheduleTravelTariffEmail(authorizationServerToken, configuration, scheduleOptions))
                        {
                            Log.Information("Schedule Travel tariff  email Processed successfully");
                        }
                        break;

                    case (int)ScheduleOptions.ScheduleAutoQcExpense:
                        // first option - schedule type, second option - entity id
                        if (scheduleAutoQcExpense(authorizationServerToken, configuration, scheduleOptions))
                        {
                            Log.Information("Schedule Auto Qc Expense Processed successfully");
                        }
                        break;

                    case (int)ScheduleOptions.ScheduleQcInspectionExpenseEmail:
                        // first option - schedule type, second option - entity id
                        if (scheduleQcExpenseEmail(authorizationServerToken, configuration, scheduleOptions))
                        {
                            Log.Information("Schedule Qc Inspection Expense email Processed successfully");
                        }
                        break;
                    case (int)ScheduleOptions.ScheduleClaimReminderEmail:
                        // first option - schedule type, second option - entity id
                        if (scheduleClaimReminderEmail(authorizationServerToken, configuration, scheduleOptions))
                        {
                            Log.Information("Schedule Claim Reminder email Processed successfully");
                        }
                        break;
                    case (int)ScheduleOptions.ScheduleFastReport:
                        // first option - schedule type, second option - entity id
                        if (ScheduleFastReport(authorizationServerToken, configuration, scheduleOptions))
                        {
                            Log.Information("Schedule Fast report Processed successfully");
                        }
                        break;

                    case (int)ScheduleOptions.ScheduleCarrefourDailyResult:
                        // first option - schedule type, second option - entity id
                        if (scheduleCarrefourDailyResultEmail(authorizationServerToken, configuration, scheduleOptions))
                        {
                            Log.Information("Schedule Carrefour Daily Result  Email Processed successfully");
                        }
                        break;

                    case (int)ScheduleOptions.InsertBookingFilesAsZip:
                        // first option - schedule type, second option - entity id
                        if (InsertBookingFilesAsZip(authorizationServerToken, configuration, scheduleOptions))
                        {
                            Log.Information("Insert Booking Files to zip processed successfully");
                        }
                        break;
                    case (int)ScheduleOptions.ScheduleBookingCS:
                        // first option - schedule type, second option - entity id
                        if (ScheduleBookingCS(authorizationServerToken, configuration, scheduleOptions))
                        {
                            Log.Information("Schedule booking cs processed successfully");
                        }
                        break;
                        
                    case (int)ScheduleOptions.ScheduleMissedMSchart:
                        // first option - schedule type, second option - entity id
                        if (ScheduleMissedMSchart(authorizationServerToken, configuration, scheduleOptions))
                        {
                            Log.Information("Schedule Missed MSchart Result  Email Processed successfully");
                        }
                        break;

                    case (int)ScheduleOptions.SchedulePlanningForCS:
                        // first option - schedule type, second option - entity id , third - office ids
                        if (SchedulePlanningForCS(authorizationServerToken, configuration, scheduleOptions))
                        {
                            Log.Information("Schedule Planning For CS Processed successfully");
                        }
                        break;
                    case (int)ScheduleOptions.BBGInitialBookingExtract:
                        if (BBGInitialBookingExtract(authorizationServerToken, configuration, scheduleOptions))
                        {
                            Log.Information("BBG Initial Booking Extract Processed successfully");
                        }
                        break;
                }


                Log.Information("Schedule Ended");

            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message.ToString());
            }
        }

        //Getting the configuration data from the appsettings.json
        public IConfigurationRoot GetConfigurationData()
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();

        }

        //Generating the API Token using the api client id and client secret id
        public AuthorizationServerAnswer GenerateAPIToken(IConfigurationRoot configuration)
        {
            var client = new HttpClient();
            IdentityServerModel identityServerModel = new IdentityServerModel
            {
                ClientId = Convert.ToString(configuration["APIClientID"]),
                ClientSecret = Convert.ToString(configuration["APIClientSecret"])
            };
            Log.Information("Get the token using API credentials starts");
            var tokenResponse = client.PostAsync(Convert.ToString(configuration["IdentityServerUrl"]), new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(identityServerModel), Encoding.UTF8, "application/json")).Result;
            var token = tokenResponse.Content.ReadAsStringAsync().Result;
            Log.Information("Get the token using API credentials ends " + token);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<AuthorizationServerAnswer>(token);
        }

        //Call the schedule qc api to send emails
        public bool scheduleQCEmailAPI(AuthorizationServerAnswer authorizationServerToken, IConfigurationRoot configuration, string[] scheduleOptions)
        {
            Log.Information("read schedule api url starts");

            string QCScheduleEmailAPIUrl = Convert.ToString(configuration["ScheduleQCEmailAPIURL"]);
            string strOfficeId = scheduleOptions.Length > 0 ? scheduleOptions.Skip(1).FirstOrDefault() : string.Empty;

            Log.Information("read schedule api url starts for -" + strOfficeId + " ");

            QCScheduleEmailAPIUrl = QCScheduleEmailAPIUrl + "?offices=" + strOfficeId;

            Log.Information("read schedule api url ends- " + QCScheduleEmailAPIUrl + "");
            bool IsSuccessStatus = false;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                           new AuthenticationHeaderValue("Bearer", authorizationServerToken.access_token);

                // set entity id from the background job - from third param
                string entityId = scheduleOptions.Length > 0 ? scheduleOptions.Skip(2).FirstOrDefault() : string.Empty;
                if (!string.IsNullOrWhiteSpace(entityId))
                {
                    var entityIdEncrypted = EncryptStringAES(entityId);
                    httpClient.DefaultRequestHeaders.Add("entityId", entityIdEncrypted);
                }

                HttpResponseMessage response = httpClient.GetAsync(QCScheduleEmailAPIUrl).Result;
                IsSuccessStatus = response.IsSuccessStatusCode;

            }
            return IsSuccessStatus;
        }

        /// <summary>
        /// Schedule Fb Report fetch
        /// </summary>
        /// <param name="authorizationServerToken"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public bool scheduleFbReportFetch(AuthorizationServerAnswer authorizationServerToken, IConfigurationRoot configuration, string[] scheduleOptions)
        {
            Log.Information("read schedule api url starts");

            string ScheduleFbReportFetchUrl = Convert.ToString(configuration["ScheduleFbReportFetchAPI"]);

            Log.Information("read schedule api url ends- " + ScheduleFbReportFetchUrl + "");

            bool IsSuccessStatus = false;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                           new AuthenticationHeaderValue("Bearer", authorizationServerToken.access_token);

                // set entity id from the background job
                string entityId = scheduleOptions.Length > 0 ? scheduleOptions.Skip(1).FirstOrDefault() : string.Empty;
                if (!string.IsNullOrWhiteSpace(entityId))
                {
                    var entityIdEncrypted = EncryptStringAES(entityId);
                    httpClient.DefaultRequestHeaders.Add("entityId", entityIdEncrypted);
                }

                HttpResponseMessage response = httpClient.GetAsync(ScheduleFbReportFetchUrl).Result;
                IsSuccessStatus = response.IsSuccessStatusCode;
            }

            return IsSuccessStatus;
        }


        /// <summary>
        /// Schedule Fb Report fetch
        /// </summary>
        /// <param name="authorizationServerToken"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public bool ScheduleFastReport(AuthorizationServerAnswer authorizationServerToken, IConfigurationRoot configuration, string[] scheduleOptions)
        {
            Log.Information("read schedule api url starts");

            string ScheduleFbReportUrl = Convert.ToString(configuration["ScheduleFastReportAPI"]);

            Log.Information("read schedule api url ends- " + ScheduleFbReportUrl + "");

            bool IsSuccessStatus = false;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                           new AuthenticationHeaderValue("Bearer", authorizationServerToken.access_token);

                // set entity id from the background job
                string entityId = scheduleOptions.Length > 0 ? scheduleOptions.Skip(1).FirstOrDefault() : string.Empty;
                if (!string.IsNullOrWhiteSpace(entityId))
                {
                    var entityIdEncrypted = EncryptStringAES(entityId);
                    httpClient.DefaultRequestHeaders.Add("entityId", entityIdEncrypted);
                }

                HttpResponseMessage response = httpClient.GetAsync(ScheduleFbReportUrl).Result;
                IsSuccessStatus = response.IsSuccessStatusCode;
            }

            return IsSuccessStatus;
        }

        /// <summary>
        /// Schedule job Request
        /// </summary>
        /// <param name="authorizationServerToken"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public bool scheduleCulturaPackingInfo(AuthorizationServerAnswer authorizationServerToken, IConfigurationRoot configuration, string[] scheduleOptions)
        {
            Log.Information("read schedule cultura job api url starts");

            string ScheduleJobUrl = Convert.ToString(configuration["ScheduleCulturaPackingInfoAPI"]);

            Log.Information("read schedule cultura job api url ends- " + ScheduleJobUrl + "");

            bool IsSuccessStatus = false;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                           new AuthenticationHeaderValue("Bearer", authorizationServerToken.access_token);

                // set entity id from the background job
                string entityId = scheduleOptions.Length > 0 ? scheduleOptions.Skip(1).FirstOrDefault() : string.Empty;
                if (!string.IsNullOrWhiteSpace(entityId))
                {
                    var entityIdEncrypted = EncryptStringAES(entityId);
                    httpClient.DefaultRequestHeaders.Add("entityId", entityIdEncrypted);
                }

                HttpResponseMessage response = httpClient.GetAsync(ScheduleJobUrl).Result;
                IsSuccessStatus = response.IsSuccessStatusCode;
            }

            return IsSuccessStatus;
        }
        /// <summary>
        /// Schedule travel tariff email
        /// </summary>
        /// <param name="authorizationServerToken"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public bool scheduleTravelTariffEmail(AuthorizationServerAnswer authorizationServerToken, IConfigurationRoot configuration, string[] scheduleOptions)
        {
            Log.Information("read schedule travel tariff email api url starts");

            string requestUrl = Convert.ToString(configuration["ScheduleTravelTariffEmailAPI"]);

            Log.Information("read schedule travel tariff email api url ends- " + requestUrl + "");

            bool IsSuccessStatus = false;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                           new AuthenticationHeaderValue("Bearer", authorizationServerToken.access_token);

                // set entity id from the background job
                string entityId = scheduleOptions.Length > 0 ? scheduleOptions.Skip(1).FirstOrDefault() : string.Empty;
                if (!string.IsNullOrWhiteSpace(entityId))
                {
                    var entityIdEncrypted = EncryptStringAES(entityId);
                    httpClient.DefaultRequestHeaders.Add("entityId", entityIdEncrypted);
                }

                HttpResponseMessage response = httpClient.GetAsync(requestUrl).Result;
                IsSuccessStatus = response.IsSuccessStatusCode;
            }

            return IsSuccessStatus;
        }

        public bool scheduleAutoQcExpense(AuthorizationServerAnswer authorizationServerToken, IConfigurationRoot configuration, string[] scheduleOptions)
        {
            Log.Information("read schedule auto qc expense api url starts");

            string requestUrl = Convert.ToString(configuration["ScheduleAutoQcExpenseAPI"]);

            Log.Information("read schedule auto qc expense api url ends- " + requestUrl + "");

            bool IsSuccessStatus = false;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                           new AuthenticationHeaderValue("Bearer", authorizationServerToken.access_token);

                // set entity id from the background job
                string entityId = scheduleOptions.Length > 0 ? scheduleOptions.Skip(1).FirstOrDefault() : string.Empty;
                if (!string.IsNullOrWhiteSpace(entityId))
                {
                    var entityIdEncrypted = EncryptStringAES(entityId);
                    httpClient.DefaultRequestHeaders.Add("entityId", entityIdEncrypted);
                }

                HttpResponseMessage response = httpClient.GetAsync(requestUrl).Result;
                IsSuccessStatus = response.IsSuccessStatusCode;
            }

            return IsSuccessStatus;
        }


        public bool scheduleQcExpenseEmail(AuthorizationServerAnswer authorizationServerToken, IConfigurationRoot configuration, string[] scheduleOptions)
        {
            Log.Information("read schedule qc expense email api url starts");

            string requestUrl = Convert.ToString(configuration["ScheduleQcExpenseEmailAPI"]);


            Log.Information("read schedule qc expense email api url ends- " + requestUrl + "");

            bool IsSuccessStatus = false;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                           new AuthenticationHeaderValue("Bearer", authorizationServerToken.access_token);

                // set entity id from the background job
                string entityId = scheduleOptions.Length > 0 ? scheduleOptions.Skip(1).FirstOrDefault() : string.Empty;
                if (!string.IsNullOrWhiteSpace(entityId))
                {
                    var entityIdEncrypted = EncryptStringAES(entityId);
                    httpClient.DefaultRequestHeaders.Add("entityId", entityIdEncrypted);
                }

                HttpResponseMessage response = httpClient.GetAsync(requestUrl).Result;
                IsSuccessStatus = response.IsSuccessStatusCode;
            }

            return IsSuccessStatus;
        }

        /// <summary>
        /// schedule Carrefour Daily Result Email
        /// </summary>
        /// <param name="authorizationServerToken"></param>
        /// <param name="configuration"></param>
        /// <param name="scheduleOptions"></param>
        /// <returns></returns>
        public bool scheduleCarrefourDailyResultEmail(AuthorizationServerAnswer authorizationServerToken, IConfigurationRoot configuration, string[] scheduleOptions)
        {
            Log.Information("read schedule Carrefour Daily Result email api url starts");

            string requestUrl = Convert.ToString(configuration["ScheduleCarrfourDailyResultEmailAPI"]);

            Log.Information("read schedule Carrefour Daily Result email api url ends- " + requestUrl + "");

            bool IsSuccessStatus = false;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                           new AuthenticationHeaderValue("Bearer", authorizationServerToken.access_token);

                // set entity id from the background job
                string entityId = scheduleOptions.Length > 0 ? scheduleOptions.Skip(1).FirstOrDefault() : string.Empty;
                if (!string.IsNullOrWhiteSpace(entityId))
                {
                    var entityIdEncrypted = EncryptStringAES(entityId);
                    httpClient.DefaultRequestHeaders.Add("entityId", entityIdEncrypted);
                }

                HttpResponseMessage response = httpClient.GetAsync(requestUrl).Result;
                IsSuccessStatus = response.IsSuccessStatusCode;
            }

            return IsSuccessStatus;
        }

        public string EncryptStringAES(string plainText)
        {
            var keybytes = Encoding.UTF8.GetBytes("1234567891012345");
            var iv = Encoding.UTF8.GetBytes("1234567891012345");

            var encryoFromJavascript = EncryptStringToBytes(plainText, keybytes, iv);
            return Convert.ToBase64String(encryoFromJavascript);
        }

        private byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.  
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.  
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.  
            return encrypted;
        }


        public bool scheduleClaimReminderEmail(AuthorizationServerAnswer authorizationServerToken, IConfigurationRoot configuration, string[] scheduleOptions)
        {
            Log.Information("read schedule claim reminder email api url starts");

            string requestUrl = Convert.ToString(configuration["ScheduleFasterReportAPI"]);

            Log.Information("read schedule fast report push api url ends- " + requestUrl + "");

            bool IsSuccessStatus = false;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                           new AuthenticationHeaderValue("Bearer", authorizationServerToken.access_token);

                // set entity id from the background job
                string entityId = scheduleOptions.Length > 0 ? scheduleOptions.Skip(1).FirstOrDefault() : string.Empty;
                if (!string.IsNullOrWhiteSpace(entityId))
                {
                    var entityIdEncrypted = EncryptStringAES(entityId);
                    httpClient.DefaultRequestHeaders.Add("entityId", entityIdEncrypted);
                }

                HttpResponseMessage response = httpClient.PostAsync(requestUrl, null).Result;
                IsSuccessStatus = response.IsSuccessStatusCode;
            }

            return IsSuccessStatus;
        }

        public bool InsertBookingFilesAsZip(AuthorizationServerAnswer authorizationServerToken, IConfigurationRoot configuration, string[] scheduleOptions)
        {
            Log.Information("insert booking files as zip starts");

            string requestUrl = Convert.ToString(configuration["InsertBookingFilesAsZip"]);

            Log.Information("insert booking files as zip ends- " + requestUrl + "");

            bool IsSuccessStatus = false;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                           new AuthenticationHeaderValue("Bearer", authorizationServerToken.access_token);

                // set entity id from the background job
                string entityId = scheduleOptions.Length > 0 ? scheduleOptions.Skip(1).FirstOrDefault() : string.Empty;
                if (!string.IsNullOrWhiteSpace(entityId))
                {
                    var entityIdEncrypted = EncryptStringAES(entityId);
                    httpClient.DefaultRequestHeaders.Add("entityId", entityIdEncrypted);
                }

                HttpResponseMessage response = httpClient.PostAsync(requestUrl, null).Result;
                IsSuccessStatus = response.IsSuccessStatusCode;
            }

            return IsSuccessStatus;
        }

        public bool ScheduleMissedMSchart(AuthorizationServerAnswer authorizationServerToken, IConfigurationRoot configuration, string[] scheduleOptions)
        {
            //Log.Information("read schedule missed MSchart Result email api url starts");

            string requestUrl = Convert.ToString(configuration["ScheduleMissedMSchartEmailAPI"]);

            //Log.Information("read schedule missed MSchart Result email api url ends- " + requestUrl + "");

            bool IsSuccessStatus = false;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                           new AuthenticationHeaderValue("Bearer", authorizationServerToken.access_token);

                // set entity id from the background job
                string entityId = scheduleOptions.Length > 0 ? scheduleOptions.Skip(1).FirstOrDefault() : string.Empty;
                if (!string.IsNullOrWhiteSpace(entityId))
                {
                    var entityIdEncrypted = EncryptStringAES(entityId);
                    httpClient.DefaultRequestHeaders.Add("entityId", entityIdEncrypted);
                }

                HttpResponseMessage response = httpClient.GetAsync(requestUrl).Result;
                IsSuccessStatus = response.IsSuccessStatusCode;
            }

            return IsSuccessStatus;
        }

        public bool SchedulePlanningForCS(AuthorizationServerAnswer authorizationServerToken, IConfigurationRoot configuration, string[] scheduleOptions)
        {
            Log.Information("read schedule api url starts");

            string requestUrl = Convert.ToString(configuration["SchedulePlanningForCSAPIURL"]);
            string strOfficeId = scheduleOptions.Length > 0 ? scheduleOptions.Skip(2).FirstOrDefault() : string.Empty;
            string strConfigureId = scheduleOptions.Length > 0 ? scheduleOptions.Skip(3).FirstOrDefault() : string.Empty;

            Log.Information("read schedule api url starts for -" + strOfficeId + " ");

            requestUrl = requestUrl + "?offices=" + strOfficeId + "&configureId=" + strConfigureId;

            Log.Information("read schedule api url ends- " + requestUrl + "");
            bool IsSuccessStatus = false;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                           new AuthenticationHeaderValue("Bearer", authorizationServerToken.access_token);

                // set entity id from the background job - from third param
                string entityId = scheduleOptions.Length > 0 ? scheduleOptions.Skip(1).FirstOrDefault() : string.Empty;
                if (!string.IsNullOrWhiteSpace(entityId))
                {
                    var entityIdEncrypted = EncryptStringAES(entityId);
                    httpClient.DefaultRequestHeaders.Add("entityId", entityIdEncrypted);
                }
                HttpResponseMessage response = httpClient.GetAsync(requestUrl).Result;
                IsSuccessStatus = response.IsSuccessStatusCode;
            }
            return IsSuccessStatus;
        }

        /// <summary>
        /// Schedule Fb Report fetch
        /// </summary>
        /// <param name="authorizationServerToken"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private bool ScheduleBookingCS(AuthorizationServerAnswer authorizationServerToken, IConfigurationRoot configuration, string[] scheduleOptions)
        {
            Log.Information("read schedule api url starts");

            string scheduleBookingCsUrl = Convert.ToString(configuration["ScheduleBookingCSAPI"]);

            Log.Information("read schedule api url ends- " + scheduleBookingCsUrl + "");

            bool isSuccessStatus = false;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                           new AuthenticationHeaderValue("Bearer", authorizationServerToken.access_token);

                // set entity id from the background job
                string entityId = scheduleOptions.Length > 0 ? scheduleOptions.Skip(1).FirstOrDefault() : string.Empty;
                if (!string.IsNullOrWhiteSpace(entityId))
                {
                    var entityIdEncrypted = EncryptStringAES(entityId);
                    httpClient.DefaultRequestHeaders.Add("entityId", entityIdEncrypted);
                }

                HttpResponseMessage response = httpClient.GetAsync(scheduleBookingCsUrl).Result;
                isSuccessStatus = response.IsSuccessStatusCode;
                Log.Information("send schedule api status- " + isSuccessStatus + "");
            }

            return isSuccessStatus;
        }
        private bool BBGInitialBookingExtract(AuthorizationServerAnswer authorizationServerToken, IConfigurationRoot configuration, string[] scheduleOptions)
        {
            Log.Information("read bbg initial booking extract result email api url starts");

            string requestUrl = Convert.ToString(configuration["BBGInitialBookingExtractEmailAPI"]);

            Log.Information("read bbg initial booking extract result email api url ends- " + requestUrl);

            bool IsSuccessStatus = false;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                           new AuthenticationHeaderValue("Bearer", authorizationServerToken.access_token);

                // set entity id from the background job
                string entityId = scheduleOptions.Length > 0 ? scheduleOptions.Skip(1).FirstOrDefault() : string.Empty;
                if (!string.IsNullOrWhiteSpace(entityId))
                {
                    var entityIdEncrypted = EncryptStringAES(entityId);
                    httpClient.DefaultRequestHeaders.Add("entityId", entityIdEncrypted);
                }

                HttpResponseMessage response = httpClient.GetAsync(requestUrl).Result;
                IsSuccessStatus = response.IsSuccessStatusCode;
            }
            return IsSuccessStatus;
        }
    }
}



public interface ISchedulerDataAccess
{
    void ScheduleProcess(string[] scheduleOptions);
}

public class IdentityServerModel
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}

public class AuthorizationServerAnswer
{
    public string access_token { get; set; }
    public string expires_in { get; set; }
    public string token_type { get; set; }
}

public enum ScheduleOptions
{
    ScheduleQcEmail = 1,
    ScheduleFbReport = 2,
    ScheduleCulturaPackingInfo = 3,
    ScheduleTravelTariffEmail = 4,
    ScheduleAutoQcExpense = 5,
    ScheduleQcInspectionExpenseEmail = 6,
    ScheduleClaimReminderEmail = 7,
    ScheduleFastReport = 8,
    ScheduleCarrefourDailyResult = 9,
    InsertBookingFilesAsZip = 10,
    ScheduleMissedMSchart = 11,    
    ScheduleBookingCS = 12,
    SchedulePlanningForCS = 13,
    BBGInitialBookingExtract = 14
}




public class ClaimReminderType
{
    public int Id { get; set; }
    public string ReminderType { get; set; }
}