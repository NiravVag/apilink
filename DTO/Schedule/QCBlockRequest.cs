using DTO.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Schedule
{
    public class QCBlockRequest
    {
        public int Id { get; set; }

        [RequiredGreaterThanZero(ErrorMessage = "EDIT_QC_BLOCK.LBL_QC_REQUIRED")]
        public int QCId { get; set; }

        public IEnumerable<int> ProductCategoryIds { get; set; }
        public IEnumerable<int> CustomerIds { get; set; }
        public IEnumerable<int> SupplierIds { get; set; }
        public IEnumerable<int> FactoryIds { get; set; }
        public IEnumerable<int> ProductCategorySubIds { get; set; }
        public IEnumerable<int> ProductCategorySub2Ids { get; set; }
    }

    public enum QCBlockResponseResult
    {
        Success = 1,
        Failure = 2,
        RequestNotCorrectFormat = 3,
        NoDataFound = 4,
        IsExists = 5,
        SelectAnyOtherField = 6
    }

    public class SaveQCBlockResponse
    {
        public int Id { get; set; }
        public QCBlockResponseResult Result { get; set; }
    }

    public class EditQCBlockResponse
    {
        public QCBlockRequest QCBlockDetails { get; set; }
        public QCBlockResponseResult Result { get; set; }
    }
    public class QCBlockSummaryRequest
    {
        public IEnumerable<int> OfficeIds { get; set; }
        public IEnumerable<int> QCIds { get; set; }
        public int? Index { get; set; }
        public int? PageSize { get; set; }
    }
    public class DeleteQCBlockResponse
    {
        public QCBlockResponseResult Result { get; set; }
    }
}
