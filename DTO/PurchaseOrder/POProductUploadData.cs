using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.PurchaseOrder
{
    public class POProductUploadData
    {
        public string PoNumber { get; set; }
        public string ProductReference { get; set; }
        public string ProductDescription { get; set; }
        public string Etd { get; set; }
        public string DestinationCountry { get; set; }
        public string Quantity { get; set; }
        public string Barcode { get; set; }
        public string FactoryReference { get; set; }
        public string ColorCode { get; set; }
        public string ColorName { get; set; }
        public ProductUploadErrorData ErrorData { get; set; }
    }

    public enum ProductUploadErrorData
    {
        PoNoMandatory=1,
        ProductRefMandatory = 2,
        QuantityMandatory=3,
        EtdNotValidDateFormat=4,
        CountryNotAvailable=5,
        PoProductAlreadyExists=6,
        InvalidQuantityData=7,
        ProductRefDescMandatory=8,
        QuantityShouldBeZero=9,
        PoProductDuplicate=10,
        NonErrorData=11,
        EmptyRows=12,
        BarcodecharacterLimitExceeded=13,
        ColorCodeMandatory=14,
        ColorNameMandatory = 15
    }

    public class SaveProductData
    {
        public int Id { get; set; }
        public string ProductReference { get; set; }
        public string ProductDescription { get; set; }
    }

    public class SavePurchaseOrderUploadData
    {
        public int CustomerId { get; set; }
        public string PoNumber { get; set; }

    }

    public class SavePurchaseOrderDetailUploadData
    {
        public int? ProductId { get; set; }
        public int? CountryId { get; set; }
        public string Etd { get; set; }
        public string DestinationCountry { get; set; }
        public int Quantity { get; set; }
    }

    public class ExistingPoProductList
    {
        public int PoId { get; set; }
        public string PoName { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public string ColorCode { get; set; }
        public string ColorName { get; set; }

    }

    public class PoProductUploadRequest
    {
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public int BusinessLineId { get; set; }
        public List<BookingPoProductDataRequest> poProductRequest { get; set; }
    }

    public class BookingPoProductDataRequest
    {
        [JsonProperty("poId")]
        public int PoId { get; set; }
        [JsonProperty("productId")]
        public int ProductId { get; set; }
    }

    public class POMappedSupplier
    {
        public int? PoId { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
    }

    public class POMappedFactory
    {
        public int? PoId { get; set; }
        public int? FactoryId { get; set; }
        public string FactoryName { get; set; }
    }



    public class FileProvider
    {
        public string TestString { get; set; }
        public IList<IFormFile> Files { get; set; }
        public List<BookingPoProductDataRequest> PoProductRequest { get; set; }
    }

}
