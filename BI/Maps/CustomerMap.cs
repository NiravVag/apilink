using DTO.Common;
using DTO.Customer;
using DTO.CustomerProducts;
using DTO.HumanResource;
using DTO.Invoice;
using DTO.InvoicePreview;
using DTO.References;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static BI.TenantProvider;

namespace BI.Maps
{
    public class CustomerMap : ApiCommonData
    {
        public CustomerItem GetCustomerItem(CuCustomer entity, string code, int? creditTerm = null)
        {
            if (entity == null)
                return null;

            return new CustomerItem
            {
                Id = entity.Id,
                Name = entity.CustomerName ?? "",
                Code = code,
                Email = entity.Email,
                CreditTerm = creditTerm,
                BookingDefaultComments = entity.BookingDefaultComments
            };
        }

        public CustomerItem GetCustomerItems(CuCustomer entity, string groupName)
        {
            if (entity == null)
                return null;

            return new CustomerItem
            {
                Id = entity.Id,
                Name = entity.CustomerName,
                GroupName = groupName
            };
        }


        public CustomerDetails GetCustomerDetails(CuCustomer entity)
        {
            if (entity == null)
                return null;

            var item = new CustomerDetails
            {
                ZohoCustomerId = entity.ZohoCustomerId != null ? entity.ZohoCustomerId : null,
                Id = entity.Id,
                Name = entity.CustomerName,
                Fax = entity.Fax,
                Group = entity.Group,
                GlCode = entity.GlCode,
                Category = entity.Category,
                Email = entity.Email,
                ComplexityLevel = entity.ComplexityLevel,
                BusinessCountry = entity.CuCustomerBusinessCountries?.Select(x => x.BusinessCountryId).ToArray(),
                ApiServiceIds = entity.CuApiServices.Where(x => x.Active.HasValue && x.Active.Value).Select(x => x.ServiceId).ToArray(),
                BusinessType = entity.BusinessType,
                Phone = entity.Phone,
                OtherPhone = entity.OtherPhone,
                Others = entity.Others,
                Kam = entity.CuKams.Where(x => x.Active.HasValue && x.Active == 1)?.Select(x => x.KamId).ToArray(),
                MargetSegment = entity.MargetSegment,
                Website = entity.Website,
                SkillsRequired = entity.SkillsRequired,
                ProspectStatus = entity.ProspectStatus,
                QuatationName = entity.QuatationName,
                IcRequired = entity.IcRequired,
                Language = entity.Language,
                StartDate = Static_Data_Common.GetCustomDate(entity.StartDate),
                Comments = entity.Comments,
                BookingDefaultComments = entity.BookingDefaultComments,
                InvoiceType = entity.InvoiceType,
                CustomerAddresses = GetAddressList(entity),
                AccountingLeader = entity.AccountingLeaderId,
                SalesIncharge = entity.CuSalesIncharges.Where(x => x.Active.HasValue && x.Active == 1)?.Select(x => x.StaffId).ToArray(),
                ActivitiesLevel = entity.ActvitiesLevelId,
                RelationshipStatus = entity.RelationshipStatusId,
                BrandPriority = entity.CuBrandpriorities.Where(x => x.Active.HasValue && x.Active == 1)?.Select(x => x.BrandpriorityId).ToArray(),
                DirectCompetitor = entity.DirectCompetitor,
                CustomerEntityIds = entity.CuEntities.Where(x => x.Active == true).Select(y => y.EntityId).ToList(),
                CompanyId = entity.CompanyId.GetValueOrDefault(),
                SisterCompanyIds = entity.CuSisterCompanyCustomers.Where(x => x.Active == true).Select(y => y.SisterCompanyId).ToList()
            };


            return item;

        }

        private List<CustomerAddress> GetAddressList(CuCustomer entity)
        {
            if (entity == null)
                return null;

            List<CustomerAddress> lstCustomerAddress = new List<CustomerAddress>();

            foreach (var item in entity.CuAddresses.Where(x => x.Active.HasValue && x.Active.Value))
            {
                var address = new CustomerAddress
                {
                    Id = item.Id,
                    Address = item.Address,
                    BoxPost = item.BoxPost,
                    ZipCode = item.ZipCode,
                    AddressType = item.AddressType,
                    CountryId = item.CountryId,
                    CityId = item.CityId
                };
                lstCustomerAddress.Add(address);
            }

            return lstCustomerAddress;
        }

