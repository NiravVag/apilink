using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Master
{
    public class MasterDataRequest
    {
        public Guid Id { get; set; }

        public int SearchId { get; set; }

        public int EntityId { get; set; }

        public List<SearchData> SearchList { get; set; }

        public MasterDataType MasterDataType { get; set; }

        public ExternalClient ExternalClient { get; set; }
    }

    public class SearchData
    {
        public int SearchId { get; set; }

        public MasterDataAction MasterDataAction { get; set; }
    }

    public enum MasterDataAction
    {
        Create = 1,
        Update = 2
    }
}
