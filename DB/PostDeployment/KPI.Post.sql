

--drop view V_GetInspectionDetails

--drop view V_GetCustomerDetails

--drop view V_GetSupplierDetails

--drop view V_GetFactoryDetails

	--drop table KPI_TemplateSubModule

	--drop table KPI_TemplateColumn

	--drop table KPI_Template

	--drop table KPI_Column

	--drop table AP_SubModuleRole

	--drop table AP_SubModule

	--drop table AP_ModuleRole

	--drop table AP_Module

	--drop table REF_DataSourceType

	--drop table REF_SignEquality


	
	--CREATE TABLE [REF_DataSourceType] (
	--	[Id] INT PRIMARY KEY, 
	--	[Name] NVARCHAR(300) NOT NULL
	--)


	--CREATE TABLE [AP_Module] (
	--	[Id] INT IDENTITY(1,1) PRIMARY KEY, 
	--	[Name] NVARCHAR(300) NOT NULL, 
	--	[DataSourceName] VARCHAR(200) NOT NULL,
	--	[DataSourceTypeId] INT NOT NULL,   
	--	[Active] BIT NOT NULL,
	--	FOREIGN KEY ([DataSourceTypeId]) REFERENCES [REF_DataSourceType](Id)
	--)

	--CREATE TABLE AP_ModuleRole (
	--	IdModule INT NOT NULL, 
	--	IdRole INT NOT NULL,
	--	PRIMARY KEY (IdModule, IdRole),
	--	FOREIGN KEY ([IdModule]) REFERENCES AP_Module(Id),
	--	FOREIGN KEY ([IdRole]) REFERENCES it_role(Id)
	--)

 --  CREATE TABLE [AP_SubModule] (
	--	[Id] INT IDENTITY(1,1) PRIMARY KEY, 
	--	[Name] NVARCHAR(300) NOT NULL, 
	--	[IdModule] INT NOT NULL,
	--	[DataSourceName] VARCHAR(200) NOT NULL,
	--	[DataSourceTypeId] INT NOT NULL,   
	--	[Active] BIT NOT NULL,
	--	FOREIGN KEY ([IdModule]) REFERENCES AP_Module(Id),
	--	FOREIGN KEY ([DataSourceTypeId]) REFERENCES [REF_DataSourceType](Id)
	--)

 -- 	CREATE TABLE AP_SubModuleRole (
	--	IdSubModule INT NOT NULL, 
	--	IdRole INT NOT NULL,
	--	PRIMARY KEY (IdSubModule, IdRole),
	--	FOREIGN KEY ([IdSubModule]) REFERENCES AP_SubModule(Id),
	--	FOREIGN KEY ([IdRole]) REFERENCES it_role(Id)
	--)

 --   CREATE TABLE REF_SignEquality(
	--Id INT NOT NULL PRIMARY KEY,
	--[Label] VARCHAR(200) NOT NULL
 --  )

 --  	CREATE TABLE [KPI_Column] (
	--	[Id] INT IDENTITY(1,1) PRIMARY KEY, 
	--	[FieldLabel] NVARCHAR(300) NOT NULL,
	--	[FieldName] NVARCHAR(300) NOT NULL, 
	--	[FieldType] VARCHAR(20) NULL,
	--	[IdSubModule] INT NULL,
	--	[IdModule] INT NULL,
	--	[CanFilter] BIT NOT NULL,
	--	[CanShowInResult] BIT NOT NULL,
	--	[FilterIsMultiple] BIT NULL, 
	--	[FilterDataSourceName] VARCHAR(200) NULL,
	--	[FilterDataSourceTypeId] INT NULL, 
	--	[FilterDataSourceFieldValue] VARCHAR(100) NULL,
	--	[FilterDataSourceFieldName] VARCHAR(100) NULL,
	--	[FilterDataSourceFieldCondition] VARCHAR(100) NULL,
	--	[FilterDataSourceFieldConditionValue] VARCHAR(100) NULL,
	--	[FilterRequired] BIT NOT NULL, 
	--	[FilterSignEqualityId]  INT NULL,
	--	[Active] BIT NOT NULL,
	--	[IsLocationId] BIT NOT NULL, 
	--	[IsCustomerId] BIT NOT NULL,
	--	[IsKey] BIT NOT NULL,
	--	FOREIGN  KEY ([IdSubModule]) REFERENCES [AP_SubModule](Id),
	--	FOREIGN  KEY ([IdModule]) REFERENCES [AP_Module](Id),
	--	FOREIGN KEY ([FilterDataSourceTypeId]) REFERENCES [REF_DataSourceType](Id),
	--	FOREIGN KEY ([FilterSignEqualityId]) REFERENCES [REF_SignEquality](Id)
	--)

	--	CREATE TABLE KPI_Template(
	--	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	--	[Name] NVARCHAR(200) NOT NULL,
	--	[UserId] INT NOT NULL,
	--	[CreatedDate] DATETIME NOT NULL, 
	--	[UpdatedDate] DATETIME NULL,
	--	[IsShared] BIT NOT NULL,
	--	[UseXlsFormulas] BIT NOT NULL, 
	--	[IdModule] INT NOT NULL,
	--	FOREIGN  KEY ([UserId]) REFERENCES [IT_UserMaster](Id),
	--	FOREIGN  KEY ([IdModule]) REFERENCES [AP_Module](Id)
	--)

	--CREATE TABLE KPI_TemplateColumn (
	--	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	--	IdTemplate INT NOT NULL, 
	--	IdColumn INT NOT NULL,
	--	[ColumnName] NVARCHAR(200)  NULL,
	--	[SumFooter] BIT NULL,
	--	[Group] BIT NULL,
	--	[OrderColumn] INT NULL, 
	--	[OrderFilter] INT NULL, 
	--	[SelectMultiple] BIT NULL,
	--	[Required] BIT NOT NULL,
	--	FOREIGN  KEY (IdTemplate) REFERENCES [KPI_Template](Id),
	--	FOREIGN  KEY (IdColumn) REFERENCES [KPI_Column](Id)
	--)

	--CREATE TABLE KPI_TemplateSubModule(
	--	IdTemplate INT NOT NULL, 
	--	IdSubModule INT NOT NULL,
	--	PRIMARY KEY(IdTemplate, IdSubModule),
	--	FOREIGN  KEY ([IdSubModule]) REFERENCES [AP_SubModule](Id),
	--	FOREIGN  KEY ([IdTemplate]) REFERENCES [KPI_Template](Id)
	--)

