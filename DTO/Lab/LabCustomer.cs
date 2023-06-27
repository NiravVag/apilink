using Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Lab
{
	public class LabCustomer
	{
		public int LabId { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int CustomerId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string CustomerName { get; set; }
        public string Code { get; set; }
	}
}
