using DTO.CommonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Claim
{
    public class EditClaimResponse
    {
        public ClaimDetails Claim { get; set; }
        public BookingClaimData BookingClaimData { get; set; }
        public CommonDataSource BookingIdList { get; set; }
        public IEnumerable<CommonDataSource> ReportList { get; set; }
        public IEnumerable<CommonDataSource> DefectFamilyList { get; set; }
        public IEnumerable<CommonDataSource> ClaimDepartmentList { get; set; }
        public IEnumerable<CommonDataSource> ClaimCustomerRequestList { get; set; }
        public IEnumerable<CommonDataSource> CustomerRequestRefundList { get; set; }
        public InvoiceDetail InvoiceDetail { get; set; }
        public EditClaimResult Result { get; set; }
    }

    public enum EditClaimResult
    {
        Success = 1,
        CannotGetClaim = 2,
        CannotGetEntities = 3
    }
}
