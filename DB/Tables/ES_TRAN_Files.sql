CREATE TABLE [dbo].[ES_TRAN_Files]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1),
  [Inspection_Id] int null,
  [Audit_Id] int null,
  [Report_Id] int null,
  [File_Type_Id] int null,
 [File_Link] nvarchar(3000),
 [File_Name] nvarchar(1000), 	
 [Unique_Id] nvarchar(1000),
 [Active] bit,
 [Entity_Id] int null,
[Created_On] DATETIME NOT NULL default getdate(),  
 [Created_By] int NULL,
[Deleted_On] DATETIME NOT NULL default getdate(),  
[Deleted_By] int NULL
  CONSTRAINT FK_ES_TRAN_Files_Inspection_Id FOREIGN KEY ([Inspection_Id]) REFERENCES [dbo].[Insp_transaction](Id),
  CONSTRAINT FK_ES_TRAN_Files_Auidt_Id FOREIGN KEY ([Audit_Id]) REFERENCES [dbo].[AUD_Transaction](Id),
  CONSTRAINT FK_ES_TRAN_Files_Report_Id FOREIGN KEY ([Report_Id]) REFERENCES [dbo].[FB_Report_Details](Id),
  CONSTRAINT FK_ES_TRAN_Files_File_Type_Id FOREIGN KEY ([File_Type_Id]) REFERENCES [dbo].[ES_REF_File_Type](Id),
  CONSTRAINT FK_ES_TRAN_Files_Entity_Id FOREIGN KEY ([Entity_Id]) REFERENCES [dbo].[AP_Entity](Id),
  CONSTRAINT FK_ES_TRAN_Files_Created_By FOREIGN KEY ([Created_By]) REFERENCES [dbo].[IT_UserMaster](Id),
  CONSTRAINT FK_ES_TRAN_Files_Deleted_By FOREIGN KEY ([Deleted_By]) REFERENCES [dbo].[IT_UserMaster](Id)
)