using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;
using DTO.Location;

namespace DTO.UserAccount
{
    public class UserAccountSearchRequest
    {
        public IEnumerable<Country> CountryValues { get; set; }

        public int UserTypeId { get; set; }

        public int? Id { get; set; }

        public int? Index { get; set; }

        public int? pageSize { get; set; }

        public string Name { get; set; }

    }
}
