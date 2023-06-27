using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Lab
{
	public class LabTypeResponse
	{
		public IEnumerable<LabType> TypeList { get; set; }
		public LabTypeResult Result { get; set; }
	}
	public enum LabTypeResult
	{
		Success = 1,
		CannotGetLabTypeList = 2
	}
}
