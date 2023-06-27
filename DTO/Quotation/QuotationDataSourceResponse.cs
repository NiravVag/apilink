using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Quotation
{
    public class QuotationDataSourceResponse
    {

        public IEnumerable<DataSource> DataSource { get; set;  }

        public QuotationDataSourceResult Result { get; set;  }
    }

    public enum QuotationDataSourceResult
    {
        Success = 1,
        CountryEmpty =2,
        ServiceEmpty  =3,
        NotFound  = 4,
        CustomerEmpty = 5,
    }

    public class DataSource
    {
        public int Id { get; set;  }

        public string Name { get; set;  }

        public string Address { get; set;  }

        public int InvoiceType { get; set; }

        public bool IsForwardToManager { get; set; }

        public int? CountyId { get; set; }

        public int? CityId { get; set; }

        public int? ProvinceId { get; set; }
    }




}
