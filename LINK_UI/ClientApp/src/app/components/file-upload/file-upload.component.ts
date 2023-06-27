import { Component, OnInit, Input } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FileStoreService } from 'src/app/_Services/filestore/filestore.service';
import { FileContainerList, FileUploadResponseResult } from '../common/static-data-common';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { AttachmentFile, FileInfo } from 'src/app/_Models/fileupload/fileupload';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.scss']
})
export class FileUploadComponent implements OnInit {

  fileInfo: FileInfo;

  callFromBookingPage: any;

  private _translate: TranslateService;
  private _toastr: ToastrService;
  public recentUploads: Array<AttachmentFile>;
  public fileAttachments: Array<AttachmentFile>;
  public pageLoader: boolean = false;
  public fileDescription: string;

  constructor
    (
      translate: TranslateService,
      toastr: ToastrService,
      public activeModal: NgbActiveModal,
      public fileStoreService: FileStoreService,
      public utilityService: UtilityService
    ) {
    this.recentUploads = [];
    this.fileAttachments = [];
    this._translate = translate;
    this._toastr = toastr;
  }

  @Input() fromParent;


  @Input() callFromBooking;

  ngOnInit() {
    this.fileInfo = this.fromParent;
    this.callFromBookingPage = this.callFromBooking;

    //if (this.callFromBooking)
    //  this.fromBooking = true;

    if (!this.callFromBookingPage) {
      // load recent uploads
      var dataUploads = JSON.parse(localStorage.getItem('recentUploads'));

      if (dataUploads) {
        dataUploads.forEach(element => {

          // Make array of file extensions
          var extensions = (this.fileInfo.uploadFileExtensions.split(','))
            .map(function (x) { return x.toLocaleUpperCase().trim() });

          // Get file extension
          var ext = element.fileName.toUpperCase().split('.').pop() || element.fileName;
          // Check the extension exists
          var exists = extensions.includes(ext);

          if (exists) {
            element.status = null;
            element.isSelected = false;
            this.recentUploads.push(element)
          }
        });
      }
    }
  }


  closeModal(sendData) {
    this.activeModal.close(sendData);
  }

  public showError(title: string, msg: string, _disableTimeOut?: boolean) {
    let tradTitle: string = "";
    let tradMessage: string = "";

    this._translate.get(title).subscribe((text: string) => { tradTitle = text });
    this._translate.get(msg).subscribe((text: string) => { tradMessage = text });

    this._toastr.error(tradMessage, tradTitle, { disableTimeOut: _disableTimeOut });
  }

  insertFile() {
    this.fileAttachments = [];
    var selectedItem = this.recentUploads.filter(x => x.isSelected);

    if (selectedItem.length > 0) {
      selectedItem.forEach(element => {
        this.fileAttachments.push(element);
      });

      this.activeModal.close(this.fileAttachments);
    }
    else {
      this.showError("EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE", "EDIT_CUSTOMER_PRODUCT.MSG_SELECT_FILE");
    }
  }

  getFile(file: AttachmentFile) {
    this.pageLoader = true;
    this.fileStoreService.downloadBlobFile(file.uniqueld, this.fileInfo.containerId)
      .subscribe(res => {
        this.downloadFile(res, file.mimeType);
      },
        error => {
          this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', 'EDIT_CUSTOMER_PRODUCT.MSG_UNKNONW_ERROR');
        });

  }

  downloadFile(data, mimeType) {
    const blob = new Blob([data], { type: mimeType });
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob, "export.xlsx");
    }
    else {
      const url = window.URL.createObjectURL(blob);
      var a = document.createElement('a');
      a.href = url;
      a.download = url.substr(url.lastIndexOf('/') + 1);
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    }
    this.pageLoader = false;
  }


  async selectFiles(event) {
    if (event && !event.error && event.files) {

      if (event.files.length > this.fileInfo.uploadLimit) {
        this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', 'EDIT_CUSTOMER_PRODUCT.FILEUPLOAD_LIMIT_EXCEEDED')
      }
      else {

        if (!this.callFromBookingPage)
          var dataUploads = JSON.parse(localStorage.getItem('recentUploads'));

        this.recentUploads = [];

        if (dataUploads) {
          this.recentUploads = dataUploads;
          this.recentUploads.forEach(element => {
            element.status = null;
            element.isSelected = false;
          });
        }
        for (let file of event.files) {

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

        }

        //upload to cloud
        for (let file of this.recentUploads) {
          await this.uploadFileData(file);
        }


      }


    }
    else if (event && event.error && event.errorMessage) {
      this.showError('EDIT_CUSTOMER_PRODUCT.MSG_UPLOAD_FILE', event.errorMessage);
    }
  }

  async uploadFileData(fileItem) {

    debugger;
    // upload to cloud - selected files and the status is new

    if (this.recentUploads) {

      var response = await this.fileStoreService.uploadFileData(this.fileInfo.containerId, [fileItem]);
     
      if (response && response.fileUploadDataList) {

        if (response.fileUploadDataList.length > 0) {
          var element = response.fileUploadDataList[0];
          if (element.result == FileUploadResponseResult.Sucess) {

            //Find index of specific object using findIndex method.    
            let objIndex = this.recentUploads.findIndex((x => x.uniqueld == element.fileName));

            if (objIndex > -1) {
              this.recentUploads[objIndex].fileUrl = element.fileCloudUri;
              this.recentUploads[objIndex].status = 100;
              this.recentUploads[objIndex].isSelected = true;

              // copy the items
              var cloneUploads = [...this.recentUploads];
              localStorage.setItem('recentUploads', JSON.stringify(cloneUploads));

            }
          }
          else {
            // handle failure case
          }
        }

        /* response.fileUploadDataList.forEach(element => {



        }); */

      }

    }
  }




  removeRecentUploads(index) {
    this.recentUploads.splice(index, 1);
    // copy the items and update local storage
    var cloneUploads = [...this.recentUploads];
    localStorage.setItem('recentUploads', JSON.stringify(cloneUploads));
  }

  newUniqueId() {
    return Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15);
  }
}
