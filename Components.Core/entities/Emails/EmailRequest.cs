using DTO.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace Components.Core.entities.Emails
{
    public class EmailRequest
    {
        public Guid Id { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public IEnumerable<string> Recepients { get; set; }

        public IEnumerable<string> CCList { get; set; }

        public Action<Guid, EMailState, string> ReturnToUpdate { get; set; }

        public IEnumerable<FileResponse> FileList { get; set; }

        public int EntityId { get; set; }

    }

    public class EmailDataRequest
    {
        public Guid Id { get; set; }

        public int EmailQueueId { get; set; }

        public int TryCount { get; set; }
    }

    public class EmailInfoRequest
    {
        public Guid Id { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public int EmailQueueId { get; set; }

        public int TryCount { get; set; }

        public IEnumerable<string> Recepients { get; set; }

        public IEnumerable<string> CCList { get; set; }

        public IEnumerable<FileResponse> FileList { get; set; }

    }

    public enum EMailState
    {
        Ok = 1,
        KO = 2,
        Pending = 3,
        Canceled = 4
    }
    public class FbReportFetchRequest
    {
        public Guid Id { get; set; }

        public int ReportId { get; set; }

        public int FbReportId { get; set; }
    }
}
