using DTO.Email;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class SetInspNotifyResponse
	{
        //public Guid TaskId { get; set; }

        public Guid NotificationId { get; set; }

        public int BookingId { get; set; }

        public int? StatusId { get; set; }

        public string StatusName { get; set; }

        public bool? IsEdit { get; set; }

        public int UserId { get; set; }

        public int UserTypeId { get; set; }

        public int ManagerUserId { get; set; }

        public IEnumerable<User.User> UserList { get; set; }

        public IEnumerable<User.User> ToRecipients { get; set; }

        public BookingItem Data { get; set; }

        public SetInspStatusResult Result { get; set; }

        public bool quotationExists { get; set; }

        public string CustomerEmail { get; set; }

        public int? QuotationId { get; set; }

        public int? OfficeId { get; set; }
    }

    public enum SetInspStatusResult
	{
        Success = 1,
        CannotUpdateStatus = 2, 
        NoAccess = 3, 
        InspNotFound = 4
    }
}
