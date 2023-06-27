using DTO.CommonClass;
using DTO.Inspection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ProductManagement
{
    public class ProductSubCategory3Response
    {
        public List<CommonDataSource> ProductSubCategory3List { get; set; }
        public ProductSubCategory3Result Result { get; set; }
    }

    public enum ProductSubCategory3Result
    {
        Success = 1,
        CannotGetList = 2,
        Failed = 3
    }
}
