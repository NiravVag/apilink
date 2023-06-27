using DTO.Customer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Supplier
{
    public class SupplierData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RegionalName { get; set; }
        public string Address { get; set; }
        public string RegionalAddress { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ContactEmail { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string Remarks { get; set; }
        public string Entity { get; set; }
        public IEnumerable<int> SupplierEntityIds { get; set; }
        public virtual ICollection<SuEntity> SuEntities { get; set; }
        public string Type { get; set; }
        public int? TypeId { get; set; }
        public bool IsView { get; set; }
        public bool IsContactEmailMatched { get; set; }
        public bool IsSupplierEmailMatched { get; set; }
        public bool IsContactPhoneMatched { get; set; }
        public bool IsSupplierPhoneMatched { get; set; }
        public SupplierDataResult Result { get; set; }
        public IEnumerable<CustomerItem> CustomerList { get; set; }
    }


    public class SupplierDetail
    {
        public string Name { get; set; }
        public string RegionalName { get; set; }
        public string Address { get; set; }
        public string RegionalAddress { get; set; }
        public string Entity { get; set; }
        public string Type { get; set; }

    }

    public class SupplierContactDetail
    {
        public string ContactEmail { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
    }

    public class SupplierDataResponse
    {
        public List<SupplierData> Data { get; set; }
        public SupplierDataResult Result { get; set; }
    }

    public enum SupplierDataResult
    {
        Success = 1,
        NotFound = 2
    }
}