GO
INSERT INTO [REF_DataSourceType]([Id], [Name]) VALUES (1, 'Table'),(2, 'View')
GO



INSERT INTO [AP_Module]([Name], [Active],[DataSourceTypeId], [DataSourceName]) 
VALUES ('Inspection',1, 2, 'V_GetInspectionDetails')

GO 

INSERT INTO AP_ModuleRole(IdModule, IdRole)
SELECT Id, 4 AS IdRole -- Id 4 is the dev role
FROM AP_Module
GO




INSERT INTO [AP_SubModule]([Name], [IdModule], [DataSourceName], [DataSourceTypeId], [Active])
SELECT 'Products'  As [Name],  [Id] As [IdModule], 'V_GetProductDetails' as [DataSourceName] ,  2 AS [DataSourceTypeId], 1 as [Active]
FROM [AP_Module] 
WHERE [Name] = 'Inspection'



INSERT INTO [AP_SubModule]([Name], [IdModule], [DataSourceName], [DataSourceTypeId], [Active])
SELECT 'Purchase Order'  As [Name],  [Id] As [IdModule], 'V_GetPurchaseOrderDetails' as [DataSourceName] ,  2 AS [DataSourceTypeId], 1 as [Active]
FROM [AP_Module] 
WHERE [Name] = 'Inspection'

INSERT INTO [AP_SubModule]([Name], [IdModule], [DataSourceName], [DataSourceTypeId], [Active])
SELECT 'Quotation'  As [Name],  [Id] As [IdModule], 'V_GetQuotationDetails' as [DataSourceName] ,  2 AS [DataSourceTypeId], 1 as [Active]
FROM [AP_Module] 
WHERE [Name] = 'Inspection'

INSERT INTO [AP_SubModule]([Name], [IdModule], [DataSourceName], [DataSourceTypeId], [Active])
SELECT 'Schedule QC'  As [Name],  [Id] As [IdModule], 'V_GetQCDetails' as [DataSourceName] ,  2 AS [DataSourceTypeId], 1 as [Active]
FROM [AP_Module] 
WHERE [Name] = 'Inspection'

GO

INSERT INTO AP_SubModuleRole(IdSubModule, IdRole)
SELECT Id, 4 AS IdRole -- Id 4 is the dev role
FROM AP_SubModule
GO

