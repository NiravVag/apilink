import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { EmailPreviewMaster } from 'src/app/_Models/email-preview/email-preview-model';
import { EmailAddressType, EmailPreviewData, EmailPreviewValidator, EmailSendResult, EmailTempModelData, EmailValidOption } from 'src/app/_Models/email-send/edit-email-send.model';
import { UtilityService } from 'src/app/_Services/common/utility.service';
import { EditEmailSendService } from 'src/app/_Services/email-send/edit-email-send.service';
import { ReferenceService } from 'src/app/_Services/reference/reference.service';
import { Validator } from '../../common';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-email-preview',
  templateUrl: './email-preview.component.html',
  styleUrls: ['./email-preview.component.scss']
})
export class EmailPreviewComponent implements OnInit {

  emailPreviewMaster: EmailPreviewMaster;
  emailPreviewValidator: EmailPreviewValidator;
  config: AngularEditorConfig;
  componentDestroyed$: Subject<boolean> = new Subject();
  emailSelectedIndex: number = 0;
  _emailAddressType = EmailAddressType;
  _emailValidOption = EmailValidOption;

  @Input() emailModelItem: Array<EmailPreviewData>;
  @Input() serialNumberList: any;
  @Input() emailPreviewTitle:string;
  @Output() sendEmailEvent = new EventEmitter<any>();
  @Output() closeEmailPreview = new EventEmitter<any>();

  constructor(private toastr: ToastrService,
    public activeModal: NgbActiveModal, public pathroute: ActivatedRoute, public emailSendService: EditEmailSendService,
    public validator: Validator, public utility: UtilityService, private translate: TranslateService, public modalService: NgbModal,
    public referenceService: ReferenceService) {

    this.emailPreviewMaster = new EmailPreviewMaster();
    this.emailPreviewValidator = new EmailPreviewValidator();


    this.config = {
      editable: true,
      spellcheck: true,
      height: '15rem',
      minHeight: '5rem',
      placeholder: 'Enter text here...',
      translate: 'no',
      defaultParagraphSeparator: 'p',
      defaultFontName: 'Arial',
      sanitize: true,
      toolbarHiddenButtons: [
        [
          'strikeThrough',
          'subscript',
          'superscript',
          'indent',
          'outdent',
          'heading',
          'fontName',
          'fontSize',
          'customClasses',
          'unlink',
          'insertImage',
          'insertVideo',
          'insertHorizontalRule',
          'removeFormat',
          'toggleEditorMode'
        ]
      ]
    };
  }

  ngOnInit(): void {
    this.emailPreviewMaster.serialNumberList = this.serialNumberList;
    this.emailPreviewValidator.bccListValid = true;
  }

  //CheckBoxAll Emailpreview
  selectAllEmailPreview(event) {
    this.emailPreviewMaster.isSelectedEmailPreview = event.target.checked;
    this.emailModelItem.forEach(item => {
      if (event.target.checked) {
        if (this.validateEmailData(item.emailId)) {
          item.isEmailSelected = true;
        }
      } else {
        item.isEmailSelected = false;
      }
    });
    this.isSendEmailValid();
  }

  //process the div email item
  processEmailItem(emailItemIndex) {

    this.emailSelectedIndex = emailItemIndex;
    if (this.validateEmailPreviewData()) {

      this.emailModelItem[emailItemIndex].isEmailValid = true;

      if (this.emailModelItem[emailItemIndex].emailValidOption != EmailValidOption.EmailSuccess) {
        this.emailModelItem[emailItemIndex].isenabled = false;
      }

      //make the email item active
      this.emailModelItem.forEach(emailItem => {
        emailItem.active = false;
      });
      this.emailModelItem[emailItemIndex].active = true;

    }
    else {
      this.emailModelItem[emailItemIndex].isEmailValid = false;
    }
  }

  closePreview() {
    this.closeEmailPreview.next(null);
  }

