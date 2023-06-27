using DTO.Location;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Lab
{
	public class LabSearchRequest
	{
        [JsonProperty(PropertyName = "labTypeValues")]
        public IEnumerable<int> TypeValues { get; set; }
        [JsonProperty(PropertyName = "labNameValues")]
        public IEnumerable<int> LabValues { get; set; }

        public IEnumerable<int> CountryValues { get; set; }

        public int? Index { get; set; }

		public int? pageSize { get; set; }
	}

	public class Type
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}
}
