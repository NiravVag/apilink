

/*
------------------------------------------------------------------------------------
   [AP_Entity]		
------------------------------------------------------------------------------------
*/	

INSERT INTO AP_Entity(Name, Active)
VALUES('API',1)
GO 
/*
------------------------------------------------------------------------------------
   [REF_Zone]		
------------------------------------------------------------------------------------
*/	
INSERT INTO REF_Zone(Id, Name,Active)
VALUES(1,'Zone',1)


:r .\REFTranslation.Post.sql
:r .\RefArea.Post.sql	
:r .\RefCountry.Post.sql
:r .\RefProvince.Post.sql
:r .\RefCity.Post.sql
:r .\HrHoliday.Post.sql

/*
--------------------------------------------------------------------------------------
    [REF_LocationType]			
--------------------------------------------------------------------------------------
*/

SET IDENTITY_INSERT [dbo].[REF_LocationType] ON 
GO
INSERT [dbo].[REF_LocationType] ([Id], [SGT_Location_Type], [Active],[EntityId]) VALUES (1, N'Head Office', 1,1)
INSERT [dbo].[REF_LocationType] ([Id], [SGT_Location_Type], [Active],[EntityId]) VALUES (2, N'Regional Head Office', 1,1)
INSERT [dbo].[REF_LocationType] ([Id], [SGT_Location_Type], [Active],[EntityId]) VALUES (3, N'Branch Office', 1,1)
INSERT [dbo].[REF_LocationType] ([Id], [SGT_Location_Type], [Active],[EntityId]) VALUES (4, N'Warehouse', 1,1)
INSERT [dbo].[REF_LocationType] ([Id], [SGT_Location_Type], [Active],[EntityId]) VALUES (5, N'Other', 1,1)
GO
SET IDENTITY_INSERT [dbo].[REF_LocationType] OFF
GO


/*
--------------------------------------------------------------------------------------
    [REF_Currency]		
--------------------------------------------------------------------------------------
*/

