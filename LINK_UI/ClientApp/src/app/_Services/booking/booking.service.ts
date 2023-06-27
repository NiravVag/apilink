import { Injectable, inject, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, first } from 'rxjs/operators';

import { InspectionBookingsummarymodel, HolidayRequest } from "../../_Models/booking/inspectionbookingsummarymodel";
import { Observable, of } from 'rxjs';
import { EditInspectionBooking, InspectionFileAttachment, BookingCustomerContactRequest, ProductValidationResponse, InspectionPickingExistRequest, DraftInspectionBooking, EntPageFieldAccessResponse, EntPageRequest, POProductList, PoProductDetailRequest, POProductDetailResponse } from '../../_Models/booking/inspectionbooking.model';
import { BookingSearchRedirectPage } from '../../components/common/static-data-common';
import { SplitBooking } from 'src/app/_Models/booking/splitbookingmodel';
import { CustomerDecisionSaveRequest } from 'src/app/_Models/booking/inspectioncustomerdecision';
import { CSConfigListResponse, PriceCategoryRequest, UserAccessRequest } from 'src/app/_Models/booking/bookingpreview.model';
import { ProductValidateData } from 'src/app/_Models/booking/productValidate.model';
import { promise } from 'protractor';
import { SaveMasterContactRequest } from 'src/app/_Models/booking/mastercontact.model';
import { ProductList } from 'src/app/_Models/purchaseorder/edit-purchaseorder.model';
import { SaveLabAddressRequestData } from 'src/app/_Models/booking/LabDetailsModel';

