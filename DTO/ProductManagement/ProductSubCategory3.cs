using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ProductManagement
{
    public class SaveProductCategorySub3
    {
        public int Id { get; set; }
        public int ProductSubCategory2Id { get; set; }
        public string Name { get; set; }
        public bool WorkLoadMatrixChecked { get; set; }
        public double? PreparationTime { get; set; }
        public int? EightHourSampleSize { get; set; }
    }
    public class SaveProductCategorySub3Response
    {
        public int Id { get; set; }
        public SaveProductManagementResult Result { get; set; }
    }

    public class ProdCatSub3Data
    {
        public int Id { get; set; }
        public int ProdCategoryId { get; set; }
        public string ProdCategoryName { get; set; }
        public int ProdSubCategoryId { get; set; }
        public string ProdSubCategoryName { get; set; }
        public int ProdCategorySub2Id { get; set; }
        public string ProdCategorySub2Name { get; set; }
        public string ProdCategorySub3 { get; set; }
        public double? PreparationTime { get; set; }
        public int? EightHourSampleSize { get; set; }
        public bool WorkLoadMatrixChecked { get; set; }
    }

    public class ProdCatSub3SummaryResponse
    {
        public List<ProdCatSub3Data> Data { get; set; }
        public bool HasITRole { get; set; }
        public ProductManagementResult Result { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }

    public class DeleteProdCategorySub3Response
    {
        public int Id { get; set; }
        public DeleteProductManagementResult Result { get; set; }
    }

    public class EditProdCategorySub3Response
    {
        public ProdCatSub3Data Data { get; set; }
        public EditProductManagementResult Result { get; set; }
    }

    public class ProdCatSub3SummaryRequest
    {
        public int? ProductCategoryId { get; set; }
        public int? ProductSubCategoryId { get; set; }
        public List<int?> ProductCategorySub2Values { get; set; }
        public string ProductCategorySub3Name { get; set; }
        public int? Index { get; set; }
        public int? PageSize { get; set; }
    }
    public class ExportProdCatSub3Data
    {
        [Description("Product Category")]
        public string ProdCategoryName { get; set; }
        [Description("Product Sub Category (Sub Category 1)")]
        public string ProdSubCategoryName { get; set; }
        [Description("Product Name (Sub Category 2)")]
        public string ProdCategorySub2Name { get; set; }
        [Description("Product Details (Sub Category 3)")]
        public string ProdCategorySub3 { get; set; }
        [Description("Preparation Time")]
        public double? PreparationTime { get; set; }
        [Description("Sample Size 8h")]
        public int? EightHourSampleSize { get; set; }
    }
}
