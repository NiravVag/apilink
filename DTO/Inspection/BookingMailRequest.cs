using DTO.CancelBooking;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.References;
using DTO.Supplier;
using DTO.UserAccount;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class BookingMailRequest
	{
		public int BookingId { get; set; }

		public string CustomerName { get; set; }

		public string CustomerMail { get; set; }

		public string SupplierName { get; set; }

        public string SupplierContactName { get; set; }

        public string SupplierPhone { get; set; }

		public string SupplierAddress { get; set; }

		public string SupplierMail { get; set; }

		public int ServiceTypeId { get; set; }

		public string ServiceType { get; set; }

		public string FactoryName { get; set; }

        public string FactoryContactName { get; set; }

        public string FactoryPhone { get; set; }
	
		public string FactoryMail { get; set; }

		public string FactoryAddress { get; set; }

		public string FactoryRegionalAddress { get; set; }

		public bool IsChinaCountry { get; set; }

		public string ApplicantName { get; set; }

		public string ApplicantEmail { get; set; }

		public int? OfficeId { get; set; }

		public int StatusId { get; set; }

		public string StatusName { get; set; }

		public string UserName { get; set; }

		public string ApiUserMail { get; set; }

		public string ApiUserContactMail { get; set; }

		public string ServiceDateFrom { get; set; }

		public string ServiceDateTo { get; set; }

		public string Brand { get; set; }

		public string Department { get; set; }

		public string Buyer { get; set; }

		public string Season { get; set; }

		public string BookingComment { get; set; }

		public string TimeType { get; set; }

		public string ReasonType { get; set; }

		public string Comment { get; set; }

        public string HoldReason { get; set; }

        public string HoldReasonType { get; set; }

        public Guid GuidId { get; set; }

		public List<string> MailToList { get; set; }

		public List<string> MailCCList { get; set; }

		public List<InspectionPOProductItem> InspectionPoList { get; set; }

		public List<InspectionPOProductItem> CancelPoList { get; set; }

		public List<BookingFileAttachment> InspectionFileAttachments { get; set; }

		public string RescheduleServiceDateFrom { get; set; }

		public string RescheduleServiceDateTo { get; set; }

		public bool IsAfter48Hours { get; set; }

        public bool IsEmailRequired { get; set; }

        public string ProductCategory { get; set; }

        public int SplitBookingId { get; set; }

        public bool quotationExists { get; set; }

        public int? QuotationId { get; set; }

        public string AEUserEmail { get; set; }

        public int TotalContainers { get; set; }

        public string FirstServiceDateFrom { get; set; }

        public string FirstServiceDateTo { get; set; }

        public string InspConfirmEngDocPath { get; set; }

        public string InspConfirmCnDocPath { get; set; }
		public string InspectionConfirmFooter { get; set; }
		public string InspectionConfirmChineseFooter { get; set; }

		public string InspectionRescheduleEnglishFooter { get; set; }
		public string InspectionRescheduleChineseFooter { get; set; }

		public string CustomerBookingNo { get; set; }
		public int? BookingType { get; set; }
		public string BookingTypeValue { get; set; }
		public string EntityName { get; set; }
		public string SenderEmail { get; set; }
		public string ShipmentDate { get; set; }
		public string ApplyDate { get; set; }
        public string SeasonYear { get; set; }
        public string CanceledBy { get; set; }
        public int BusinessLine { get; set; }
		public List<InspectionPOColorTransaction> POColorTransactionList { get; set; }
		public bool IsShowSoftLineItems { get; set; }
		public string ReInspectionTypeName { get; set; }
		public string CusBookingComments { get; set; }
		public string FactoryCountry { get; set; }
        public bool DACorrelation { get; set; }
        public string DAEmail { get; set; }
        public string DAName { get; set; }
    }

	public class InspectionPOProductItem
	{
		public string PoNo { get; set; }

        public int PoDetailId { get; set; }

        public string ProductId { get; set; }

		public string ProductDesc { get; set; }

		public int BookingQty { get; set; }

		public int? PickingQty { get; set; }

		public string Remarks { get; set; }

        public int Id { get; set; }

        public int? SampleQty { get; set; }

        public int? CombineAqlQty { get; set; }

        public int? CombineProductId { get; set; }

        public int CombineProductCount { get; set; }

        public string AqlLevel { get; set; }

        public bool IsParentProduct { get; set; }

        public string DestinationCountry { get; set; }
        public string SubCategory2 { get; set; }
		public string ColorCode { get; set; }
		public string ColorName { get; set; }
		public string Unit { get; set; }
    }

	public class InspectionFileAttachments
    {
		public string FileName { get; set; }
		public string FileUrl { get; set; }
		public bool? IsBookingEmailNotification { get; set; }
	}

	public class BookingMapEmailData
    {
		public InspectionBookingDetail BookingDetail { get; set; }
		public List<BookingServiceType> ServiceTypes { get; set; }
		public InspectionHoldReasons BookingHoldReasons { get; set; }
		public List<CommonDataSource> BrandList { get; set; }
		public List<CommonDataSource> DepartmentList { get; set; }
		public List<CommonDataSource> BuyerList { get; set; }
		public List<InspectionSupplierFactoryContacts> Factcontactlist { get; set; }
		public List<Contact> SupplierContacts { get; set; }
		public List<Contact> FactoryContacts { get; set; }

		public SupplierAddress FactoryAddress { get; set; }
		public SupplierAddress SupplierAddress { get; set; }
		public List<InspectionSupplierFactoryContacts> InspSupplierContacts { get; set; }

		public CommonDataSource CustomerSeasonData { get; set; }


	}
}