  //validate the email preview data
  validateEmailPreviewData() {
    this.emailPreviewValidator = new EmailPreviewValidator();
    this.emailModelItem[this.emailSelectedIndex].toMailText = "";
    this.emailModelItem[this.emailSelectedIndex].ccMailText = "";
    this.emailModelItem[this.emailSelectedIndex].bccMailText = "";
    var isValid = true;
    //#region Email ToList Validation
    if (this.emailModelItem[this.emailSelectedIndex].emailToList) {
      //if tomail list is empty
      if (this.emailModelItem[this.emailSelectedIndex].emailToList.length == 0) {
        this.emailPreviewValidator.toListValid = false;
        this.emailPreviewValidator.toListErrorMessage = this.utility.textTranslate('EMAIL_SEND_PREVIEW.MSG_TOLIST_CANNOT_EMPTY');
        this.showError("EMAIL_SEND_PREVIEW.LBL_TITLE", "EMAIL_SEND_PREVIEW.MSG_TOLIST_CANNOT_EMPTY");
        isValid = false;
      }
      //if email to list is invalid
      else if (this.emailModelItem[this.emailSelectedIndex].emailToList.length > 0) {
        for (var i = 0; i < this.emailModelItem[this.emailSelectedIndex].emailToList.length; i++) {
          if (!this.emailIsValid(this.emailModelItem[this.emailSelectedIndex].emailToList[i])) {
            isValid = false;
            this.emailPreviewValidator.toListValid = false;
            this.emailPreviewValidator.toListErrorMessage = this.utility.textTranslate('EMAIL_SEND_PREVIEW.MSG_INVALID_EMAIL_FORMAT');
            this.showError("EMAIL_SEND_PREVIEW.LBL_TITLE", "EMAIL_SEND_PREVIEW.MSG_INVALID_EMAIL_FORMAT");
            return isValid;
          }
        }
      }
    }
    //#endregion Email ToList Validation

    //#region Email CCList Validation
    if (this.emailModelItem[this.emailSelectedIndex].emailCCList) {
      // if ccmail list is empty
      if (this.emailModelItem[this.emailSelectedIndex].emailCCList.length == 0) {
        this.emailPreviewValidator.ccListValid = false;
        this.emailPreviewValidator.ccListErrorMessage = this.utility.textTranslate('EMAIL_SEND_PREVIEW.MSG_CCLIST_CANNOT_EMPTY');
        this.showError("EMAIL_SEND_PREVIEW.LBL_TITLE", "EMAIL_SEND_PREVIEW.MSG_CCLIST_CANNOT_EMPTY");
        isValid = false;
      }
      //if email cclist is invalid
      else if (this.emailModelItem[this.emailSelectedIndex].emailCCList.length > 0) {
        for (var i = 0; i < this.emailModelItem[this.emailSelectedIndex].emailCCList.length; i++) {
          if (!this.emailIsValid(this.emailModelItem[this.emailSelectedIndex].emailCCList[i])) {
            isValid = false;
            this.emailPreviewValidator.ccListValid = false;
            this.emailPreviewValidator.ccListErrorMessage = this.utility.textTranslate('EMAIL_SEND_PREVIEW.MSG_INVALID_EMAIL_FORMAT');
            this.showError("EMAIL_SEND_PREVIEW.LBL_TITLE", "EMAIL_SEND_PREVIEW.MSG_INVALID_EMAIL_FORMAT");
            return isValid;
          }
        }
      }
    }
    //#endregion Email CCList Validation


    //#region Email BCCList Validation
    if (this.emailModelItem[this.emailSelectedIndex].emailBCCList) {
      this.emailPreviewValidator.bccListValid = true;
      //if email cclist is invalid
      if (this.emailModelItem[this.emailSelectedIndex].emailBCCList.length > 0) {
        for (var i = 0; i < this.emailModelItem[this.emailSelectedIndex].emailBCCList.length; i++) {
          if (!this.emailIsValid(this.emailModelItem[this.emailSelectedIndex].emailBCCList[i])) {
            isValid = false;
            this.emailPreviewValidator.bccListValid = false;
            this.emailPreviewValidator.bccListErrorMessage = this.utility.textTranslate('EMAIL_SEND_PREVIEW.MSG_INVALID_EMAIL_FORMAT');
            this.showError("EMAIL_SEND_PREVIEW.LBL_TITLE", "EMAIL_SEND_PREVIEW.MSG_INVALID_EMAIL_FORMAT");
            return isValid;
          }
        }
      }
    }
    //#endregion Email BCCList Validation

    //#region Email Subject Validation
    if (this.emailModelItem[this.emailSelectedIndex].emailSubject == '') {
      this.emailPreviewValidator.subjectValid = false;
      this.emailPreviewValidator.subjectErrorMessage = this.utility.textTranslate('EMAIL_SEND_PREVIEW.MSG_SUBJECT_CANNOT_EMPTY');
      this.showError("EMAIL_SEND_PREVIEW.LBL_TITLE", "EMAIL_SEND_PREVIEW.MSG_SUBJECT_CANNOT_EMPTY");
      isValid = false;
    }
    if (this.emailModelItem[this.emailSelectedIndex].emailBody == '') {
      this.emailPreviewValidator.emailBodyValid = false;
      this.emailPreviewValidator.emailBodyErrorMessage = this.utility.textTranslate('EMAIL_SEND_PREVIEW.MSG_SUBJECT_CANNOT_EMPTY');
      this.showError("EMAIL_SEND_PREVIEW.LBL_TITLE", "EMAIL_SEND_PREVIEW.MSG_BODY_CANNOT_EMPTY");
      isValid = false;
    }
    //#endregion Email Subject Validation
    return isValid;
  }

