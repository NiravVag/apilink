using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.ProductManagement
{
    public enum SaveProductManagementResult
    {
        Success = 1,
        CannotSaveProductManagement = 2,
        CurrentProductManagementNotFound = 3,
        CannotMapRequestToEntites = 4,
        DuplicateName = 5,
        CannotSaveInFB = 6,
        CannotUpdateInFB = 7
    }

    public enum ProductManagementResult
    {
        Success = 1,
        CannotGetProductManagementList = 2,
        NotFound = 3
    }

    public enum EditProductManagementResult
    {
        Success = 1,
        CanNotGetProductManagementList = 2,
        CanNotGetProductManagement = 3
    }

    public enum DeleteProductManagementResult
    {
        Success = 1,
        NotFound = 2,
        CannotDelete = 3,
        ProductMapped = 4
    }
}