INSERT INTO REF_SignEquality(Id, Label)
	VALUES(1, 'Equal'),
		  (2, 'Greater'),
		  (3, 'Less'), 
		  (4, 'GreaterOrEqual'), 
		  (5, 'LessOrEqual')
GO


-- Inspection Fileds start here

GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId],
[IsCustomerId], [IsKey]) VALUES (N'Id', N'Id', N'INT', NULL, 1, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[KPI_Column] ( [FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId],
[IsCustomerId], [IsKey]) VALUES ( N'Customer', N'Customer_Name', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO

GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Customer Address', N'Customer_Address', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO

GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Customer Country', N'Customer_Country', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO

GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Customer City', N'Customer_City', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO

GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Brand Name', N'Brand_Name', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO

GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Department Name', N'Department_Name', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO

GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Merchandiser Name', N'Merchandiser_Name', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO

GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Buyer Name', N'Buyer_Name', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO



GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId],
[IsCustomerId], [IsKey]) VALUES ( N'Supplier', N'Supplier_Name', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO


GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Supplier Address', N'Supplier_Address', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO

GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Supplier Regional_Address', N'Supplier_Regional_Address', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO

GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Supplier Country', N'Supplier_Country', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO

GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Supplier Province', N'Supplier_Province', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO

GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Supplier City', N'Supplier_City', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO

GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Supplier County', N'Supplier_County', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO


INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Factory', N'Factory_Name', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO

GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Factory Address', N'Factory_Address', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO

GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Factory Regional Address', N'Factory_Regional_Address', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO

GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Factory Country', N'Factory_Country', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO

GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Factory Province', N'Factory_Province', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO

GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Factory City', N'Factory_City', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO

GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Factory County', N'Factory_County', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO

GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Factory Town', N'Factory_Town', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO


GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Service Type', N'ServiceType', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 1, 0)
GO

GO
INSERT [dbo].[KPI_Column] ( [FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId],
[IsCustomerId], [IsKey]) VALUES ( N'Office', N'Office', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 1, 0, 0)
GO
INSERT [dbo].[KPI_Column] ([FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId], 
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId],
[IsCustomerId], [IsKey]) VALUES ( N'Status', N'Status', N'VARCHAR', NULL, 1, 0, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, 1, 0, 0, 0)
GO


-- Inspection Fileds end here



-- Inspection Filter start here
GO
INSERT [dbo].[KPI_Column] ( [FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId],
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES (N'Office', N'Location_Id', N'INT', NULL, 1, 1, 0, 1, N'REF_LOCATION', 1, N'Id', N'Location_Name', NULL, NULL, 0, NULL, 1, 0, 0, 0)
GO

INSERT [dbo].[KPI_Column] ( [FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId],
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'Service From', N'ServiceDate_From', N'DATE', NULL, 1, 1, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 1, 4, 1, 0, 0, 0)
GO
INSERT [dbo].[KPI_Column] ( [FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId],
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId],
[IsCustomerId], [IsKey]) VALUES ( N'Service To', N'ServiceDate_To', N'DATE', NULL, 1, 1, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 1, 5, 1, 0, 0, 0)
GO


GO
INSERT [dbo].[KPI_Column] ( [FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId],
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES ( N'First Service From', N'FirstServiceDate_From', N'DATE', NULL, 1, 1, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, 4, 1, 0, 0, 0)
GO
INSERT [dbo].[KPI_Column] ( [FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId],
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId],
[IsCustomerId], [IsKey]) VALUES ( N'First Service To', N'FirstServiceDate_To', N'DATE', NULL, 1, 1, 1, 0, NULL, NULL, NULL, NULL, NULL, NULL, 0, 5, 1, 0, 0, 0)
GO