  //remove the email from the to list
  removeToMail(index) {
    this.emailModelItem[this.emailSelectedIndex].emailToList.splice(index, 1);
    if (this.emailModelItem[this.emailSelectedIndex].emailToList.length == 0) {
      this.emailPreviewValidator.toListErrorMessage = this.utility.textTranslate("EMAIL_SEND_PREVIEW.MSG_TOLIST_CANNOT_EMPTY");
      this.emailPreviewValidator.toListValid = false;
    }
  }

  //remove the email from the cc list
  removeCCMail(index) {
    this.emailModelItem[this.emailSelectedIndex].emailCCList.splice(index, 1);
    if (this.emailModelItem[this.emailSelectedIndex].emailCCList.length == 0) {
      this.emailPreviewValidator.ccListErrorMessage = this.utility.textTranslate("EMAIL_SEND_PREVIEW.MSG_TOLIST_CANNOT_EMPTY");
      this.emailPreviewValidator.ccListValid = false;
    }
  }

  //remove the email from the bcc list
  removeBCCMail(index) {
    this.emailModelItem[this.emailSelectedIndex].emailBCCList.splice(index, 1);
  }


  //handling the to list and cc list input 
  assignEmailOnKeyPress(input, type) {
    let email = '';
    //13-Enter,188-comma,32-spacebar,186-semicolon
    if ([13, 188, 32, 186].includes(input.keyCode)) {
      this.assignEmail(input, type);
    }
  }

  assignEmailOnFocusOut(input, type) {
    this.assignEmail(input, type);
  }

