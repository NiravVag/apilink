using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.InspectionPicking
{
    public class SaveInspectionPickingResponse
    {
        public int Id { get; set; }
        public SaveInspectionPickingResult Result { get; set; }      
    }
    public enum SaveInspectionPickingResult
    {
        Success = 1,
        InspectionPickingIsNotSaved = 2,
        InspectionPickingIsNotFound = 3,
        InspectionPickingExists = 4
    }
}
