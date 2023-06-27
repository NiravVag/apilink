using DTO.Common;
using DTO.Supplier;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Inspection
{
    public class SaveEaqfInsepectionRequest
    {
        public int BookingId { get; set; }

        [Required(ErrorMessage = "EDIT_COMPLAINT.MSG_USER_REQ")]
        public int UserId { get; set; }

        [RequiredGreaterThanZero(ErrorMessage = "EDIT_BOOKING.MSG_CUS_REQ")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_CUS_SERVICETYPE_REQ")]
        public int ServiceTypeId { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_SERVICE_FROMDATE_REQ")]
        public DateObject ServiceDateFrom { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_Service_TODATE_REQ")]
        [DateGreaterThanAttribute(otherPropertyName = "ServiceDateFrom", ErrorMessage = "EDIT_BOOKING.MSG_Service_TODATE_REQ")]
        public DateObject ServiceDateTo { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_BUSINESS_LINE_REQ")]
        public int BusinessLine { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_PORDUCT_CATEGORY")]
        public int? ProductCategoryId { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_PRODUCTCATEGORYSUB_REQ")]
        public int? ProductSubCategoryId { get; set; }                  

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_PRODUCT_TYPE_REQ")]
        public int ProductType { get; set; }

        [RequiredList(ErrorMessage = "EDIT_BOOKING.MSG_BOOKING_SELECT_PO")]
        public List<SaveEaqfInspectionPOProductDetails> EaqfInspectionProductList { get; set; }
        public int ReportRequest { get; set; }
        public bool IsSameDayReport { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_SUP_REQ")]
        public EaqfSupplierDetails Vendor { get; set; }

        public EaqfSupplierDetails Factory { get; set; }
        public List<Attachments> Attachments { get; set; }

        [StringLength(5000)]
        public string Instructions { get; set; }
        public bool InspectionCertificate { get; set; }
        public bool? IsGoldenSampleAvailable { get; set; }
        [StringLength(500)]
        public string GoldenSampleComments { get; set; }
        public bool? IsSampleCollection { get; set; }
        [StringLength(500)]
        public string SampleCollectionComments { get; set; }
        [Required(ErrorMessage = "EDIT_BOOKING.MSG_PRODUCTION_STATUS_REQ")]
        public int ProductionStatus { get; set; }
        public int PackingStatus { get; set; }
        public string EaqfRef { get; set; }

    }

    public class SaveEaqfInspectionPOProductDetails
    {
        [Required(ErrorMessage = "EDIT_BOOKING.MSG_PO_REQ")]
        public string PoNo { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_BOOKING_PROD_REF_REQ")]
        public string ProductReference { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.Unit_MSG_REQ")]
        public int Unit { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_BOOKING_QTY_REQ")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_BOOKING_AQL_LEVEL_REQ")]
        public int? AqlLevel { get; set; }
        [Required(ErrorMessage = "EDIT_BOOKING.MSG_BOOKING_AQL_CRITICAL_REQ")]
        public int? AQLCritical { get; set; }
        [Required(ErrorMessage = "EDIT_BOOKING.MSG_BOOKING_AQL_MAJOR_REQ")]
        public int? AQLMajor { get; set; }
        [Required(ErrorMessage = "EDIT_BOOKING.MSG_BOOKING_AQL_MINOR_REQ")]
        public int? AQLMinor { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }

        [Required(ErrorMessage = "EDIT_BOOKING.MSG_BOOKING_PO_COUNTRY_REQ")]
        [StringLength(2)]
        public string DestinationCountry { get; set; }

    }

    public class Attachments
    {
        public string Url { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
    }


}



