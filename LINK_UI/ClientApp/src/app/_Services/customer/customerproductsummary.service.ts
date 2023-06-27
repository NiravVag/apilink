import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { EditCustomerProductModel, AttachmentFile, CustomerProductFileResponse, PoProductRequest, CustomerProductDetailResponse } from 'src/app/_Models/customer/editcustomerproduct.model';
import { ProductCategorySourceRequest, ProductSubCategory2SourceRequest, ProductDataSourceRequest, DataSource, DataSourceResponse, ProductSubCategory3SourceRequest } from 'src/app/_Models/common/common.model';
import { Observable } from 'rxjs';
import { of } from 'rxjs';
import { CustomerProductDataSourceResponse, ProductList } from 'src/app/_Models/purchaseorder/edit-purchaseorder.model';
import { OcrTableRequest } from 'src/app/_Models/customer/ocr-master-model';

@Injectable({ providedIn: 'root' })

export class CustomerProduct {
  url: string
  fileServer: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string, @Inject('FILE_SERVER_URL') fileServerUrl: string) {
    this.url = baseUrl;
    this.fileServer = fileServerUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);

    if (this.fileServer.charAt(this.fileServer.length - 1) == '/')
      this.fileServer = this.fileServer.substring(0, this.fileServer.length - 1);
  }

  getProductCategoryList() {
    return this.http.get<any>(`${this.url}/api/customerproduct/getProductsCategory`)
      .pipe(map(response => {
        return response;
      }));
  }

  getProductSubCategoryList(id: number) {
    return this.http.get<any>(`${this.url}/api/customerproduct/getProductSubCategory/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getProductCategorySub2(id: number) {
    return this.http.get<any>(`${this.url}/api/customerproduct/getProductCategorySub2/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }


  getProductsByCustomer(id: number) {
    return this.http.get<any>(`${this.url}/api/customerproduct/productscategorybycustomer/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getProductDataSource(model: ProductDataSourceRequest, term?: string) {
    model.searchText = term ? term : model.searchText;
    return this.http.post<any>(`${this.url}/api/customerproduct/CustomerProductDataSourceList`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }



  async getBaseProductDataSource(model: ProductDataSourceRequest): Promise<CustomerProductDataSourceResponse> {
    return await this.http.post<CustomerProductDataSourceResponse>(`${this.url}/api/customerproduct/CustomerProductDataSourceList`, model).toPromise();
  }

  getPOProductListByName(item, term, productList) {
    if (productList && productList.length > 0 && !term) {
      const obsFrom3 = of(productList);
      return obsFrom3;
    }
    else {
      var request = item.poDetails.productRequest;
      item.poDetails.productRequest.searchText = "";
      if (term)
        item.poDetails.productRequest.searchText = term;

      return this.http.post<any>(`${this.url}/api/customerproduct/CustomerProductDataSourceList`, request)
        .pipe(
          map(

            response => {
              var prdList = response.dataSourceList as ProductList[];
              return prdList;
            }));
    }

  }


  getCustomerProductSummary(request) {
    return this.http.post<any>(`${this.url}/api/customerproduct/search`, request)
      .pipe(map(response => {
        return response;
      }));
  }
  exportSummary(request) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/customerproduct/export-customerproducts`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getEditCustomerProduct(id) {

    if (id == null)
      return this.http.get<any>(`${this.url}/api/customerproduct/add`);
    else
      return this.http.get<any>(`${this.url}/api/customerproduct/${id}`);
  }

  saveCustomerProduct(model: EditCustomerProductModel) {
    return this.http.post<any>(`${this.url}/api/customerproduct`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  saveCustomerProductList(model: Array<EditCustomerProductModel>) {
    return this.http.post<any>(`${this.url}/api/customerproduct/savecustomerproductlist`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  deleteCustomerProduct(id: number) {
    return this.http.delete<any>(`${this.url}/api/customerproduct/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getFile(id, container) {
    return this.http.get(`${this.fileServer}/api/FileStore/downloadfile/${id}/${container}`, { responseType: 'blob' });
  }

  getProductFile(id) {
    return this.http.get(`${this.url}/api/InspectionBooking/productfile/${id}`, { responseType: 'blob' });
  }

  getProductCategoryDataSource(model: ProductCategorySourceRequest, term?: string) {
    model.searchText = term ? term : model.searchText;
    return this.http.post<any>(`${this.url}/api/CustomerProduct/GetProductCategoryDataSource`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }


  getProductSubCategoryDataSource(model: ProductCategorySourceRequest, term?: string) {
    model.searchText = term ? term : model.searchText;
    return this.http.post<any>(`${this.url}/api/CustomerProduct/GetProductSubCategoryDataSource`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  //get product sub category 2 list
  getProductSubCategory2DataSource(model: ProductSubCategory2SourceRequest, term?: string) {
    model.searchText = term ? term : "";
    if (term) {
      model.searchText = term;
      model.productSubCategory2Ids = [];
    }
    return this.http.post<any>(`${this.url}/api/CustomerProduct/get-product-subcategory2-details`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  //get the product sub category2 list for the booking screen
  getProductInternalDataSource(model: ProductSubCategory2SourceRequest, term?: string, initialLoad?: boolean) {
    if (term || initialLoad) {
      model.searchText = term ? term : "";
      return this.http.post<any>(`${this.url}/api/CustomerProduct/get-product-subcategory2-details`, model)
        .pipe(map(response => {
          return response.dataSourceList;
        }));
    }
    //return default observable data
    var obs = new Observable((observer) => {
      observer.next("1")
    })
    return obs;
  }
  getFileTypeList() {
    return this.http.get<any>(`${this.url}/api/customerproduct/fileTypeList`);
  }


  //get product sub category 3 list
  getProductSubCategory3DataSource(model: ProductSubCategory3SourceRequest, term?: string) {
    model.searchText = term ? term : "";
    if (term) {
      model.searchText = term;
      model.productSubCategory3Ids = [];
    }
    return this.http.post<any>(`${this.url}/api/CustomerProduct/get-product-subcategory3-details`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  getProductCategorySub3(id: number) {
    return this.http.get<any>(`${this.url}/api/productmanagement/product-category-sub3-list/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  async getProductFileUrls(productId): Promise<CustomerProductFileResponse> {
    return await this.http.get<any>(`${this.url}/api/customerproduct/get-product-file-urls/${productId}`).toPromise();
  }

  async getCustomerProductByProductIds(model: PoProductRequest): Promise<CustomerProductDetailResponse> {
    return await this.http.post<CustomerProductDetailResponse>(`${this.url}/api/customerproduct/products-by-customer`, model).toPromise();
  }

  getMsChartFileFormat(customerId: number) {
    return this.http.get<any>(`${this.url}/api/CustomerProduct/msChartFileFormatByCustomer/${customerId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getOcrTableData(request: OcrTableRequest) {
    return this.http.post<any>(`${this.url}/api/CustomerProduct/OcrTableData`, request)
      .pipe(map(response => {
        return response;
      }));
  }

  exportOcr(request) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/customerproduct/export-ocr`, request, { headers: headers, responseType: 'blob' as 'json' });
  }
}
