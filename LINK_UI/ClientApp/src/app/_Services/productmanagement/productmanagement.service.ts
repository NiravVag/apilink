import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { ProductCategoryModel } from 'src/app/_Models/productmanagement/productcategory.model';
import { ProductSubCategoryModel } from 'src/app/_Models/productmanagement/productsubcategory.model';
import { ProductCategorySub2Model } from 'src/app/_Models/productmanagement/productcategorysub2.model';
import { EditProductCategorySub3Model } from 'src/app/_Models/productmanagement/productcategorysub3.model';

@Injectable({
  providedIn: 'root'
})
export class ProductManagementService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  getProductCategorySummary() {
    return this.http.get<any>(`${this.url}/api/productmanagement/productcategory`)
      .pipe(map(Response => { return Response; }));
  }

  getProductCategoryById(id) {
    return this.http.get<any>(`${this.url}/api/productmanagement/productcategory/${id}`)
      .pipe(map(Response => { return Response; }));
  }

  deleteProductCategoryById(id) {
    return this.http.delete<any>(`${this.url}/api/productmanagement/productcategory/${id}`)
      .pipe(map(Response => { return Response; }));
  }

  saveProductCategory(model: ProductCategoryModel) {
    if (!model.id)//add
      return this.http.post<any>(`${this.url}/api/productmanagement/productcategory`, model)
        .pipe(map(response => {
          return response;
        }));
    else//update
      return this.updateProductCategory(model);
  }

  updateProductCategory(model: ProductCategoryModel) {

    return this.http.put<any>(`${this.url}/api/productmanagement/productcategory`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getProductSubCategorySummary(id?) {
    if (!id)
      return this.http.get<any>(`${this.url}/api/productmanagement/productsubcategory`)
        .pipe(map(Response => { return Response; }));
    else
      return this.getsubbycategoryid(id);
  }

  getProductSubCategoryById(id) {
    return this.http.get<any>(`${this.url}/api/productmanagement/productsubcategory/${id}`)
      .pipe(map(Response => { return Response; }));
  }

  deleteProductSubCategoryById(id) {
    return this.http.delete<any>(`${this.url}/api/productmanagement/productsubcategory/${id}`)
      .pipe(map(Response => { return Response; }));
  }

  saveProductSubCategory(model: ProductSubCategoryModel) {
    if (!model.id)//add
      return this.http.post<any>(`${this.url}/api/productmanagement/productsubcategory`, model)
        .pipe(map(response => {
          return response;
        }));
    else
      return this.updateProductSubCategory(model);
  }

  updateProductSubCategory(model: ProductSubCategoryModel) {
    return this.http.put<any>(`${this.url}/api/productmanagement/productsubcategory`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getProductCategorySub2Summary() {
    return this.http.get<any>(`${this.url}/api/productmanagement/productcategorysub2`)
      .pipe(map(Response => { return Response; }));
  }

  getProductCategorySub2SearchSummary(request) {
    return this.http.post<any>(`${this.url}/api/productmanagement/searchproductcategorysub2`, request)
      .pipe(map(response => { return response; }));
  }

  getProductCategorySub2ById(id) {
    if (id == null)
      id = "";
    return this.http.get<any>(`${this.url}/api/productmanagement/productcategorysub2/${id}`)
      .pipe(map(Response => { return Response; }));
  }

  deleteProductCategorySub2ById(id) {
    return this.http.delete<any>(`${this.url}/api/productmanagement/productcategorysub2/${id}`)
      .pipe(map(Response => { return Response; }));
  }

  saveProductCategorySub2(model: ProductCategorySub2Model) {
    return this.http.post<any>(`${this.url}/api/productmanagement/productcategorysub2`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  updateProductCategorySub2(model: ProductCategorySub2Model) {
    return this.http.put<any>(`${this.url}/api/productmanagement/productcategorysub2`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getsubbycategoryid(id) {
    return this.http.get<any>(`${this.url}/api/productmanagement/productcategory/productsubcategories/${id}`);
  }

  gettypebysubcategoryid(id) {
    return this.http.get<any>(`${this.url}/api/productmanagement/productsubcategory/productcategorysub2s/${id}`);
  }

  getProductCategoryList() {
    return this.http.get<any>(`${this.url}/api/productmanagement/productcategorylist`)
      .pipe(map(Response => { return Response; }));
  }

  getProductSubCategoryList() {
    return this.http.get<any>(`${this.url}/api/productmanagement/productsubcategorylist`)
      .pipe(map(Response => { return Response; }));
  }

  getProductCategorySub3SearchSummary(request) {
    return this.http.post<any>(`${this.url}/api/productmanagement/product-category-sub3`, request)
      .pipe(map(response => { return response; }));
  }

  async saveProductCategorySub3(model: EditProductCategorySub3Model) {

    if (model.id > 0) {
      return await this.http.put<any>(`${this.url}/api/productmanagement/productcategorysub3s/update`, model)
        .toPromise();
    }
    else {
      return await this.http.post<any>(`${this.url}/api/productmanagement/productcategorysub3s/save`, model)
        .toPromise()
    }
  }  

  async getCategorySub3ById(id) {
    return this.http.get<any>(`${this.url}/api/productmanagement/product-category-sub3/${id}`).toPromise();
  }

  async deleteProductCategorySub3ById(id) {
    return await this.http.delete<any>(`${this.url}/api/productmanagement/delete-productcategorysub3/${id}`).toPromise();
  }

  exportSummary(request) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/productmanagement/Export-product-category-sub3`, request, { headers: headers, responseType: 'blob' as 'json' });
  }
}