  //assign email to label box
  assignEmail(input, type) {
    let email = input.target.value.trim();
    if (type == this._emailAddressType.toList) {
      this.emailPreviewValidator.toListValid = true;
      this.emailPreviewValidator.toListErrorMessage = "";
    }
    else if (type == this._emailAddressType.ccList) {
      this.emailPreviewValidator.ccListValid = true;
      this.emailPreviewValidator.ccListErrorMessage = "";
    }
    else if (type == this._emailAddressType.bccList) {
      this.emailPreviewValidator.bccListValid = true;
      this.emailPreviewValidator.bccListErrorMessage = "";
    }
    if (email) {
      if (!this.emailIsValid(email)) {
        if (type == this._emailAddressType.toList) {
          this.emailPreviewValidator.toListValid = false;
          this.emailPreviewValidator.toListErrorMessage = this.utility.textTranslate('EMAIL_SEND_PREVIEW.MSG_INVALID_EMAIL_FORMAT');
        }
        else if (type == this._emailAddressType.ccList) {
          this.emailPreviewValidator.ccListValid = false;
          this.emailPreviewValidator.ccListErrorMessage = this.utility.textTranslate('EMAIL_SEND_PREVIEW.MSG_INVALID_EMAIL_FORMAT');
        }
        else if (type == this._emailAddressType.bccList) {
          this.emailPreviewValidator.bccListValid = false;
          this.emailPreviewValidator.bccListErrorMessage = this.utility.textTranslate('EMAIL_SEND_PREVIEW.MSG_INVALID_EMAIL_FORMAT');
        }

        return false;
      }
      else {
        email = email.trim().replace(',', '');
        var emailItem = this.emailModelItem[this.emailSelectedIndex];
        if (type == this._emailAddressType.toList) {
          // this.emailTempModel.toMailArray.push(email);
          if (emailItem)
            emailItem.emailToList.push(email);

          this.emailPreviewValidator.toListValid = true;
          this.emailPreviewValidator.toListErrorMessage = "";
        }
        else if (type == this._emailAddressType.ccList) {
          //  this.emailTempModel.ccMailArray.push(email);
          if (emailItem)
            emailItem.emailCCList.push(email);
          this.emailPreviewValidator.ccListValid = true;
          this.emailPreviewValidator.ccListErrorMessage = "";
        }
        else if (type == this._emailAddressType.bccList) {
          if (emailItem)
            emailItem.emailBCCList.push(email);
          this.emailPreviewValidator.bccListValid = true;
          this.emailPreviewValidator.bccListErrorMessage = "";
        }
        input.target.value = '';
      }
    }
  }

  //assign back the subject data to email preview data
  changeEmailSubject(input) {
    let subject = '';
    this.emailPreviewValidator.subjectErrorMessage = "";
    this.emailPreviewValidator.subjectValid = true;
    if (input.target.value == '') {
      this.emailPreviewValidator.subjectErrorMessage = this.utility.textTranslate("EMAIL_SEND_PREVIEW.MSG_SUBJECT_CANNOT_EMPTY");
      this.emailPreviewValidator.subjectValid = false;
      var emailItem = this.emailModelItem[this.emailSelectedIndex];
      emailItem.isEmailSelected = false;
    }
    else {
      if ([13, 188, 32, 186].includes(input.keyCode)) {
        subject = input.target.value;
        var emailItem = this.emailModelItem[this.emailSelectedIndex];
        emailItem.emailSubject = subject;
      }
    }
  }

  //clicking tab from email subject
  focusOutEmailSubject(input) {
    if (input.target.value != '') {
      let subject = input.target.value;
      var emailItem = this.emailModelItem[this.emailSelectedIndex];
      emailItem.emailSubject = subject;
    }
    else if (input.target.value == '') {
      //  var emailItem = this.emailModelItem[this.emailSelectedIndex];
      //this.emailTempModel.emailSubject = emailItem.emailSubject;
      this.emailPreviewValidator.subjectErrorMessage = "";
      this.emailPreviewValidator.subjectValid = true;

    }

  }

  onChangeEmailBody(event) {
    var emailItem = this.emailModelItem[this.emailSelectedIndex];
    emailItem.emailBody = event;
  }

  changeEmailCheckBox(emailId, event, isEmailSelected) {

    if (!this.validateEmailData(emailId)) {
      if (event.target.checked)
        event.target.checked = false;
      this.emailPreviewMaster.isEmailReady = false;
    }
    this.isSendEmailValid();
  }



  isSendEmailValid() {
    this.emailPreviewMaster.isEmailReady = false;
    var emailItems = this.emailModelItem.filter(x => x.isEmailSelected == true);
    if (emailItems && emailItems.length > 0) {
      this.emailPreviewMaster.isEmailReady = true;
      this.emailPreviewMaster.isSelectedEmailPreview = this.emailModelItem.every(function (item: EmailPreviewData) {
        return item.isEmailSelected;
      });
    }
    return this.emailPreviewMaster.isEmailReady;
  }


