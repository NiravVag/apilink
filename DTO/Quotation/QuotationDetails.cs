using DTO.Common;
using DTO.References;
using Entities;
using Entities.Enums;
using System.Collections.Generic;

namespace DTO.Quotation
{
    public class QuotationDetails
    {
        public QuotationDetails()
        {
            this.IsToForward = true;
        }

        public int Id { get; set; }

        public Location.Country Country { get; set; }

        public DTO.References.Service Service { get; set; }

        public BillingMethod BillingMethod { get; set; }

        public BillPaidBy BillingPaidBy { get; set; }

        public DataSource Customer { get; set; }

        public string CustomerLegalName { get; set; }

        public IEnumerable<QuotationEntityContact> CustomerContactList { get; set; }

        public DataSource Supplier { get; set; }

        public string SupplierLegalName { get; set; }

        public IEnumerable<QuotationEntityContact> SupplierContactList { get; set; }

        public DataSource Factory { get; set; }

        public string LegalFactoryName { get; set; }


        public string FactoryAddress { get; set; }

        public IEnumerable<QuotationEntityContact> FactoryContactList { get; set; }

        public DataSource Office { get; set; }


        public IEnumerable<QuotationEntityContact> InternalContactList { get; set; }

        public double InspectionFees { get; set; }

        public double EstimatedManday { get; set; }

        public Currency Currency { get; set; }

        public double? TravelCostsAir { get; set; }

        public double? TravelCostsLand { get; set; }

        public double? TravelCostsHotel { get; set; }

        public double? OtherCosts { get; set; }

        public double? Discount { get; set; }

        public double TotalCost { get; set; }

        public string ApiRemark { get; set; }

        public string CustomerRemark { get; set; }

        public QuotationStatus StatusId { get; set; }

        public string StatusLabel { get; set; }

        public IEnumerable<Order> OrderList { get; set; }

        public bool IsToForward { get; set; }

        public string CreatedDate { get; set; }

        public string ETD { get; set; }

        public string InspecCreatedDate { get; set; }

        public IEnumerable<QuotationPDFVersion> QuotationPDFList { get; set; }

        public ServiceType ServiceTypeList { get; set; }

        public string ApiInternalRemark { get; set; }

        public string BookingNo { get; set; }

        public bool IsBookingInvoiced { get; set; }

        public string ServiceTypeAbbreviation { get; set; }

        public int? BillingEntity { get; set; }

        public DateObject ConfirmDate { get; set; }

        public int TotalContainers { get; set; }

        public int? PaymentTerm { get; set; }

        public bool skipclientconfirmation { get; set; }

        public string ValidatedUserName { get; set; }

        public bool SkipQuotationSentToClient { get; set; }

        public IEnumerable<EntMasterConfig> EntityMasterConfigs { get; set; }

        public int? RuleId { get; set; }
        public string EntityName { get; set; }
        public string SenderEmail { get; set; }
        public int UserId { get; set; }
        public string PoNO { get; set; }
        public string ProductRef { get; set; }
        public string FactoryCountry { get; set; }
        public string PaymentTermsValue { get; set; }
        public int? PaymentTermsCount { get; set; }
        public IEnumerable<FactoryBookingInfo> FactoryBookingInfoList { get; set; }
    }

    public class QuotationEntityContact
    {
        public int ContactId { get; set; }

        public string ContactName { get; set; }

        public string ContactEmail { get; set; }

        public string ContactPhone { get; set; }

        public bool Quotation { get; set; }

        public bool Email { get; set; }

        public QuotationEntityType EntityType { get; set; }

        public int? UserId { get; set; }

        public bool CustomerAE { get; set; }
        public bool InvoiceEmail { get; set; }
    }

    public class QuotationPDFVersion
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string FileLink { get; set; }

        public string SendDate { get; set; }

    }

    public enum QuotationEntityType
    {
        Customer = 1,
        Supplier = 2,
        Factory = 3,
        Internal = 4
    }

    public class QuotationCheckpointData
    {
        public int CheckpointId { get; set; }
        public List<int> BrandIdList { get; set; }
        public List<int> DeptList { get; set; }
        public List<int> ServiceTypeIdList { get; set; }
    }

    public class BookingQuotationData
    {
        public int BookingId { get; set; }
        public List<int> BrandIdList { get; set; }
        public List<int> DeptList { get; set; }
        public List<int> ServiceTypeIdList { get; set; }
    }

}