SET IDENTITY_INSERT [dbo].[REF_Currency] ON 
GO
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (14, N'BDT', 50, 2, N'Taka', N'Updated on 01/Jan/2006', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (16, N'BGN', 975, 2, N'Bulgarian Lev', N'', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (31, N'CHF', 1, 2, N'Swiss Franc', N'', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (34, N'CNY', 156, 2, N'Yuan Renminbi', N'Updated On 01/Dec/2005', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (46, N'ECV', 983, 2, N'Unidad de Valor Constante (UVC)', N'update 04/08/2004', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (51, N'EUR', 978, 2, N'Euro', N'Updated On 01/Jan/2006', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (63, N'HKD', 344, 2, N'Hong Kong Dollar', N'DO NOT CHANGE - CS', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (68, N'IDR', 360, 2, N'Rupiah', N'update 04/08/2004,Updated on 01/Jan/2006', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (70, N'INR', 356, 2, N'Indian Rupee', N'Updated On 01/Jan/2006', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (76, N'JPY', 392, 0, N'Yen', N'update 01/Dec/2005', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (81, N'KPW', 408, 2, N'North Korean Won', N'Updated On 01/Jan/2006', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (94, N'MAD', 504, 2, N'Moroccan Dirham', N'update 04/08/2004', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (100, N'MOP', 446, 2, N'Pataca', N'', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (108, N'MYR', 458, 2, N'Malaysian Ringgit', N'update 04/08/2004,Updated On 01/Jan/2006', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (120, N'PHP', 608, 2, N'Philippine Peso', N'', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (121, N'PKR', 586, 2, N'Pakistan Rupee', N'update 04/08/2004', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (134, N'SGD', 702, 2, N'Singapore Dollar', N'Updated on 01/Jan/2006', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (145, N'THB', 764, 2, N'Baht', N'Updated On 01/Jan/2006', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (148, N'TND', 788, 3, N'Tunisian Dinar', N'update 15/11/2005 by Kamel', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (150, N'TRL', 792, 0, N'Turkish Lira', N'update 04/08/2004', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (152, N'TWD', 901, 2, N'New Taiwan Dollar', N'Updated On 01/Jan/2006', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (156, N'USD', 840, 2, N'US Dollar', N'Updated on 01/Dec/2005', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (174, N'RMB', 923, NULL, N'Yuen Remimbi', N'', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (175, N'HRK', 123, NULL, N'Kuna', N'', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (176, N'VDN', NULL, NULL, N'Vietnam Don', N'', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (177, N'KRW', 999, 2, N'South Korean Won', N'created on Nov 8,2010', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (178, N'RON', 946, NULL, N'Romanian leu', N'created on Nov 15,2010', 1)
INSERT [dbo].[REF_Currency] ([Id], [CurrencyCodeA], [CurrencyCodeN], [MinorUnit], [CurrencyName], [Comment], [Active]) VALUES (179, N'POL', NULL, NULL, N'Polish Zloty', N'Created on March 04,2012 - Liven', 1)
GO
SET IDENTITY_INSERT [dbo].[REF_Currency] OFF
GO

/*²
--------------------------------------------------------------------------------------
    [REF_Location]			
--------------------------------------------------------------------------------------
*/
INSERT INTO [dbo].[REF_Location]([Address], [City_Id],[ZipCode], [LocationType_Id], [Location_Name], [Master_Currency_Id], [Default_Currency_Id], [Tel], Fax,[Comment],  [Active], [Email], [Address2],[OfficeCode],EntityId)
VALUES ('Units 506-8, 5/F, Laford Centre, No. 838 Lai Chi Kok Road, Kowloon',2, '838', 1, 'API Home', 63, 34, '+852 3719 8613', '', 'Check data to test', 1, 'mosaab.jebarat@sgtgroup.net','Units 506-8, 5/F, Laford Centre, No. 838 Lai Chi Kok Road, Kowloon','APIHOME',1)

INSERT INTO REF_Country_Location(CountryId, LocationId)
values(38, 1)

/*
------------------------------------------------------------------------------------
   REF_MarketSegment		
------------------------------------------------------------------------------------
*/	

INSERT INTO REF_MarketSegment(Name, Active,EntityId) 
VALUES ('Home',1,1),
	   ('Furniture',1,1),
	   ('Electrical',1,1),
	   ('Toys',1,1),
	   ('Festive',1,1),
	   ('DIY',1,1)
GO

/*
------------------------------------------------------------------------------------
   [REF_ProductCategory]		
------------------------------------------------------------------------------------
*/	
SET IDENTITY_INSERT [REF_ProductCategory] ON 
GO
INSERT [dbo].[REF_ProductCategory] ([Id], [Name], [Active], [EntityId]) VALUES (1, N'Electronic & Electrical', 1, 1)
GO
INSERT [dbo].[REF_ProductCategory] ([Id], [Name], [Active], [EntityId]) VALUES (2, N'Furniture', 1, 1)
GO
INSERT [dbo].[REF_ProductCategory] ([Id], [Name], [Active], [EntityId]) VALUES (3, N'Toys', 1, 1)
GO
INSERT [dbo].[REF_ProductCategory] ([Id], [Name], [Active], [EntityId]) VALUES (4, N'Baby Care', 1, 1)
GO
INSERT [dbo].[REF_ProductCategory] ([Id], [Name], [Active], [EntityId]) VALUES (5, N'Tools', 1, 1)
GO
INSERT [dbo].[REF_ProductCategory] ([Id], [Name], [Active], [EntityId]) VALUES (6, N'Sports  Fitness and Camping ', 1, 1)
GO
INSERT [dbo].[REF_ProductCategory] ([Id], [Name], [Active], [EntityId]) VALUES (7, N'Personal Care', 1, 1)
GO
INSERT [dbo].[REF_ProductCategory] ([Id], [Name], [Active], [EntityId]) VALUES (8, N'Home Products', 1, 1)
GO
INSERT [dbo].[REF_ProductCategory] ([Id], [Name], [Active], [EntityId]) VALUES (9, N'Stationery', 1, 1)
GO
INSERT [dbo].[REF_ProductCategory] ([Id], [Name], [Active], [EntityId]) VALUES (10, N'Luggage & Bags', 1, 1)
GO
INSERT [dbo].[REF_ProductCategory] ([Id], [Name], [Active], [EntityId]) VALUES (11, N'Car Accessories', 1, 1)
GO
INSERT [dbo].[REF_ProductCategory] ([Id], [Name], [Active], [EntityId]) VALUES (12, N'NFR', 1, 1)
GO
INSERT [dbo].[REF_ProductCategory] ([Id], [Name], [Active], [EntityId]) VALUES (13, N'Textile and Footware', 1, 1)
GO
SET IDENTITY_INSERT [dbo].[REF_ProductCategory] OFF
GO

/*
------------------------------------------------------------------------------------
   [REF_Expertise]		
------------------------------------------------------------------------------------
*/	

INSERT INTO [dbo].[REF_Expertise](Name,Active,EntityId) 
VALUES	('Mass Market',1,1),
		('Toys',1,1),
		('Infant Toys',1,1),
		('Electrical',1,1),
		('Wood Items',1,1),
		('Festive decorations',1,1),
		('DIY',1,1),
		('Cookware',1,1),
		('Kitchen Ware',1,1),
		('Outdoor furniture',1,1),
		('Indoor furniture',1,1)
GO

/*
------------------------------------------------------------------------------------
   [Customer Page Master Data]		
------------------------------------------------------------------------------------
*/	


insert into [dbo].[language] values ('EN')
insert into  [dbo].[language] values ('FR')
insert into  [dbo].[language] values ('CN')
--insert into  [dbo].[language] values ('TUR')
--insert into  [dbo].[language] values ('IND')

insert into [dbo].[REF_ProspectStatus] values ('Inprogress')
insert into [dbo].[REF_ProspectStatus] values ('Completed')
insert into [dbo].[REF_ProspectStatus] values ('Pending')



/*update [dbo].[CU_Customer] set [Group] =1*/	

insert into [dbo].[REF_BusinessType](Name,EntityId) values ('Sprts wear',1)
insert into [dbo].[REF_BusinessType](Name,EntityId) values ('Home textile',1)
insert into [dbo].[REF_BusinessType](Name,EntityId) values ('Children Wear',1)
insert into [dbo].[REF_BusinessType](Name,EntityId) values ('Others',1)
insert into [dbo].[REF_BusinessType](Name,EntityId) values ('Fabrics',1)

insert into [dbo].[REF_InvoiceType](Name,EntityId) values ('Monthly Invoice',1)
insert into [dbo].[REF_InvoiceType](Name,EntityId) values ('Pre Invoice',1)


insert into [dbo].[REF_AddressType](Name,EntityId) values ('Head Office',1)
insert into [dbo].[REF_AddressType](Name,EntityId) values ('Accounting',1)
insert into [dbo].[REF_AddressType](Name,EntityId) values ('Warehouse',1)
insert into [dbo].[REF_AddressType](Name,EntityId) values ('Regional Office',1)



/*
------------------------------------------------------------------------------------
   [HR_Profile]		
------------------------------------------------------------------------------------
*/	
INSERT INTO HR_Profile(ProfileName, Active,EntityId)
VALUES  ('Auditor',1,NULL),
		('Cutomer Service',1,NULL),
		('Planner',1,1),
		('Inspector',1,1),
		('Finance',1,1),
		('Management',1,1),
		('IT',1,1),
		('HR',1,1),
		('KAM',1,1),
		('Quotation', 1,1),
		('Report Checker', 1,1),
		('Scheduler',1,1),
		('Other',1,1) 
GO

/*
------------------------------------------------------------------------------------
   [HR_EMployeeType]		
------------------------------------------------------------------------------------
*/	
SET IDENTITY_INSERT [dbo].[HR_EMployeeType] ON 
GO

INSERT INTO [HR_EMployeeType]([Id],[EmployeeTypeName],EntityId)
VALUES(1,'Permanent',1),(2,'Outsource',1),(3,'Other',1)

SET IDENTITY_INSERT [dbo].[HR_EMployeeType] OFF
GO
/*
------------------------------------------------------------------------------------
   HR_Department		
------------------------------------------------------------------------------------
*/	
SET IDENTITY_INSERT [dbo].[HR_Department] ON 
GO
INSERT [dbo].[HR_Department] ([Id], [Department_Name], [Active], [Department_Code],EntityId) VALUES (1, N'HR', 1, N'HRA',1)
INSERT [dbo].[HR_Department] ([Id], [Department_Name], [Active], [Department_Code],EntityId) VALUES (2, N'Finance', 1, N'FA',1)
INSERT [dbo].[HR_Department] ([Id], [Department_Name], [Active], [Department_Code],EntityId) VALUES (3, N'IT', 1, N'IT',1)
INSERT [dbo].[HR_Department] ([Id], [Department_Name], [Active], [Department_Code],EntityId) VALUES (4, N'Quality & Internal Audit', 1, N'QIA',1)
INSERT [dbo].[HR_Department] ([Id], [Department_Name], [Active], [Department_Code],EntityId) VALUES (5, N'Sales', 1, N'Sales',1)
INSERT [dbo].[HR_Department] ([Id], [Department_Name], [Active], [Department_Code],EntityId) VALUES (6, N'Customer Care', 1, N'CC',1)
INSERT [dbo].[HR_Department] ([Id], [Department_Name], [Active], [Department_Code],EntityId) VALUES (7, N'Inspection', 1, N'Inspection',1)
INSERT [dbo].[HR_Department] ([Id], [Department_Name], [Active], [Department_Code],EntityId) VALUES (8, N'Audit', 1, N'Audit',1)
INSERT [dbo].[HR_Department] ([Id], [Department_Name], [Active], [Department_Code],EntityId) VALUES (9, N'Management', 1, N'Management',1)
GO
SET IDENTITY_INSERT [dbo].[HR_Department] OFF
GO

/*
------------------------------------------------------------------------------------
   [HR_Qualification]		
------------------------------------------------------------------------------------
*/	

SET IDENTITY_INSERT [dbo].[HR_Qualification] ON 
GO
INSERT [dbo].[HR_Qualification] ([Id], [Qualification_Name], [Active],EntityId) VALUES (1, N'School', 1,1)
INSERT [dbo].[HR_Qualification] ([Id], [Qualification_Name], [Active],EntityId) VALUES (2, N'Diploma', 1,1)
INSERT [dbo].[HR_Qualification] ([Id], [Qualification_Name], [Active],EntityId) VALUES (3, N'Graduate', 1,1)
INSERT [dbo].[HR_Qualification] ([Id], [Qualification_Name], [Active],EntityId) VALUES (4, N'Post Graduate', 1,1)
INSERT [dbo].[HR_Qualification] ([Id], [Qualification_Name], [Active],EntityId) VALUES (5, N'Doctorate (PhD)', 1,1)
GO
SET IDENTITY_INSERT [dbo].[HR_Qualification] OFF
GO


/*
------------------------------------------------------------------------------------
   [HR_Position]		
------------------------------------------------------------------------------------
*/	

SET IDENTITY_INSERT [dbo].[HR_Position] ON 
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (1, N'HR Manager', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (2, N'HR Specialist', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (3, N'HR & Administration Assistant & Receptionist', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (4, N'Cleaner', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (5, N'Administative Supporter', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (6, N'Chief Financial Officer', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (7, N'Receivable Clerk', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (8, N'Accounting Clerk', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (9, N'Accounting Supervisor', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (10, N'Chief Accountant', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (11, N'IT Director', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (12, N'IT Supervisor', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (13, N'IT Officer', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (14, N'IT Technician', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (15, N'Programmer', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (16, N'Senior Software Analyst', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (17, N'Senior Software Architect', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (18, N'Network & Systems Adminstrator', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (19, N'Quality Manager', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (20, N'Internal Audit Manager', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (21, N'Internal Auditor', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (22, N'Sales & Business Development Manager', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (23, N'Keyp Account & Marketing Manager', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (24, N'Technical Key Account Manager', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (25, N'Key Account Manager', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (26, N'Customer Service Manager', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (27, N'Supervisor', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (28, N'Leader', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (29, N'Customer Service Representative', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (30, N'Electrical Engineer', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (31, N'Scheduler', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (32, N'Report Checker', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (33, N'Implant', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (34, N'Senior Vice President of North China', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (35, N'Vice President of Sourth China', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (36, N'Senior Technical Advisor', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (37, N'Operation Manager', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (38, N'President Director', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (39, N'Vice Manager', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (40, N'Officer Supervisor', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (41, N'Product Engineer', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (42, N'Inspection Supervisor', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (43, N'Seniro Inspector', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (44, N'Inspector', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (45, N'TCF', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (46, N'Operation Assistant', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (47, N'Secretary/Typist', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (48, N'Administrator', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (49, N'Head of CSR and Audit', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (50, N'Head of Sustainabillity Audit', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (51, N'Technical Audit Manager', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (52, N'Audit Deputy Manager', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (53, N'Audit Executive Coordinator', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (54, N'Operation Supervisor', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (55, N'Europe Aduit Coordinator', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (56, N'Chief Executive Officer', 1, 1)
GO
INSERT [dbo].[HR_Position] ([Id], [Position_Name], [Active], [EntityId]) VALUES (57, N'Chief Operation Officer', 1, 1)
GO
SET IDENTITY_INSERT [dbo].[HR_Position] OFF
GO


/*
------------------------------------------------------------------------------------
   [HR_FileType]	
------------------------------------------------------------------------------------
*/
INSERT INTO HR_FileType(FileTypeName, Active,EntityId) 
VALUES ('Resume',1,1),
		('Job Description ( JD )',1,1),
		('Certification',1,1),
		('ID card / Passport',1,1),
		('Code of Conduct',1,1),
		('Code of Integrity',1,1),
		('Others',1,1)
GO

/*
------------------------------------------------------------------------------------
   [HR_Staff]	
------------------------------------------------------------------------------------
*/	

SET IDENTITY_INSERT [dbo].[HR_Staff] ON 
GO
DECLARE @LocationId INT 
SELECT @LocationId = Id FROM dbo.[REF_LOCATION]

INSERT [dbo].[HR_Staff] ([Id], [Person_Name], [Gender], [Marital_Status], [Birth_Date], [Join_Date], [salary_Currency_Id], [Parent_Staff_Id], [Department_Id], [Qualification_Id], [Location_Id], [Active], [Paid_Holiday_Days_Per_Year], [Comments], [Nationality_Country_Id], [Name_Chinese], [Working_Days_Of_Week], [Passport_no], [Probation_Period], [place_of_Birth], [Bank_Name], [Bank_Account_No], [Salary], [Emp_no], [Emergency_Call], [Leave_Date], [GL_Code], [Probation_Expired_Date], [Labor_Contract_Expired_Date], [Created_At], [Created_By], [Modified_At], [Modified_By], [Prefer_Currency_Id], [OutSource], [Position_Id], [Start_Port], [GraduateSchool], [EmergencyContactName], [GraduateDate], [EmergencyContactPhone], [EmaiLAddress], [CompanyMobileNo], [SocialInsuranceCardNo], [HousingFuncard], [PlacePurchasingSIHF], [LaborContractPeriod],  [Current_ZipCode], [Current_Town], [StartWorkingDate], [TotalWorkingYears], [Home_CityId], [Home_Address], [Current_CityId], [Current_Address], [PayrollCurrencyId], [EmployeeTypeId],EntityId) VALUES (1, N'Liven Varghese', N'M', N'M', CAST(N'1977-04-19T00:00:00.000' AS DateTime), CAST(N'2004-07-22T00:00:00.000' AS DateTime), 70, NULL, 5, 4, @locationId, 1, 21, N'', 38, NULL, 5, NULL, 6, N'Pariyaram', NULL, NULL, 0, N'SgT-ID IT 001', N'914802746624', NULL, N'61121', NULL, NULL, NULL, NULL, CAST(N'2017-11-02T09:54:41.307' AS DateTime), CAST(175 AS Numeric(18, 0)), NULL, 0, 35, 3, NULL, NULL, NULL, NULL, N'liven.varghese@gmail.com', NULL, NULL, NULL, NULL, NULL,  NULL, NULL, NULL, NULL, NULL, N'Pariyaram P.O', NULL, N'20 H , Tower 5 , Sherwood Court , Tin shui Wau', 63, 1,1)
INSERT [dbo].[HR_Staff] ([Id], [Person_Name], [Gender], [Marital_Status], [Birth_Date], [Join_Date], [salary_Currency_Id], [Parent_Staff_Id], [Department_Id], [Qualification_Id], [Location_Id], [Active], [Paid_Holiday_Days_Per_Year], [Comments], [Nationality_Country_Id], [Name_Chinese], [Working_Days_Of_Week], [Passport_no], [Probation_Period], [place_of_Birth], [Bank_Name], [Bank_Account_No], [Salary], [Emp_no], [Emergency_Call], [Leave_Date], [GL_Code], [Probation_Expired_Date], [Labor_Contract_Expired_Date], [Created_At], [Created_By], [Modified_At], [Modified_By], [Prefer_Currency_Id], [OutSource], [Position_Id], [Start_Port], [GraduateSchool], [EmergencyContactName], [GraduateDate], [EmergencyContactPhone], [EmaiLAddress], [CompanyMobileNo], [SocialInsuranceCardNo], [HousingFuncard], [PlacePurchasingSIHF], [LaborContractPeriod],  [Current_ZipCode], [Current_Town], [StartWorkingDate], [TotalWorkingYears], [Home_CityId], [Home_Address], [Current_CityId], [Current_Address], [PayrollCurrencyId], [EmployeeTypeId],EntityId) VALUES (2, N'Mosaab JEBARAT', N'M', N'M', CAST(N'2009-01-01T00:00:00.000' AS DateTime), CAST(N'2009-01-26T00:00:00.000' AS DateTime), 148, 1, 5, 3, @locationId, 1, 0, N'', 38, NULL, 5.5, NULL, 0, N'', NULL, NULL, 0, N'SGT-IT-TUNIS-01', N'', NULL, N'50121', NULL, NULL, NULL, NULL, CAST(N'2017-10-30T18:33:40.290' AS DateTime), CAST(706 AS Numeric(18, 0)), NULL, 1, 11, 1001, NULL, NULL, NULL, NULL, N'mosaab.jebarat@sgtgroup.net', NULL, NULL, NULL, NULL, NULL,  NULL, NULL, NULL, NULL, NULL, N'Nourr jaafer Ariana', NULL, N'Avenue de la division leclerc  Antony', 148, 2,1)
INSERT [dbo].[HR_Staff] ([Id], [Person_Name], [Gender], [Marital_Status], [Birth_Date], [Join_Date], [salary_Currency_Id], [Parent_Staff_Id], [Department_Id], [Qualification_Id], [Location_Id], [Active], [Paid_Holiday_Days_Per_Year], [Comments], [Nationality_Country_Id], [Name_Chinese], [Working_Days_Of_Week], [Passport_no], [Probation_Period], [place_of_Birth], [Bank_Name], [Bank_Account_No], [Salary], [Emp_no], [Emergency_Call], [Leave_Date], [GL_Code], [Probation_Expired_Date], [Labor_Contract_Expired_Date], [Created_At], [Created_By], [Modified_At], [Modified_By], [Prefer_Currency_Id], [OutSource], [Position_Id], [Start_Port], [GraduateSchool], [EmergencyContactName], [GraduateDate], [EmergencyContactPhone], [EmaiLAddress], [CompanyMobileNo], [SocialInsuranceCardNo], [HousingFuncard], [PlacePurchasingSIHF], [LaborContractPeriod],  [Current_ZipCode], [Current_Town], [StartWorkingDate], [TotalWorkingYears], [Home_CityId], [Home_Address], [Current_CityId], [Current_Address], [PayrollCurrencyId], [EmployeeTypeId],EntityId) VALUES (3, N'Nixon Antony', N'M', N'U', CAST(N'1988-05-08T00:00:00.000' AS DateTime), CAST(N'2015-06-01T00:00:00.000' AS DateTime), 34, 1, 5, 3, 1, @locationId, 14, N'', 38, N'nixon',  0, N'J8560264', 0, N'India', N'农业银行', N'''6228480128295959679', 0, NULL, N'A.V Antony91-484-2695578', NULL, N'50121', NULL, NULL, CAST(N'2015-06-01T09:53:14.753' AS DateTime), CAST(1121 AS Numeric(18, 0)), CAST(N'2017-11-07T13:08:59.297' AS DateTime), CAST(1326 AS Numeric(18, 0)), NULL, 0, 11, 3, N'Bharata Mata College', N'Alex Antony', CAST(N'2008-06-01T00:00:00.000' AS DateTime), N'+91 484 2695578', N'nixon.antony@sgtgroup.net', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, N'Ambadan House, Anappara P.O, Anappara, Ernakulam, Kerala,683581', 1, N'20E-3，20th Floor,Longyunge,Guohuang building,Caitian Road,Futian district,Shenzhen', 34, 1,1)
INSERT [dbo].[HR_Staff] ([Id], [Person_Name], [Gender], [Marital_Status], [Birth_Date], [Join_Date], [salary_Currency_Id], [Parent_Staff_Id], [Department_Id], [Qualification_Id], [Location_Id], [Active], [Paid_Holiday_Days_Per_Year], [Comments], [Nationality_Country_Id], [Name_Chinese], [Working_Days_Of_Week], [Passport_no], [Probation_Period], [place_of_Birth], [Bank_Name], [Bank_Account_No], [Salary], [Emp_no], [Emergency_Call], [Leave_Date], [GL_Code], [Probation_Expired_Date], [Labor_Contract_Expired_Date], [Created_At], [Created_By], [Modified_At], [Modified_By], [Prefer_Currency_Id], [OutSource], [Position_Id], [Start_Port], [GraduateSchool], [EmergencyContactName], [GraduateDate], [EmergencyContactPhone], [EmaiLAddress], [CompanyMobileNo], [SocialInsuranceCardNo], [HousingFuncard], [PlacePurchasingSIHF], [LaborContractPeriod],  [Current_ZipCode], [Current_Town], [StartWorkingDate], [TotalWorkingYears], [Home_CityId], [Home_Address], [Current_CityId], [Current_Address], [PayrollCurrencyId], [EmployeeTypeId],EntityId) VALUES (4, N'Test Inspector', N'M', N'M', CAST(N'2009-01-01T00:00:00.000' AS DateTime), CAST(N'2009-01-26T00:00:00.000' AS DateTime), 148, 1, 5, 3, @locationId, 1, 0, N'', 38, NULL, 5.5, NULL, 0, N'', NULL, NULL, 0, N'SGT-IT-TUNIS-01', N'', NULL, N'50121', NULL, NULL, NULL, NULL, CAST(N'2017-10-30T18:33:40.290' AS DateTime), CAST(706 AS Numeric(18, 0)), NULL, 1, 11, 1001, NULL, NULL, NULL, NULL, N'mosaab.jebarat@sgtgroup.net', NULL, NULL, NULL, NULL, NULL,  NULL, NULL, NULL, NULL, NULL, N'Nourr jaafer Ariana', NULL, N'Avenue de la division leclerc  Antony', 148, 2,1)
INSERT [dbo].[HR_Staff] ([Id], [Person_Name], [Gender], [Marital_Status], [Birth_Date], [Join_Date], [salary_Currency_Id], [Parent_Staff_Id], [Department_Id], [Qualification_Id], [Location_Id], [Active], [Paid_Holiday_Days_Per_Year], [Comments], [Nationality_Country_Id], [Name_Chinese], [Working_Days_Of_Week], [Passport_no], [Probation_Period], [place_of_Birth], [Bank_Name], [Bank_Account_No], [Salary], [Emp_no], [Emergency_Call], [Leave_Date], [GL_Code], [Probation_Expired_Date], [Labor_Contract_Expired_Date], [Created_At], [Created_By], [Modified_At], [Modified_By], [Prefer_Currency_Id], [OutSource], [Position_Id], [Start_Port], [GraduateSchool], [EmergencyContactName], [GraduateDate], [EmergencyContactPhone], [EmaiLAddress], [CompanyMobileNo], [SocialInsuranceCardNo], [HousingFuncard], [PlacePurchasingSIHF], [LaborContractPeriod], [Current_ZipCode], [Current_Town], [StartWorkingDate], [TotalWorkingYears], [Home_CityId], [Home_Address], [Current_CityId], [Current_Address], [PayrollCurrencyId], [EmployeeTypeId], [ManagerId], [SkypeId], [LocalLanguage], [CompanyEmail], [AnnualLeave], [EntityId]) VALUES (5,  N'TEST DEMO', N'M', N'M', NULL, CAST(N'2019-01-21T00:00:00.000' AS DateTime), NULL, 4, 4, 3, 1, 1, NULL, NULL, 38, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'1234', N'1234', NULL, NULL, CAST(N'2019-02-04T00:00:00.000' AS DateTime), CAST(N'2019-02-04T00:00:00.000' AS DateTime), CAST(N'2019-01-28T09:03:25.087' AS DateTime), CAST(1 AS Numeric(18, 0)), CAST(N'2019-02-11T16:40:24.570' AS DateTime), CAST(4 AS Numeric(18, 0)), NULL, NULL, 4, NULL, NULL, NULL, NULL, NULL, N'test@apilink.demo', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'sqdssds', NULL, N'sqdssds', 51, 1, NULL, NULL, NULL, N'test@apidemo.com', NULL, 1)
INSERT [dbo].[HR_Staff] ([Id], [Person_Name], [Gender], [Marital_Status], [Birth_Date], [Join_Date], [salary_Currency_Id], [Parent_Staff_Id], [Department_Id], [Qualification_Id], [Location_Id], [Active], [Paid_Holiday_Days_Per_Year], [Comments], [Nationality_Country_Id], [Name_Chinese], [Working_Days_Of_Week], [Passport_no], [Probation_Period], [place_of_Birth], [Bank_Name], [Bank_Account_No], [Salary], [Emp_no], [Emergency_Call], [Leave_Date], [GL_Code], [Probation_Expired_Date], [Labor_Contract_Expired_Date], [Created_At], [Created_By], [Modified_At], [Modified_By], [Prefer_Currency_Id], [OutSource], [Position_Id], [Start_Port], [GraduateSchool], [EmergencyContactName], [GraduateDate], [EmergencyContactPhone], [EmaiLAddress], [CompanyMobileNo], [SocialInsuranceCardNo], [HousingFuncard], [PlacePurchasingSIHF], [LaborContractPeriod], [Current_ZipCode], [Current_Town], [StartWorkingDate], [TotalWorkingYears], [Home_CityId], [Home_Address], [Current_CityId], [Current_Address], [PayrollCurrencyId], [EmployeeTypeId], [ManagerId], [SkypeId], [LocalLanguage], [CompanyEmail], [AnnualLeave], [EntityId]) VALUES (6,  N'Claim user', N'M', N'M', NULL, CAST(N'2019-01-21T00:00:00.000' AS DateTime), NULL, 4, 4, 3, 1, 1, NULL, NULL, 38, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'1234', N'1234', NULL, NULL, CAST(N'2019-02-04T00:00:00.000' AS DateTime), CAST(N'2019-02-04T00:00:00.000' AS DateTime), CAST(N'2019-01-28T09:03:25.087' AS DateTime), CAST(1 AS Numeric(18, 0)), CAST(N'2019-02-11T16:40:24.570' AS DateTime), CAST(4 AS Numeric(18, 0)), NULL, NULL, 4, NULL, NULL, NULL, NULL, NULL, N'test@apilink.demo', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'sqdssds', NULL, N'sqdssds', 51, 1, NULL, NULL, NULL, N'test@apidemo.com', NULL, 1)
INSERT [dbo].[HR_Staff] ([Id], [Person_Name], [Gender], [Marital_Status], [Birth_Date], [Join_Date], [salary_Currency_Id], [Parent_Staff_Id], [Department_Id], [Qualification_Id], [Location_Id], [Active], [Paid_Holiday_Days_Per_Year], [Comments], [Nationality_Country_Id], [Name_Chinese], [Working_Days_Of_Week], [Passport_no], [Probation_Period], [place_of_Birth], [Bank_Name], [Bank_Account_No], [Salary], [Emp_no], [Emergency_Call], [Leave_Date], [GL_Code], [Probation_Expired_Date], [Labor_Contract_Expired_Date], [Created_At], [Created_By], [Modified_At], [Modified_By], [Prefer_Currency_Id], [OutSource], [Position_Id], [Start_Port], [GraduateSchool], [EmergencyContactName], [GraduateDate], [EmergencyContactPhone], [EmaiLAddress], [CompanyMobileNo], [SocialInsuranceCardNo], [HousingFuncard], [PlacePurchasingSIHF], [LaborContractPeriod],  [Current_ZipCode], [Current_Town], [StartWorkingDate], [TotalWorkingYears], [Home_CityId], [Home_Address], [Current_CityId], [Current_Address], [PayrollCurrencyId], [EmployeeTypeId],EntityId) VALUES (7, N'Ronika Jency', N'F', N'U', CAST(N'1993-05-08T00:00:00.000' AS DateTime), CAST(N'2019-06-01T00:00:00.000' AS DateTime), 34, 1, 5, 3, 1, @locationId, 14, N'', 38, N'Ronika',  0, N'J8560264', 0, N'India', N'农业银行', N'''6228489898295959679', 0, NULL, N'2695578', NULL, N'50121', NULL, NULL, CAST(N'2019-06-01T09:53:14.753' AS DateTime), CAST(1121 AS Numeric(18, 0)), CAST(N'2019-11-07T13:08:59.297' AS DateTime), CAST(1326 AS Numeric(18, 0)), NULL, 0, 11, 3, N'Holy Cross College', N'Joseph Alex', CAST(N'2007-06-01T00:00:00.000' AS DateTime), N'+91 49876767668', N'ronika_jency@api-hk.com', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, N'Dheeran Managar, Trichy -12', 1, N'Axis Praha, Banglore', 34, 1,1)
GO


SET IDENTITY_INSERT [dbo].[HR_Staff] OFF
GO



INSERT [dbo].[HR_Staff_Profile] (staff_id,profile_id) values (3,9)
INSERT [dbo].[HR_Staff_Profile] (staff_id,profile_id) values (4,9)
INSERT [dbo].[HR_Staff_Profile] (staff_id,profile_id) values (7,9)


/*
--------------------------------------------------------------------------------------
    [HR_OfficeControl]			
--------------------------------------------------------------------------------------
*/

insert into [dbo].[HR_OfficeControl](StaffId,LocationId) values (6, 1)
insert into [dbo].[HR_OfficeControl](StaffId,LocationId) values (7, 1)

/*
--------------------------------------------------------------------------------------
    IT_UserType			
--------------------------------------------------------------------------------------
*/

INSERT INTO IT_UserType(Id, Label) 
	VALUES (1,'Internal User'),
		   (2, 'Customer'),
		   (3, 'Supplier'),
		   (4, 'Factory')
GO

/*
--------------------------------------------------------------------------------------
    IT_UserMaster			
--------------------------------------------------------------------------------------
*/

INSERT INTO IT_UserMaster(Login_name, Password,Active,StatusId,FullName,EntityId,StaffId,UserTypeId)
VALUES ('mosaab', 'Tm9zdHJxZHEsdXMx',1, 1, 'Mosaab JEBARAT', 1, 2,1),
		('nixon', 'bml4b24jMDY=',1, 1, 'Nixon Antony', 1,3,1), 
		('mosaab_sgt', 'Tm9zdHJxZHEsdXMx',1, 1, 'Mosaab JEBARAT', 1,null,1),
		('apidemo','YXBpZGVtbw==', 1, 1,'Test User', 1, 5, 1),
		('liven', 'YXBpZGVtbw==', 1, 1, 'Liven Varghese', 1, 1, 1),
		('claimuser', 'YXBpZGVtbw==', 1, 1,'Claim User',  1, 6, 1),
		('ronika', 'YXBpZGVtbw==',1, 1, 'Ronika Jency', 1,7,1)



INSERT INTO IT_UserMaster(Login_name, Password,Active,StatusId,FullName,EntityId,CustomerId,UserTypeId)
VALUES ('DMT','YXBpZGVtbw==', 1, 1,'DMT User', 1, NULL, 2)

INSERT INTO IT_UserMaster(Login_name, Password,Active,StatusId,FullName,EntityId,StaffId,UserTypeId)
VALUES ('inspector','YXBpZGVtbw==', 1, 1,'Inspector test', 1, 4, 1)

GO


/*
--------------------------------------------------------------------------------------
    IT_Roles			
--------------------------------------------------------------------------------------
*/

GO
SET IDENTITY_INSERT [dbo].[IT_Role] ON 
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (1, N'Operation Team', 1, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (2, N'Inspector', 1, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (3, N'Planning', 1, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (4, N'IT-Team', 1, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (5, N'Manager ( Manager Role Access )', 1, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (6, N'AE', 1, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (7, N'Expense ( Expense Role Access )', 1, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (8, N'TechnicalTeam+Management', 1, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (9, N'Operation Management', 1, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (10, N'HR ( HR Role Access)', 1, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (11, N'HR+Management', 1, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (12, N'Accounting', 1, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (13, N'KAM', 1, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (14, N'CEO', 1, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (15, N'Sales Team', 1, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (16, N'Supervisor', 1, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (17, N'OutSource', 0, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (18, N'LeaveApproveEmailHR ( HR Email Role Access )', 1, 1, 0, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (19, N'ExpenseClaimNotification', 1, 1, 0, 1)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (20, N'Quotation Request', 1, 1, 1, 1)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (21, N'Quotation Manager', 1, 1, 1, 1)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (22, N'Quotation Confirmation', 1, 1, 1, 1)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (23, N'Inspection Request', 1, 1, 1, 1)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (24, N'Inspection Confirmed', 1, 1, 1, 1)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (25, N'Inspection Verified', 1, 1, 1, 1)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (26, N'Inspection Schedule', 1, 1, 1, 1)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (27, N'Quotation Send', 1, 1, 1, 1)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (28, N'LAB(EXPENSE)', 1, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (29, N'Customer', 1, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (30, N'Supplier', 1, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (31, N'Factory', 1, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (32, N'Inspection Certificate', 1, 1, 1, 1)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (33, N'Edit Insp customer decision', 1, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (34, N'View Insp customer decision', 1, 1, 1, 0)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (35, N'Report Checker', 1, 1, 1, 1)
GO
INSERT [dbo].[IT_Role] ([Id], [RoleName], [Active], [EntityId], [PrimaryRole], [SecondaryRole]) VALUES (36, N'FBAdmin', 1, 1, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[IT_Role] OFF
GO



GO
-------- IT_Right_Type ------------------

INSERT INTO IT_Right_Type ([Name], [Active])
VALUES ('Inspection', 1),
	   ('Audit', 1)

-------- IT_Right_Type ------------------

/*
--------------------------------------------------------------------------------------
    IT_Rights			
--------------------------------------------------------------------------------------
*/

DECLARE @ParentId INT , @ParentIdLvl2 INT, @IdTran INT, @IdItem INT
SET IDENTITY_INSERT [dbo].[IT_Right] ON 

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (1, NULL, N'Dashboard', N'Dashboard', N'intdashboard', 1, 1, N'fa fa-sign-in', 1, NULL, 1, NULL, 1, 1)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (2, NULL, N'Booking', NULL, NULL, 1, 1, N'fa fa-search', 2, NULL, 2, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (3, 2, N'Audit', N'Audit', NULL, 1, 1, N'fa fa-sign-in', 3, NULL, 3, NULL, 1, 2)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (4, 3, NULL, N'Booking', N'auditedit/edit-audit', 0, 1, N'fa fa-plus-square', 4, 4, NULL, NULL, 1, 2)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (5, 3, NULL, N'Search', N'auditsummary/audit-summary', 0, 1, N'fa fa-plus-square', 5, 5, NULL, NULL, 1, 2)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (6, 2, N'Inspection', N'Inspection', NULL, 1, 1, N'fa fa-sign-in', 6, NULL, 6, NULL, 1, 1)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (7, 6, NULL, N'Booking', N'inspedit/edit-booking', 0, 1, N'fa fa-plus-square', 4, 7, NULL, NULL, 1, 1)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (8, 6, NULL, N'Purchase Orders', N'posearch/purchaseorder-summary', 0, 1, N'fa fa-plus-square', 7, 8, NULL, NULL, 1, 1)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (9, 6, NULL, N'Upload PO', N'poupload/uplaod-purchaseorder', 0, 1, N'fa fa-plus-square', 8, 9, NULL, NULL, 1, 1)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (10, NULL, N'Scheduling', NULL, NULL, 1, 1, N'fa fa-search', 6, NULL, 10, NULL, 1, 1)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (11, NULL, N'Report', N'#', NULL, 1, 1, N'fa fa-search', 8, NULL, 11, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (12, NULL, N'Invoice', N'#', NULL, 1, 1, N'fa fa-search', 10, NULL, 12, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (13, NULL, N'Support', NULL, NULL, 1, 1, N'fa fa-search', 11, NULL, 13, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (14, 13, N'Human Resource', N'Human Resource', NULL, 1, 1, N'fa fa-sign-in', 14, NULL, 14, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (15, 14, NULL, N'Staff Register', N'staffedit/edit-staff', 0, 1, N'fa fa-plus-square', 15, 15, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (16, 14, NULL, N'Staff Summary', N'staffsearch/staff-summary', 0, 1, N'fa fa-plus-square', 16, 16, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (17, 14, NULL, N'Leave Application', N'leaverequest/leave-request', 0, 1, N'fa fa-plus-square', 18, 17, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (18, 14, NULL, N'Leave Summary', N'leavesearch/leave-summary', 0, 1, N'fa fa-plus-square', 19, 18, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (19, 14, NULL, N'Leave Approval', N'leavesearch/leave-approve', 0, 1, N'fa fa-plus-square', 20, 19, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (20, 14, NULL, N'Holiday Register', N'holiday/holiday-master', 0, 1, N'fa fa-plus-square', 22, 20, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (21, 14, NULL, N'Holiday Calendar', N'holiday/show-holiday', 0, 1, N'fa fa-plus-square', 23, 21, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (22, 13, N'Expense Claim', N'Expense Claim', NULL, 1, 1, N'fa fa-sign-in', 24, NULL, 22, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (23, 22, NULL, N'Register', N'expenseclaim/expense-claim', 0, 1, N'fa fa-plus-square', 25, 23, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (24, 22, NULL, N'Summary', N'expensesearch/expenseclaim-list', 0, 1, N'fa fa-plus-square', 26, 24, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (25, 22, NULL, N'Approve', N'expensesearch/expenseclaim-approve', 0, 1, N'fa fa-plus-square', 26, 25, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (26, NULL, N'Master', NULL, NULL, 1, 1, N'fa fa-search', 27, NULL, 26, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (27, 26, N'Area Management', N'Area Management', NULL, 1, 1, N'fa fa-sign-in', 28, NULL, 27, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (28, 27, NULL, N'Country', N'country/country-summary', 0, 1, N'fa fa-plus-square', 29, 28, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (29, 27, NULL, N'Province', N'province/province-summary', 0, 1, N'fa fa-plus-square', 30, 29, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (30, 27, NULL, N'City', N'city/city-summary', 0, 1, N'fa fa-plus-square', 31, 30, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (31, 27, NULL, N'Office', N'office/office-summary', 0, 1, N'fa fa-plus-square', 32, 31, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (32, 27, NULL, N'Office Access', N'officeconfig/office-control', 0, 1, N'fa fa-plus-square', 32, 32, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (33, 26, N'Supplier/Factory', N'Supplier/Factory', NULL, 1, 1, N'fa fa-sign-in', 33, NULL, 33, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (34, 33, NULL, N'New', N'supplieredit/new-supplier', 0, 1, N'fa fa-plus-square', 34, 34, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (35, 33, NULL, N'Search', N'suppliersearch/supplier-summary', 0, 1, N'fa fa-plus-square', 35, 35, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (36, 26, N'Customer', N'Customer', NULL, 1, 1, N'fa fa-sign-in', 36, NULL, 36, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (37, 36, NULL, N'Register', N'cusedit/new-customer', 0, 1, N'fa fa-plus-square', 37, 37, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (38, 36, NULL, N'Search', N'cussearch/customer-summary', 0, 1, N'fa fa-plus-square', 38, 38, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (39, 36, NULL, N'Products', N'cusproductsearch/customer-productsummary', 0, 1, N'fa fa-plus-square', 39, 39, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (40, 26, N'Exchange Rate', N'Exchange Rate', NULL, 1, 1, N'fa fa-sign-in', 42, NULL, 40, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (41, 40, NULL, N'New', N'exchangerate/edit-exchange', 0, 1, N'fa fa-plus-square', 43, 41, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (42, 40, NULL, N'Search', N'ratematrix/rate-matrix', 0, 1, N'fa fa-plus-square', 44, 42, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (43, 26, N'Product Management', NULL, NULL, 1, 1, N'fa fa-sign-in', 15, NULL, 43, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (44, 43, NULL, N'CateSELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  ry', N'productcateSELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  ry/product-cateSELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  ry', 0, 1, N'fa fa-plus-square', 17, 44, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId)VALUES(4, @IdItem) -- IT-Team,(1, @IdItem) -- CS  
INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (45, 43, NULL, N'Sub Catery', N'productsubcatery/product-subcatery', 0, 1, N'fa fa-plus-square', 18, 45, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (46, 43, NULL, N'Sub Catery 2', N'productsub2catery/product-catery-sub2-summary', 0, 1, N'fa fa-search', 19, 46, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (47, 13, N'User Management', N'User Management', NULL, 1, 1, N'fa fa-sign-in', 27, NULL, 26, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (48, 47, NULL, N'Account Register', N'usersearch/user-account-summary', 0, 1, N'fa fa-plus-square', 28, 27, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (49, 47, NULL, N'Role-Right', N'roleright/role-right-configuration', 0, 1, N'fa fa-plus-square', 28, 29, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (50, 13, N'CS Configuration', N'CS Configuration', NULL, 1, 1, N'fa fa-sign-in', 14, NULL, 30, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (51, 50, NULL, N'Summary', N'csconfig-summary', 0, 1, N'fa fa-plus-square', 15, 31, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (52, 50, NULL, N'Register', N'csconfig-register', 0, 1, N'fa fa-plus-square', 16, 32, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (53, 6, NULL, N'Search', N'inspsummary/booking-summary', 0, 1, N'fa fa-plus-square', 6, 9, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (54, 6, NULL, N'ReInspection', N'inspsummary/reinspection-booking/1', 0, 1, N'fa fa-plus-square', 8, 9, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (55, 6, NULL, N'ReBooking', N'inspsummary/re-booking/2', 0, 1, N'fa fa-plus-square', 9, 9, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (56, 26, N'Lab', NULL, NULL, 1, 1, N'fa fa-sign-in', 15, NULL, 55, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (57, 56, NULL, N'Register', N'labedit/new-lab', 0, 1, N'fa fa-plus-square', 17, 56, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (58, 56, NULL, N'Summary', N'labsearch/lab-summary', 0, 1, N'fa fa-search', 19, 57, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (59, 27, NULL, N'County', N'county/county-summary', 0, 1, N'fa fa-plus-square', 34, 9, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (60, 27, NULL, N'Town', N'town/town-summary', 0, 1, N'fa fa-plus-square', 35, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (61, NULL, N'Quotation', NULL, NULL, 1, 1, N'fa fa-plus-square', 36, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (62, 61, NULL, N'Register', N'quotation/new-quotation', 0, 1, N'fa fa-plus-square', 37, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (63, 61, NULL, N' Approve', N'quotation/quotation-approve', 0, 1, N'fa fa-plus-square', 38, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (64, 61, NULL, N'Summary', N'quotation/quotation-summary', 0, 1, N'fa fa-plus-square', 39, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (65, 61, NULL, N'Pending', N'inspsummary/quotation-pending/3', 0, 1, N'fa fa-plus-square', 40, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (66, 61, NULL, N'Confirm', N'quotation/quotation-confirm', 0, 1, N'fa fa-plus-square', 41, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (67, 61, N'New / Edit', N'Quotation Register', NULL, 1, 0, N'fa fa-plus-square', 42, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (68, 61, NULL, N'Pending Verification', N'quotation/quotation-rejected', 0, 1, N'fa fa-sign-in', 28, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (69, 61, NULL, N'Pending Sent To Client', N'quotation/quotation-clientpending', 0, 1, N'fa fa-sign-in', 28, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (70, 6, NULL, N'Pending Verification', N'inspsummary/booking-pendingverification', 0, 1, N'fa fa-plus-square', 8, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (71, 6, NULL, N'Pending Confirmation', N'inspsummary/booking-pendingconfirmation', 0, 1, N'fa fa-plus-square', 8, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (72, 10, NULL, N'Summary', N'schedule/schedule-summary', 0, 1, N'fa fa-sign-in', 8, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (73, 10, NULL, N'Pending', N'schedule/schedule-pending', 0, 1, N'fa fa-sign-in', 8, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (74, 11, NULL, N'Inspection Report', N'report/customer-report', 0, 1, N'fa fa-plus-square', 8, NULL, NULL, NULL, 1, 1)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (75, 11, NULL, N'Filling & Review', N'reportsummary/report-summary', 0, 1, N'fa fa-plus-square', 8, NULL, NULL, NULL, 1, 1)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (76, 11, N'IC / GL', N'IC / GL', NULL, 1, 1, N'fa fa-sign-in', 28, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (77, 76, N'Register', N'Register', N'inspectioncertificateedit', 0, 1, N'fa fa-sign-in', 28, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (78, 76, N'Search', N'Search', N'inspectioncertificatesearch', 0, 1, N'fa fa-sign-in', 28, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (79, 11, NULL, N'Audit Report', N'audcusreport', 0, 1, N'fa fa-plus-square', 8, NULL, NULL, NULL, 1, 2)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (80, 76, N'Pending', N'Pending', N'inspectioncertificatepending', 0, 1, N'fa fa-sign-in', 29, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (81, 11, N'FullBridge', N'FullBridge', N'fullbridge/fullbridge-summary', 0, 1, NULL, NULL, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (82, 36, NULL, N'Extra Fields', N'dfcustomerconfigsummary/dfcustomerconfig-summary', 0, 1, NULL, NULL, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (83, 36, NULL, N'Price Card', N'pricecardsummary/price-card-summary', 0, 1, NULL, 31, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (84, 11, N'Custom KPI', N'Custom KPI', N'customkpi', 0, 1, NULL, 32, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (85, 10, N'Man Day Forecast', N'Man Day Forecast', N'schedule/qc-availability', NULL, 1, NULL, 9, NULL, NULL, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], [MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (86, 12, N'Travel Matrix', N'Travel Matrix', N'travelmatrix/travel-matrix', 0, 1, N'fa fa-sign-in', 8, NULL, 101, NULL, 1, NULL)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], 
[MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (87, NULL, N'statistics', NULL, NULL, 1, 1, N'fa fa-search', 2, 
NULL, 2, NULL, 1, NULL)

SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking],
[MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (88, 87, N'Manday Dashboard', N'Manday Dashboard', NULL,
1, 1, N'fa fa-sign-in', 3, NULL, 3, NULL, 1, 2)

SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ([Id], [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking],
[MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (89, 87, N'Manday Utilization Dashboard', N'Manday Utilization Dashboard', NULL,
1, 1, N'fa fa-sign-in', 3, NULL, 3, NULL, 1, 2)

SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS
GO

SET IDENTITY_INSERT [dbo].[IT_Right] OFF

--------------------Quotation End---------------------------------------

GO

/*
------------------------------------------------------------------------------------
   IT_UserRole		
------------------------------------------------------------------------------------
*/	

INSERT INTO IT_UserRole(RoleId, UserId)
SELECT DISTINCT IT_Role.Id, IT_UserMaster.Id 
From IT_Role,IT_UserMaster
WHERE IT_Role.RoleName = 'IT-Team' and IT_UserMaster.Login_name IN ('mosaab','nixon','mosaab_sgt','apidemo', 'liven','ronika')

INSERT INTO IT_UserRole(RoleId, UserId)
SELECT DISTINCT IT_Role.Id, IT_UserMaster.Id 
From IT_Role,IT_UserMaster
WHERE IT_Role.RoleName = 'CS' and IT_UserMaster.Login_name IN ('DMT')

INSERT INTO IT_UserRole(RoleId, UserId)
SELECT DISTINCT IT_Role.Id, IT_UserMaster.Id 
From IT_Role,IT_UserMaster
WHERE IT_Role.RoleName = 'Inspector' and IT_UserMaster.Login_name IN ('Inspector')

INSERT INTO IT_UserRole(RoleId, UserId)
SELECT TOP 1  7 As RoleId,  IT_UserMaster.Id 
From IT_UserMaster
WHERE IT_UserMaster.Login_name IN ('claimuser')

INSERT INTO IT_UserRole(RoleId, UserId)
SELECT TOP 1  5 As RoleId,  IT_UserMaster.Id 
From IT_UserMaster
WHERE IT_UserMaster.Login_name IN ('liven')

GO

/*
------------------------------------------------------------------------------------
   [dbo].[EM_ExchangeRateType]		
------------------------------------------------------------------------------------
*/	

DECLARE  @IdTran INT

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Client', NULL,15)
SELECT @IdTran = SCOPE_IDENTITY()

INSERT INTO [dbo].[EM_ExchangeRateType]([Label], [Active],[TypeTransId]) 
Values ('Customer', 1, @IdTran)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Etat des frais', NULL,15)
SELECT @IdTran = SCOPE_IDENTITY()
INSERT INTO [dbo].[EM_ExchangeRateType]([Label], [Active],[TypeTransId])  VALUES ('Expense Claim',1, @IdTran)

GO
/*
------------------------------------------------------------------------------------
   [dbo].[HR_HolidayDayType]		
------------------------------------------------------------------------------------
*/	


SET IDENTITY_INSERT [dbo].[HR_HolidayDayType] ON 
GO

DECLARE @TransId INT 

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Jour entier', NULL,16)
SELECT @TransId = SCOPE_IDENTITY()
INSERT INTO HR_HolidayDayType (Id, Label,TypeTransId) VALUES(0, 'Whole day', @TransId)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Matin', NULL,16)
SELECT @TransId = SCOPE_IDENTITY()
INSERT INTO HR_HolidayDayType (Id, Label,TypeTransId) VALUES(1, 'Morning', @TransId)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Après midi', NULL,16)
SELECT @TransId = SCOPE_IDENTITY()
INSERT INTO HR_HolidayDayType (Id, Label,TypeTransId) VALUES(2, 'Afternoon', @TransId)

SET IDENTITY_INSERT [dbo].[HR_HolidayDayType] OFF 
GO



/*
------------------------------------------------------------------------------------
   [dbo].[EC_ExpensesTypes]		
------------------------------------------------------------------------------------
*/	


SET IDENTITY_INSERT [dbo].[EC_ExpensesTypes] ON 
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId], [IsTravel]) VALUES (1, N'Travelling by Plane', 1, 59, 1,1)
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId], [IsTravel]) VALUES (2, N'Travelling by Ferry', 1, 60, 1,1)
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId], [IsTravel]) VALUES (3, N'Travelling by Taxi', 1, 61, 1,1)
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId], [IsTravel]) VALUES (4, N'Travelling by Train', 1, 62, 1,1)
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId], [IsTravel]) VALUES (5, N'Travelling by Bus', 1, 63, 1,1)
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId], [IsTravel]) VALUES (6, N'Travelling (Other modes)', 1, 64, 1,1)
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId], [IsTravel]) VALUES (7, N'Hotel Expenses', 1, 65, 1,0)
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId], [IsTravel]) VALUES (8, N'Laundry', 1, 66, 1,0)
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId], [IsTravel]) VALUES (9, N'Airport Taxes', 1, 67, 1,0)
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId], [IsTravel]) VALUES (10, N'Visa', 1, 68, 1,0)
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId], [IsTravel]) VALUES (11, N'Other Expenses', 1, 69, 1,0)
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId],[IsTravel]) VALUES (12, N'Surgery', 1, 70, 1,0)
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId],[IsTravel]) VALUES (13, N'Medicines', 1, 71, 1,0)
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId],[IsTravel]) VALUES (14, N'Out Patient', 1, 72, 1,0)
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId],[IsTravel]) VALUES (15, N'Hospitalisation', 1, 73, 1,0)
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId],[IsTravel]) VALUES (16, N'Routine Checkup', 1, 74, 1,0)
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId],[IsTravel]) VALUES (17, N'Miscellaneous expense', 1, 75, 1,0)
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId],[IsTravel]) VALUES (18, N'Stationary', 1, 76, 1,0)
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId],[IsTravel]) VALUES (19, N'Magazine Expense', 1, 77, 1,0)
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId],[IsTravel]) VALUES (20, N'Food Allowance', 1, 78, 1,0)
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId],[IsTravel]) VALUES (21, N'Manday Cost', 1, 79, 1,0)
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId],[IsTravel]) VALUES (22, N'Phone', 1, 80, 1,0)
GO
INSERT [dbo].[EC_ExpensesTypes] ([Id], [Description], [Active], [TypeTransId], [EntityId],[IsTravel]) VALUES (23, N'Entertainment', 1, 81, 1,0)
GO
SET IDENTITY_INSERT [dbo].[EC_ExpensesTypes] OFF
GO

/*
------------------------------------------------------------------------------------
   [dbo].[EC_ExpClaimStatus]	
------------------------------------------------------------------------------------
*/	

SET IDENTITY_INSERT [dbo].[EC_ExpClaimStatus] ON 
GO

DECLARE @TransId INT 

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'En attente', NULL,18)
SELECT @TransId = SCOPE_IDENTITY()
INSERT INTO [dbo].[EC_ExpClaimStatus](Id,Description, [TranId],[EntityId]) VALUES (1, 'Pending', @TransId,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Validé', NULL,18)
SELECT @TransId = SCOPE_IDENTITY()
INSERT INTO [dbo].[EC_ExpClaimStatus](Id,Description, [TranId],[EntityId]) VALUES (2, 'Approved', @TransId,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Rejeté', NULL,18)
SELECT @TransId = SCOPE_IDENTITY()
INSERT INTO [dbo].[EC_ExpClaimStatus](Id,Description, [TranId],[EntityId]) VALUES (3, 'Rejected', @TransId,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Payé', NULL,18)
SELECT @TransId = SCOPE_IDENTITY()
INSERT INTO [dbo].[EC_ExpClaimStatus](Id,Description, [TranId],[EntityId]) VALUES (4, 'Paid', @TransId,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Vérifié', NULL,18)
SELECT @TransId = SCOPE_IDENTITY()
INSERT INTO [dbo].[EC_ExpClaimStatus](Id,Description, [TranId],[EntityId]) VALUES (5, 'Checked', @TransId,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Annulé', NULL,18)
SELECT @TransId = SCOPE_IDENTITY()
insert into EC_ExpClaimStatus(Id,Description, [TranId],[EntityId]) VALUES (6, 'Canceled', @TransId,1)

SET IDENTITY_INSERT [dbo].[EC_ExpClaimStatus] OFF 
GO


/*
------------------------------------------------------------------------------------
   [dbo].[EC_PaymenTypes]
------------------------------------------------------------------------------------
*/	


SET IDENTITY_INSERT [dbo].[EC_PaymenTypes] ON 
GO

DECLARE @TransId INT 

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Cash', NULL,19)
SELECT @TransId = SCOPE_IDENTITY()
INSERT INTO [dbo].[EC_PaymenTypes](Id,[Description], [TransId],[EntityId]) VALUES (1, 'Cash', @TransId,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Chèque', NULL,19)
SELECT @TransId = SCOPE_IDENTITY()
INSERT INTO [dbo].[EC_PaymenTypes](Id,[Description], [TransId],[EntityId]) VALUES (2, 'Cheque', @TransId,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Projet de demande', NULL,19)
SELECT @TransId = SCOPE_IDENTITY()
INSERT INTO [dbo].[EC_PaymenTypes](Id,[Description], [TransId],[EntityId]) VALUES (3, 'Demand Draft', @TransId,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Carte de crédit', NULL,19)
SELECT @TransId = SCOPE_IDENTITY()
INSERT INTO [dbo].[EC_PaymenTypes](Id,[Description], [TransId],[EntityId]) VALUES (4, 'Credit Card', @TransId,1)

SET IDENTITY_INSERT [dbo].[EC_PaymenTypes] OFF 
GO

/*
------------------------------------------------------------------------------------
   [dbo].[Season]
------------------------------------------------------------------------------------
*/	

SET IDENTITY_INSERT [dbo].[REF_Season] ON 
GO
INSERT [dbo].[REF_Season] ([Id], [Name], [Code], [Default], [Active], [EntityId]) VALUES (1, N'Summer', N'', 1, 1, 1)
GO
INSERT [dbo].[REF_Season] ([Id], [Name], [Code], [Default], [Active], [EntityId]) VALUES (2, N'Winter', N'', 1, 1, 1)
GO
INSERT [dbo].[REF_Season] ([Id], [Name], [Code], [Default], [Active], [EntityId]) VALUES (3, N'Id-group-season', N'', NULL, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[REF_Season] OFF
GO

/*
------------------------------------------------------------------------------------
   [dbo].[Customer season]
------------------------------------------------------------------------------------
	

SET IDENTITY_INSERT [dbo].[CU_Season] ON 
GO
INSERT [dbo].[CU_Season] ([Id], [Customer_Id], [Season_Id], [Active]) VALUES (1, 35, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[CU_Season] OFF
GO
*/

/*
------------------------------------------------------------------------------------
   [dbo].[Evaluation Round]
------------------------------------------------------------------------------------
*/
SET IDENTITY_INSERT [dbo].[AUD_EvaluationRound] ON 
GO
INSERT [dbo].[AUD_EvaluationRound] ([Id], [Name], [Active], [EntityId]) VALUES (1, N'Initial', 1, 1)
GO
INSERT [dbo].[AUD_EvaluationRound] ([Id], [Name], [Active], [EntityId]) VALUES (2, N'Round 1', 1, 1)
GO
SET IDENTITY_INSERT [dbo].[AUD_EvaluationRound] OFF
GO

/*
------------------------------------------------------------------------------------
   [dbo].[season year]
------------------------------------------------------------------------------------
*/

SET IDENTITY_INSERT [dbo].[REF_Season_Year] ON 
GO
INSERT [dbo].[REF_Season_Year] ([Id], [Year], [Active]) VALUES (1, 2019, 1)
GO
INSERT [dbo].[REF_Season_Year] ([Id], [Year], [Active]) VALUES (2, 2020, 1)
GO
SET IDENTITY_INSERT [dbo].[REF_Season_Year] OFF
GO

/*
------------------------------------------------------------------------------------
   [dbo].[Service]
------------------------------------------------------------------------------------
*/

SET IDENTITY_INSERT [dbo].[REF_Service] ON 
GO
INSERT [dbo].[REF_Service] ([Id], [Name], [Active]) VALUES (1, N'Inspection', 1)
GO
INSERT [dbo].[REF_Service] ([Id], [Name], [Active]) VALUES (2, N'Audit', 1)
GO
SET IDENTITY_INSERT [dbo].[REF_Service] OFF
GO

/*
------------------------------------------------------------------------------------
   [dbo].[Service Type]
------------------------------------------------------------------------------------
*/

SET IDENTITY_INSERT [dbo].[REF_ServiceType] ON 
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (1, N'Social Audit', 1, 1, NULL, NULL, NULL)
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (2, N'Chemical Audit', 1, 1, NULL, NULL, NULL)
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (3, N'Technical Audit', 1, 1, NULL, NULL, NULL)
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (4, N'Enviornmental Audit', 1, 1, NULL, NULL, NULL)
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (5, N'Final Random Inspection', 1, 1, 0, 151, N'FRI 
')
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (6, N'Initial Production Check', 1, 1, 0, 152, N'IPC
')
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (7, N'During Production Inspection', 1, 1, 0, 153, N'DPI
')
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (8, N'Container Loading Supervision', 1, 1, 0, 154, N'CLS
')
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (9, N'Full Production Check', 1, 1, 0, 155, NULL)
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (10, N'Picking Only', 1, 1, 0, 156, NULL)
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (11, N'Pre-production Visit', 1, 1, 0, 157, N'PPV
')
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (12, N'Inline inspection', 0, 1, 0, 158, NULL)
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (13, N'Factory Self-Inspection', 1, 1, 0, 159, N'FSI
')
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (14, N'Pallet Inspection', 1, 1, 0, 160, N'PI
')
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (15, N'Final Random Re-Inspection', 1, 1, 1, 322, NULL)
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (16, N'Witness Audit', 1, 1, NULL, NULL, NULL)
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (17, N'Construction Check', 1, 1, 0, 240, N'CC')
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (18, N'Golden Sample Validation', 1, 1, 0, 241, N'GSV')
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (19, N'In-Production Process Assessment', 1, 1, 0, 242, N'IPPA')
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (20, N'Product Verification', 1, 1, 0, 243, N'PV')
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (21, N'Sample Check', 1, 1, 0, 244, N'SampleCheck')
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (22, N'On-site Critical Test', 1, 1, 0, 245, N'OnsiteCriticalTest')
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (23, N'Sedex Audit', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation]) VALUES (24, N'Self Inspection Training', 1, NULL, NULL, NULL, N'SIT')
GO
SET IDENTITY_INSERT [dbo].[REF_ServiceType] OFF
GO

/*
------------------------------------------------------------------------------------
   [dbo].[Service Type]
------------------------------------------------------------------------------------


SET IDENTITY_INSERT [dbo].[CU_ServiceType] ON 
GO
INSERT [dbo].[CU_ServiceType] ([Id], [CustomerId], [ServiceId], [ServiceTypeId], [Active]) VALUES (1, 35, 1, 1, 1)
GO
INSERT [dbo].[CU_ServiceType] ([Id], [CustomerId], [ServiceId], [ServiceTypeId], [Active]) VALUES (2, 35, 1, 2, 1)
GO
SET IDENTITY_INSERT [dbo].[CU_ServiceType] OFF
GO
*/

/*
------------------------------------------------------------------------------------
   [dbo].[customer Brand]
------------------------------------------------------------------------------------


SET IDENTITY_INSERT [dbo].[CU_Brand] ON 
GO
INSERT [dbo].[CU_Brand] ([Id], [Name], [Customer_Id], [Active]) VALUES (1, N'GAP', 35, 1)
GO
SET IDENTITY_INSERT [dbo].[CU_Brand] OFF
GO

------------------------------------------------------------------------------------
   [dbo].[customer contact]
------------------------------------------------------------------------------------

SET IDENTITY_INSERT [dbo].[CU_Contact] ON 
INSERT [dbo].[CU_Contact] ([Id], [Customer_id], [Contact_name], [active],[Phone],[Email],[Office]) VALUES (1, 35, N'jino', 1,'12345678','jino@g.com',1)
GO
INSERT [dbo].[CU_Contact] ([Id], [Customer_id], [Contact_name], [active],[Phone],[Email],[Office]) VALUES (2, 35, N'manu', 1,'12345678','manu@g.com',1)
GO
SET IDENTITY_INSERT [dbo].[CU_Contact] OFF
GO

------------------------------------------------------------------------------------
   [dbo].[customer department]
------------------------------------------------------------------------------------

SET IDENTITY_INSERT [dbo].[CU_Department] ON 
GO
INSERT [dbo].[CU_Department] ([Id], [Name], [Customer_Id], [Active],code) VALUES (1, N'accessories', 35, 1,'')
GO
INSERT [dbo].[CU_Department] ([Id], [Name], [Customer_Id], [Active],code) VALUES (2, N'garment', 35, 1,'')
GO
SET IDENTITY_INSERT [dbo].[CU_Department] OFF
GO
*/

/*
------------------------------------------------------------------------------------
   [dbo].[Contact Type]
------------------------------------------------------------------------------------
*/

SET IDENTITY_INSERT [dbo].[CU_ContactType] ON 
GO
INSERT [dbo].[CU_ContactType] ([Id], [ContactType],[EntityId]) VALUES (1,'Operations',1)
GO
INSERT [dbo].[CU_ContactType] ([Id], [ContactType],[EntityId]) VALUES (2,'Accounting',1)
GO
INSERT [dbo].[CU_ContactType] ([Id], [ContactType],[EntityId]) VALUES (3,'Management',1)
GO
INSERT [dbo].[CU_ContactType] ([Id], [ContactType],[EntityId]) VALUES (4,'QC manager',1)
GO
INSERT [dbo].[CU_ContactType] ([Id], [ContactType],[EntityId]) VALUES (5,'Merchandiser manager',1)
GO
INSERT [dbo].[CU_ContactType] ([Id], [ContactType],[EntityId]) VALUES (6,'BD manager',1)
GO
SET IDENTITY_INSERT [dbo].[CU_ContactType] OFF
GO

/*
------------------------------------------------------------------------------------
   [dbo].[customer config]
------------------------------------------------------------------------------------

SET IDENTITY_INSERT [dbo].[CU_CS_Configuration] ON 
GO
INSERT [dbo].[CU_CS_Configuration] ([Id], [Customer_Id], [User_Id], [Active]) VALUES (1, 35, 3, 1)
GO
SET IDENTITY_INSERT [dbo].[CU_CS_Configuration] OFF
GO

------------------------------------------------------------------------------------
   [dbo].[Office Control]
------------------------------------------------------------------------------------

INSERT [dbo].[HR_OfficeControl] ([StaffId], [LocationId]) VALUES (3, 1)
GO

*/

/*
------------------------------------------------------------------------------------
   [dbo].[Booking Contact]
------------------------------------------------------------------------------------


SET IDENTITY_INSERT [dbo].[AUD_BookingContact] ON 
GO
INSERT [dbo].[AUD_BookingContact] ([Id], [Factory_Country_Id], [Office_Id], [Booking_EmailTo], [BookingEmailCC], [PenaltyEmail], [ContactInformation], [Active],EntityId) VALUES (1, 44, 1, N'test@gmail.com', N'test2@gmail.com', NULL, N'Contact Operation team', 1,1)
SET IDENTITY_INSERT [dbo].[AUD_BookingContact] OFF
GO
*/

/*
------------------------------------------------------------------------------------
   [dbo].[Booking Contact]
------------------------------------------------------------------------------------

SET IDENTITY_INSERT [dbo].[AUD_BookingRules] ON 
GO
INSERT [dbo].[AUD_BookingRules] ([Id], [Customer_id], [LeadDays], [Factory_CountryId], [IsDefault], [Active], [Booking_Rule],[EntityId]) VALUES (1, NULL, 5, 44, 1, 1, N'follow the rules',1)
SET IDENTITY_INSERT [dbo].[AUD_BookingRules] OFF
GO
*/

/*
------------------------------------------------------------------------------------
   [dbo].[Audit Status]
------------------------------------------------------------------------------------
*/

SET IDENTITY_INSERT [dbo].[AUD_Status] ON 
GO
INSERT [dbo].[AUD_Status] ([Id], [Status], [Active]) VALUES (1, N'Received', 1)
GO
INSERT [dbo].[AUD_Status] ([Id], [Status], [Active]) VALUES (2, N'Confirmed', 1)
GO
INSERT [dbo].[AUD_Status] ([Id], [Status], [Active]) VALUES (3, N'Postpone', 1)
GO
INSERT [dbo].[AUD_Status] ([Id], [Status], [Active]) VALUES (4, N'Cancel', 1)
GO
INSERT [dbo].[AUD_Status] ([Id], [Status], [Active]) VALUES (5, N'Scheduled', 1)
GO
INSERT [dbo].[AUD_Status] ([Id], [Status], [Active]) VALUES (6, N'Audited', 1)
GO
SET IDENTITY_INSERT [dbo].[AUD_Status] OFF
GO

/*
------------------------------------------------------------------------------------
   [dbo].[Audit Booking Contact]
------------------------------------------------------------------------------------
*/
GO
SET IDENTITY_INSERT [dbo].[AUD_BookingContact] ON 
GO
INSERT [dbo].[AUD_BookingContact] ([Id], [Factory_Country_Id], [Office_Id], [Booking_EmailTo], [BookingEmailCC], [PenaltyEmail], [ContactInformation], [Active], [EntityId]) VALUES (1, 44, 1, N'', N'', N'', N'', 1, 1)
GO
SET IDENTITY_INSERT [dbo].[AUD_BookingContact] OFF
GO


/*
------------------------------------------------------------------------------------
   [dbo].[Audit Cancel / reschedule Reason]
------------------------------------------------------------------------------------
*/

GO
SET IDENTITY_INSERT [dbo].[AUD_Cancel_Reschedule_Reasons] ON 
GO
INSERT [dbo].[AUD_Cancel_Reschedule_Reasons] ([Id], [Reason], [IsCancel], [IsReschedule], [IsDefault], [IsSgT], [Customer_Id], [Active], [EntityId]) VALUES (1, N'Goods not ready', 1, 0, 1, 1, NULL, 1, 1)
GO
INSERT [dbo].[AUD_Cancel_Reschedule_Reasons] ([Id], [Reason], [IsCancel], [IsReschedule], [IsDefault], [IsSgT], [Customer_Id], [Active], [EntityId]) VALUES (2, N'Supplier Postpone', 0, 1, 1, 0, NULL, 1, 1)
GO
INSERT [dbo].[AUD_Cancel_Reschedule_Reasons] ([Id], [Reason], [IsCancel], [IsReschedule], [IsDefault], [IsSgT], [Customer_Id], [Active], [EntityId]) VALUES (3, N'Qc not vailable', 0, 1, 1, 1, NULL, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[AUD_Cancel_Reschedule_Reasons] OFF
GO

/*
------------------------------------------------------------------------------------
   [dbo].[Audit work process]
------------------------------------------------------------------------------------
*/

SET IDENTITY_INSERT [dbo].[AUD_WorkProcess] ON 
INSERT INTO [dbo].[AUD_WorkProcess]([Id],[Name],[Active],[Entity_Id])VALUES(1,'cutting',1,1)
GO
INSERT INTO [dbo].[AUD_WorkProcess]([Id],[Name],[Active],[Entity_Id])VALUES(2,'sewing',1,1)
GO
SET IDENTITY_INSERT [dbo].[AUD_WorkProcess] OFF
GO

/*
------------------------------------------------------------------------------------
   [dbo].[Audit type]
------------------------------------------------------------------------------------
*/

SET IDENTITY_INSERT [dbo].[AUD_Type] ON 
INSERT INTO [dbo].[AUD_Type]([Id],[Name],[Active],[Entity_Id])VALUES(1,'Announced',1,1)
GO
INSERT INTO [dbo].[AUD_Type]([Id],[Name],[Active],[Entity_Id])VALUES(2,'Semi-announced',1,1)
GO
INSERT INTO [dbo].[AUD_Type]([Id],[Name],[Active],[Entity_Id])VALUES(3,'Unannounced',1,1)
GO
SET IDENTITY_INSERT [dbo].[AUD_Type] OFF
GO

/*
------------------------------------------------------------------------------------
   [dbo].[Audit Booking Rule]
------------------------------------------------------------------------------------
*/

SET IDENTITY_INSERT [dbo].[AUD_BookingRules] ON 
GO
INSERT [dbo].[AUD_BookingRules] ([Id], [Customer_id], [LeadDays], [Factory_CountryId], [IsDefault], [Active], [Booking_Rule],[EntityId]) VALUES (1, NULL, 5, 44, 1, 1, N'',1)
SET IDENTITY_INSERT [dbo].[AUD_BookingRules] OFF
GO

/*
------------------------------------------------------------------------------------
   [dbo].[EC_FoodAllowance]
------------------------------------------------------------------------------------
*/	

GO
SET IDENTITY_INSERT [dbo].[EC_FoodAllowance] ON 
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (37, 14, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(23.00 AS Decimal(18, 2)), 51, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (38, 18, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(500.00 AS Decimal(18, 2)), 14, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (39, 21, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(23.00 AS Decimal(18, 2)), 51, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (41, 38, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(40.00 AS Decimal(18, 2)), 34, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (42, 72, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(23.00 AS Decimal(18, 2)), 51, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (43, 73, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(23.00 AS Decimal(18, 2)), 51, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (44, 80, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(23.00 AS Decimal(18, 2)), 51, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (45, 83, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(23.00 AS Decimal(18, 2)), 51, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (46, 96, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(70.00 AS Decimal(18, 2)), 63, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (47, 99, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(385.00 AS Decimal(18, 2)), 70, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (48, 100, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(86207.00 AS Decimal(18, 2)), 68, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (49, 105, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(23.00 AS Decimal(18, 2)), 51, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (50, 113, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(14493.00 AS Decimal(18, 2)), 177, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (52, 117, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(23.00 AS Decimal(18, 2)), 51, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (53, 123, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(23.00 AS Decimal(18, 2)), 51, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (54, 129, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(32.00 AS Decimal(18, 2)), 108, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (56, 150, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(23.00 AS Decimal(18, 2)), 51, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (57, 162, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(706.00 AS Decimal(18, 2)), 121, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (58, 169, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(353.00 AS Decimal(18, 2)), 120, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (59, 172, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(23.00 AS Decimal(18, 2)), 51, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (60, 191, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(11.00 AS Decimal(18, 2)), 134, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (61, 192, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(23.00 AS Decimal(18, 2)), 51, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (62, 193, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(23.00 AS Decimal(18, 2)), 51, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (63, 198, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(23.00 AS Decimal(18, 2)), 51, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (64, 4, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(270.00 AS Decimal(18, 2)), 152, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (65, 3, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(200.00 AS Decimal(18, 2)), 145, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (66, 2, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(166667.00 AS Decimal(18, 2)), 176, 1, 1)
GO
INSERT [dbo].[EC_FoodAllowance] ([Id], [CountryId], [StartDate], [EndDate], [FoodAllowance], [CurrencyId], [UserId], [EntityId]) VALUES (67, 1, CAST(N'2019-01-01T00:00:00.000' AS DateTime), CAST(N'2019-12-31T00:00:00.000' AS DateTime), CAST(16.00 AS Decimal(18, 2)), 156, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[EC_FoodAllowance] OFF
GO

/*
------------------------------------------------------------------------------------
   [dbo].[EC_Status_Role]
------------------------------------------------------------------------------------
*/	

INSERT INTO EC_Status_Role(IdRole, IdStatus)
VALUES(4, 1), (4, 2), (4, 3), (4, 4), (4, 5) -- IT-TEAM 
	 , (5, 5), (5,3) -- management : checked + rejected
	  ,(7, 2), (7,4) -- Claim access : approved + payed
GO

/*
------------------------------------------------------------------------------------
   [dbo].[HR_Leave_Status]
------------------------------------------------------------------------------------
*/	

SET IDENTITY_INSERT [dbo].[HR_Leave_Status] ON 
GO

DECLARE @IdTran INT

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Demande', NULL,20)
SELECT @IdTran =  SCOPE_IDENTITY()

INSERT INTO  [dbo].[HR_Leave_Status](Id,Label, IdTran)
VALUES (1, 'Request', @IdTran)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Validé', NULL,20)
SELECT @IdTran =  SCOPE_IDENTITY()

INSERT INTO  [dbo].[HR_Leave_Status](Id,Label, IdTran)
VALUES (3, 'Approved', @IdTran)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Rejeté', NULL,20)
SELECT @IdTran =  SCOPE_IDENTITY()

INSERT INTO  [dbo].[HR_Leave_Status](Id,Label, IdTran)
VALUES (4, 'Rejected', @IdTran)


INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Cancelled', NULL,20)
SELECT @IdTran =  SCOPE_IDENTITY()

INSERT INTO [dbo].[HR_Leave_Status](Id,[Label],[IdTran],[EntityId])
VALUES  (5,'Cancelled',@IdTran,1)

GO         

SET IDENTITY_INSERT [dbo].[HR_Leave_Status] OFF 
GO



/*
------------------------------------------------------------------------------------
   [dbo].[HR_Leave_Type]
------------------------------------------------------------------------------------
*/	

SET IDENTITY_INSERT [dbo].[HR_Leave_Type] ON 
GO
DECLARE @IdTran INT

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Annuel', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (1, N'Annual', 1, NULL,@IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Annuel', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (2, N'Maternié', 1, NULL,@IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Congé maladie', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (3, N'Sick leave', 1,NULL,@IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Accident', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (4, N'Casual', 1, NULL,@IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Départ précoce', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (7, N'Early Leaving', 1, NULL,@IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Educatif', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (8, N'Educational', 1, NULL,@IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Congé non payé', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (19, N'Unpaid Leave', 1, NULL,@IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Congé de mariage', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (20, N'Wedding leave', 1,NULL,@IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Congé de funéraire', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (21, N'Funeral leave', 1,NULL,@IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'En service', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (22, N'On Duty', 1, NULL,@IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Congé paternité', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (25, N'Paternity Leave', 1, NULL,@IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Congé mariage', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (26, N'Marriage Leave', 1, NULL, @IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Congé récompense', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (27, N'Compensation Leave', 1, NULL,@IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Congé Paternité (10 jours)', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (32, N'Paternity leave(10 days)', 1, 10,@IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Congé maternié (98 jours)', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (33, N'Maternity(98 days)', 1, 98,@IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Congé mariage (3 jours)', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (34, N'Marriage leave(3 days)', 1, 3, @IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Congé maternité (15 jours)', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (35, N'Maternity(15 days)', 1, 15, @IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Congé maternité (35 jours)', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (36, N'Maternity(35 days)', 1, 35, @IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Congé maternité (42 jours)', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (37, N'Maternity(42 days)', 1, 42, @IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Congé maternité (13 jours)', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (38, N'Marriage leave(13 days)', 1, 13, @IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Allaitement', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (40, N'Breastfeeding leave', 1,NULL,@IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Maternité (30 jours)', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (41, N'Maternity(30 days)', 1, 1, @IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Bilan de grossesse', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (42, N'Pregnancy check-ups', 1,NULL,@IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Congé funéraire', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (43, N'Funeral leave (3 days)', 1, 3, @IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Congé paternité (15 jours)', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (44, N'Paternity leave (15 days)', 1, 15, @IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Congé Césarienne (30 jours)', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (45, N'Caesarean leave( 30 days)', 1, 30, @IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Maternité (50 jours)', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (46, N'Maternity(50 days)', 1, 50, @IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Maternité (80 jours)', 1,21)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[HR_Leave_Type] ([Id], [Description],  [Active], [Total_Days], [IdTran],[EntityId]) VALUES (47, N'Maternity(80 days)', 1, 80, @IdTran,1)
GO
SET IDENTITY_INSERT [dbo].[HR_Leave_Type] OFF
GO

/*
------------------------------------------------------------------------------------
   [dbo].[Pick Type and level]
------------------------------------------------------------------------------------
*/

SET IDENTITY_INSERT [dbo].[REF_PickType] ON 
GO

Insert into REF_PickType(Id,Value,Active) Values (1,'Single',1)
Insert into REF_PickType(Id,Value,Active) Values (2,'Double',1)

SET IDENTITY_INSERT [dbo].[REF_PickType] OFF 
GO

SET IDENTITY_INSERT [dbo].[REF_Pick1] ON 
GO

Insert into REF_Pick1(Id,Value,Active) Values (1,0,1)
Insert into REF_Pick1(Id,Value,Active) Values (2,0.065,1)
Insert into REF_Pick1(Id,Value,Active) Values (3,0.1,1)
Insert into REF_Pick1(Id,Value,Active) Values (4,0.15,1)
Insert into REF_Pick1(Id,Value,Active) Values (5,0.25,1)
Insert into REF_Pick1(Id,Value,Active) Values (6,0.4,1)
Insert into REF_Pick1(Id,Value,Active) Values (7,0.65,1)
Insert into REF_Pick1(Id,Value,Active) Values (8,1,1)
Insert into REF_Pick1(Id,Value,Active) Values (9,1.5,1)
Insert into REF_Pick1(Id,Value,Active) Values (10,2.5,1)
Insert into REF_Pick1(Id,Value,Active) Values (11,4,1)
Insert into REF_Pick1(Id,Value,Active) Values (12,6.5,1)

SET IDENTITY_INSERT [dbo].[REF_Pick1] OFF 
GO

SET IDENTITY_INSERT [dbo].[REF_Pick2] ON 
GO

Insert into REF_Pick2(Id,Value,Active) Values (1,0.65,1)
Insert into REF_Pick2(Id,Value,Active) Values (2,1,1)
Insert into REF_Pick2(Id,Value,Active) Values (3,1.5,1)
Insert into REF_Pick2(Id,Value,Active) Values (4,2.5,1)
Insert into REF_Pick2(Id,Value,Active) Values (5,4,1)
Insert into REF_Pick2(Id,Value,Active) Values (6,6.5,1)

SET IDENTITY_INSERT [dbo].[REF_Pick2] OFF
GO

SET IDENTITY_INSERT [dbo].[REF_LevelPick1] ON 
GO

Insert into REF_LevelPick1(Id,Value,Active) Values (1,'I',1)
Insert into REF_LevelPick1(Id,Value,Active) Values (2,'II',1)
Insert into REF_LevelPick1(Id,Value,Active) Values (3,'III',1)
Insert into REF_LevelPick1(Id,Value,Active) Values (4,'S1',1)
Insert into REF_LevelPick1(Id,Value,Active) Values (5,'S2',1)
Insert into REF_LevelPick1(Id,Value,Active) Values (6,'S3',1)

SET IDENTITY_INSERT [dbo].[REF_LevelPick1] OFF 
GO

SET IDENTITY_INSERT [dbo].[REF_LevelPick2] ON 
GO

Insert into REF_LevelPick2(Id,Value,Active) Values (1,'I',1)
Insert into REF_LevelPick2(Id,Value,Active) Values (2,'II',1)
Insert into REF_LevelPick2(Id,Value,Active) Values (3,'III',1)
Insert into REF_LevelPick2(Id,Value,Active) Values (4,'S1',1)
Insert into REF_LevelPick2(Id,Value,Active) Values (5,'S2',1)
Insert into REF_LevelPick2(Id,Value,Active) Values (6,'S3',1)
Insert into REF_LevelPick2(Id,Value,Active) Values (7,'S4',1)

SET IDENTITY_INSERT [dbo].[REF_LevelPick2] OFF 
GO

/*
------------------------------------------------------------------------------------
   [dbo].[Defect Classification]
------------------------------------------------------------------------------------
*/

SET IDENTITY_INSERT [dbo].[REF_DefectClassification] ON 
GO

Insert into REF_DefectClassification(Id,Value,Active,EntityId) Values (1,'Size',1,1)
Insert into REF_DefectClassification(Id,Value,Active,EntityId) Values (2,'Color',1,1)
Insert into REF_DefectClassification(Id,Value,Active,EntityId) Values (3,'No Classification',1,1)
Insert into REF_DefectClassification(Id,Value,Active,EntityId) Values (4,'Size And Color',1,1)

SET IDENTITY_INSERT [dbo].[REF_DefectClassification] OFF 
GO

/*
------------------------------------------------------------------------------------
   [dbo].[ Report Unit]
------------------------------------------------------------------------------------
*/

SET IDENTITY_INSERT [dbo].[REF_ReportUnit] ON 
GO

Insert into REF_ReportUnit(Id,Value,Active) Values (1,'CM',1)
Insert into REF_ReportUnit(Id,Value,Active) Values (2,'INCH',1)

SET IDENTITY_INSERT [dbo].[REF_ReportUnit] OFF 
GO

/*
------------------------------------------------------------------------------------
   [dbo].[Product Sub category]
------------------------------------------------------------------------------------
*/

SET IDENTITY_INSERT [dbo].[REF_ProductCategory_Sub] ON 
GO

GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (1, N'Household Appliance', 1, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (2, N'Lighting', 1, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (3, N'Consumer electronic', 1, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (4, N'Computer Hardware', 1, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (5, N'Audio & Video', 1, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (7, N'Power tools', 1, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (8, N'Outdoor Furniture', 2, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (9, N'Indoor Furniture', 2, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (10, N'Children Furniture', 2, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (11, N'Electrical Toys', 3, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (12, N'Non Electrical Toys', 3, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (13, N'Baby Care item', 4, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (14, N'Hand tools', 5, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (15, N'Pneumatic tools', 5, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (16, N'Hardware', 5, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (17, N'Tool accessaries', 5, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (18, N'Fitness', 6, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (19, N'Water sports', 6, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (20, N'Sporting items', 6, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (21, N'Camping', 6, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (22, N'PPE', 7, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (23, N'Body beauty', 7, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (24, N'Personal Ornament', 7, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (25, N'kitchen item', 8, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (26, N'Bathroom item', 8, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (27, N'Home decoration', 8, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (28, N'Home accessories', 8, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (29, N'Garden', 8, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (30, N'Pet Products', 8, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (31, N'Stationery', 9, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (32, N'Luggage & Bags', 10, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (33, N'Car Accessories', 11, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (34, N'NFR', 12, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (35, N'Garment', 13, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (36, N'Footware', 13, 1, 1)
GO
INSERT [dbo].[REF_ProductCategory_Sub] ([Id], [Name], [ProductCategoryID], [Active], [EntityId]) VALUES (37, N'Home textile', 13, 1, 1)
GO

SET IDENTITY_INSERT [dbo].[REF_ProductCategory_Sub] OFF 
GO

/*
------------------------------------------------------------------------------------
   [dbo].[REF_ProductCategory_Sub2]
------------------------------------------------------------------------------------
*/

SET IDENTITY_INSERT [dbo].[REF_ProductCategory_Sub2] ON 
GO

Insert into REF_ProductCategory_Sub2(Id,Name,ProductSubCategoryID,Active,EntityId) Values (1,'HP-Telivision',1,1,1)
Insert into REF_ProductCategory_Sub2(Id,Name,ProductSubCategoryID,Active,EntityId) Values (2,'TOSHIBA-Telivision',1,1,1)
Insert into REF_ProductCategory_Sub2(Id,Name,ProductSubCategoryID,Active,EntityId) Values (3,'HP-Laptop',2,1,1)
Insert into REF_ProductCategory_Sub2(Id,Name,ProductSubCategoryID,Active,EntityId) Values (4,'TOSHIBA-Laptop',2,1,1)
Insert into REF_ProductCategory_Sub2(Id,Name,ProductSubCategoryID,Active,EntityId) Values (5,'Whirlpool-Refrigerator',3,1,1)
Insert into REF_ProductCategory_Sub2(Id,Name,ProductSubCategoryID,Active,EntityId) Values (6,'LG-Refrigerator',3,1,1)
Insert into REF_ProductCategory_Sub2(Id,Name,ProductSubCategoryID,Active,EntityId) Values (7,'Dining Table',4,1,1)
Insert into REF_ProductCategory_Sub2(Id,Name,ProductSubCategoryID,Active,EntityId) Values (8,'Drink Table',4,1,1)
Insert into REF_ProductCategory_Sub2(Id,Name,ProductSubCategoryID,Active,EntityId) Values (9,'Shoes',5,1,1)
Insert into REF_ProductCategory_Sub2(Id,Name,ProductSubCategoryID,Active,EntityId) Values (10,'Jacket',5,1,1)

SET IDENTITY_INSERT [dbo].[REF_ProductCategory_Sub2] OFF 
GO

/*
------------------------------------------------------------------------------------
   [dbo].[REF_ProductCategory_Sub2]
------------------------------------------------------------------------------------


SET IDENTITY_INSERT [dbo].[CU_Products] ON 
GO

Insert into CU_Products(Id,ProductID,[Product Description],ProductCategory,ProductSubCategory,
InternalProductType,CustomerID,Active,CreatedBy,CreatedTime)
Values(1,'PR101','108 Litres Refrigerator',1,3,5,35,1,1,getdate())


Insert into CU_Products(Id,ProductID,[Product Description],ProductCategory,ProductSubCategory,
InternalProductType,CustomerID,Active,CreatedBy,CreatedTime)
Values(2,'PR102','108 Litres Refrigerator',1,3,6,35,1,1,getdate())


SET IDENTITY_INSERT [dbo].[CU_Products] OFF 
GO
*/

/*
------------------------------------------------------------------------------------
   [dbo].[MID_TaskType]
------------------------------------------------------------------------------------
*/	
INSERT [dbo].[MID_TaskType] ([Id], [Label], [EntityId]) VALUES (1, N'Leave To Approve', 1)
GO
INSERT [dbo].[MID_TaskType] ([Id], [Label], [EntityId]) VALUES (2, N'Expense to approve', 1)
GO
INSERT [dbo].[MID_TaskType] ([Id], [Label], [EntityId]) VALUES (3, N'Expense to check', 1)
GO
INSERT [dbo].[MID_TaskType] ([Id], [Label], [EntityId]) VALUES (4, N'Expense to Pay', 1)
GO
INSERT [dbo].[MID_TaskType] ([Id], [Label], [EntityId]) VALUES (5, N'Verify Inspection', 1)
GO
INSERT [dbo].[MID_TaskType] ([Id], [Label], [EntityId]) VALUES (6, N'Confirm Inspection', 1)
GO
INSERT [dbo].[MID_TaskType] ([Id], [Label], [EntityId]) VALUES (7, N'quotation approve', 1)
GO
INSERT [dbo].[MID_TaskType] ([Id], [Label], [EntityId]) VALUES (8, N'SplitInspectionBooking', 1)
GO
INSERT [dbo].[MID_TaskType] ([Id], [Label], [EntityId]) VALUES (9, N'Inspection Schedule', 1)
GO
INSERT [dbo].[MID_TaskType] ([Id], [Label], [EntityId]) VALUES (10, N'Quotation Modify', 1)
GO
INSERT [dbo].[MID_TaskType] ([Id], [Label], [EntityId]) VALUES (11, N'Quotation Sent', 1)
GO
INSERT [dbo].[MID_TaskType] ([Id], [Label], [EntityId]) VALUES (12, N'Quotation Customer Confirmed', 1)
GO
INSERT [dbo].[MID_TaskType] ([Id], [Label], [EntityId]) VALUES (13, N'Quotation Customer Reject', 1)
GO
INSERT [dbo].[MID_TaskType] ([Id], [Label], [EntityId]) VALUES (14, N'Quotation Pending', 1)
GO

GO


/*
------------------------------------------------------------------------------------
   [dbo].[MID_NotificationType]
------------------------------------------------------------------------------------
*/	
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (1, N'Leave Approved', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (2, N'Leave Rejected', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (3, N'Expense approved', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (4, N'Expense checked', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (5, N'Expense Paied', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (6, N'Expense Rejected', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (7, N'LeaveApproveEmailHR', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (8, N'Leave Cancelled', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (9, N'Expense Cancelled', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (10, N'Inspection Requested', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (11, N'Inspection Confirmed', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (12, N'Inspection Cancelled', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (13, N'Inspection Modified', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (14, N'Inspection Verified', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (15, N'Inspection Rescheduled', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (16, N'Inspection Split', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (17, N'Quoatation Created', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (18, N'Quoatation Approved', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (19, N'Quoatation Sent', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (20, N'Quoatation Customer Confirmed', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (21, N'Quoatation Customer Rejected', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (22, N'Quoatation Cancelled', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (23, N'Quoatation Rejected', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (24, N'Quoatation Modified', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (25, N'Booking Quantity Change', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (26, N'Report Validated', 1)
GO
INSERT [dbo].[MID_NotificationType] ([Id], [Label], [EntityId]) VALUES (27, N'Inspection Hold', 1)
GO

		   
GO

/*
--------------------------------------------------------------------------------------
   [SU_Level]		
--------------------------------------------------------------------------------------
*/

INSERT INTO  [dbo].[SU_Level]([Level])
VALUES ('A'), ('B'), ('C'),('D')
GO 

/*
--------------------------------------------------------------------------------------
[SU_Type]
--------------------------------------------------------------------------------------
*/

DECLARE  @IdTran INT

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Usine', NULL,12)
SELECT @IdTran = SCOPE_IDENTITY()
INSERT INTO [dbo].[SU_Type] ([Type], TypeTransId) VALUES('Factory', @IdTran)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N' Préstataire - agent', NULL,12)
SELECT @IdTran = SCOPE_IDENTITY()
INSERT INTO [dbo].[SU_Type] ([Type], TypeTransId) VALUES('Supplier', @IdTran)

--INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Préstataire - Traideur', NULL,12)
--SELECT @IdTran = SCOPE_IDENTITY()
--INSERT INTO [dbo].[SU_Type] ([Type], TypeTransId,EntityId) VALUES('Supplier - Trading', @IdTran,1)

--INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N' Préstataire - Import / Export', NULL,12)
--SELECT @IdTran = SCOPE_IDENTITY()
--INSERT INTO [dbo].[SU_Type] ([Type], TypeTransId,EntityId) VALUES('Supplier - Import / Export', @IdTran,1)

--INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Préstataire / Vente Local', NULL,12)
--SELECT @IdTran = SCOPE_IDENTITY()
--INSERT INTO [dbo].[SU_Type] ([Type], TypeTransId,EntityId) VALUES('Supplier - Buying House', @IdTran,1)

GO

/*
--------------------------------------------------------------------------------------
[SU_OwnlerShip]
--------------------------------------------------------------------------------------
*/

DECLARE  @IdTran INT

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Rien', NULL,13)
SELECT @IdTran = SCOPE_IDENTITY()
INSERT INTO [dbo].[SU_OwnlerShip]([Name], Name_TranId,EntityId) Values ('None', @IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Majeur Partiel', NULL,13)
SELECT @IdTran = SCOPE_IDENTITY()
INSERT INTO [dbo].[SU_OwnlerShip]([Name], Name_TranId,EntityId) Values ('Partial Major', @IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Mineur partiel', NULL,13)
SELECT @IdTran = SCOPE_IDENTITY()
INSERT INTO [dbo].[SU_OwnlerShip]([Name], Name_TranId,EntityId) Values ('Partial Minor', @IdTran,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Tout', NULL,13)
SELECT @IdTran = SCOPE_IDENTITY()
INSERT INTO [dbo].[SU_OwnlerShip]([Name], Name_TranId,EntityId) Values ('Full', @IdTran,1)
GO

/*
--------------------------------------------------------------------------------------
[SU_AddressType]
--------------------------------------------------------------------------------------
*/


DECLARE  @IdTran INT

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Tête Office', NULL,14)
SELECT @IdTran = SCOPE_IDENTITY()
INSERT INTO [dbo].[SU_AddressType]([Address_Type], [Address_Type_Flag],[TranslationId])
VALUES('Head Office', 'H', @IdTran)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Facturation', NULL,14)
SELECT @IdTran = SCOPE_IDENTITY()
INSERT INTO [dbo].[SU_AddressType]([Address_Type], [Address_Type_Flag],[TranslationId],EntityId)
VALUES('Accounting', 'B', @IdTran,1)


INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Plateforme', NULL,14)
SELECT @IdTran = SCOPE_IDENTITY()
INSERT INTO [dbo].[SU_AddressType]([Address_Type], [Address_Type_Flag],[TranslationId],EntityId)
VALUES('Regional Office', 'R', @IdTran,1)
GO

/*
--------------------------------------------------------------------------------------
[REF_Unit]
--------------------------------------------------------------------------------------
*/

     GO
SET IDENTITY_INSERT [dbo].[REF_Unit] ON 
GO
INSERT [dbo].[REF_Unit] ([Id], [Name], [Active], [EntityId]) VALUES (1, N'Set', 1, 1)
GO
INSERT [dbo].[REF_Unit] ([Id], [Name], [Active], [EntityId]) VALUES (2, N'Pairs', 1, 1)
GO
INSERT [dbo].[REF_Unit] ([Id], [Name], [Active], [EntityId]) VALUES (3, N'Yards', 1, 1)
GO
INSERT [dbo].[REF_Unit] ([Id], [Name], [Active], [EntityId]) VALUES (4, N'Pcs', 1, 1)
GO
SET IDENTITY_INSERT [dbo].[REF_Unit] OFF
GO



/*
--------------------------------------------------------------------------------------
[INSP_Status]
--------------------------------------------------------------------------------------
*/

	SET IDENTITY_INSERT [dbo].[INSP_Status] ON 
	GO

INSERT [dbo].[INSP_Status] ([Id],[Status], [Active],[Entity_Id], [Priority]) VALUES (1,N'Requested', 1,1, 1)
	GO
	INSERT [dbo].[INSP_Status] ([Id],[Status], [Active],[Entity_Id], [Priority]) VALUES (2, N'Confirmed', 1,1,5)
	GO
	INSERT [dbo].[INSP_Status] ([Id],[Status], [Active],[Entity_Id], [Priority]) VALUES (3, N'Postpone', 1,1,8)
	GO
	INSERT [dbo].[INSP_Status] ([Id],[Status], [Active],[Entity_Id], [Priority]) VALUES (4, N'Cancelled', 1,1, 2)
	GO
	INSERT [dbo].[INSP_Status] ([Id],[Status], [Active],[Entity_Id], [Priority]) VALUES (5, N'Scheduled', 1,1,10)
	GO
	INSERT [dbo].[INSP_Status] ([Id],[Status], [Active],[Entity_Id], [Priority]) VALUES (6, N'Inspected', 1,1,7)
	GO
	INSERT [dbo].[INSP_Status] ([Id],[Status], [Active],[Entity_Id], [Priority]) VALUES (7, N'Validated', 1,1,9)
	GO
	INSERT [dbo].[INSP_Status] ([Id],[Status], [Active],[Entity_Id], [Priority]) VALUES (8, N'Verified', 1,1,3)
	GO
	INSERT [dbo].[INSP_Status] ([Id],[Status], [Active],[Entity_Id], [Priority]) VALUES (9, N'AllocatedQC', 1,1,6)
	GO
	INSERT [dbo].[INSP_Status] ([Id],[Status], [Active],[Entity_Id], [Priority]) VALUES (10, N'OnHold', 1,1,4)
	GO
	SET IDENTITY_INSERT [dbo].[INSP_Status] OFF
	GO
/*
--------------------------------------------------------------------------------------
[REF_AQL_Pick_SampleSize_CodeValue]
--------------------------------------------------------------------------------------
*/

INSERT [dbo].[REF_AQL_Pick_SampleSize_CodeValue] ([Sample_Size_Code], [Sample_Size]) VALUES (N'A', 2)
GO
INSERT [dbo].[REF_AQL_Pick_SampleSize_CodeValue] ([Sample_Size_Code], [Sample_Size]) VALUES (N'B', 3)
GO
INSERT [dbo].[REF_AQL_Pick_SampleSize_CodeValue] ([Sample_Size_Code], [Sample_Size]) VALUES (N'C', 5)
GO
INSERT [dbo].[REF_AQL_Pick_SampleSize_CodeValue] ([Sample_Size_Code], [Sample_Size]) VALUES (N'D', 8)
GO
INSERT [dbo].[REF_AQL_Pick_SampleSize_CodeValue] ([Sample_Size_Code], [Sample_Size]) VALUES (N'E', 13)
GO
INSERT [dbo].[REF_AQL_Pick_SampleSize_CodeValue] ([Sample_Size_Code], [Sample_Size]) VALUES (N'F', 20)
GO
INSERT [dbo].[REF_AQL_Pick_SampleSize_CodeValue] ([Sample_Size_Code], [Sample_Size]) VALUES (N'G', 32)
GO
INSERT [dbo].[REF_AQL_Pick_SampleSize_CodeValue] ([Sample_Size_Code], [Sample_Size]) VALUES (N'H', 50)
GO
INSERT [dbo].[REF_AQL_Pick_SampleSize_CodeValue] ([Sample_Size_Code], [Sample_Size]) VALUES (N'J', 80)
GO
INSERT [dbo].[REF_AQL_Pick_SampleSize_CodeValue] ([Sample_Size_Code], [Sample_Size]) VALUES (N'K', 125)
GO
INSERT [dbo].[REF_AQL_Pick_SampleSize_CodeValue] ([Sample_Size_Code], [Sample_Size]) VALUES (N'L', 200)
GO
INSERT [dbo].[REF_AQL_Pick_SampleSize_CodeValue] ([Sample_Size_Code], [Sample_Size]) VALUES (N'M', 315)
GO
INSERT [dbo].[REF_AQL_Pick_SampleSize_CodeValue] ([Sample_Size_Code], [Sample_Size]) VALUES (N'N', 500)
GO
INSERT [dbo].[REF_AQL_Pick_SampleSize_CodeValue] ([Sample_Size_Code], [Sample_Size]) VALUES (N'P', 800)
GO
INSERT [dbo].[REF_AQL_Pick_SampleSize_CodeValue] ([Sample_Size_Code], [Sample_Size]) VALUES (N'Q', 1250)
GO
INSERT [dbo].[REF_AQL_Pick_SampleSize_CodeValue] ([Sample_Size_Code], [Sample_Size]) VALUES (N'R', 2000)
GO

/*
--------------------------------------------------------------------------------------
[REF_AQL_Sample_Code]
--------------------------------------------------------------------------------------
*/

SET IDENTITY_INSERT [dbo].[REF_AQL_Sample_Code] ON 
GO
INSERT [dbo].[REF_AQL_Sample_Code] ([Sample_Size_range_Code_Id], [Min_size], [Max_size], [Level_I_Sample_Size_Code], [Level_II_Sample_Size_Code], [Level_III_Sample_Size_Code],
[LEVEL_S1_SAMPLE_SIZE_CODE], [LEVEL_S2_SAMPLE_SIZE_CODE], [LEVEL_S3_SAMPLE_SIZE_CODE], [LEVEL_S4_SAMPLE_SIZE_CODE]) VALUES (1, 2, 8, N'A', N'A', N'B', N'A', N'A', N'A', N'A')
GO
INSERT [dbo].[REF_AQL_Sample_Code] ([Sample_Size_range_Code_Id], [Min_size], [Max_size], [Level_I_Sample_Size_Code], [Level_II_Sample_Size_Code], [Level_III_Sample_Size_Code], 
[LEVEL_S1_SAMPLE_SIZE_CODE], [LEVEL_S2_SAMPLE_SIZE_CODE], [LEVEL_S3_SAMPLE_SIZE_CODE], [LEVEL_S4_SAMPLE_SIZE_CODE]) VALUES (2, 9, 15, N'A', N'B', N'C', N'A', N'A', N'A', N'A')
GO
INSERT [dbo].[REF_AQL_Sample_Code] ([Sample_Size_range_Code_Id], [Min_size], [Max_size], [Level_I_Sample_Size_Code], [Level_II_Sample_Size_Code], [Level_III_Sample_Size_Code], 
[LEVEL_S1_SAMPLE_SIZE_CODE], [LEVEL_S2_SAMPLE_SIZE_CODE], [LEVEL_S3_SAMPLE_SIZE_CODE], [LEVEL_S4_SAMPLE_SIZE_CODE]) VALUES (3, 16, 25, N'B', N'C', N'D', N'A', N'A', N'B', N'B')
GO
INSERT [dbo].[REF_AQL_Sample_Code] ([Sample_Size_range_Code_Id], [Min_size], [Max_size], [Level_I_Sample_Size_Code], [Level_II_Sample_Size_Code], [Level_III_Sample_Size_Code],
[LEVEL_S1_SAMPLE_SIZE_CODE], [LEVEL_S2_SAMPLE_SIZE_CODE], [LEVEL_S3_SAMPLE_SIZE_CODE], [LEVEL_S4_SAMPLE_SIZE_CODE]) VALUES (4, 26, 50, N'C', N'D', N'E', N'A', N'B', N'B', N'C')
GO
INSERT [dbo].[REF_AQL_Sample_Code] ([Sample_Size_range_Code_Id], [Min_size], [Max_size], [Level_I_Sample_Size_Code], [Level_II_Sample_Size_Code], [Level_III_Sample_Size_Code],
[LEVEL_S1_SAMPLE_SIZE_CODE], [LEVEL_S2_SAMPLE_SIZE_CODE], [LEVEL_S3_SAMPLE_SIZE_CODE], [LEVEL_S4_SAMPLE_SIZE_CODE]) VALUES (5, 51, 90, N'C', N'E', N'F', N'B', N'B', N'C', N'C')
GO
INSERT [dbo].[REF_AQL_Sample_Code] ([Sample_Size_range_Code_Id], [Min_size], [Max_size], [Level_I_Sample_Size_Code], [Level_II_Sample_Size_Code], [Level_III_Sample_Size_Code],
[LEVEL_S1_SAMPLE_SIZE_CODE], [LEVEL_S2_SAMPLE_SIZE_CODE], [LEVEL_S3_SAMPLE_SIZE_CODE], [LEVEL_S4_SAMPLE_SIZE_CODE]) VALUES (6, 91, 150, N'D', N'F', N'G', N'B', N'B', N'C', N'D')
GO
INSERT [dbo].[REF_AQL_Sample_Code] ([Sample_Size_range_Code_Id], [Min_size], [Max_size], [Level_I_Sample_Size_Code], [Level_II_Sample_Size_Code], [Level_III_Sample_Size_Code], 
[LEVEL_S1_SAMPLE_SIZE_CODE], [LEVEL_S2_SAMPLE_SIZE_CODE], [LEVEL_S3_SAMPLE_SIZE_CODE], [LEVEL_S4_SAMPLE_SIZE_CODE]) VALUES (7, 151, 280, N'E', N'G', N'H', N'B', N'C', N'D', N'E')
GO
INSERT [dbo].[REF_AQL_Sample_Code] ([Sample_Size_range_Code_Id], [Min_size], [Max_size], [Level_I_Sample_Size_Code], [Level_II_Sample_Size_Code], [Level_III_Sample_Size_Code], 
[LEVEL_S1_SAMPLE_SIZE_CODE], [LEVEL_S2_SAMPLE_SIZE_CODE], [LEVEL_S3_SAMPLE_SIZE_CODE], [LEVEL_S4_SAMPLE_SIZE_CODE]) VALUES (8, 281, 500, N'F', N'H', N'J', N'B', N'C', N'D', N'E')
GO
INSERT [dbo].[REF_AQL_Sample_Code] ([Sample_Size_range_Code_Id], [Min_size], [Max_size], [Level_I_Sample_Size_Code], [Level_II_Sample_Size_Code], [Level_III_Sample_Size_Code],
[LEVEL_S1_SAMPLE_SIZE_CODE], [LEVEL_S2_SAMPLE_SIZE_CODE], [LEVEL_S3_SAMPLE_SIZE_CODE], [LEVEL_S4_SAMPLE_SIZE_CODE]) VALUES (9, 501, 1200, N'G', N'J', N'K', N'C', N'C', N'E', N'F')
GO
INSERT [dbo].[REF_AQL_Sample_Code] ([Sample_Size_range_Code_Id], [Min_size], [Max_size], [Level_I_Sample_Size_Code], [Level_II_Sample_Size_Code], [Level_III_Sample_Size_Code],
[LEVEL_S1_SAMPLE_SIZE_CODE], [LEVEL_S2_SAMPLE_SIZE_CODE], [LEVEL_S3_SAMPLE_SIZE_CODE], [LEVEL_S4_SAMPLE_SIZE_CODE]) VALUES (10, 1201, 3200, N'H', N'K', N'L', N'C', N'D', N'E', N'G')
GO
INSERT [dbo].[REF_AQL_Sample_Code] ([Sample_Size_range_Code_Id], [Min_size], [Max_size], [Level_I_Sample_Size_Code], [Level_II_Sample_Size_Code], [Level_III_Sample_Size_Code],
[LEVEL_S1_SAMPLE_SIZE_CODE], [LEVEL_S2_SAMPLE_SIZE_CODE], [LEVEL_S3_SAMPLE_SIZE_CODE], [LEVEL_S4_SAMPLE_SIZE_CODE]) VALUES (11, 3201, 10000, N'J', N'L', N'M', N'C', N'D', N'F', N'G')
GO
INSERT [dbo].[REF_AQL_Sample_Code] ([Sample_Size_range_Code_Id], [Min_size], [Max_size], [Level_I_Sample_Size_Code], [Level_II_Sample_Size_Code], [Level_III_Sample_Size_Code],
[LEVEL_S1_SAMPLE_SIZE_CODE], [LEVEL_S2_SAMPLE_SIZE_CODE], [LEVEL_S3_SAMPLE_SIZE_CODE], [LEVEL_S4_SAMPLE_SIZE_CODE]) VALUES (12, 10001, 35000, N'K', N'M', N'N', N'C', N'D', N'F', N'H')
GO
INSERT [dbo].[REF_AQL_Sample_Code] ([Sample_Size_range_Code_Id], [Min_size], [Max_size], [Level_I_Sample_Size_Code], [Level_II_Sample_Size_Code], [Level_III_Sample_Size_Code], 
[LEVEL_S1_SAMPLE_SIZE_CODE], [LEVEL_S2_SAMPLE_SIZE_CODE], [LEVEL_S3_SAMPLE_SIZE_CODE], [LEVEL_S4_SAMPLE_SIZE_CODE]) VALUES (13, 35001, 150000, N'L', N'N', N'P', N'D', N'E', N'G', N'J')
GO
INSERT [dbo].[REF_AQL_Sample_Code] ([Sample_Size_range_Code_Id], [Min_size], [Max_size], [Level_I_Sample_Size_Code], [Level_II_Sample_Size_Code], [Level_III_Sample_Size_Code],
[LEVEL_S1_SAMPLE_SIZE_CODE], [LEVEL_S2_SAMPLE_SIZE_CODE], [LEVEL_S3_SAMPLE_SIZE_CODE], [LEVEL_S4_SAMPLE_SIZE_CODE]) VALUES (14, 150001, 500000, N'M', N'P', N'Q', N'D', N'E', N'G', N'J')
GO
INSERT [dbo].[REF_AQL_Sample_Code] ([Sample_Size_range_Code_Id], [Min_size], [Max_size], [Level_I_Sample_Size_Code], [Level_II_Sample_Size_Code], [Level_III_Sample_Size_Code],
[LEVEL_S1_SAMPLE_SIZE_CODE], [LEVEL_S2_SAMPLE_SIZE_CODE], [LEVEL_S3_SAMPLE_SIZE_CODE], [LEVEL_S4_SAMPLE_SIZE_CODE]) VALUES (15, 500001, 999999999, N'N', N'Q', N'R', N'D', N'E', N'H', N'K')
GO
SET IDENTITY_INSERT [dbo].[REF_AQL_Sample_Code] OFF
GO
/*
--------------------------------------------------------------------------------------
[INSP_Cancel_Reasons]
--------------------------------------------------------------------------------------
*/
INSERT [dbo].[INSP_Cancel_Reasons] (Reason,IsDefault,IsAPI,Customer_Id,Active,EntityId) VALUES(N'Goods Not Ready',1,0,null,1,1)
GO
INSERT [dbo].[INSP_Cancel_Reasons] (Reason,IsDefault,IsAPI,Customer_Id,Active,EntityId) VALUES('Production fault',1,1,null,1,1)
GO
INSERT [dbo].[INSP_Cancel_Reasons] (Reason,IsDefault,IsAPI,Customer_Id,Active,EntityId) VALUES('QC Not Available',0,1,null,1,1)
GO
--After customer insert you have to execute the below insert query
--INSERT INTO [dbo].[INSP_Cancel_Reasons] ([Reason],[IsDefault],[IsAPI],[Customer_Id],[Active],[EntityId]) VALUES  ('Some Delay',1,1,1,1,1)
GO
/*
--------------------------------------------------------------------------------------
[INSP_Reschedule_Reasons]
--------------------------------------------------------------------------------------
*/
INSERT [dbo].[INSP_Reschedule_Reasons] (Reason,IsDefault,IsAPI,Customer_Id,Active,EntityId) VALUES(N'Goods Not Ready',1,0,null,1,1)
GO
INSERT [dbo].[INSP_Reschedule_Reasons] (Reason,IsDefault,IsAPI,Customer_Id,Active,EntityId) VALUES('Production fault',1,1,null,1,1)
GO
INSERT [dbo].[INSP_Reschedule_Reasons] (Reason,IsDefault,IsAPI,Customer_Id,Active,EntityId) VALUES('QC Not Available',0,1,null,1,1)
GO
--After customer insert you have to execute the below insert query
--INSERT [dbo].[INSP_Reschedule_Reasons] (Reason,IsDefault,IsAPI,Customer_Id,Active,EntityId) VALUES('Delay in process',1,1,1,1,1)
GO


INSERT INTO [dbo].[CU_CheckPointType]
           ([Name]
           ,[Active]
           ,[Entity_Id])
     VALUES
           ('Quotation required',1,1),
		   ('quotation approve by manager',1,1),
		   ('PO Quantity Modification Allowed',1,1),
		   ('IC Required',1,1),
		   ('Customer decision required',1,1)
GO

/*
--------------------------------------------------------------------------------------
QU_BillMethod
--------------------------------------------------------------------------------------
*/
INSERT INTO QU_BillMethod(Id, Label)
	VALUES(1, 'Man day')
		 ,(2, 'Sampling')
GO

/*
--------------------------------------------------------------------------------------
QU_PaidBy
--------------------------------------------------------------------------------------
*/
INSERT INTO QU_PaidBy(Id, Label)
	VALUES(1, 'Customer')
		 ,(2, 'Supplier')
		 ,(3, 'Factory')


/*
--------------------------------------------------------------------------------------
INSP_LAB_AddressType
--------------------------------------------------------------------------------------
*/
GO
INSERT [dbo].[INSP_LAB_AddressType] ([Address_type],[TranslationId],[EntityId]) VALUES (N'Head Office', 1, 1),  (N'Regional  Office', 1, 1)

/*
--------------------------------------------------------------------------------------
INSP_LAB_Type
--------------------------------------------------------------------------------------
*/
GO
INSERT [dbo].[INSP_LAB_Type] ([Type],[TypeTransId],[EntityId]) VALUES (N'Internal', 1, 1),  (N'External', 1, 1)

----------------------Email Type and Module---------------

SET IDENTITY_INSERT [dbo].[MID_Email_Modules] ON 
GO
INSERT [dbo].[MID_Email_Modules] ([Id], [Name], [Active], [EntityId], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [DeletedBy], [DeletedOn]) VALUES (1, N'Inspection Booking', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[MID_Email_Modules] ([Id], [Name], [Active], [EntityId], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [DeletedBy], [DeletedOn]) VALUES (2, N'Audit Booking', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[MID_Email_Modules] ([Id], [Name], [Active], [EntityId], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [DeletedBy], [DeletedOn]) VALUES (3, N'Quotation', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[MID_Email_Modules] OFF
GO
SET IDENTITY_INSERT [dbo].[MID_EmailTypes] ON 
GO
INSERT [dbo].[MID_EmailTypes] ([Id], [Name], [ModuleId], [Active], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [DeletedBy], [DeletedOn]) VALUES (1, N'Inspection Booking Request', 1, 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[MID_EmailTypes] ([Id], [Name], [ModuleId], [Active], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [DeletedBy], [DeletedOn]) VALUES (2, N'Inspection Booking Confirm', 1, 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[MID_EmailTypes] ([Id], [Name], [ModuleId], [Active], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [DeletedBy], [DeletedOn]) VALUES (3, N'Inspection Booking Cancel', 1, 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[MID_EmailTypes] ([Id], [Name], [ModuleId], [Active], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [DeletedBy], [DeletedOn]) VALUES (4, N'Inspection Booking Reschedule', 1, 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[MID_EmailTypes] ([Id], [Name], [ModuleId], [Active], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [DeletedBy], [DeletedOn]) VALUES (5, N'Inspection Booking Split', 1, 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[MID_EmailTypes] ([Id], [Name], [ModuleId], [Active], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [DeletedBy], [DeletedOn]) VALUES (6, N'Audit Booking Request', 2, 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[MID_EmailTypes] ([Id], [Name], [ModuleId], [Active], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [DeletedBy], [DeletedOn]) VALUES (7, N'Audit Booking Confimed', 2, 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[MID_EmailTypes] ([Id], [Name], [ModuleId], [Active], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [DeletedBy], [DeletedOn]) VALUES (8, N'Audit Booking Cancel', 2, 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[MID_EmailTypes] ([Id], [Name], [ModuleId], [Active], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [DeletedBy], [DeletedOn]) VALUES (9, N'Audit Booking Reschedule', 2, 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[MID_EmailTypes] ([Id], [Name], [ModuleId], [Active], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [DeletedBy], [DeletedOn]) VALUES (10, N'Quotation Request', 3, 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[MID_EmailTypes] ([Id], [Name], [ModuleId], [Active], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [DeletedBy], [DeletedOn]) VALUES (11, N'Quotation Approved', 3, 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[MID_EmailTypes] ([Id], [Name], [ModuleId], [Active], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [DeletedBy], [DeletedOn]) VALUES (12, N'Quotation Sent To Customer', 3, 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[MID_EmailTypes] ([Id], [Name], [ModuleId], [Active], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [DeletedBy], [DeletedOn]) VALUES (13, N'Quotation Confimed By Customer', 3, 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[MID_EmailTypes] ([Id], [Name], [ModuleId], [Active], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [DeletedBy], [DeletedOn]) VALUES (14, N'Quotation Reject By Customer', 3, 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[MID_EmailTypes] ([Id], [Name], [ModuleId], [Active], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [DeletedBy], [DeletedOn]) VALUES (15, N'Quotation Reject By Manager', 3, 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[MID_EmailTypes] ([Id], [Name], [ModuleId], [Active], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [DeletedBy], [DeletedOn]) VALUES (16, N'Quotation Cancel', 3, 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[MID_EmailTypes] OFF
GO


---------------------Email Type and  Module---------------
/*
--------------------------------------------------------------------------------------
QU_Status
--------------------------------------------------------------------------------------
*/

DECLARE @TransId INT 


INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Vérifié', NULL,22)
SELECT @TransId = SCOPE_IDENTITY()
INSERT INTO [dbo].QU_Status(Id,Label, [TranId],[EntityId]) VALUES (1, 'Quotation Requested', @TransId,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Validé', NULL,22)
SELECT @TransId = SCOPE_IDENTITY()
INSERT INTO [dbo].QU_Status(Id,Label, [TranId],[EntityId]) VALUES (2, 'Manager Approved', @TransId,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Rejeté', NULL,22)
SELECT @TransId = SCOPE_IDENTITY()
INSERT INTO [dbo].QU_Status(Id,Label, [TranId],[EntityId]) VALUES (3, 'Manager Rejected', @TransId,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Confirmé ', NULL,22)
SELECT @TransId = SCOPE_IDENTITY()
INSERT INTO [dbo].QU_Status(Id,Label, [TranId],[EntityId]) VALUES (4, 'Quotation Verified', @TransId,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Annulé', NULL,22)
SELECT @TransId = SCOPE_IDENTITY()
insert into QU_Status(Id,Label, [TranId],[EntityId]) VALUES (5, 'Canceled', @TransId,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Rejet client', NULL,22)
SELECT @TransId = SCOPE_IDENTITY()
insert into QU_Status(Id,Label, [TranId],[EntityId]) VALUES (6, 'Customer Rejected', @TransId,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Confirmation client', NULL,22)
SELECT @TransId = SCOPE_IDENTITY()
insert into QU_Status(Id,Label, [TranId],[EntityId]) VALUES (7, 'Customer Validated', @TransId,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Envoyé', NULL,22)
SELECT @TransId = SCOPE_IDENTITY()
insert into QU_Status(Id,Label, [TranId],[EntityId]) VALUES (8, 'Sent To Client', @TransId,1)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Rejeté', NULL,22)
SELECT @TransId = SCOPE_IDENTITY()
INSERT INTO [dbo].QU_Status(Id,Label, [TranId],[EntityId]) VALUES (9, 'AE Rejected', @TransId,1)

GO 

/*
--------------------------------------------------------------------------------------
CU_CheckPoints
--------------------------------------------------------------------------------------
*/
----After customer insert you have to execute the below insert query
--insert into CU_CheckPoints(CheckpointTypeId, ServiceId, Active,CustomerId) values (1,1,1,1),(1,2,1,1)

---------------------Email Type and  Module---------------


---------------[REF_ReInspectionType] Data Starts---------------

SET IDENTITY_INSERT [dbo].[REF_ReInspectionType] ON 
GO
 
INSERT [dbo].[REF_ReInspectionType] ([Id],[Name],[Active]) Values (1,N'ReInspection-1',1)
INSERT [dbo].[REF_ReInspectionType] ([Id],[Name],[Active]) Values (2,N'ReInspection-2',1)
INSERT [dbo].[REF_ReInspectionType] ([Id],[Name],[Active]) Values (3,N'ReInspection-3',1)

SET IDENTITY_INSERT [dbo].[REF_ReInspectionType] OFF
GO

---------------[REF_ReInspectionType] Data Ends---------------

/*
------------------------------------------------------------------------------------
   [dbo].[IT_Role]
------------------------------------------------------------------------------------
*/	

/*
------------------------------------------------------------------------------------
   [dbo].[MID_NotificationType]
------------------------------------------------------------------------------------



------------------------------------------------------------------------------------
   [dbo].[SU_CreditTerm]
------------------------------------------------------------------------------------
*/	
INSERT INTO SU_CreditTerm([Name],Active,CreatedBy,CreatedOn)
     VALUES
           ('Invoice after 30 days', 1, 2, GETDATE()),
		   ('Invoice after 15 days', 1, 2, GETDATE())
GO
--------------- [SU_CreditTerm] Data Ends---------------

/*
------------------------------------------------------------------------------------
   [dbo].[SU_Status]
------------------------------------------------------------------------------------
*/	
INSERT INTO SU_Status([Name],Active,CreatedBy,CreatedOn)
     VALUES
           ('Confirmed', 1, 2, GETDATE()),
		   ('Cancelled', 1, 2, GETDATE()),
		   ('Suspended', 1, 2, GETDATE())
GO
--------------- [SU_Status] Data Ends---------------

/*
------------------------------------------------------------------------------------
   [dbo].[SCH_QCType]
------------------------------------------------------------------------------------
*/
SET IDENTITY_INSERT [dbo].[SCH_QCType] On
Go
INSERT INTO [dbo].[SCH_QCType] (Id, [Type], CreatedBy, CreatedOn) 
	VALUES 
		(1, 'QC', 2, GETDATE()),
		(2, 'Additional_QC', 2, GETDATE())
Go
SET IDENTITY_INSERT [dbo].[SCH_QCType] OFF
Go
--------------- [SCH_QCType] Data Ends---------------


------------SplitPreviousBookingNo Added-------------------
--ALTER TABLE INSP_Transaction ADD SplitPreviousBookingNo INT
------------SplitPreviousBookingNo Added-------------------

INSERT INTO REF_AQL_Pick_SampleSize_Acce_Code(Id,Sample_Size_Code,PickValue,Acc_sample_Size_Code,Accepted,Rejected)VALUES
  (1,'K',0.065,'L',0,1),
  (2,'M',0.065,'L',0,1),
  (3,'N',0.065,'P',1,2),
  (4,'J',0.10,'K',0,1),
  (5,'L',0.10,'K',0,1),
  (6,'M',0.10,'N',1,2),
  (7,'H',0.15,'J',0,1),
  (8,'K',0.15,'J',0,1),
  (9,'L',0.15,'M',0,1),
  (10,'G',0.25,'H',0,1),
  (11,'J',0.25,'H',0,1),
  (12,'K',0.25,'L',1,2),
  (13,'F',0.40,'G',0,1),
  (14,'H',0.40,'G',0,1),
  (15,'J',0.40,'K',1,2),
  (16,'E',0.65,'F',0,1),
  (17,'G',0.65,'F',0,1),
  (18,'H',0.65,'J',1,2),
  (19,'D',1.0,'E',0,1),
  (20,'F',1.0,'E',0,1),
  (21,'G',1.0,'H',1,2),
  (22,'R',1.0,'Q',21,22),
  (23,'C',1.5,'D',0,1),
  (24,'E',1.5,'D',0,1),
  (25,'F',1.5,'G',1,2),
  (26,'Q',1.5,'P',21,22),
  (27,'B',2.5,'C',0,1),
  (28,'D',2.5,'C',0,1),
  (29,'E',2.5,'F',1,2),
  (30,'P',2.5,'N',21,22),
  (31,'A',4.0,'B',0,1),
  (32,'C',4.0,'B',0,1),
  (33,'D',4.0,'E',1,2),
  (34,'N',4.0,'M',21,22),
  (35,'B',6.5,'A',0,1),
  (36,'C',6.5,'D',1,2),
  (37,'M',6.5,'L',21,22)


  --ALTER TABLE CU_Customer ADD [BookingDefaultComments] NVARCHAR(3000) NULL


  -- Fullbridge script updates start ---

  update IT_Right set menuname='ReportSummary',path='reportsummary/report-summary' where id=15

  update IT_Right set TitleName='FullBridge', menuname='FullBridgeSummary',path='fullbridge/fullbridge-summary' where id=16

  -- FB Master data updates 

  update REF_Service set Fb_Service_Id=32 where id=1
  update REF_Service set Fb_Service_Id=33 where id=2

  update REF_ServiceType set Fb_ServiceType_Id=151 where id=1
  update REF_ServiceType set Fb_ServiceType_Id=152 where id=2
  update REF_ServiceType set Fb_ServiceType_Id=153 where id=3
  update REF_ServiceType set Fb_ServiceType_Id=154 where id=4
  update REF_ServiceType set Fb_ServiceType_Id=155 where id=5
  update REF_ServiceType set Fb_ServiceType_Id=156 where id=6
  update REF_ServiceType set Fb_ServiceType_Id=157 where id=7
  update REF_ServiceType set Fb_ServiceType_Id=158 where id=8

  update REF_ProductCategory set Fb_ProductCategory_Id=3606 where id=1
  update REF_ProductCategory set Fb_ProductCategory_Id=3607 where id=2

  update REF_ProductCategory set Fb_ProductCategory_Id=3608 where id=3
  update REF_ProductCategory set Fb_ProductCategory_Id=3609 where id=4

  update REF_ProductCategory set Fb_ProductCategory_Id=3610 where id=5
  update REF_ProductCategory set Fb_ProductCategory_Id=3611 where id=6

  update REF_ProductCategory set Fb_ProductCategory_Id=3612 where id=7
  update REF_ProductCategory set Fb_ProductCategory_Id=3613 where id=8

  update REF_ProductCategory set Fb_ProductCategory_Id=3614 where id=9
  update REF_ProductCategory set Fb_ProductCategory_Id=3615 where id=10

  update REF_ProductCategory set Fb_ProductCategory_Id=3616 where id=11
  update REF_ProductCategory set Fb_ProductCategory_Id=3617 where id=12
  update REF_ProductCategory set Fb_ProductCategory_Id=3618 where id=13


  -- Fullbridge script updates end ---
  --ALTER TABLE CU_Customer ADD [BookingDefaultComments] NVARCHAR(3000) NULL

-------Update status name from Postpone to Reschedule ------

update INSP_Status set [Status] = 'Rescheduled' where [Status] = 'Postpone'

update AUD_Status set [Status] = 'Rescheduled' where [Status] = 'Postpone'

-------Update status name from Postpone to Reschedule ------

------------County, Town Insert Start----------------------

insert into REF_County(City_Id,County_Name, Active, Created_By, Created_On)
	Values 
(105, 'Huaining' ,1,2,GETDATE()),
(105, 'Zongyang' ,1,2,GETDATE()),
(105, 'Qianshan' ,1,2,GETDATE())

insert into REF_Town(CountyId,TownName, Active, CreatedBy, CreatedOn)
	Values 
(1, 'Leibu' ,1,2,GETDATE()),
(2, 'Zongyang' ,1,2,GETDATE()),
(3, 'Tangjiafang' ,1,2,GETDATE())
------------county, Town Insert End -----------------------

 -- Full bridge status start --

  INSERT INTO [dbo].[FB_Status_Type] (Name,Active)VALUES('Mission',1)
  INSERT INTO [dbo].[FB_Status_Type] (Name,Active)VALUES('ReportPreparation',1)
  INSERT INTO [dbo].[FB_Status_Type] (Name,Active)VALUES('ReportFilling',1)
  INSERT INTO [dbo].[FB_Status_Type] (Name,Active)VALUES('ReportReview',1)
  INSERT INTO [dbo].[FB_Status_Type] (Name,Active)VALUES('Report',1)

  -- Mission status 
  INSERT INTO [dbo].[FB_Status] (Type,StatusName,[FBStatusName],Active) VALUES(1,'Draft','Draft',1)
  INSERT INTO [dbo].[FB_Status] (Type,StatusName,[FBStatusName],Active) VALUES(1,'Confirmed','Confirmed',1)
  INSERT INTO [dbo].[FB_Status] (Type,StatusName,[FBStatusName],Active) VALUES(1,'Completed','Completed',1)

  -- Report Preparation status

  INSERT INTO [dbo].[FB_Status] (Type,StatusName,[FBStatusName],Active) VALUES(2,'Not Started','Not Started',1)
  INSERT INTO [dbo].[FB_Status] (Type,StatusName,[FBStatusName],Active) VALUES(2,'In Progress','InProgress',1)
  INSERT INTO [dbo].[FB_Status] (Type,StatusName,[FBStatusName],Active) VALUES(2,'Validated','Validated',1)


    -- Report Filling status 
  INSERT INTO [dbo].[FB_Status] (Type,StatusName,[FBStatusName],Active) VALUES(3,'Not Started','Not Started',1)
  INSERT INTO [dbo].[FB_Status] (Type,StatusName,[FBStatusName],Active) VALUES(3,'In Progress','InProgress',1)
  INSERT INTO [dbo].[FB_Status] (Type,StatusName,[FBStatusName],Active) VALUES(3,'Validated','Validated',1)


      -- Report Review status 
  INSERT INTO [dbo].[FB_Status] (Type,StatusName,[FBStatusName],Active) VALUES(4,'Not Started','Not Started',1)
  INSERT INTO [dbo].[FB_Status] (Type,StatusName,[FBStatusName],Active) VALUES(4,'In Progress','InProgress',1)
  INSERT INTO [dbo].[FB_Status] (Type,StatusName,[FBStatusName],Active) VALUES(4,'Validated','Validated',1)



   -- Report status 
  INSERT INTO [dbo].[FB_Status] (Type,StatusName,[FBStatusName],Active) VALUES(5,'Draft','Draft',1)
  INSERT INTO [dbo].[FB_Status] (Type,StatusName,[FBStatusName],Active) VALUES(5,'Archive','Archive',1)
  INSERT INTO [dbo].[FB_Status] (Type,StatusName,[FBStatusName],Active) VALUES(5,'Validated','Validated',1)
  INSERT INTO [dbo].[FB_Status] (Type,StatusName,[FBStatusName],Active) VALUES(5,'InValidated','InValidated',1)

   -- Full bridge status end --

   
--FB Report details Added start ---

INSERT INTO FB_Report_InspSummary_Type(type,active)values('Main',1)
INSERT INTO FB_Report_InspSummary_Type(type,active)values('Sub',1)

--FB Report details Added end ---

-- QC Expense Claim Start --

Insert into [EC_ExpenseClaimtype](Name,Active) Values ('Audit',1)
Insert into [EC_ExpenseClaimtype](Name,Active) Values ('Inspection',1)
Insert into [EC_ExpenseClaimtype](Name,Active) Values ('Non Inspection',1)

-- QC Expense Claim End --

--Add Insert query  INSP_IC_Status starts--

INSERT INTO INSP_IC_Status VALUES('Created',1)
INSERT INTO INSP_IC_Status VALUES('Cancel',1)

--Add Insert query  INSP_IC_Status ends --

--Add Insert query  INSP_IC_Title Starts --

insert into INSP_IC_Title values('Inspection Certificate',1)
insert into INSP_IC_Title values('Green Light',1)
--Add Insert query  INSP_IC_Title ends --

------- insert billing entity ------------
INSERT INTO [dbo].[REF_Billing_Entity] ([Name], [Active])
VALUES ('Asia Pacific Inspection Ltd - HONG KONG', 1),
		('Guangzhou Ouyatai - CHINA', 1),
		('Asia Pacific Inspection Vietnam Company Ltd - VIETNAM', 1),
		('API Audit Limited - HONG KONG (Audit)', 1)

------- insert billing entity ------------

---[REF_INSP_CUS_decision] insert--
Insert into [dbo].[REF_INSP_CUS_decision] (Name,Active) values('Pass',1)
Insert into [dbo].[REF_INSP_CUS_decision] (Name,Active) values('Fail',1)
Insert into [dbo].[REF_INSP_CUS_decision] (Name,Active) values('Pending',1)
--[REF_INSP_CUS_decision] insert---

--[FB_Report_Result]  insert---
Insert into [dbo].[FB_Report_Result] (ResultName,Active) values('Pass',1)
Insert into [dbo].[FB_Report_Result] (ResultName,Active) values('Fail',1)
Insert into [dbo].[FB_Report_Result] (ResultName,Active) values('Pending',1)
Insert into [dbo].[FB_Report_Result] (ResultName,Active) values('notapplicable',1)
Insert into [dbo].[FB_Report_Result] (ResultName,Active) values('Missing',1)
---[FB_Report_Result] insert--


--INV_TM_Type insert--
insert into INV_TM_Type (name,Active) values('Standard A',1)
insert into INV_TM_Type (name,Active) values('Standard B',1)
insert into INV_TM_Type (name,Active) values('Customized',1)
--INV_TM_Type insert--


-- CU_PR_Details alter query starts--

Alter table CU_PR_Details add TravelMatrixTypeId int
Alter table CU_PR_Details Add CONSTRAINT CU_PR_Details_TravelMatrixTypeId FOREIGN KEY ([TravelMatrixTypeId]) REFERENCES [dbo].[INV_TM_Type](Id)

--CU_PR_Details alter query ends --
GO

--------------kpi template type master data start--------
INSERT INTO [dbo].[REF_KPI_Template_Type]
           ([Name],[Active])
     VALUES
           ('Statistics',1),
		   ('Invoice',1)

--------------kpi template type master data end--------
:r .\KPI.Post.sql