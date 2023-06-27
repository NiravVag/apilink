CREATE TABLE [dbo].[ES_SU_Template_Details]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	 Field_Id int,
	 Template_Id int,
	 Sort int,
	 Max_Char int,
	 IsTitle bit,
	 TitleCustomName nvarchar(400),
	 Max_Items int,
	 DateFormat int,
	 IsDateSeperator bit,
	 CreatedBy int,
	CreatedOn datetime default getdate(),
	 CONSTRAINT ES_SU_Template_Details_Field_Id FOREIGN KEY (Field_Id) REFERENCES [dbo].[ES_SU_PreDefined_Fields](Id),
	 CONSTRAINT ES_SU_Template_Details_Template_Id FOREIGN KEY (Template_Id) REFERENCES [dbo].[ES_SU_Template_Master](Id),
	 CONSTRAINT ES_SU_Template_Details_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	 CONSTRAINT FK_Date_Format FOREIGN KEY(DateFormat) REFERENCES [dbo].[REF_DateFormat](Id)
)
