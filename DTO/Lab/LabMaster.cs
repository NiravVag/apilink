using DTO.CommonClass;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Lab
{

    public class LabMaster
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public LabTypeEnum Type { get; set; }
    }

    public class LabDataList
    {
        public IList<LabMaster> LabList { get; set; }
        public LabDataResult Result { get; set; }
    }
    public enum LabDataResult
    {
        Success = 1,
        CannotGetTypeList = 2
    }

    public class LabAddressDataList
    {
        public IEnumerable<LabMaster> LabAddressList { get; set; }
        public LabDataAddressResult Result { get; set; }
    }

    public enum LabDataAddressResult
    {
        Success = 1,
        CannotGetTypeList = 2
    }

    public class LabAddressData
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string RegionalLanguage { get; set; }
        public int CountryId { get; set; }
        public int? LabId { get; set; }
    }

    public class LabAddressRequest
    {
        public List<int?> labIdList { get; set; }
    }

    public class LabAddressListResponse
    {
        public List<LabBaseAddress> AddressList { get; set; }
        public LabAddressResult Result { get; set; }
    }

    public class LabBaseAddress
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? LabId { get; set; }
    }

    public enum LabAddressResult
    {
        Success = 1,
        CannotGetTypeList = 2
    }

    public class LabContactsDataList
    {
        public IEnumerable<LabMaster> LabContactList { get; set; }
        public LabDataContactsResult Result { get; set; }
    }
    public enum LabDataContactsResult
    {
        Success = 1,
        CannotGetTypeList = 2
    }

    public class LabContactRequest
    {
        public List<int?> LabIdList { get; set; }
        public int? CustomerId { get; set; }
    }

    public class LabContactsListResponse
    {
        public List<LabBaseContact> LabContactList { get; set; }
        public LabDataContactsListResult Result { get; set; }
    }

    public class LabBaseContact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? LabId { get; set; }
    }

    public enum LabDataContactsListResult
    {
        Success = 1,
        CannotGetTypeList = 2
    }
}
