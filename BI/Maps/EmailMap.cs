using DTO.Common;
using DTO.Email;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BI.Maps
{
    public class EmailMap : ApiCommonData
    {
        public  EmailContact GetInternalEmailContact(HrStaff entity)
        {
            if (entity == null)
                return null;
            return new EmailContact
            {
                ContactEmail = entity.CompanyEmail ?? "",
                ContactName = entity.PersonName ?? "",
                ContactPhno = entity.CompanyMobileNo ?? "",
                Id = entity.Id
            };
        }
        public  EmailContact GetCusEmailContact(CuContact entity)
        {
            if (entity == null)
                return null;
            return new EmailContact
            {
                ContactEmail = entity.Email ?? "",
                ContactName = entity.ContactName ?? "",
                ContactPhno = entity.Phone ?? "",
                Id = entity.Id
            };
        }
        public  EmailContact GetSupplierEmailContact(SuContact entity)
        {
            if (entity == null)
                return null;
            return new EmailContact
            {
                ContactEmail = entity.Mail ?? "",
                ContactName = entity.ContactName ?? "",
                ContactPhno = entity.Phone ?? "",
                Id = entity.Id
            };
        }
    }
}
