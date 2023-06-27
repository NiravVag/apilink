using System.Collections.Generic;

namespace DTO.InvoiceDataAccess
{
    public class SaveInvoiceDataAccessRequest
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public IEnumerable<int> CustomerIdList { get; set; }
        public IEnumerable<int> InvoiceTypeIdList { get; set; }
        public IEnumerable<int> OfficeIdList { get; set; }
    }

    public class InvoiceDataAccessSummaryRequest
    {
        public int? Index { get; set; }
        public int? pageSize { get; set; }
        public int StaffId { get; set; }
        public IEnumerable<int> CustomerIdList { get; set; }
        public IEnumerable<int> InvoiceTypeIdList { get; set; }
        public IEnumerable<int> OfficeIdList { get; set; }
    }

    public class InvoiceDataAccessSummaryResponse
    {
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public IEnumerable<InvoiceDataAccessSummaryItem> InvoiceDataAccessSummaryList { get; set; }
        public InvoiceDataAccessSummaryResult Result { get; set; }
    }

    public enum InvoiceDataAccessSummaryResult
    {
        Success = 1,
        Failed = 2,
        RequestNotCorrectFormat = 3,
        NotFound = 4,
        Error = 5,
    }

    public class InvoiceDataAccessSummaryItem
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public IEnumerable<InvoiceCustomerDataAccess> CustomerList { get; set; }
        public IEnumerable<InvoiceTypeDataAccess> InvoiceTypeList { get; set; }
        public IEnumerable<InvoiceOfficeDataAccess> OfficeList { get; set; }
    }

    public class InvoiceDataAccessEditSummaryItem
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public IEnumerable<int> CustomerList { get; set; }
        public IEnumerable<int> InvoiceTypeList { get; set; }
        public IEnumerable<int> OfficeList { get; set; }
    }

    public class EditInvoiceDataAccessResponse
    {
        public InvoiceDataAccessEditSummaryItem InvoiceDataAccess { get; set; }
        public InvoiceDataAccessResponseResult Result { get; set; }
    }
    public enum InvoiceDataAccessResponseResult
    {
        Success = 1,
        Failed = 2,
        RequestNotCorrectFormat = 3,
        NotFound = 4,
        Error = 5,
    }

    public enum DeleteInvoiceDataAccessResponseResult
    {
        Success = 1,
        Failed = 2,
        RequestNotCorrectFormat = 3,
        NotFound = 4,
        Error = 5,
    }

    public class InvoiceCustomerDataAccess
    {
        public int DataAccessId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
    }

    public class InvoiceTypeDataAccess
    {
        public int DataAccessId { get; set; }
        public int InvoiceTypeId { get; set; }
        public string InvoiceTypeName { get; set; }
    }

    public class InvoiceOfficeDataAccess
    {
        public int DataAccessId { get; set; }
        public int InvoiceOfficeId { get; set; }
        public string InvoiceOfficeName { get; set; }
    }

    public class InvoiceDataAccess
    {
        public int StaffId { get; set; }
        public IEnumerable<int> CustomerIds { get; set; }
        public IEnumerable<int> OfficeIds { get; set; }
        public IEnumerable<int> InvoiceTypes { get; set; }
    }
}
