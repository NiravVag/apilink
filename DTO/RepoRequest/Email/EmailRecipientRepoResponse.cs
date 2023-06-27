using System;
using System.Collections.Generic;
using System.Text;
using Entities;
namespace DTO.RepoRequest.Email
{
  public  class EmailRecipientRepoResponse
    {
        public IEnumerable<MidEmailRecipientsConfiguration> RecipientConfigList { get; set; }

        public InspEmailRecipientRepoResponseResult Result { get; set; }
    }
    public enum InspEmailRecipientRepoResponseResult
    {
        success=1,
        NoBookingFound=2
    }
}
