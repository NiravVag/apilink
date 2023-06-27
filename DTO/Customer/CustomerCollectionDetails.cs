using DTO.CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerCollectionDetails
    {
        public int Id { get; set; }
        public List<CustomerCollection> CustomerCollectionList { get; set; }
        public List<int> RemoveIds { get; set; }
        public CustomerCollectionListResult Result { get; set; }
        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }
    }

    public class CustomerCollection
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CustomerCollectionListResponse
    {
        public IEnumerable<CommonDataSource> CustomerBrandsList { get; set; }
        public CustomerCollectionListResult Result { get; set; }
    }

    public class CustomerCollectionListSummary
    {
        public int? Index { get; set; }
        public int? PageSize { get; set; }
        public int Id { get; set; }
    }

    public enum CustomerCollectionListResult
    {
        Success = 1,
        CannotGetCollection = 2,
        RequestNotCorrectFormat = 3,
        NotFound = 4
    }
}
