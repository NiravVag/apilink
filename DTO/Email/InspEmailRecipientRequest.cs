using System;
using System.Collections.Generic;
using System.Text;
using Entities.Enums;
using Entities;
namespace DTO.Email
{
   public class InspEmailRecipientRequest
    {
        public IEnumerable<int> LstInspectionIds { get; set; }

        public EmailType EmailtypeId { get; set; }
    }
}
