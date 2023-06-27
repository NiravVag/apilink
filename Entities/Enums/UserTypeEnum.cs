using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Enums
{
    public enum UserTypeEnum
    {
         InternalUser = 1, 
         Customer = 2, 
         Supplier = 3,
         Factory = 4,
         OutSource=5
    }

    public enum APIServiceEnum
    {
        Inspection = 1,
        Audit = 2,
        TCF = 3,
        LabTesting = 4
    }

    public enum EmployeeTypeEnum
    {
        Permanent = 1,
        OutSource = 2,
        Other = 3,
    }
}
