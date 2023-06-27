using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Lab
{
	public class LabType
	{
		public int Id { get; set; }
		public string Type { get; set; }
		public int? TypeTransId { get; set; }
		public int? EntityId { get; set; }
	}
}