        public CuCustomer MapCustomerEntity(CustomerDetails request, int entityId, int userId, bool isITTeamRole)
        {
            if (request == null)
                return null;

            var customer = new CuCustomer
            {
                ZohoCustomerId = request.ZohoCustomerId,
                Id = request.Id,
                CustomerName = request.Name?.Trim()?.ToUpper(),
                Fax = request.Fax?.Trim(),
                GlCode = request.GlCode?.Trim(),
                Category = request.Category,
                Email = request.Email?.Trim(),
                ComplexityLevel = request.ComplexityLevel,
                BusinessType = request.BusinessType,
                Phone = request.Phone?.Trim(),
                OtherPhone = request.OtherPhone?.Trim(),
                Others = request.Others?.Trim(),
                MargetSegment = request.MargetSegment,
                Website = request.Website?.Trim(),
                SkillsRequired = request.SkillsRequired,
                ProspectStatus = request.ProspectStatus,
                QuatationName = request.QuatationName?.Trim(),
                Language = request.Language,
                Group = request.Group == 0 ? null : request.Group,
                InvoiceType = request.InvoiceType,
                StartDate = request.StartDate.ToDateTime(),
                Comments = request.Comments?.Trim(),
                BookingDefaultComments = request.BookingDefaultComments?.Trim(),
                AccountingLeaderId = request.AccountingLeader,
                ActvitiesLevelId = request.ActivitiesLevel,
                RelationshipStatusId = request.RelationshipStatus,
                DirectCompetitor = request.DirectCompetitor?.Trim(),
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                Active = true,
                CompanyId = request.CompanyId,
            };

            if (request.BusinessCountry != null)
            {
                customer.CuCustomerBusinessCountries = new HashSet<CuCustomerBusinessCountry>();
                foreach (var item in request.BusinessCountry)
                {
                    customer.CuCustomerBusinessCountries.Add(new CuCustomerBusinessCountry
                    {
                        BusinessCountryId = item,
                    });
                }
            }

            if (request.CustomerEntityIds != null && request.CustomerEntityIds.Any())
            {
                foreach (var entId in request.CustomerEntityIds)
                {
                    customer.CuEntities.Add(new CuEntity()
                    {
                        EntityId = entId,
                        Active = true,
                        CreatedBy = userId,
                        CreatedOn = DateTime.Now
                    });
                }
            }

            if (isITTeamRole)
            {
                //map the sister company ids
                if (request.SisterCompanyIds != null && request.SisterCompanyIds.Any())
                {
                    foreach (var customerId in request.SisterCompanyIds)
                    {
                        customer.CuSisterCompanyCustomers.Add(new CuSisterCompany()
                        {
                            CreatedBy = userId,
                            CreatedOn = DateTime.Now,
                            Active = true,
                            SisterCompanyId = customerId,
                            EntityId = entityId
                        });
                    }

                }
            }


            //add the api services to customer
            AddCustomerAPIServices(request, customer, userId);

            if (request.SalesCountry != null)
            {
                customer.CuCustomerSalesCountries = new HashSet<CuCustomerSalesCountry>();
                foreach (var item in request.SalesCountry)
                {
                    customer.CuCustomerSalesCountries.Add(new CuCustomerSalesCountry
                    {
                        SalesCountryId = item,
                    });
                }
            }

            if (request.Kam != null)
            {
                customer.CuKams = new HashSet<CuKam>();
                foreach (var item in request.Kam)
                {
                    customer.CuKams.Add(new CuKam
                    {
                        KamId = item,
                        CreatedBy = userId,
                        CreatedOn = DateTime.Now,
                        Active = 1
                    });
                }
            }

            if (request.SalesIncharge != null)
            {
                customer.CuSalesIncharges = new HashSet<CuSalesIncharge>();
                foreach (var item in request.SalesIncharge)
                {
                    customer.CuSalesIncharges.Add(new CuSalesIncharge
                    {
                        StaffId = item,
                        CreatedBy = userId,
                        CreatedOn = DateTime.Now,
                        Active = 1
                    });
                }
            }

            //add the Brand Priority to customer
            AddBrandPriority(request, customer, userId);


            return customer;
        }

        /// <summary>
        /// add the API services for the customer
        /// </summary>
        /// <param name="request"></param>
        /// <param name="customer"></param>
        /// <param name="userId"></param>
        private void AddCustomerAPIServices(CustomerDetails request, CuCustomer customer, int userId)
        {
            if (request.ApiServiceIds != null)
            {
                foreach (var id in request.ApiServiceIds)
                {
                    customer.CuApiServices.Add(new CuApiService()
                    {
                        ServiceId = id,
                        Active = true,
                        CreatedBy = userId,
                        CreatedOn = DateTime.Now
                    });
                }
            }
        }

        /// <summary>
        /// add the Brand Priority for the customer
        /// </summary>
        /// <param name="request"></param>
        /// <param name="customer"></param>
        /// <param name="userId"></param>
        private void AddBrandPriority(CustomerDetails request, CuCustomer customer, int userId)
        {
            if (request.BrandPriority != null)
            {
                foreach (var id in request.BrandPriority)
                {
                    customer.CuBrandpriorities.Add(new CuBrandpriority()
                    {
                        BrandpriorityId = id,
                        Active = 1,
                        CreatedBy = userId,
                        CreatedOn = DateTime.Now
                    });
                }
            }
        }

        public CuAddress MapCustomerAddressEntity(CustomerAddress request, int userId)
        {
            if (request == null)
                return null;

            var address = new CuAddress
            {
                Address = request.Address?.Trim(),
                BoxPost = request.BoxPost?.Trim(),
                ZipCode = request.ZipCode?.Trim(),
                AddressType = request.AddressType,
                CountryId = request.CountryId,
                CityId = request.CityId,
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                Active = true
            };

            return address;
        }

        public CustomerItem GetCustomerItemList(CuCustomer entity, string code)
        {
            if (entity == null)
                return null;
            return new CustomerItem
            {
                Id = entity.Id,
                Name = entity.CustomerName?.Trim(),
                Email = entity.Email?.Trim(),
                Fax = entity.Fax?.Trim(),
                ComplexityLevel = entity.ComplexityLevel,
                StartDate = entity.StartDate.GetCustomDate(),
                Website = entity.Website?.Trim(),
                Others = entity.Others?.Trim(),
                ProspectStatus = entity.ProspectStatus,
                SkillsRequired = entity.SkillsRequired,
                Kam = entity.Kam,
                Phone = entity.Phone,
                Category = entity.Category,
                MargetSegment = entity.MargetSegment,
                BusinessCountry = entity.CuCustomerBusinessCountries?.Select(x => x.BusinessCountryId).ToArray(),
                OtherPhone = entity.Others?.Trim(),
                Language = entity.Language,
                BusinessType = entity.BusinessType,
                QuatationName = entity.QuatationName?.Trim(),
                IcRequired = entity.IcRequired,
                GlCode = entity.GlCode?.Trim(),
                Comments = entity.Comments?.Trim(),
                GlRequired = entity.GlRequired,
                InvoiceType = entity.InvoiceType,
                Group = entity.Group,

            };
        }

