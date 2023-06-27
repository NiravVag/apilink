using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.PurchaseOrder
{
    public class PurchaseOrderSearchResponse    {        public int TotalCount { get; set; }        public int Index { get; set; }        public int PageSize { get; set; }        public int PageCount { get; set; }

        public IEnumerable<PurchaseOrderSearchData> Data { get; set; }        public PurchaseOrderSearchResult Result { get; set; }    }

    public class PurchaseOrderRepoData
    {
        public int Id { get; set; }

        public string Pono { get; set; }

        public int PoId { get; set; }

        public int? CustomerId { get; set; }       

        public string CustomerName { get; set; }

        public List<PurchaseOrderDetailsRepoData> orderDetails { get; set; }
    }

    public class PurchaseOrderDetailsRepoData
    {
        public int Id { get; set; }

        public int? SupplierId { get; set; }

        public int PoId { get; set; }

        public int ProductId { get; set; }

        public int? FactoryId { get; set; }

        public DateTime? ETD { get; set; }

        public int? DestinationCountryId { get; set; }

        public string DestinationCountry { get; set; }
    }
    
    public class PurchaseOrderDetailsRepo
    {
        public string Pono { get; set; }

        public int? CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerRefNo { get; set; }

        public int Id { get; set; }

        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }

        public int PoId { get; set; }

        public string ProductId { get; set; }
        public string ProductDescription { get; set; }

        public int? FactoryId { get; set; }
        public string FactoryName { get; set; }

        public DateTime? ETD { get; set; }

        public int Qty { get; set; }

        public int? DestinationCountryId { get; set; }
        public string DestinationCountry { get; set; }
    }
    public class PurchaseOrderExportDataResponse
    {
        public IEnumerable<PurchaseOrderExportDataItem> PurchaseOrderExportDataData { get; set; }
        public PurchaseOrderSearchResult Result { get; set; }
    }
    
    public class PurchaseOrderExportDataItem
    {
       
        public string CustomerName { get; set; }
        public string PO { get; set; }
        public string CustomerReferenceNo { get; set; }
        public string ProductId { get; set; }
        public string ProductDescription { get; set; }
        public string DestinationCountry { get; set; }
        public string ETD { get; set; }
        public string Supplier { get; set; }
        public string Factory { get; set; }
        public int Qty { get; set; }
    }

    public enum PurchaseOrderSearchResult    {        Success = 1,        NotFound = 2    }
}
