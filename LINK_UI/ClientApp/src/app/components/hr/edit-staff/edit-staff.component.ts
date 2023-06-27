import { Component } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { HrService } from '../../../_Services/hr/hr.service'
import { AuthenticationService } from '../../../_Services/user/authentication.service'
import { catchError, debounceTime, distinctUntilChanged, first, switchMap, takeUntil, tap } from 'rxjs/operators';
import { EditStaffModel, renewModel, jobModel, trainingModel, attachedFileModel, StaffCountyFilterMaster, EditStaffMasterModel, HROutSourceCompanyRequest, HROutSourceCompanyMaster, SaveHROutSourceCompany, SaveHROutSourceResult, hrPhotoModel } from '../../../_Models/hr/edit-staff.model'
import { UserModel } from '../../../_Models/user/user.model'
import { Validator, JsonHelper } from '../../common'
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { DetailComponent } from '../../common/detail.component';
import { HRProfileEnum, Country, HRStaffStatus, ListSize, FileUploadResponseResult, FileContainerList, RoleEnum } from '../../common/static-data-common'
import { CommonCountySourceRequest, CommonDataSourceRequest, ResponseResult } from 'src/app/_Models/common/common.model';
import { BehaviorSubject, of, Subject } from 'rxjs';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { TravelTariffService } from 'src/app/_Services/traveltariff/traveltariff.service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AttachmentFile } from 'src/app/_Models/fileupload/fileupload';
import { FileStoreService } from 'src/app/_Services/filestore/filestore.service';


@Component({
  selector: 'app-editStaff',
  templateUrl: './edit-staff.component.html',
  styleUrls: ['./edit-staff.component.css']
})
export class EditStaffComponent extends DetailComponent {
  componentDestroyed$: Subject<boolean> = new Subject();
  public model: EditStaffModel;
  public data: any;

  public subDept: Array<any>;
  public homeStates: Array<any>;
  public homeCities: Array<any>;
  public currentStates: Array<any>;
  public currentCities: Array<any>;
  public requiredArr: Array<any>;
  private jsonHelper: JsonHelper;
  public renewValidators: Array<any> = [];
  public jobValidators: Array<any> = [];
  public trainingValidators: Array<any> = [];
  private _translate: TranslateService;
  private _toastr: ToastrService;
  public _saveloader: boolean = false;
  public staffCountyFilterRequest: StaffCountyFilterMaster;
  public countyRequest: CommonCountySourceRequest;
  uploaded: any;
  url = '';
  currentUser: UserModel;
  hasPicture = false;
  hasServerPicture = false;
  loadPicture = false;
  loadingPicture = false;
  hrProfileEnum = HRProfileEnum;
  isShowForeCast = false;
  currentCountyRequired = "";
  _country = Country;
  masterModel: EditStaffMasterModel;
  public apiEntityList: any;
  public apiEntityLoading: boolean = false;
  public apiServiceList: any;
  public apiServiceLoading: boolean = false;
  public entityServiceList: any;
  public entityServiceLoading: boolean = false;
  public primaryEntityList: any;
  public primaryEntityLoading: boolean = false;
  fileLoading: any = [];
  hrOutSourceCompanyRequest: HROutSourceCompanyRequest;
  hrOutSourceCompanyList: any;
  hrOutSourceCompanyLoading = false;
  hrOutSourceCompanyListInput: BehaviorSubject<string>;
  isShowBand: boolean = false;
  saveHROutSourceCompany: SaveHROutSourceCompany;

  hrOutSourceCompanyMaster: HROutSourceCompanyMaster;

  public modelRef: NgbModalRef;
  public recentUploads: Array<AttachmentFile>;

  homeIsNotCurrent(model: EditStaffModel) { return model == null || !model.homeIsCurrent; }
  noEmployees(model: EditStaffModel) { return this.data != null && this.data.reportHeadList == null || this.data.reportHeadList.length == 0; }

  constructor(
    toastr: ToastrService,
    private authService: AuthenticationService,
    public validator: Validator,
    route: ActivatedRoute,
    public referenceService: ReferenceService,
    private service: HrService,
    router: Router,
    translate: TranslateService,
    public utility: UtilityService,
    public modalService: NgbModal,
    public fileStoreService: FileStoreService,
    private travelTariffService: TravelTariffService) {

    super(router, route, translate, toastr);
    this.recentUploads = [];
    this.jsonHelper = validator.jsonHelper;
    this.validator.setJSON("hr/edit-staff.valid.json");
    this.validator.setModelAsync(() => this.model);

    this.currentUser = authService.getCurrentUser();
    if (this.currentUser.roles.find(x => x.id == RoleEnum.HumanResource))
      this.isShowBand = true;

    this._translate = translate;
    this._toastr = toastr;
    this.staffCountyFilterRequest = new StaffCountyFilterMaster();
    this.countyRequest = new CommonCountySourceRequest();

    this.hrOutSourceCompanyRequest = new CommonDataSourceRequest();
    this.hrOutSourceCompanyListInput = new BehaviorSubject<string>("");
    this.hrOutSourceCompanyList = [];

    this.hrOutSourceCompanyMaster = new HROutSourceCompanyMaster();

    this.saveHROutSourceCompany = new SaveHROutSourceCompany();
  }

  getViewPath(): string {
    return "staffedit/view-staff";
  }

  getEditPath(): string {
    return "staffedit/edit-staff";
  }

  onInit(id?) {
    this.init(id);
  }


