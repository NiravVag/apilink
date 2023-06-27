CREATE TABLE [dbo].[REF_County]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [City_Id] INT NOT NULL,
	[County_Name] nvarchar(500) NOT NULL,
	[County_Code] nvarchar(500) NULL,
	[Active] bit NOT NULL,
	[Created_By] int NULL,
	[Created_On] datetime,
	[Modified_By] int NULL,
	[Modified_On] datetime,
	[Deleted_By] int NULL,
	[Deleted_On] datetime,
	[ZoneId] INT NULL, 
	FOREIGN KEY (City_Id) REFERENCES[dbo].[REF_CITY](Id),
    FOREIGN KEY (Created_By) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY (Modified_By) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY (Deleted_By) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT REF_County_ZoneId  FOREIGN KEY(ZoneId) REFERENCES [REF_Zone](Id)
)
