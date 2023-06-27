using DTO.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.OtherManday
{
    public class SaveOtherMandayRequest
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        [Required]
        public int OfficeCountryId { get; set; }
        [Required]
        public int QcId { get; set; }
        [Required]
        public int OperationalCountryId { get; set; }
        [Required]
        public int PurposeId { get; set; }
        [Required]
        public DateObject ServiceDate { get; set; }
        [Required]
        public double Manday { get; set; }
        public string Remarks { get; set; }
    }
    public enum OtherMandayResult
    {
        Success = 1,
        Failure = 2,
        NotFound = 3,
        AlreadyExists = 4,
        RequestNotCorrectFormat = 5
    }
    public class SaveOtherMandayResponse
    {
        public int Id { get; set; }
        public OtherMandayResult Result { get; set; }
    }

    public class OtherMandaySummaryResponse
    {
        public List<OtherMandayData> Data { get; set; }
        public OtherMandayResult Result { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }
    public class OtherMandayData
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int OfficeCountryId { get; set; }
        public string OfficeCountryName { get; set; }
        public int QcId { get; set; }
        public string QcName { get; set; }
        public int OperationalCountryId { get; set; }
        public string OperationalCountryName { get; set; }
        public int PurposeId { get; set; }
        public string Purpose { get; set; }
        public string ServiceDate { get; set; }
        public double Manday { get; set; }
        public string Remarks { get; set; }

        public DateObject ServiceDateObject { get; set; }
    }
    public class OtherMandayDataRepo
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int? OfficeCountryId { get; set; }
        public string OfficeCountryName { get; set; }
        public int? QcId { get; set; }
        public string QcName { get; set; }
        public int? OperationalCountryId { get; set; }
        public string OperationalCountryName { get; set; }
        public int? PurposeId { get; set; }
        public string Purpose { get; set; }
        public DateTime? ServiceDate { get; set; }
        public double? Manday { get; set; }
        public string Remarks { get; set; }
        public string OfficeName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
    public class DeleteOtherMandayResponse
    {
        public int Id { get; set; }
        public OtherMandayResult Result { get; set; }
    }

    public class EditOtherMandayResponse
    {
        public OtherMandayData Data { get; set; }
        public OtherMandayResult Result { get; set; }
    }

    public class OtherMandaySummaryRequest
    {
        public int? CustomerId { get; set; }
        public DateObject ServiceFromDate { get; set; }
        public DateObject ServiceToDate { get; set; }
        public int? OperationalCountryId { get; set; }
        public int? OfficeCountryId { get; set; }
        public int? QcId { get; set; }
        public int? Index { get; set; }
        public int? PageSize { get; set; }
    }

    public class ExportOtherMandayData
    {
        [Description("Customer")]
        public string CustomerName { get; set; }
        [Description("Office Country")]
        public string OfficeCountryName { get; set; }
        [Description("QC")]
        public string QcName { get; set; }
        [Description("Operation Country")]
        public string OperationalCountryName { get; set; }
        [Description("Purpose")]
        public string Purpose { get; set; }
        [Description("Service Date")]
        public DateTime? ServiceDate { get; set; }
        [Description("Manday")]
        public double Manday { get; set; }
        [Description("Qc Office Name")]

        public string OfficeName { get; set; }
        [Description("Created By")]
        public string CreatedBy { get; set; }

        [Description("CreatedOn")]
        public DateTime? CreatedOn { get; set; }
        [Description("Remarks")]
        public string Remarks { get; set; }
    }
}
