using DTO.Common;
using DTO.CustomReport;
using DTO.Inspection;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Maps
{
    public class InspectionCustomReportMap : ApiCommonData
    {
        public InspectionCustomReportItem InspCustomReportMap(InspTransaction entity, IEnumerable<PoDetails> poDetails, List<BookingBrandAccess> BookingBrandList, List<BookingDeptAccess> BookingDeptList, List<BookingBuyerAccess> BookingBuyerList)
        {
            string ponumber = "";
            string color = "";
            string colorCode = "";

            var bookingId = entity.Id;
            if (poDetails != null && poDetails.Any())
            {
                ponumber = string.Join(",", poDetails.Where(x => x.BookingId == bookingId).Select(x => x.PoNumber).Distinct().ToList());
            }
            var poTransactions = entity.InspPurchaseOrderTransactions.Where(x => x.InspectionId == bookingId).ToList();
            if (poTransactions.Count > 0)
            {
                var inspPOColorTransactions = poTransactions.SelectMany(x => x.InspPurchaseOrderColorTransactions).ToList();
                if (inspPOColorTransactions.Count > 0)
                {
                    color = string.Join(",", inspPOColorTransactions.Select(x => x.ColorName).ToList());
                    colorCode = string.Join(",", inspPOColorTransactions.Select(x => x.ColorCode).ToList());
                }
            }

            var FBReportDetail = entity?.FbReportDetails.FirstOrDefault();
            var bookingItem = new InspectionCustomReportItem()
            {
                InspectionNo = bookingId,
                CustomerId = entity?.CustomerId,
                CustomerName = entity?.Customer?.CustomerName,
                CustomerContacts = string.Join(",", entity.InspTranCuContacts.Where(x => x.Active).Select(x => x.Contact.ContactName).Distinct()),
                PONumber = ponumber,
                Brand = string.Join(",", entity?.InspTranCuBrands.Where(x => x.Active).Select(x => x.Brand?.Name).Distinct()),
                Department = string.Join(",", entity.InspTranCuDepartments.Where(x => x.Active).Select(x => x.Department.Name).Distinct()),
                Buyer = string.Join(",", entity.InspTranCuBuyers.Where(x => x.Active).Select(x => x.Buyer.Name).Distinct()),
                Collection = entity.Collection.Name,
                ServiceType = entity?.InspTranServiceTypes?.Select(x => x.ServiceType?.Name)?.FirstOrDefault(),
                FactoryAddress = entity?.Supplier?.SuAddresses.Select(x => x.Address).FirstOrDefault(),
                Inspection_Date = entity.ServiceDateFrom == entity.ServiceDateTo ? entity.ServiceDateFrom.ToString(StandardDateFormat) : string.Join(" - ", entity.ServiceDateFrom.ToString(StandardDateFormat), entity.ServiceDateTo.ToString(StandardDateFormat)),
                OfficeId = entity.OfficeId,
                Office = entity.Office.LocationName,
                ProductCategory = entity?.ProductCategory?.Name,
                ProductSubCategory = entity?.ProductSubCategory?.Name,
                Season = entity?.Season?.Season?.Name + " " + entity.SeasonYear?.Year.ToString(),
                CustomerProductCategory = entity.CuProductCategoryNavigation.Name,
                ProductRef = string.Join(",", entity?.InspProductTransactions.Where(x => x.Active.HasValue && x.Active.Value).Select(x => x.Product?.ProductId).ToList()),
                ProductDesc = string.Join(",", entity?.InspProductTransactions.Where(x => x.Active.HasValue && x.Active.Value).Select(x => x.Product?.ProductDescription).ToList()),
                Color = color,
                ColorCode = colorCode,
                ETD = string.Join(",", entity?.InspPurchaseOrderTransactions.Select(x => x.Etd).Distinct())

            };
            return bookingItem;
        }
    }
}
