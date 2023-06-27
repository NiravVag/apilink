using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Enums
{
    public enum FBStatus
    {
        MissionDraft = 1,
        MissionConfirmed = 2,
        MissionCompleted = 3,

        ReportPreparationNotstarted = 4,
        ReportPreparationInProgress = 5,
        ReportPreparationValidated = 6,

        ReportFillingNotstarted = 7,
        ReportFillingInprogress = 8,
        ReportFillingValidated = 9,

        ReportReviewNotStarted = 10,
        ReportReviewInprogress = 11,
        ReportReviewValidated = 12,

        ReportDraft = 13,
        ReportArchive = 14,
        ReportValidated = 15,
        ReportInValidated = 16
    }


    public enum InspSummaryType
    {
        Main = 1,
        Sub = 2
    }

    public enum FBReportResult
    {
        Pass = 1,
        Fail = 2,
        Pending = 3,
        Not_Applicable = 4,
        Missing = 5,
        Conformed = 6,
        NotConformed = 7,
        Delay = 8,
        Note = 9
    }

    public enum FbReportFetch
    {
        NotValidated = 1,
        All = 2
    }


    public enum FBStatusType
    {
        Mission = 1,
        ReportFilling = 2,
        ReportReview = 3,
        Report = 4
    }

    public enum FBStatusColorcode
    {
        Orange = 1,
        White = 2,
        Green = 3,
        Red = 4,
        Black = 5
    }
}
