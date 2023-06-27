import { NotificationFilterType, NotificationMessage } from "./notification.model";
import { TaskFilterType, TaskMessage } from "./task.model";

export class HeaderMasterModel {
    public notificationList: Array<NotificationMessage> = [];
    public taskList: Array<TaskMessage> = [];

    // notification
    notifiactionSkip: number = 0;
    notifiactionLoader: boolean;
    notificationCount: ManageCount = new ManageCount();
    notificationType: any;
    todayNotificationList: NotificationMessage[];
    yesterdayNotificationList: NotificationMessage[];
    olderNotificationList: NotificationMessage[];
    notificationTypeList: { id: NotificationFilterType; name: string; }[];
    notificationIsUnread?: boolean;
    notifiactionTotalCount: number = 0;
    
    // task
    taskSkip: number = 0;
    taskLoader: boolean;
    taskCount: ManageCount = new ManageCount();
    taskType: any;
    todayTaskList: TaskMessage[];
    yesterdayTaskList: TaskMessage[];
    olderTaskList: TaskMessage[];
    taskTypeList: { id: TaskFilterType; name: string; }[];
    taskIsUnread?: boolean;
    taskTotalCount: number = 0;
    //Manage Header Active Class
    
	showNotificationPanel: boolean = false;
	showTaskPanel: boolean = false;
	showHelpPanel: boolean = false;
	showUserPanel: boolean = false;
}
export class ManageCount{
    todayCount:number = 0;
    yesterdayCount:number = 0;
    olderCount:number = 0;
    totalCount:number = 0;
}
