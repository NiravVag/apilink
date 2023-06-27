using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Inspection
{
    public class SaveMasterContactRequest
    {
        public List<MasterContact> contactList { get; set; }
        public int MasterContactTypeId { get; set; }
        public int CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? FactoryId { get; set; }
    }
    public class MasterContact
    {
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string PhoneNo { get; set; }
    }

    public class SaveMasterContactResponse
    {
        public List<string> DuplicateEmailIds { get; set; }
        public SaveMasterContactResult Result { get; set; }
    }

    public enum SaveMasterContactResult
    {
        Success = 1,
        Failed = 2,
        DuplicateEmailFound=3
    }

    public enum MasterContactType
    {
        Customer = 1,
        Supplier = 2,
        Factory = 3
    }

    public enum CustomerContactType
    {
        Operation=1
    }
}
