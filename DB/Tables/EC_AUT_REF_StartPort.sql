CREATE TABLE [dbo].[EC_AUT_REF_StartPort]
(
	[Id] int not null primary key identity(1,1),
	[StartPortName] nvarchar(1000),
	[Active] bit, 	
	[Sort] bit, 	
	[Entity_Id] INT , 
	[CreatedOn] datetime,
	[UpdatedOn] datetime,
	[DeletedOn] datetime,
	[UpdatedBy] int null,
	[DeletedBy] int null,
	[CreatedBy] int null,
	CONSTRAINT FK_EC_AUT_REF_StartPort_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
	CONSTRAINT FK_EC_AUT_REF_StartPort_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_EC_AUT_REF_StartPort_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
    CONSTRAINT FK_EC_AUT_REF_StartPort_Entity_Id FOREIGN KEY([Entity_Id]) REFERENCES [dbo].[AP_Entity](Id)
)
