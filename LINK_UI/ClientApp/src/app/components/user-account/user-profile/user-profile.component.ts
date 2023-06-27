import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbPanel } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { AttachmentFile } from 'src/app/_Models/fileupload/fileupload';
import { UserProfileModel, ResponseResult } from 'src/app/_Models/useraccount/userprofile.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { FileStoreService } from 'src/app/_Services/filestore/filestore.service';
import { UserProfileService } from 'src/app/_Services/UserAccount/userprofile.service';
import { DetailComponent } from '../../common/detail.component';
import { JsonHelper } from '../../common/jsonHelper';
import { FileContainerList } from '../../common/static-data-common';
import { Validator } from '../../common/validator';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent extends DetailComponent {

  activeIds: Array<any>;
  url: string = 'assets/images/user-upload.svg';
  public model: UserProfileModel;
  public uploadfileimage: string = "assets/images/user-upload.svg";
  public smallUploadButtonText = "Upload";
  public uploadLimit = 1;
  public fileSize = 5000000;
  public uploadFileExtensions = 'png,jpg,jpeg';
  public pageLoader: boolean = false;
  public saveLoader: boolean = false;
  public jsonHelper: JsonHelper;
  public loadingPicture = false;

  getViewPath(): string {
    return "";
  }
  getEditPath(): string {
    return "";
  }
  constructor(private modalService: NgbModal, router: Router, public validator: Validator,
    route: ActivatedRoute, translate: TranslateService, toastr: ToastrService, 
    private service: UserProfileService,public utility: UtilityService,
    private fileService: FileStoreService) {

    super(router, route, translate, toastr, modalService);
  }

  onInit(id?: any) {
    this.initValidator(); 
    this.initialize(id);
  }

  initialize(id) {
    this.model = new UserProfileModel();

    this.getData(id);

  }

  initValidator(){
    this.validator.isSubmitted = false;
    this.validator.setJSON("useraccount/user-profile.valid.json");
    this.validator.setModelAsync(() => this.model);
    this.jsonHelper = this.validator.jsonHelper;
  }

  async getData(userId) {
    this.pageLoader = true;
    var response = await this.service.getUserProfileSummary(userId);

    if (response.result == ResponseResult.Success)
      this.mapProfileData(response.data);

      this.pageLoader = false;
  }

  mapProfileData(response) {
    this.model = {
      userId: response.userId,
      userName: response.userName,
      displayName: response.displayName,
      emailId: response.emailId,
      phone: response.phone,
      profileImageName: response.profileImageName,
      profileImageUrl: response.profileImageUrl
    }
  }

  async save() {
    this.saveLoader = true;
    this.validator.isSubmitted = true;
    if(this.isformValid()) {
    var res = await this.service.save(this.model);

    if (res.result == ResponseResult.Success) {
      this.showSuccess('USER_PROFILE.TITLE', 'USER_PROFILE.MSG_SAVE_SUCCESSFUL');
      this.saveLoader = false;
    }

    else if(res.result == ResponseResult.EmailAlreadyExists) {
      this.showError('USER_PROFILE.TITLE', 'USER_PROFILE.MSG_EMAIL_EXISTS');
      this.saveLoader = false;
    }

    else {
      this.showError('USER_PROFILE.TITLE', 'EDIT_INSPECTION_CERTIFICATE.MSG_SAVE_FAILED');
      this.saveLoader = false;
    }
  }
  this.saveLoader = false;
  }

  getPicure() {
    document.getElementById("fileInput").click();
  }

  onSelectFile(event) {
    var fileType = event.target.files[0].name.split('.')[1].toLowerCase()
    var typeExists = this.uploadFileExtensions.includes(fileType);

    if(typeExists) {
      this.loadingPicture = true;
    this.cloudUpload(event);
    }

    else {
      this.showError('USER_PROFILE.TITLE', 'USER_PROFILE.LBL_FILE_FORMAT');
    }
  }

  setBlobFile(blob) {
    var reader = new FileReader();

    reader.readAsDataURL(blob); // read file as data url

    reader.onload = (event) => { // called once readAsDataURL is completed
      this.url = (<FileReaderEventTarget>event.target).result;
      //this.model.picture = blob;
      //this.loadingPicture = false;
    }
  }

  getUrl() {
    return this.model.profileImageUrl != null ? this.model.profileImageUrl : this.url;
  }

  cloudUpload(event) {
    var uploadFiles = [];
    if (event && !event.error && event.target.files) {

      if (event.target.files.length > this.uploadLimit) {
        this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', 'EDIT_CUSTOMER_PRODUCT.FILEUPLOAD_LIMIT_EXCEEDED')
      }
      else {
       // this.pageLoader = true;
        for (let file of event.target.files) {
          var guid = this.newUniqueId();
          let fileItem: AttachmentFile = {
            fileName: file.name,
            file: file,
            fileSize:file.fileSize,
            isNew: true,
            id: 0,
            status: 0,
            mimeType: file.type,
            fileUrl: "",
            uniqueld: guid,
            isSelected: false,
						fileDescription:null
          };
          uploadFiles.unshift(fileItem);

          // upload to cloud - selected files and the status is new
          if (uploadFiles) {

            this.fileService.uploadFiles(FileContainerList.Hr, [fileItem]).subscribe(response => {
              if (response && response.fileUploadDataList) {
                this.model.profileImageUrl = response.fileUploadDataList[0].fileCloudUri;
                this.model.profileImageName = response.fileUploadDataList[0].fileName;
                this.url = this.model.profileImageUrl;
                this.loadingPicture = false;
              }
              this.pageLoader = false;
            },
              error => {
                //this.pageLoader = false;
                this.loadingPicture = false;
                this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', 'EDIT_CUSTOMER_PRODUCT.MSG_UNKNONW_ERROR');
              });
          }
        }
      }
    }
    else {
      this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', event.errorMessage);
    }
  }

  newUniqueId() {
    return Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15);
  }

  isformValid() {
    return this.validator.isValid('displayName')
      && this.validator.isValid('emailId')
  }

}



interface FileReaderEventTarget extends EventTarget {
  result: string
}

interface FileReaderEvent extends Event {
  target: FileReaderEventTarget;
  getMessage(): string;
}
