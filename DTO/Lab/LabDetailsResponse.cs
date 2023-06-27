using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Lab
{
	public class LabDetailsResponse
	{
		public IEnumerable<LabItemSelect> LabList { get; set; }
		public LabDetailsResult Result { get; set; }
	}
	public enum LabDetailsResult
	{
		Success = 1,
		CannotGetLabDetailsList = 2
	}

    public class LabItemSelect
    {
        public int Id { get; set; }
        public string LabName { get; set; }
    }
}
