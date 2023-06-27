CREATE TABLE [dbo].[INSP_Container_Transaction]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	

    [Inspection_Id] INT NOT NULL, 
	[Container_Id] INT NOT NULL,    
	[TotalBookingQuantity] INT NOT NULL,	
	[Remarks] varchar(max) null,
	
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
	[UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 	

    [Active] BIT NULL,

	[Fb_Report_Id] INT NULL, 


    [ContainerSize] INT NULL, 
	[EntityId] INT NULL,

    FOREIGN KEY([Inspection_Id]) REFERENCES [dbo].[INSP_Transaction](Id),	
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([Fb_Report_Id]) REFERENCES [FB_Report_Details](Id),
	FOREIGN KEY(ContainerSize) REFERENCES [REF_Container_Size](Id),
	CONSTRAINT FK_INSP_Container_Transaction_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)
