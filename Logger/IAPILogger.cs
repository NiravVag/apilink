using System;
using System.Collections.Generic;
using System.Text;

namespace LoggerComponent
{
    public interface IAPILogger
    {
        void SaveRestAPILog(RestApiLog restApiLog);
    }
}
