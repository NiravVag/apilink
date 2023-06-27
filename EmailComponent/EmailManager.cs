using Components.Core.contracts;
using Components.Core.entities.Emails;
using DTO.File;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmailComponent
{
    public class EmailManager : IEmailManager
    {
        private readonly ApismtpSettings _apiMailSettings;
        private readonly SgtsmtpSettings _sgtMailSettings;
        private readonly AqfsmtpSettings _aqfMailSettings;
        private readonly ILogger<EmailManager> _logger = null;
        private Action<Guid, EMailState, string> ReturnToUpdate { get; set; }
        private List<EmailRequest> _emailList = new List<EmailRequest>();


        public EmailManager(IOptions<ApismtpSettings> apiMailSettings, IOptions<SgtsmtpSettings> sgtMailSettings,
            IOptions<AqfsmtpSettings> aqfMailSettings,
            ILogger<EmailManager> logger)
        {
            _apiMailSettings = apiMailSettings.Value;
            _sgtMailSettings = sgtMailSettings.Value;
            _aqfMailSettings = aqfMailSettings.Value;
            _logger = logger;
        }

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {

            // Get the unique identifier for this asynchronous operation.

            Guid token = (Guid)e.UserState;

            var email = _emailList.FirstOrDefault(x => x.Id == token);

            if (email != null)
                _emailList.Remove(email);

            var newEmail = _emailList.FirstOrDefault();

            if (newEmail != null)
                SendEmail(newEmail, 0);
        }

        public void SendEmails(IEnumerable<EmailRequest> emailList)
        {

            _emailList = emailList.ToList();

            var email = _emailList.FirstOrDefault();

            if (email != null)
                SendEmail(email, 0);

        }

        /// <summary>
        /// get email settings based on entity
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public SmtpSettings GetMailSettingConfiguration(int entityId)
        {
            // default api settings
            SmtpSettings _settings = _apiMailSettings;

            if (entityId == (int)CompanyData.api)
            {
                _settings = _apiMailSettings;
            }
            else if (entityId == (int)CompanyData.sgt)
            {
                _settings = _sgtMailSettings;
            }
            else if (entityId == (int)CompanyData.aqf)
            {
                _settings = _aqfMailSettings;
            }
            return _settings;
        }

        public enum CompanyData
        {
            api = 1,
            sgt = 2,
            aqf = 3
        }



        public bool SendEmail(EmailInfoRequest request, int entityId)
        {
            bool isResult = false;
            var _settings = GetMailSettingConfiguration(entityId);
            try
            {
                if (request.Recepients == null || !request.Recepients.Any())
                    return isResult;

                var client = new SmtpClient(_settings.ServerName, _settings.PortNumber);
                client.EnableSsl = _settings.SMTPSSL;
                client.Credentials = new NetworkCredential(_settings.AccountName, _settings.Password);


                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_settings.SenderEmail);
                string EmailBody = request.Body ?? "";
                if (_settings.Dev)
                {
                    var devemail = _settings.RecepientDEV.Split(';');
                    foreach (var item in devemail)
                    {
                        if (!string.IsNullOrEmpty(item) && IsValidEmail(item))
                            mailMessage.To.Add(item);
                    }

                    EmailBody = "-- Debug Info --"
                            + "<br>Real Email From: " + _settings.SenderEmail
                            + "<br>Real Email To: " + (request.Recepients != null ? request.Recepients.Aggregate((x, y) => x + ";" + y) : "")
                            + "<br>Real Email CC: " + (request.CCList != null && request.CCList.Any() ? request.CCList.Aggregate((x, y) => x + ";" + y) : "")
                            + "<br><br>"
                            + request.Body;
                }
                else
                {
                    //To list
                    foreach (var item in request.Recepients)
                    {
                        if (!string.IsNullOrEmpty(item) && IsValidEmail(item))
                            mailMessage.To.Add(item);
                    }
                    //cc list
                    if (request.CCList != null && request.CCList.Any())
                    {

                        foreach (var item in request.CCList)
                        {
                            if (!string.IsNullOrEmpty(item) && IsValidEmail(item))
                                mailMessage.CC.Add(item);
                        }
                    }
                    //bcc list
                    var bccemail = _settings.BCCEmail.Split(';');
                    foreach (var item in bccemail)
                    {
                        if (!string.IsNullOrEmpty(item) && IsValidEmail(item))
                            mailMessage.Bcc.Add(item);
                    }
                }

                mailMessage.Body = EmailBody;
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = request.Subject.Replace('\r', ' ').Replace('\n', ' ');

                if (request.FileList != null)
                {
                    foreach (var file in request.FileList)
                    {
                        // Remove invalid character from the filename and apply -
                        file.Name = Regex.Replace(file.Name.Trim(), "[^A-Za-z0-9-_. ]+", "-");

                        if (file.FileStorageType == (int)FileStorageType.Link)
                        {
                            if (!string.IsNullOrEmpty(file.FileLink))
                            {
                                byte[] response = new System.Net.WebClient().DownloadData(file.FileLink);

                                mailMessage.Attachments.Add(new Attachment(new MemoryStream(response), file.Name, file.MimeType));
                            }
                        }
                        else
                        {
                            mailMessage.Attachments.Add(new Attachment(new MemoryStream(file.Content), file.Name, file.MimeType));
                        }
                    }

                }

                client.Send(mailMessage);
                isResult = true;
                return isResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                throw ex;
            }
        }
        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public void SendEmail(EmailRequest request, int entityId)
        {
            var _settings = GetMailSettingConfiguration(entityId);
            try
            {
                if (request.Recepients == null || !request.Recepients.Any())
                    return;


                var client = new SmtpClient(_settings.ServerName, _settings.PortNumber);
                client.EnableSsl = _settings.SMTPSSL;
                client.Credentials = new NetworkCredential(_settings.AccountName, _settings.Password);
                client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

                ReturnToUpdate = request.ReturnToUpdate;

                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_settings.SenderEmail);
                string EmailBody = request.Body ?? "";
                if (_settings.Dev)
                {
                    var devemail = _settings.RecepientDEV.Split(';');
                    foreach (var item in devemail)
                        mailMessage.To.Add(item);

                    EmailBody = "-- Debug Info --"
                            + "<br>Real Email From: " + _settings.SenderEmail
                            + "<br>Real Email To: " + (request.Recepients != null ? request.Recepients.Aggregate((x, y) => x + ";" + y) : "")
                            + "<br>Real Email CC: " + (request.CCList != null && request.CCList.Any() ? request.CCList.Aggregate((x, y) => x + ";" + y) : "")
                            + "<br><br>"
                            + request.Body;
                }
                else
                {
                    //To list
                    foreach (var item in request.Recepients)
                    {
                        if (!string.IsNullOrEmpty(item) && IsValidEmail(item))
                            mailMessage.To.Add(item);
                    }
                    
                    //bcc list
                    var bccemail = _settings.BCCEmail.Split(';');
                    foreach (var item in bccemail)
                    {
                        if (!string.IsNullOrEmpty(item) && IsValidEmail(item))
                            mailMessage.Bcc.Add(item);
                    }
                }
                //cc list
                if (request.CCList != null && request.CCList.Any())
                {

                    foreach (var item in request.CCList)
                    {
                        if (!string.IsNullOrEmpty(item) && IsValidEmail(item))
                            mailMessage.CC.Add(item);
                    }
                }

                mailMessage.Body = EmailBody;
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = request.Subject.Replace('\r', ' ').Replace('\n', ' ');

                if (request.FileList != null)
                {
                    foreach (var file in request.FileList)
                    {
                        // Remove invalid character from the filename and apply -
                        file.Name = Regex.Replace(file.Name.Trim(), "[^A-Za-z0-9-_. ]+", "-");

                        mailMessage.Attachments.Add(new Attachment(new MemoryStream(file.Content), file.Name, file.MimeType));

                    }

                }

                client.SendAsync(mailMessage, request.Id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }


        public void SendClaimEmailToManager(EmailRequest request, int entityId)
        {
            var _settings = GetMailSettingConfiguration(entityId);
            try
            {
                if (request.Recepients == null || !request.Recepients.Any())
                    return;

                var client = new SmtpClient(_settings.ServerName, _settings.PortNumber);
                client.EnableSsl = _settings.SMTPSSL;
                client.Credentials = new NetworkCredential(_settings.AccountName, _settings.Password);
                client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

                ReturnToUpdate = request.ReturnToUpdate;

                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_settings.AccountName);
                string EmailBody = request.Body ?? "";
                if (_settings.Dev)
                {
                    var devemail = _settings.RecepientDEV.Split(';');
                    foreach (var item in devemail)
                        mailMessage.To.Add(item);

                    EmailBody = "-- Debug Info --"
                            + "<br>Real Email From: " + _settings.AccountName
                            + "<br>Real Email To: " + (request.Recepients != null ? request.Recepients.Aggregate((x, y) => x + ";" + y) : "")
                            + "<br>Real Email CC: " + (request.CCList != null && request.CCList.Any() ? request.CCList.Aggregate((x, y) => x + ";" + y) : "")
                            + "<br><br>"
                            + request.Body;
                }
                else
                {
                    //To list
                    foreach (var item in request.Recepients)
                    {
                        if (!string.IsNullOrEmpty(item) && IsValidEmail(item))
                            mailMessage.To.Add(item);
                    }
                    //bcc list
                    var bccemail = _settings.BCCEmail.Split(';');
                    foreach (var item in bccemail)
                    {
                        if (!string.IsNullOrEmpty(item) && IsValidEmail(item))
                            mailMessage.Bcc.Add(item);
                    }
                }
                //cc list
                if (request.CCList != null && request.CCList.Any())
                {

                    foreach (var item in request.CCList)
                    {
                        if (!string.IsNullOrEmpty(item) && IsValidEmail(item))
                            mailMessage.CC.Add(item);
                    }

                }

                mailMessage.Body = EmailBody;
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = request.Subject.Replace('\r', ' ').Replace('\n', ' ');

                if (request.FileList != null)
                {
                    foreach (var file in request.FileList)
                        mailMessage.Attachments.Add(new Attachment(new MemoryStream(file.Content), file.Name, file.MimeType));
                }


                client.SendAsync(mailMessage, request.Id);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
    }
}
