import { Component, NgModule } from '@angular/core';
import { HrService } from '../../../_Services/hr/hr.service'
import { first } from 'rxjs/operators';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-officeControl',
  templateUrl: './officecontrol.component.html',
  styleUrls: ['./officecontrol.component.css']
})

export class OfficeControlComponent {

  public data: any;
  loading = false;
  error = '';
  public model: Array<any>;
  public currentStaff: any;

  constructor(private service: HrService, private translate: TranslateService, private toastr: ToastrService) {
 
    this.data = service.getOfficeControls()
      .pipe()
      .subscribe(
        data => {
          if (data && data.result == 1) {
            this.data = data;
          }
          else {
            this.error = data.result;
            this.loading = false;
            // TODO check error from result
          }
          console.log(this.data);

        },
        error => {
          this.error = error;
          this.loading = false;
        });
  }

   
  getData() {
    console.log(this.currentStaff);
    this.service.getOffices(this.currentStaff.id)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1 && response.data && response.data.length > 0) {
            this.model = response.data; 
          }
          else {
            this.model = [];
          }
        },
        error => {
          this.error = error;
          this.loading = false;
        });
  }

  save() {
    this.service.saveOfficeControl(this.currentStaff.id,this.model)
      .pipe()
      .subscribe(
        response => {
          if (response && response.result == 1) {
            this.showSuccess('OFFICE_CONTROL.MSG_TITLE', 'OFFICE_CONTROL.MSG_SAVED', this.currentStaff.staffName); 
          }
          else {
            this.showError('OFFICE_CONTROL.MSG_TITLE', 'OFFICE_CONTROL.MSG_NOT_SAVED', this.currentStaff.staffName);
          }
        },
        error => {
          this.error = error;
          this.loading = false;
          this.showError('OFFICE_CONTROL.MSG_TITLE', 'OFFICE_CONTROL.MSG_NOT_SAVED', this.currentStaff.staffName);
        });
  }

  showSuccess(title: string, msg: string, parameter : string) {
    let tradTitle: string = "";
    let tradMessage: string = "";

    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

    tradMessage = tradMessage.replace("{0}", parameter);
    this.toastr.success(tradTitle, tradMessage);
  }

  showError(title: string, msg: string, parameter: string) {
    let tradTitle: string = "";
    let tradMessage: string = "";

    this.translate.get(title).subscribe((text: string) => { tradTitle = text });
    this.translate.get(msg).subscribe((text: string) => { tradMessage = text });

    tradMessage = tradMessage.replace("{0}", parameter);

    this.toastr.error(tradTitle, tradMessage);
  }
}