@Injectable({
  providedIn: 'root'
})
export class BookingService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  GetCustomerByUserType() {
    return this.http.get<any>(`${this.url}/api/Customer/GetCustomerByUsertType`);
  }

  GetPOByCustomerAndProducts(customerId, productCategoryId, supplierId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/POListByCustomerandProducts/${customerId}/${productCategoryId}/${supplierId}`);
  }

  UpdateBookingStatus(bookingId, statusId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/updatebookigstatus/${bookingId}/${statusId}`);
  }

  /* GetPOProductListByPoNumber(pono, supplierId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/BookingPOProducts/${pono}/${supplierId}`);
  } */

  GetSeasonYear() {
    return this.http.get<any>(`${this.url}/api/reference/GetSeasonYear`);
  }

  GetOffice() {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetBookingOffice`);
  }
  EditBooking(id) {
    if (id == null)
      return this.http.get<any>(`${this.url}/api/InspectionBooking/add`);
    else
      return this.http.get<any>(`${this.url}/api/InspectionBooking/EditBooking/${id}`);
  }
  GetBookingDetailsByCusId(id) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetBookingDetailsByCustomerId/${id}`);
  }
  GetPickingAndCombineOrders(id) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetPickingAndCombineOrders/${id}`);
  }
  GetSupplierDetailsByCusIdSupId(supid, cusid, bookingId) {
    if (cusid != null)
      return this.http.get<any>(`${this.url}/api/InspectionBooking/GetSupplierDetailsById/${cusid},${supid},${bookingId}`);
    else
      return this.http.get<any>(`${this.url}/api/InspectionBooking/GetSupplierDetailsById/${supid}`);
  }
  GetFactoryDetailsByCusIdFactId(factid, cusid, bookingId) {
    if (cusid != null)
      return this.http.get<any>(`${this.url}/api/InspectionBooking/GetFactoryDetailsById/${cusid},${factid},${bookingId}`);
    else
      return this.http.get<any>(`${this.url}/api/InspectionBooking/GetFactoryDetailsById/${factid}`);
  }
  saveBooking(model: EditInspectionBooking) {
    return this.http.post<any>(`${this.url}/api/InspectionBooking/save`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  saveInspectionDraftBooking(model: DraftInspectionBooking) {
    return this.http.post<any>(`${this.url}/api/InspectionBooking/save-draft-inspection-booking`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getInspectionDraftBookings() {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/get-draft-bookings`);
  }

  removeInspectionDraftBookings(draftInspectionBookingId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/remove-draft-booking/${draftInspectionBookingId}`);
  }

  confirmEmailBooking(model: EditInspectionBooking) {
    return this.http.post<any>(`${this.url}/api/InspectionBooking/confirmemail`, model)
      .pipe(map(response => {
        return response;
      }));
  }



  cancelBooking(model: SplitBooking) {
    return this.http.post<any>(`${this.url}/api/InspectionBooking/cancelbooking`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  newBooking(model: SplitBooking) {
    return this.http.post<any>(`${this.url}/api/InspectionBooking/newbooking`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getFile(id) {
    return this.http.get(`${this.url}/api/InspectionBooking/file/${id}`, { responseType: 'blob' });
  }
  getBookingTerms() {
    return this.http.get(`${this.url}/api/InspectionBooking/fileTerms`, { responseType: 'blob' });
  }

  getProductFile(id) {
    return this.http.get(`${this.url}/api/InspectionBooking/productfile/${id}`, { responseType: 'blob' });
  }


  Getinspectionbookingsummary() {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetInspectionBookingsummary`);
  }
  Getsupplierbycusid(cusid) {
    return this.http.get<any>(`${this.url}/api/Supplier/GetsupplierBycustomerid/${cusid}`).pipe(map(response => { return response; }));
  }
  Getfactorybysupplierid(supid) {
    return this.http.get<any>(`${this.url}/api/Supplier/GetfactoryBysupid/${supid}`);
  }
  SearchInspectionBookingSummary(model: InspectionBookingsummarymodel) {
    return this.http.post<any>(`${this.url}/api/InspectionBooking/searchInspection`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  SearchInspectionReportSummary(model: InspectionBookingsummarymodel) {
    return this.http.post<any>(`${this.url}/api/InspectionBooking/SearchInspectionReports`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  UpdateFBBookingDetails(bookingId) {
    return this.http.get<any>(`${this.url}/api/FBReport/UpdateFBBookingDetails/${bookingId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  UpdateFBProductDetails(bookingId) {
    return this.http.get<any>(`${this.url}/api/FBReport/UpdateFBProductDetails/${bookingId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  CreateFBMission(bookingId) {
    return this.http.get<any>(`${this.url}/api/FBReport/CreateFBMission/${bookingId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  DeleteFBMission(bookingId) {
    return this.http.get<any>(`${this.url}/api/FBReport/DeleteFBMission/${bookingId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  CreateFBReport(bookingId, poDetailId) {
    return this.http.get<any>(`${this.url}/api/FBReport/CreateFBReport/${bookingId}/${poDetailId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  DeleteFBReport(bookingId, reportId, apiReportId) {
    return this.http.get<any>(`${this.url}/api/FBReport/DeleteFBReport/${bookingId}/${reportId}/${apiReportId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  fetchFBReport(reportId, apiReportId) {
    return this.http.get<any>(`${this.url}/api/FBReport/FetchFBReport/${reportId}/${apiReportId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  fetchFBReportByBooking(bookingId, fetchOption) {
    return this.http.get<any>(`${this.url}/api/FBReport/fetch-fb-report-by-booking/${bookingId}/${fetchOption}`)
      .pipe(map(response => {
        return response;
      }));
  }


  exportSummary(request: InspectionBookingsummarymodel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/InspectionBooking/ExportInspectionSearchSummary`, request, { headers: headers, responseType: 'blob' as 'json' });
  }





  uploadProductAttachedFiles(attachedList: Array<InspectionFileAttachment>, bookingId) {
    const formData = new FormData();

    attachedList.map(x => {
      formData.append(x.uniqueld, null);
    });

    return this.http.post(`${this.url}/api/InspectionBooking/productattached/${bookingId}`, formData)
      .pipe(map(response => {
        return response;
      }));

  }

  GetUnitList() {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/getunitdetails/`);
  }
  GetInspectionServiceType(id) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetServiceInspection/${id}`);
  }

  GetProductsByCustomerAndCategory(id) {
    return this.http.get<any>(`${this.url}/api/customerproduct/productscategorybycustomer/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  GetProductsByPOAndCategory(customerId, poid) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/productsbycustomerpoandcategory/${customerId}/${poid}`);
  }

  Getbookingsummary() {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/Getbookingsummary`);
  }

  GetAqlByServiceType(customerId, serviceTypeId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/getaqlbyservicetype/${customerId}/${serviceTypeId}`);
  }

  GetCancelBookingDetails(id, typeid) {
    return this.http.get<any>(`${this.url}/api/CancelBooking/getCancelEditBookingDetails/${id},${typeid}`);
  }

  SaveCancelBooking(model: any, typeId) {
    if (typeId == BookingSearchRedirectPage.Cancel) {
      return this.http.post<any>(`${this.url}/api/CancelBooking/saveCancelBooking`, model)
        .pipe(map(response => {
          return response;
        }));
    } else if (typeId == BookingSearchRedirectPage.Reschedule) {
      return this.http.post<any>(`${this.url}/api/CancelBooking/saveReschedule`, model)
        .pipe(map(response => {
          return response;
        }));
    }
  }


  SaveMasterDataToFb(bookingId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/SaveMasterDataToFB/${bookingId}`);
  }



  GetCurrency() {
    return this.http.get<any>(`${this.url}/api/CancelBooking/getCurrency`)
      .pipe(map(response => {
        return response;
      }));
  }

  GetReason(id, typeId) {
    if (typeId == BookingSearchRedirectPage.Cancel) {
      return this.http.get<any>(`${this.url}/api/CancelBooking/getReason/${id}`)
        .pipe(map(response => {
          return response;
        }));
    } else if (typeId == BookingSearchRedirectPage.Reschedule) {
      return this.http.get<any>(`${this.url}/api/CancelBooking/getRescheduleReason/${id}`)
        .pipe(map(response => {
          return response;
        }));
    }
  }

  GetInspBookingRules(customerId, factoryId) {
    if (factoryId)
      return this.http.get<any>(`${this.url}/api/InspBookingRuleContact/getInspBookingRuleDetails/${customerId}?factoryId=` + factoryId);
    else
      return this.http.get<any>(`${this.url}/api/InspBookingRuleContact/getInspBookingRuleDetails/${customerId}`);

  }

  GetInspBookingContact(factoryId, customerId) {
    return this.http.get<any>(`${this.url}/api/InspBookingRuleContact/getInspBookingContactDetails/${factoryId}/${customerId}`);
  }

  GetReInspectionServiceType(id) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetReInspectionServiceType/${id}`);
  }

  GetReInspectionTypes() {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetReInspectionServiceTypes`);
  }

  GetInspectionStaffDetails(id) {
    return this.http.get<any>(`${this.url}/api/HumanResource/staff/bookingstaffdetails/${id}`);
  }
  GetUserApplicantDetails() {
    return this.http.get<any>(`${this.url}/api/UserAccount/get-user-applicant-details`);
  }
  Getcustomercontactsbybrandordept(customerId, brandId, deptId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/getcustomercontactsbybrandordept/${customerId},${brandId},${deptId}`);
  }

  GetCustomerContacts(model: BookingCustomerContactRequest) {
    return this.http.post<any>(`${this.url}/api/InspectionBooking/getcustomercontactsbybrandordept`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  //get boolean variable to show popup for cancel and reschedule
  IsHolidayExists(model: HolidayRequest) {
    return this.http.post<any>(`${this.url}/api/InspectionBooking/isHolidayExists`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  //get factory details based on customer and supplier
  GetFactoryDetailsByCusSupId(supid, cusid) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetFactoryDetailsByCusSupId/${cusid},${supid}`);
  }

  GetProductDetailsByBooking(bookingId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetBookingProductStatus/${bookingId}`);
  }

  GetProductDataByBooking(bookingId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetBookingProducts/${bookingId}`);
  }

  GetContainerDetailsByBooking(bookingId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetBookingContainerStatus/${bookingId}`);
  }

  GetContainerReportDetailsByBooking(bookingId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetBookingContainers/${bookingId}`);
  }


  GetPODetailsByBookingAndProduct(bookingId, productRefId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetBookingPoListbyProduct/${bookingId}/${productRefId}`);
  }

  GetPODetailsByBookingAndConatinerAndProduct(bookingId, containerRefId, productRefId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetBookingPoListbyContainerAndProduct/${bookingId}/${containerRefId}/${productRefId}`);
  }

  GetProductListByBookingAndContainer(bookingId, containerRefId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetBookingProductListbyContainer/${bookingId}/${containerRefId}`);
  }

  GetBookingAndReportDetails(bookingId, reportId, containerId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetBookingProductsReports/${bookingId}/${reportId}/${containerId}`);
  }

  GetGetInspectionSummary(reportId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetInspectionSummary/${reportId}`);
  }

  GetInspectionDefects(reportId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetInspectionDefects/${reportId}`);
  }

  GetInspectionDefectsbyProducts(reportId, inspPoId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetInspectionDefects/${reportId}/${inspPoId}`);
  }

  GetInspectionDefectsbyContainer(reportId, inspPoId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetInspectionDefectsByContainer/${reportId}/${inspPoId}`);
  }


  preview(id: number) {
    return this.http.get(`${this.url}/api/InternalFBReports/qcinspectiondetaildownload/${id}`, { responseType: 'blob' });
  }

  GetPickingPdf(bookingId) {
    return this.http.get(`${this.url}/api/InternalFBReports/GetQcPicking/${bookingId}`, { responseType: 'blob' });
  }

  GetFbTemplateList() {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/getfbReportTemplateList/`);
  }

  getPriceCategory(model: PriceCategoryRequest) {
    return this.http.post<any>(`${this.url}/api/InspectionBooking/getPriceCategory`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  bookingProductValidationInfo(bookingId, poTranId, productId): Promise<ProductValidationResponse> {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/bookingProductValidationInfo/${bookingId}/${poTranId}/${productId}`).toPromise();
  }

  bookingProductsValidation(request): Promise<Array<ProductValidateData>> {
    return this.http.post<any>(`${this.url}/api/InspectionBooking/bookingProductValidation`, request).toPromise();
  }

  getPreviousBookingApplicantDetails() {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/booking-applicant-details`);
  }


  getInvoiceType() {
    return this.http.get<any>(`${this.url}/api/customer/invoicetype`);

  }

  exportProductSummary(bookingId: number) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/InspectionBooking/ExportInspectionProductSummary/`, bookingId, { headers: headers, responseType: 'blob' as 'json' });
  }

  getBookingStatusList() {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/booking-status-data`);
  }
  getCSNames(model: UserAccessRequest) {
    return this.http.post<any>(`${this.url}/api/InspectionBooking/cs-names`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  /*  getCSList(model: UserAccessRequest) {
     return this.http.post<any>(`${this.url}/api/InspectionBooking/cs-list`, model)
       .pipe(map(response => {
         return response;
       }));
   } */

  async getCSList(model: UserAccessRequest): Promise<CSConfigListResponse> {
    return await this.http.post<any>(`${this.url}/api/InspectionBooking/cs-list`, model).toPromise();
  }

  async getEntPageAccessList(model: EntPageRequest): Promise<EntPageFieldAccessResponse> {
    return await this.http.post<any>(`${this.url}/api/InspectionBooking/get-entpagefield-access`, model).toPromise();
  }

  getBookingSummaryStatusList() {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/Getbookingsummarystatus`);
  }

  getAEUserList() {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetAEUserList`);
  }
  getEditBookingCustomerRelatedDetails(id, bookingId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetEditCustomerDetails/${id}/${bookingId}`);
  }

  getEditBookingOffice(bookingId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetEditBookingOffice/${bookingId}`);
  }

  getEditBookingUnit(bookingId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetEditBookingUnit/${bookingId}`);
  }
  getInspectionHoldReasonTypes() {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetHoldReasonTypes`);
  }
  getEditBookingInspectionLocations(bookingId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/booking-inspection-locations/${bookingId}`);
  }
  getEditBookingShipmentTypes(bookingId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/booking-shipment-types/${bookingId}`);
  }
  getEditBookingCuProductCategory(customerId, bookingId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/booking-cu-product-category/${customerId}/${bookingId}`);
  }
  getEditBookingSeason(customerId, bookingId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/get-booking-season-config/${customerId}/${bookingId}`);
  }
  getEditBookingBusinessLines(bookingId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/booking-business-lines/${bookingId}`);
  }

  getInspectionProductBaseDetail(bookingId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/get-inspproduct-base-detail/${bookingId}`);
  }

  async checkInspectionPickingExists(model: InspectionPickingExistRequest): Promise<boolean> {
    return await this.http.post<any>(`${this.url}/api/InspectionBooking/GetInspectionPickingExists`, model).toPromise();
  }

  getBookingFileAttachment(bookingId) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/get-booking-attachment/${bookingId}`);
  }

  saveMasterContact(model: SaveMasterContactRequest) {
    return this.http.post<any>(`${this.url}/api/InspectionBooking/save-master-contact`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  saveLabAddressData(model: SaveLabAddressRequestData) {
    return this.http.post<any>(`${this.url}/api/lab/save-lab-address-list`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getPOProductListByName(item, term, editBookingLoading, poProductList) {
    if (poProductList && poProductList.length > 0 && !term) {
      const obsFrom3 = of(poProductList);
      return obsFrom3;
    }
    else if (item && editBookingLoading && !term) {
      var productList = new Array<POProductList>();
      var poproduct = new POProductList();
      poproduct.id = item.poProductDetail.productId;
      poproduct.name = item.poProductDetail.productName;
      poproduct.description = item.poProductDetail.productDesc;
      productList.push(poproduct);
      const obsFrom3 = of(productList);
      return obsFrom3;
    }
    else {
      var request = item.poProductDetail.productRequest;
      if (term)
        item.poProductDetail.productRequest.searchText = term;

      return this.http.post<any>(`${this.url}/api/customerproduct/CustomerProductDetailsDataSourceList`, request)
        .pipe(
          map(

            response => {
              var prdList = response.productList as POProductList[];
              return prdList;
            }));
    }

  }

  async getPoProductDetail(model: PoProductDetailRequest): Promise<POProductDetailResponse> {
    return await this.http.post<any>(`${this.url}/api/InspectionBooking/get-po-product-details`, model).toPromise();
  }

  getCustomerAddressContactDetails(customerId) {
    return this.http.get<any>(`${this.url}/api/customer/get-customer-contacts-address-list/${customerId}`);
  }

  getLabAddressDetails(labAddressRequest) {
    return this.http.post<any>(`${this.url}/api/lab/lab-address-by-lab-list`, labAddressRequest)
      .pipe(map(response => {
        return response;
      }));
  }

  getLabContactDetails(labContactRequest) {
    return this.http.post<any>(`${this.url}/api/lab/lab-contacts-by-lab-list`, labContactRequest)
      .pipe(map(response => {
        return response;
      }));
  }

}