  init(id?) {
    this.loading = true;

    this.model = new EditStaffModel();
    this.masterModel = new EditStaffMasterModel();

    this.url = './assets/img/male.jpg';
    this.data = {};
    this.loadPicture = false;
    this.validator.isSubmitted = false;
    this.model.isForecastApplicable = true;

    if (!this.model.id)
      this.getHROutSourceCompanyBySearch();

    this.getBandList();
    this.getSocialInsuranceTypeList();
    this.getHRPayrollCompanyList();
    this.getEntityList();
    this.getServiceList();
    this.getStartportList();
    // this.waitingService.open();
    this.service.getEditStaff(id)
      .pipe()
      .subscribe(
        res => {
          this.renewValidators = [];
          this.jobValidators = [];
          this.trainingValidators = [];
          this.model.apiEntityIds = [];
          this.model.apiServiceIds = [];

          if (res && res.result == 1) {
            this.data = res;

            if (id) {
              this.model = {
                id: res.staff.id,
                employeeNo: res.staff.employeeNo,
                dateBirth: res.staff.dateBirth,
                countryId: res.staff.countryId == null ? 0 : res.staff.countryId,
                countryName: res.staff.countryName,
                staffName: res.staff.staffName,
                qualificationId: res.staff.qualificationId,
                localLanguage: res.staff.localLanguage,
                graduate: res.staff.graduate,
                emergencyContact: res.staff.emergencyContact,
                gender: res.staff.gender,
                graduateDate: res.staff.graduateDate,
                emergencyContactPhone: res.staff.emergencyContactPhone,
                martial: res.staff.martial,
                passportNo: res.staff.passportNo,
                skypeId: res.staff.skypeId,
                email: res.staff.email,
                phone: res.staff.phone,
                joinDate: res.staff.joinDate,
                positionId: res.staff.positionId,
                companyEmail: res.staff.companyEmail,
                bankName: res.staff.bankName,
                reportHeadId: res.staff.reportHeadId,
                managerId: res.staff.managerId,
                companyMobile: res.staff.companyMobile,
                bankAccount: res.staff.bankAccount,
                employeeTypeId: res.staff.employeeTypeId,
                annualLeave: res.staff.annualLeave,
                assCardNo: res.staff.assCardNo,
                officeId: res.staff.officeId,
                probExpDate: res.staff.probExpDate,
                housingFundCard: res.staff.housingFundCard,
                qcStartPlaceId: res.staff.qcStartPlaceId,
                probatonPeriod: res.staff.probatonPeriod,
                placePurchSiHf: res.staff.placePurchSiHf,
                departmentId: res.staff.departmentId,
                subdepartmentId: res.staff.subDepartmentId,
                opCountryValues: res.staff.opCountryValues,
                profileValues: res.staff.profileValues,
                opCountryItems: this.getopCountryItems(res.staff.opCountryValues),
                profileItems: this.getProfileItems(res.staff.profileValues),
                apiEntityIds: res.staff.apiEntityIds,
                apiServiceIds: res.staff.apiServiceIds,
                entityServiceIds: res.staff.entityServiceIds,
                marketSegmentValues: res.staff.marketSegmentValues,
                productCategoryValues: res.staff.productCategoryValues,
                expertiseValues: res.staff.expertiseValues,
                payrollCurrencyId: res.staff.payrollCurrencyId,
                homeCountryId: res.staff.homeCountryId == null ? 0 : res.staff.homeCountryId,
                homeStateId: res.staff.homeStateId == null ? 0 : res.staff.homeStateId,
                homeCityId: res.staff.homeCityId == null ? 0 : res.staff.homeCityId,
                homeAddress: res.staff.homeAddress,
                currentCountryId: res.staff.currentCountryId == null ? 0 : res.staff.currentCountryId,
                currentStateId: res.staff.currentStateId == null ? 0 : res.staff.currentStateId,
                currentCityId: res.staff.currentCityId == null ? 0 : res.staff.currentCityId,
                currentCountyId: res.staff.currentCountyId,
                currentAddress: res.staff.currentAddress,
                startWkDate: res.staff.startWkDate,
                workingYears: res.staff.workingYears,
                totWkYearsGarment: res.staff.totWkYearsGarment,
                isForecastApplicable: res.staff.isForecastApplicable,
                statusName: res.staff.statusName,
                statusColor: HRStaffStatus.filter(a => a.id == res.staff.statusId) && HRStaffStatus.filter(a => a.id == res.staff.statusId).length > 0 ?
                  HRStaffStatus.find(a => a.id == res.staff.statusId).color : "",
                statusId: res.staff.statusId,
                majorSubject: res.staff.majorSubject,
                bandId: res.staff.bandId,
                emergencyContactRelationship: res.staff.emergencyContactRelationship,
                globalGrading: res.staff.globalGrading,
                hukoLocationId: res.staff.hukoLocationId,
                noticePeriod: res.staff.noticePeriod,
                socialInsuranceTypeId: res.staff.socialInsuranceTypeId,
                primaryEntity: res.staff.primaryEntity,
                hrOutSourceCompanyId: res.staff.hrOutSourceCompanyId,
                companyId: res.staff.companyId,
                payrollCompany: res.staff.payrollCompany == null ? 0 : res.staff.payrollCompany,
                renewList: res.staff.renewList.map((x) => {
                  var renew: renewModel = {
                    id: x.id,
                    startDate: x.startDate,
                    endDate: x.endDate
                  };

                  this.renewValidators.push({ renew: renew, validator: Validator.getValidator(renew, "hr/edit-renew.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
                  return renew;
                }),
                homeIsCurrent: res.staff.homeIsCurrent,
                jobList: res.staff.jobList.map((x) => {

                  var job: jobModel = {
                    id: x.id,
                    company: x.company,
                    position: x.position,
                    salary: x.salary,
                    currencyId: x.currencyId,
                    startDate: x.startDate,
                    endDate: x.endDate,
                  };

                  this.jobValidators.push({ job: job, validator: Validator.getValidator(job, "hr/edit-job.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
                  return job;
                }),
                trainingList: res.staff.trainingList.map((x) => {
                  var training: trainingModel = {
                    id: x.id,
                    trainingTopic: x.trainingTopic,
                    trainer: x.trainer,
                    comments: x.comments,
                    startDate: x.startDate,
                    endDate: x.endDate,
                  }

                  this.trainingValidators.push({ training: training, validator: Validator.getValidator(training, "hr/edit-training.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
                  return training;
                }),
                attachedList: res.staff.attachedList.map((x) => {
                  var attached: attachedFileModel = {
                    id: x.id,
                    fileName: x.fileName,
                    fileTypeId: x.fileTypeId,
                    userId: x.userId,
                    userName: x.userName,
                    uploadedDate: x.uploadedDate,
                    file: null,
                    isNew: false,
                    mimeType: x.mimeType,
                    uniqueId: x.uniqueId,
                    fileUrl: x.fileUrl
                  }
                  return attached;
                }),
                hrPhoto:
                {
                  guidId: res.staff.hrPhoto?.guidId,
                  fileName: res.staff.hrPhoto?.fileName,
                  userId: res.staff.hrPhoto?.userId,
                  uniqueId: res.staff.hrPhoto?.uniqueId,
                  fileUrl: res.staff.hrPhoto?.fileUrl
                },
                picture: null,
                startPortId: res.staff.startPortId
              }
              this.onChnageEntityServiceList();
              //toggle the is forecast applicable option
              this.showHRProfile();
              this.getCountyByCityData(true);
              this.isCurrentCountyRequired();

              this.homeStates = res.homeStateList;
              this.homeCities = res.homeCityList;
              this.url = res.staff.hrPhoto?.fileUrl;
              this.currentStates = res.currentStateList;
              this.currentCities = res.currentCityList;
              this.subDept = res.subDepartmentList;

              if (this.model.attachedList == null || this.model.attachedList.length == 0)
                this.model.attachedList.push({ id: 0, fileName: "", fileTypeId: 0, userId: 0, userName: "", uploadedDate: "", file: null, isNew: false, mimeType: "", uniqueId: null, fileUrl: "" });

              if (this.model.hrPhoto == null) {
                this.model.hrPhoto.guidId = "";
                this.model.hrPhoto.userId = 0;
                this.model.hrPhoto.uniqueId = null;
                this.model.hrPhoto.fileName = "";
                this.model.hrPhoto.fileUrl = "";
              }
              this.hasServerPicture = res.staff.hasServerPicture;

            }
            this.getHukoLocationListBySearch();
            // else
            // this.validator.setModel(this.model);
            if (this.model.renewList == null || this.model.renewList.length == 0)
              this.addRenewRow();
            if (this.model.jobList == null || this.model.jobList.length == 0)
              this.addJobRow();
            if (this.model.trainingList == null || this.model.trainingList.length == 0)
              this.addTraining();

            if (this.model.hrOutSourceCompanyId)
              this.hrOutSourceCompanyRequest.id = this.model.hrOutSourceCompanyId;

            this.getHROutSourceCompanyBySearch();

            //this.waitingService.close();
          }
          else {
            this.error = res.result;
            //this.waitingService.close();
          }
          this.loading = false;
        },
        error => {
          this.setError(error);
          this.loading = false;
        });
  }

  getUrl() {
    if (!this.hasServerPicture && !this.hasPicture) {
      if (!this.model || this.model.gender == "0" || this.model.gender == 'M')
        this.url = './assets/img/male.jpg';
      else
        this.url = './assets/img/female.jpg';

      return this.url;
    }
    else if (this.hasPicture) {
      return this.url;
    }
    else if (!this.loadPicture && this.model.id != null) {
      this.loadPicture = true;
      this.loadingPicture = true;
      this.url = this.model.hrPhoto?.fileUrl;
      this.loadPicture = false;
      this.loadingPicture = false;
      return this.url;
    }
    return this.url;
  }

  onSelectFile(event) {

    if (event.target.files && event.target.files[0]) {
      this.loadingPicture = true;
      this.hasPicture = true;
      this.setBlobFile(event.target.files[0]);

      var file = event.target.files[0];
      var guid = this.newUniqueId();
      let fileItem: AttachmentFile = {
        fileName: file.name,
        file: file,
        fileSize: file.size,
        isNew: true,
        id: 0,
        status: 0,
        mimeType: file.type,
        fileUrl: "",
        uniqueld: guid,
        isSelected: false,
        fileDescription: file.fileDescription
      };
      this.recentUploads.unshift(fileItem);

      // upload to cloud - selected files and the status is new
      if (this.recentUploads) {
        this.fileStoreService.uploadFiles(FileContainerList.Hr, [fileItem]).subscribe(response => {
          if (response && response.fileUploadDataList) {
            response.fileUploadDataList.forEach(element => {
              if (element.result == FileUploadResponseResult.Sucess) {
                var item = this.model.hrPhoto;
                item.uniqueId = element.fileName;
                item.fileUrl = element.fileCloudUri;
                item.fileName = file.name;
                item.userId = this.currentUser.id;
              }
            })
          }
        },
          error => {
            this.showError('EDIT_STAFF.MSG_UPLOAD_FILE', 'EDIT_STAFF.MSG_UNKNONW_ERROR');
          });
      }
      //this.loadingPicture = false;
    }
  }

  setBlobFile(blob) {
    var reader = new FileReader();

    reader.readAsDataURL(blob); // read file as data url

    reader.onload = (event) => { // called once readAsDataURL is completed
      this.url = (<FileReaderEventTarget>event.target).result;
      this.model.picture = blob;
      this.loadingPicture = false;
    }
  }

  removePicture() {
    this.model.hrPhoto = null;
    this.hasPicture = false;
    this.hasServerPicture = false;
  }

  addRenewRow() {
    var renew: renewModel = {
      id: 0,
      startDate: "",
      endDate: ""
    };

    this.model.renewList.push(renew);
    this.renewValidators.push({ renew: renew, validator: Validator.getValidator(renew, "hr/edit-renew.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
    return renew;

  }

  removeRenewRow(index) {
    this.model.renewList.splice(index, 1);
    this.renewValidators.splice(index, 1);
  }

  addJobRow() {
    let job: jobModel = { id: 0, company: "", position: "", salary: "", currencyId: 0, startDate: "", endDate: "" };
    this.model.jobList.push(job);
    this.jobValidators.push({ job: job, validator: Validator.getValidator(job, "hr/edit-job.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) });
  }

  removejobRow(index) {
    this.model.jobList.splice(index, 1);
    this.jobValidators.splice(index, 1);
  }

  addTraining() {
    let training: trainingModel = { id: 0, trainingTopic: "", startDate: "", endDate: "", trainer: "", comments: "" };
    this.model.trainingList.push(training);
    this.trainingValidators.push({ training: training, validator: Validator.getValidator(training, "hr/edit-training.valid.json", this.jsonHelper, this.validator.isSubmitted, this._toastr, this._translate) })
  }

  removeTraining(index) {
    this.model.trainingList.splice(index, 1);
    this.trainingValidators.splice(index, 1);
  }

  addAttachment() {
    this.model.attachedList.push({ id: 0, fileName: "", fileTypeId: 0, userId: 0, userName: "", uploadedDate: "", file: null, isNew: false, mimeType: "", uniqueId: null, fileUrl: "" });
  }

  removeAttachment(index) {
    this.model.attachedList.splice(index, 1);
  }

  selectFile(event, index) {
    if (event.target.files && event.target.files[0]) {
      this.fileLoading[index] = true;
      var file = event.target.files[0];
      var guid = this.newUniqueId();
      let fileItem: AttachmentFile = {
        fileName: file.name,
        file: file,
        fileSize: file.size,
        isNew: true,
        id: 0,
        status: 0,
        mimeType: file.type,
        fileUrl: "",
        uniqueld: guid,
        isSelected: false,
        fileDescription: file.fileDescription
      };
      this.recentUploads.unshift(fileItem);

      // upload to cloud - selected files and the status is new
      if (this.recentUploads) {
        this.fileStoreService.uploadFiles(FileContainerList.Hr, [fileItem]).subscribe(response => {
          if (response && response.fileUploadDataList) {
            response.fileUploadDataList.forEach(element => {
              if (element.result == FileUploadResponseResult.Sucess) {
                var item = this.model.attachedList[index];
                var date = new Date();
                item.file = file;
                item.uniqueId = element.fileName;
                item.fileUrl = element.fileCloudUri;
                item.fileName = file.name;
                item.uploadedDate = date.getFullYear() + "-" + ("0" + (date.getMonth() + 1)).slice(-2) + "-" + ("0" + (date.getDate())).slice(-2);
                item.userName = this.currentUser.fullName;
                item.userId = this.currentUser.id;
                item.isNew = true;
              }
            })
          }
          this.fileLoading[index] = false;
        },
          error => {
            this.fileLoading[index] = false;
            this.showError('EDIT_STAFF.MSG_UPLOAD_FILE', 'EDIT_STAFF.MSG_UNKNONW_ERROR');
          });
      }
    }
  }

  newUniqueId() {
    return Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15);
  }

  getNationality() {
    if (this.model.countryId > 0) {
      var country = this.data.countryList.filter(x => x.id == this.model.countryId)[0];
      return country.countryName;
    }

    return "";
  }

  refreshSubDept(deptId) {
    this.model.subdepartmentId = 0;
    this.service.getSubDepratments(deptId)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.subDept = res.data;
          }
          else {
            this.subDept = [];
          }
        },
        error => {
          this.error = error;
          this.loading = false;
        });
  }

  addOrRemoveMS(event, id) {
    if (event.currentTarget.checked) {

      if (this.model.marketSegmentValues == null)
        this.model.marketSegmentValues = [];

      this.model.marketSegmentValues.push(id);
    }
    else {
      var index = this.model.marketSegmentValues.indexOf(id);

      if (index >= 0)
        this.model.marketSegmentValues.splice(index, 1);
    }
  }

  addOrRemovePC(event, id) {
    if (event.currentTarget.checked) {

      if (this.model.productCategoryValues == null)
        this.model.productCategoryValues = [];

      this.model.productCategoryValues.push(id);
    }
    else {
      var index = this.model.productCategoryValues.indexOf(id);

      if (index >= 0)
        this.model.productCategoryValues.splice(index, 1);
    }
  }

  addOrRemoveExpertise(event, id) {
    if (event.currentTarget.checked) {

      if (this.model.expertiseValues == null)
        this.model.expertiseValues = [];

      this.model.expertiseValues.push(id);
    }
    else {
      var index = this.model.expertiseValues.indexOf(id);

      if (index >= 0)
        this.model.expertiseValues.splice(index, 1);
    }
  }
  refreshHomeStates(countryId) {
    this.getStates(countryId).subscribe(
      res => {
        if (res && res.result == 1) {
          this.homeStates = res.data;
          this.model.homeStateId = 0;
        }
        else {
          this.homeStates = [];
        }
      },
      error => {
        this.homeStates = [];
      });
  }

  refreshCurrentStates(countryId) {
    this.getStates(countryId).subscribe(
      res => {
        if (res && res.result == 1) {
          this.currentStates = res.data;
          this.model.currentStateId = 0;
        }
        else {
          this.currentStates = [];
        }
      },
      error => {
        this.currentStates = [];
      });
  }

  refreshHomeCities(stateId) {
    this.getCities(stateId)
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.homeCities = res.data;
            this.model.homeCityId = 0;
          }
          else {
            this.homeCities = [];
          }
        },
        error => {
          this.homeCities = [];
        });
  }

  refreshCurrentCities(stateId) {
    this.getCities(stateId)
      .subscribe(
        res => {
          if (res && res.result == 1) {
            this.currentCities = res.data;
            this.model.currentCityId = 0;
          }
          else {
            this.currentCities = [];
          }
        },
        error => {
          this.currentCities = [];
        });
  }




  getStates(countryId) {
    return this.service.getStates(countryId)
      .pipe()
  }

  getCities(stateId) {
    return this.service.getCities(stateId)
      .pipe();
  }

  save() {
    this.validator.initTost();
    this.validator.isSubmitted = true;

    for (let item of this.renewValidators)
      item.validator.isSubmitted = true;

    for (let item of this.jobValidators)
      item.validator.isSubmitted = true;

    for (let item of this.trainingValidators)
      item.validator.isSubmitted = true;

    if (this.isFormValid() && this.validateEntityService()) {


      this._saveloader = true;
      //    this.waitingService.open();
      this.model.opCountryValues = this.getValues(this.model.opCountryItems);
      this.model.profileValues = this.getValues(this.model.profileItems);
      this.service.saveStaff(this.model)
        .subscribe(
          res => {
            if (res && res.result == 1) {

              let loadElement = false;
              if (!loadElement) {
                //this.waitingService.close();
                this.showSuccess('EDIT_STAFF.TITLE', 'EDIT_STAFF.MSG_SAVE_SUCCESS');
                if (this.fromSummary)
                  this.return("staffsearch/staff-summary");
                else
                  this.init();
              }
            }
            else {

              switch (res.result) {
                case 2:
                  this.showError('EDIT_STAFF.TITLE', 'EDIT_STAFF.MSG_CANNOT_ADDSTAFF');
                  break;
                case 3:
                  this.showError('EDIT_STAFF.TITLE', 'EDIT_STAFF.MSG_CURRENTSTAFF_NOTFOUND');
                  break;
                case 4:
                  this.showError('EDIT_STAFF.TITLE', 'EDIT_STAFF.MSG_CANNOTMAPREQUEST');
                  break;
                case 5:
                  this.showError('EDIT_STAFF.TITLE', 'EDIT_STAFF.MSG_CURRENTSTAFF_EXISTS_SAME_MAIL');
                  break;
              }
              // this.waitingService.close();
            }
            this._saveloader = false;
          },
          error => {
            this.showError('EDIT_STAFF.TITLE', 'EDIT_STAFF.MSG_UNKNONW_ERROR');
            this._saveloader = false;
          });
    }
  }

  getFile(file: attachedFileModel) {
    this.fileStoreService.downloadBlobFile(file.uniqueId, FileContainerList.Hr)
      .subscribe(res => {
        this.downloadFile(res, file.mimeType, file.fileName);
      },
        error => {
          this.showError('EDIT_BOOKING.TITLE', 'EDIT_BOOKING.MSG_UNKNOWN_ERROR');
        });

  }

  downloadFile(data, mimeType, filename) {
    let windowNavigator: any = window.Navigator;
    const blob = new Blob([data], { type: mimeType });
    if (windowNavigator && windowNavigator.msSaveOrOpenBlob) {
      windowNavigator.msSaveOrOpenBlob(blob, "export.xlsx");
    }
    else {
      const url = window.URL.createObjectURL(blob);
      var a = document.createElement('a');
      a.href = url;
      a.download = filename ? filename : "export";//url.substr(url.lastIndexOf('/') + 1);
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    }
  }

  getValues(items) {
    if (items == null)
      return [];

    return items.map(x => x.id);
  }

  getopCountryItems(values) {
    if (this.data.countryList == null || values == null)
      return [];
    return this.data.countryList.filter(x => values.indexOf(x.id) >= 0).map(x => { return { id: x.id, countryName: x.countryName } });
  }

  getProfileItems(values) {
    if (this.data.profileList == null || values == null)
      return [];
    return this.data.profileList.filter(x => values.indexOf(x.id) >= 0).map(x => { return { id: x.id, name: x.name } });
  }

  isFormValid() {
    return this.validator.isValid('employeeNo')
      && this.validator.isValid('dateBirth')
      && this.validator.isValid('countryId')
      && this.validator.isValid('staffName')
      && this.validator.isValid('qualificationId')
      && this.validator.isValid('gender')
      && this.validator.isValid('graduateDate')
      && this.validator.isValid('martial')
      && this.validator.isValid('email')
      && this.validator.isValid('phone')
      && this.validator.isValid('joinDate')
      && this.validator.isValid('positionId')
      && this.validator.isValid('companyEmail')
      && this.isReportHeadValid()
      && this.validator.isValid('profileItems')
      && this.validator.isValid('employeeTypeId')
      && this.validator.isValid('officeId')
      && this.validator.isValid('probExpDate')
      && this.validator.isValid('opCountryItems')
      && this.validator.isValid('departmentId')
      && this.validator.isValid('payrollCurrencyId')
      && this.validator.isValid('homeCountryId')
      && this.validator.isValid('homeStateId')
      && this.validator.isValid('homeCityId')
      && this.validator.isValid('homeAddress')
      && this.IsBandIdValid()
      && this.validator.isValid('companyId')
      && this.validator.isValid('payrollCompany')
      && this.validator.isValid('startWkDate')
      && this.validator.isValidIfAsync('currentCountryId', this.homeIsNotCurrent)
      && this.validator.isValidIfAsync('currentStateId', this.homeIsNotCurrent)
      && this.validator.isValidIfAsync('currentCityId', this.homeIsNotCurrent)
      && this.validator.isValidIfAsync('currentAddress', this.homeIsNotCurrent)
      && this.isCrrentCountyValid()
      && this.renewValidators.every((x) => x.validator.isValid('startDate') && x.validator.isValid('endDate'))
      && this.jobValidators.every((x) => x.validator.isValid('startDate') && x.validator.isValid('endDate'))
      && this.trainingValidators.every((x) => x.validator.isValid('startDate') && x.validator.isValid('endDate'))
      && this.validator.isValid('apiServiceIds')
      && this.validator.isValid('apiEntityIds')
      && this.validator.isValid('primaryEntity')
      && this.validator.isValid('entityServiceIds');
  }
  IsBandIdValid() {
    if (this.isShowBand)
      return this.validator.isValid('bandId');
    else
      return true;
  }

  isReportHeadValid(): boolean {
    if (!this.validator.isSubmitted)
      return true;

    if (this.data.reportHeadList == null || this.data.reportHeadList.length == 0)
      return true;

    return this.validator.isValid('reportHeadId');

  }
  changeisforecastApplicable(event) {
    this.isCurrentCountyRequired();
  }
  changeOpCountry(event) {
    this.isCurrentCountyRequired();
  }

  isCurrentCountyRequired() {

    if (this.model.opCountryItems == null || this.model.opCountryItems.length == 0)
      return "";

    let _OpCountryCHINA = this.model.opCountryItems.filter(
      country => country.id === this._country.China).length;

    if (this.model.isForecastApplicable && _OpCountryCHINA > 0)
      this.currentCountyRequired = "required";
    else
      this.currentCountyRequired = "";

  }
  //check currenty county based on operational country only for  CHINA and isforecastapplicable is true
  isCrrentCountyValid(): boolean {

    if (!this.validator.isSubmitted)
      return true;

    if (this.model.opCountryItems == null || this.model.opCountryItems.length == 0)
      return true;

    let _OpCountryCHINA = this.model.opCountryItems.filter(
      country => country.id === this._country.China).length;

    if (this.model.isForecastApplicable && _OpCountryCHINA > 0)
      return this.validator.isValid('currentCountyId');
    else
      return true;


  }


  /**
   * function to toggle tabs on click
   * @param {event} event     [current event]
   * @param {string} tabTarget [targeted tab id]
   */
  toggleTab(event, tabTarget) {
    let tabs = event.target.parentNode.children;
    for (let tab of tabs) {
      tab.classList.remove('active');
    }
    event.target.classList.add('active');

    let tabContainers = document.querySelector('#' + tabTarget).parentNode.childNodes;
    for (let container of <any>tabContainers) {
      container.classList.remove('active');
    }
    document.getElementById(tabTarget).classList.add('active');
  }

  getPicure() {
    document.getElementById("fileInput").click();
  }

  //show isForecastApplicable checkbox if hr profile is inspector
  showHRProfile() {
    this.isShowForeCast = false;
    var isInspector = this.model.profileItems.find(x => x.id == this.hrProfileEnum.Inspector);
    if (isInspector) {
      this.isShowForeCast = true;
    }
    else {
      this.model.isForecastApplicable = false;
    }
  }


  refreshCurrentCounty(countyId) {
    this.staffCountyFilterRequest = new StaffCountyFilterMaster();
    this.model.currentCountyId = null;
    this.model.currentCityId = countyId;
    this.getCountyByCityData(true);
  }

  //fetch the County data with virtual scroll
  getCountyByCityData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.countyRequest.searchText = this.staffCountyFilterRequest.countyInput.getValue();
      this.countyRequest.skip = this.staffCountyFilterRequest.countyList.length;
    }
    this.countyRequest.cityId = this.model.currentCityId;
    this.staffCountyFilterRequest.countyLoading = true;
    this.service.getCountyByCityDataSourceList(this.countyRequest).
      subscribe(countyData => {
        if (countyData && countyData.length > 0) {
          this.staffCountyFilterRequest.countyList = this.staffCountyFilterRequest.countyList.concat(countyData);
        }
        if (isDefaultLoad)
          this.countyRequest = new CommonCountySourceRequest();
        this.staffCountyFilterRequest.countyLoading = false;
      }),
      error => {
        this.staffCountyFilterRequest.countyLoading = false;
        this.setError(error);
      };
  }

  //fetch the first 10 counties on load
  getCountyByCityListBySearch() {
    this.staffCountyFilterRequest.countyInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.staffCountyFilterRequest.countyLoading = true),
      switchMap(term => term
        ? this.service.getCountyByCityDataSourceList(this.countyRequest, term)
        : this.service.getCountyByCityDataSourceList(this.countyRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.staffCountyFilterRequest.countyLoading = false))
      ))
      .subscribe(data => {
        this.staffCountyFilterRequest.countyList = data;
        this.staffCountyFilterRequest.countyLoading = false;
      });
  }

  //fetch the first 10 city on load
  getHukoLocationListBySearch() {
    if (this.model.hukoLocationId && this.model.hukoLocationId > 0) {
      this.masterModel.hukoLocationModelRequest.id = this.model.hukoLocationId;
    }
    else {
      this.masterModel.hukoLocationModelRequest.id = null;
    }

    this.masterModel.hukoLocationInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.masterModel.hukoLocationLoading = true),
      switchMap(term => term
        ? this.service.getHukoLocationDataSourceList(this.masterModel.hukoLocationModelRequest, term)
        : this.service.getHukoLocationDataSourceList(this.masterModel.hukoLocationModelRequest)
          .pipe(
            catchError(() => of([])), // empty list on error
            tap(() => this.masterModel.hukoLocationLoading = false))
      ))
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(data => {
        this.masterModel.hukoLocationList = data;
        this.masterModel.hukoLocationLoading = false;
      });
  }

  //scroll end call to add data
  getHukoLocationData() {
    this.masterModel.hukoLocationModelRequest.searchText = this.masterModel.hukoLocationInput.getValue();
    this.masterModel.hukoLocationModelRequest.skip = this.masterModel.hukoLocationList.length;

    // if (this.model.hukoLocationId && this.model.hukoLocationId > 0) {
    //   this.masterModel.hukoLocationModelRequest.id = this.model.hukoLocationId;
    // }
    // else {
    //   this.masterModel.hukoLocationModelRequest.id = null;
    // }

    this.masterModel.hukoLocationLoading = true;
    this.service.getHukoLocationDataSourceList(this.masterModel.hukoLocationModelRequest)
      .pipe(takeUntil(this.componentDestroyed$)).
      subscribe(data => {
        if (data && data.length > 0) {
          this.masterModel.hukoLocationList = this.masterModel.hukoLocationList.concat(data);
        }
        this.masterModel.hukoLocationModelRequest = new CommonDataSourceRequest();
        this.masterModel.hukoLocationLoading = false;
      }),
      error => {
        this.masterModel.hukoLocationLoading = false;
        this.setError(error);
      };
  }

  //clear huko location
  clearHukoLocation() {
    this.getHukoLocationListBySearch();
  }

  //get social insurance type list
  getSocialInsuranceTypeList() {
    this.masterModel.socialInsuranceTypeLoading = true;
    this.service.getSocialInsuranceTypeList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.socialInsuranceTypeList = response.dataSourceList;

          this.masterModel.socialInsuranceTypeLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.socialInsuranceTypeLoading = false;
        });
  }

  //get band list
  getBandList() {
    this.masterModel.bandLoading = true;
    this.service.getBandList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.bandList = response.dataSourceList;

          this.masterModel.bandLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.bandLoading = false;
        });
  }
  //numeric validation for maxItems
  numericValidation(event) {
    this.utility.numericValidation(event, 3);
  }


  //get entity list
  getEntityList() {
    this.apiEntityLoading = true;
    this.referenceService.getEntityList()
      .pipe(first())
      .subscribe(
        response => {
          this.apiEntityLoading = false;
          if (response && response.result == ResponseResult.Success) {
            this.apiEntityList = response.dataSourceList;
            let entity: string = this.utility.getEntityName();
            this.model.companyId = this.apiEntityList?.find(x => x.name == entity).id;
          }
        },
        error => {
          this.apiEntityLoading = false;
          this.setError(error);
        });
  }

  //get entity list
  getServiceList() {
    this.apiServiceLoading = true;
    this.referenceService.getServiceList()
      .pipe(first())
      .subscribe(
        response => {
          this.apiServiceLoading = false;
          if (response && response.result == ResponseResult.Success) {
            this.apiServiceList = response.dataSourceList;
          }
        },
        error => {
          this.apiServiceLoading = false;
          this.setError(error);
        });
  }

  //get hr payroll company list
  getHRPayrollCompanyList() {
    this.masterModel.payrollCompanyLoading = true;
    this.service.getHRPayrollCompanyList()
      .pipe(takeUntil(this.componentDestroyed$))
      .subscribe(
        response => {
          if (response && response.result == ResponseResult.Success)
            this.masterModel.payrollCompanyList = response.hrPayrollCompanyList;

          this.masterModel.payrollCompanyLoading = false;
        },
        error => {
          this.setError(error);
          this.masterModel.payrollCompanyLoading = false;
        });
  }

  onClearEntityList() {
    this.model.entityServiceIds = null;
    this.model.primaryEntity = null;
  }

  onClearServiceList() {
    this.model.entityServiceIds = null;
  }

  onChnageEntityServiceList() {
    this.entityServiceList = [];
    this.primaryEntityList = [];
    if (this.model.apiServiceIds && this.model.apiEntityIds && this.apiEntityList && this.apiServiceList) {
      let index = 1;

      this.primaryEntityList = this.apiEntityList.filter(x => this.model.apiEntityIds.includes(x.id));
      if (this.primaryEntityList.find(x => x.id == this.model.primaryEntity) == undefined) {
        this.model.primaryEntity = null;
      }

      this.model.apiServiceIds.forEach(service => {
        this.model.apiEntityIds.forEach(entity => {
          var entityName = this.apiEntityList.find(x => x.id == entity).name;
          var serviceName = this.apiServiceList.find(x => x.id == service).name;
          var entityService = entityName + "(" + serviceName + ")";

          this.entityServiceList.push({ id: index, entityId: entity, serviceId: service, name: entityService });
          index = index + 1;
        })
      })

      this.model.entityServiceIds = this.model.entityServiceIds.filter(y => this.model.apiEntityIds.includes(y.entityId) && this.model.apiServiceIds.includes(y.serviceId))
    }
  }

  async getStartportList() {
    this.masterModel.startingPortNameListLoading = true;
    var response = await this.travelTariffService.getStartPortList();
    if (response && response.result == ResponseResult.Success) {
      this.masterModel.startingPortNameList = response.dataSourceList;
    }
    this.masterModel.startingPortNameListLoading = false;
  }

  getHROutSourceCompanyBySearch() {

    this.hrOutSourceCompanyListInput.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.hrOutSourceCompanyLoading = true),
      switchMap(term => term
        ? this.service.getHROutSourceCompanyList(this.hrOutSourceCompanyRequest, term)
        : this.service.getHROutSourceCompanyList(this.hrOutSourceCompanyRequest)
          .pipe(takeUntil(this.componentDestroyed$),
            catchError(() => of([])), // empty list on error
            tap(() => this.hrOutSourceCompanyLoading = false))
      ))
      .subscribe(data => {
        this.hrOutSourceCompanyList = data;
        this.hrOutSourceCompanyLoading = false;
      });
  }

  //fetch the customer data with virtual scroll
  getHROutSourceCompanyData(isDefaultLoad: boolean) {
    if (isDefaultLoad) {
      this.hrOutSourceCompanyRequest.searchText = this.hrOutSourceCompanyListInput.getValue();
      this.hrOutSourceCompanyRequest.skip = this.hrOutSourceCompanyList.length;
    }

    this.hrOutSourceCompanyLoading = true;
    this.service.getHROutSourceCompanyList(this.hrOutSourceCompanyRequest).
      pipe(takeUntil(this.componentDestroyed$), first()).
      subscribe(hrOutSourceCompanyData => {

        if (hrOutSourceCompanyData && hrOutSourceCompanyData.length > 0) {
          this.hrOutSourceCompanyList = this.hrOutSourceCompanyList.concat(hrOutSourceCompanyData);
        }
        if (isDefaultLoad) {
          //this.request = new CommonDataSourceRequest();
          this.hrOutSourceCompanyRequest.skip = 0;
          this.hrOutSourceCompanyRequest.take = ListSize;
        }
        this.hrOutSourceCompanyLoading = false;
      }),
      error => {
        this.hrOutSourceCompanyLoading = false;
        this.setError(error);
      };
  }

  clearHROutSourceCompany() {
    this.hrOutSourceCompanyRequest.id = null;
    this.getHROutSourceCompanyBySearch();
  }

  //open the hr outsource company details
  addHROutSourceCompanyName(content) {
    this.saveHROutSourceCompany.name = null;
    this.modelRef = this.modalService.open(content, { windowClass: "smModelWidth", centered: true, backdrop: 'static' });
  }

  saveHROutSourceCompanyData() {
    if (!this.saveHROutSourceCompany.name)
      this.showWarning('EDIT_STAFF.TITLE', 'EDIT_STAFF.MSG_HR_OTS_COMPANY_NAME_REQ');
    else {
      this.service.saveHROutSourceCompany(this.saveHROutSourceCompany)
        .subscribe(
          res => {
            if (res && res.result == SaveHROutSourceResult.Success) {

              this.showSuccess('EDIT_STAFF.TITLE', 'EDIT_STAFF.MSG_HR_OTS_COMPANY_SAVE_SUCCESS');
              this.modelRef.close();
              this.hrOutSourceCompanyRequest.id = null;
              this.getHROutSourceCompanyBySearch();
            }
            else {

              switch (res.result) {
                case SaveHROutSourceResult.NameAlreadyExists:
                  this.showError('EDIT_STAFF.TITLE', 'EDIT_STAFF.MSG_HR_OTS_COMPANY_NAME_ALREADY_EXISTS');
                  break;
                case SaveHROutSourceResult.Failure:
                  this.showError('EDIT_STAFF.TITLE', 'EDIT_STAFF.MSG_UNKNONW_ERROR');
                  break;
              }
              // this.waitingService.close();
            }
            this._saveloader = false;
          },
          error => {
            this.showError('EDIT_STAFF.TITLE', 'EDIT_STAFF.MSG_UNKNONW_ERROR');
            this._saveloader = false;
          });
    }

  }
  validateEntityService() {
    let result = this.model.apiEntityIds.every(y => this.model.entityServiceIds.map(z => z.entityId).includes(y))

    if (!result) {
      this.showWarning('EDIT_STAFF.TITLE', 'EDIT_SUPPLIER.MSG_ENTITY_SERVICE_REQUIRED');
    }
    return result;
  }

}


interface FileReaderEventTarget extends EventTarget {
  result: string
}

interface FileReaderEvent extends Event {
  target: FileReaderEventTarget;
  getMessage(): string;
}


