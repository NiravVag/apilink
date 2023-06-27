using DTO.Common;
using DTO.CommonClass;
using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class BookingPoProduct   
    {
        public int Id { get; set; }

        public int PoId { get; set; }

        public string Pono { get; set; }

        public int ProductId { get; set; }

        public bool? IsNewProduct { get; set; }

        public int? ProductCategoryId { get; set; }

        public int? ProductSubCategoryId { get; set; }

        public string ProductCategoryName { get; set; }

        public string ProductSubCategoryName { get; set; }

        public int? ProductCategorySub2Id { get; set; }

        public string ProductCategorySub2Name { get; set; }

        public IEnumerable<ProductSubCategory> ProductCategorySubList { get; set; }

        public IEnumerable<CommonDataSource> ProductCategorySub2List { get; set; }

        public string ProductName { get; set; }

        public string ProductDesc { get; set; }

        public string ProductRemarks { get; set; }

        public int ProductQuantity { get; set; }

        public int ProductReminingQuantity { get; set; }

        public bool Selected { get; set; }

        public bool ProductCategoryMapped { get; set; }

        public bool ProductSubCategoryMapped { get; set; }

        public bool ProductCategorySub2Mapped { get; set; }

        public int? Aql { get; set; }

        public int? Critical { get; set; }

        public int? Major { get; set; }

        public int? Minor { get; set; }

        public int? DestinationCountryID { get; set; }

        public string FactoryReference { get; set; }

        public DateObject ETD { get; set; }

        public string Barcode { get; set; }

        public string CustomerReferencePo { get; set; }

        public IEnumerable<ProductFileAttachmentRepsonse> ProductFileAttachments { get; set; }

    }
}
