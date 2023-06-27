using DTO.Lab;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Lab
{
    public class LabDetails
    {
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string LabName { get; set; }
        [JsonProperty(PropertyName = "comment")]
        public string Comments { get; set; }
        public bool? Active { get; set; }
        public int? TypeId { get; set; }
        public string LegalName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        [JsonProperty(PropertyName = "webSite")]
        public string Website { get; set; }
        public string Mobile { get; set; }
        [JsonProperty(PropertyName = "locLanguageName")]
        public string RegionalName { get; set; }
        [JsonProperty(PropertyName = "contactPersonName")]
        public string ContactPerson { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public string GlCode { get; set; }
        public int? EntityId { get; set; }
        public IEnumerable<LabAddress> AddressList { get; set; }
        [JsonProperty(PropertyName = "customerContactList")]
        public IEnumerable<LabContact> ContactsList { get; set; }
        public IEnumerable<LabCustomer> CustomerList { get; set; }
    }

    public class SaveLabAddressData
    {
        public int CountryId { get; set; }
        [JsonProperty("regionId")]
        public int ProvinceId { get; set; }
        public int CityId { get; set; }
        public string ZipCode { get; set; }
        [JsonProperty("way")]
        public string Address { get; set; }
        public string LocalLanguage { get; set; }
        public string AddressTypeId { get; set; }
    }

    public class SaveLabAddressRequestData
    {
        public int labId { get; set; }
        public List<SaveLabAddressData> labAddressList { get; set; }
    }

    public enum LabAddressTypeEnum
    {
        HeadOffice=1,
        RegionOffice=2
    }

    public class SaveLabAddressResponse
    {
        public SaveLabAddressResult Result { get; set; }
    }
    public enum SaveLabAddressResult
    {
        Success = 1,
        LabAddressIsNotSaved = 2,
        LabAddressIsNotFound = 3,
    }
}

