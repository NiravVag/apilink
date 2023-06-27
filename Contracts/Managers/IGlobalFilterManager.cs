using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Managers
{

    public interface ITenantProvider
    {
        int GetCompanyId();
    }
}
