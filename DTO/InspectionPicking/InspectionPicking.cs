using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class InspectionPickingData
    {
        public int Id { get; set; }

        public int? LabId { get; set; }

        public int? CustomerId { get; set; }

        public int? LabAddressId { get; set; }

        public int? CusAddressId { get; set; }

        public int PoId { get; set; }

        public string PoName { get; set; }

        public int PoTranId { get; set; }

        public int PickingQuantity { get; set; }

        public string Remarks { get; set; }

        public int? LabType { get; set; }

        public int ProductID { get; set; }

        public bool Active { get; set; }

        public IEnumerable<InspectionPickingContact> InspectionPickingContacts { get; set; }
    }

    public class QcPickingData
    {
        public int BookingId { get; set; }
        public string CustomerName { get; set; }
        public string LabName { get; set; }
        public string FactoryName { get; set; }
        public string CustomerBookingNo { get; set; }
        public string SupplierName { get; set; }
        public string LabAddress { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string CsName { get; set; }
        public string CsPhone { get; set; }
        public string Comment { get; set; }
        public string SupplierEmail { get; set; }
        public int PoTransId { get; set; }
        public string ServiceDate { get; set; }
        public int PickingQuantity { get; set; }
        public string RegionalAddress { get; set; }
        public bool Lab { get; set; }
        public bool Customer { get; set; }
        public int AddressId { get; set; }
        public string StaffName { get; set; }
        public IEnumerable<PickingProductData> Products { get; set; }
    }

    public class PickingLabAddressItem
    {        
        public string LabAddress { get; set; }
        public string LabName { get; set; }
        public List<string> ContactName { get; set; }
        public List<string> Email { get; set; }
        public List<string> Telephone { get; set; }
        public string CsName { get; set; }
        public string CsPhone { get; set; }
        public string Comment { get; set; }
        public int PoTransId { get; set; }
        public int PickingQuantity { get; set; }
        public string RegionalAddress { get; set; }
        public bool Lab { get; set; }
        public int AddressId { get; set; }
    }

    public class PickingProductData
    {
        public string ProductId { get; set; }
        public string FactoryReference { get; set; }
        public string PONumber { get; set; }
        public string DestinationCountry { get; set; }
        public int PickingQuantity { get; set; }
        public bool isLab { get; set; }
        public int AddressId { get; set; }
    }

    public class QcPickingItem
    {
        public int BookingId { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerBookingNo { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string SupplierEmail { get; set; }
        public int PoTransId { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public int? OfficeId { get; set; }
    }

    public class EntMasterConfigItem
    {
        public string Entity { get; set; }
        public string ImageLogo { get; set; }
    }
}
