using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.TCF
{
    public class TCFCustomer
    {
        public string customerName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string mobileNumber { get; set; }
        public string glCode { get; set; }
    }

    public class TCFCustomerContact
    {
        public int apiCustomerContactId { get; set; }
        public string contactName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string mobileNumber { get; set; }
        public string glCode { get; set; }
    }

    public class TCFSupplier
    {
        public int apiSupplierId { get; set; }
        public string supplierName { get; set; }
        public List<string> glCode { get; set; }
    }

    public class TCFSupplierCustomer
    {
        public int SupplierId { get; set; }
        public string GlCode { get; set; }
    }

    public class TCFUserRepo
    {
        public int apiLinkUserId { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public int userTypeId { get; set; }
        public string userType { get; set; }
        public int? customerId { get; set; }
        public int? supplierId { get; set; }
        public string glCode { get; set; }
    }

    public class TCFUser
    {
        public int apiLinkUserId { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string userType { get; set; }
        public int apiUserTypeId { get; set; }
        public string glCode { get; set; }
    }

    public class TCFSettings
    {
        public string BaseUrl { get; set; }
        public string MasterToken { get; set; }
        public string CustomerContactRequestUrl { get; set; }
        public string CustomerContactUpdateUrl { get; set; }
        public string CustomerContactByIdtUrl { get; set; }
        public string SupplierRequestUrl { get; set; }
        public string SupplierUpdateUrl { get; set; }
        public string SupplierByIdtUrl { get; set; }
        public string UserRequestUrl { get; set; }
        public string UserUpdateUrl { get; set; }
        public string UserByIdUrl { get; set; }
        public string SupplierContactRequestUrl { get; set; }
        public string SupplierContactUpdateUrl { get; set; }
        public string SupplierContactUpdateRequestUrl { get; set; }
        public string SupplierContacsByIdListUrl { get; set; }
        public string ProductRequestUrl { get; set; }
        public string ProductUpdateUrl { get; set; }
        public string ProductIdByUrl { get; set; }
        public string BuyerRequestUrl { get; set; }
        public string BuyerUpdateUrl { get; set; }
        public string BuyerByIdListUrl { get; set; }
        public string UploadTCFDocumentUrl { get; set; }
        public string CustomerRequestUrl { get; set; }
        public string CustomerUpdateUrl { get; set; }
        public string CustomerByGLCode { get; set; }
        public string UploadFilesUrl { get; set; }
    }

    public class TCFMasterRequestLogInfo
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int DataType { get; set; }
        public string RequestUrl { get; set; }
        public string LogInformation { get; set; }
        public string ResponseMessage { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public enum TCFDataType
    {
        User = 1,
        Customercontact = 2,
        Supplier = 3,
        SupplierContact = 4,
        Buyer = 5,
        UserTokenGeneration = 6,
        Product = 7,
        Customer = 8
    }

    public class TCFResponseMessage
    {
        public bool IsSuccess { get; set; }
        public string ResponseMessage { get; set; }
    }

    public class TCFBuyer
    {
        public int apiBuyerId { get; set; }
        public string buyerName { get; set; }
        public string glCode { get; set; }
    }

    public class Suppliercontact
    {
        public int apiSupplierContactId { get; set; }
        public string contactName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string mobileNumber { get; set; }
        public int apiSupplierId { get; set; }
    }

    public class TCFSupplierContact
    {
        public List<Suppliercontact> supplierContacts { get; set; }
    }

    public class TCFProduct
    {
        public int apiProductId { get; set; }
        public string productRef { get; set; }
        public string productDescription { get; set; }
        public string barCode { get; set; }
        public int? productCategoryId { get; set; }
        public int? productSubCategoryId { get; set; }
    }

    public class TCFSupplierContactIdListRequest
    {
        public List<int> apiSupplierContactIds { get; set; }
    }

    public class TCFBuyerIdListRequest
    {
        public List<int> buyerIds { get; set; }
    }

    public class TCFBuyerRequest
    {
        public List<TCFBuyer> buyers { get; set; }
    }


}
