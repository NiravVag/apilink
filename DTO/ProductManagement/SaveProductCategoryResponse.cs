using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.ProductManagement
{
    public class SaveProductCategoryResponse
    {
        public int ProductId { get; set; }
        public SaveProductManagementResult Result { get; set; }
    }
}
