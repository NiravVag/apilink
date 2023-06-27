﻿CREATE TABLE [dbo].[CU_Contact]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY ,
	[ZohoCustomerId] bigint,
	[ZohoContactId] bigint,
	[Customer_id] [int] NOT NULL,
	[Contact_name] [nvarchar](200) NOT NULL,
	[Job_Title] NVARCHAR(250) NULL, 
    [Email] NVARCHAR(100) NOT NULL, 
    [Mobile] NVARCHAR(20) NULL, 
    [Phone] NVARCHAR(100) NOT NULL, 
    [Fax] NVARCHAR(200) NULL, 
    [Others] NVARCHAR(200) NULL, 
    [Office] INT  NULL, 
    [Comments] NVARCHAR(200) NULL, 
    [Promotional_Email] BIT NULL, 
    [Active] BIT NOT NULL DEFAULT 1, 
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
	[EntityId] INT NULL,
	[PrimaryEntity] [int] NULL,
	[ReportTo] INT NULL,
    [LastName] NVARCHAR(500),
    CONSTRAINT CU_Contact_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id),
    FOREIGN KEY ([Customer_id]) REFERENCES [dbo].[CU_Customer](Id),
	FOREIGN KEY ([Office]) REFERENCES [dbo].[CU_Address](Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
    FOREIGN KEY ([ReportTo]) REFERENCES [dbo].[CU_Contact](Id),
	CONSTRAINT FK_CU_Contact_PrimaryEntity FOREIGN KEY(PrimaryEntity) REFERENCES AP_Entity(Id)
)