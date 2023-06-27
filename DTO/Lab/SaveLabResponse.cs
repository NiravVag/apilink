using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Lab
{
	public class SaveLabResponse
	{
		public SaveLabResult Result { get; set; }
	}
	public enum SaveLabResult
	{
		Success = 1,
		LabIsNotSaved = 2,
		LabIsNotFound = 3,
		LabExists = 4
	}
}
