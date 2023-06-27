using DTO.File;
using DTO.Inspection;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Managers
{
    public interface IPickingPDF
    {

        /// <summary>
        /// Create PDF Document for Inspection certificate
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        FileResponse CreatePickingDocument(IEnumerable<QcPickingData> model, EntMasterConfigItem entMasterConfig);
    }
}
