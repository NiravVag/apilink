using DTO.Common;
using DTO.Inspection;
using DTO.MobileApp;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BI.Maps.APP
{
    public class QuotationMobileMap : ApiCommonData
    {
        public List<MobileInspQuotationData> MapQuotationProducts(List<MobilePendingQuotation> quotDataList, IEnumerable<BookingProductsData> productList, List<QuTranStatusLog> quotStatusData, int pageIndex)
        {
            var response = new List<MobileInspQuotationData>();

            //page index - 1 * 10 (pagesize) + 1
            var key = ((pageIndex - 1) * PageSize) + 1;

            foreach (var quotData in quotDataList)
            {
                var product = productList.Where(x => x.BookingId == quotData.BookingId).ToList();
                var sampleQty = product.Where(x => x.CombineProductId > 0).Sum(x => x.CombineAqlQuantity) + product.Where(x => !(x.CombineProductId > 0)).Sum(x => x.AqlQuantity);
                response.Add(new MobileInspQuotationData()
                {
                    key = key++,
                    quotationId = quotData.QuotationId,
                    supplier = quotData.SupplierName,
                    factory = quotData.FactoryName,
                    serviceDate = quotData.ServiceDateFrom == quotData.ServiceDateTo ? string.Format("{0: " + StandardDateFormat3 + "}", quotData.ServiceDateFrom) : string.Format("{0: " + StandardDateFormat3 + "}", quotData.ServiceDateFrom) + " to " + string.Format("{0: " + StandardDateFormat3 + "}", quotData.ServiceDateTo),
                    bookingId = string.Join(" ,", quotDataList.Where(x => x.QuotationId == quotData.QuotationId).Select(x => x.BookingId)),
                    productRef = string.Join(" ,", product?.Select(x => x.ProductName).Distinct()),
                    productDesc = string.Join(" ,", product?.Select(x => x.ProductDescription).Distinct()),
                    bookingQty = product?.Sum(x => x.BookingQuantity),
                    samplingQty = sampleQty,
                    manDay = quotData.ManDay,
                    poNumber = product?.Select(x => x.PoNumber).FirstOrDefault(),
                    unitPrice = quotData.UnitPrice,
                    serviceFee = quotData.ServiceFee,
                    travelAir = quotData.TravelAir,
                    travelLand = quotData.TravelLand,
                    hotel = quotData.Hotel,
                    totalPrice = quotData.TotalPrice,
                    otherCost = quotData.OtherCost,
                    statusId = quotData.QuotationStatusId,
                    isApproved = quotData.QuotationStatusId == (int)QuotationStatus.CustomerValidated,
                    currency = quotData.Currency,
                    combinedProductCount = productList.Count(x => x.BookingId == quotData.BookingId && x.CombineProductId > 0),
                    approvedDate = quotStatusData.Any(x => x.QuotationId == quotData.QuotationId && x.StatusId == (int)QuotationStatus.CustomerValidated) ? string.Format("{0: " + StandardDateFormat3 + "}", quotStatusData.Where(x => x.QuotationId == quotData.QuotationId && x.StatusId == (int)QuotationStatus.CustomerValidated)?.OrderByDescending(x => x.CreatedOn).Select(x => x.StatusChangeDate).FirstOrDefault()) : "",
                    discount = quotData.Discount ?? 0
                });
            }

            return response;
        }
    }
}
