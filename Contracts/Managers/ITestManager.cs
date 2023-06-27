using DTO.FullBridge;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Managers
{
   public interface ITestManager
    {
        bool TestMethod(FbReportDataRequest request);
    }
}
