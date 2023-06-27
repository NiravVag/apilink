using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.CustomerProducts
{
    public class OcrTableRequest
    {
        public string file { get; set; }
        public string brand_name { get; set; }
        public string brand_format { get; set; }
    }
    public class OcrSettings
    {
        public string BaseUrl { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string OCRTableRequestUrl { get; set; }
    }

    public class ExportOcrData
    {
        [Description("Size")]
        public string Code { get; set; }
        [Description("Code")]
        public string Mpcode { get; set; }
        [Description("Description")]
        public string Description { get; set; }
        [Description("Req. Value")]
        public double? Required { get; set; }
        [Description("Tol +")]
        public double? Tolerance1Up { get; set; }
        [Description("Tol -")]
        public double? Tolerance1Down { get; set; }
    }
}
