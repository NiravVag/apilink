using DTO.CommonClass;
using DTO.Customer;
using DTO.Location;
using DTO.References;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Quotation
{
    public class QuotationResponse
    {
        public IEnumerable<Country> CountryList { get; set;  }

        public IEnumerable<References.Service> ServiceList { get; set;  }

        public IEnumerable<BillingMethod> BillingMethodList { get; set;  }

        public IEnumerable<BillPaidBy> BillPaidByList { get; set;  }
        
        public IEnumerable<DataSource> OfficeList { get; set;  }

        public IEnumerable<Currency> CurrencyList { get; set;  }

        public QuotationDetails Model { get; set;  }

        public IEnumerable<DataSource> CustomerList { get; set;  }

        public IEnumerable<DataSource> SupplierList { get; set; }


        public IEnumerable<DataSource> FactoryList { get; set; }

        public IEnumerable<QuotationAbility>  Abilities { get; set;  }

        public IEnumerable<CommonDataSource> BillingEntities { get; set; }

        public IEnumerable<CustomerSource> PaymentTermList { get; set; }
        public IEnumerable<PaymentTypeSource> PaymentTermsValueList { get; set; }

        public QuotationResult Result { get; set;  }

        public bool IsPreInvoiceContactMandatoryInQuotation { get; set; }


    }

    public enum QuotationResult
    {
        Success = 1, 
        CannotFindCountryList = 2, 
        CannotFindServiceList = 3,
        CannotFindBillingMethodList = 4,
        CannotFindBillPaidByList = 5,
        CannotFindOfficeList  = 6,
        CannotFindCurrencies = 7,
        CurrentQuotationNotFound = 8,
        CannotGetCustList = 9,
        CannotGetSuppList = 10,
        CannotGetFactoryList = 11,
        NoAccess = 12,
        CannotFindBillingEntities = 13,
        CannotFindPaymentTerms = 14
    }

}
