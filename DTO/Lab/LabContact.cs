using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Lab
{
	public class LabContact
	{
        [JsonProperty(PropertyName = "contactId")]
        public int Id { get; set; }
		public int LabId { get; set; }
		public string ContactName { get; set; }
		public string Phone { get; set; }
		public string Fax { get; set; }
        [JsonProperty(PropertyName = "contactEmail")]
        public string Mail { get; set; }
		public bool? Active { get; set; }
		public string JobTitle { get; set; }
		public string Mobile { get; set; }
		public string Comment { get; set; }
        // using for json only
        public IEnumerable<LabCustomer> CustomerList { get; set; }
    }
}
