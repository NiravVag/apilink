CREATE TABLE [dbo].[ES_SU_PreDefined_Fields]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	[Field_Name] nvarchar(max),  
	[Field_Alias_Name] nvarchar(max),
	[Max_Char] int,
	[IsText] bit,
	[DataType] int NULL,
	Active bit,
	CreatedBy int,
	CreatedOn datetime default getdate(),
	DeletedBy int,
	DeletedOn datetime,
	UpdatedOn datetime,
	UpdatedBy int
	CONSTRAINT ES_SU_PreDefined_Fields_DeletedBy FOREIGN KEY (DeletedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT ES_SU_PreDefined_Fields_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT ES_SU_PreDefined_Fields_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_SU_DataType FOREIGN KEY (DataType) REFERENCES [dbo].[ES_SU_DataType](Id)
)
