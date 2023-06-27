using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.InspectionCertificate
{
    public class InspectionCertificateData
    {
        public int IcId { get; set; }
        public string IcNo { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<InspIcTranProduct> InspIcTranProducts { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
    }
}
