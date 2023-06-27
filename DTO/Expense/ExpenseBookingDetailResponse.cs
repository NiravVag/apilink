using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Expense
{
    public class ExpenseBookingDetailResponse
    {
        public IEnumerable<ExpenseBookingDetail> expenseBookingDetailList { get; set; }
        public ExpenseBookingDetailAccess expenseBookingDetailAccess { get; set; }
        public ExpenseBookingDetailResult Result { get; set; }
    }

    public class ExpenseBookingDetail
    {
        public int BookingId { get; set; }
        public string Customer { get; set; }
        public string Supplier { get; set; }
        public string Factory { get; set; }
        public string ServiceType { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public bool Selected { get; set; }
        public int? QCId { get; set; }
        public string QCName { get; set; }
    }

    public enum ExpenseBookingDetailResult
    {
        Success = 1, NotFound = 2
    }
}
