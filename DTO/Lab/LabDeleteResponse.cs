using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Lab
{
	public class LabDeleteResponse
	{
		public int Id { get; set; }

		public LabDeleteResult Result { get; set; }
	}

	public enum LabDeleteResult
	{
		Success = 1,
		NotFound = 2
	}
}
