using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Lab
{
	public class LabSearchResponse
	{
		public IEnumerable<LabItem> Data { get; set; }

		public int TotalCount { get; set; }

		public int Index { get; set; }

		public int PageSize { get; set; }

		public int PageCount { get; set; }

		public LabSearchResult Result { get; set; }
	}
	public enum LabSearchResult
	{
		Success = 1,
		NotFound = 2
	}

	public class LabItem
	{
		public int Id { get; set; }

		public string LabName { get; set; }

		public string CountryName { get; set; }

		public string ProvinceName { get; set; }

		public string CityName { get; set; }

		public string TypeName { get; set; }

	}
}
