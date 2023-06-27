using Contracts.Repositories;
using DTO.Common;
using DTO.Lab;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BI.Maps
{
    public class LabMap : ApiCommonData
    {
        public LabType GetLabType(InspLabType entity)
        {
            if (entity == null)
                return null;
            return new LabType
            {
                Id = entity.Id,
                Type = entity.Type
            };
        }

        public LabMaster GetLabMasterData(InspLabDetail entity)
        {
            if (entity == null)
                return null;
            return new LabMaster
            {
                Id = entity.Id,
                Name = entity.LabName,
                Type = LabTypeEnum.Lab
            };
        }

        public LabMaster GetLabMasterAddressData(InspLabAddress entity)
        {
            if (entity == null)
                return null;
            return new LabMaster
            {
                Id = entity.Id,
                Name = entity.CountryId == Convert.ToInt32(CountryEnum.China) ? (entity.Address + "(" + entity.RegionalLanguage + ")") : entity.Address
            };
        }

        public LabMaster GetLabMasterContactsData(InspLabContact entity)
        {
            if (entity == null)
                return null;
            return new LabMaster
            {
                Id = entity.Id,
                Name = entity.ContactName
            };
        }

        public LabAddressType GetLabAddressType(InspLabAddressType entity)
        {
            if (entity == null)
                return null;
            return new LabAddressType
            {
                Id = entity.Id,
                AddressType = entity.AddressType,
                TranslationId = entity.TranslationId,
                EntityId = entity.EntityId,
            };
        }

        public LabCustomer GetLabCustomer(InspLabCustomer entity)
        {
            if (entity == null)
                return null;

            return new LabCustomer
            {
                LabId = entity.LabId,
                CustomerId = entity.CustomerId,
                Code = entity.Code,
                CustomerName = entity.Customer.CustomerName
            };
        }

        public LabCustomerContact GetLabCustomerContact(InspLabCustomerContact entity)
        {
            if (entity == null)
                return null;

            return new LabCustomerContact
            {
                LabId = entity.LabId,
                CustomerId = entity.CustomerId,
                ContactId = entity.ContactId
            };
        }

        public LabCustomer GetLabCustomerContactForFE(InspLabCustomerContact entity)
        {
            if (entity == null)
                return null;

            return new LabCustomer
            {
                LabId = entity.LabId,
                CustomerId = entity.CustomerId,
                CustomerName = entity.Customer.CustomerName
            };
        }

        public LabAddress GetLabAddress(InspLabAddress entity)
        {
            if (entity == null)
                return null;

            return new LabAddress
            {
                Id = entity.Id,
                CountryId = entity.CountryId,
                ProvinceId = entity.ProvinceId,
                CityId = entity.CityId,
                ZipCode = entity.ZipCode,
                Address = entity.Address,
                RegionalLanguage = entity.RegionalLanguage,
                LabId = entity.LabId,
                AddressTypeId = entity.AddressTypeId
            };
        }

        public LabDetails GetLabDetails(InspLabDetail entity)
        {
            if (entity == null)
                return null;
            return new LabDetails
            {
                Id = entity.Id,
                LabName = entity.LabName,
                Comments = entity.Comments,
                Active = entity.Active,
                TypeId = entity.TypeId,
                LegalName = entity.LegalName,
                Email = entity.Email,
                Phone = entity.Phone,
                Fax = entity.Fax,
                Website = entity.Website,
                Mobile = entity.Mobile,
                RegionalName = entity.RegionalName,
                ContactPerson = entity.ContactPerson,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedBy,
                GlCode = entity.GlCode,
                EntityId = entity.EntityId,
                AddressList = entity.InspLabAddresses.Select(GetAddress),
                ContactsList = entity.InspLabContacts.Where(x => x.Active.HasValue && x.Active.Value).Select(GetLabContact),
                CustomerList = entity.InspLabCustomers.Select(GetLabCustomer)
            };
        }

        public LabAddress GetAddress(InspLabAddress entity)
        {
            if (entity == null)
                return null;

            return new LabAddress
            {
                Id = entity.Id,
                CityId = entity.CityId,
                CountryId = entity.CountryId,
                RegionalLanguage = entity.RegionalLanguage,
                ZipCode = entity.ZipCode,
                ProvinceId = entity.ProvinceId,
                AddressTypeId = entity.AddressTypeId == null ? 0 : entity.AddressTypeId.Value,
                Address = entity.Address,
                LabId = entity.LabId == null ? 0 : entity.LabId.Value
            };

        }

        public LabContact GetLabContact(InspLabContact entity)
        {
            if (entity == null)
                return null;

            return new LabContact
            {
                Id = entity.Id,
                LabId = entity.LabId,
                Active = entity.Active,
                ContactName = entity.ContactName,
                Comment = entity.Comment,
                Fax = entity.Fax,
                JobTitle = entity.JobTitle,
                Mobile = entity.Mobile,
                Phone = entity.Phone,
                Mail = entity.Mail,
                CustomerList = entity.InspLabCustomerContacts.Select(GetLabCustomerContactForFE)

            };

        }

        public InspLabDetail MapLabDetailEntity(LabDetails request, int entityId, int userId)
        {
            // Lab Details
            var entity = new InspLabDetail
            {
                LabName = request.LabName?.Trim(),
                Active = true,
                EntityId = entityId,
                Comments = request.Comments?.Trim(),
                LegalName = request.LegalName?.Trim(),
                Fax = request.Fax?.Trim(),
                Email = request.Email?.Trim(),
                ContactPerson = request.ContactPerson?.Trim(),
                CreatedDate = DateTime.Now,
                CreatedBy = userId,
                GlCode = request.GlCode,
                RegionalName = request.RegionalName?.Trim(),
                Mobile = request.Mobile?.Trim(),
                Phone = request.Phone?.Trim(),
                TypeId = request.TypeId,
                Website = request.Website?.Trim()
            };

            // Lab Address List
            if (request.AddressList != null)
            {
                foreach (var item in request.AddressList.Where(x => x.CountryId > 0))
                {
                    var address = new InspLabAddress
                    {
                        Id = item.Id,
                        LabId = request.Id,
                        CountryId = item.CountryId,
                        CityId = item.CityId,
                        ProvinceId = item.ProvinceId,
                        ZipCode = item.ZipCode,
                        Address = item.Address?.Trim(),
                        AddressTypeId = item.AddressTypeId,
                        RegionalLanguage = item.RegionalLanguage?.Trim()
                    };
                    entity.InspLabAddresses.Add(address);
                }
            }

            // Lab Customer List
            if (request.CustomerList != null)
            {
                foreach (var item in request.CustomerList)
                {
                    var customer = new InspLabCustomer
                    {
                        LabId = request.Id,
                        CustomerId = item.CustomerId,
                        Code = item.Code?.Trim()
                    };
                    entity.InspLabCustomers.Add(customer);
                }
            }

            // Lab Contact List
            if (request.ContactsList != null)
            {
                foreach (var item in request.ContactsList.Where(x => !string.IsNullOrEmpty(x.ContactName)))
                {
                    // Lab Customer Contact List

                    var contact = new InspLabContact
                    {
                        Id = item.Id,
                        LabId = request.Id,
                        Active = true,
                        Comment = item.Comment?.Trim(),
                        ContactName = item.ContactName?.Trim(),
                        Fax = item.Fax?.Trim(),
                        JobTitle = item.JobTitle?.Trim(),
                        Mail = item.Mail?.Trim(),
                        Mobile = item.Mobile?.Trim(),
                        Phone = item.Phone?.Trim(),
                    };

                    if (item.CustomerList != null)
                    {
                        foreach (var customer in item.CustomerList)
                        {
                            var custContact = new InspLabCustomerContact
                            {
                                LabId = request.Id,
                                CustomerId = customer.CustomerId,
                                Contact = contact
                            };
                            entity.InspLabCustomerContacts.Add(custContact);
                        }
                    }
                    entity.InspLabContacts.Add(contact);
                }
            }
            return entity;
        }

        public void UpdateLabEnity(InspLabDetail entity, LabDetails request, ILabRepository _labRepo)
        {
            // Lab Details
            entity.LabName = request.LabName?.Trim();
            entity.Comments = request.Comments?.Trim();
            entity.LegalName = request.LegalName?.Trim();
            entity.Fax = request.Fax?.Trim();
            entity.Email = request.Email?.Trim();
            entity.ContactPerson = request.ContactPerson?.Trim();
            entity.GlCode = request.GlCode;
            entity.RegionalName = request.RegionalName?.Trim();
            entity.Mobile = request.Mobile?.Trim();
            entity.Phone = request.Phone?.Trim();
            entity.TypeId = request.TypeId;
            entity.Website = request.Website?.Trim();

            // Address List
            var addressIds = request.AddressList.Where(x => x.CountryId > 0).Select(x => x.Id).ToArray();
            var lstAddressToremove = new List<InspLabAddress>();
            var labAddresses = entity.InspLabAddresses.Where(x => !addressIds.Contains(x.Id)).ToList();
            foreach (var item in labAddresses)
            {
                lstAddressToremove.Add(item);
                entity.InspLabAddresses.Remove(item);
            }
            _labRepo.RemoveEntities(lstAddressToremove);

            if (request.AddressList != null)
            {
                foreach (var item in request.AddressList.Where(x => x.Id <= 0 && x.CountryId > 0))
                    entity.InspLabAddresses.Add(new InspLabAddress
                    {
                        CountryId = item.CountryId,
                        CityId = item.CityId,
                        ProvinceId = item.ProvinceId,
                        ZipCode = item.ZipCode,
                        Address = item.Address?.Trim(),
                        AddressTypeId = item.AddressTypeId,
                        RegionalLanguage = item.RegionalLanguage?.Trim()
                    });

                var lstAddressToEdit = new List<InspLabAddress>();
                foreach (var item in request.AddressList.Where(x => x.Id > 0 && x.CountryId > 0))
                {
                    var address = entity.InspLabAddresses.FirstOrDefault(x => x.Id == item.Id);

                    if (address != null)
                    {
                        lstAddressToEdit.Add(address);

                        address.CountryId = item.CountryId;
                        address.CityId = item.CityId;
                        address.RegionalLanguage = item.RegionalLanguage;
                        address.ZipCode = item.ZipCode;
                        address.Address = item.Address?.Trim();
                        address.AddressTypeId = item.AddressTypeId;
                        address.ProvinceId = item.ProvinceId;
                    }
                }
                if (lstAddressToEdit.Count > 0)
                    _labRepo.EditEntities(lstAddressToEdit);
            }

            // CustomerList
            var labCustomers = entity.InspLabCustomers.ToList();
            foreach (var item in labCustomers)
                entity.InspLabCustomers.Remove(item);

            if (labCustomers.Count > 0)
                _labRepo.RemoveEntities(labCustomers);

            if (request.CustomerList != null)
            {
                foreach (var item in request.CustomerList)
                    entity.InspLabCustomers.Add(new InspLabCustomer
                    {
                        CustomerId = item.CustomerId,
                        Code = item.Code?.Trim()
                    });
            }


            // Customer ContactList
            var lstContactToRemove = new List<InspLabContact>();

            var labCustomerContacts = entity.InspLabCustomerContacts.ToList();

            foreach (var item in labCustomerContacts)
                entity.InspLabCustomerContacts.Remove(item);
            _labRepo.RemoveEntities(labCustomerContacts);

            if (request.ContactsList != null)
            {
                var ContactIds = request.ContactsList.Where(x => x.Id > 0 && !string.IsNullOrEmpty(x.ContactName)).Select(x => x.Id).ToArray();
                var labContacts = entity.InspLabContacts.Where(x => !ContactIds.Contains(x.Id)).ToList();

                foreach (var item in labContacts)
                {
                    item.Active = false;
                    lstContactToRemove.Add(item);
                }

                if (lstContactToRemove.Count > 0)
                    _labRepo.EditEntities(lstContactToRemove);

                foreach (var item in request.ContactsList.Where(x => x.Id <= 0 && !string.IsNullOrEmpty(x.ContactName)))
                {
                    var contact = new InspLabContact
                    {
                        Comment = item.Comment?.Trim(),
                        ContactName = item.ContactName?.Trim(),
                        Fax = item.Fax?.Trim(),
                        JobTitle = item.JobTitle?.Trim(),
                        Mail = item.Mail?.Trim(),
                        Mobile = item.Mobile?.Trim(),
                        Phone = item.Phone?.Trim()
                    };

                    entity.InspLabContacts.Add(contact);

                    foreach (var cust in item.CustomerList)
                    {
                        entity.InspLabCustomerContacts.Add(new InspLabCustomerContact
                        {
                            LabId = entity.Id,
                            CustomerId = cust.CustomerId,
                            Contact = contact
                        });
                    }
                }

                foreach (var item in request.ContactsList.Where(x => x.Id > 0 && !string.IsNullOrEmpty(x.ContactName)))
                {
                    var contact = entity.InspLabContacts.FirstOrDefault(x => x.Id == item.Id);

                    if (contact != null)
                    {
                        contact.Comment = item.Comment?.Trim();
                        contact.ContactName = item.ContactName?.Trim();
                        contact.Fax = item.Fax?.Trim();
                        contact.JobTitle = item.JobTitle?.Trim();
                        contact.Mail = item.Mail?.Trim();
                        contact.Mobile = item.Mobile?.Trim();
                        contact.Phone = item.Phone?.Trim();


                        foreach (var cust in item.CustomerList)
                        {
                            entity.InspLabCustomerContacts.Add(new InspLabCustomerContact
                            {
                                LabId = entity.Id,
                                CustomerId = cust.CustomerId,
                                ContactId = item.Id
                            });
                        }

                    }
                }
            }
        }

        public LabItem MapLabItem(InspLabDetail entity)
        {
            if (entity == null)
                return null;

            var address = entity.InspLabAddresses?.FirstOrDefault();

            var item = new LabItem
            {
                Id = entity.Id,
                LabName = entity.LabName,
                CountryName = address?.Country?.CountryName,
                ProvinceName = address?.Province?.ProvinceName,
                CityName = address?.City?.CityName,
                TypeName = entity.Type?.Type
            };
            return item;
        }
        public LabItemSelect GetLabSelect(InspLabDetail entity)
        {
            if (entity == null)
                return null;
            return new LabItemSelect
            {
                Id = entity.Id,
                LabName = entity.LabName
            };
        }

        /// <summary>
        /// Get the lab address data from labaddressdata entity
        /// </summary>
        /// <param name="addressData"></param>
        /// <returns></returns>
        public LabBaseAddress GetLabAddressData(LabAddressData addressData)
        {
            if (addressData == null)
                return null;
            return new LabBaseAddress
            {
                Id = addressData.Id,
                Name = addressData.CountryId == (int)CountryEnum.China ? (addressData.Address + "(" + addressData.RegionalLanguage + ")") : addressData.Address,
                LabId = addressData.LabId
            };
        }

        public InspLabAddress MapLabAddressData(SaveLabAddressData addressData)
        {
            return new InspLabAddress
            {
                CountryId = addressData.CountryId,
                CityId = addressData.CityId,
                ProvinceId = addressData.ProvinceId,
                ZipCode = addressData.ZipCode,
                Address = addressData.Address?.Trim(),
                AddressTypeId = (int)LabAddressTypeEnum.RegionOffice,
                RegionalLanguage = addressData.LocalLanguage?.Trim()
            };
        }
    }
}
