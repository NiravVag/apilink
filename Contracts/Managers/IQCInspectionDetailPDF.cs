using DTO.FBInternalReport;
using DTO.File;
using DTO.Inspection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Managers
{
    public interface IQCInspectionDetailPDF
    {
        FileResponse CreateQCInspectionDetailDocument(QCInspectionDetailsPDF model);
    }
}
