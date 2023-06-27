using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Lab
{
	public class LabAddressType
	{
		public int Id { get; set; }
		public string AddressType { get; set; }
		public int? TranslationId { get; set; }
		public int? EntityId { get; set; }
	}
}
