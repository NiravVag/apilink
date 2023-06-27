using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Claim
{
    public class SaveClaimResponse
    {
        public int Id { get; set; }
        public SaveClaimResult Result { get; set; }
        public List<ErrorData> ErrorList { get; set; }
    }

    public enum SaveClaimResult
    {
        Success = 1,
        CannotAddClaim = 2,
        CurrentClaimNotFound = 3,
        CannotMapRequestToEntites = 4,
        CustomErrors = 5,
        DuplicateEmailIDExists = 6,
        ClaimSavedNotificationError = 7,
        ClaimReportsExist = 8,
    }
}
