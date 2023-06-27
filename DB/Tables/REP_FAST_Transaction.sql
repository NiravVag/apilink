Create Table REP_FAST_Transaction(
Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
ReportId int,
BookingId int,
Status int,
CreatedOn datetime NOT NULL DEFAULT GETDATE(),
TryCount int,
IT_Notification bit,
[ReportLink] NVARCHAR(2000) NULL, 
    CONSTRAINT FK_REP_FAST_Transaction_ReportId FOREIGN KEY (ReportId) REFERENCES FB_Report_Details(Id),
CONSTRAINT FK_REP_FAST_Transaction_BookingId FOREIGN KEY (BookingId) REFERENCES INSP_Transaction(Id),
CONSTRAINT FK_REP_FAST_Transaction_Status FOREIGN KEY (Status) REFERENCES REP_FAST_REF_Status(Id)
)