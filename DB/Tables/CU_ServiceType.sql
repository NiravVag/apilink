﻿CREATE TABLE [dbo].[CU_ServiceType]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [CustomerId] INT NOT NULL, 
    [ServiceId] INT NOT NULL, 
    [ServiceTypeId] INT NOT NULL, 
    [Active] BIT NOT NULL,
	[PickType] INT NULL, 
    [LevelPick1] INT NULL, 
    [LevelPick2] INT NULL, 
    [CriticalPick1] INT NULL, 
    [CriticalPick2] INT NULL, 
    [MajorTolerancePick1] INT NULL, 
    [MajorTolerancePick2] INT NULL, 
    [MinorTolerancePick1] INT NULL, 
    [MinorTolerancePick2] INT NULL, 
    [AllowAQLModification] BIT NULL, 
    [DefectClassification] INT NULL, 
    [CheckMeasurementPoints] BIT NULL, 
    [ReportUnit] INT NULL, 
    [ProductCategoryId] INT NULL, 
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
    [CustomServiceTypeName] NVARCHAR(3000) NULL, 
	[EntityId] INT NULL,
    [IgnoreAcceptanceLevel] BIT,
	CONSTRAINT CU_ServiceType_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id),
    FOREIGN KEY([CustomerId]) REFERENCES CU_Customer(Id),
	FOREIGN KEY([ServiceId]) REFERENCES REF_Service(Id),
	FOREIGN KEY([ServiceTypeId]) REFERENCES REF_ServiceType(Id),
	FOREIGN KEY([PickType]) REFERENCES REF_PickType(Id),
	FOREIGN KEY([LevelPick1]) REFERENCES REF_LevelPick1(Id),
	FOREIGN KEY([LevelPick2]) REFERENCES REF_LevelPick2(Id),
	FOREIGN KEY([CriticalPick1]) REFERENCES REF_Pick1(Id),
	FOREIGN KEY([CriticalPick2]) REFERENCES REF_Pick2(Id),
	FOREIGN KEY([MajorTolerancePick1]) REFERENCES REF_Pick1(Id),
	FOREIGN KEY([MajorTolerancePick2]) REFERENCES REF_Pick2(Id),
	FOREIGN KEY([MinorTolerancePick1]) REFERENCES REF_Pick1(Id),
	FOREIGN KEY([MinorTolerancePick2]) REFERENCES REF_Pick2(Id),
	FOREIGN KEY([DefectClassification]) REFERENCES REF_DefectClassification(Id),
	FOREIGN KEY([ReportUnit]) REFERENCES REF_ReportUnit(Id),
	FOREIGN KEY([ProductCategoryId]) REFERENCES REF_ProductCategory(Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)