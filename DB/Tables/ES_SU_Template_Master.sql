CREATE TABLE [dbo].[ES_SU_Template_Master]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	Template_Name nvarchar(max),
	Template_Display_Name nvarchar(max),
	Customer_Id int null,
	[Email_Type_Id] INT NULL,
	[Module_Id] INT NULL,
	[Delimiter_Id] INT NULL,
	Active bit,
	CreatedBy int,
	CreatedOn datetime default getdate(),
	DeletedBy int,
	DeletedOn datetime,
	UpdatedOn datetime,
	UpdatedBy int
	CONSTRAINT ES_SU_Template_Master_DeletedBy FOREIGN KEY (DeletedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT ES_SU_Template_Master_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT ES_SU_Template_Master_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT ES_SU_Template_Master_Customer_Id FOREIGN KEY (Customer_Id) REFERENCES [dbo].[cu_customer](Id),
	 CONSTRAINT FK_ES_SU_Template_Master_Module_Id FOREIGN KEY([Module_Id]) REFERENCES [dbo].[ES_SU_Module](Id),
	 CONSTRAINT FK_ES_SU_Template_Master_Email_Type_Id FOREIGN KEY([Email_Type_Id]) REFERENCES [dbo].[ES_Type](Id),
	 CONSTRAINT FK_ES_SU_Template_Master_Delimiter_Id FOREIGN KEY([Delimiter_Id]) REFERENCES [dbo].[Ref_Delimiter](Id)
)
