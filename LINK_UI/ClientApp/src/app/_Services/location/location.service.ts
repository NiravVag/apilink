import { Injectable, inject, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, first } from 'rxjs/operators';
import { EditCountrymodel } from '../../_Models/location/edit-countrymodel'
import { EditProvincemodel } from '../../_Models/location/edit-provincemodel'
import { EditCitymodel } from '../../_Models/location/edit-citymodel'
import { EditCountymodel } from '../../_Models/location/edit-countymodel'
import { CountySummaryModel } from 'src/app/_Models/location/countysummarymodel';
import { EditTownModel } from 'src/app/_Models/location/edit-townmodel';
import { CityDataSourceRequest, CommonZoneSourceRequest, CountryDataSourceRequest, DataSourceResponse, ProvinceDataSourceRequest, StartPortDataSourceRequest, TownDataSourceRequest } from 'src/app/_Models/common/common.model';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LocationService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }
  getCountrySummary() {
    return this.http.get<any>(`${this.url}/api/Location`)
      .pipe(map(Response => { return Response; }));
  }
  getCountrySearchSummary(request) {
    return this.http.post<any>(`${this.url}/api/Location/GetSearchCountry`, request)
      .pipe(map(response => { return response; }));
  }
  geteditcountry(id) {
    if (id == null)
      id = "";
    return this.http.get<any>(`${this.url}/api/Location/country/edit/${id}`);
  }
  getprovincebycountryid(id) {
    return this.http.get<any>(`${this.url}/api/Location/states/${id}`);
  }

  getTownsByCountyId(id) {
    return this.http.get<any>(`${this.url}/api/Location/towns/${id}`);
  }
  getcitybyprovinceid(id) {
    return this.http.get<any>(`${this.url}/api/Location/cities/${id}`);
  }
  getcitybycountryid(id) {
    return this.http.get<any>(`${this.url}/api/Location/country/cities/${id}`);
  }
  getProvinceSearchSummary(request) {
    return this.http.post<any>(`${this.url}/api/Location/GetProvinceSearch`, request)
      .pipe(map(response => { return response; }));
  }
  geteditProvince(id) {
    if (id == null)
      id = "";
    return this.http.get<any>(`${this.url}/api/Location/province/edit/${id}`);
  }
  getCitySearchSummary(request) {
    return this.http.post<any>(`${this.url}/api/Location/GetCitySearch`, request)
      .pipe(map(response => { return response; }));
  }
  geteditCity(id) {
    if (id == null)
      id = "";
    return this.http.get<any>(`${this.url}/api/Location/city/edit/${id}`);
  }
  saveCountry(model: EditCountrymodel) {
    return this.http.post<any>(`${this.url}/api/Location/country/save`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  saveProvince(model: EditProvincemodel) {
    return this.http.post<any>(`${this.url}/api/Location/province/save`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  saveCity(model: EditCitymodel) {
    return this.http.post<any>(`${this.url}/api/Location/city/save`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  saveCounty(model: EditCountymodel) {
    return this.http.post<any>(`${this.url}/api/Location/county/save`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  getcountybycity(id) {
    return this.http.get<any>(`${this.url}/api/Location/cities/counties/${id}`);
  }

  getCountySearchSummary(request) {
    return this.http.post<any>(`${this.url}/api/Location/GetCountySearch`, request)
      .pipe(map(response => { return response; }));
  }

  geteditCounty(id) {
    if (id == null)
      id = 0;
    return this.http.get<any>(`${this.url}/api/Location/county/edit/${id}`);
  }

  deleteCounty(id) {
    return this.http.delete<any>(`${this.url}/api/Location/county/delete/${id}`);
  }

  getEditTown(id) {
    if (id == null)
      id = 0;
    return this.http.get<any>(`${this.url}/api/Location/town/edit/${id}`);
  }

  saveTown(model: EditTownModel) {
    return this.http.post<any>(`${this.url}/api/Location/town/save`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getTownSearchSummary(request) {
    return this.http.post<any>(`${this.url}/api/Location/town/search`, request)
      .pipe(map(response => { return response; }));
  }

  getCountryforCounty(id) {
    return this.http.get<any>(`${this.url}/api/Location/county/countries/${id}`);
  }

  gettownbycounty(id) {
    return this.http.get<any>(`${this.url}/api/Location/county/town/${id}`);
  }

  deleteTown(id) {
    return this.http.delete<any>(`${this.url}/api/Location/town/delete/${id}`);
  }

  async getBaseCountryDataSource(model: CountryDataSourceRequest): Promise<DataSourceResponse> {
    return await this.http.post<DataSourceResponse>(`${this.url}/api/Location/getcountrydatasource`, model).toPromise();
  }

  getCountryDataSourceList(model: CountryDataSourceRequest, term?: string) {
    model.searchText = term ? term : "";
    return this.http.post<any>(`${this.url}/api/Location/getcountrydatasource`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  getCountryListByName(item, term, countryList) {
    if (countryList && countryList.length > 0 && !term) {
      const obsFrom3 = of(countryList);
      return obsFrom3;
    }

    else {
      item.poDetails.countryRequest.searchText = "";
      if (term)
        item.poDetails.countryRequest.searchText = term;

      return this.http.post<any>(`${this.url}/api/Location/getcountrydatasource`, item.poDetails.countryRequest)
        .pipe(
          map(

            response => {
              return response.dataSourceList;
            }));
    }

  }

  GetCountriesByOffice(officeIdList: number[]) {
    return this.http.post<any>(`${this.url}/api/Location/GetCountriesByOffice`, officeIdList)
      .pipe(map(response => {
        return response;
      }));
  }

  //Get zone by search text
  getZoneDataSourceList(model: CommonZoneSourceRequest, term?: string) {
    model.searchText = term ? term : model.searchText;
    return this.http.post<any>(`${this.url}/api/Location/GetZoneDatasource`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  getProvinceDataSourceList(model: ProvinceDataSourceRequest, term?: string) {
    model.searchText = term ? term : "";
    return this.http.post<any>(`${this.url}/api/Location/getprovincedatasource`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  getCityDataSourceList(model: CityDataSourceRequest, term?: string) {
    model.searchText = term ? term : "";
    return this.http.post<any>(`${this.url}/api/Location/getcitydatasource`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  getTownDataSourceList(model: TownDataSourceRequest, term?: string) {
    model.searchText = term ? term : "";
    return this.http.post<any>(`${this.url}/api/Location/GetTownDataSource`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  getstartPortDataSourceList(model: StartPortDataSourceRequest, term?: string) {
    model.searchText = term ? term : "";
    return this.http.post<any>(`${this.url}/api/Location/GetStartPortDataSource`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }
  getOfficeCountryDataSourceList(model: CountryDataSourceRequest, term?: string) {
    model.searchText = term ? term : "";
    return this.http.post<any>(`${this.url}/api/Location/getOfficeCountryDatasource`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  getProvinceByCountryIds(request) {
    return this.http.post<any>(`${this.url}/api/Location/get-province-by-countryids`, request)
      .pipe(map(response => {
        return response;
      }));
  }

  getCityByProvinceIds(request) {
    return this.http.post<any>(`${this.url}/api/Location/get-city-by-provinceids`, request)
      .pipe(map(response => {
        return response;
      }));
  }

  getCityListBySearch(model: CityDataSourceRequest, term?: string) {
    model.searchText = term ? term : "";
    return this.http.post<any>(`${this.url}/api/Location/getcitylistbysearch`, model)
      .pipe(map(response => {
        return response;
      }));
  }
}
