using DTO.Common;
using DTO.CommonClass;
using DTO.HumanResource;
using DTO.Location;
using DTO.Supplier;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BI.Maps
{
    public class SupplierMap : ApiCommonData
    {
        public SupplierType GetSuppType(SuType entity)
        {
            if (entity == null)
                return null;

            return new SupplierType
            {
                Id = entity.Id,
                Name = entity.TypeTransId.GetTranslation(entity.Type)
            };
        }

        public SupplierItem MapSupplierItem(SuSupplier entity, int level)
        {
            if (entity == null)
                return null;

            var address = entity.SuAddresses?.FirstOrDefault();

            var item = new SupplierItem
            {
                Id = entity.Id,
                CountryName = address?.Country?.CountryName,
                RegionName = address?.Region?.ProvinceName,
                RegionalLanguageName = entity.LocalName,
                CityName = address?.City?.CityName,
                CountyName = address?.County?.CountyName,
                TownName = address?.Town?.TownName,
                Name = entity.SupplierName,
                TypeId = entity.TypeId == null ? 0 : entity.TypeId.Value,
                TypeName = entity.Type?.Type,
                Phone = entity?.Phone,
                Email = entity?.Email,
                Way = address?.Address

            };

            if (level == (int)Entities.Enums.Supplier_Level.Parent)
            {
                if (entity.TypeId != (int)Supplier_Type.Factory && entity.SuSupplierFactorySuppliers != null &&
                    entity.SuSupplierFactorySuppliers.Any(x => x.Parent != null && x.Parent.Active))

                    item.List = entity.SuSupplierFactorySuppliers?.Where(x => x.Parent != null && x.Parent.Active).Select(x => MapSupplierItem(x.Parent, (int)Entities.Enums.Supplier_Level.Child));

                if (entity.TypeId == (int)Supplier_Type.Factory && entity.SuSupplierFactoryParents != null && entity.SuSupplierFactoryParents.Any(x => x.Supplier != null && x.Supplier.Active))
                    item.List = entity.SuSupplierFactoryParents?.Where(x => x.Supplier != null && x.Supplier.Active).Select(x => MapSupplierItem(x.Supplier, (int)Entities.Enums.Supplier_Level.Child));
            }

            item.CanBeDeleted = IfSupplierCanBeDeleted(entity, item.List);
            return item;
        }

        public Level Getlevel(SuLevel entity)
        {
            if (entity == null)
                return null;

            return new Level
            {
                Id = entity.Id,
                Name = entity.Level
            };

        }

        public OwnerShip GetOwner(SuOwnlerShip entity)
        {
            if (entity == null)
                return null;

            return new OwnerShip
            {
                Id = entity.Id,
                Name = entity.NameTranId.GetTranslation(entity.Name)
            };
        }

        public SupplierDetails GetSupplierDetails(SuSupplier entity, List<SupplierGradeRepo> grades = null)
        {
            if (entity == null)
                return null;

            CustomerMap customerMap = new CustomerMap();

            var item = new SupplierDetails
            {
                Id = entity.Id,
                Comment = entity.Comments,
                ContactPersonName = entity.ContactPerson,
                DailyProduction = entity.DailyProduction,
                LevelId = entity.LevelId,
                Email = entity.Email,
                GlCode = entity.GlCode,
                LegalName = entity.LegalName,
                LocLanguageName = entity.LocalName,
                Mobile = entity.Mobile,
                Name = entity.SupplierName,
                OwnerId = entity.OwnerShipId,
                Fax = entity.Fax,
                Phone = entity.Phone,
                TotalStaff = entity.TotalStaff,
                TypeId = entity.TypeId,
                WebSite = entity.Website,
                AddressList = entity.SuAddresses.Select(GetAddress),
                CustomerList = entity.SuSupplierCustomers.Where(x => x.Customer.Active.HasValue && x.Customer.Active.Value).Select(x => GetSupplierMappedCustomerItem(x.Customer, x)),
                SupplierContactList = entity.SuContacts.Where(x => x.Active != null && x.Active.Value).Select(GetSupplierContact),
                IsNewSupplier = false,
                CreditTerm = entity.CreditTermId,
                Status = entity.StatusId.Value,
                VatNo = entity.Vatno,
                ApiServiceIds = entity.SuApiServices.Where(x => x.Active.HasValue && x.Active.Value).Select(x => x.ServiceId).ToArray(),
                SupplierEntityIds = entity.SuEntities.Where(x => x.Active == true).Select(x => x.EntityId).ToList(),
                CompanyId = entity.CompanyId.GetValueOrDefault(),

            };

            if (grades != null)
            {
                item.GradeList = grades.Select(x => new SupplierGrade()
                {
                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    CustomerName = x.CustomerName,
                    Level = !string.IsNullOrEmpty(x.CustomName) ? x.CustomName : x.Level,
                    LevelId = x.LevelId,
                    PeriodFrom = Static_Data_Common.GetCustomDate(x.PeriodFrom),
                    PeriodTo = Static_Data_Common.GetCustomDate(x.PeriodTo)
                }).ToList();
            }

            if (entity.SuSupplierFactoryParents != null && entity.SuSupplierFactoryParents.Any(x => x.Supplier != null && x.Supplier.Active))
                item.SupplierParentList = entity.SuSupplierFactoryParents.Where(x => x.Supplier != null && x.Supplier.Active).Select(x => MapSupplierItem(x.Supplier, 1));

            return item;

        }

        /// <summary>
        /// Get the customer item mapped to the supplier
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="suCustomer"></param>
        /// <returns></returns>
        public SupplierMappedCustomer GetSupplierMappedCustomerItem(CuCustomer entity, SuSupplierCustomer suCustomer)
        {
            if (entity == null)
                return null;

            return new SupplierMappedCustomer
            {
                Id = entity.Id,
                Name = entity.CustomerName ?? "",
                Code = suCustomer.Code,
                CreditTerm = suCustomer.CreditTerm,
                IsStatisticsVisibility = suCustomer.IsStatisticsVisibility.GetValueOrDefault()
            };
        }


        public suppliercontact GetSupplierContact(SuContact entity)
        {
            if (entity == null)
                return null;
            CustomerMap customerMap = new CustomerMap();
            return new suppliercontact
            {
                ContactId = entity.Id,
                ContactName = entity.ContactName,
                Comment = entity.Comment,
                ContactEmail = entity.Mail,
                Fax = entity.Fax,
                JobTitle = entity.JobTitle,
                Mobile = entity.Mobile,
                Phone = entity.Phone,
                CustomerList = entity.SuSupplierCustomerContacts.Where(x => x.Customer != null && x.Customer.Active.HasValue && x.Customer.Active.Value).Select(x => customerMap.GetCustomerItem(x.Customer, "")),
                ContactAPIServiceIds = entity.SuContactApiServices.Where(x => x.Active).Select(x => x.ServiceId).ToList(),
                ApiEntityIds = entity.SuContactEntityMaps.Select(x => x.EntityId).ToList(),
                EntityServiceIds = entity.SuContactEntityServiceMaps?.
                                        Select(x => new EntityService()
                                        {
                                            Id = x.Id,
                                            ServiceId = x.ServiceId,
                                            EntityId = x.EntityId,
                                            Name = x.Entity.Name + "(" + x.Service.Name + ")"
                                        }).ToArray(),
                PrimaryEntity = entity.PrimaryEntity.GetValueOrDefault()
            };
        }


        public Address GetAddress(SuAddress entity)
        {
            if (entity == null)
                return null;

            return new Address
            {
                Id = entity.Id,
                CityId = entity.CityId,
                CountryId = entity.CountryId,
                RegionId = entity.RegionId,
                ZipCode = entity.ZipCode,
                Way = entity.Address,
                AddressTypeId = entity.AddressTypeId == null ? 0 : entity.AddressTypeId.Value,
                Longitude = entity.Longitude,
                Latitude = entity.Latitude,
                LocalLanguage = entity.LocalLanguage,
                CountyId = entity.CountyId == null ? 0 : entity.CountyId.Value,
                TownId = entity.TownId == null ? 0 : entity.TownId.Value
            };

        }

        public AddressType GetAddressType(SuAddressType entity)
        {
            if (entity == null)
                return null;

            return new AddressType
            {
                Id = entity.Id,
                Code = entity.AddressTypeFlag,
                Name = entity.TranslationId.GetTranslation(entity.AddressType)
            };
        }

        public CreditTerm GetCreditTerm(SuCreditTerm entity)
        {
            if (entity == null)
                return null;

            return new CreditTerm
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public Status GetStatus(SuStatus entity)
        {
            if (entity == null)
                return null;

            return new Status
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        private bool IfSupplierCanBeDeleted(SuSupplier entity, IEnumerable<SupplierItem> list)
        {
            if (entity.InspTransactionSuppliers.Count > 0 || entity.AudTransactionSuppliers.Count > 0 || entity.CuPoSuppliers.Any(x => x.Active.HasValue && x.Active.Value) ||
                    entity.InspTransactionFactories.Count > 0 || entity.AudTransactionFactories.Count > 0 || entity.CuPoFactories.Any(x => x.Active.HasValue && x.Active.Value)
                  || (entity.SuSupplierFactorySuppliers.Count > 0 && list != null))

                return false;

            return true;
        }

        private bool IfSupplierItemCanBeDeleted(SupplierInvolvedData entity)
        {
            if (entity == null)
            {
                return false;
            }

            if (entity.InspectionFactoryAvailable || entity.InspectionSupplierAvailable || entity.PurchaseOrderFactoryAvailable ||
                    entity.PurchaseOrderSupplierAvailable || entity.AuditFactoryAvailable ||
                    entity.AuditSupplierAvailable
                  || (entity.SupplierFactoryMapAvailable && entity.SupplierParentAvailable))

                return false;

            return true;
        }


        public SupplierSearchItem MapSupplierSearchItem(SupplierSearchItemRepo entity,
            IEnumerable<SupplierAddressData> addressList,
            IEnumerable<SupplierInvolvedData> involvedItems)
        {
            if (entity == null)
                return null;

            var address = addressList != null ? addressList.Where(x => x.SupplierId == entity.Id)?.FirstOrDefault() : null;

            var involvedItem = involvedItems.Where(x => x.SupplierId == entity.Id)?.FirstOrDefault();

            var item = new SupplierSearchItem
            {
                Id = entity.Id,
                CountryName = entity?.CountryName ?? address?.CountryName,
                RegionName = entity?.RegionName ?? address?.RegionName,
                RegionalLanguageName = entity?.LocalName,
                CityName = entity?.CityName ?? address?.CityName,
                CountyName = entity?.CountyName ?? address?.CountyName,
                TownName = entity?.TownName ?? address?.TownName,
                Name = entity.Name,
                TypeId = entity.TypeId == null ? 0 : entity.TypeId.Value,
                TypeName = entity?.TypeName
            };

            item.CanBeDeleted = IfSupplierItemCanBeDeleted(involvedItem);

            return item;
        }

        //map the supplier details for export
        public SupplierExportItem MapSupplierSummaryExport(SupplierAddressData address, IEnumerable<SupplierAddressData> addressDetails,
            List<SupplierCustomerRepo> customerDetails, List<SupplierContactRepo> contactDetails, List<SupplierExportRepo> FactoryNameDetails,
            List<SupplierItemData> supplierDetails, List<SupplierServiceExportRepo> suApiServiceDetails)
        {
            var supplierDetail = supplierDetails.FirstOrDefault(x => x.SupplierId == address.SupplierId);

            var customerSupplierDetails = customerDetails.Where(x => x.SupplierId == address.SupplierId).ToList();

            var contactSupplierDetails = contactDetails.Where(x => x.SupplierId == address.SupplierId).ToList();

            List<SupplierExportRepo> supplierNameDetailList = null;

            if (supplierDetail.TypeId == (int)Supplier_Type.Factory)
            {
                supplierNameDetailList = FactoryNameDetails.Where(x => x.FactoryId == address.SupplierId).ToList();
            }

            List<string> CustomerNameCodeList = new List<string>();
            List<string> CustomerContactList = new List<string>();

            foreach (var item in customerSupplierDetails)
            {
                string code = string.Empty;

                if (!string.IsNullOrWhiteSpace(item.Code))
                {
                    code = "(" + item.Code + ")";
                }
                CustomerNameCodeList.Add(item.CustomerName + code);
            }

            foreach (var item in contactSupplierDetails)
            {
                string emailPhone = string.Empty;

                if (!string.IsNullOrWhiteSpace(item.PhoneNumber) && !string.IsNullOrWhiteSpace(item.Email))
                {
                    emailPhone = "(" + item.Email + "," + item.PhoneNumber + ")";

                }
                else if (!string.IsNullOrWhiteSpace(item.Email))
                {
                    emailPhone = "(" + item.Email + ")";

                }
                else if (!string.IsNullOrWhiteSpace(item.PhoneNumber))
                {
                    emailPhone = "(" + item.PhoneNumber + ")";

                }

                CustomerContactList.Add(item.ContactName + emailPhone);
            }
            return new SupplierExportItem()
            {
                Name = supplierDetail.Name ?? string.Empty,
                RegionalName = supplierDetail.RegionalName ?? string.Empty,
                Type = supplierDetail.Type ?? string.Empty,
                Service = string.Join(", ", suApiServiceDetails.Where(x => x.SupplierId == address.SupplierId).Select(x => x.ServiceName).ToList()),
                Status = supplierDetail.Status ?? string.Empty,
                Email = supplierDetail.Email ?? string.Empty,
                Phone = supplierDetail.Phone ?? string.Empty,
                Fax = supplierDetail.Fax ?? string.Empty,
                Website = supplierDetail.Website ?? string.Empty,
                ContactPerson = supplierDetail.ContactPerson ?? string.Empty,
                GLCode = supplierDetail.GLCode ?? string.Empty,
                Address = address.Address ?? string.Empty,
                RegionalLanguageAddress = address.RegionalLanguageName ?? string.Empty,
                Zip = address.ZipCode ?? string.Empty,
                Country = address.CountryName ?? string.Empty,
                Province = address.RegionName ?? string.Empty,
                City = address.CityName ?? string.Empty,
                County = address.CountyName ?? string.Empty,
                Town = address.TownName ?? string.Empty,
                Latitude = address.Latitude.GetValueOrDefault(),
                Longitude = address.Longitude.GetValueOrDefault(),
                OfficeType = address.OfficeType ?? string.Empty,
                CustomerWithCode = string.Join(", ", CustomerNameCodeList),
                Contact = string.Join(", ", CustomerContactList),
                Supplier = supplierDetail.TypeId == (int)Supplier_Type.Factory ? string.Join(", ", supplierNameDetailList.Select(x => x.SupplierName).ToList()) : string.Empty
            };
        }

        public CommonAddressDataSource MapSupplierAddress(SupplierAddress entity)
        {
            return new CommonAddressDataSource()
            {
                AddressType = entity.SupplierAddresstype,
                Id = entity.Id,
                Name = entity.Address
            };
        }

        public SupplierData GetExistSupplierDetails(SuSupplier entity)
        {
            if (entity == null)
                return null;

            var item = new SupplierData
            {
                Id = entity.Id,
                Type = entity.Type?.Type,
                TypeId = entity.TypeId,
                Name = entity.SupplierName,
                RegionalName = entity.LocalName,
                Address = entity.SuAddresses?.Select(x => x.Address).FirstOrDefault(),
                RegionalAddress = entity.SuAddresses?.Select(x => x.LocalLanguage).FirstOrDefault(),
                ContactEmail = entity.SuContacts?.Select(x => x.Mail).FirstOrDefault(),
                ContactPhone = entity.SuContacts?.Select(x => x.Phone).FirstOrDefault(),
                ContactName = entity.SuContacts?.Select(x => x.ContactName).FirstOrDefault(),
                SupplierEntityIds = entity.SuEntities.Where(x => x.Active == true).Select(x => x.EntityId)
            };
            return item;
        }
    }
}