  validateEmailData(emailId) {
    var emailItem = this.emailModelItem[this.emailSelectedIndex];
    this.emailPreviewValidator = new EmailPreviewValidator();
    this.emailModelItem[this.emailSelectedIndex].toMailText = "";
    this.emailModelItem[this.emailSelectedIndex].ccMailText = "";
    this.emailPreviewValidator.bccListValid=true;
    var isValid = true;
    //#region Email ToList Validation
    if (emailItem.emailToList) {
      //if tomail list is empty
      if (emailItem.emailToList.length == 0) {
        this.emailPreviewValidator.toListValid = false;
        isValid = false;
      }
      //if email to list is invalid
      else if (emailItem.emailToList.length > 0) {
        for (var i = 0; i < emailItem.emailToList.length; i++) {
          if (!this.emailIsValid(emailItem.emailToList[i])) {
            isValid = false;
            this.emailPreviewValidator.toListValid = false;
            this.emailPreviewValidator.toListErrorMessage = this.utility.textTranslate('EMAIL_SEND_PREVIEW.MSG_INVALID_EMAIL_FORMAT');
            return isValid;
          }
        }
      }
    }
    //#endregion Email ToList Validation

    //#region Email CCList Validation
    if (emailItem.emailCCList) {
      //if ccmail list is empty
      if (emailItem.emailCCList.length == 0) {
        this.emailPreviewValidator.ccListValid = false;
        this.emailPreviewValidator.ccListErrorMessage = this.utility.textTranslate('EMAIL_SEND_PREVIEW.MSG_CCLIST_CANNOT_EMPTY');
        isValid = false;
      }
      //if email cclist is invalid
      else if (emailItem.emailCCList.length > 0) {
        for (var i = 0; i < emailItem.emailCCList.length; i++) {
          if (!this.emailIsValid(emailItem.emailCCList[i])) {
            isValid = false;
            this.emailPreviewValidator.ccListValid = false;
            this.emailPreviewValidator.ccListErrorMessage = this.utility.textTranslate('EMAIL_SEND_PREVIEW.MSG_INVALID_EMAIL_FORMAT');
            return isValid;
          }
        }
      }
    }
    //#endregion Email CCList Validation

    //#region Email Subject Validation
    if (emailItem.emailSubject == '') {
      this.emailPreviewValidator.subjectValid = false;
      this.emailPreviewValidator.subjectErrorMessage = this.utility.textTranslate('EMAIL_SEND_PREVIEW.MSG_SUBJECT_CANNOT_EMPTY');
      isValid = false;
    }
    if (emailItem.emailBody == '') {
      this.emailPreviewValidator.emailBodyValid = false;
      this.emailPreviewValidator.emailBodyErrorMessage = this.utility.textTranslate('EMAIL_SEND_PREVIEW.MSG_SUBJECT_CANNOT_EMPTY');
      isValid = false;
    }
    //#endregion Email Subject Validation
    return isValid;
  }

  //validate the email content
  emailIsValid(email) {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)
  }

  //sending the mail
  sendEmail() {
    this.emailPreviewMaster.isEmailSendLoading = true;
    if (this.isSendEmailValid()) {
      if (this.validateEmailPreviewData()) {
        this.sendEmailEvent.next(this.emailModelItem);
        this.emailPreviewMaster.isEmailSendLoading = true;
      }
    }
    else {
      // this.showWarning('EMAIL_SEND_PREVIEW.LBL_TITLE', 'EMAIL_SEND_PREVIEW.MSG_SELECT_ATLEAST_ONE_REPORT');
      this.emailPreviewMaster.isEmailSendLoading = false;
    }
  }
  public showError(title: string, msg: string, _disableTimeOut?: boolean) {
    let tradTitle: string = "";
    let tradMessage: string = "";

    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

    this.toastr.error(tradMessage, tradTitle, { disableTimeOut: _disableTimeOut });
  }
  public showWarning(title: string, msg: string, _disableTimeOut?: boolean) {
    let tradTitle: string = "";
    let tradMessage: string = "";

    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

    this.toastr.warning(tradMessage, tradTitle, { disableTimeOut: _disableTimeOut });
  }
}
