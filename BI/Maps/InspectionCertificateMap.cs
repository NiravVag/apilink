using DTO.Common;
using DTO.Inspection;
using DTO.InspectionCertificate;
using DTO.Report;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BI.Maps
{
    public class InspectionCertificateMap : ApiCommonData
    {
        public List<ICBookingSearchResponse> BookingSearchIC(List<ICBookingSearchRepoResponse> bookingList, List<ICBookingSearchProductResponse> productList,
                                List<ICBookingProductFB> fbList, List<InspIcTranProduct> icProductList, List<FBReportCustomerDecision> customerDecisionList
            , List<CuCheckPoint> customerCheckPointList, IEnumerable<ServiceTypeList> serviceTypeList, IEnumerable<int> loggedUserRoleList)
        {
            List<ICBookingSearchResponse> objICBookingSearchResponseList = new List<ICBookingSearchResponse>();
            if (bookingList?.Count > 0)
            {
                foreach (var bookingitem in bookingList)
                {
                    ICBookingSearchResponse objICBookingSearchResponse = new ICBookingSearchResponse();
                    objICBookingSearchResponse.ProductList = new List<ICBookingSearchProductResponse>();
                    objICBookingSearchResponse.BusinessLine = bookingitem.BusinessLine;
                    var productData = productList.Where(x => x.BookingId == bookingitem.BookingNumber).ToList();

                    // Check CustomerDecision checkpoint for customer
                    bool isCustomerDecisionRequired = customerCheckPointList.Where(x => x.CustomerId == bookingitem.CustomerId).Any();

                    foreach (var productItem in productData)
                    {
                        ICBookingSearchProductResponse objICBookingSearchProductResponse = new ICBookingSearchProductResponse();

                        ICBookingProductFB fbData = null;
                        if (bookingitem.BusinessLine == (int)BusinessLine.HardLine)
                        {
                            fbData = fbList.Where(x => x.inspoTransid == productItem.InspPOTransactionId && x.FBReportId == productItem.FBReportId).FirstOrDefault();
                        }
                        else if (bookingitem.BusinessLine == (int)BusinessLine.SoftLine)
                        {
                            fbData = fbList.Where(x => x.InsPOColorTransId == productItem.PoColorId && x.FBReportId == productItem.FBReportId).FirstOrDefault();
                        }

                        //remaining qty logic
                        var ICQty = icProductList.Where(x => x.BookingProductId == productItem.InspPOTransactionId && x.ShipmentQty.HasValue).
                                                                            Sum(x => x.ShipmentQty.Value);

                        objICBookingSearchProductResponse.PONo = productItem.PONo;
                        objICBookingSearchProductResponse.POId = productItem.POId;
                        objICBookingSearchProductResponse.ProductCode = productItem.ProductCode;
                        objICBookingSearchProductResponse.ProductDescription = productItem.ProductDescription;
                        objICBookingSearchProductResponse.DestinationCountry = productItem.DestinationCountry;
                        objICBookingSearchProductResponse.Unit = productItem.Unit;
                        objICBookingSearchProductResponse.Color = productItem.Color;
                        objICBookingSearchProductResponse.ColorCode = productItem.ColorCode;
                        objICBookingSearchProductResponse.PoColorId = productItem.PoColorId;
                        objICBookingSearchProductResponse.InspPOTransactionId = productItem.InspPOTransactionId;

                        if (fbData != null && fbData?.FBStatus != null && fbData.FBStatus == (int)FBStatus.ReportValidated)
                        {
                            // CustomerDecision checkpoint true and customer decision status pass for the report  to enable the checkbox
                            objICBookingSearchProductResponse.EnableCheckbox = (loggedUserRoleList != null && loggedUserRoleList.Any() &&
                                                                                loggedUserRoleList.Contains((int)RoleEnum.InspectionCertificate) &&
                                                                                isCustomerDecisionRequired && customerDecisionList?.Where(x => x.ReportId ==
                                                                                productItem.FBReportId && x.CustomerDecisionId == (int)InspCustomerDecisionEnum.Pass)?
                                                                                .Count() > 0);

                            objICBookingSearchProductResponse.ShipmentQty = 0;
                            objICBookingSearchProductResponse.TotalICQty = ICQty;
                            objICBookingSearchProductResponse.PresentedQty = fbData.FBPresentedQty;

                            //remaining qty logic
                            if (fbData != null && fbData.FBPresentedQty > 0)
                            {
                                objICBookingSearchProductResponse.RemainingQty = fbData.FBPresentedQty - ICQty;

                                //reamining qty is greater than 0 we should have add the product to list or else that product already issued IC for all quantity
                                if (objICBookingSearchProductResponse.RemainingQty > 0)
                                    objICBookingSearchResponse.ProductList.Add(objICBookingSearchProductResponse);
                            }
                            else
                                objICBookingSearchResponse.ProductList.Add(objICBookingSearchProductResponse);
                        }
                        else
                            objICBookingSearchResponse.ProductList.Add(objICBookingSearchProductResponse);
                    }
                    //no product for the booking 
                    if (objICBookingSearchResponse.ProductList != null && objICBookingSearchResponse.ProductList.Any() &&
                            objICBookingSearchResponse.ProductList.Any(x => x.EnableCheckbox))
                    {
                        objICBookingSearchResponse.IsExpand = false;
                        objICBookingSearchResponse.CustomerName = bookingitem.Customer?.CustomerName;
                        objICBookingSearchResponse.ServiceFromDate = bookingitem.ServiceFromDate.ToString(StandardDateFormat);
                        objICBookingSearchResponse.ServiceToDate = bookingitem.ServiceToDate.ToString(StandardDateFormat);
                        objICBookingSearchResponse.SupplierName = bookingitem.SupplierName;
                        objICBookingSearchResponse.BookingNumber = bookingitem.BookingNumber;
                        objICBookingSearchResponse.FactoryName = bookingitem.FactoryName;
                        objICBookingSearchResponse.CustomerId = bookingitem.CustomerId;
                        objICBookingSearchResponse.SupplierId = bookingitem.SupplierId;
                        objICBookingSearchResponse.BookingStatus = bookingitem.BookingStatus;
                        objICBookingSearchResponse.ServiceType = serviceTypeList.Where(x => x.InspectionId == bookingitem.BookingNumber).Select(x => x.serviceTypeName).FirstOrDefault();

                        //IC role should have and atleast one product have remaining qty and the product checkbox enable
                        objICBookingSearchResponse.EnableCheckbox = loggedUserRoleList != null && loggedUserRoleList.Any()
                                                                    && loggedUserRoleList.Contains((int)RoleEnum.InspectionCertificate) &&
                                                                    objICBookingSearchResponse.ProductList.Where(x => x.RemainingQty > 0 && x.EnableCheckbox).Any();
                        //
                        objICBookingSearchResponse.ProductList = objICBookingSearchResponse.ProductList.OrderByDescending(x => x.EnableCheckbox).ToList();

                        objICBookingSearchResponseList.Add(objICBookingSearchResponse);
                    }
                }
            }
            return objICBookingSearchResponseList;
        }

        public List<ICItem> ICSummarySearchMap(List<ICItemRepo> ICDataList)
        {
            List<ICItem> ICItemList = new List<ICItem>();

            var lstIcid = ICDataList.Select(x => x.ICId).Distinct();

            foreach (int icid in lstIcid)
            {
                var icrepoitem = ICDataList.FirstOrDefault(x => x.ICId == icid);
                var lstservicedate = ICDataList.Where(x => x.ICId == icid);
                ICItemList.Add(new ICItem()
                {
                    BookingNo = string.Join(", ", ICDataList.Where(x => x.ICId == icid).Select(x => x.BookingNo).Distinct()),
                    Customer = icrepoitem.Customer,
                    ICId = icid,
                    ICNo = icrepoitem.ICNo,
                    ServiceDate = string.Join(", ", GetServiceDate(lstservicedate).Distinct()),
                    StatusId = icrepoitem.StatusId,
                    StatusName = icrepoitem.StatusName,
                    Supplier = icrepoitem.Supplier,
                    BuyerName = icrepoitem.BuyerName ?? icrepoitem.Customer
                });
            }
            return ICItemList;
            //foreach (var icData in ICDataList)
            //{
            //    var poTransactionIDs = icData.InspIcTranProducts.Select(x => x.BookingProductId);
            //    var inspTransactions = inspTransactionList.Where(x => poTransactionIDs.Contains(x.PoTransactionId));
            //    if (inspTransactions != null && inspTransactions.Any())
            //    {
            //        ICItem icItem = new ICItem();
            //        var inspTransaction = inspTransactions.FirstOrDefault();
            //        icItem.ICId = icData.IcId;
            //        icItem.ICNo = icData.IcNo;
            //        icItem.Customer = inspTransaction?.CustomerName;
            //        icItem.Supplier = inspTransaction?.SupplierName;
            //        icItem.BookingNo = string.Join(", ", inspTransactions.Select(x => x.BookingNumber).Distinct());
            //        icItem.ServiceDate = string.Join(", ", GetServiceDate(inspTransactions).Distinct());
            //        icItem.StatusId = icData.StatusId;
            //        icItem.StatusName = icData.StatusName;
            //        ICItemList.Add(icItem);
            //    }
            //}


        }

        public List<string> GetServiceDate(IEnumerable<ICItemRepo> inspectionTransactionList)
        {
            List<string> serviceDates = new List<string>();
            foreach (var inspTransaction in inspectionTransactionList)
            {
                if (inspTransaction.ServiceDateFrom == inspTransaction.ServiceDateTo)
                    serviceDates.Add(inspTransaction.ServiceDateFrom.ToString(StandardDateFormat));
                else
                    serviceDates.Add("(" + inspTransaction.ServiceDateFrom.ToString(StandardDateFormat) + "-"
                                            + inspTransaction.ServiceDateTo.ToString(StandardDateFormat) + ")");
            }

            return serviceDates;
        }

        public ICStatus GetICStatusList(InspIcStatus entity)
        {
            if (entity == null)
                return null;
            return new ICStatus
            {
                id = entity.Id,
                name = entity.StatusName,
            };
        }

        public List<QuantityPoId> RemainingQtyCalculationMap(List<QuantityPoId> productICQtyList, List<QuantityPoId> fbPresentedQtyList)
        {
            List<QuantityPoId> remaingQtyList = new List<QuantityPoId>();

            foreach (var fbqty1 in fbPresentedQtyList)
            {
                var icqty = productICQtyList?.Where(x => x.InspPoTransactionId == fbqty1.InspPoTransactionId).FirstOrDefault();
                remaingQtyList.Add(new QuantityPoId()
                {
                    InspPoTransactionId = fbqty1.InspPoTransactionId,
                    Quantity = (fbqty1?.Quantity) - (icqty?.Quantity ?? 0),
                    PresentedQty = fbqty1?.Quantity ?? 0,
                    TotalICQty = icqty?.Quantity ?? 0
                });
            }

            return remaingQtyList;
        }
        public List<InspectionCertificateBookingRequest> RemainingQtyMap(List<InspectionCertificateBookingRequest> icProductList, List<QuantityPoId> remainingQtyList)
        {
            foreach (var icProduct in icProductList)
            {
                //remove the current shipmentqty from total ic qty
                var totalicqty = remainingQtyList?.Where(x => x.InspPoTransactionId == icProduct.BookingProductId).Select(x => x.TotalICQty).FirstOrDefault() - icProduct.ShipmentQty ?? 0;

                icProduct.RemainingQty = remainingQtyList?.Where(x => x.InspPoTransactionId == icProduct.BookingProductId).Select(x => x.Quantity).FirstOrDefault();
                icProduct.TotalICQty = totalicqty;
                icProduct.PresentedQty = remainingQtyList?.Where(x => x.InspPoTransactionId == icProduct.BookingProductId).Select(x => x.PresentedQty).FirstOrDefault() ?? 0;
            }
            return icProductList;
        }
        //product table map from pending ic 
        public List<ProductBookingICResponse> BookingProductMapIC(List<int> bookingIdList, int businessLine, List<ICBookingSearchProductResponse> productList,
                                                                          List<ICBookingProductFB> fbList, List<InspIcTranProduct> icProductList)
        {
            List<ProductBookingICResponse> objICBookingProductResponseList = new List<ProductBookingICResponse>();
            if (bookingIdList != null)
            {
                foreach (var bookingId in bookingIdList)
                {

                    var productData = productList.Any() ? productList.Where(x => x.BookingId == bookingId).ToList()
                                        : new List<ICBookingSearchProductResponse>();
                    foreach (var productItem in productData)
                    {
                        ProductBookingICResponse objICBookingProductResponse = new ProductBookingICResponse();
                        objICBookingProductResponse.BookingNumber = bookingId;

                        ICBookingProductFB fbData = null;
                        if (businessLine == (int)BusinessLine.HardLine)
                        {
                            fbData = fbList.Where(x => x.inspoTransid == productItem.InspPOTransactionId && x.FBReportId == productItem.FBReportId).FirstOrDefault();
                        }
                        else if (businessLine == (int)BusinessLine.SoftLine)
                        {
                            fbData = fbList.Where(x => x.InsPOColorTransId == productItem.PoColorId && x.FBReportId == productItem.FBReportId).FirstOrDefault();
                        }

                        //remaining qty logic
                        var ICQty = icProductList.Where(x => x.BookingProductId == productItem.InspPOTransactionId && x.ShipmentQty.HasValue).
                                                                            Sum(x => x.ShipmentQty.Value);

                        if (fbData != null && fbData?.FBStatus != null && fbData.FBStatus == (int)FBStatus.ReportValidated)
                        {
                            objICBookingProductResponse.ShipmentQty = 0;
                            objICBookingProductResponse.TotalICQty = ICQty;
                            objICBookingProductResponse.PresentedQty = fbData.FBPresentedQty;
                            //remaining qty logic
                            if (fbData != null && fbData.FBPresentedQty > 0)
                            {
                                objICBookingProductResponse.RemainingQty = fbData.FBPresentedQty - ICQty;
                            }
                        }

                        objICBookingProductResponse.PONo = productItem.PONo;
                        objICBookingProductResponse.POId = productItem.POId;
                        objICBookingProductResponse.ProductCode = productItem.ProductCode;
                        objICBookingProductResponse.ProductDescription = productItem.ProductDescription;
                        objICBookingProductResponse.DestinationCountry = productItem.DestinationCountry;
                        objICBookingProductResponse.Unit = productItem.Unit;
                        objICBookingProductResponse.BookingProductId = productItem.InspPOTransactionId;
                        objICBookingProductResponse.Color = productItem.Color;
                        objICBookingProductResponse.ColorCode = productItem.ColorCode;
                        objICBookingProductResponse.PoColorId = productItem.PoColorId;
                        objICBookingProductResponse.BusinessLine = productItem.BusinessLine;
                        objICBookingProductResponseList.Add(objICBookingProductResponse);
                    }
                }
            }
            return objICBookingProductResponseList;
        }
    }
}
