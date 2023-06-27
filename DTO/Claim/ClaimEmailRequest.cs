using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Claim
{
    public class ClaimEmailRequest
    {
        public int Id { get; set; }
        public string ClaimNo { get; set; }
        public string ClaimURL { get; set; }
        public string UserName { get; set; }
        public DateTime ClaimDate { get; set; }
        public int InspectionNo { get; set; }
        public string CustomerBookingNo{ get; set; }
        public string InspectionURL { get; set; }

        public string InspectionDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        public string CustomerMail { get; set; }

        public string SupplierName { get; set; }

        public string SupplierContactName { get; set; }

        public string SupplierPhone { get; set; }

        public string SupplierAddress { get; set; }

        public string SupplierMail { get; set; }

        public int ServiceTypeId { get; set; }

        public string ServiceType { get; set; }

        public string FactoryName { get; set; }

        public string FactoryContactName { get; set; }

        public string FactoryPhone { get; set; }

        public string FactoryMail { get; set; }

        public string FactoryAddress { get; set; }

        public string FactoryRegionalAddress { get; set; }
        public bool IsChinaCountry { get; set; }
        public int? OfficeId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string Priority { get; set; }
        public string FinalDecision { get; set; }
        public double? FinalAmount { get; set; }
        public string FinalCurrencyName  { get; set; }
        public string ClaimRefundRemarks { get; set; }
        public List<string> FinalDecisionName { get; set; }
        public string ProductRef { get; set; }
        public string ProductName { get; set; }
    }
}
