using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Lab
{
	public class EditLabResponse
	{
		public LabDetails LabDetails { get; set; }
		public EditLabResult Result { get; set; }
	}

	public enum EditLabResult
	{
		Success = 1,
		CannotGetSelectLab = 2
		
	}
}
