using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Audit
{
    public class AuditProductCategoryResponse
    {
        public IEnumerable<AuditProductCategory> Data { get; set; }
        public AuditProductCategoryResult Result { get; set; }
    }
    public class AuditProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public enum AuditProductCategoryResult
    {
        Success = 1,
        CannotGetList = 2,
        Failed = 3,
        RequestNotCorrectFormat = 4,
        ServiceIdRequired = 5
    }
}
