CREATE TABLE [dbo].[INSP_LAB_Address]
(
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL PRIMARY KEY,
	[CountryId] [int] NOT NULL,
	[ProvinceId] [int] NOT NULL,
	[CityId] [int] NOT NULL,
	[ZipCode] [nvarchar](20) NULL,
	[Address] [nvarchar](200) NULL,
	[RegionalLanguage] [nvarchar](20) NULL,
	[Lab_Id] [int] NULL,
	[AddressTypeId] [int] NULL,
	FOREIGN KEY([CountryId]) REFERENCES [dbo].[REF_Country](Id),
	FOREIGN KEY([ProvinceId]) REFERENCES [dbo].[REF_Province](Id),
	FOREIGN KEY([CityId]) REFERENCES [dbo].[REF_City](Id),
	FOREIGN KEY([Lab_Id]) REFERENCES [dbo].[INSP_LAB_Details](Id),
	FOREIGN KEY([AddressTypeId]) REFERENCES [dbo].[INSP_LAB_AddressType](Id)

)
