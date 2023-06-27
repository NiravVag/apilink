import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, first } from 'rxjs/operators';
import { TaskResponse, TaskResult, TaskMessage, TaskType, TaskSearchRequest } from '../../_Models/header/task.model';
import { NotificationResponse, NotificationSearchRequest } from '../../_Models/header/notification.model';
import { UserModel } from '../../_Models/user/user.model';
import { UtilityService } from '../common/utility.service';
import { UnReadNotificationResponse } from 'src/app/_Models/header/un-read-notification-response';

@Injectable({ providedIn: 'root' })
export class HeaderService {

  url: string;
  public taskList: Array<TaskMessage>;
  private User: UserModel;

  constructor(private http: HttpClient,public utility: UtilityService, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);

    if (localStorage.getItem('currentUser'))
      this.User = JSON.parse(localStorage.getItem('currentUser'));

  }

  public getTasks() {

    var EntityName=this.utility.getEntityName();

    this.http.get<TaskResponse>(`${this.url}/api/user/tasks`)
      .pipe()
      .subscribe(
        res => {
          if (res && res.result == TaskResult.Success) {
            this.taskList = [];
            res.data.map(x => {

              switch (x.type) {
                case TaskType.LeaveToApprove:
                  this.taskList.push(new TaskMessage(x, `Leave request pending - ${x.staffName}, take action.`, `/${EntityName}/leaverequest/leave-request/${x.linkId}`, "leave-approve.svg"));
                  break;
                case TaskType.ExpenseToApprove:
                  this.taskList.push(new TaskMessage(x, `Expense claim checked - ${x.staffName}, take action.`, `/${EntityName}/expenseclaim/expense-claim/${x.linkId}`, "expense-approved.svg"));
                  break;
                case TaskType.ExpenseToCheck:
                  this.taskList.push(new TaskMessage(x, `Expense claim pending - ${x.staffName}, take action.`, `/${EntityName}/expenseclaim/expense-claim/${x.linkId}`, "expense-checked.svg"));
                  break;
                case TaskType.ExpenseToPay:
                  this.taskList.push(new TaskMessage(x, `Expense claim approved - ${x.staffName}, take action.`, `/${EntityName}/expenseclaim/expense-claim/${x.linkId}`, "expense-paied.svg"));
                  break;
                case TaskType.InspectionVerified:
                  this.taskList.push(new TaskMessage(x, `Inspection booking to Verify - INS ${x.linkId}.`, `/${EntityName}/inspedit/edit-booking/${x.linkId}`, "inspection-verified.svg"));
                  break;
                case TaskType.InspectionConfirmed:
                  this.taskList.push(new TaskMessage(x, `Inspection booking to Confirm - INS ${x.linkId}.`, `/${EntityName}/inspedit/edit-booking/${x.linkId}`, "inspection-confirmed.svg"));
                  break;
                case TaskType.SplitInspectionBooking:
                  this.taskList.push(new TaskMessage(x, `Inspection booking to Verify (Split from INS - ${x.parentId}) - INS ${x.linkId}.`, `/${EntityName}/inspedit/edit-booking/${x.linkId}`, "splitbooking.svg"));
                  break;
                case TaskType.ScheduleInspection:
                  this.taskList.push(new TaskMessage(x, `Inspection booking to Schedule - INS ${x.linkId}.`, `/${EntityName}/schedule/schedule-summary/${x.linkId}`, "inspection-rescheduled.svg"));
                  break;
                case TaskType.QuotationPending:
                  this.taskList.push(new TaskMessage(x, `Quotation to be Create   ${x.linkId}.`, `/${EntityName}/inspsummary/quotation-pending/3/${x.linkId}`, "quoatation-default.svg"));
                  break;
                case TaskType.QuotationModify:
                  this.taskList.push(new TaskMessage(x, `Quotation to be Modify  ${x.linkId}.`, `/${EntityName}/quotations/edit-quotation/${x.linkId}`, "quoatation-modified.svg"));
                  break;
                case TaskType.QuotationSent:
                  this.taskList.push(new TaskMessage(x, `Quotation to be Sent to Client  ${x.linkId}.`, `/${EntityName}/quotations/edit-quotation/${x.linkId}`, "quoatation-sent.svg"));
                  break;
                case TaskType.QuotationCustomerConfirmed:
                  this.taskList.push(new TaskMessage(x, `Quotation to be Validate  ${x.linkId}.`, `/${EntityName}/quotations/edit-quotation/${x.linkId}`, "quoatation-confirmed.svg"));
                  break;
                case TaskType.QuotationCustomerReject:
                  this.taskList.push(new TaskMessage(x, `Quotation to be Re-Send (customer rejected)  ${x.linkId}.`, `/${EntityName}/quotations/edit-quotation/${x.linkId}`, "quoatation-rejected.svg"));
                  break;
                case TaskType.QuotationToApprove:
                  this.taskList.push(new TaskMessage(x, `Quotation to be Approve  ${x.linkId}.`, `/${EntityName}/quotations/edit-quotation/${x.linkId}`, "quoatation-approved.svg"));
                  break;
                case TaskType.UpdateCustomerToFB:
                  this.taskList.push(new TaskMessage(x, `Customer update failed in FB.To recreate  ${x.linkId}.`, `/${EntityName}/cusedit/edit-customer/${x.linkId}`, "error_notification.svg"));
                  break;
                case TaskType.UpdateSupplierToFB:
                  this.taskList.push(new TaskMessage(x, `Supplier update failed in FB.To recreate ${x.linkId}.`, `/${EntityName}/supplieredit/edit-supplier/${x.linkId}`, "error_notification.svg"));
                  break;
                case TaskType.UpdateFactoryToFB:
                  this.taskList.push(new TaskMessage(x, `Factory update failed in FB.To recreate ${x.linkId}.`, `/${EntityName}/supplieredit/edit-supplier/${x.linkId}`, "error_notification.svg"));
                  break;
                case TaskType.UpdateProductToFB:
                  this.taskList.push(new TaskMessage(x, `Product update failed in FB.To recreate  ${x.linkId}.`, `/${EntityName}/cusproductedit/edit-customer-product/${x.linkId}`, "error_notification.svg"));
                  break;
              }
            });
          }
          else {
            this.taskList = [];
          }
        },
        error => {
          console.error(error);
        });
  }

  public getNotifications(request: NotificationSearchRequest) {
    return this.http.post<NotificationResponse>(`${this.url}/api/user/notifications`,request)
      .pipe(map(response => {
        return response;
      }));
  }

  public readNotification(id: string) {
    return this.http.get<any>(`${this.url}/api/user/readnotif/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  public readAllNotification() {
    return this.http.get<any>(`${this.url}/api/user/readallnotif`)
      .pipe(map(response => {
        return response;
      }));
  }

  public unReadNotificationCount(request: NotificationSearchRequest){
    return this.http.post<UnReadNotificationResponse>(`${this.url}/api/user/unreadnotifcount`,request)
      .pipe(map(response => {
        return response;
      }));
  }

  public getTasklist(request: TaskSearchRequest) {
    return this.http.post<TaskResponse>(`${this.url}/api/user/tasklist`,request)
      .pipe(map(response => {
        return response;
      }));
  }

  public doneTask(id: string) {
    return this.http.get<any>(`${this.url}/api/user/donetask/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  public allDoneTask() {
    return this.http.get<any>(`${this.url}/api/user/alldonetask`)
      .pipe(map(response => {
        return response;
      }));
  }

  public notDoneTaskCount(request: TaskSearchRequest){
    return this.http.post<any>(`${this.url}/api/user/notdonetaskcount`,request)
      .pipe(map(response => {
        return response;
      }));
  }

}
