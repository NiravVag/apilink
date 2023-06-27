
CREATE TABLE [dbo].[SU_Address](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
	[CountryId] INT NOT NULL,
	[RegionId] INT NOT NULL,
	[CityId] INT NOT NULL,
	[ZipCode] NVARCHAR(20) NULL,
	[Address] NVARCHAR(200) NOT NULL,
	[LocalLanguage] NVARCHAR(20) NULL,
	Supplier_Id INT NOT NULL,
	Longitude DECIMAL(12,9) NULL,
	Latitude DECIMAL(12,9) NULL,
	[AddressTypeId] INT NULL,
	geoPoint as geography::Point(ISNULL(Latitude,0), ISNULL(Longitude,0), 4326),
	[CountyId] INT NULL, 
    [TownId] INT NULL, 
    FOREIGN KEY(Supplier_Id) REFERENCES [dbo].[SU_Supplier](Id),
	FOREIGN KEY([CountryId]) REFERENCES [dbo].[REF_Country](Id),
	FOREIGN KEY([RegionId]) REFERENCES [dbo].[REF_Province](Id),
	FOREIGN KEY([CityId]) REFERENCES [dbo].[REF_City](Id),
	FOREIGN KEY([AddressTypeId]) REFERENCES [dbo].[SU_AddressType](Id),
	FOREIGN KEY([CountyId]) REFERENCES [dbo].[REF_County](Id),
	FOREIGN KEY([TownId]) REFERENCES [dbo].[REF_Town](Id)
	)