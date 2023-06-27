using DTO.CommonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.CustomerProducts
{
    public class MSChartFileFormatResponse
    {
        public IEnumerable<MSChartFileFormat> DataSourceList { get; set; }
        public DataSourceResult Result { get; set; }
    }
    public class MSChartFileFormat
    {
        public int Id { get; set; }
        public string OcrCustomerName { get; set; }
        public string OcrFileFormat { get; set; }
    }
}
