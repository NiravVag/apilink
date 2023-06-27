using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.RepoRequest.Email
{
   public class EmailRecipientRepoRequest
    {
        public IEnumerable<int> LstBrand { get; set; }

        public IEnumerable<int> LstDepartment { get; set; }

        public IEnumerable<int> LstBuyer { get; set; }

        public IEnumerable<int> LstCustomer { get; set; }

        public IEnumerable<int> LstSupplier { get; set; }

        public IEnumerable<int> LstFactory { get; set; }

        public IEnumerable<int> LstPODestinationCountry { get; set; }

        public IEnumerable<int> LstFactoryCountry { get; set; }

        public IEnumerable<int> LstOffice { get; set; }

        public IEnumerable<int> LstService { get; set; }

        public IEnumerable<int> LstServiceType { get; set; }

        public IEnumerable<int> LstProductcategory { get; set; }

        public IEnumerable<int> LstSubProductcategory { get; set; }

        public EmailType EmailtypeId { get; set; }
    }
}
