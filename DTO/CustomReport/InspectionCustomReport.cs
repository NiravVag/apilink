using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.CustomReport
{
    public class InspectionCustomReport
    {
        public int InspectionNo { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public ICollection<InspTranCuContact> CustomerContacts { get; set; }
        public ICollection<InspTranCuBrand> Brand { get; set; }

        public ICollection<InspTranCuDepartment> Department { get; set; }
        public ICollection<InspTranCuBuyer> Buyer { get; set; }
        public string Collection { get; set; }
        public string ServiceType { get; set; }
        public string FactoryAddress { get; set; }
        public string PONumber { get; set; }
        public string ProductRef { get; set; }
        public string ProductDesc { get; set; }
        public DateTime Inspection_Date { get; set; }
        public string Office { get; set; }
        public int? OfficeId { get; set; }
        public string Color { get; set; }
        public string ColorCode { get; set; }
        public RefProductCategory ProductCategory { get; set; }
        public RefProductCategorySub ProductSubCategory { get; set; }
        public string Season { get; set; }
        public string ETD { get; set; }
        public string CustomerProductCategory { get; set; }
    }

    public class InspectionCustomReportItem
    {
        public int InspectionNo { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContacts { get; set; }
        public string Brand { get; set; }
        public string Department { get; set; }
        public string Buyer { get; set; }

        public string Collection { get; set; }
        public string ServiceType { get; set; }
        public string FactoryAddress { get; set; }
        public string PONumber { get; set; }
        public string ProductRef { get; set; }
        public string ProductDesc { get; set; }
        public string Inspection_Date { get; set; }
        public string Office { get; set; }
        public int? OfficeId { get; set; }
        public string Color { get; set; }
        public string ColorCode { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public string Season { get; set; }
        public string ETD { get; set; }
        public string CustomerProductCategory { get; set; }


        public DateTime Inspection_From_Date { get; set; }
        public DateTime Inspection_To_Date { get; set; }

        public int? FBReportMapId { get; set; }
        public string ReportTitle { get; set; }
        public string MissionTitle { get; set; }
        public string OverAllResult { get; set; }
        public string FactoryName { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public string FactoryCountry { get; set; }
        public string SupplierName { get; set; }
        
        public string CsIds { get; set; }
        public string CsNames { get; set; }
        public string QCIds { get; set; }
        public string QCNames { get; set; }

        public int? ProductCategoryId { get; set; }
        public int? ServiceTypeId { get; set; }
        public int? BrandId { get; set; }
        public int? DepartId { get; set; }
        public int? ReInspectionTypeId { get; set; }
    }
    public class InspectionCustomReportStaff
    {
        public int? StaffId { get; set; }
        public string PersonName { get; set; }
    }

    public class InspPurchaseOrderColorTrans
    {
        public int ColorTransactionId { get; set; }

        public int? PoTransactionId { get; set; }

        public int? ProductRefId { get; set; }
        public int? ProductId { get; set; }
        public string ColorCode { get; set; }

        public string ColorName { get; set; }

        public int? BookingQuantity { get; set; }

        public int? PickingQuantity { get; set; }
        public int BookingId { get; set; }

    }
}
