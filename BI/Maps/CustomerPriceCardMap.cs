using DTO.Common;
using DTO.Customer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BI.Maps
{
    public class CustomerPriceCardMap : ApiCommonData
    {
        //get data to map and show the info with formated fields
        public List<CustomerPriceCardSummaryItem> GetDataMap(List<CustomerPriceCardSummaryItem> entity, ExportMapRequest paramList)
        {
            return entity.Select(priceCardData => new CustomerPriceCardSummaryItem
            {
                BillMethodName = priceCardData.BillMethodName,
                BillToName = priceCardData.BillToName,
                CurrencyName = priceCardData.CurrencyName,
                CustomerName = priceCardData.CustomerName,
                Id = priceCardData.Id,
                ServiceName = priceCardData.ServiceName,
                UnitPrice = priceCardData.UnitPrice,
                FactoryCountryList = string.Join(", ", paramList.CountryData.Where(x => x.PriceId == priceCardData.Id).Select(x => x.Name).Distinct()),
                DepartmentName = string.Join(", ", paramList.DeptData.Where(x => x.PriceId == priceCardData.Id).Select(x => x.Name).Distinct()),
                PriceCategory = string.Join(", ", paramList.PriceCategory.Where(x => x.PriceId == priceCardData.Id).Select(x => x.Name).Distinct()),
                PeriodTo = priceCardData.PeriodToDate?.ToString(StandardDateFormat),
                PeriodFrom = priceCardData.PeriodFromDate?.ToString(StandardDateFormat),
                ServiceTypeList = string.Join(", ", paramList.ServiceType.Where(x => x.PriceId == priceCardData.Id).Select(x => x.Name).Distinct()),
                TravelIncluded = priceCardData.TravelIncludedBool == null ? null : priceCardData.TravelIncludedBool.HasValue && priceCardData.TravelIncludedBool.Value ? "Yes" : "No",
                TaxInclude = priceCardData.TaxIncludedBool == null ? null : priceCardData.TaxIncludedBool.HasValue && priceCardData.TaxIncludedBool.Value ? "Yes" : "No",
                SupplierNameList = string.Join(", ", paramList.SupData.Where(x => x.PriceId == priceCardData.Id).Select(x => x.Name).Distinct()),
                ProductCategoryNameList = string.Join(", ", paramList.ProductCategory.Where(x => x.PriceId == priceCardData.Id).Select(x => x.Name).Distinct()),
                FactoryProvinceList = string.Join(", ", paramList.ProvinceData.Where(x => x.PriceId == priceCardData.Id).Select(x => x.Name).Distinct()),
                Remarks = priceCardData.Remarks,
                FreeTraveKM = priceCardData.FreeTraveKM,
                CreatedByName = priceCardData.CreatedByName,
                CreatedOn = priceCardData.CreatedOn,
                UpdatedByName = priceCardData.UpdatedByName,
                UpdatedOn = priceCardData.UpdatedOn
            }).OrderByDescending(y => y.PeriodFrom).ThenBy(y => y.CustomerName).ToList();
        }

        //show in quotation page
        public QuotationCustomerPriceCard GetPriceCardQuotationMap(QuotationCustomerPriceCardData entity, ExportMapRequest list)
        {
            return new QuotationCustomerPriceCard()
            {
                Id = entity.Id,
                Remarks = entity.Remarks,
                FreeTravelKM = entity.FreeTravelKM,
                TaxIncluded = entity.TaxIncluded == null ? null : entity.TaxIncluded.HasValue && entity.TaxIncluded.Value ? "Yes" : "No",
                TravelIncluded = entity.TravelIncluded == null ? null : entity.TravelIncluded.HasValue && entity.TravelIncluded.Value ? "Yes" : "No",
                UnitPrice = entity.UnitPrice,
                ServiceTypeNames = string.Join(", ", list.ServiceType.Where(x => x.PriceId == entity.Id).Select(x => x.Name).Distinct()),
                ProductCategoryNames = string.Join(", ", list.ProductCategory.Where(x => x.PriceId == entity.Id).Select(x => x.Name).Distinct()),
                FactoryCountryNames = string.Join(", ", list.CountryData.Where(x => x.PriceId == entity.Id).Select(x => x.Name).Distinct()),
                BrandNames = string.Join(", ", list.BrandData.Where(x => x.PriceId == entity.Id).Select(x => x.Name).Distinct()),
                BuyerNames = string.Join(", ", list.BuyerData.Where(x => x.PriceId == entity.Id).Select(x => x.Name).Distinct()),
                DepartmentNames = string.Join(", ", list.DeptData.Where(x => x.PriceId == entity.Id).Select(x => x.Name).Distinct()),
                PriceCategoryNames = string.Join(", ", list.PriceCategory.Where(x => x.PriceId == entity.Id).Select(x => x.Name).Distinct()),
                BillingMethodName = entity.BillingMethodName,
                CustomerName = entity.CustomerName,
                PeriodFrom = entity.PeriodFrom?.ToString(StandardDateFormat),
                PeriodTo = entity.PeriodTo?.ToString(StandardDateFormat),
                TravelMatrixTypeId = entity.TravelMatrixTypeId,
                CurrencyId = entity.CurrencyId,
                CurrencyName = entity.CurrencyName,
                BillingMethodId = entity.BillingMethodId,
                BillingPaidById = entity.BillingPaidById,
                BillingPaidBy = entity.BillingPaidBy,
                ProvinceNames = string.Join(", ", list.ProvinceData.Where(x => x.PriceId == entity.Id).Select(x => x.Name).Distinct()),
                BillingEntityId = entity.BillingEntityId,
                PaymentTermsValue = entity.PaymentTermsValue,
                PaymentTermsCount = entity.PaymentTermsCount
            };
        }

        public ExportSummary MapExportSummary(ExportSummaryItem item, ExportMapRequest param)
        {

            return new ExportSummary()
            {
                UnitPrice = item.UnitPrice,
                CustomerName = item.CustomerName,
                CurrencyName = item.CurrencyName,
                PeriodTo = item.PeriodTo?.ToString(StandardDateFormat),
                PeriodFrom = item.PeriodFrom?.ToString(StandardDateFormat),
                BillMethod = item.BillMethod,
                BillPaidBy = item.BillPaidBy,
                FreeTravelKM = item.FreeTravelKM,
                Remarks = item.Remarks,
                TaxInclude = item.TaxInclude == null ? null : item.TaxInclude.HasValue && item.TaxInclude.Value ? "Yes" : "No",
                TraveInclude = item.TraveInclude == null ? null : item.TraveInclude.HasValue && item.TraveInclude.Value ? "Yes" : "No",
                Service = item.Service,
                ServiceTypes = string.Join(" ,", param.ServiceType?.Where(x => x.PriceId == item.Id).Select(x => x.Name)),
                SupplierNames = string.Join(" ,", param.SupData?.Where(x => x.PriceId == item.Id).Select(x => x.Name)),
                ProductCategorys = string.Join(" ,", param.ProductCategory?.Where(x => x.PriceId == item.Id).Select(x => x.Name)),
                FactoryCountry = string.Join(" ,", param.CountryData?.Where(x => x.PriceId == item.Id).Select(x => x.Name)),
                FactoryProvince = string.Join(" ,", param.ProvinceData?.Where(x => x.PriceId == item.Id).Select(x => x.Name)),
                Department = string.Join(" ,", param.DeptData?.Where(x => x.PriceId == item.Id).Select(x => x.Name)),
                Brand = string.Join(" ,", param.BrandData?.Where(x => x.PriceId == item.Id).Select(x => x.Name)),
                Buyer = string.Join(" ,", param.BuyerData?.Where(x => x.PriceId == item.Id).Select(x => x.Name)),
                PriceCategory = string.Join(" ,", param.PriceCategory?.Where(x => x.PriceId == item.Id).Select(x => x.Name)),
                HolidayType = string.Join(" ,", param.HolidayType?.Where(x => x.PriceId == item.Id).Select(x => x.Name)),
                MinBillingFeePerDay = item.MinBillingFeePerDay,
                MaximumProductCount = item.MaximumProductCount,
                InvoiceRequest = item.InvoiceRequest,
                BillingEntity = item.BillingEntity,
                InvoiceBank = item.InvoiceBank,
                BilledName = item.BilledName,
                ContactName = string.Join(" ,", param.Contact?.Where(x => x.PriceId == item.Id).Select(x => x.Name)),
                BillingAddress = item.BillingAddress,
                InvoiceDigitalNo = item.InvoiceDigitalNo,
                InvoiceNoPrefix = item.InvoiceNoPrefix,
                InvoiceOffice = item.InvoiceOffice,
                PaymentTypeValue = item.PaymentTypeValue,
                PaymentTerms = item.PaymentTerms,
                InspectionFee = item.InspectionFee,
                TravelExpense = item.TravelExpense,
                Discount = item.Discount,
                OtherFee = item.OtherFee,
                TariffType = item.TariffType,
                CreatedByName = item.CreatedByName,
                CreatedOn = item.CreatedOn.ToString(StandardDateFormat),
                UpdatedByName = item.UpdatedByName,
                UpdatedOn = item.UpdatedOn?.ToString(StandardDateFormat)
            };
        }

        /// <summary>
        /// Map Customer price sampling related entity for insert
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CuPrSampling MapInsertCustomerSampling(CustomerPriceCard request)
        {
            CuPrSampling entity = new CuPrSampling()
            {
                MaxProductCount = request.MaxProductCount,
                SampleSizeBySet = request.SampleSizeBySet,
                MinBillingDay = request.MinBillingDay,
                MaxSampleSize = request.MaxSampleSize,
                AdditionalSampleSize = request.AdditionalSampleSize,
                AdditionalSamplePrice = request.AdditionalSamplePrice,
                Quantity8 = request.Quantity8,
                Quantity13 = request.Quantity13,
                Quantity20 = request.Quantity20,
                Quantity32 = request.Quantity32,
                Quantity50 = request.Quantity50,
                Quantity80 = request.Quantity80,
                Quantity125 = request.Quantity125,
                Quantity200 = request.Quantity200,
                Quantity315 = request.Quantity315,
                Quantity500 = request.Quantity500,
                Quantity800 = request.Quantity800,
                Quantity1250 = request.Quantity1250,
                Active = true
            };

            return entity;
        }

        public InvTranInvoiceRequest MapInvoiceRequest(PriceInvoiceRequest request)
        {
            InvTranInvoiceRequest entity = new InvTranInvoiceRequest()
            {
                BilledAddress = request.BilledAddress,
                BilledName = request.BilledName,
                BrandId = request.BrandId,
                BuyerId = request.BuyerId,
                DepartmentId = request.DepartmentId,
                ProductCategoryId = request.ProductCategoryId,
                Active = true,
                CreatedOn = DateTime.Now
            };

            return entity;
        }

        /// <summary>
        /// Map Invoice Request Contacts
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public InvTranInvoiceRequestContact MapInvoiceRequestContact(PriceInvoiceRequestContact request)
        {
            InvTranInvoiceRequestContact entity = new InvTranInvoiceRequestContact()
            {
                ContactId = request.ContactId,
                InvoiceRequestId = null,
                IsCommon = request.IsCommon,
                Active = true,
                CreatedOn = DateTime.Now,
                CuPriceCardId = request.CuPriceCardId
            };

            return entity;
        }

        /// <summary>
        /// Map sampling related entity for update
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public CuPrSampling MapUpdateCustomerSampling(CustomerPriceCard request, CuPrSampling entity)
        {
            entity.MaxProductCount = request.MaxProductCount;
            entity.SampleSizeBySet = request.SampleSizeBySet;
            entity.MinBillingDay = request.MinBillingDay;
            entity.MaxSampleSize = request.MaxSampleSize;
            entity.AdditionalSampleSize = request.AdditionalSampleSize;
            entity.AdditionalSamplePrice = request.AdditionalSamplePrice;
            entity.Quantity8 = request.Quantity8;
            entity.Quantity13 = request.Quantity13;
            entity.Quantity20 = request.Quantity20;
            entity.Quantity32 = request.Quantity32;
            entity.Quantity50 = request.Quantity50;
            entity.Quantity80 = request.Quantity80;
            entity.Quantity125 = request.Quantity125;
            entity.Quantity200 = request.Quantity200;
            entity.Quantity315 = request.Quantity315;
            entity.Quantity500 = request.Quantity500;
            entity.Quantity800 = request.Quantity800;
            entity.Quantity1250 = request.Quantity1250;
            return entity;
        }

        /// <summary>
        /// Map sampling related entity for edit fetch
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="customerPriceCard"></param>
        /// <returns></returns>
        public CustomerPriceCard MapCustomerSampling(CuPrSampling entity, CustomerPriceCard customerPriceCard)
        {
            customerPriceCard.MaxProductCount = entity.MaxProductCount;
            customerPriceCard.SampleSizeBySet = entity.SampleSizeBySet;
            customerPriceCard.MinBillingDay = entity.MinBillingDay;
            customerPriceCard.MaxSampleSize = entity.MaxSampleSize;
            customerPriceCard.AdditionalSampleSize = entity.AdditionalSampleSize;
            customerPriceCard.AdditionalSamplePrice = entity.AdditionalSamplePrice;
            customerPriceCard.Quantity8 = entity.Quantity8;
            customerPriceCard.Quantity13 = entity.Quantity13;
            customerPriceCard.Quantity20 = entity.Quantity20;
            customerPriceCard.Quantity32 = entity.Quantity32;
            customerPriceCard.Quantity50 = entity.Quantity50;
            customerPriceCard.Quantity80 = entity.Quantity80;
            customerPriceCard.Quantity125 = entity.Quantity125;
            customerPriceCard.Quantity200 = entity.Quantity200;
            customerPriceCard.Quantity315 = entity.Quantity315;
            customerPriceCard.Quantity500 = entity.Quantity500;
            customerPriceCard.Quantity800 = entity.Quantity800;
            customerPriceCard.Quantity1250 = entity.Quantity1250;
            return customerPriceCard;
        }

        /// <summary>
        /// Map customer price card for edit fetch
        /// </summary>
        /// <param name="customerPriceCardRepo"></param>
        /// <returns></returns>
        public CustomerPriceCard MapCustomerPriceCard(CustomerPriceCardRepo customerPriceCardRepo, ExportMapRequest list)
        {
            CustomerPriceCard customerPriceCard = new CustomerPriceCard()
            {
                TariffTypeId = customerPriceCardRepo.TariffTypeId,
                BillingMethodId = customerPriceCardRepo.BillingMethodId,
                BillingToId = customerPriceCardRepo.BillingToId,
                CustomerId = customerPriceCardRepo.CustomerId,
                CurrencyId = customerPriceCardRepo.CurrencyId,
                Remarks = customerPriceCardRepo.Remarks,
                FreeTravelKM = customerPriceCardRepo.FreeTravelKM.GetValueOrDefault(),
                Id = customerPriceCardRepo.Id,
                ServiceId = customerPriceCardRepo.ServiceId,
                TaxIncluded = customerPriceCardRepo.TaxIncluded,
                TravelIncluded = customerPriceCardRepo.TravelIncluded,
                UnitPrice = customerPriceCardRepo.UnitPrice,
                SupplierIdList = list.SupData.Select(x => x.Id).ToList(),
                ProductCategoryIdList = list.ProductCategory.Select(x => x.Id).ToList(),
                ProductSubCategoryIdList = list.ProductSubCategory.Select(x => x.Id).ToList(),
                ServiceTypeIdList = list.ServiceType.Select(x => x.Id).ToList(),
                FactoryCountryIdList = list.CountryData.Select(x => x.Id).ToList(),
                FactoryProvinceIdList = list.ProvinceData.Select(x => x.Id).ToList(),
                FactoryCityIdList = list.CityData.Select(x => x.Id).ToList(),
                DepartmentIdList = list.DeptData.Select(x => x.Id).ToList(),
                BrandIdList = list.BrandData.Select(x => x.Id).ToList(),
                BuyerIdList = list.BuyerData.Select(x => x.Id).ToList(),
                PriceCategoryIdList = list.PriceCategory.Select(x => x.Id).ToList(),
                HolidayTypeIdList = list.HolidayType.Select(x => x.Id).ToList(),
                InspectionLocationList = list.InspectionLocation.Select(x => x.Id).ToList(),

                PriceToEachProduct = customerPriceCardRepo.PriceToEachProduct,
                ProductPrice = customerPriceCardRepo.ProductPrice,
                PeriodFrom = customerPriceCardRepo.PeriodFrom,
                PeriodTo = customerPriceCardRepo.PeriodTo,
                HolidayPrice = customerPriceCardRepo.HolidayPrice,

                InvoiceRequestSelectAll = customerPriceCardRepo.InvoiceRequestSelectAll,
                IsInvoiceConfigured = customerPriceCardRepo.IsInvoiceConfigured,

                InvoiceRequestType = customerPriceCardRepo.InvoiceRequestType,
                PriceComplexType= customerPriceCardRepo.PriceComplexType,
                InvoiceRequestAddress = customerPriceCardRepo.InvoiceRequestAddress,
                InvoiceRequestBilledName = customerPriceCardRepo.InvoiceRequestBilledName,
                InvoiceRequestContact = list.Contact.Select(x => x.Id).Distinct().ToList(),
                InvoiceOffice = customerPriceCardRepo.InvoiceOffice,

                BillingEntity = customerPriceCardRepo.BillingEntity,
                BankAccount = customerPriceCardRepo.BankAccount,
                PaymentDuration = customerPriceCardRepo.PaymentDuration,
                PaymentTerms = customerPriceCardRepo.PaymentTerms,

                InvoiceNoDigit = customerPriceCardRepo.InvoiceNoDigit,
                InvoiceNoPrefix = customerPriceCardRepo.InvoiceNoPrefix,

                MaxProductCount = customerPriceCardRepo.MaxProductCount,
                SampleSizeBySet = customerPriceCardRepo.SampleSizeBySet,
                MinBillingDay = customerPriceCardRepo.MinBillingDay,
                MaxSampleSize = customerPriceCardRepo.MaxSampleSize,
                AdditionalSampleSize = customerPriceCardRepo.AdditionalSampleSize,
                AdditionalSamplePrice = customerPriceCardRepo.AdditionalSamplePrice,
                Quantity8 = customerPriceCardRepo.Quantity8,
                Quantity13 = customerPriceCardRepo.Quantity13,
                Quantity20 = customerPriceCardRepo.Quantity20,
                Quantity32 = customerPriceCardRepo.Quantity32,
                Quantity50 = customerPriceCardRepo.Quantity50,
                Quantity80 = customerPriceCardRepo.Quantity80,
                Quantity125 = customerPriceCardRepo.Quantity125,
                Quantity200 = customerPriceCardRepo.Quantity200,
                Quantity315 = customerPriceCardRepo.Quantity315,
                Quantity500 = customerPriceCardRepo.Quantity500,
                Quantity800 = customerPriceCardRepo.Quantity800,
                Quantity1250 = customerPriceCardRepo.Quantity1250,

                InvoiceInspFeeFrom = customerPriceCardRepo.InvoiceInspFeeFrom,
                InvoiceHotelFeeFrom = customerPriceCardRepo.InvoiceHotelFeeFrom,
                InvoiceDiscountFeeFrom = customerPriceCardRepo.InvoiceDiscountFeeFrom,
                InvoiceOtherFeeFrom = customerPriceCardRepo.InvoiceOtherFeeFrom,
                InvoiceTravelExpense = customerPriceCardRepo.InvoiceTravelExpense,

                MaxFeeStyle = customerPriceCardRepo.MaxFeeStyle,
                BillFrequency = customerPriceCardRepo.BillFrequency,
                BillQuantityType = customerPriceCardRepo.BillQuantityType,
                InterventionType = customerPriceCardRepo.InterventionType,
                InvoiceSubject = customerPriceCardRepo.InvoiceSubject,
                SubCategorySelectAll = customerPriceCardRepo.SubCategorySelectAll,
                IsSpecial = customerPriceCardRepo.IsSpecial,
                MandayReports = customerPriceCardRepo.MandayReports,
                MandayBuffer = customerPriceCardRepo.MandayBuffer,
                MandayProductivity = customerPriceCardRepo.MandayProductivity

            };

            return customerPriceCard;
        }

        /// <summary>
        /// Map Holiday Type Data
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public CustomerPriceHolidayType MapHolidayType(CuPrHolidayType entity)
        {
            CustomerPriceHolidayType holidayType = new CustomerPriceHolidayType()
            {
                Id = entity.Id,
                PriceId = entity.CuPriceId,
                HolidayInfoId = entity.HolidayInfoId.GetValueOrDefault(),
                Active = entity.Active
            };
            return holidayType;
        }
    }
}
