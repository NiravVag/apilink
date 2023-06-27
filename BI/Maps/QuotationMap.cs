using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.CustomerPriceCard;
using DTO.DynamicFields;
using DTO.Inspection;
using DTO.Quotation;
using DTO.References;
using DTO.Report;
using DTO.Supplier;
using DTO.User;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Maps
{
    public class QuotationMap : ApiCommonData
    {

        public BillingMethod GetBillingMethod(QuBillMethod entity)
        {
            if (entity == null)
                return null;

            return new BillingMethod
            {
                Id = entity.Id,
                Label = entity.Label
            };

        }

        public BillPaidBy GetBillPaidBy(QuPaidBy entity)
        {
            if (entity == null)
                return null;

            return new BillPaidBy
            {
                Id = entity.Id,
                Name = entity.Label
            };

        }

        public DataSource GetDataSource(CuCustomer entity, int serviceId)
        {
            if (entity == null)
                return null;

            return new DataSource
            {
                Id = entity.Id,
                Name = entity.CustomerName,
                IsForwardToManager = serviceId > 0 ? entity.CuServiceTypes.Any(y => y.Service != null && y.Service.CuCheckPoints.Any(z => z.CustomerId == entity.Id && z.Active && z.ServiceId == serviceId && z.CheckpointTypeId == (int)CheckPointTypeEnum.QuotationApproveByManager)) : false,
                InvoiceType = entity.InvoiceType.GetValueOrDefault()
            };
        }

        //Get the customer datasource list for quotation summary
        public DataSource GetCustomerDataSource(CuCustomer entity, int serviceId)
        {
            if (entity == null)
                return null;

            return new DataSource
            {
                Id = entity.Id,
                Name = entity.CustomerName
            };
        }

        public DataSource GetDataSource(SuSupplier entity)
        {
            if (entity == null)
                return null;

            return new DataSource
            {
                Id = entity.Id,
                Name = string.Concat(entity.SupplierName, !string.IsNullOrEmpty(entity.LocalName) ? "(" + entity.LocalName + ")" : ""),
                CountyId = entity.SuAddresses.Select(x => x.CountyId).FirstOrDefault(),
                CityId = entity.SuAddresses.Select(x => x.CityId).FirstOrDefault(),
                ProvinceId = entity.SuAddresses.Select(x => x.RegionId).FirstOrDefault()
            };
        }
        public DataSource GetDataSource(DTO.OfficeLocation.Office entity)
        {
            if (entity == null)
                return null;

            return new DataSource
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }

        public QuotationEntityContact GetContact(CuContact entity)
        {
            if (entity == null)
                return null;

            return new QuotationEntityContact
            {
                ContactId = entity.Id,
                ContactName = entity.ContactName,
                ContactEmail = entity.Email,
                ContactPhone = entity.Phone,
                EntityType = QuotationEntityType.Customer
            };
        }

        public QuotationEntityContact GetContact(HrStaff entity)
        {
            if (entity == null)
                return null;

            return new QuotationEntityContact
            {
                ContactId = entity.Id,
                ContactName = entity.PersonName,
                ContactEmail = entity.CompanyEmail,
                ContactPhone = entity.CompanyMobileNo,
                EntityType = QuotationEntityType.Internal,
            };
        }

        public QuotationEntityContact GetContact(SuContact entity, QuotationEntityType type)
        {
            if (entity == null)
                return null;

            return new QuotationEntityContact
            {
                ContactId = entity.Id,
                ContactName = entity.ContactName,
                ContactEmail = entity.Mail,
                ContactPhone = entity.Phone,
                EntityType = type
            };
        }

        public Order GetInspection(InspTransaction entity, IEnumerable<QuInspProduct> inspProductList, string productCategory, bool isEdit, List<BookingContainersData> containerList, List<InspectionBookingDFData> dFData)
        {
            if (entity == null)
                return null;

            List<DynamicFieldData> dynamicFields = null;
            if (dFData != null)
            {
                dynamicFields = dFData.Where(x => x.BookingNo == entity.Id)
                    .Select(x => new DynamicFieldData()
                    {
                        DynamicFieldValue = x.DFValue,
                        DynamicFieldName = x.DFName
                    }).ToList();
            }

            var order = new Order
            {
                Id = entity.Id,
                InspectionFrom = entity.ServiceDateFrom.ToString(StandardDateFormat),
                InspectionTo = entity.ServiceDateTo.ToString(StandardDateFormat),
                InternalBookingRemarks = entity?.InternalComments,
                QcBookingRemarks = entity?.QcbookingComments,
                orderCost = GetInspOrderCost(entity.QuQuotationInsps.FirstOrDefault()),
                ServiceTypeList = entity.InspTranServiceTypes.Where(x => x.Active).Select(x => new ServiceType
                {
                    Id = x.ServiceTypeId,
                    Name = x.ServiceType?.Name
                }),
                Office = entity.Office?.LocationName,
                LocationId = entity.OfficeId,
                StatusId = entity.StatusId,
                ProductCategory = productCategory,
                ProductList = GetProduct(entity, inspProductList),
                ContainerList = containerList,
                IsPicking = entity.IsPickingRequired,
                IsContainerServiceType = entity.InspTranServiceTypes.Any(x => x.Active && x.ServiceTypeId == (int)InspectionServiceTypeEnum.Container),
                PriceCategoryName = entity?.PriceCategory?.Name,
                StatusName = entity?.Status?.Status,
                BookingZipFileUrl = entity.InspTranFileAttachmentZips.FirstOrDefault(x => x.Active.HasValue && x.Active.Value)?.FileUrl,
                PaymentOption = entity.PaymentOptionsNavigation?.Name,
                DynamicFieldData = dynamicFields
            };

            return order;
        }

        public double CalWorkingHours(IEnumerable<QuotationBookingProductRepo> productList, double? _cri)
        {
            double calculatedWorkingHours = 0;

            foreach (var item in productList)
            {
                var list = item.CombineProductId > 0 ? productList.Where(x => x.CombineProductId == item.CombineProductId).ToList() : null;

                double bookingQty = item.BookingQty;
                double totalBookingQty = list?.Sum(x => x.BookingQty) ?? item.BookingQty;
                double sampleSize = list?.Sum(x => x.CombineAQL) ?? item.SampleSize;
                double timepreparation = item.TimePrepatation ?? 0;
                double customerRequirementIndex = _cri.GetValueOrDefault();
                double sampleSize8h = item.SampleSize8h ?? 0;

                //Task ALD 1623 for reference
                //Formula A(LS1/ LS * SS / S81 * 8 + Tp1 * CRI - 0.5)+(LS2 / LS * SS / S82 * 8 + Tp2 * CRI - 0.5) +…+(LSN / LS * SS / S8N * 8 + TpN * CRI - 0.5) + 0.5

                //e.g.1 LS - LotSize = BookingQty/ SS = Sample Size/ S8 = SampleSize8h from Cu Products/ Tp = TimePreparation from CuProducts

                //Single item in the report, LS1 = 1000, LS = 1000, SS = 80, S81 = 80, Tp1 = 1.75, CRI = 1.2
                //1000 / 1000 * 80 / 80 * 8 + 1.75 * 1.2 - 0.5 + 0.5 = 10.1 Hours

                //e.g.2

                //Two items combined in the report, LS1 = 1000, LS2 = 4000, LS = 5000, SS = 200, S81 = 80, S82 = 200, Tp1 = 1.75, Tp2 = 1.25, CRI = 0.8
                //(1000 / 5000 * 200 / 80 * 8 + 1.75 * 0.8 - 0.5) + (4000 / 5000 * 200 / 200 * 8 + 1.25 * 0.8 - 0.5) + 0.5 = 12.3 Hours

                calculatedWorkingHours = totalBookingQty > 0 && sampleSize8h > 0 ? calculatedWorkingHours + ((bookingQty / totalBookingQty) * (sampleSize / sampleSize8h) * 8 + (timepreparation * customerRequirementIndex) - 0.5) : calculatedWorkingHours;
            }

            var noOfReports = productList.Count(x => x.CombineProductId == 0 || x.CombineProductId == null) + productList.Where(x => x.CombineProductId > 0).Select(x => x.CombineProductId).Distinct().Count();

            calculatedWorkingHours = calculatedWorkingHours + (noOfReports * 0.5);

            return calculatedWorkingHours;
        }

        public OrderCost GetInspOrderCost(QuQuotationInsp entity)
        {
            if (entity == null)
                return new OrderCost();

            return new OrderCost
            {
                InspFees = entity.InspFees,
                NoOfManday = entity.NoOfManDay,
                TravelAir = entity.TravelAir,
                TravelHotel = entity.TravelHotel,
                TravelLand = entity.TravelLand,
                UnitPrice = entity.UnitPrice,
                TravelManday = entity.NoOfTravelManDay,
                TravelDistance = entity.TravelDistance,
                TravelTime = entity.TravelTime,
                CalculatedWorkingHours = entity.CalculatedWorkingHrs.GetValueOrDefault(),
                CalculatedManday = entity.CalculatedWorkingManDay.GetValueOrDefault(),
                BilledQtyType = entity.BilledQtyType,
                Quantity = entity.Quantity
            };
        }
        public OrderCost GetAuditOrderCost(QuQuotationAudit entity)
        {
            if (entity == null)
                return new OrderCost();
            return new OrderCost
            {
                InspFees = entity.InspFees,
                NoOfManday = entity.NoOfManDay,
                TravelAir = entity.TravelAir,
                TravelHotel = entity.TravelHotel,
                TravelLand = entity.TravelLand,
                UnitPrice = entity.UnitPrice,
                TravelManday = entity.NoOfTravelManDay,
                TravelDistance = entity.TravelDistance,
                TravelTime = entity.TravelTime
            };
        }
        public QuotationItem GetQuotationInspItem(QuotationItemRepo entity,
            Func<QuotationStatus?, IEnumerable<QuotationAbility>> funcGetAbilities,
            List<int> adeoCustomerIds,
            IEnumerable<QuotationInsp> quInspAudit,
            IEnumerable<QuInspProduct> quInspProduct,
            List<BookingBrandAccess> brandList,
            List<BookingDeptAccess> departmentList, List<SupplierCode> supplierCodes)
        {
            if (entity == null)
                return null;

            string BookingNoCusBookingNo = "";
            string bookingStatusCommaSeperated = "";
            List<string> serviceDateList = new List<string>();
            List<string> bookingStatusList = new List<string>();
            List<string> serviceNameList = new List<string>();
            List<string> BrandNameList = new List<string>();
            List<string> DepartmentNameList = new List<string>();
            bool? IsEAQF = false;

            List<BookingStatusColorList> bookingIdStatusList = new List<BookingStatusColorList>();

            //var additionalinfo = lstaddinfo.Any() ? lstaddinfo.Where(x => x.quotationid == entity.QuotationId).FirstOrDefault() : null;

            var isAdeoClientQuotation = adeoCustomerIds.Contains(entity.CustomerId) && entity.StatusId != (int)QuotationStatus.Canceled && entity.ServiceId == (int)Entities.Enums.Service.InspectionId ? true : false;

            var quInspItems = quInspAudit.Where(x => x.QuotationId == entity.QuotationId).ToList();
            var quInspProdItems = quInspProduct.Where(x => x.IdQuotation == entity.QuotationId).ToList();
            if (quInspItems != null && quInspItems.Any())
            {
                var lstbookingids = quInspItems.Select(x => x.BookingId).Distinct().ToList();
                var lstbookingidcusbookingnumber = quInspItems.Select(x => new
                {
                    x.CusBookingNo,
                    x.BookingId
                }
                ).Distinct().ToList();

                BookingNoCusBookingNo = string.Join(", ", lstbookingidcusbookingnumber.Select(x => "#" + x.BookingId +
                (!string.IsNullOrWhiteSpace(x.CusBookingNo) ? " / " + x.CusBookingNo : "")).ToList());

                lstbookingids.ForEach(element =>
                {
                    var CustomerBookingNo = lstbookingidcusbookingnumber.Where(x => x.BookingId == element).Select(x => x.CusBookingNo).FirstOrDefault();

                    //get inspection status name based on id
                    bookingStatusList.Add(quInspItems?.Where(x => x.BookingId == element)
                                                                   .Select(x => x.BookingStatusName).FirstOrDefault());

                    //get the booking status id
                    var status = quInspItems?.Where(x => x.BookingId == element)
                                                                   .Select(x => new { x.BookingStatusId, x.BookingStatusName }).FirstOrDefault();

                    //add the booking number and booking status color to a list
                    bookingIdStatusList.Add(new BookingStatusColorList()
                    {
                        BookingId = "#" + element + (!string.IsNullOrWhiteSpace(CustomerBookingNo) ? " / " + CustomerBookingNo : ""),
                        StatusColor = InspectionStatusColor.GetValueOrDefault(status.BookingStatusId, ""),
                        StatusName = status.BookingStatusName
                    });

                    //get service data
                    var fromdate = quInspItems.Where(x => x.BookingId == element).Select(x => x.ServiceDateFrom).FirstOrDefault();
                    var todate = quInspItems.Where(x => x.BookingId == element).Select(x => x.ServiceDateTo).FirstOrDefault();
                    if (fromdate != null && todate != null)
                    {
                        string servicedate = fromdate.Date == todate.Date ? fromdate.Date.ToString(StandardDateFormat) : fromdate.Date.ToString(StandardDateFormat) + "-" + todate.Date.ToString(StandardDateFormat);
                        serviceDateList.Add(servicedate);
                    }

                    var serviceName = quInspItems.Where(x => x.BookingId == element).Select(x => x.ServiceTypeName).FirstOrDefault();
                    if (!string.IsNullOrEmpty(serviceName))
                    {
                        serviceNameList.Add(serviceName);
                    }

                    var _brandNameList = brandList.Where(x => x.BookingId == element).Select(x => x.BrandName).ToList();
                    if (_brandNameList.Any())
                    {
                        BrandNameList.AddRange(_brandNameList);
                    }

                    var _departmentNameList = departmentList.Where(x => x.BookingId == element).Select(x => x.DeptName).ToList();
                    if (_departmentNameList.Any())
                    {
                        DepartmentNameList.AddRange(_departmentNameList);
                    }

                    IsEAQF = quInspItems?.Where(x => x.BookingId == element).FirstOrDefault()?.IsEAQF;

                });

                //bookingstatusList taken the value as comma seperated
                bookingStatusCommaSeperated = bookingStatusList != null && bookingStatusList.Any() ? string.Join(", ", bookingStatusList) : "";
            }


            int productCount = 0;

            if (quInspProdItems != null && quInspProdItems.Any())
            {
                productCount = quInspProdItems.Select(x => x.ProductTranId).Distinct().Count();
            }

            var supplierCode = supplierCodes?.Where(x => x.CustomerId == entity.CustomerId && x.SupplierId == entity.SupplierId).Select(x => x.Code).FirstOrDefault();

            var supplierNameAndCode = !string.IsNullOrEmpty(supplierCode) ?
                                        "(" + supplierCode + ") - " + entity?.SupplierName.ToLower() : entity?.SupplierName.ToLower();

            return new QuotationItem
            {
                BookingNoCusBookingNo = BookingNoCusBookingNo,
                CustomerId = entity.CustomerId,
                CustomerName = entity.CustomerName,
                FactoryName = entity.FactoryName.ToLower(),
                SupplierName = supplierNameAndCode,
                Office = entity.Office,
                QuotationDate = entity.QuotationDate,
                QuotationId = entity.QuotationId,
                ServiceDateList = string.Join(",", serviceDateList),
                ServiceType = string.Join(",", serviceNameList.Distinct()),
                StatusId = entity.StatusId,
                StatusName = entity.StatusName,
                Abilities = funcGetAbilities((QuotationStatus)entity.StatusId),
                EstimatedManDay = entity.EstimatedManDay,
                InspectionFees = entity.InspectionFees,
                TravelCost = entity.TravelCost,
                TotalCost = entity.TotalCost,
                discount = entity.discount,
                ServiceId = entity.ServiceId,
                BookingStatusList = bookingStatusCommaSeperated,
                IsAdeoClient = isAdeoClientQuotation,
                BillingEntity = entity.BillingEntity,
                OtherCost = entity.OtherCost,
                PaymentTerm = entity.PaymentTerm,
                BillMethodName = entity.BillMethodName,
                BillPaidById = entity.BillPaidById,
                ProductCount = productCount,
                CurrencyName = entity.CurrencyName,
                BillPaidByName = entity.BillPaidByName,
                ValidatedBy = entity.ValidatedBy,
                ValidatedUserName = entity.ValidatedUserName,
                ValidatedOn = entity.ValidatedOn,
                customerRemark = entity.customerRemark,
                BookingIdStatusColorList = bookingIdStatusList,
                BrandName = string.Join(", ", BrandNameList.Distinct()),
                DepartmentName = string.Join(", ", DepartmentNameList.Distinct()),
                IsEAQF = IsEAQF
            };
        }

        public QuotationItem GetQuotatioAuditItem(QuotationItemRepo entity,
            Func<QuotationStatus?, IEnumerable<QuotationAbility>> funcGetAbilities,
            List<int> adeoCustomerIds,
            IEnumerable<QuotationInvoiceItem> quotationAuditandInvoices, List<SupplierCode> supplierCodes)
        {
            if (entity == null)
                return null;

            string BookingNoCusBookingNo = "";
            string bookingStatusCommaSeperated = "";
            List<string> serviceDateList = new List<string>();
            List<string> bookingStatusList = new List<string>();
            List<string> serviceNameList = new List<string>();
            List<string> BrandNameList = new List<string>();
            List<string> DepartmentNameList = new List<string>();


            List<BookingStatusColorList> bookingIdStatusList = new List<BookingStatusColorList>();

            //var additionalinfo = lstaddinfo.Any() ? lstaddinfo.Where(x => x.quotationid == entity.QuotationId).FirstOrDefault() : null;

            var isAdeoClientQuotation = adeoCustomerIds.Contains(entity.CustomerId) && entity.StatusId != (int)QuotationStatus.Canceled && entity.ServiceId == (int)Entities.Enums.Service.InspectionId ? true : false;

            var quAuditItems = quotationAuditandInvoices.Where(x => x.QuotationId == entity.QuotationId).ToList();

            if (quAuditItems != null && quAuditItems.Any())
            {
                var lstbookingids = quAuditItems.Select(x => x.BookingId).Distinct().ToList();

                BookingNoCusBookingNo = string.Join(", ", lstbookingids);
                //service date
                lstbookingids.ForEach(element =>
                {
                    //get audit status name based on id
                    bookingStatusList.Add(quAuditItems?.Where(x => x.BookingId == element)
                                                                 .Select(x => x.BookingStatusName).FirstOrDefault());

                    //get the booking status id
                    var status = quAuditItems?.Where(x => x.BookingId == element)
                                                                   .Select(x => new { x.BookingStatusId, x.BookingStatusName }).FirstOrDefault();

                    //add the audit number and audit status color to a list
                    bookingIdStatusList.Add(new BookingStatusColorList()
                    {
                        BookingId = element.ToString(),
                        StatusColor = AuditStatusColor.GetValueOrDefault(status.BookingStatusId, ""),
                        StatusName = status.BookingStatusName
                    });


                    //get service data
                    var fromdate = quAuditItems.Where(x => x.BookingId == element).Select(x => x.ServiceDateFrom).FirstOrDefault();
                    var todate = quAuditItems.Where(x => x.BookingId == element).Select(x => x.ServiceDateTo).FirstOrDefault();
                    if (fromdate != null && todate != null)
                    {
                        string servicedate = fromdate.Date == todate.Date ? fromdate.Date.ToString(StandardDateFormat) : fromdate.Date.ToString(StandardDateFormat) + "-" + todate.Date.ToString(StandardDateFormat);
                        serviceDateList.Add(servicedate);
                    }

                    var serviceName = quAuditItems.Where(x => x.BookingId == element).Select(x => x.ServiceTypeName).FirstOrDefault();
                    if (!string.IsNullOrEmpty(serviceName))
                    {
                        serviceNameList.Add(serviceName);
                    }

                    var brandName = quAuditItems.Where(x => x.BookingId == element).Select(x => x.BrandName).FirstOrDefault();
                    if (!string.IsNullOrEmpty(brandName))
                    {
                        BrandNameList.Add(brandName);
                    }

                    var departmentName = quAuditItems.Where(x => x.BookingId == element).Select(x => x.DepartmentName).FirstOrDefault();
                    if (!string.IsNullOrEmpty(departmentName))
                    {
                        DepartmentNameList.Add(departmentName);
                    }

                });
                //bookingstatusList taking the value as comma seperated
                bookingStatusCommaSeperated = bookingStatusList != null && bookingStatusList.Any() ? string.Join(", ", bookingStatusList) : "";
            }

            var supplierCode = supplierCodes?.Where(x => x.CustomerId == entity.CustomerId && x.SupplierId == entity.SupplierId).Select(x => x.Code).FirstOrDefault();

            var supplierNameAndCode = !string.IsNullOrEmpty(supplierCode) ?
                                        "(" + supplierCode + ") - " + entity?.SupplierName.ToLower() : entity?.SupplierName.ToLower();

            return new QuotationItem
            {
                BookingNoCusBookingNo = BookingNoCusBookingNo,
                CustomerId = entity.CustomerId,
                CustomerName = entity.CustomerName,
                FactoryName = entity.FactoryName.ToLower(),
                SupplierName = supplierNameAndCode,
                Office = entity.Office,
                QuotationDate = entity.QuotationDate,
                QuotationId = entity.QuotationId,
                ServiceDateList = string.Join(",", serviceDateList),
                ServiceType = string.Join(",", serviceNameList),
                StatusId = entity.StatusId,
                StatusName = entity.StatusName,
                Abilities = funcGetAbilities((QuotationStatus)entity.StatusId),
                EstimatedManDay = entity.EstimatedManDay,
                InspectionFees = entity.InspectionFees,
                TravelCost = entity.TravelCost,
                TotalCost = entity.TotalCost,
                discount = entity.discount,
                ServiceId = entity.ServiceId,
                BookingStatusList = bookingStatusCommaSeperated,
                IsAdeoClient = isAdeoClientQuotation,
                BillingEntity = entity.BillingEntity,
                OtherCost = entity.OtherCost,
                PaymentTerm = entity.PaymentTerm,
                BillMethodName = entity.BillMethodName,
                BillPaidById = entity.BillPaidById,
                CurrencyName = entity.CurrencyName,
                BillPaidByName = entity.BillPaidByName,
                ValidatedBy = entity.ValidatedBy,
                ValidatedUserName = entity.ValidatedUserName,
                ValidatedOn = entity.ValidatedOn,
                customerRemark = entity.customerRemark,
                BookingIdStatusColorList = bookingIdStatusList,
                BrandName = string.Join(", ", BrandNameList.Distinct()),
                DepartmentName = string.Join(", ", DepartmentNameList.Distinct())
            };
        }

        public QuotationDetails GetQuotation(QuQuotation entity, QuQuotation poDetail, List<PoDetails> productDetails)
        {
            if (entity == null)
                return null;

            LocationMap locationMap = new LocationMap();
            ReferenceMap ReferenceMap = new ReferenceMap();

            var quotation = new QuotationDetails
            {
                ApiInternalRemark = entity.ApiInternalRemark,
                ApiRemark = entity.ApiRemark,
                BillingMethod = GetBillingMethod(entity.BillingMethod),
                BillingPaidBy = GetBillPaidBy(entity.BillingPaidBy),
                Country = locationMap.GetCountry(entity.Country),
                Currency = locationMap.GetCurrency(entity.Currency),
                Customer = GetDataSource(entity.Customer, entity.ServiceId),
                CustomerLegalName = entity.CustomerLegalName,
                CustomerRemark = entity.CustomerRemark,
                Discount = entity.Discount,
                EstimatedManday = entity.EstimatedManday,
                Factory = GetDataSource(entity.Factory),
                Id = entity.Id,
                InspectionFees = entity.InspectionFees,
                LegalFactoryName = entity.LegalFactoryName,
                Office = new DataSource
                {
                    Id = entity.OfficeId,
                    Name = entity.Office?.LocationName
                },
                OtherCosts = entity.OtherCosts,
                Service = ReferenceMap.GetService(entity.Service),
                Supplier = GetDataSource(entity.Supplier),
                SupplierLegalName = entity.SupplierLegalName,
                TotalCost = entity.TotalCost,
                TravelCostsAir = entity.TravelCostsAir,
                TravelCostsHotel = entity.TravelCostsHotel,
                TravelCostsLand = entity.TravelCostsLand,
                CustomerContactList = entity.QuQuotationCustomerContacts.Select(x => new QuotationEntityContact
                {
                    ContactEmail = x.IdContactNavigation?.Email,
                    ContactId = x.IdContact,
                    ContactName = x.IdContactNavigation.ContactName,
                    ContactPhone = x.IdContactNavigation.Phone,
                    EntityType = QuotationEntityType.Customer,
                    Email = x.Email,
                    Quotation = x.Quotation,
                    InvoiceEmail = x.InvoiceEmail
                }),
                SupplierContactList = entity.QuQuotationSupplierContacts.Select(x => new QuotationEntityContact
                {
                    ContactEmail = x.IdContactNavigation?.Mail,
                    ContactId = x.IdContact,
                    ContactName = x.IdContactNavigation.ContactName,
                    ContactPhone = x.IdContactNavigation.Phone,
                    EntityType = QuotationEntityType.Supplier,
                    Email = x.Email,
                    Quotation = x.Quotation,
                    InvoiceEmail = x.InvoiceEmail
                }),
                FactoryContactList = entity.QuQuotationFactoryContacts.Select(x => new QuotationEntityContact
                {
                    ContactEmail = x.IdContactNavigation?.Mail,
                    ContactId = x.IdContact,
                    ContactName = x.IdContactNavigation.ContactName,
                    ContactPhone = x.IdContactNavigation.Phone,
                    EntityType = QuotationEntityType.Factory,
                    Email = x.Email,
                    Quotation = x.Quotation,
                    InvoiceEmail = x.InvoiceEmail
                }),
                InternalContactList = entity.QuQuotationContacts.Select(x => new QuotationEntityContact
                {
                    ContactEmail = x.IdContactNavigation?.CompanyEmail,
                    ContactId = x.IdContact,
                    ContactName = x.IdContactNavigation.PersonName,
                    ContactPhone = x.IdContactNavigation.CompanyMobileNo,
                    EntityType = QuotationEntityType.Internal,
                    Email = x.Email,
                    Quotation = x.Quotation,
                    UserId = x.IdContactNavigation.ItUserMasters.FirstOrDefault()?.Id
                }),
                StatusId = (QuotationStatus)entity.IdStatus,
                StatusLabel = entity.IdStatusNavigation?.TranId.GetTranslation(entity.IdStatusNavigation?.Label),
                FactoryAddress = entity.FactoryAddress,
                IsToForward = (QuotationStatus)entity.IdStatus == QuotationStatus.QuotationCreated,
                CreatedDate = entity.CreatedDate.ToString(StandardDateFormat),
                ServiceTypeList = poDetail.QuQuotationInsps.Select(y => new ServiceType
                {
                    CriticalPick = y.IdBookingNavigation.InspProductTransactions.FirstOrDefault(x => x.Active.HasValue && x.Active.Value)?.CriticalNavigation?.Value,
                    MajorTolerancePick = y.IdBookingNavigation.InspProductTransactions.FirstOrDefault(x => x.Active.HasValue && x.Active.Value)?.MajorNavigation?.Value,
                    MinorTolerancePick = y.IdBookingNavigation.InspProductTransactions.FirstOrDefault(x => x.Active.HasValue && x.Active.Value).MinorNavigation?.Value
                }).FirstOrDefault(),
                BillingEntity = entity.BillingEntity,
                PaymentTerm = entity.PaymentTerms,
                RuleId = entity.RuleId,
                ProductRef = productDetails != null ? productDetails?.FirstOrDefault()?.ProductId + (productDetails?.GroupBy(x => x.ProductId).Count() > 1 ? $" (+{productDetails?.GroupBy(x => x.ProductId).Count() - 1})" : "") : "",
                PoNO = productDetails != null ? productDetails?.FirstOrDefault()?.PoNumber + (productDetails?.GroupBy(x => x.PoNumber).Count() > 1 ? $" (+{productDetails?.GroupBy(x => x.PoNumber).Count() - 1})" : "") : "",
                FactoryCountry = entity.Factory.SuAddresses.Select(x => x.Country.CountryName).FirstOrDefault(),
                PaymentTermsValue = entity.PaymentTermsValue,
                PaymentTermsCount = entity.PaymentTermsCount
            };

            if (entity.ServiceId == (int)Entities.Enums.Service.AuditId && entity.QuQuotationAudits.Any())
                quotation.OrderList = entity.QuQuotationAudits.Select(x => GetAudit(x.IdBookingNavigation));

            return quotation;

        }
        public QuQuotationInspManday GetQuQuotationInspManday(QuotationManday item, int userId)
        {
            return new QuQuotationInspManday
            {
                BookingId = item.BookingId,
                NoOfManday = item.ManDay ?? 0,
                Remarks = item.Remarks,
                ServiceDate = DateTime.ParseExact(item.ServiceDate, StandardDateFormat, CultureInfo.InvariantCulture),
                CreatedBy = userId,
                Active = true
            };
        }
        public QuQuotationAudManday GetQuQuotationAuditManday(QuotationManday item, int userId)
        {
            return new QuQuotationAudManday
            {
                BookingId = item.BookingId,
                NoOfManday = item.ManDay ?? 0,
                Remarks = item.Remarks,
                ServiceDate = DateTime.ParseExact(item.ServiceDate, StandardDateFormat, CultureInfo.InvariantCulture),
                CreatedBy = userId
            };
        }
        public QuotationManday GetQuQuotationInspMandayDTO(QuQuotationInspManday item)
        {
            return new QuotationManday
            {
                BookingId = item.BookingId,
                ManDay = item.NoOfManday ?? 0,
                Remarks = item.Remarks,
                ServiceDate = item.ServiceDate.ToString(StandardDateFormat)
            };
        }
        public QuotationManday GetQuQuotationAuditMandayDTO(QuQuotationAudManday item)
        {
            return new QuotationManday
            {
                BookingId = item.BookingId,
                ManDay = item.NoOfManday ?? 0,
                Remarks = item.Remarks,
                ServiceDate = item.ServiceDate.ToString(StandardDateFormat)
            };
        }
        private List<QuotProduct> GetProduct(InspTransaction entity, IEnumerable<QuInspProduct> inspProductList)
        {
            try
            {
                List<QuotProduct> objList = new List<QuotProduct>();

                var serviceTypeId = entity.InspTranServiceTypes.Where(x => x.Active).Select(x => x.ServiceTypeId).FirstOrDefault();

                foreach (var inspProduct in entity.InspProductTransactions.Where(x => x.Active.HasValue && x.Active.Value))
                {
                    var combineproductcount = 1;

                    int? combineproductid = 0;

                    bool isparentProduct = false;

                    string strPoNumber = string.Empty;

                    //if the products are combined and AQL is not selected, make the first product in the list as parent product
                    var isAQLSelected = entity.InspProductTransactions.Where(z => z.CombineProductId == inspProduct.CombineProductId
                                && z.CombineAqlQuantity.GetValueOrDefault() > 0).Count();

                    // var isAQLSelected = inspProduct.CombineAqlQuantity > 0 ? true : false;

                    int? sample = null;
                    string aqlDescription = "";

                    if (inspProduct.CombineProductId != null)
                    {
                        combineproductid = inspProduct.CombineProductId ?? 0;

                        combineproductcount = entity.InspProductTransactions.Where(x => x.Active.HasValue && x.Active.Value
                                                            && x.CombineProductId == combineproductid).Count();
                        strPoNumber = string.Join(", ", inspProduct?.InspPurchaseOrderTransactions?.Where(x => x.Active.HasValue && x.Active.Value
                         && x.ProductRef.CombineProductId == combineproductid)?.Select(x => x?.Po?.Pono).Distinct().ToArray());

                        sample = inspProduct.CombineAqlQuantity ?? 0;
                        isparentProduct = (inspProduct.CombineProductId.GetValueOrDefault() == 0) ? true : (inspProduct.CombineAqlQuantity != null &&
                                            inspProduct.CombineAqlQuantity != 0) ? true : (isAQLSelected == 0 && entity.InspProductTransactions
                                            .Where(z => z.CombineProductId == inspProduct.CombineProductId).OrderBy(x => x.Product.ProductId)
                                            .FirstOrDefault().ProductId == inspProduct.ProductId ? true : false);

                    }
                    else
                    {
                        strPoNumber = string.Join(", ", inspProduct?.InspPurchaseOrderTransactions.Where(x => x.Active.HasValue && x.Active.Value).Select(x => x?.Po?.Pono));
                        sample = inspProduct?.AqlQuantity ?? 0;
                        isparentProduct = true;
                    }

                    if (inspProductList != null)
                    {
                        // removed po comparison
                        var item = inspProductList.FirstOrDefault(x => x.ProductTran.InspectionId == inspProduct.InspectionId
                        && x.ProductTran.ProductId == inspProduct.ProductId);

                        if (item != null)
                        {
                            aqlDescription = item.AqlLevelDesc;
                        }
                    }

                    var aqlLevelAndSampleType = inspProduct?.SampleType > 0 ? inspProduct.AqlNavigation?.Value + " (" + inspProduct.SampleTypeNavigation?.SampleType + ")" :
                        inspProduct.AqlNavigation?.Value;

                    var unitAndUnitCount = inspProduct?.UnitCount.GetValueOrDefault() > 0 ? inspProduct?.UnitNavigation?.Name + " (" + inspProduct?.UnitCount + ")" :
                        inspProduct?.UnitNavigation?.Name;

                    objList.Add(new QuotProduct
                    {
                        ProductRefId = inspProduct.Id,
                        Id = inspProduct.ProductId, //inspProduct.PoDetail.ProductId,
                        InspPoId = inspProduct.Id,
                        PoNo = strPoNumber,
                        Destination = string.Join(", ", inspProduct.InspPurchaseOrderTransactions?.Select(x => x.DestinationCountry?.CountryName)),  //Join(", ", inspProduct.PoDetail?.DestinationCountry?.CountryName),

                        ProductId = inspProduct.Product?.ProductId,
                        ProductDescription = inspProduct.Product?.ProductDescription,
                        AqlLevel = inspProduct.AqlNavigation?.Value,
                        AqlLevelAndSampleType = aqlLevelAndSampleType,
                        AqlLevelDescription = aqlDescription,
                        //total booking qty
                        BookingQty = inspProduct.TotalBookingQuantity,
                        unitAndUnitCount = unitAndUnitCount,
                        FactoryReference = inspProduct?.Product?.FactoryReference,
                        SampleQty = sample.GetValueOrDefault(),
                        CombineProductList = null, //GetCombinedProductList(inspProduct.ProductId, entity, inspProductList, aqlList),
                        PictList = inspProduct?.Product?.CuProductFileAttachments?.Where(y => y.Active && y.FileTypeId.HasValue && y.FileTypeId.Value == (int)ProductRefFileType.ProductRefPictures).Select(x => x.Id),
                        ProductCategory = inspProduct.Product?.ProductCategoryNavigation?.Name,
                        ProductSubCategory = inspProduct?.Product?.ProductSubCategoryNavigation?.Name,
                        ProductSubCategory2 = inspProduct?.Product?.ProductCategorySub2Navigation?.Name,
                        ProductSubCategory3 = inspProduct?.Product?.ProductCategorySub3Navigation?.Name,
                        PickingQty = inspProduct?.InspPurchaseOrderTransactions?.Sum(x => x.PickingQuantity), //inspProduct.PickingQuantity
                        ProductRemarks = inspProduct?.Remarks,
                        CombineProductCount = combineproductcount,
                        CombineProductId = combineproductid.Value,
                        IsParentProduct = isparentProduct,
                        CombineAqlQuantity = inspProduct.CombineAqlQuantity.GetValueOrDefault(), // Added for sorting
                        TimePreparation = inspProduct?.Product?.TimePreparation,
                        SampleSize8h = inspProduct?.Product?.SampleSize8h,
                        CustomerRequirementIndex = entity?.Customer?.CuServiceTypes?.Where(x => x.Active && x.ServiceTypeId == serviceTypeId).Select(x => x.CustomerRequirementIndex).FirstOrDefault()
                    });
                }
                return objList.OrderBy(x => x.CombineProductId).ThenByDescending(x => x.CombineAqlQuantity).ThenBy(x => x.ProductId).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //private  IEnumerable<QuotProduct> GetCombinedProductList(int productId, InspTransaction entity, IEnumerable<QuQuotationPo> inspProductList, IEnumerable<AqlInfo> aqlList)
        //{
        //    var items = entity.InspPoCombineTransactions.Where(x => x.CombineProductId == productId);

        //    if (items.Any())
        //    {
        //        foreach (var item in items)
        //        {
        //            var inspProduct = entity.InspPoTransactions.FirstOrDefault(x => x.ProductId == item.ProductId);

        //            yield return GetProduct(inspProduct, entity, inspProductList, aqlList);
        //        }
        //    }

        //}


        public Order GetAudit(AudTransaction entity)
        {
            if (entity == null)
                return null;


            return new Order
            {
                Id = entity.Id,
                InspectionFrom = entity.ServiceDateFrom.ToString(StandardDateFormat),
                InspectionTo = entity.ServiceDateTo.ToString(StandardDateFormat),
                InternalBookingRemarks = entity.InternalComments,
                orderCost = GetAuditOrderCost(entity.QuQuotationAudits.FirstOrDefault()),
                ServiceTypeList = entity.AudTranServiceTypes.Where(x => x.Active).Select(x => new ServiceType
                {
                    Id = x.ServiceTypeId,
                    Name = x.ServiceType?.Name
                }),
                Office = entity.Office?.LocationName,
                StatusId = entity.StatusId,
                LocationId = entity.OfficeId
            };

        }

        public QuotStatus GetStatus(QuStatus entity)
        {
            if (entity == null)
                return null;

            return new QuotStatus
            {
                Id = entity.Id,
                Label = entity.TranId.GetTranslation(entity.Label)
            };
        }

        public QuotationSummaryStatus GetQuotationStatuswithColor(QuStatus entity)
        {
            if (entity == null)
                return null;
            return new QuotationSummaryStatus
            {
                Id = entity.Id,
                StatusName = entity.TranId.GetTranslation(entity.Label),
                StatusColor = QuotationStatusColor.GetValueOrDefault(entity.Id, "")
            };
        }
        public QuotationSummaryStatus GetQuotationStatuswithColor(QuotationSummaryStatus entity)
        {
            if (entity == null)
                return null;
            return new QuotationSummaryStatus
            {
                Id = entity.Id,
                StatusName = entity.StatusName,
                TotalCount = entity.TotalCount,
                StatusColor = QuotationStatusColor.GetValueOrDefault(entity.Id, "")
            };
        }
        public QuotationManday getQuotationManDay(DateTime serviceDate, int bookingId)
        {
            //if (entity == null)
            //    return null;
            return new QuotationManday
            {
                ServiceDate = serviceDate.ToString(StandardDateFormat),
                BookingId = bookingId
            };
        }

        public DataSource GetDataSource(RefServiceType entity)
        {
            if (entity == null)
                return null;

            return new DataSource
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }
        //map the aedetails to QuotationEntityContact
        public QuotationEntityContact GetAEContact(AEDetails entity)
        {
            if (entity == null)
                return null;

            return new QuotationEntityContact
            {
                ContactId = entity.StaffId,
                ContactName = entity.FullName,
                ContactEmail = entity.EmailAddress,
                ContactPhone = entity.MobileNumber,
                EntityType = QuotationEntityType.Internal,
                CustomerAE = (entity.UserType == (int)HRProfile.CS)
            };
        }

        public QuotationExport ClientQuotationMap(ClientQuotationItem quotation, List<QuotationBookingItem> productList, List<SupplierCode> supCodes, int productQty, int totalSamplingQuantity)
        {
            return new QuotationExport
            {
                QuotationId = quotation.QuotationId,
                BookingId = productList.FirstOrDefault().BookingId,
                QuotationDate = quotation.QuotationDate.ToString(StandardDateFormat),
                CustomerId = quotation.CustomerId,
                SupplierName = quotation.SupplierName,
                FactoryName = quotation.FatoryName,
                SupplierCode = supCodes.Where(x => x.SupplierId == quotation.SupplierId).Select(x => x.Code).FirstOrDefault(),
                FactoryCode = supCodes.Where(x => x.SupplierId == quotation.FactoryId).Select(x => x.Code).FirstOrDefault(),
                InspectionLocation = quotation.InspectionLocation.Select(x => x.Address).FirstOrDefault(),
                ServiceFromDate = productList.FirstOrDefault().ServiceFromDate.GetValueOrDefault().ToString(StandardDateFormat),
                ServiceToDate = productList.FirstOrDefault().ServiceToDate.GetValueOrDefault().ToString(StandardDateFormat),
                ProductDescription = quotation.ProductCategory,
                Sampling = quotation.AQL,
                ProductQuantity = productQty,
                QuotationUnitPrice = quotation.QuotationPrice / quotation.ManDay,
                TotalPrice = quotation.QuotationPrice,
                TravelCost = quotation.TravelCost ?? 0,
                Transportcost = quotation.TravelCostAir ?? 0 + quotation.TravelCostLand,
                HotelCost = quotation.HotelCost ?? 0,
                OtherCost = quotation.OtherCost ?? 0,
                PoInformation = productList,
                ManDay = quotation.ManDay,
                DepartmentName = quotation.DepartmentName,
                CustomeServiceTypeName = quotation.CuServiceType.Where(x => x.Active && x.ServiceTypeId == quotation.InspServiceTypeId).Select(x => x.CustomServiceTypeName).FirstOrDefault(),
                TotalCost = quotation.QuotationPrice + quotation.Transportcost.GetValueOrDefault() + quotation.TravelCost.GetValueOrDefault() + quotation.HotelCost.GetValueOrDefault() + quotation.OtherCost.GetValueOrDefault(),
                TotalSamplingSize = totalSamplingQuantity
            };
        }

        public string GetValidatedByName(int QuStatus, int? UserType, string Name, string entityName, int loginUserTypeId)
        {
            string _validatebyName = string.Empty;
            if (!UserType.HasValue || QuStatus != (int)QuotationStatus.CustomerValidated)
                return _validatebyName;

            int userType = UserType.Value;
            switch (userType)
            {
                case (int)UserTypeEnum.InternalUser:
                    _validatebyName = loginUserTypeId == (int)UserTypeEnum.InternalUser ?
                      entityName + " (" + Name + ")" : entityName;
                    break;
                case (int)UserTypeEnum.Customer:
                    _validatebyName = "Customer (" + Name + ")";
                    break;
                case (int)UserTypeEnum.Supplier:
                    _validatebyName = "Supplier (" + Name + ")";
                    break;
                case (int)UserTypeEnum.Factory:
                    _validatebyName = "Factory (" + Name + ")";
                    break;
                default:
                    _validatebyName = string.Empty;
                    break;
            }

            return _validatebyName;
        }

        // mapping Quotation Summary Insp Export Details
        public List<QuotationInspProdExportItem> GetQuotationSummaryInspExport(IEnumerable<QuotationInspAuditExportRepo> quotationInspProductDataList,
                                                                      IEnumerable<ServiceTypeList> serviceTypeList,
                                                                      List<QuotationInvoiceItem> quotationInvoices,
                                                                      List<Tuple<int, string, string>> quobillings,
                                                                      List<InvoiceInfo> invoicedetails,
                                                                      IEnumerable<SupplierAddressData> factAddressList,
                                                                      List<BookingProductPoRepo> bookingProductPoList,
                                                                      List<BookingBrandAccess> brandList,
                                                                      List<BookingBuyerAccess> buyerList,
                                                                      List<BookingDeptAccess> departmentList,
                                                                      bool isInternalUser, string entityName,
                                                                      int loginUserTypeId, List<SupplierCode> supplierCodeList
                                                                       )
        {
            List<QuotationInspProdExportItem> result = new List<QuotationInspProdExportItem>();
            bool isNewQuotation = true;
            int preQuotationId = 0;
            double totalFee = 0;
            foreach (var quotation in quotationInspProductDataList)
            {

                isNewQuotation = preQuotationId != quotation.QuotationId;

                var quBill = quobillings.Where(x => x.Item1 == quotation.QuotationId);
                var billingaddress = quBill.Select(x => x.Item2).FirstOrDefault();
                var billingcontact = quBill.Select(x => x.Item3).FirstOrDefault();

                var isactualdate = quotation.FBReportStartDate != null && quotation.FBReportEndDate != null;
                if (isNewQuotation)
                {
                    totalFee = quotation?.TotalFee.GetValueOrDefault() ?? 0 + quotation?.OtherCost.GetValueOrDefault() ?? 0 - quotation?.Discount.GetValueOrDefault() ?? 0;
                }
                else
                {
                    totalFee = 0;
                }
                var _validatebyName = GetValidatedByName(quotation.QuotationStatusId, quotation.ValidatedByUserType, quotation.ValidatedByName, entityName,
                    loginUserTypeId);

                //var _serviceDate = quotation.ServiceDateFrom.Date == quotation.ServiceDateTo.Date ?
                //                           quotation.ServiceDateFrom.ToString(StandardDateFormat) :
                //                           quotation.ServiceDateFrom.ToString(StandardDateFormat)
                //                            + "-" + quotation.ServiceDateTo.ToString(StandardDateFormat);

                var _factoryAddress = factAddressList.FirstOrDefault(x => x.SupplierId == quotation.FactoryId);
                var _invoicedetails = invoicedetails.Where(y => y.Inspectiono == quotation.BookingId).FirstOrDefault();
                var _InvoicesByQuotation = quotationInvoices.Where(y => y.QuotationId == quotation.QuotationId && y.BookingId == quotation.BookingId).FirstOrDefault();
                var _fbReportStartDate = quotation.FBReportStartDate;
                var _fbReportEndDate = quotation.FBReportEndDate;

                var _supplierCode = supplierCodeList?.Where(x => x.CustomerId == quotation.CustomerId && x.SupplierId == quotation.SupplierId).Select(x => x.Code).FirstOrDefault();

                var data = new QuotationInspProdExportItem
                {
                    QuotationNo = quotation.QuotationId,
                    Customer = quotation.CustomerName,
                    Supplier = quotation.SupplierName,
                    SupplierCode = _supplierCode,
                    Factory = quotation.FactoryName,
                    QuotationDate = quotation.QuotationDate,
                    Office = quotation.Office,
                    Status = quotation.QuotationStatus,
                    TotalManDay = isNewQuotation ? Convert.ToDouble(quotationInvoices.Where(y => y.BookingId == quotation.BookingId && y.QuotationId == quotation.QuotationId).Select(y => y.Manday).FirstOrDefault()) : 0,
                    InspectionCost = isNewQuotation ? quotation.InspectionCost : 0,
                    Discount = isNewQuotation ? quotation.Discount : 0,
                    TravelCost = isNewQuotation ? quotation.TravelCostAir + quotation.TravelCostLand + quotation.TravelCostHotel : 0,
                    TotalFees = totalFee,
                    Currency = isNewQuotation ? quotation.Currency : "",
                    BillPaidBy = quotation.BillPaidBy,
                    BillPaidByAddress = billingaddress,
                    BillPaidByContact = billingcontact,
                    APIRemark = quotation.APIRemark,
                    CustomerRemark = quotation.CustomerRemark,
                    CustomerLegalName = quotation.CustomerLegalName,
                    SupplierLegalName = quotation.SupplierLegalName,
                    FactoryLegalName = quotation.FactoryLegalName,
                    BookingNo = quotation.BookingId,
                    ServiceFromDate = quotation.ServiceDateFrom.Date,
                    ServiceToDate = quotation.ServiceDateTo.Date,
                    ProductReference = quotation.ProductReference,
                    ProductDesc = quotation.ProductDescription,
                    BookingQty = quotation.BookingQty,
                    SampleSize = quotation.CombineAQL > 0 ? quotation.CombineAQL : quotation.SampleSize,
                    ReportStatus = quotation.ReportResult,
                    FactoryReference = quotation.FactoryRef,
                    PoNumber = String.Join(",", bookingProductPoList.Where(x => x.BookingId == quotation.BookingId && x.ProductRefId == quotation.ProductRefId).Select(x => x.PoName).ToList()),
                    ServiceType = serviceTypeList.Where(z => z.InspectionId == quotation.BookingId).FirstOrDefault()?.serviceTypeName,
                    InspectionStatus = quotation.BookingStatus,
                    ActualInspectionDate = !isactualdate ? "" : _fbReportStartDate == _fbReportEndDate ?
                                            _fbReportStartDate.Value.ToString(StandardDateFormat) :
                                            _fbReportStartDate.Value.ToString(StandardDateFormat)
                                            + "-" + _fbReportEndDate.Value.ToString(StandardDateFormat),
                    BillingEntity = quotation.BillingEntity,
                    OtherCost = isNewQuotation ? quotation.OtherCost.GetValueOrDefault() : 0,
                    InvoiceDate = _invoicedetails?.InvoiceDate,
                    InvoiceNo = _invoicedetails?.InvoiceNo,
                    InvoiceRemarks = _invoicedetails?.InvoiceREmarks,
                    PaymentTerm = quotation.PaymentTerm,
                    Traveldistance = _InvoicesByQuotation.TravelDistance,
                    TravelTime = _InvoicesByQuotation.TravelTime,
                    CustomerBookingNo = quotation.CustomerBookingNo,
                    CountryName = _factoryAddress?.CountryName,
                    ProvinceName = _factoryAddress?.RegionName,
                    CityName = _factoryAddress?.CityName,
                    CountyName = _factoryAddress?.CountyName,
                    ValidatedOn = quotation.ValidatedOn,
                    ValidatedByName = _validatebyName,
                    BrandName = string.Join(",", brandList.Where(x => x.BookingId == quotation.BookingId).Select(x => x.BrandName).ToList()),
                    DepartmentName = string.Join(",", departmentList.Where(X => X.BookingId == quotation.BookingId).Select(x => x.DeptName).ToList()),
                    BuyerName = string.Join(",", buyerList.Where(X => X.BookingId == quotation.BookingId).Select(x => x.BuyerName).ToList()),
                    CalculatedWorkingHours = isInternalUser ? _InvoicesByQuotation.CalculatedWorkingHours : null,
                    CalculatedWorkingManday = isInternalUser ? _InvoicesByQuotation.CalculatedWorkingManday : null,
                    BilledQtyType = _InvoicesByQuotation.BilledQtyType,
                    Quantity = _InvoicesByQuotation.Quantity
                };
                result.Add(data);

                preQuotationId = quotation.QuotationId;

            }
            return result;
        }
        // mapping Quotation Summary Audit Export Details
        public List<QuotationAuditExportItem> GetQuotationSummaryAuditExport(IEnumerable<QuotationInspAuditExportRepo> quotationAuditDataList,
                                                                    List<Tuple<int, string, string>> quobillings,
                                                                    IEnumerable<SupplierAddressData> factAddressList,
                                                                     IEnumerable<QuotationInvoiceItem> quotationAudits,
                                                                    IEnumerable<QuotationAuditReportItem> quotationAuditReport,
                                                                     List<InvoiceInfo> quotationAuditInvoice, string entityName,
                                                                     int loginUserTypeId, List<SupplierCode> supplierCodeList)
        {
            List<QuotationAuditExportItem> result = new List<QuotationAuditExportItem>();


            bool isNewQuotation = true;
            int preQuotationId = 0;
            foreach (var quotation in quotationAuditDataList)
            {
                isNewQuotation = preQuotationId != quotation.QuotationId;

                var quBill = quobillings.Where(x => x.Item1 == quotation.QuotationId);
                var billingaddress = quBill.Select(x => x.Item2).FirstOrDefault();
                var billingcontact = quBill.Select(x => x.Item3).FirstOrDefault();

                var _validatebyName = GetValidatedByName(quotation.QuotationStatusId, quotation.ValidatedByUserType, quotation.ValidatedByName, entityName,
                    loginUserTypeId);

                string _actualservicedate = string.Empty;
                var auditreportinfo = quotationAuditReport.Where(x => x.BookingId == quotation.BookingId).FirstOrDefault();
                if (auditreportinfo != null)
                {
                    _actualservicedate = auditreportinfo.ServiceDateFrom.Date == auditreportinfo.ServiceDateTo.Date ? auditreportinfo.ServiceDateFrom.Date.ToString(StandardDateFormat) : auditreportinfo.ServiceDateFrom.Date.ToString(StandardDateFormat) + "-" + auditreportinfo.ServiceDateTo.Date.ToString(StandardDateFormat);
                }

                var _serviceDateFrom = quotation.ServiceDateFrom;
                var _serviceDateTo = quotation.ServiceDateTo;
                var _factoryAddress = factAddressList.FirstOrDefault(x => x.SupplierId == quotation.FactoryId);
                var _quotationAudits = quotationAudits.Where(y => y.BookingId == quotation.BookingId).FirstOrDefault();
                var _InvoicesByQuotation = quotationAudits.Where(y => y.QuotationId == quotation.QuotationId).FirstOrDefault();
                var _quotationAuditInvoice = quotationAuditInvoice.Where(y => y.AuditNo == quotation.BookingId).FirstOrDefault();

                var _supplierCode = supplierCodeList?.Where(x => x.CustomerId == quotation.CustomerId && x.SupplierId == quotation.SupplierId).Select(x => x.Code).FirstOrDefault();

                var data = new QuotationAuditExportItem
                {
                    QuotationNo = quotation.QuotationId,
                    BookingNo = quotation.BookingId,
                    Customer = quotation.CustomerName,
                    Supplier = quotation.SupplierName,
                    SupplierCode = _supplierCode,
                    Factory = quotation.FactoryName,
                    QuotationDate = quotation.QuotationDate,
                    ServiceFromDate = _serviceDateFrom.Date,
                    ServiceToDate = _serviceDateTo.Date,
                    ServiceType = quotationAudits.Where(z => z.BookingId == quotation.BookingId).FirstOrDefault()?.ServiceTypeName,
                    Office = quotation.Office,
                    Status = quotation.QuotationStatus,
                    TotalManDay = isNewQuotation ? quotation.EstimatedManDay : 0,
                    InspectionCost = isNewQuotation ? quotation.InspectionCost : 0,
                    Discount = isNewQuotation ? quotation.Discount : 0,
                    TravelCost = isNewQuotation ? quotation.TravelCostAir + quotation.TravelCostLand + quotation.TravelCostHotel : 0,
                    OtherCost = isNewQuotation ? quotation.OtherCost.GetValueOrDefault() : 0,
                    TotalFees = isNewQuotation ? quotation.TotalFee.GetValueOrDefault() : 0,
                    Currency = isNewQuotation ? quotation.Currency : "",
                    BillPaidBy = quotation.BillPaidBy,
                    BillPaidByAddress = billingaddress,
                    BillPaidByContact = billingcontact,
                    APIRemark = quotation.APIRemark,
                    CustomerRemark = quotation.CustomerRemark,
                    CustomerLegalName = quotation.CustomerLegalName,
                    SupplierLegalName = quotation.SupplierLegalName,
                    FactoryLegalName = quotation.FactoryLegalName,
                    CountryName = _factoryAddress?.CountryName,
                    ProvinceName = _factoryAddress?.RegionName,
                    CityName = _factoryAddress?.CityName,
                    CountyName = _factoryAddress?.CountyName,
                    ActualInspectionDate = _actualservicedate,
                    InspectionStatus = _quotationAudits.BookingStatusName,
                    BillingEntity = quotation.BillingEntity,
                    InvoiceNo = _quotationAuditInvoice?.InvoiceNo,
                    InvoiceDate = _quotationAuditInvoice?.InvoiceDate,
                    InvoiceRemarks = _quotationAuditInvoice?.InvoiceREmarks,
                    PaymentTerm = quotation.PaymentTerm,
                    Traveldistance = _InvoicesByQuotation.TravelDistance,
                    TravelTime = _InvoicesByQuotation.TravelTime,
                    ValidatedOn = quotation.ValidatedOn.HasValue ? quotation.ValidatedOn.Value : null,
                    ValidatedByName = _validatebyName,
                    BrandName = quotation.BrandName,
                    DepartmentName = quotation.DepartmentName
                };
                result.Add(data);

                preQuotationId = quotation.QuotationId;
            }


            return result;


        }

        public FactoryBookingInfo FactoryBookingInfo(BookingDetail booking, List<ServiceTypeList> serviceTypelst, IEnumerable<ScheduleProductsData> productlst, IEnumerable<ScheduleContainersRepo> containerlst, List<DTO.Invoice.QuotationInspectionTravelCost> quotationlst, List<DTO.Invoice.InvoiceDetail> invoicelst)
        {
            var quotation = quotationlst.FirstOrDefault(x => x.BookingId == booking.BookingId);
            var inspProducts = productlst.Where(x => x.BookingId == booking.BookingId).ToList();
            var inspContainers = containerlst.Where(x => x.BookingId == booking.BookingId).ToList();
            var inspInvoices = invoicelst.Where(x => x.InspectionId == booking.BookingId && x.CalculateInspectionFee == (int)InvoiceFeesFrom.Carrefour).ToList();
            var serviceType = serviceTypelst.FirstOrDefault(x => x.InspectionId == booking.BookingId);

            var factoryBookingInfo = new FactoryBookingInfo
            {
                BookingId = booking.BookingId,
                CustomerName = booking.CustomerName,
                ServiceDate = booking.ServiceFrom == booking.ServiceTo ? booking.ServiceFrom.ToString(StandardDateFormat) : string.Join(" - ", booking.ServiceFrom.ToString(StandardDateFormat), booking.ServiceTo.ToString(StandardDateFormat)),
                ProductCount = inspProducts.Select(x => x.ProductId).Distinct().Count(),
                ReportCount = (serviceType?.serviceTypeId == (int)InspectionServiceTypeEnum.Container) ?
                                inspContainers.Where(x => x.ContainerId > 0).Select(x => x.ContainerId).Distinct().Count() :
                                inspProducts.Count(x => x.CombineProductId == 0) + inspProducts.Where(x => x.CombineProductId != 0).Select(x => x.CombineProductId).Distinct().Count(),
                SampleSize = inspProducts.Where(x => x.CombineProductId > 0).Sum(x => x.CombineAqlQuantity.GetValueOrDefault()) +
                                inspProducts.Where(x => !(x.CombineProductId > 0)).Sum(x => x.AqlQuantity.GetValueOrDefault()),
                ServiceTypeName = serviceType?.serviceTypeName
            };

            if (inspInvoices.Any())
            {
                factoryBookingInfo.InspectionFee = inspInvoices.Sum(x => x.InspectionFees.GetValueOrDefault());
                factoryBookingInfo.TravelCost = inspInvoices.Sum(x => x.TravelTotalFees.GetValueOrDefault());
                factoryBookingInfo.OtherCost = inspInvoices.Sum(x => x.OtherFees.GetValueOrDefault());
            }
            if (quotation != null)
            {
                factoryBookingInfo.Manday = quotation.Mandays;
                factoryBookingInfo.IsQuotation = true;
                factoryBookingInfo.SuggestedManday = quotation.SuggestedManday;
                if (!inspInvoices.Any())
                {
                    factoryBookingInfo.InspectionFee = quotation.InspectionFees;
                    factoryBookingInfo.TravelCost = quotation.TravelAirCost + quotation.TravelHotelCost + quotation.TravelLandCost;
                    factoryBookingInfo.OtherCost = quotation.OtherCost;
                }

            }
            return factoryBookingInfo;
        }
    }
}