GO
INSERT [dbo].[KPI_Column] ( [FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId],
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES (N'Customer', N'Customer_Id', N'INT', NULL, 1, 1, 0, 1, N'V_GetCustomerDetails', 2, N'Id', N'Customer_Name', NULL, NULL, 0, NULL, 1, 0, 0, 0)
GO


GO
INSERT [dbo].[KPI_Column] ( [FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId],
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES (N'Supplier', N'Supplier_Id', N'INT', NULL, 1, 1, 0, 1, N'V_GetSupplierDetails', 2, N'Id', N'Supplier_Name', N'Customer_Id', NULL, 0, NULL, 1, 0, 1, 0)
GO

GO
INSERT [dbo].[KPI_Column] ( [FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId],
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES (N'Factory', N'Factory_Id', N'INT', NULL, 1, 1, 0, 1, N'V_GetFactoryDetails', 2, N'Id', N'Factory_Name',  N'Customer_Id', NULL, 0, NULL, 1, 0, 1, 0)
GO

GO
INSERT [dbo].[KPI_Column] ( [FieldLabel], [FieldName], [FieldType], [IdSubModule], [IdModule], [CanFilter], [CanShowInResult], [FilterIsMultiple], [FilterDataSourceName], [FilterDataSourceTypeId],
[FilterDataSourceFieldValue], [FilterDataSourceFieldName], [FilterDataSourceFieldCondition], [FilterDataSourceFieldConditionValue], [FilterRequired], [FilterSignEqualityId], [Active], [IsLocationId], 
[IsCustomerId], [IsKey]) VALUES (N'Status', N'Status_Id', N'INT', NULL, 1, 1, 0, 1, N'INSP_Status', 1, N'Id', N'Status', NULL, NULL, 0, NULL, 1, 0, 0, 0)
GO


-- Inspection Filter end here


-- Product start here 


INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Inspection_Id' AS [FieldLabel], 'Inspection_Id' AS [FieldName], 'INT' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 0 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 1 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Products'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Product Name' AS [FieldLabel], 'Product_Name' AS [FieldName], 'VARCHAR' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Products'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Product Description' AS [FieldLabel], 'Product_Description' AS [FieldName], 'VARCHAR' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Products'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Product Category' AS [FieldLabel], 'Product_Category' AS [FieldName], 'VARCHAR' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Products'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Product Sub_Category' AS [FieldLabel], 'Product_Sub_Category' AS [FieldName], 'VARCHAR' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Products'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Report Title' AS [FieldLabel], 'Report_Title' AS [FieldName], 'VARCHAR' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Products'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Report FromDate' AS [FieldLabel], 'Report_FromDate' AS [FieldName], 'VARCHAR' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Products'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Report ToDate' AS [FieldLabel], 'Report_ToDate' AS [FieldName], 'VARCHAR' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Products'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Report Result' AS [FieldLabel], 'Report_Result' AS [FieldName], 'VARCHAR' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Products'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Defect CriticalMax' AS [FieldLabel], 'Defect_Critical_Max' AS [FieldName], 'INT' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Products'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Defect MajorMax' AS [FieldLabel], 'Defect_Major_Max' AS [FieldName], 'INT' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Products'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Defect MinorMax' AS [FieldLabel], 'Defect_Minor_Max' AS [FieldName], 'INT' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Products'

-- Product end  here 


-- po start here

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Inspection_Id' AS [FieldLabel], 'Inspection_Id' AS [FieldName], 'INT' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 0 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 1 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Purchase Order'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Po Name' AS [FieldLabel], 'PoName' AS [FieldName], 'VARCHAR' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Purchase Order'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Po Quantity' AS [FieldLabel], 'PoQuantity' AS [FieldName], 'INT' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Purchase Order'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Destination Country' AS [FieldLabel], 'Destination_Country' AS [FieldName], 'VARCHAR' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Purchase Order'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'ETD' AS [FieldLabel], 'ETD' AS [FieldName], 'DATE' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Purchase Order'

-- po end here


-- Quotation Fields start here

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'IdBooking' AS [FieldLabel], 'IdBooking' AS [FieldName], 'INT' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 0 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 1 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Quotation'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'IdQuotation' AS [FieldLabel], 'IdQuotation' AS [FieldName], 'INT' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 0 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Quotation'


INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Insp Fees' AS [FieldLabel], 'InspFees' AS [FieldName], 'INT' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Quotation'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Invoice No' AS [FieldLabel], 'InvoiceNo' AS [FieldName], 'INT' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Quotation'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Travel Air' AS [FieldLabel], 'TravelAir' AS [FieldName], 'INT' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Quotation'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Travel Distance' AS [FieldLabel], 'TravelDistance' AS [FieldName], 'INT' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Quotation'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Travel Hotel' AS [FieldLabel], 'TravelHotel' AS [FieldName], 'INT' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Quotation'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Travel Land' AS [FieldLabel], 'TravelLand' AS [FieldName], 'INT' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Quotation'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Travel Time' AS [FieldLabel], 'TravelTime' AS [FieldName], 'INT' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Quotation'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Unit Price' AS [FieldLabel], 'UnitPrice' AS [FieldName], 'INT' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Quotation'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'NoOfManDay' AS [FieldLabel], 'NoOfManDay' AS [FieldName], 'INT' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Quotation'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Total Cost' AS [FieldLabel], 'TotalCost' AS [FieldName], 'INT' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Quotation'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'NoOfTravel ManDay' AS [FieldLabel], 'NoOfTravelManDay' AS [FieldName], 'INT' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Quotation'


INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Invoice Date' AS [FieldLabel], 'InvoiceDate' AS [FieldName], 'DATE' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Quotation'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Invoice Remarks' AS [FieldLabel], 'InvoiceRemarks' AS [FieldName], 'VARCHAR' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Quotation'

--Quotation Fields end here


-- QC start here 


INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'Inspection_Id' AS [FieldLabel], 'Inspection_Id' AS [FieldName], 'INT' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 0 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 1 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Schedule QC'

INSERT INTO [KPI_Column]([FieldLabel], [FieldName], [FieldType],[IdSubModule], [CanFilter], [CanShowInResult], [FilterIsMultiple],[FilterDataSourceName],[FilterDataSourceTypeId],[FilterDataSourceFieldValue],
[FilterDataSourceFieldName], [FilterRequired], [Active],[IsLocationId],[IsCustomerId],[IsKey])
SELECT TOP 1 'QC Name' AS [FieldLabel], 'Schedule_QCName' AS [FieldName], 'VARCHAR' AS [FieldType], Id As [IdSubModule], 0 AS [CanFilter], 1 AS [CanShowInResult], 0 AS [FilterIsMultiple], 
NULL AS [FilterDataSourceName], NULL AS [FilterDataSourceTypeId], NULL AS [FilterDataSourceFieldValue], NULL AS [FilterDataSourceFieldName], 0 AS [FilterRequired], 1 AS [Active], 0 AS [IsLocationId], 
0 AS [IsCustomerId], 0 AS [IsKey]
FROM [AP_SubModule] 
WHERE Name = 'Schedule QC'


-- Views start here 

--CREATE VIEW [dbo].[V_GetInspectionDetails]
--AS
--    SELECT 

--    INSPECTION.Id,

--	INSPECTION.ServiceDate_From,

--	INSPECTION.ServiceDate_To,

--	INSPECTION.FirstServiceDate_From,

--	INSPECTION.FirstServiceDate_To,

--	INSPSTATUS.Id AS Status_Id,

--	INSPSTATUS.Status,

--	INSPECTION.CreatedOn,

--	CUSTOMER.Id AS Customer_Id,

--	CUSTOMER.Customer_Name,

--	CUADDRESS.Address AS Customer_Address,

--	CUCOUNTRY.Country_Name AS Customer_Country,

--	CUCITY.City_Name AS Customer_City,

--	CUBRAND.Id AS Brand_Id,

--	CUBRAND.Name AS Brand_Name,

--	CUDEPT.Id AS Department_Id,

--	CUDEPT.Name AS Department_Name,

--	CUMERCH.Id AS Merchandiser_Id,

--	CUMERCH.Contact_name AS Merchandiser_Name,

--	CUBUYER.Id AS Buyer_Id,

--	CUBUYER.Name AS Buyer_Name,

--	SUPPLIER.Id AS Supplier_Id,
--	SUPPLIER.Supplier_Name AS Supplier_Name,
--	SUADDRESS.Address AS Supplier_Address,
--	SUADDRESS.LocalLanguage AS Supplier_Regional_Address,
--	SUCOUNTRY.Country_Name AS Supplier_Country,
--	SUPROVINCE.Province_Name AS Supplier_Province,
--	SUCITY.City_Name AS Supplier_City,
--	SUCOUNTY.County_Name AS Supplier_County,

--	FACTORY.Id AS Factory_Id,
--	FACTORY.Supplier_Name AS Factory_Name,
--	FAADDRESS.Address AS Factory_Address,
--	FAADDRESS.LocalLanguage AS Factory_Regional_Address,
--	FACOUNTRY.Country_Name AS Factory_Country,
--	FAPROVINCE.Province_Name AS Factory_Province,
--	FACITY.City_Name AS Factory_City,
--	FACOUNTY.County_Name AS Factory_County,
--	FACTOWN.TownName AS Factory_Town,

--	INSPOFFICE.Id AS Location_Id,
--	INSPOFFICE.Location_Name AS Office,

--	SERVICETYPE.Id As ServiceTypeId,
--	SERVICETYPE.Name As ServiceType

--	FROM INSP_Transaction INSPECTION  


--	-- Customer and Customer Address
--	INNER JOIN CU_Customer CUSTOMER  ON CUSTOMER.Id = INSPECTION.Customer_Id
--	INNER JOIN CU_Address CUADDRESS  ON CUADDRESS.Customer_Id = INSPECTION.Customer_Id and CUADDRESS.Address_Type=1 -- only head office
--	LEFT JOIN REF_Country CUCOUNTRY ON CUCOUNTRY.Id=CUADDRESS.Country_Id
--	LEFT JOIN REF_City CUCITY ON CUCITY.Id=CUADDRESS.City_Id

--	-- Brand
--	LEFT JOIN INSP_TRAN_CU_Brand INSPBRAND  ON INSPBRAND.Inspection_Id = INSPECTION.Id and INSPBRAND.Active=1
--	LEFT JOIN CU_Brand CUBRAND ON CUBRAND.Id=INSPBRAND.Brand_Id

--	-- Department
--	LEFT JOIN INSP_TRAN_CU_Department INSPDEPT  ON INSPDEPT.Inspection_Id = INSPECTION.Id and INSPDEPT.Active=1
--	LEFT JOIN CU_Department CUDEPT ON CUDEPT.Id=INSPDEPT.Department_Id

--	-- Buyer
--	LEFT JOIN INSP_TRAN_CU_Buyer INSPBUYER  ON INSPBUYER.Inspection_Id = INSPECTION.Id and INSPBUYER.Active=1
--	LEFT JOIN CU_Buyer CUBUYER ON CUBUYER.Id=INSPBUYER.Buyer_Id

--	-- Merchandiser
--	LEFT JOIN INSP_TRAN_CU_Merchandiser INSPMERCH  ON INSPMERCH.Inspection_Id = INSPECTION.Id and INSPMERCH.Active=1
--	LEFT JOIN CU_Contact CUMERCH ON CUMERCH.Id=INSPMERCH.Merchandiser_Id


--	-- Supplier and Supplier Address 
--	INNER JOIN SU_Supplier SUPPLIER  ON SUPPLIER.Id = INSPECTION.Supplier_Id
--	INNER JOIN SU_Address SUADDRESS  ON SUADDRESS.Supplier_Id = SUPPLIER.Id and SUADDRESS.AddressTypeId=1  -- only head office
--	INNER JOIN REF_Country SUCOUNTRY ON SUCOUNTRY.Id=SUADDRESS.CountryId
--	INNER JOIN REF_Province SUPROVINCE ON  SUPROVINCE.Id=SUADDRESS.RegionId
--	INNER JOIN REF_City SUCITY ON SUCITY.Id=SUADDRESS.CityId
--    LEFT JOIN REF_County SUCOUNTY ON SUCOUNTY.Id=SUADDRESS.CountyId 

--	-- Factory and Factory Address 

--	INNER JOIN SU_Supplier FACTORY  ON FACTORY.Id = INSPECTION.Factory_Id
--	INNER JOIN SU_Address FAADDRESS  ON FAADDRESS.Supplier_Id = FACTORY.Id and FAADDRESS.AddressTypeId=1 -- only head office
--	INNER JOIN REF_Country FACOUNTRY ON FACOUNTRY.Id=FAADDRESS.CountryId
--	INNER JOIN REF_Province FAPROVINCE ON  FAPROVINCE.Id=FAADDRESS.RegionId
--	INNER JOIN REF_City FACITY ON FACITY.Id=FAADDRESS.CityId
--    LEFT JOIN  REF_County FACOUNTY ON FACOUNTY.Id=FAADDRESS.CountyId 	
--	LEFT JOIN  REF_Town FACTOWN ON FACTOWN.Id=FAADDRESS.TownId 		

--	-- Inspection status 
--	INNER JOIN INSP_Status INSPSTATUS ON INSPSTATUS.Id=INSPECTION.Status_Id

--	-- Inspection Office
--	LEFT JOIN REF_Location INSPOFFICE ON INSPOFFICE.Id=INSPECTION.Office_Id

--	-- Inspection Service fetch only for active service
--	INNER JOIN INSP_TRAN_ServiceType INSPSERVICETYPE ON INSPSERVICETYPE.Inspection_Id=INSPECTION.Id and INSPSERVICETYPE.Active=1
--	INNER JOIN REF_ServiceType SERVICETYPE ON SERVICETYPE.Id=INSPSERVICETYPE.ServiceType_Id


--CREATE VIEW [dbo].[V_GetCustomerDetails]
--AS 
--	SELECT 
--	Id,
--	Customer_Name
--	from CU_Customer where Active=1


--CREATE VIEW [dbo].[V_GetFactoryDetails]
--AS 
--	SELECT 
--	SU.Id,
--	SU.Supplier_Name as Factory_Name,
--	SU_CUS.Customer_Id
--	from SU_Supplier SU inner join SU_Supplier_Customer SU_CUS on SU_CUS.Supplier_Id=SU.Id
--	where SU.Type_id=1 and SU.Active=1


--CREATE VIEW [dbo].[V_GetSupplierDetails]

--AS 

--	SELECT 
--	SU.Id,
--	SU.Supplier_Name,
--	SU_CUS.Customer_Id
--	from SU_Supplier SU inner join SU_Supplier_Customer SU_CUS on SU_CUS.Supplier_Id=SU.Id
--	where Type_id=2 and Active=1

--	-- Views End here

--CREATE VIEW [dbo].[V_GetProductDetails]
--    AS

--	SELECT 

--		-- PRODUCTS
--	INSPPRODUCT.Inspection_Id,
--	CUPRODUCT.ProductID AS Product_Name,
--	CUPRODUCT.[Product Description] AS Product_Description,
--	PRODUCTCAT.Name AS Product_Category,
--	PRODUCTSUBCAT.Name AS Product_Sub_Category,
--		-- Reports

--	REPORT.ReportTitle AS Report_Title,
--	REPORT.ServiceFromDate AS Report_FromDate,
--	REPORT.ServiceToDate AS Report_ToDate,
--	REPORT.ResultId AS Report_ResultId,
--	REPROTRES.ResultName AS Report_Result,
--	REPORT.CriticalMax As Defect_Critical_Max,
--	REPORT.MajorMax As Defect_Major_Max,
--	REPORT.MinorMax As Defect_Minor_Max

--	FROM 

--	-- Inspection Products
--	INSP_Product_Transaction INSPPRODUCT
--	LEFT JOIN CU_Products CUPRODUCT on CUPRODUCT.Id=INSPPRODUCT.Product_Id 
--	LEFT JOIN REF_ProductCategory PRODUCTCAT on PRODUCTCAT.Id=CUPRODUCT.ProductCategory
--	LEFT JOIN REF_ProductCategory_Sub PRODUCTSUBCAT on PRODUCTSUBCAT.Id=CUPRODUCT.ProductSubCategory

--	-- Report related information

--	LEFT JOIN FB_Report_Details REPORT on REPORT.Id=INSPPRODUCT.Fb_Report_Id and REPORT.Active=1
--	LEFT JOIN FB_Report_Result REPROTRES on REPROTRES.Id =REPORT.ResultId 

--	WHERE INSPPRODUCT.Active=1

--CREATE VIEW [dbo].[V_GetPurchaseOrderDetails]
--    AS

--	SELECT 

--	INSPPO.Inspection_Id,	
--	CUPO.PONO AS PoName,
--	INSPPO.ETD,
--	PODestinationCountry.Country_Name AS Destination_Country,
--	INSPPO.BookingQuantity AS PoQuantity

--	FROM 

--	 	-- Inspection Purchase order
--	INSP_PurchaseOrder_Transaction INSPPO 
--	LEFT JOIN CU_PurchaseOrder CUPO on CUPO.Id=INSPPO.PO_Id 
--	LEFT JOIN REF_Country PODestinationCountry on PODestinationCountry.Id=INSPPO.Destination_Country_Id 

--	WHERE INSPPO.Active=1
		
--CREATE VIEW [dbo].[V_GetQCDetails]
-- AS
--SELECT

 
--	SCHQC.BookingId AS Inspection_Id,
--	STAFFQC.Person_Name AS Schedule_QCName

--	FROM 

--	SCH_Schedule_QC SCHQC 
--	INNER JOIN HR_Staff STAFFQC on STAFFQC.Id=SCHQC.QCId

--	WHERE SCHQC.Active=1
