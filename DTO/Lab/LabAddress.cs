using Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Lab
{
	public class LabAddress
	{
		public int Id { get; set; }
		public int CountryId { get; set; }
        [JsonProperty(PropertyName = "regionId")]
        public int ProvinceId { get; set; }
		public int CityId { get; set; }
		public string ZipCode { get; set; }
        [JsonProperty(PropertyName = "way")]
        public string Address { get; set; }
        [JsonProperty(PropertyName = "localLanguage")]
        public string RegionalLanguage { get; set; }
		public int? LabId { get; set; }
		public int? AddressTypeId { get; set; }
	}
}
