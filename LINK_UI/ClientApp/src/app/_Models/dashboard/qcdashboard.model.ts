export class QcDashboardSearchRequest
{
   
    public customerId:number;
    public serviceDateFrom: any;
    public serviceDateTo: any;
}
export class QcDashboard {
    public rejectionChartLoading: boolean;
    public productivityChartLoading: boolean;
    public isProductivityChartRender: boolean;
}
export class QcDashboardCalendarResponse {
    public todayCount: number;
    public tomorrowCount: number;
    public upcomingAllocatedCount: number;
    public result: QcDashboardResponseResult;
    public qcCalendar: Array<QcDashboardCalendar>;
}

export class QcDashboardCalendar {
    public serviceDateFrom: string;
    public calendarDay: number;
    public calendarDayName: string;
    public calendarDate: number;
    public calendarMonth: number;
    public calendarMonthName: string;
    public dayType: number;
    public dayClass: string;
    public qcCalendarSchedule: Array<QcDashboardCalendarScheduleItem>;
}

export class QcDashboardCalendarScheduleItem {
    public tooltipIds: string;
    public bookingIds: string;
    public bookingIdClass: string;
    public factoryId: number;
    public factoryName: string;
    public factoryAddress: string;
    public statusName: string;
    public statusId: number;
    public serviceDateFrom: string;
    public serviceDateTo: string;
}
export class QcDashboardChartReportResponse {
    
    public result: QcDashboardResponseResult;
    public qcReportscount: Array<QcDashboardChartReportItem>;
}
export class QcDashboardChartReportItem {
    public reportCount: number;
    public serviceDate: any;
     
}

export class QcRejectionReportsResponse {
    public inspectedBooking: number;
    public rejectionBooking: number;
    public qcRejectionBooking: number;
    public rejectionPercentage: number;
    public qcRejectionPercentage: number;
}
export class QcDashboardCountResponse {
    public customerCount: number;
    public factoryCount: number;
    public inspectionCount: number;
    public reportCount: number;
}
 
export enum QcDashboardResponseResult
{
    Success = 1,
    NotFound = 2,
    Other = 3
}
