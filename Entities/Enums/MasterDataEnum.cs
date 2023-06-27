using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Enums
{
    public enum MasterDataType
    {
        CustomerCreation = 1,
        SupplierCreation = 2,
        FactoryCreation = 3,
        FactoryUpdation = 4,
        ProductCreation = 5,
        CustomerContactCreation = 6,
        UserCreation = 7,
        BuyerCreation = 8,
        SupplierContactCreation = 9
    }

    public enum ExternalClient
    {
        FullBridge = 1,
        TCF = 2
    }
}
