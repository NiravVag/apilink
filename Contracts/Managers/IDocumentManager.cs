using DTO.File;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
   public interface IDocumentManager
    {
        /// <summary>
        /// Get Inspection Terms and Condition file
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>EvaluationRound</returns>
        FileResponse GetFileData(string filepath, string inspBookTerms);
    }
}
