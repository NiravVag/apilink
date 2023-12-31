﻿CREATE TABLE Aud_FB_Report_Checkpoints(
	Id int PRIMARY KEY IDENTITY(1,1) NOT NULL,
	AuditId int,
	ChekPointName nvarchar(1000),
	ScoreValue nvarchar(100),
	ScorePercentage nvarchar(100),
	Grade nvarchar(100),
	Remarks nvarchar(4000),
	Major nvarchar(500),
	Minor nvarchar(500),
	Critical nvarchar(500),
	ZeroTolerance nvarchar(500),
	MaxPoint nvarchar(500),
	CreatedOn datetime,
	CreatedBy int,
	Active bit,
	DeletedOn datetime,
	DeletedBy int,
	CONSTRAINT FK_Aud_FB_Report_Checkpoints_AuditId FOREIGN KEY(AuditId) REFERENCES AUD_Transaction (Id),
	CONSTRAINT FK_Aud_FB_Report_Checkpoints_CreatedBy FOREIGN KEY(CreatedBy) REFERENCES IT_UserMaster (Id),
	CONSTRAINT FK_Aud_FB_Report_Checkpoints_DeletedBy FOREIGN KEY(DeletedBy) REFERENCES IT_UserMaster (Id)
);