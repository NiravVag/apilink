using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.PurchaseOrder
{
    public class PurchaseOrderUpload
    {
        public int? Id { get; set; }
        public int? PoId { get; set; }
        public string Pono { get; set; }
        public string Product { get; set; }
        public int ProductId { get; set; }
        public string ProductBarcode { get; set; }
        public string ProductDescription { get; set; }
        public string FtyRef { get; set; }
        public string Etd { get; set; }
        public string Quantity { get; set; }
        public string DestinationCountry { get; set; }
        public int? CountryId { get; set; }
        public string Customer { get; set; }
        public int CustomerId { get; set; }
        public string Supplier { get; set; }
        public int SupplierId { get; set; }
        public string CustomerContact { get; set; }
        public string CustomerDepartment { get; set; }
        public string AEID { get; set; }
        public string OfficeIncharged { get; set; }
        public string BookingDate { get; set; }
        public bool IsProductNew { get; set; } = false;
        public bool IsSelected { get; set; } = false;
        public PurchaseOrderStatus purchaseOrderStatus { get; set; }
    }

    public enum PurchaseOrderStatus
    {
        Uploaded = 1,
        NotUploaded = 2
    }

    public class MSChartUpload
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string MpCode { get; set; }
        public string Required { get; set; }
        public string Tol1Up { get; set; }
        public string Tol1Down { get; set; }
        public string Tol2Up { get; set; }
        public string Tol2Down { get; set; }
    }
}
