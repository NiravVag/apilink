using DTO.File;
using DTO.InspectionCertificate;

namespace Contracts.Managers
{
    public interface IPDF
    {
        /// <summary>
        /// Create PDF Document for Inspection certificate
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        FileResponse CreateICDocument(InspectionCertificatePDF model);
    }
}
