using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.WorkLoadMatrix
{
    public class SaveWorkLoadMatrixRequest
    {
        public int Id { get; set; }
        [Required]
        public int ProductSubCategory3Id { get; set; }
        [Required] 
        public double PreparationTime { get; set; }
        [Required] 
        public int EightHourSampleSize { get; set; }
    }
    public enum WorkLoadMatrixResult
    {
        Success = 1,
        Failure = 2,
        NotFound = 3,
        AlreadyExists = 4,
        RequestNotCorrectFormat = 5
    }
    public class SaveWorkLoadMatrixResponse
    {
        public int Id { get; set; }
        public WorkLoadMatrixResult Result { get; set; }
    }

    public class WorkLoadMatrixData
    {
        public int Id { get; set; }
        public int? ProdCategoryId { get; set; }
        public string ProdCategoryName { get; set; }
        public int? ProdSubCategoryId { get; set; }
        public string ProdSubCategoryName { get; set; }
        public int? ProdCategorySub2Id { get; set; }
        public string ProdCategorySub2Name { get; set; }
        public int? ProdCategorySub3Id { get; set; }
        public string ProdCategorySub3Name { get; set; }
        public double? PreparationTime { get; set; }
        public int? EightHourSampleSize { get; set; }
    }

    public class ExportWorkLoadMatrixData
    {
        [Description("Product Category")]
        public string ProdCategoryName { get; set; }
        [Description("Product Sub Category (Sub Category 1)")]
        public string ProdSubCategoryName { get; set; }
        [Description("Product Name (Sub Category 2)")]
        public string ProdCategorySub2Name { get; set; }
        [Description("Product Details (Sub Category 3)")]
        public string ProdCategorySub3Name { get; set; }
        [System.ComponentModel.Description("Preparation Time")]
        public double? PreparationTime { get; set; }
        [Description("Sample Size 8h")]
        public int? EightHourSampleSize { get; set; }
    }

    public class WorkLoadMatrixSummaryResponse
    {
        public List<WorkLoadMatrixData> Data { get; set; }
        public bool HasITRole { get; set; }
        public WorkLoadMatrixResult Result { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }

    public class DeleteWorkLoadMatrixResponse
    {
        public int Id { get; set; }
        public WorkLoadMatrixResult Result { get; set; }
    }

    public class EditWorkLoadMatrixResponse
    {
        public WorkLoadMatrixData Data { get; set; }
        public WorkLoadMatrixResult Result { get; set; }
    }

    public class WorkLoadMatrixSummaryRequest
    {
        public int? ProductCategoryId { get; set; }
        public int? ProductSubCategoryId { get; set; }
        public List<int?> ProductCategorySub2IdList { get; set; }
        public List<int?> ProductCategorySub3IdList { get; set; }
        public bool WorkLoadMatrixNotConfigured { get; set; }
        public int? Index { get; set; }
        public int? PageSize { get; set; }
    }
}
