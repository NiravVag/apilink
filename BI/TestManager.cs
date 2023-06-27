using Contracts.Managers;
using DTO.FullBridge;
using System;
using System.Collections.Generic;
using System.Text;

namespace BI
{
    public class TestManager : ITestManager
    {
        public bool TestMethod(FbReportDataRequest request)
        {
            return true;
        }
    }
}
