using DTO.Customer;
using DTO.Location;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Supplier
{
    public class EditSupplierResponse
    {
        public IEnumerable<SupplierType> TypeList { get; set;  }

        public IEnumerable<Level> LevelList { get; set;  }

        public IEnumerable<OwnerShip> OwnerList { get; set; }

        public IEnumerable<CustomerItem> CustomerList { get; set;  }

        public IEnumerable<Country> CountryList { get; set;  }

        public IEnumerable<SupplierItem> SupplierList { get; set;  }

        public IEnumerable<AddressType> AddressTypeList { get; set;  }

        public IEnumerable<CreditTerm> CreditTermList { get; set; }

        public IEnumerable<Status> StatusList { get; set; }

        public SupplierDetails SupplierDetails { get; set;  }

        public bool IsAnyActiveSupplierContactUser { get; set; }
        public EditSupplierResult Result { get; set;  }
        public List<LevelCustom> LevelCustomList { get; set; }
    }

    public enum EditSupplierResult
    {
        Success = 1,
        CannotGetTypeList = 2,
        CannotGetLevelList = 3,
        CannotGetOwnerList = 4,
        CannotGetCustomerList = 5, 
        CannotGetCountryList = 6, 
        CannotGetSelectSupplier = 9,
        CannotGetAddressTypes = 10,
        CannotGetCreditTerm = 11,
        CannotGetStatus = 12,
        CannotGetLevelCustomList = 13
    }


}
