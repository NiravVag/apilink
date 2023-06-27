using System;
using System.Collections.Generic;
using System.Text;
using DTO.Location;
namespace DTO.OfficeLocation
{
    public class Office
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OfficeCode { get; set; }
        public string Fax { get; set; }
        public string Tel { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Email { get; set; }
        public OfficeType Type { get; set; }
        public int LocationTypeId { get; set; }
        public int? HeadOffice { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public int? Master_Currency_Id { get; set; }
        public int? Default_Currency_Id { get; set; }
        public int? ParentId { get; set; }
        public List<int> OperationCountries { get; set; }
        public string Comment { get; set; }
        public int? EntityId { get; set; }
        public string OperationCountriesName { get; set; }

    }

    public class OperationCountry
    {
        public int Id { get; set; }
        public List<int> OperationCountries { get; set; }
        public string OperationCountriesName { get; set; }

    }
    public class OfficeType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? EntityId { get; set; }
    }

    public class OfficeResponse
    {
        public IEnumerable<Office> Offices { get; set; }
        public OfficeResponseResult Result { get; set; }
    }
    public enum OfficeResponseResult
    {
        success=1,
        Error=2
    }
}