        public void UpdateCustomerEnity(CuCustomer entity, CustomerDetails request, int userId)
        {
            entity.Id = request.Id;
            entity.CustomerName = request.Name?.Trim()?.ToUpper();
            entity.Email = request.Email?.Trim();
            entity.Fax = request.Fax?.Trim();
            entity.ComplexityLevel = request.ComplexityLevel;
            entity.StartDate = request.StartDate.ToDateTime();
            entity.Website = request.Website?.Trim();
            entity.Others = request.Others?.Trim();
            entity.ProspectStatus = request.ProspectStatus;
            entity.SkillsRequired = request.SkillsRequired;
            entity.Phone = request.Phone?.Trim();
            entity.Category = request.Category;
            entity.MargetSegment = request.MargetSegment;
            //entity.BusinessCountry = request.BusinessCountry;
            entity.OtherPhone = request.Others?.Trim();
            entity.Language = request.Language;
            entity.BusinessType = request.BusinessType;
            entity.QuatationName = request.QuatationName?.Trim();
            entity.IcRequired = request.IcRequired;
            entity.GlCode = request.GlCode?.Trim();
            entity.Comments = request.Comments?.Trim();
            entity.BookingDefaultComments = request.BookingDefaultComments?.Trim();
            entity.AccountingLeaderId = request.AccountingLeader;
            entity.ActvitiesLevelId = request.ActivitiesLevel;
            entity.RelationshipStatusId = request.RelationshipStatus;
            entity.DirectCompetitor = request.DirectCompetitor?.Trim();
            entity.Group = request.Group == 0 ? null : request.Group;
            entity.InvoiceType = request.InvoiceType;
            entity.UpdatedBy = userId;
            entity.UpdatedOn = DateTime.Now;
            entity.CompanyId = request.CompanyId;
            if (request.BusinessCountry != null)
            {
                if (entity.CuCustomerBusinessCountries == null)
                    entity.CuCustomerBusinessCountries = new HashSet<CuCustomerBusinessCountry>();

                entity.CuCustomerBusinessCountries.Clear();

                foreach (var item in request.BusinessCountry)
                {
                    entity.CuCustomerBusinessCountries.Add(new CuCustomerBusinessCountry
                    {
                        BusinessCountryId = item
                    });
                }
            }

            if (request.SalesCountry != null)
            {
                if (entity.CuCustomerSalesCountries == null)
                    entity.CuCustomerSalesCountries = new HashSet<CuCustomerSalesCountry>();

                entity.CuCustomerSalesCountries.Clear();

                foreach (var item in request.SalesCountry)
                {
                    entity.CuCustomerSalesCountries.Add(new CuCustomerSalesCountry
                    {
                        SalesCountryId = item
                    });
                }
            }
        }

