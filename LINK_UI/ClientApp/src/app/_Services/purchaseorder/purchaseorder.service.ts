import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, first } from 'rxjs/operators';
import { EditPurchaseOrderModel, AttachmentFile, AutoPoNumber, purchaseOrderDetailModel, RemovePurchaseOrderDetail } from '../../_Models/purchaseorder/edit-purchaseorder.model';
import { commonDataSource, POProductList } from 'src/app/_Models/booking/inspectionbooking.model';
import { of } from 'rxjs';
import { PoBookingDetailResponse } from 'src/app/_Models/purchaseorder/purchaseordersummary.model';
import { CommonDataSourceRequest, SupplierDataSourceRequest } from 'src/app/_Models/common/common.model';

@Injectable({ providedIn: 'root' })
export class PurchaseOrderService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }



  getPurchaseOrderSummary() {
    return this.http.get<any>(`${this.url}/api/purchaseorder`)
      .pipe(map(response => {
        return response;
      }));
  }


  getPurchaseOrderByCustomerId(id: number) {
    return this.http.get<any>(`${this.url}/api/purchaseorder/purchaseordersbycustomer/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getPurchaseOrderById(id: number) {
    return this.http.get<any>(`${this.url}/api/purchaseorder/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }
  // Get the po Data By ID
  getPurchaseOrderDataById(id: number) {
    return this.http.get<any>(`${this.url}/api/purchaseorder/PurchaseOrderById/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }
  // Get the po product details with paging
  SearchPurchaseOrderDetails(model: purchaseOrderDetailModel) {
    return this.http.post<any>(`${this.url}/api/purchaseorder/SearchPurchaseOrderDetails`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  // Get the po Attachments
  GetPOattachments(id: number) {
    return this.http.get<any>(`${this.url}/api/purchaseorder/PurchaseOrderAttachmentsById/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }
  // Remove PO pruduct 
  RemovePurchaseOrderDetail(model: RemovePurchaseOrderDetail) {
    return this.http.post<any>(`${this.url}/api/purchaseorder/RemovePurchaseOrderDetail`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getDataSummary(request) {
    return this.http.post<any>(`${this.url}/api/purchaseorder/search`, request)
      .pipe(map(response => {

        return response;
      }));
  }
  exportSummary(request) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/purchaseorder/export-purchaseorderproducts`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  deletePurchaseOrder(id: number) {
    return this.http.delete<any>(`${this.url}/api/purchaseorder/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  savePurchaseOrder(model: EditPurchaseOrderModel) {

    return this.http.post<any>(`${this.url}/api/purchaseorder`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  savePurchaseOrderList(model: any) {

    return this.http.post<any>(`${this.url}/api/purchaseorder/savepolistfromexcel`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  updatePurchaseOrder(id: number, model: EditPurchaseOrderModel) {
    return this.http.post<any>(`${this.url}/api/purchaseorder/${id}`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getFile(id) {
    return this.http.get(`${this.url}/api/purchaseorder/file/${id}`, { responseType: 'blob' });
  }

  uploadAttachedFiles(poID, attachedList: Array<AttachmentFile>) {

    const formData = new FormData();

    attachedList.map(x => {
      formData.append(x.uniqueld, x.file);
    });

    return this.http.post(`${this.url}/api/purchaseorder/attached/${poID}`, formData)
      .pipe(map(response => {
        return response;
      }));

  }

  uploadPurchaseOrder(customerId, attachedList: Array<AttachmentFile>) {

    const formData = new FormData();

    attachedList.map(x => {
      formData.append(x.uniqueld, x.file);
    });

    return this.http.post<any>(`${this.url}/api/purchaseorder/uploadpurchaseorder/${customerId}`, formData)
      .pipe(map(response => {
        return response;
      }));

  }


  /* uploadPurchaseOrderLite(customerId, supplierId,attachedList: Array<AttachmentFile>) {

    const formData = new FormData();

    attachedList.map(x => {
      formData.append(x.uniqueld, x.file);
    });


    return this.http.post<any>(`${this.url}/api/purchaseorder/upload-po-product-data/${customerId}/${supplierId}`, formData)
      .pipe(map(response => {
        return response;
      }));



  } */

  uploadPurchaseOrderLite(poProductUploadRequest, attachedList: Array<AttachmentFile>) {
    const formData = new FormData();

    attachedList.map(x => {
      formData.append(x.uniqueld, x.file);
    });

    formData.append("customerId", poProductUploadRequest.customerId);
    formData.append("supplierId", poProductUploadRequest.supplierId);
    formData.append("businessLineId", poProductUploadRequest.businessLineId);

    formData.append('poProductRequest', JSON.stringify(poProductUploadRequest.existingBookingPoProductList));

    return this.http.post<any>(`${this.url}/api/purchaseorder/upload-po-product-data/`, formData)
      .pipe(map(response => {
        return response;
      }));

  }

  //get factory details based on customer and supplier
  GetFactoryDetailsByCusSupId(supid, cusid) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetFactoryDetailsByCusSupId/${cusid},${supid}`);
  }

  getPOListByName(poName, customerId) {
    var model = new AutoPoNumber();
    model.customerId = customerId;
    model.poname = poName;
    return this.http.post<any>(`${this.url}/api/purchaseorder/getpolistbyname`, model)
      .pipe(map(response => {
        return response.poDataSource;
      }));
  }

  getPODataSource(term, item, editBookingLoading, poList, bookingProductList, currentPoList) {
    if (poList && poList.length > 0 && !term) {
      const obsFrom3 = of(poList);
      return obsFrom3;
    }
    else if (item && editBookingLoading && !term) {
      var poDataSourceList = new Array<commonDataSource>();
      var poData = new commonDataSource();
      poData.id = item.poProductDetail.poId;
      poData.name = item.poProductDetail.poName;
      poDataSourceList.push(poData);

      const obsFrom3 = of(poDataSourceList);
      return obsFrom3;
    }
    else if (item) {
      if (term)
        item.poProductDetail.requestPoDataSourceModel.searchText = term;
      //autosearch will not work if search with the term value or we select the searched item
      /*  else
         item.poProductDetail.requestPoDataSourceModel.searchText = ""; */
      return this.http.post<any>(`${this.url}/api/purchaseorder/getpodatasource`, item.poProductDetail.requestPoDataSourceModel)
        .pipe(map(response => {

          return response.poDataSource;
          /* var poList = response.poDataSource;
          //add polist in the booking product list (in booking page) will get added to resulted polist
          if (poList && bookingProductList) {
            //take the po id list from the booking product list
            var bookingPoIdList = bookingProductList.filter(x => x.poProductDetail.poId).map(x => x.poProductDetail.poId);

            if (bookingPoIdList) {
              //take the distinct po ids
              var distinctPoIds = bookingPoIdList.filter((n, i) => bookingPoIdList.indexOf(n) === i);
              //loop through the distinct po ids
              distinctPoIds.forEach(poId => {
                //take the booking product row using poid 
                var bookingProductData = bookingProductList.find(x => x.poProductDetail.poId == poId);

                if (bookingProductData) {

                  if (bookingProductData.poProductDetail.poId && bookingProductData.poProductDetail.poName) {

                    if (currentPoList) {
                      //check data is available in the current po list and push data if it is not
                      //available in the list
                      var currentPoAvailable = currentPoList.find(x => x.name == bookingProductData.poProductDetail.poName);
                      if (!currentPoAvailable) {
                        var poData = new commonDataSource();
                        poData.id = bookingProductData.poProductDetail.poId;
                        poData.name = bookingProductData.poProductDetail.poName;
                        poList.push(poData);
                      }
                      //if it is available in the current po list
                      else if (currentPoAvailable) {
                        //check if it is available in the po list
                        //remove the po id from the duplicates
                        var poIndex = poList.findIndex(x => x.name == currentPoAvailable.name);
                        if (poIndex != -1)
                          poList.splice(poIndex, 1);
                      }
                    }
                    //data is loading for the first time(without term search)
                    else if (!term) {
                      //check booking po name available in the po list if not push to polist
                      var poAvailable = poList.find(x => x.name == bookingProductData.poProductDetail.poName);
                      if (!poAvailable) {
                        var poData = new commonDataSource();
                        poData.id = bookingProductData.poProductDetail.poId;
                        poData.name = bookingProductData.poProductDetail.poName;
                        poList.push(poData);
                      }
                    }
                  }
                }
              });
            }
          }
          //if no polist not available for the search text and it is available in the booking product list
          //then push to polist
          else if (!poList) {
            //case sensitive search regular expression
            if (item.poProductDetail.requestPoDataSourceModel.searchText) {
              var termSearch = new RegExp(item.poProductDetail.requestPoDataSourceModel.searchText, 'i');
              //search booking product data
              var bookingProductData = bookingProductList.find(x => termSearch.test(x.poProductDetail.poName));
              //if it is availalble then push to polist
              if (bookingProductData) {

                if (currentPoList) {
                  var currentPoAvailable = currentPoList.find(x => x.name == bookingProductData.poProductDetail.poName);
                  if (!currentPoAvailable) {
                    var poData = new commonDataSource();
                    poData.id = bookingProductData.poProductDetail.poId;
                    poData.name = bookingProductData.poProductDetail.poName;
                    poList.push(poData);
                  }
                }
                else {
                  var poData = new commonDataSource();
                  poData.id = bookingProductData.poProductDetail.poId;
                  poData.name = bookingProductData.poProductDetail.poName;
                  poList = [];
                  poList.push(poData);
                }
              }
            }

          }
          return poList; */
        }));
    }
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
      var request = item.poProductDetail.requestPoProductDataSourceModel;
      item.poProductDetail.requestPoProductDataSourceModel.searchText = "";
      if (term)
        item.poProductDetail.requestPoProductDataSourceModel.searchText = term;

      return this.http.post<any>(`${this.url}/api/inspectionbooking/get-booking-po-product-details`, request)
        .pipe(
          map(

            response => {
              var prdList = response.productList as POProductList[];
              return prdList;
            }));
    }

  }

  getSearchPOList(term, requestPoDataSourceModel) {
    if (term)
      requestPoDataSourceModel.searchText = term;

    return this.http.post<any>(`${this.url}/api/purchaseorder/getpodatasource`, requestPoDataSourceModel)
      .pipe(map(response => {
        var poList = response.poDataSource;
        return response.poDataSource;
      }));
  }

  getSearchPOProductList(poProductDataRequest) {

    return this.http.post<any>(`${this.url}/api/purchaseorder/getpoproductdata`, poProductDataRequest)
      .pipe(map(response => {
        return response;
      }));

  }

  /*  getPurchaseOrderUploadFile() {
     return this.http.get(`${this.url}/api/purchaseorder/fileTerms`, { responseType: 'blob' });
   } */

  /*  getPurchaseOrderUploadFile() {
     return this.http.get<any>(`${this.url}/api/purchaseorder/fileTerms`);
   } */

  downloadFile(typeId) {
    return this.http.get(`${this.url}/api/inspectionbooking/get-purchase-order-sample-file/${typeId}`, { responseType: 'blob' });
  }

  async getPOBookingDetails(poId: number): Promise<PoBookingDetailResponse> {
    return this.http.get<any>(`${this.url}/api/purchaseorder/get-po-booking-details/${poId}`).toPromise();
  }

  getSupplierDataSourceList(supplierList, model: CommonDataSourceRequest, term?: string) {
    model.searchText = "";
    if (term) {
      model.searchText = term;
      model.idList = null;
    }
    if (supplierList && supplierList.length > 0 && !term) {
      const obsFrom3 = of(supplierList);
      return obsFrom3;
    }
    else {
      return this.http.post<any>(`${this.url}/api/Supplier/getfactorydatasource`, model)
        .pipe(map(response => {
          /* var dataSourceList = response.dataSourceList;
          if (supplierList && supplierList.length > 0 && !term) {
            if (dataSourceList && dataSourceList.length > 0)
              dataSourceList = dataSourceList.concat(supplierList);
            else
              dataSourceList = supplierList;
          } */

          return response.dataSourceList;

        }));
    }
  }

  getFactoryDataSource(factoryList, model: SupplierDataSourceRequest, serviceId: number, term?: string) {
    model.searchText = "";
    if (term) {
      model.searchText = term;
      model.supplierIds = null;
    }
    model.serviceId = serviceId;
    if (factoryList && factoryList.length > 0 && !term) {
      const obsFrom3 = of(factoryList);
      return obsFrom3;
    }
    else {
      return this.http.post<any>(`${this.url}/api/Supplier/GetSupplierDataSourceList`, model)
        .pipe(map(response => {
          return response.dataSourceList;
        }));
    }
  }

}
