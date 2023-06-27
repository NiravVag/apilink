using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Lab
{
	public class LabAddressTypeResponse
	{
		public IEnumerable<LabAddressType> AddressTypeList { get; set; }
		public LabAddressTypeResult Result { get; set; }
	}
	public enum LabAddressTypeResult
	{
		Success = 1,
		CannotGetLabAddressTypeList = 2
	}
}