        public CustomerBrand GetCustomerBrand(CuBrand entity)
        {
            if (entity == null)
                return null;
            return new CustomerBrand
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public CustomerDepartment GetCustomerDepartment(CuDepartment entity)
        {
            if (entity == null)
                return null;
            return new CustomerDepartment
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public CustomerBuyers GetCustomerBuyer(CuBuyer entity)
        {
            if (entity == null)
                return null;
            return new CustomerBuyers
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public Season GetCustomerSeason(CuSeason entity)
        {
            if (entity == null)
                return null;
            return new Season
            {
                Id = entity.SeasonId,
                Name = entity.Season?.Name,
                Code = entity.Season?.Code
            };
        }

        public CustomerContact GetCustomerContact(CuContact entity)
        {
            if (entity == null)
                return null;
            return new CustomerContact
            {
                Id = entity.Id,
                ContactName = entity.ContactName,
                LastName = entity.LastName,
                JobTitle = entity.JobTitle,
                Email = entity.Email,
                Mobile = entity.Mobile,
                Phone = entity.Phone,
                Fax = entity.Fax,
                Others = entity.Others,
                Office = entity.Office,
                Comments = entity.Comments,
                //ContactType = entity.ContactType,
                PromotionalEmail = entity.PromotionalEmail,
                CustomerId = entity.CustomerId,
                Active = entity.Active.GetValueOrDefault()
            };
        }

        public ServiceType GetCustomerServiceType(CuServiceType entity)
        {
            if (entity == null)
                return null;
            return new ServiceType
            {
                Id = entity.ServiceTypeId,
                Name = entity.ServiceType?.Name
            };
        }

        public CustomerContact GetCustomerContactItem(CuContact entity)
        {
            if (entity == null)
                return null;

            return new CustomerContact
            {
                Id = entity.Id,
                ContactName = entity.ContactName,
                LastName = entity.LastName,
                JobTitle = entity.JobTitle,
                Email = entity.Email,
                Mobile = entity.Mobile,
                Phone = entity.Phone,
                Fax = entity.Fax,
                Others = entity.Others,
                Office = entity.Office,
                Comments = entity.Comments,
                //ContactType = entity.ContactType,
                PromotionalEmail = entity.PromotionalEmail,
                CustomerId = entity.CustomerId,
                Brand = GetContactBrand(entity.CuContactBrands),
                Department = GetContactDepartment(entity.CuContactDepartments),
                Service = GetContactService(entity.CuContactServices),
                ReportToName = entity.ReportToNavigation?.ContactName
            };
        }

        public string GetContactBrand(IEnumerable<CuContactBrand> entity)
        {
            if (entity == null)
                return null;

            var brandList = entity.Where(x => x.Active).ToList();
            var brand = "";
            if (brandList != null)
            {
                var i = 0;
                foreach (var item in brandList)
                {
                    if (i < brandList.Count - 1)
                        brand = brand + item.Brand.Name + ", ";
                    else
                        brand = brand + item.Brand.Name;
                    i++;
                }
            }
            return brand;
        }

        public string GetContactDepartment(IEnumerable<CuContactDepartment> entity)
        {
            var departmentList = entity.Where(x => x.Active).ToList();
            var department = "";
            if (departmentList != null)
            {
                var i = 0;
                foreach (var item in departmentList)
                {
                    if (i < departmentList.Count - 1)
                        department = department + item.Department.Name + ", ";
                    else
                        department = department + item.Department.Name;
                    i++;
                }
            }
            return department;
        }

        public string GetContactService(IEnumerable<CuContactService> entity)
        {
            var serviceList = entity.Where(x => x.Active).ToList();
            var service = "";
            if (serviceList != null)
            {
                var i = 0;
                foreach (var item in serviceList)
                {
                    if (i < serviceList.Count - 1)
                        service = service + item.Service.Name + ", ";
                    else
                        service = service + item.Service.Name;
                    i++;
                }
            }
            return service;
        }


        public List<ContactType> GetContactTypes(IEnumerable<CuContactType> contactTypes)
        {
            if (contactTypes == null)
                return null;

            List<ContactType> lstContactType = new List<ContactType>();

            foreach (var item in contactTypes)
            {
                var contactType = new ContactType
                {
                    id = item.Id,
                    Type = item.ContactType
                };
                lstContactType.Add(contactType);
            }

            return lstContactType;
        }

        public CustomerContactDetails GetCustomerContactDetails(CuContact entity, List<CustomerAddressData> customerAddressDataList, IEnumerable<CuContactType> cuContactType,
            List<int> contactSisterCompanyIds)
        {
            if (entity == null)
                return null;

            var item = new CustomerContactDetails
            {
                Id = entity.Id,
                Name = entity.ContactName,
                LastName = entity.LastName,
                JobTitle = entity.JobTitle,
                Email = entity.Email,
                Mobile = entity.Mobile,
                Phone = entity.Phone,
                Fax = entity.Fax,
                Others = entity.Others,
                Office = entity.Office,
                CustomerID = entity.CustomerId,
                Comments = entity.Comments,
                PromotionalEmail = entity.PromotionalEmail,
                ContactTypes = entity.CuCustomerContactTypes?.Select(x => x.ContactTypeId).ToArray(),
                CustomerAddressList = customerAddressDataList,
                ContactTypeList = GetContactTypes(cuContactType),
                ContactBrandList = entity.CuContactBrands?.Where(x => x.Active).Select(x => x.BrandId).ToArray(),
                ContactDepartmentList = entity.CuContactDepartments?.Where(x => x.Active).Select(x => x.DepartmentId).ToArray(),
                ContactServiceList = entity.CuContactServices?.Where(x => x.Active).Select(x => x.ServiceId).ToArray(),
                ApiEntityIds = entity.CuContactEntityMaps?.Select(x => x.EntityId).ToArray(),
                EntityServiceIds = entity.CuContactEntityServiceMaps?.
                Select(x => new EntityService()
                {
                    Id = x.Id,
                    ServiceId = x.ServiceId,
                    EntityId = x.EntityId,
                    Name = x.Entity.Name + "(" + x.Service.Name + ")"
                }).ToArray(),
                PrimaryEntity = entity.PrimaryEntity.GetValueOrDefault(),
                ReportTo = entity.ReportTo.GetValueOrDefault(),
                ContactSisterCompanyIds = contactSisterCompanyIds
            };


            return item;

        }

        public CustomerContactDetails MapSaveCustomerContact(SaveCustomerContactDetails request, int customerId, int? contactId)
        {
            if (request == null)
                return null;

            CustomerContactDetails customerContactDetails = new CustomerContactDetails();
            if (contactId != null)
                customerContactDetails.Id = contactId.Value;
            customerContactDetails.CustomerID = customerId;
            customerContactDetails.Name = request.Name;
            customerContactDetails.LastName = request.LastName;
            customerContactDetails.JobTitle = request.JobTitle;
            customerContactDetails.Email = request.Email;
            customerContactDetails.Mobile = request.Mobile;
            customerContactDetails.Phone = request.Phone;
            customerContactDetails.Fax = request.Fax;
            customerContactDetails.Others = request.Others;
            if (request.Office != 0)
                customerContactDetails.Office = request.Office;
            else
                customerContactDetails.Office = 1;
            customerContactDetails.Comments = request.Comments;
            customerContactDetails.PromotionalEmail = request.PromotionalEmail;
            if (request.ContactTypes != null && (request.ContactTypes.Contains(0) || request.ContactTypes.Count() == 0))
            {
                request.ContactTypes.Clear();
                request.ContactTypes.Add(1);
            }

            customerContactDetails.ContactTypes = request.ContactTypes;

            return customerContactDetails;
        }

        public CustomerContactDetails MapSaveZohoCustomerContact(SaveZohoCrmCustomerContactDetails request, int customerId,
                                                                            List<ZohoCustomerAddress> customerAddressList)
        {
            if (request == null)
                return null;

            CustomerContactDetails customerContactDetails = new CustomerContactDetails();
            customerContactDetails.CustomerID = customerId;
            customerContactDetails.Name = request.Name;
            customerContactDetails.JobTitle = request.JobTitle;
            customerContactDetails.Email = request.Email;
            customerContactDetails.Mobile = request.Mobile;

            customerContactDetails.Phone = !String.IsNullOrEmpty(request.Phone) ? request.Phone : "";

            customerContactDetails.Fax = request.Fax;

            //get the head office address data
            var homeAddress = customerAddressList.FirstOrDefault(x => x.AddressType ==
                                                  (int)ZohoCustomerAddressTypeEnum.HeadOffice);
            if (homeAddress != null)
                customerContactDetails.Office = homeAddress.Id;

            customerContactDetails.Comments = request.Comments;
            customerContactDetails.PromotionalEmail = request.PromotionalEmail;

            //add operations as default contact type
            customerContactDetails.ContactTypes = new List<int>() { (int)ZohoCustomerContactType.Operations };

            return customerContactDetails;
        }

        public CuContact MapCustomerContactEntity(CustomerContactDetails request, int userID, int entityId)
        {
            if (request == null)
                return null;

            var customerContact = new CuContact
            {
                Id = request.Id,
                ZohoCustomerId = request.ZohoCustomerId,
                ZohoContactId = request.ZohoContactId,
                ContactName = request.Name?.Trim(),
                LastName = request.LastName?.Trim(),
                Fax = request.Fax?.Trim(),
                JobTitle = request.JobTitle?.Trim(),
                Email = request.Email?.Trim(),
                Mobile = request.Mobile?.Trim(),
                Phone = request.Phone?.Trim(),
                Others = request.Others?.Trim(),
                CustomerId = request.CustomerID,
                Office = request.Office,
                Comments = request.Comments?.Trim(),
                PromotionalEmail = request.PromotionalEmail,
                CreatedBy = userID,
                CreatedOn = DateTime.Now,
                PrimaryEntity = request.PrimaryEntity
            };

            if (request.ContactTypes != null)
            {
                customerContact.CuCustomerContactTypes = new HashSet<CuCustomerContactType>();
                foreach (var item in request.ContactTypes)
                {
                    customerContact.CuCustomerContactTypes.Add(new CuCustomerContactType
                    {
                        ContactTypeId = item,
                    });
                }
            }

            if (request.ContactBrandList != null)
            {
                customerContact.CuContactBrands = new HashSet<CuContactBrand>();
                foreach (var item in request.ContactBrandList)
                {
                    customerContact.CuContactBrands.Add(new CuContactBrand
                    {
                        BrandId = item,
                        Active = true,
                        CreatedBy = userID,
                        CreatedOn = DateTime.Now
                    });
                }
            }

            //add the contact sister company ids
            if (request.ContactSisterCompanyIds != null && request.ContactSisterCompanyIds.Any())
            {
                foreach (var customerId in request.ContactSisterCompanyIds)
                {
                    customerContact.CuContactSisterCompanies.Add(new CuContactSisterCompany()
                    {
                        CreatedBy = userID,
                        CreatedOn = DateTime.Now,
                        Active = true,
                        SisterCompanyId = customerId,
                        EntityId = entityId
                    });
                }

            }

            if (request.ContactDepartmentList != null)
            {
                customerContact.CuContactDepartments = new HashSet<CuContactDepartment>();
                foreach (var item in request.ContactDepartmentList)
                {
                    customerContact.CuContactDepartments.Add(new CuContactDepartment
                    {
                        DepartmentId = item,
                        Active = true,
                        CreatedBy = userID,
                        CreatedOn = DateTime.Now
                    });
                }
            }

            if (request.ContactServiceList != null)
            {
                customerContact.CuContactServices = new HashSet<CuContactService>();
                foreach (var item in request.ContactServiceList)
                {
                    customerContact.CuContactServices.Add(new CuContactService
                    {
                        ServiceId = item,
                        Active = true,
                        CreatedBy = userID,
                        CreatedOn = DateTime.Now
                    });
                }
            }
            // Add api entity ids for customer contacts
            if (request.ApiEntityIds != null)
            {
                customerContact.CuContactEntityMaps = new HashSet<CuContactEntityMap>();
                foreach (var item in request.ApiEntityIds)
                {
                    customerContact.CuContactEntityMaps.Add(new CuContactEntityMap
                    {
                        EntityId = item
                    });
                }
            }

            //configure api entity and service for customer contacts
            if (request.EntityServiceIds != null && request.EntityServiceIds.Any())
            {
                customerContact.CuContactEntityServiceMaps = new List<CuContactEntityServiceMap>();
                foreach (var data in request.EntityServiceIds)
                {
                    var cuContactServiceEntity = new CuContactEntityServiceMap
                    {
                        EntityId = data.EntityId,
                        ServiceId = data.ServiceId
                    };
                    customerContact.CuContactEntityServiceMaps.Add(cuContactServiceEntity);
                }
            }
            else if (request.ApiEntityIds != null && request.ContactServiceList != null)
            {
                foreach (var entity in request.ApiEntityIds)
                {
                    foreach (var serviceId in request.ContactServiceList)
                    {
                        customerContact.CuContactEntityServiceMaps.Add(new CuContactEntityServiceMap
                        {
                            EntityId = entity,
                            ServiceId = serviceId
                        });
                    }
                }
            }

            return customerContact;
        }

        public void UpdateCustomerContactEnity(CuContact entity, CustomerContactDetails request, int userID)
        {
            entity.Id = request.Id;
            entity.ContactName = request.Name?.Trim();
            entity.LastName = request.LastName?.Trim();
            entity.JobTitle = request.JobTitle?.Trim();
            entity.Email = request.Email?.Trim();
            entity.Mobile = request.Mobile?.Trim();
            entity.Phone = request.Phone?.Trim();
            entity.Fax = request.Fax?.Trim();
            entity.Others = request.Others?.Trim();
            entity.Office = request.Office;
            entity.Comments = request.Comments?.Trim();
            entity.PromotionalEmail = request.PromotionalEmail;
            entity.UpdatedBy = userID;
            entity.UpdatedOn = DateTime.Now;
            entity.PrimaryEntity = request.PrimaryEntity;
            entity.ReportTo = request.ReportTo;
            if (entity.CuCustomerContactTypes == null)
                entity.CuCustomerContactTypes = new HashSet<CuCustomerContactType>();

            entity.CuCustomerContactTypes.Clear();

            foreach (var item in request.ContactTypes)
            {
                entity.CuCustomerContactTypes.Add(new CuCustomerContactType
                {
                    ContactTypeId = item
                });
            }
            if (entity.CuContactBrands == null)
                entity.CuContactBrands = new HashSet<CuContactBrand>();

            // find the item not exist in current list and update active
            foreach (var item in entity.CuContactBrands.Where(x => !request.ContactBrandList.Contains(x.BrandId) && x.Active))
            {
                item.Active = false;
                item.DeletedBy = userID;
                item.DeletedOn = DateTime.Now;
            }

            if (request.ContactBrandList != null)
            {
                // find item not exist in entity and add new item
                foreach (var item in request.ContactBrandList.Where(x => !entity.CuContactBrands.Where(z => z.Active).Select(y => y.BrandId).Contains(x)))
                {
                    entity.CuContactBrands.Add(new CuContactBrand
                    {
                        BrandId = item,
                        Active = true,
                        CreatedBy = userID,
                        CreatedOn = DateTime.Now
                    });
                }
            }


            if (entity.CuContactDepartments == null)
                entity.CuContactDepartments = new HashSet<CuContactDepartment>();
            // find the item not exist in current list and update active
            foreach (var item in entity.CuContactDepartments.Where(x => !request.ContactDepartmentList.Contains(x.DepartmentId) && x.Active))
            {
                item.Active = false;
                item.DeletedBy = userID;
                item.DeletedOn = DateTime.Now;
            }

            if (request.ContactDepartmentList != null)
            {
                // find item not exist in entity and add new item
                foreach (var item in request.ContactDepartmentList.Where(x => !entity.CuContactDepartments.Where(z => z.Active).Select(y => y.DepartmentId).Contains(x)))
                {
                    entity.CuContactDepartments.Add(new CuContactDepartment
                    {
                        DepartmentId = item,
                        Active = true,
                        CreatedBy = userID,
                        CreatedOn = DateTime.Now
                    });
                }
            }


            if (entity.CuContactServices == null)
                entity.CuContactServices = new HashSet<CuContactService>();
            // find the item not exist in current list and update active
            foreach (var item in entity.CuContactServices.Where(x => !request.ContactServiceList.Contains(x.ServiceId) && x.Active))
            {
                item.Active = false;
                item.DeletedBy = userID;
                item.DeletedOn = DateTime.Now;
            }
            if (request.ContactServiceList != null)
            {
                // find item not exist in entity and add new item
                foreach (var item in request.ContactServiceList.Where(x => !entity.CuContactServices.Where(z => z.Active).Select(y => y.ServiceId).Contains(x)))
                {
                    entity.CuContactServices.Add(new CuContactService
                    {
                        ServiceId = item,
                        Active = true,
                        CreatedBy = userID,
                        CreatedOn = DateTime.Now
                    });
                }
            }

            // Add entity and Service mapping for customer contacts
            if (entity.CuContactEntityServiceMaps == null)
                entity.CuContactEntityServiceMaps = new HashSet<CuContactEntityServiceMap>();
            if (request.EntityServiceIds != null && request.EntityServiceIds.Any())
            {
                foreach (var item in request.EntityServiceIds)
                {
                    entity.CuContactEntityServiceMaps.Add(new CuContactEntityServiceMap
                    {
                        EntityId = item.EntityId,
                        ServiceId = item.ServiceId
                    });
                }
            }
            else if (request.ApiEntityIds != null && request.ContactServiceList != null)
            {
                foreach (var entityId in request.ApiEntityIds)
                {
                    foreach (var serviceId in request.ContactServiceList)
                    {
                        entity.CuContactEntityServiceMaps.Add(new CuContactEntityServiceMap
                        {
                            EntityId = entityId,
                            ServiceId = serviceId
                        });
                    }
                }
            }
        }

        public CustomerDepartments MapCustomerDepartmentEntity(CuDepartment entity)
        {
            if (entity == null)
                return null;
            return new CustomerDepartments
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code
            };
        }

        public EditCustomerServiceConfigData MapCustomerServiceConfigData(CuServiceType entity, string customerName)
        {
            if (entity == null)
                return null;

            var item = new EditCustomerServiceConfigData
            {
                Id = entity.Id,
                Service = entity.ServiceId,
                ServiceType = entity.ServiceTypeId,
                ProductCategory = entity.ProductCategoryId,
                PickType = entity.PickType,
                LevelPick1 = entity.LevelPick1,
                LevelPick2 = entity.LevelPick2,
                CriticalPick1 = entity.CriticalPick1,
                CriticalPick2 = entity.CriticalPick2,
                MajorTolerancePick1 = entity.MajorTolerancePick1,
                MajorTolerancePick2 = entity.MajorTolerancePick2,
                MinorTolerancePick1 = entity.MinorTolerancePick1,
                MinorTolerancePick2 = entity.MinorTolerancePick2,
                AllowAQLModification = entity.AllowAqlmodification,
                IgnoreAcceptanceLevel = entity.IgnoreAcceptanceLevel,
                DefectClassification = entity.DefectClassification,
                ReportUnit = entity.ReportUnit,
                CheckMeasurementPoints = entity.CheckMeasurementPoints,
                Active = entity.Active,
                CustomerID = entity.CustomerId,
                CustomerName = customerName,
                CustomServiceTypeName = entity.CustomServiceTypeName,
                CustomerRequirementIndex = entity.CustomerRequirementIndex,
                DpPoint = entity.DpPoint
                //ContactType=entity.ContactType,
            };
            return item;

        }

        public static CuServiceType MapCustomerServiceConfigEntity(EditCustomerServiceConfigData request, int userID, int _entityId)
        {
            if (request == null)
                return null;

            var customer = new CuServiceType
            {
                Id = request.Id,
                ServiceId = request.Service,
                ServiceTypeId = request.ServiceType,
                PickType = request.PickType,
                LevelPick1 = request.LevelPick1,
                LevelPick2 = request.LevelPick2,
                CriticalPick1 = request.CriticalPick1,
                CriticalPick2 = request.CriticalPick2,
                MajorTolerancePick1 = request.MajorTolerancePick1,
                MajorTolerancePick2 = request.MajorTolerancePick2,
                MinorTolerancePick1 = request.MinorTolerancePick1,
                MinorTolerancePick2 = request.MinorTolerancePick2,
                AllowAqlmodification = request.AllowAQLModification,
                IgnoreAcceptanceLevel = request.IgnoreAcceptanceLevel,
                DefectClassification = request.DefectClassification,
                CheckMeasurementPoints = request.CheckMeasurementPoints,
                ReportUnit = request.ReportUnit,
                ProductCategoryId = request.ProductCategory,
                CustomerId = request.CustomerID,
                Active = request.Active,
                CreatedBy = userID,
                CreatedOn = DateTime.Now,
                CustomServiceTypeName = request.CustomServiceTypeName,
                CustomerRequirementIndex = request.CustomerRequirementIndex,
                EntityId = _entityId,
                DpPoint = request.DpPoint
            };

            return customer;
        }

        public void UpdateCustomerServiceConfigEntity(CuServiceType entity, EditCustomerServiceConfigData request, int userID)
        {

            entity.Id = request.Id;
            entity.ServiceId = request.Service;
            entity.ServiceTypeId = request.ServiceType;
            entity.PickType = request.PickType;
            entity.LevelPick1 = request.LevelPick1;
            entity.LevelPick2 = request.LevelPick2;
            entity.CriticalPick1 = request.CriticalPick1;
            entity.CriticalPick2 = request.CriticalPick2;
            entity.MajorTolerancePick1 = request.MajorTolerancePick1;
            entity.MajorTolerancePick2 = request.MajorTolerancePick2;
            entity.MinorTolerancePick1 = request.MinorTolerancePick1;
            entity.MinorTolerancePick2 = request.MinorTolerancePick2;
            entity.AllowAqlmodification = request.AllowAQLModification;
            entity.IgnoreAcceptanceLevel = request.IgnoreAcceptanceLevel;
            entity.DefectClassification = request.DefectClassification;
            entity.CheckMeasurementPoints = request.CheckMeasurementPoints;
            entity.ReportUnit = request.ReportUnit;
            entity.ProductCategoryId = request.ProductCategory;
            entity.Active = request.Active;
            entity.UpdatedBy = userID;
            entity.UpdatedOn = DateTime.Now;
            entity.CustomServiceTypeName = request.CustomServiceTypeName;
            entity.CustomerRequirementIndex = request.CustomerRequirementIndex;
            entity.DpPoint = request.DpPoint;
        }

        //Export Customer Products
        public CustomerProductExportData GetCustomerProductExportData(CustomerProductRepoExportData entity)
        {
            if (entity == null)
                return null;
            return new CustomerProductExportData
            {

                CustomerName = entity.CustomerName,
                ProductId = entity.ProductId,
                ProductDescription = entity.ProductDescription,
                Barcode = entity.Barcode,
                FactoryReference = entity.FactoryReference,
                Remarks = entity.Remarks,
                ProductCategory = entity.ProductCategory,
                ProductSubCategory = entity.ProductSubCategory,
                ProductCategorySub2 = entity.ProductCategorySub2,
                ProductCategorySub3 = entity.ProductCategorySub3,
                IsNewProduct = entity.IsNewProduct.GetValueOrDefault() == false ? "No" : "Yes",
                SampleSize8h = entity.SampleSize8h,
                TimePreparation = entity.TimePreparation
            };
        }


        public void UpdateCustomerProductEntity(CuProduct entity, CustomerProduct request)
        {
            entity.Id = request.Id;
            entity.ProductId = request.ProductId.Trim();
            entity.ProductDescription = request.ProductDescription?.RemoveExtraSpace();
            entity.Remarks = request.Remarks;
            entity.CustomerId = request.CustomerId;
            entity.ProductCategory = request.ProductCategory;
            entity.ProductSubCategory = request.ProductSubCategory;
            entity.ProductCategorySub2 = request.ProductCategorySub2;
            entity.Barcode = request.Barcode;
            entity.FactoryReference = request.FactoryReference;
            entity.IsNewProduct = request.isNewProduct;
            entity.ProductCategorySub3 = request.ProductCategorySub3;
            entity.TimePreparation = request.TimePreparation;
            entity.SampleSize8h = request.SampleSize8h;
            entity.TpAdjustmentReason = request.TpAdjustmentReason;
            entity.Unit = request.Unit;
            entity.TechnicalComments = request.TechnicalComments;

            if (request.ProductCategorySub3 == null)
            {
                entity.SampleSize8h = null;
                entity.TimePreparation = null;
            }
        }

        public CustomerBuyers MapCustomerBuyerEntity(CuBuyer entity)
        {
            if (entity == null)
                return null;
            return new CustomerBuyers
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                apiServiceIds = entity.CuBuyerApiServices.Where(x => x.Active).Select(x => x.ServiceId).ToList()
            };
        }

        public CuContact UpdateCuContactBrandEntity(CuContact entity, int userID)
        {
            if (entity != null)
            {
                // save contact brand
                if (entity.CuContactBrands != null)
                {
                    foreach (var item in entity.CuContactBrands)
                    {
                        item.Active = false;
                        item.DeletedBy = userID;
                        item.DeletedOn = DateTime.Now;
                    }
                }

                // save contact department
                if (entity.CuContactDepartments != null)
                {
                    foreach (var item in entity.CuContactDepartments)
                    {
                        item.Active = false;
                        item.DeletedBy = userID;
                        item.DeletedOn = DateTime.Now;
                    }
                }

                // save contact service
                if (entity.CuContactServices != null)
                {
                    foreach (var item in entity.CuContactServices)
                    {
                        item.Active = false;
                        item.DeletedBy = userID;
                        item.DeletedOn = DateTime.Now;
                    }
                }
            }

            return entity;
        }

        public CustomerDetails MapZohoCustomerEntity(SaveCustomerCrmRequest request, int? customerId, int? companyId, string glcode = null, DateTime? startdate = null)
        {
            if (request == null)
                return null;
            //if customer id is null means update operation .
            var customer = new CustomerDetails
            {
                Id = customerId != null ? customerId.Value : 0,
                ZohoCustomerId = request.ZohoCustomerId,
                Name = request.Name.Trim(),
                Fax = request.Fax?.Trim(),
                GlCode = customerId == null ? request.GlCode?.Trim() : glcode,
                Email = request.Email?.Trim(),
                Phone = request.Phone?.Trim(),
                Website = request.Website?.Trim(),
                Comments = request.Comments?.Trim(),
                InvoiceType = (int)ZohoInvoiceTypeEnum.MonthlyInvoice,
                BusinessCountry = new List<int>() { (int)ZohoCountryEnum.China },
                ApiServiceIds = new List<int?>() { (int)Entities.Enums.Service.InspectionId },
                CompanyId = companyId
            };

            if (customerId == null)
            {
                DateTime startDate;
                DateTime.TryParseExact(request.StartDate, StandardDateFormat, CultureInfo.InvariantCulture,
                                                                                        DateTimeStyles.None, out startDate);
                customer.StartDate = startDate.GetCustomDate();
            }
            else
            {
                customer.StartDate = startdate.GetCustomDate();
            }

            return customer;
        }

        public CustomerContactData MapCustomerContactData(CuContact entity)
        {
            CustomerContactData contactData = new CustomerContactData();
            if (entity == null)
                return null;
            return new CustomerContactData
            {
                Id = entity.Id,
                Name = entity.ContactName,
                JobTitle = entity.JobTitle,
                Email = entity.Email,
                Mobile = entity.Mobile,
                Phone = entity.Phone,
                Fax = entity.Fax,
                Others = entity.Others,
                Office = entity.Office,
                Comments = entity.Comments,
                CustomerID = entity.CustomerId,
                ZohoCustomerId = entity.ZohoCustomerId != null ? Convert.ToString(entity.ZohoCustomerId) : "",
                ZohoContactId = entity.ZohoContactId != null ? Convert.ToString(entity.ZohoContactId) : "",
                //ContactType = entity.ContactType,
                PromotionalEmail = entity.PromotionalEmail
            };
        }

        public DataCommon GetCustomerData(CuCustomer entity)
        {
            if (entity == null)
                return null;

            return new DataCommon
            {
                Key = entity.Id.ToString(),
                Value = entity.CustomerName
            };
        }

        public CuCustomer MapCustomersEntity(SaveEaqfCustomerRequest request, int entityId, int userId)
        {
            if (request == null)
                return null;

            var customer = new CuCustomer
            {
                CustomerName = request.CompanyName?.Trim()?.ToUpper(),
                Email = request.Email?.Trim(),
                Website = request.Website?.Trim(),
                Phone = request.Phone?.Trim(),
                InvoiceType = (int)INVInvoiceType.PreInvoice,
                GlCode = Guid.NewGuid().ToString(),
                CompanyId = entityId,
                StartDate = DateTime.Now,
                CreatedBy = userId,
                CreatedOn = DateTime.Now,
                IsEaqf = true,
                Active = true
            };

            customer.CuEntities.Add(new CuEntity()
            {
                EntityId = entityId,
                Active = true,
                CreatedBy = userId,
                CreatedOn = DateTime.Now
            });

            //add the api services to customer
            customer.CuApiServices.Add(new CuApiService()
            {
                ServiceId = (int)APIServiceEnum.Inspection,
                Active = true,
                CreatedBy = userId,
                CreatedOn = DateTime.Now
            });

            return customer;
        }

        public CustomerGetDetails GetEAQFCustomerMap(CuCustomer customer, List<CuContactData> customerContact, List<GetEaqfCustomerAddressData> customerAddress)
        {
            var customerContactInfo = customerContact.FirstOrDefault(y => y.CustomerId == customer.Id);
            return new CustomerGetDetails()
            {
                Id = customer.Id,
                Email = customer.Email,
                CompanyName = customer.CustomerName,
                Country = customerAddress?.FirstOrDefault(y => y.CustomerId == customer.Id)?.Country,
                FirstName = customerContactInfo?.FirstName,
                LastName = customerContactInfo?.LastName,
                Mobile = customerContactInfo?.Mobile,
                UserId = customerContactInfo?.UserId
                // AddressList = customerAddress
            };
        }
    }
}
