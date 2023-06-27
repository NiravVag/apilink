using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.InvoiceDataAccess
{
    public class SaveInvoiceDataAccessResponse
    {
        public int Id { get; set; }
        public SaveInvoiceDataAccessResult Result { get; set; }
    }

    public enum SaveInvoiceDataAccessResult
    {
        Success = 1,
        Failed = 2,
        RequestNotCorrectFormat = 3,
        NotFound = 4,
        Error = 5,
        Exists = 6,
    }
}
