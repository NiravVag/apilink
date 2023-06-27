using DTO.Common;
using DTO.CommonClass;
using DTO.FBInternalReport;
using DTO.Inspection;
using DTO.Report;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BI.Maps
{
    public class FBInternalReportMap : ApiCommonData
    {
        public InternalFBReportItem GetInspectionReportResult(InternalReportBookingValues entity, IEnumerable<InspectionStatus> inspectionStatus, IEnumerable<ServiceTypeList> serviceTypeList)
        {
            return new InternalFBReportItem()
            {
                BookingId = entity.BookingId,
                ReportNo = entity.FbReportId,
                CustomerBookingNo = entity.CustomerBookingNo,
                CustomerId = entity.CustomerId,
                CustomerName = entity.CustomerName,
                FactoryName = entity.SupplierName,
                ServiceDateFrom = entity?.ServiceDateFrom.ToString(StandardDateFormat),
                ServiceDateTo = entity?.ServiceDateTo.ToString(StandardDateFormat),
                SupplierName = entity?.SupplierName,
                ServiceType = serviceTypeList.Where(x => x.InspectionId == entity.BookingId).Select(x => x.serviceTypeName).FirstOrDefault(),
                StatusId = entity.StatusId,
                StatusName = inspectionStatus?.Where(x => x.Id == entity.StatusId)?.Select(x => x.StatusName)?.FirstOrDefault(),
                IsPicking = false,
                PreviousBookingNo = entity.PreviousBookingNo,
                FactoryId = entity.FactoryId,
                SupplierId = entity.SupplierId
            };
        }

        private string GetResultTextColor(string title)
        {
            if (title != null)
            {
                if (title.ToLower() == "pass")
                {
                    return ReportResult.Limegreen.ToString();
                }
                else if (title.ToLower() == "fail")
                {
                    return ReportResult.Red.ToString();
                }
                else
                {
                    return ReportResult.Orange.ToString();
                }
            }
            return null;
        }

        public InternalReportProductItem GetProductList(InternalReportProducts products, FBReportDetails reportList, IEnumerable<InternalReportProducts> productList)
        {
            //if the products are combined and AQL is not selected, make the first product in the list as parent product
            var isAQLSelected = productList.Where(z => z.CombineProductId == products.CombineProductId && z.CombineAqlQuantity.GetValueOrDefault() > 0).Count();

            return new InternalReportProductItem()
            {
                bookingId = products.BookingId,
                ProductId = products.ProductId,
                ProductName = products.ProductName,
                ProductDescription = products.ProductDescription,
                ProductQuantity = products.ProductQuantity,
                ProductCategoryName = products.ProductCategoryName,
                ProductSubCategoryName = products.ProductSubCategoryName,
                FbReportId = products.FbReportId,
                ColorCode = ReportResult.FFFF.ToString(),
                CombineProductId = products.CombineProductId,
                ReportStatus = products.FbReportId > 0 ? reportList.StatusName : "",
                FillingStatus = products.FbReportId > 0 ? reportList.FillingStatus : "",
                ReviewStatus = products.FbReportId > 0 ? reportList.ReviewStatus : "",
                PONumber = products.PONumber,
                InspectedQuantity = products.FbReportId > 0 ? reportList.InspectedQuantity : null,
                finalReportLink = products.FbReportId > 0 ? reportList.FinalReportPath : "",
                CombineProductCount = products.CombineProductId > 0 ? productList.Where(x => x.CombineProductId == products.CombineProductId).Count() : 1,
                //IsParentProduct = (products.CombineProductId.GetValueOrDefault() == 0) ? true : (products.CombineAqlQuantity != null && products.CombineAqlQuantity != 0) ? true : false,
                IsParentProduct = (products.CombineProductId.GetValueOrDefault() == 0) ? true : (products.CombineAqlQuantity != null &&
                    products.CombineAqlQuantity != 0) ? true : (isAQLSelected == 0 && productList.Where(z => z.CombineProductId == products.CombineProductId).FirstOrDefault().ProductId == products.ProductId ? true : false),
                Result = products.FbReportId > 0 ? reportList.OverAllResult : null,
                ReportTitle = products.FbReportId > 0 ? reportList.ReportTitle : "",
                ResultColor = GetResultTextColor(products.FbReportId > 0 ? reportList.OverAllResult : null),
                ContainerName = products.ContainerId != null ? InspectionServiceTypeData.GetValueOrDefault((int)InspectionServiceTypeEnum.Container) + products.ContainerId : ""
            };
        }

        public InternalFBReportItem MapInternalProductsToBooking(InternalFBReportItem entity, IEnumerable<InternalReportProductItem> products)
        {
            return new InternalFBReportItem()
            {
                BookingId = entity.BookingId,
                CustomerBookingNo = entity.CustomerBookingNo,
                CustomerId = entity.CustomerId,
                CustomerName = entity.CustomerName,
                FactoryName = entity.SupplierName,
                ServiceDateFrom = entity?.ServiceDateFrom,
                ServiceDateTo = entity?.ServiceDateTo,
                SupplierName = entity?.SupplierName,
                ServiceType = entity.ServiceType,
                Office = entity.Office,
                StatusId = entity.StatusId,
                IsPicking = false,
                PreviousBookingNo = entity.PreviousBookingNo,
                FactoryId = entity.FactoryId,
                ReportProducts = products.Where(x => x.bookingId == entity.BookingId).Select(x => new InternalReportProductItem
                {
                    bookingId = x.bookingId,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    ProductDescription = x.ProductDescription,
                    ProductQuantity = x.ProductQuantity,
                    ProductCategoryName = x.ProductCategoryName,
                    ProductSubCategoryName = x.ProductSubCategoryName,
                    FbReportId = x.FbReportId,
                    CombineProductId = x.CombineProductId,
                    ReportStatus = x.ReportStatus,
                    FillingStatus = x.FillingStatus,
                    ReviewStatus = x.ReviewStatus,
                    PONumber = x.PONumber,
                    InspectedQuantity = x.InspectedQuantity,
                    finalReportLink = x.finalReportLink,
                    Result = x.Result,
                    ReportTitle = x.ReportTitle,
                    ContainerName = x.ContainerName
                })
            };
        }

        public QCInspectionDetailsPDF MapQCInspectionDetailPDF(QCInspectionDetailsRepo qcInspectionDetails, List<QCInspectionProductDetails> qcInspectionProductDetails,
                                                    string qcName, List<BookingContainer> containerList, List<CommonDataSource> brandList,
                                                    List<CommonDataSource> deptList, List<CommonDataSource> inspectionCsList)
        {
            QCInspectionDetailsPDF qcInspectionDetailsPDF = new QCInspectionDetailsPDF();
            qcInspectionDetailsPDF.Customer = qcInspectionDetails.Customer;
            qcInspectionDetailsPDF.InspectionID = qcInspectionDetails.InspectionID;
            qcInspectionDetailsPDF.Supplier = qcInspectionDetails.Supplier;
            qcInspectionDetailsPDF.Factory = qcInspectionDetails.Factory;
            qcInspectionDetailsPDF.CustomerBookingNo = qcInspectionDetails.CustomerBookingNo;
            //qcInspectionDetailsPDF.FactoryContact = string.Join("|", qcInspectionDetails.FactoryContacts.
            //                    Select(x => x.ContactName));
            qcInspectionDetailsPDF.FactoryContact = MapContactNamePhone(qcInspectionDetails.FactoryContacts);
            //qcInspectionDetailsPDF.FactoryContactPhoneNo = string.Join("|", qcInspectionDetails.FactoryContacts.
            //                    Select(x => x.Phone));
            qcInspectionDetailsPDF.FactoryPhoneNo = qcInspectionDetails.FactoryPhoneNo;
            qcInspectionDetailsPDF.QCName = qcName;
            qcInspectionDetailsPDF.Comments = String.Concat(qcInspectionDetails.ScheduleComments, " ", qcInspectionDetails.QCBookingComments);
            qcInspectionDetailsPDF.FactoryAddress = qcInspectionDetails.FactoryAddress;
            qcInspectionDetailsPDF.FactoryRegionalAddress = qcInspectionDetails.FactoryRegionalAddress;
            qcInspectionDetailsPDF.ServiceType = qcInspectionDetails.ServiceTypeName;
            if (qcInspectionDetails.ServiceDateFrom == qcInspectionDetails.ServiceDateTo)
                qcInspectionDetailsPDF.ServiceDate = qcInspectionDetails.ServiceDateFrom.ToString(StandardDateFormat);
            else
                qcInspectionDetailsPDF.ServiceDate = string.Concat(qcInspectionDetails.ServiceDateFrom.ToString(StandardDateFormat), "-",
                                                    qcInspectionDetails.ServiceDateTo.ToString(StandardDateFormat));
            if (qcInspectionProductDetails != null)
            {
                if (qcInspectionDetails.InspectionServiceTypes.Any(x => x.ServiceTypeId == (int)InspectionServiceTypeEnum.Container))
                {
                    qcInspectionDetailsPDF.TotalNumberofReports = containerList.Select(x => x.ContainerId).Distinct().Count();
                }
                else
                {
                    qcInspectionDetailsPDF.TotalNumberofReports = qcInspectionProductDetails.Where(x => x.CombineProductId == null).Select(x => x.ProductName).Distinct().Count() +
                                                                            qcInspectionProductDetails.Where(x => x.CombineProductId != null).
                                                                            Select(x => x.CombineProductId).Distinct().Count();
                }
                if (qcInspectionDetails.QuQuotationInspMandays != null && qcInspectionDetails.QuQuotationInspMandays.Any())
                {
                    qcInspectionDetailsPDF.NoofManDays = qcInspectionDetails.QuQuotationInspMandays.
                                                                Where(x => qcInspectionDetails.QuotationId == x.IdQuotation && x.NoOfManDay.HasValue).Select(x => x.NoOfManDay.Value).FirstOrDefault();
                }
                qcInspectionDetailsPDF.TotalCombineProducts = qcInspectionProductDetails.Where(x => x.CombineProductId != null).
                                                                                Select(x => x.CombineProductId).Distinct().Count();
                qcInspectionDetailsPDF.ProductDetails = qcInspectionProductDetails;
                qcInspectionDetailsPDF.BrandNames = string.Join(", ", brandList?.Select(x => x.Name).ToList());
                qcInspectionDetailsPDF.DepartmentNames = string.Join(", ", deptList?.Select(x => x.Name).ToList());
                qcInspectionDetailsPDF.CollectionName = qcInspectionDetails?.CollectionName != null ? qcInspectionDetails?.CollectionName : "";
                qcInspectionDetailsPDF.TotalSamplingSizeNonCombined = qcInspectionDetails.TotalNonCombineAQLQuantity;
                qcInspectionDetailsPDF.TotalPickingQtyNoncombined = qcInspectionProductDetails.Where(x => !(x.CombineProductId > 0)).Sum(x => x.Picking.GetValueOrDefault());
                qcInspectionDetailsPDF.CsNames = string.Join(", ", inspectionCsList?.Select(x => x.Name).ToList());
                qcInspectionDetailsPDF.BussinessLine = qcInspectionDetails.BussinessLine;
                
            }
            return qcInspectionDetailsPDF;
        }

        public QcPickingData MapPickingProducts(PickingLabAddressItem entity, IEnumerable<QcPickingItem> bookingData, IEnumerable<PickingProductData> products,
           IEnumerable<CustomerCSLocation> CSLocation, string qcName)
        {
            var bookingDetails = bookingData.Where(x => x.PoTransId == entity.PoTransId).FirstOrDefault();

            IEnumerable<string> CSName = entity != null ? CSLocation.Where(x => x.CustomerId == bookingDetails?.CustomerId &&
                                x.LocationList.Select(y => y.LocationId).ToList().Contains(bookingDetails.OfficeId.GetValueOrDefault())).Select(x => x.CsName).Distinct() : null;
            var csPhone = CSLocation != null ? CSLocation.Where(x => x.CustomerId == bookingDetails?.CustomerId &&
                                x.LocationList.Select(y => y.LocationId).ToList().Contains(bookingDetails.OfficeId.GetValueOrDefault())).Select(x => x.CscompanyPhone).Distinct() : null;

            var productList = entity.Lab ? products.Where(x => x.isLab && x.AddressId == entity.AddressId) : products.Where(x => !x.isLab && x.AddressId == entity.AddressId);
            //var productList = products.Where(x => x.AddressId == entity.AddressId );

            if (bookingDetails != null)
            {
                return new QcPickingData
                {
                    BookingId = bookingDetails.BookingId,
                    CustomerBookingNo = bookingDetails.CustomerBookingNo,
                    CustomerName = bookingDetails.CustomerName,
                    SupplierName = bookingDetails.SupplierName,
                    FactoryName = bookingDetails.FactoryName,
                    LabName = entity.LabName,
                    LabAddress = entity.LabAddress,
                    ContactName = string.Join(", ", entity.ContactName),
                    Telephone = string.Join(", ", entity.Telephone),
                    Email = string.Join(", ", entity.Email),
                    CsName = string.Join(", ", CSName),
                    CsPhone = string.Join(", ", csPhone),
                    ServiceDate = bookingDetails.ServiceDateFrom != bookingDetails.ServiceDateTo ? bookingDetails.ServiceDateFrom + "-" + bookingDetails.ServiceDateTo : bookingDetails.ServiceDateFrom,
                    StaffName = qcName != null ? qcName : "",
                    RegionalAddress = entity.RegionalAddress,
                    Products = productList.Select(x => new PickingProductData
                    {
                        ProductId = x.ProductId,
                        PickingQuantity = x.PickingQuantity,
                        PONumber = x.PONumber,
                        DestinationCountry = x.DestinationCountry,
                        FactoryReference = x.FactoryReference
                    })
                };
            }

            return new QcPickingData();
        }

        private string MapContactNamePhone(IEnumerable<InspectionSupplierFactoryContacts> contactList)
        {
            String contactNamePhone = null;
            foreach (var item in contactList)
            {
                var name = item.ContactName + (!string.IsNullOrEmpty(item.Phone) ? "(" + item.Phone + ")" : "");
                contactNamePhone = !string.IsNullOrEmpty(contactNamePhone) ? contactNamePhone + ", " + name : name;
            }

            return contactNamePhone;
        }
    }
}
