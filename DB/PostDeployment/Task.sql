

-- -- new table added for report additional photo path end



------------Dynamic Fields For customers starts------------------------


CREATE TABLE [dbo].[DF_ControlTypes]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR(50) NOT NULL, 
    [Active] BIT NOT NULL
)

CREATE TABLE [dbo].[DF_Attributes]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] NVARCHAR(100) NOT NULL,
	[DataType] NVARCHAR(10) NOT NULL,
	[Active] BIT NOT NULL
)

CREATE TABLE [dbo].[DF_ControlType_Attributes] 
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[AttributeId] INT NOT NULL,
	[ControlTypeId] INT NOT NULL,
	[DefaultValue] NVARCHAR(50) NULL,
	[Active] BIT NOT NULL, 
    FOREIGN KEY([AttributeId]) REFERENCES DF_Attributes(Id),
	FOREIGN KEY([ControlTypeId]) REFERENCES DF_ControlTypes(Id)
)
CREATE TABLE [dbo].[REF_Modules]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR(50) NOT NULL, 
    [Active] BIT NOT NULL
)

CREATE TABLE [dbo].[DF_DDL_SourceType]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR(50) NOT NULL, 
    [Active] BIT NOT NULL
)
CREATE TABLE [dbo].[DF_CU_Configuration]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[CustomerId] INT NOT NULL,
	[ModuleId] INT NOT NULL,
	[ControlTypeId] INT NOT NULL,
	[Label] NVARCHAR(100) NOT NULL,
	[Type] NVARCHAR(100),
	[DataSourceType] INT,
	[DisplayOrder] INT NOT NULL,
	[Active] BIT NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
    FOREIGN KEY(CustomerId) REFERENCES CU_Customer(Id),
	FOREIGN KEY(ModuleId) REFERENCES REF_Modules(Id),
	FOREIGN KEY(ControlTypeId) REFERENCES DF_ControlTypes(Id),
	FOREIGN KEY(DataSourceType) REFERENCES DF_DDL_SourceType(Id),
	FOREIGN KEY(CreatedBy) REFERENCES IT_UserMaster(Id),
	FOREIGN KEY(UpdatedBy) REFERENCES IT_UserMaster(Id),
	FOREIGN KEY(DeletedBy) REFERENCES IT_UserMaster(Id)
)
CREATE TABLE [dbo].[DF_Control_Attributes]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[ControlAttributeId] INT NOT NULL,
	[Value] NVARCHAR(50) NOT NULL,
	[ControlConfigurationID] INT NOT NULL,
	[Active] BIT NOT NULL,
	FOREIGN KEY(ControlConfigurationID) REFERENCES DF_CU_Configuration(Id),
	FOREIGN KEY(ControlAttributeId) REFERENCES DF_ControlType_Attributes(Id)
)



CREATE TABLE [dbo].[DF_CU_DDL_SourceType]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[CustomerId] INT NOT NULL,
	[TypeId] INT NOT NULL,
	[Active] BIT NOT NULL,
	FOREIGN KEY(CustomerId) REFERENCES [CU_Customer](Id), 
	FOREIGN KEY(TypeId) REFERENCES [DF_DDL_SourceType](Id)
)

CREATE TABLE [dbo].[DF_DDL_Source]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] nvarchar(50) NOT NULL,
	[Type] INT NOT NULL,
	[Active] bit NOT NULL,
	[ParentId] INT NULL,
	FOREIGN KEY(Type) REFERENCES DF_DDL_SourceType(Id)
)



CREATE TABLE [dbo].[INSP_DF_Transaction]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[BookingId] INT NOT NULL,
	[ControlConfigurationId] INT NOT NULL,
	[Value] NVARCHAR(100) NULL,
	[Active] BIT NOT NULL,
	[CreatedBy] INT NOT NULL,
	[CreatedOn] DateTime NOT NULL,
	[UpdatedBy] INT NULL,
	[UpdatedOn] DateTime NULL,
	[DeletedBy] INT NULL,
	[DeletedOn] DATETIME NULL,
	FOREIGN KEY(BookingId) REFERENCES INSP_Transaction(Id),
	FOREIGN KEY(ControlConfigurationId) REFERENCES DF_CU_Configuration(Id),
	FOREIGN KEY(CreatedBy) REFERENCES IT_UserMaster(Id),
	FOREIGN KEY(UpdatedBy) REFERENCES IT_UserMaster(Id),
	FOREIGN KEY(DeletedBy) REFERENCES IT_UserMaster(Id),
)



--------------[DF_ControlTypes] Starts------------------------

INSERT INTO [DF_ControlTypes] (Name,Active) VALUES ('TextBox',1)
INSERT INTO [DF_ControlTypes] (Name,Active) VALUES ('TextArea',1)
INSERT INTO [DF_ControlTypes] (Name,Active) VALUES ('DropDown',1)
INSERT INTO [DF_ControlTypes] (Name,Active) VALUES ('DatePicker',1)

--------------[DF_ControlTypes] Ends------------------------

----------------[DF_Attributes] Starts ---------------
INSERT INTO dbo.[DF_Attributes](Name,DataType,Active)
Values ('PlaceHolder','T',1)

INSERT INTO dbo.[DF_Attributes](Name,DataType,Active)
Values ('Type','T',1)

INSERT INTO dbo.[DF_Attributes](Name,DataType,Active)
Values ('MaxLength','T',1)

INSERT INTO dbo.[DF_Attributes](Name,DataType,Active)
Values ('MinLength','T',1)

INSERT INTO dbo.[DF_Attributes](Name,DataType,Active)
Values ('Disabled','B',1)

INSERT INTO dbo.[DF_Attributes](Name,DataType,Active)
Values ('ReadOnly','B',1)

INSERT INTO dbo.[DF_Attributes](Name,DataType,Active)
Values ('Required Validation','B',1)

INSERT INTO dbo.[DF_Attributes](Name,DataType,Active)
Values ('Email Validation','B',1)

INSERT INTO dbo.[DF_Attributes](Name,DataType,Active)
Values ('Multiple','B',1)

INSERT INTO dbo.[DF_Attributes](Name,DataType,Active)
Values ('IsCascading','B',1)

INSERT INTO dbo.[DF_Attributes](Name,DataType,Active)
Values ('Parent-DropDown','D',1)

----------------[DF_Attributes] Ends ---------------


---------------------------------TextBox Starts----------------------------------------------

INSERT INTO [DF_ControlType_Attributes] ([ControlTypeId],[AttributeId],[DefaultValue],[Active])
Values(1,1,'TextBox',1)

INSERT INTO [DF_ControlType_Attributes] ([ControlTypeId],[AttributeId],[DefaultValue],[Active])
Values(1,2,'text',1)

INSERT INTO [DF_ControlType_Attributes] ([ControlTypeId],[AttributeId],[DefaultValue],[Active])
Values(1,3,'50',1)

INSERT INTO [DF_ControlType_Attributes] ([ControlTypeId],[AttributeId],[DefaultValue],[Active])
Values(1,4,'5',1)

INSERT INTO [DF_ControlType_Attributes] ([ControlTypeId],[AttributeId],[DefaultValue],[Active])
Values(1,5,'false',1)

INSERT INTO [DF_ControlType_Attributes] ([ControlTypeId],[AttributeId],[DefaultValue],[Active])
Values(1,6,'false',1)

INSERT INTO [DF_ControlType_Attributes] ([ControlTypeId],[AttributeId],[DefaultValue],[Active])
Values(1,7,'false',1)

INSERT INTO [DF_ControlType_Attributes] ([ControlTypeId],[AttributeId],[DefaultValue],[Active])
Values(1,8,'false',1)


---------------------------------TextBox Ends----------------------------------------------

---------------------------------TextArea Starts----------------------------------------------

INSERT INTO [DF_ControlType_Attributes] ([ControlTypeId],[AttributeId],[DefaultValue],[Active])
Values(2,1,'TextBox',1)

INSERT INTO [DF_ControlType_Attributes] ([ControlTypeId],[AttributeId],[DefaultValue],[Active])
Values(2,3,'50',1)

INSERT INTO [DF_ControlType_Attributes] ([ControlTypeId],[AttributeId],[DefaultValue],[Active])
Values(2,4,'5',1)

INSERT INTO [DF_ControlType_Attributes] ([ControlTypeId],[AttributeId],[DefaultValue],[Active])
Values(2,5,'false',1)

INSERT INTO [DF_ControlType_Attributes] ([ControlTypeId],[AttributeId],[DefaultValue],[Active])
Values(2,6,'false',1)

INSERT INTO [DF_ControlType_Attributes] ([ControlTypeId],[AttributeId],[DefaultValue],[Active])
Values(2,7,'false',1)

-----------------------------TextArea Ends------------------------------------------

---------------------DropDown Starts---------------------------------------------------------------

INSERT INTO [DF_ControlType_Attributes] ([ControlTypeId],[AttributeId],[DefaultValue],[Active])
Values(3,1,'Select',1)

INSERT INTO [DF_ControlType_Attributes] ([ControlTypeId],[AttributeId],[DefaultValue],[Active])
Values(3,5,'false',1)

INSERT INTO [DF_ControlType_Attributes] ([ControlTypeId],[AttributeId],[DefaultValue],[Active])
Values(3,6,'false',1)

INSERT INTO [DF_ControlType_Attributes] ([ControlTypeId],[AttributeId],[DefaultValue],[Active])
Values(3,7,'false',1)

INSERT INTO [DF_ControlType_Attributes] ([ControlTypeId],[AttributeId],[DefaultValue],[Active])
Values(3,9,'false',1)

INSERT INTO [DF_ControlType_Attributes] ([ControlTypeId],[AttributeId],[DefaultValue],[Active])
Values(3,10,'false',1)

INSERT INTO [DF_ControlType_Attributes] ([ControlTypeId],[AttributeId],[DefaultValue],[Active])
Values(3,11,null,1)


------------------------------------DropDown Ends-----------------------------------------


--------------[[REF_Modules]] Starts------------------------

INSERT INTO [REF_Modules] (Name,Active) VALUES ('Inspection-Booking',1)
INSERT INTO [REF_Modules] (Name,Active) VALUES ('Audit-booking',1)

--------------[[REF_Modules]] Ends------------------------

--------------[DF_DDL_SourceType] Starts------------------------

INSERT INTO [DF_DDL_SourceType] (Name,Active) VALUES ('ECI-BDM',1)
INSERT INTO [DF_DDL_SourceType] (Name,Active) VALUES ('ECI-Office',1)
INSERT INTO [DF_DDL_SourceType] (Name,Active) VALUES ('ECI-QCM Name',1)
INSERT INTO [DF_DDL_SourceType] (Name,Active) VALUES ('ECI-Partial_Shipment ',1)
INSERT INTO [DF_DDL_SourceType] (Name,Active) VALUES ('ECI-Prepack ',1)

--------------[DF_DDL_SourceType] Ends------------------------

--------------[DF_DDL_Source] Starts------------------------
--SELECT * from [DF_DDL_Source]
--INSERT INTO [DF_DDL_Source] (Name,Type,Active,ParentId) VALUES ('Arun',1,1,null)
--INSERT INTO [DF_DDL_Source] (Name,Type,Active,ParentId) VALUES ('Joby',1,1,null)
--INSERT INTO [DF_DDL_Source] (Name,Type,Active,ParentId) VALUES ('ECI-Shanghai',2,1,null)
--INSERT INTO [DF_DDL_Source] (Name,Type,Active,ParentId) VALUES ('ECI-India',2,1,null)
--INSERT INTO [DF_DDL_Source] (Name,Type,Active,ParentId) VALUES ('ECI-Shenzhen',2,1,null)
--INSERT INTO [DF_DDL_Source] (Name,Type,Active,ParentId) VALUES ('ECI-HongKong',2,1,null)

--INSERT INTO [DF_DDL_Source] (Name,Type,Active,ParentId) VALUES ('Assam',2,1,null)
--INSERT INTO [DF_DDL_Source] (Name,Type,Active,ParentId) VALUES ('Andhra Pradesh',2,1,null)
--INSERT INTO [DF_DDL_Source] (Name,Type,Active,ParentId) VALUES ('Karnataka',2,1,2)
--INSERT INTO [DF_DDL_Source] (Name,Type,Active,ParentId) VALUES ('Kerela',2,1,2)

--INSERT INTO [DF_DDL_Source] (Name,Type,Active) VALUES ('Jaimy',3,1)
--INSERT INTO [DF_DDL_Source] (Name,Type,Active) VALUES ('Aloha',3,1)
--INSERT INTO [DF_DDL_Source] (Name,Type,Active) VALUES ('Customer-Brand-3',3,1)

--INSERT INTO [DF_DDL_Source] (Name,Type,Active) VALUES ('Yes',4,1)
--INSERT INTO [DF_DDL_Source] (Name,Type,Active) VALUES ('No',4,1)
--INSERT INTO [DF_DDL_Source] (Name,Type,Active) VALUES ('Yes',5,1)
--INSERT INTO [DF_DDL_Source] (Name,Type,Active) VALUES ('No',5,1)
--INSERT INTO [DF_DDL_Source] (Name,Type,Active) VALUES ('Customer-Contact-2',4,1)
--INSERT INTO [DF_DDL_Source] (Name,Type,Active) VALUES ('Customer-Contact-2',4,1)
--INSERT INTO [DF_DDL_Source] (Name,Type,Active) VALUES ('Customer-Contact-2',4,1)

--------------[DF_DDL_Source] Ends---------------------------

INSERT INTO DF_CU_DDL_SourceType(CustomerId,TypeId,Active) Values (80,1,1)
INSERT INTO DF_CU_DDL_SourceType(CustomerId,TypeId,Active) Values (80,2,1)
INSERT INTO DF_CU_DDL_SourceType(CustomerId,TypeId,Active) Values (80,3,1)
INSERT INTO DF_CU_DDL_SourceType(CustomerId,TypeId,Active) Values (80,4,1)
INSERT INTO DF_CU_DDL_SourceType(CustomerId,TypeId,Active) Values (80,5,1)


------------Dynamic Fields For customers ends------------------------





-- Rabbit MQ LOG email start here 

CREATE TABLE [dbo].[LOG_Email_Queue]
(
[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,

[Subject] NVARCHAR(2000) NULL,
[Body] NVARCHAR(max) NULL,

[SourceId] int NULL,
[SourceName] NVARCHAR(200) NULL,

[ToList] NVARCHAR(max) NULL,
[CCList] NVARCHAR(2000) NULL,
[BCCList] NVARCHAR(2000) NULL,

-- notstarted,success,failure from enum values - 1,2,3
[Status] INT NULL,
[SendOn] DATETIME NULL,
[TryCount] INT NULL,

[Active] BIT NOT NULL,
[CreatedBy] INT NOT NULL,
[CreatedOn] DATETIME NOT NULL,

FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)


CREATE TABLE [dbo].[LOG_Email_Queue_Attachments]
(
[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
[GuidId] [uniqueidentifier] ROWGUIDCOL NOT NULL Unique,

[Email_Queue_Id] [int] NOT NULL,
[FileName] [nvarchar](500) NOT NULL,
[File] [varbinary](max) FILESTREAM NULL,

[Active] [bit] NOT NULL,
[CreatedBy] [int] NOT NULL,
[CreatedOn] DATETIME NULL,

FOREIGN KEY ([Email_Queue_Id]) REFERENCES [dbo].[LOG_Email_Queue](Id),
FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)


-- Rabbit MQ LOG email start end 

-- Alter statement for da_usercustomer  starts--
Alter table DA_UserCustomer drop  column CS;

alter table DA_UserCustomer alter column CustomerId INT NULL;

ALTER TABLE DA_UserCustomer
ADD CONSTRAINT DA_UserCustomer_CreatedOn
Default getdate() FOR CreatedOn;
-- Alter statement for DA_UserCustomer ends--

-- Alter statement for DA_UserRoleNotificationByOffice starts--
Alter table DA_UserRoleNotificationByOffice drop constraint FK_DAUserRoleNotificationByOffice_UserId
Alter table DA_UserRoleNotificationByOffice drop column UserId

Alter table DA_UserRoleNotificationByOffice add ProductCategoryId INT Null;
ALTER TABLE DA_UserRoleNotificationByOffice ADD CONSTRAINT FK_DAUserRoleNotificationByOffice_ProductCategoryId FOREIGN KEY (ProductCategoryId) REFERENCES REF_ProductCategory(Id);

Alter table DA_UserRoleNotificationByOffice alter column OfficeId INT Null;
Alter table DA_UserRoleNotificationByOffice alter column ServiceId INT Null;
-- Alter statement for DA_UserRoleNotificationByOffice ends--




-- if DA_UserCustomer and DA_UserRoleNotificationByOffice table does not have record(empty) we can execute below query start--

-- Alter statement for da_usercustomer  starts--
Alter table DA_UserCustomer alter column CreatedOn DATETIME Not Null;

Alter table DA_UserCustomer add UserType INT NOT Null;
ALTER TABLE DA_UserCustomer ADD CONSTRAINT FK_DAUserCustomer_UserType FOREIGN KEY (UserType) REFERENCES HR_Profile(Id);

-- Alter statement for DA_UserCustomer ends--

-- Alter statement for DA_UserRoleNotificationByOffice starts--
Alter table DA_UserRoleNotificationByOffice add DAUserCustomerId INT NOT Null;
ALTER TABLE DA_UserRoleNotificationByOffice ADD CONSTRAINT FK_DAUserRoleNotificationByOffice_DAUserCustomerId FOREIGN KEY (DAUserCustomerId) REFERENCES DA_UserCustomer(Id);

-- Alter statement for DA_UserRoleNotificationByOffice ends--

-- if DA_UserCustomer and DA_UserRoleNotificationByOffice table does not have record(empty) we can execute below query ends--



-- if else DA_UserCustomer and DA_UserRoleNotificationByOffice table have record we can execute below query start--

-- Alter statement for da_usercustomer  starts--

UPDATE DA_UserCustomer SET CreatedOn = GetDate() where CreatedOn IS NULL

Alter table DA_UserCustomer alter column CreatedOn DATETIME Not Null; 

Alter table DA_UserCustomer add UserType INT NOT Null default 1;
ALTER TABLE DA_UserCustomer ADD CONSTRAINT FK_DAUserCustomer_UserType FOREIGN KEY (UserType) REFERENCES HR_Profile(Id);

-- Alter statement for DA_UserCustomer ends--

-- Alter statement for DA_UserRoleNotificationByOffice starts--
Alter table DA_UserRoleNotificationByOffice add DAUserCustomerId INT NOT Null default 1;
ALTER TABLE DA_UserRoleNotificationByOffice ADD CONSTRAINT FK_DAUserRoleNotificationByOffice_DAUserCustomerId FOREIGN KEY (DAUserCustomerId) REFERENCES DA_UserCustomer(Id);

-- Alter statement for DA_UserRoleNotificationByOffice ends--

-- if DA_UserCustomer and DA_UserRoleNotificationByOffice table have record we can execute below query ends--




-- Alter statement for it_role starts--

Alter table It_Role add SecondaryRole bit NOT NULL Default 0

-- Alter statement for it_role ends--

-- update statement for it_role starts--

update IT_Role set SecondaryRole = 1 where id in(19,20,21,22,23,24,25,26,27,32)

-- update statement for it_role ends--

--insert statement in it_role start--
insert into it_role (RoleName,Active,EntityId,PrimaryRole,SecondaryRole)
values('Report Checker',1,1,1,1)
--insert statement in it_role end--


-- Alter statement Mid_Task,Mid_Notification starts--
Alter table Mid_Task add [CreatedBy] INT Null;
ALTER TABLE Mid_Task Add FOREIGN KEY (UserId) REFERENCES [dbo].[IT_UserMaster](Id);

Alter table Mid_Task add [UpdatedBy] INT NULL;
ALTER TABLE Mid_Task ADD  FOREIGN KEY (UserId) REFERENCES [dbo].[IT_UserMaster](Id);

Alter table Mid_Task add [CreatedOn] DATETIME NOT NULL DEFAULT GETDATE()
Alter table Mid_Task add [UpdatedOn] DATETIME NULL


Alter table Mid_Notification add [CreatedOn] DATETIME NOT NULL DEFAULT GETDATE()
Alter table Mid_Notification add [UpdatedOn] DATETIME NULL
-- Alter statement Mid_Task,Mid_Notification end --

---------- Add a new column to CU_ServiceType-----------
ALTER TABLE CU_ServiceType ADD CustomServiceTypeName nvarchar(1500)
---------- Add a new column to CU_ServiceType-----------

--Remove Product Category from insp_transaction starts--
Alter table INSP_Transaction drop  column ProductCategory_Id;
--Remove Product Category from insp_transaction ends--
---------- Add a new column to CU_ServiceType-----------

------------Merchandiser Table Start-----------------------------

CREATE TABLE [dbo].[INSP_TRAN_CU_Merchandiser]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Inspection_Id] INT NOT NULL, 
    [Merchandiser_Id] INT NOT NULL, 
    [Active] BIT NOT NULL,
	[CreatedOn] DATETIME NULL, 
    [CreatedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
    [DeletedBy] INT NULL,
	FOREIGN KEY ([Inspection_Id]) REFERENCES [dbo].[INSP_Transaction](Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([Merchandiser_Id]) REFERENCES [dbo].[CU_Contact](Id)
)
------------Merchandiser Table End-----------------------------


--menu added for customer price card starts--

declare @IdTran as int, @IdItem as int
INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Customer Price Card', NULL, 11)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[IT_Right] ([ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], MenuName_IdTran) 
VALUES (56, NULL, N'Customer Price Card', N'pricecardedit/price-card', 0, 1, N'fa fa-plus-square', 40,@IdTran)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team	


	declare @IdTran as int, @IdItem as int
INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Customer Price Card Summary', NULL, 11)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[IT_Right] ([ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], MenuName_IdTran) 
VALUES (56, NULL, N'Customer Price Card Summary', N'pricecardsummary/price-card-summary', 0, 1, N'fa fa-plus-square', 40,@IdTran)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team	

	--menu added for customer price card end--


	--create table script start--

	CREATE TABLE [dbo].[CU_PR_Details] (
[Id] INT NOT NULL PRIMARY KEY identity(1,1),
[CustomerId] int not null,
[BillingMethodId] int not null,
[BillingToId] int not null,
[ServiceId] int not null, 
[CurrencyId] int not null,
[UnitPrice] float not null, 
[TaxIncluded] bit , 
[TravelIncluded] bit, 
[FreeTravelKM] int,
[Remarks] Nvarchar(3000),
[PeriodFrom] datetime,
[PeriodTo] datetime,
[Active] bit,
[CreatedBy] INT NULL, 
[DeletedBy] INT NULL, 
[UpdatedBy] INT NULL, 
[UpdatedOn] DATETIME, 
[CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
[DeletedOn] DATETIME NULL
CONSTRAINT FK_CU_PR_Details_CustomerId FOREIGN KEY (CustomerId) REFERENCES [dbo].[Cu_customer](Id),
CONSTRAINT FK_CU_PR_Details_CurrencyId FOREIGN KEY (CurrencyId) REFERENCES [dbo].[ref_currency](Id),
CONSTRAINT FK_CU_PR_Details_BillingMethodId FOREIGN KEY (BillingMethodId) REFERENCES [dbo].[QU_BillMethod](Id),
CONSTRAINT FK_CU_PR_Details_BillingToId FOREIGN KEY (BillingToId) REFERENCES [dbo].[QU_PaidBy](Id),
CONSTRAINT FK_CU_PR_Details_ServiceId FOREIGN KEY (ServiceId) REFERENCES [dbo].[REF_SERVICE](Id),
CONSTRAINT FK_CU_PR_Details_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_CU_PR_Details_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
CONSTRAINT FK_CU_PR_Details_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)

CREATE TABLE [dbo].[CU_PR_Country]
(
[Id] INT NOT NULL PRIMARY KEY identity(1,1),
[CU_PR_Id] int not null,
[FactoryCountryId] int not null,
[Active] bit,
[CreatedBy] INT NULL, 
[DeletedBy] INT NULL, 
[UpdatedBy] INT NULL, 
[UpdatedOn] DATETIME, 
[CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
[DeletedOn] DATETIME NULL
CONSTRAINT FK_CU_PR_Country_CU_PR_Id FOREIGN KEY ([CU_PR_Id]) REFERENCES [dbo].[CU_PR_Details](Id),	
CONSTRAINT FK_CU_PR_Country_FactoryCountryId FOREIGN KEY ([FactoryCountryId]) REFERENCES [dbo].[REF_Country](Id),	
CONSTRAINT FK_CU_PR_Country_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_CU_PR_Country_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
CONSTRAINT FK_CU_PR_Country_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)




CREATE TABLE [dbo].[CU_PR_ProductCategory]
(
[Id] INT NOT NULL PRIMARY KEY identity(1,1),
[CU_PR_Id] int not null,
[ProductCategoryId] int not null,
[Active] bit,
[CreatedBy] INT NULL, 
[DeletedBy] INT NULL, 
[UpdatedBy] INT NULL, 
[UpdatedOn] DATETIME, 
[CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
[DeletedOn] DATETIME NULL
CONSTRAINT FK_CU_PR_ProductCategory_CU_PR_Id FOREIGN KEY ([CU_PR_Id]) REFERENCES [dbo].[CU_PR_Details](Id),	
CONSTRAINT FK_CU_PR_ProductCategory_ProductCategoryId FOREIGN KEY ([ProductCategoryId]) REFERENCES [dbo].[REF_ProductCategory](Id),	
CONSTRAINT FK_CU_PR_ProductCategory_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_CU_PR_ProductCategory_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
CONSTRAINT FK_CU_PR_ProductCategory_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)




CREATE TABLE [dbo].[CU_PR_Province]
(
[Id] INT NOT NULL PRIMARY KEY identity(1,1),
[CU_PR_Id] int not null,
[FactoryProvinceId] int not null,
[Active] bit,
[CreatedBy] INT NULL, 
[DeletedBy] INT NULL, 
[UpdatedBy] INT NULL, 
[UpdatedOn] DATETIME, 
[CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
[DeletedOn] DATETIME NULL
CONSTRAINT FK_CU_PR_Province_CU_PR_Id FOREIGN KEY ([CU_PR_Id]) REFERENCES [dbo].[CU_PR_Details](Id),	
CONSTRAINT FK_CU_PR_Province_FactoryProvinceId FOREIGN KEY ([FactoryProvinceId]) REFERENCES [dbo].[REF_Province](Id),	
CONSTRAINT FK_CU_PR_Province_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_CU_PR_Province_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
CONSTRAINT FK_CU_PR_Province_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)




CREATE TABLE [dbo].[CU_PR_ServiceType]
(
[Id] INT NOT NULL PRIMARY KEY identity(1,1),
[CU_PR_Id] int not null,
[ServiceTypeId] int not null,
[Active] bit,
[CreatedBy] INT NULL, 
[DeletedBy] INT NULL, 
[UpdatedBy] INT NULL, 
[UpdatedOn] DATETIME, 
[CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
[DeletedOn] DATETIME NULL
CONSTRAINT FK_CU_PR_ServiceType_CU_PR_Id FOREIGN KEY ([CU_PR_Id]) REFERENCES [dbo].[CU_PR_Details](Id),	
CONSTRAINT FK_CU_PR_ServiceType_ServiceTypeId FOREIGN KEY ([ServiceTypeId]) REFERENCES [dbo].[REF_ServiceType](Id),	
CONSTRAINT FK_CU_PR_ServiceType_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_CU_PR_ServiceType_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
CONSTRAINT FK_CU_PR_ServiceType_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)




CREATE TABLE [dbo].[CU_PR_Supplier]
(
[Id] INT NOT NULL PRIMARY KEY identity(1,1),
[CU_PR_Id] int not null,
[SupplierId] int not null,
[Active] bit,
[CreatedBy] INT NULL, 
[DeletedBy] INT NULL, 
[UpdatedBy] INT NULL, 
[UpdatedOn] DATETIME, 
[CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
[DeletedOn] DATETIME NULL
CONSTRAINT FK_CU_PR_Supplier_CU_PR_Id FOREIGN KEY ([CU_PR_Id]) REFERENCES [dbo].[CU_PR_Details](Id),	
CONSTRAINT FK_CU_PR_Supplier_SupplierId FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[SU_Supplier](Id),	
CONSTRAINT FK_CU_PR_Supplier_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_CU_PR_Supplier_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
CONSTRAINT FK_CU_PR_Supplier_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)

	--create table script end--

	--------------Billing Entity Table -------------------------------

CREATE TABLE [dbo].[REF_Billing_Entity]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Active] BIT NOT NULL
)

INSERT INTO [dbo].[REF_Billing_Entity] ([Name], [Active])
VALUES ('Asia Pacific Inspection Ltd - HONG KONG', 1),
		('Guangzhou Ouyatai - CHINA', 1),
		('Asia Pacific Inspection Vietnam Company Ltd - VIETNAM', 1),
		('API Audit Limited - HONG KONG (Audit)', 1)

--Add billingEntity column in QU_Quotation
ALTER TABLE QU_Quotation
    ADD BillingEntity INT DEFAULT NULL,
    FOREIGN KEY(BillingEntity) REFERENCES [REF_Billing_Entity](Id)

--------------Billing Entity Table -------------------------------


ALTER TABLE DF_CU_Configuration ADD [FBReference] NVARCHAR(200) NULL

---Additional Information from FB to ApiLink----------------------------------

ALTER  TABLE [FB_Report_Details] ADD [ReportSummaryLink] NVARCHAR(4000) NULL
ALTER  TABLE [FB_Report_Details] ADD [InspectionStartTime] DATETIME NULL
ALTER  TABLE [FB_Report_Details] ADD [InspectionEndTime] DATETIME NULL
ALTER  TABLE [FB_Report_Details] ADD [CriticalMax] INT NULL
ALTER  TABLE [FB_Report_Details] ADD [MajorMax] INT NULL
ALTER  TABLE [FB_Report_Details] ADD [MinorMax] INT NULL

ALTER  TABLE [FB_Report_InspDefects] ADD [DefectId] INT NULL
ALTER  TABLE [FB_Report_InspDefects] ADD [CategoryId] INT NULL
ALTER  TABLE [FB_Report_InspDefects] ADD [CategoryName] NVARCHAR(3000) NULL

----------------Quotation Status Log Table ----------------------------

CREATE TABLE [dbo].[QU_TRAN_Status_Log]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [QuotationId] INT NOT NULL, 
    [BookingId] INT NULL, 
    [AuditId] INT NULL, 
    [StatusId] INT NOT NULL, 
    [StatusChangeDate] DATETIME NOT NULL, 
    [CreatedBy] INT NULL,
	[CreatedOn] DATETIME NOT NULL, 
    FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY([StatusId]) REFERENCES [dbo].[QU_Status](Id),
	FOREIGN KEY([QuotationId]) REFERENCES [QU_Quotation](Id)
)

------------Quotation Log Status table -------------------------------





-- product Level changes start ----

CREATE TABLE [dbo].[INSP_Product_Transaction]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
    [Inspection_Id] INT NOT NULL, 
	[Product_Id] INT NOT NULL, 
    [AQL] INT NULL, 
    [Critical] INT NULL, 
    [Major] INT NULL, 
    [Minor] INT NULL, 
	[Unit] INT NOT NULL, 
	[UnitCount] INT  NULL, 
	[TotalBookingQuantity] INT NOT NULL, 			
	[Combine_Product_Id] INT  NULL,
	[AQL_Quantity] INT NULL,
    [Combine_AQL_Quantity] INT NULL,
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
	[UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
	[Remarks] varchar(max) null,

    [Active] BIT NULL,
	[Fb_Report_Id] INT NULL, 
	[Fb_Mission_Product_Id] INT NULL, 
    FOREIGN KEY([Inspection_Id]) REFERENCES [dbo].[INSP_Transaction](Id),
	FOREIGN KEY([AQL]) REFERENCES [dbo].[REF_LevelPick1](Id),
	FOREIGN KEY([Critical]) REFERENCES [dbo].[REF_Pick1](Id),
	FOREIGN KEY([Major]) REFERENCES [dbo].[REF_Pick1](Id),
	FOREIGN KEY([Minor]) REFERENCES [dbo].[REF_Pick1](Id),
	FOREIGN KEY([Unit]) REFERENCES [dbo].[REF_Unit](Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([Fb_Report_Id]) REFERENCES [FB_Report_Details](Id),
	FOREIGN KEY ([Product_Id]) REFERENCES [CU_Products](Id)
)

CREATE TABLE [dbo].[INSP_PurchaseOrder_Transaction]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
	[PO_Id] INT NOT NULL,

	[Container_Ref_Id] INT  NULL, 
	[Product_Ref_Id] INT NOT NULL,
    [Inspection_Id] INT NOT NULL, 
	[BookingQuantity] INT NOT NULL, 
	[PickingQuantity] INT  NULL, 	
	[Remarks] NVARCHAR(max) NULL, 	

	[Destination_Country_Id] INT NULL, 
	[ETD] DATETIME NULL,
	
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
	[UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 

    [Active] BIT NULL,	


	FOREIGN KEY([Product_Ref_Id]) REFERENCES [dbo].[INSP_Product_Transaction](Id),
    FOREIGN KEY([Inspection_Id]) REFERENCES [dbo].[INSP_Transaction](Id),
	FOREIGN KEY ([PO_Id]) REFERENCES [dbo].[CU_PurchaseOrder](Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
    FOREIGN KEY (Destination_Country_Id) REFERENCES [dbo].[REF_Country] (Id)
)

ALTER TABLE [dbo].[INSP_TRAN_Picking]
ADD FOREIGN KEY(PO_Tran_Id) REFERENCES [INSP_PurchaseOrder_Transaction](Id)

ALTER TABLE [dbo].[FB_Report_Quantity_Details]
ADD FOREIGN KEY (InspPoTransactionId) REFERENCES [dbo].[INSP_PurchaseOrder_Transaction](Id);

--alter table FB_Report_InspDefects
--drop CONSTRAINT FK__FB_Report__InspP__40E497F3

ALTER TABLE FB_Report_InspDefects
ADD FOREIGN KEY (InspPoTransactionId) REFERENCES  [INSP_PurchaseOrder_Transaction](Id)

--alter table INSP_IC_TRAN_Products
--drop CONSTRAINT FK_INSP_IC_TRAN_Products_BookingProductId 

ALTER TABLE INSP_IC_TRAN_Products
ADD
CONSTRAINT FK_INSP_IC_TRAN_Products_BookingProductId FOREIGN KEY ([BookingProductId]) REFERENCES [dbo].[INSP_PurchaseOrder_Transaction](Id)


CREATE TABLE [dbo].[QU_INSP_Product]
(		
		IdQuotation INT NOT NULL, 
		ProductTranId INT NOT NULL,
		SampleQty INT NOT NULL,
		AqlLevelDesc NVARCHAR(600) NULL,
		PRIMARY KEY(IdQuotation, ProductTranId),
		FOREIGN KEY(IdQuotation) REFERENCES [dbo].[QU_Quotation](Id),
		FOREIGN KEY (ProductTranId) REFERENCES [dbo].[INSP_Product_Transaction](Id)
)

drop table INSP_PO_Transaction

drop table  QU_Quotation_PO

-- product Level changes end ----

--------------Invoice in booking and audit-------------------------------

ALTER TABLE QU_QUOTATION_INSP 
ADD InvoiceNo nvarchar(1000) null

ALTER TABLE QU_QUOTATION_INSP 
ADD InvoiceDate datetime null

ALTER TABLE QU_QUOTATION_INSP 
ADD InvoiceRemarks nvarchar(2000) null

ALTER TABLE QU_QUOTATION_INSP 
ADD UpdatedBy int null

ALTER TABLE QU_QUOTATION_INSP 
ADD UpdatedOn datetime null



ALTER TABLE QU_QUOTATION_AUDIT 
ADD InvoiceNo nvarchar(1000) null

ALTER TABLE QU_QUOTATION_AUDIT 
ADD InvoiceDate datetime null



ALTER TABLE QU_QUOTATION_AUDIT 
ADD InvoiceRemarks nvarchar(2000) null

ALTER TABLE QU_QUOTATION_AUDIT 


ADD UpdatedBy int null

ALTER TABLE QU_QUOTATION_AUDIT 
ADD UpdatedOn datetime null

update IT_ROLE set SecondaryRole = 1 where id = 12 and RoleName = 'Accounting'

--------------Invoice in booking and audit-------------------------------



------------Quotation Log Status table -------------------------------

---------------------------KPI--------------------------------

CREATE TABLE [dbo].[REF_KPI_Teamplate]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [CustomerId] INT NULL, 
    [Active] BIT NOT NULL
)

INSERT INTO [dbo].[REF_KPI_Teamplate]
           ([Name]
           ,[Active])
     VALUES
           ('ECI_YTD', 1)

CREATE TABLE [dbo].[INSP_CU_Status]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [StatusId] INT NOT NULL, 
    [CustomerId] INT NOT NULL, 
    [CustomStatusName] NVARCHAR(50) NOT NULL, 
    [Active] BIT NOT NULL,
	FOREIGN KEY(CustomerId) REFERENCES [CU_Customer](Id), 
	FOREIGN KEY([StatusId]) REFERENCES INSP_Status(Id)
)

--Customer ID 80 in Prod
INSERT INTO [INSP_CU_Status] (StatusId, CustomerId, CustomStatusName, Active)
VALUES (4, 80, 'cancel', 1),
(1, 80, 'Pending', 1),
(10, 80, 'Pending', 1),
(3, 80, 'Booking Change', 1),
(8, 80, 'Quotation Provided', 1),
(2, 80, 'ECI Confirmed', 1),
(5, 80, 'ECI Confirmed', 1),
(6, 80, 'Completed', 1)

---------------------------KPI--------------------------------

ALTER TABLE [INSP_PurchaseOrder_Transaction] ADD [CustomerReferencePo] NVARCHAR(1000) NULL

ALTER TABLE [CU_PurchaseOrder] ADD [CustomerReferencePo] NVARCHAR(1000) NULL



-- Container Level start -- 

CREATE TABLE [dbo].[REF_Container_Size]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] [nvarchar](200) NOT NULL,
	[Active] [bit] NOT NULL
)

INSERT INTO  REF_Container_Size (NAME,Active) VALUES('250KB',1)

INSERT INTO  REF_Container_Size (NAME,Active) VALUES('750KB',1)

INSERT INTO  REF_Container_Size (NAME,Active) VALUES('500KB',1)

CREATE TABLE [dbo].[INSP_Container_Transaction]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	

    [Inspection_Id] INT NOT NULL, 
	[Container_Id] INT NOT NULL,    
	[TotalBookingQuantity] INT NOT NULL,	
	[Remarks] varchar(max) null,
	[ContainerSize] INT NULL,    
	
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
	[UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 	

    [Active] BIT NULL,

	[Fb_Report_Id] INT NULL, 
	[Fb_Mission_Product_Id] INT NULL, 

    FOREIGN KEY([Inspection_Id]) REFERENCES [dbo].[INSP_Transaction](Id),	
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([Fb_Report_Id]) REFERENCES [FB_Report_Details](Id),
	FOREIGN KEY(ContainerSize) REFERENCES [REF_Container_Size](Id)
)

ALTER TABLE [dbo].[INSP_PurchaseOrder_Transaction]
ADD 
FOREIGN KEY(Container_Ref_Id) 
REFERENCES [INSP_Container_Transaction](Id)

INSERT [dbo].[REF_ServiceType] ([Id], [Name], [Active], [EntityId],IsReInspectedService, Abbreviation)
VALUES (9, N'Container Service', 1, 1,0,N'CS')

ALTER TABLE [dbo].[INSP_Container_Transaction]
ADD 
FOREIGN KEY(ContainerSize) 
REFERENCES [REF_Container_Size](Id)

-- Container Level end -- 

---------------- Cu_Collection, Cu_PriceCategory, [FB_report_Template] ---------------------

CREATE TABLE [dbo].[CU_Collection]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(100) NULL, 
    [CustomerId] INT NULL, 
    [Active] BIT NULL,
	FOREIGN KEY(CustomerId) REFERENCES [Cu_Customer](Id)
)

CREATE TABLE [dbo].[CU_PriceCategory]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(100) NULL, 
    [CustomerId] INT NULL, 
    [Active] BIT NULL,
	FOREIGN KEY(CustomerId) REFERENCES [Cu_Customer](Id)
)

CREATE TABLE [dbo].[FB_Report_Template]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(1000) NULL, 
    [FbTemplateId] INT NULL, 
    [Active] BIT NULL
)

ALTER TABLE INSP_TRANSACTION ADD [CollectionId] INT
FOREIGN KEY([CollectionId]) REFERENCES [CU_Collection](id)

ALTER TABLE INSP_TRANSACTION ADD [PriceCategoryId] INT
FOREIGN KEY([PriceCategoryId]) REFERENCES [CU_PriceCategory](id)

ALTER TABLE INSP_Product_Transaction ADD IsEcopack BIT

ALTER TABLE INSP_Product_Transaction ADD IsDisplay BIT

ALTER TABLE INSP_Product_Transaction ADD [FBTemplateId] INT
FOREIGN KEY([FBTemplateId]) REFERENCES [FB_Report_Template](id)

------------Cu_Collection, Cu_PriceCategory, [FB_report_Template] -------------------------

--invoice travel matrix query starts---
create table INV_TM_Type(
Id int not null primary key identity(1,1),
Name nvarchar(200),
Active bit, 
CreatedOn datetime default getdate()
)



create table INV_TM_Details
(
Id int not null primary key identity(1,1),
CustomerId int null,
TravelMatrixTypeId int not null,
CountryId int not null,
ProvinceId int not null,
CityId int not null,
CountyId int not null,
InspPortCountyId int not null,
DistanceKM float null,
TravelTime float null,
BusCost float null,
TrainCost float null,
TaxiCost float null,
HotelCost float null,
OtherCost float null,
MarkUpCost float null,
AirCost float null,
MarkUpAirCost float null,
TravelCurrencyId int null,
SourceCurrencyId int null,
FixedExchangeRate float null,
Remarks nvarchar(2000) null,
Active bit, 
CreatedOn datetime default getdate(),
UpdatedOn datetime,
DeletedOn datetime,
UpdatedBy int null,
DeletedBy int null,
CreatedBy int null,
CONSTRAINT INV_TM_Details_CustomerId  FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Cu_Customer](Id),	
CONSTRAINT INV_TM_Details_TravelMatrixTypeId FOREIGN KEY ([TravelMatrixTypeId]) REFERENCES [dbo].[INV_TM_Type](Id),	
CONSTRAINT INV_TM_Details_CountryId  FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Ref_country](Id),	
CONSTRAINT INV_TM_Details_CityId FOREIGN KEY ([CityId]) REFERENCES [dbo].[Ref_City](Id),	
CONSTRAINT INV_TM_Details_ProvinceId  FOREIGN KEY ([ProvinceId]) REFERENCES [dbo].[Ref_Province](Id),	
CONSTRAINT INV_TM_Details_TravelCurrencyId  FOREIGN KEY ([TravelCurrencyId]) REFERENCES [dbo].[Ref_currency](Id),	
CONSTRAINT INV_TM_Details_SourceCurrencyId FOREIGN KEY ([SourceCurrencyId]) REFERENCES [dbo].[Ref_currency](Id),	
CONSTRAINT INV_TM_Details_InspPortCountyId  FOREIGN KEY ([InspPortCountyId]) REFERENCES [dbo].[Ref_county](Id),	
CONSTRAINT INV_TM_Details_CountyId FOREIGN KEY ([CountyId]) REFERENCES [dbo].[Ref_county](Id),	
CONSTRAINT INV_TM_Details_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT INV_TM_Details_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
CONSTRAINT INV_TM_Details_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)


insert into INV_TM_Type (name,Active) values('Standard A',1)
insert into INV_TM_Type (name,Active) values('Standard B',1)
insert into INV_TM_Type (name,Active) values('Customized',1)

--invoice travel matrix query ends---


--menu Travel Matrix starts --
declare @IdTran as int  ,@ParentIdLvl2 as int
INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Travel Matrix', NULL,11)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[IT_Right] ([ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], TitleName_IdTran) VALUES (19, N'Travel Matrix', 'Travel Matrix', 'travelmatrix/travel-matrix', 0, 1, N'fa fa-sign-in', 8,@IdTran)
SELECT @ParentIdLvl2 = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @ParentIdLvl2) -- IT-Team

-- menu Travel Matrix ends --

------ travel column add to insp manday and aud manday------------

ALTER TABLE QU_Quotation_Insp ADD NoOfTravelManDay INT NULL

ALTER TABLE QU_Quotation_Audit ADD NoOfTravelManDay INT NULL

-------travel column add to insp manday and aud manday ------------

ALTER TABLE CU_PR_Details ADD TravelMatrixTypeId INT NULL
CONSTRAINT CU_PR_Details_TravelMatrixTypeId FOREIGN KEY ([TravelMatrixTypeId]) REFERENCES [dbo].[INV_TM_Type](Id)
-------travel column add to insp manday and aud manday ------------

-------travel time and distance to quotation insp ------------

ALTER TABLE QU_Quotation_Insp ADD TravelDistance FLOAT NULL

ALTER TABLE QU_Quotation_Insp ADD TravelTime FLOAT NULL

ALTER TABLE QU_Quotation ADD PaymentTerms INT NULL
FOREIGN KEY REFERENCES REF_InvoiceType(Id)

-------travel time and distance to quotation insp ------------


ALTER TABLE INSP_PurchaseOrder_Transaction
ADD [Fb_Mission_Product_Id] INT NULL

ALTER TABLE INSP_Product_Transaction
DROP COLUMN [Fb_Mission_Product_Id]

ALTER TABLE INSP_Container_Transaction
DROP COLUMN [Fb_Mission_Product_Id]

-------travel time and distance to quotation audit ------------

ALTER TABLE QU_Quotation_Audit ADD TravelDistance FLOAT NULL

ALTER TABLE QU_Quotation_Audit ADD TravelTime FLOAT NULL

-------travel time and distance to quotation audit ------------


ALTER TABLE [dbo].[INSP_Transaction] ADD [IsProcessing] BIT NULL DEFAULT 0


--Price Card Details Config Scripts Starts--

CREATE TABLE [dbo].[CU_PR_HolidayInfo]
(
	[Id] int identity(1,1) primary key,
	[Name] NVARCHAR(50),
	[Active] BIT
)

INSERT INTO CU_PR_HolidayInfo(Name,Active) Values ('Sun',1)
INSERT INTO CU_PR_HolidayInfo(Name,Active) Values ('Mon',1)
INSERT INTO CU_PR_HolidayInfo(Name,Active) Values ('Tue',1)
INSERT INTO CU_PR_HolidayInfo(Name,Active) Values ('Wed',1)
INSERT INTO CU_PR_HolidayInfo(Name,Active) Values ('Thu',1)
INSERT INTO CU_PR_HolidayInfo(Name,Active) Values ('Fri',1)
INSERT INTO CU_PR_HolidayInfo(Name,Active) Values ('Sat',1)
INSERT INTO CU_PR_HolidayInfo(Name,Active) Values ('Public Holiday',1)


CREATE TABLE [dbo].[CU_PR_Brand]
(
[Id] INT IDENTITY(1,1) PRIMARY KEY,
[Cu_Price_Id] INT NOT NULL,
[Brand_Id] INT,
[Active] BIT,
[CreatedOn] DATETIME,
[CreatedBy] INT,
[UpdatedOn] DATETIME,
[UpdatedBy] INT,
[DeletedOn] DATETIME,
[DeletedBy] INT,
FOREIGN KEY([Brand_Id]) REFERENCES [dbo].[CU_Brand](Id),
FOREIGN KEY([Cu_Price_Id]) REFERENCES [dbo].[CU_PR_Details](Id),
FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)


CREATE TABLE [dbo].[CU_PR_Buyer]
(
[Id] INT IDENTITY(1,1) PRIMARY KEY,
[Cu_Price_Id] INT NOT NULL,
[Buyer_Id] INT,
[Active] BIT,
[CreatedOn] DATETIME,
[CreatedBy] INT,
[UpdatedOn] DATETIME,
[UpdatedBy] INT,
[DeletedOn] DATETIME,
[DeletedBy] INT,
FOREIGN KEY([Buyer_Id]) REFERENCES [dbo].[CU_Buyer](Id),
FOREIGN KEY([Cu_Price_Id]) REFERENCES [dbo].[CU_PR_Details](Id),
FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)

CREATE TABLE [dbo].[CU_PR_Department]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Cu_Price_Id] INT NOT NULL,
	[Department_Id] INT,
	[Active] BIT,
	[CreatedOn] DATETIME,
	[CreatedBy] INT,
	[UpdatedOn] DATETIME,
	[UpdatedBy] INT,
	[DeletedOn] DATETIME,
	[DeletedBy] INT,
	FOREIGN KEY([Department_Id]) REFERENCES [dbo].[CU_Department](Id),
	FOREIGN KEY([Cu_Price_Id]) REFERENCES [dbo].[CU_PR_Details](Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)




CREATE TABLE [dbo].[CU_PR_PriceCategory]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Cu_Price_Id] INT NOT NULL,
	[PriceCategory_Id] INT,
	[Active] BIT,
	[CreatedOn] DATETIME,
	[CreatedBy] INT,
	[UpdatedOn] DATETIME,
	[UpdatedBy] INT,
	[DeletedOn] DATETIME,
	[DeletedBy] INT,
	FOREIGN KEY([PriceCategory_Id]) REFERENCES [dbo].[CU_PriceCategory](Id),
	FOREIGN KEY([Cu_Price_Id]) REFERENCES [dbo].[CU_PR_Details](Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)



CREATE TABLE [dbo].[CU_PR_HolidayType]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Cu_Price_Id] INT NOT NULL,
	[HolidayInfo_Id] INT,
	[Active] BIT,
	[CreatedOn] DATETIME,
	[CreatedBy] INT,
	[UpdatedOn] DATETIME,
	[UpdatedBy] INT,
	[DeletedOn] DATETIME,
	[DeletedBy] INT,
	FOREIGN KEY([HolidayInfo_Id]) REFERENCES [dbo].[CU_PR_HolidayInfo](Id),
	FOREIGN KEY([Cu_Price_Id]) REFERENCES [dbo].[CU_PR_Details](Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)


CREATE TABLE [dbo].[CU_PR_Sampling]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Cu_Price_Id] INT NOT NULL,
	[MaxProductCount] INT NULL,
	[SampleSizeBySet] BIT NULL,
	[MinBillingDay] FLOAT NULL,
	[MaxSampleSize] INT NULL,
	[AdditionalSampleSize] INT NULL,
	[AdditionalSamplePrice] FLOAT NULL,
	[Quantity8] FLOAT NULL,
	[Quantity13] FLOAT NULL,
	[Quantity20] FLOAT NULL,
	[Quantity32] FLOAT NULL,
	[Quantity50] FLOAT NULL,
	[Quantity80] FLOAT NULL,
	[Quantity125] FLOAT NULL,
	[Quantity200] FLOAT NULL,
	[Quantity315] FLOAT NULL,
	[Quantity500] FLOAT NULL,
	[Quantity800] FLOAT NULL,
	[Quantity1250] FLOAT NULL,
	[CreatedOn] DATETIME,
	[CreatedBy] INT,
	[UpdatedOn] DATETIME,
	[UpdatedBy] INT,
	[DeletedOn] DATETIME,
	[DeletedBy] INT,
	[Active] BIT,
	FOREIGN KEY([Cu_Price_Id]) REFERENCES [dbo].[CU_PR_Details](Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)



ALTER TABLE [CU_PR_Details] ADD HolidayPrice FLOAT NULL


--Price Card Details Config Scripts Ends--


--------------menu starts statistics starts ------------------
declare @IdItem as int
INSERT [dbo].[IT_Right] ( [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], 
[MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (NULL, N'statistics', NULL, NULL, 1, 1, N'fa fa-search', 2, 
NULL, 2, NULL, 1, NULL)

SELECT @IdItem = SCOPE_IDENTITY()

INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS

INSERT [dbo].[IT_Right] ( [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking],
[MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (87, N'Manday Dashboard', N'Manday Dashboard', NULL,
1, 1, N'fa fa-sign-in',
3, NULL, 3, NULL, 1, 2)

SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS
--------------menu starts statistics ends

--Price Card Details Config Product Price Starts--
ALTER TABLE [CU_PR_Details] ADD [ProductPrice] FLOAT NULL
ALTER TABLE [CU_PR_Details] ADD [PriceToEachProduct] BIT NULL
--Price Card Details Config Product Price Ends--


-- FB Request log start here 

CREATE TABLE [dbo].[FB_Booking_RequestLog]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[BookingId] INT NULL,
	[MissionId] INT NULL,
	[ReportId] INT NULL,
	[RequestUrl] NVARCHAR(1000) NULL,
	[MissionProductId] INT NULL,
	[LogInformation] NVARCHAR(MAX) NULL,      
	[CreatedBy] INT NULL,
	[CreatedOn] DATETIME NULL         
)
-- remove old table fb log
DROP TABLE FB_Log

-- FB Request log end here 

CREATE TABLE [dbo].[CU_PriceCategory_PCSub2]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	 CustomerId int not null,
	ProductSubCategoryId2 int not null,
	PriceCategoryId int not null,
	Active bit, 
	CreatedOn datetime default getdate(),
	CONSTRAINT CU_PriceCategory_PCSub2_CustomerId  FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Cu_Customer](Id),
	CONSTRAINT CU_PriceCategory_PCSub2_ProductSubCategoryId2 FOREIGN KEY ([ProductSubCategoryId2]) REFERENCES [dbo].[REF_ProductCategory_Sub2](Id),
	CONSTRAINT CU_PriceCategory_PCSub2_PriceCategoryId FOREIGN KEY ([PriceCategoryId]) REFERENCES [dbo].[CU_PriceCategory](Id),
)
-- [CU_PriceCategory_PCSub2] ends --


ALTER TABLE KPI_TemplateColumn
ADD FilterLazy BIT

--REF_KPI_Teamplate starts--
--GIFI customerid is 64
insert into REF_KPI_Teamplate values('GIFITemplate', 64,1)

--REF_KPI_Teamplate ends--

-----------------------------------------------------------------------------
--FB - Report Updates start here
----------------------------------------------------------------------------

ALTER TABLE FB_Report_Details ADD ProductionStatus FLOAT NULL

ALTER TABLE FB_Report_Details ADD PackingStatus FLOAT NULL

ALTER TABLE FB_Report_Quantity_Details ADD ProductionStatus FLOAT NULL

ALTER TABLE FB_Report_Quantity_Details ADD PackingStatus FLOAT NULL

ALTER TABLE FB_Report_Quantity_Details ADD TotalUnits FLOAT NULL

ALTER TABLE FB_Report_Quantity_Details ADD TotalCartons FLOAT NULL

ALTER TABLE FB_Report_Quantity_Details ADD FinishedPackedUnits FLOAT NULL

ALTER TABLE FB_Report_Quantity_Details ADD FinishedUnpackedUnits FLOAT NULL

ALTER TABLE FB_Report_Quantity_Details ADD NotFinishedUnits FLOAT NULL

ALTER TABLE FB_Report_Quantity_Details ADD SelectCtnQty FLOAT NULL

ALTER TABLE FB_Report_InspSummary ADD Sort INT NULL

ALTER TABLE FB_Report_InspSummary ALTER COLUMN [Remarks] NVARCHAR(max) NULL;

-------------------------------------------------------------------------


CREATE TABLE [dbo].[FB_Report_Comments]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ProductId] INT NULL,
	[Comments] NVARCHAR(max) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	[FbReportId] int NOT NULL,
	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FK_Report_Comments_ProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
)

CREATE TABLE [dbo].[FB_Report_PackingInfo]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
	[ProductId] int NULL,
	[MaterialType] NVARCHAR(2000) NULL,
	[PackagingDesc] NVARCHAR(max) NULL,
	[PieceNo] FLOAT NULL,
	[Quantity] NVARCHAR(1000) NULL,
	[Location] NVARCHAR(2000) NULL,
	[NetWeightPerQty] NVARCHAR(1000) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL	,
	[FbReportId] int NOT NULL,
	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FK_PackingInfo_ProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
)


CREATE TABLE [dbo].[FB_Report_Packing_BatteryInfo]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
	[ProductId] int NULL,
	[BatteryType] NVARCHAR(2000) NULL,
	[BatteryModel] NVARCHAR(2000) NULL,
	[Quantity] NVARCHAR(1000) NULL,
	[Location] NVARCHAR(2000) NULL,
	[NetWeightPerQty] NVARCHAR(1000) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL	,
	[FbReportId] int NOT NULL,
	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FK_Packing_BatteryInfo_ProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
)

CREATE TABLE [dbo].[FB_Report_Packing_Dimention]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
	[ProductId] int NULL,	
	[PackingType] NVARCHAR(2000) NULL,
	[SpecClientValuesL] NVARCHAR(2000) NULL,
	[SpecClientValuesW] NVARCHAR(2000) NULL,
	[SpecClientValuesH] NVARCHAR(2000) NULL,
	[PrintedPackValuesL] NVARCHAR(2000) NULL,
	[PrintedPackValuesW] NVARCHAR(2000) NULL,
	[PrintedPackValuesH] NVARCHAR(2000) NULL,
	[Tolerance] NVARCHAR(2000) NULL,
	[NoPcs] FLOAT NULL,
	[MeasuredValuesL] NVARCHAR(2000) NULL,
	[MeasuredValuesW] NVARCHAR(2000) NULL,
	[MeasuredValuesH] NVARCHAR(2000) NULL,
	[DiscrepancyToSpec] NVARCHAR(1000) NULL,
	[DiscrepancyToPacking] NVARCHAR(1000) NULL,
	[Result]  NVARCHAR(2000) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	[FbReportId] int NOT NULL,
	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FK_Packing_Dimention_ProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
	
)


CREATE TABLE [dbo].[FB_Report_Packing_Weight]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
	[ProductId] int NULL,
	[SpecClientValues] NVARCHAR(2000) NULL,
	[WeightPackValues] NVARCHAR(2000) NULL,
	[Tolerance] NVARCHAR(2000) NULL,
	[NoPcs] FLOAT NULL,
	[MeasuredValues] NVARCHAR(2000) NULL,
	[DiscrepancyToSpec] NVARCHAR(1000) NULL,
	[DiscrepancyToPacking] NVARCHAR(1000) NULL,
	[Result]  NVARCHAR(2000) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	[FbReportId] int NOT NULL,
	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FK_Packing_Weight_ProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
)


CREATE TABLE [dbo].[FB_Report_Problematic_Remarks]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[FbReportSummaryId] int NOT NULL,
	[ProductId] int NULL,
	[Remarks] NVARCHAR(max) NULL,
	[Result]  NVARCHAR(2000) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	FOREIGN KEY(FbReportSummaryId) REFERENCES [FB_Report_InspSummary](Id),
	CONSTRAINT FK_Problematic_Remarks_ProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
)

CREATE TABLE [dbo].[FB_Report_Product_Dimention]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
	[ProductId] int NULL,
	[SpecClientValuesL] NVARCHAR(2000) NULL,
	[SpecClientValuesW] NVARCHAR(2000) NULL,
	[SpecClientValuesH] NVARCHAR(2000) NULL,
	[DimensionPackValuesL] NVARCHAR(2000) NULL,
	[DimensionPackValuesW] NVARCHAR(2000) NULL,
	[DimensionPackValuesH] NVARCHAR(2000) NULL,
	[Tolerance] NVARCHAR(2000) NULL,
	[NoPcs] FLOAT NULL,
	[MeasuredValuesL] NVARCHAR(2000) NULL,
	[MeasuredValuesW] NVARCHAR(2000) NULL,
	[MeasuredValuesH] NVARCHAR(2000) NULL,
	[DiscrepancyToSpec] NVARCHAR(1000) NULL,
	[DiscrepancyToPack]  NVARCHAR(1000) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	[FbReportId] int NOT NULL,
	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FK_Product_Dimention_ProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
	
)


CREATE TABLE [dbo].[FB_Report_Product_Weight]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
	[ProductId] int NULL,
	[SpecClientValues] NVARCHAR(2000) NULL,
	[WeightPackValues] NVARCHAR(2000) NULL,
	[Tolerance] NVARCHAR(2000) NULL,
	[NoPcs] FLOAT NULL,
	[MeasuredValues] NVARCHAR(2000) NULL,
	[DiscrepancyToSpec] NVARCHAR(1000) NULL,
	[DiscrepancyToPack] NVARCHAR(1000) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	[FbReportId] int NOT NULL,
	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FK_Product_Weight_ProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
)


CREATE TABLE [dbo].[FB_Report_Sample_Pickings]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
	[ProductId] int NULL,
	[SampleType] NVARCHAR(2000) NULL,
	[Destination] NVARCHAR(2000) NULL,
	[Quantity] NVARCHAR(1000) NULL,
	[Comments] NVARCHAR(max) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	[FbReportId] int NOT NULL,
	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FK_Sample_Pickings_ProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)

)


CREATE TABLE [dbo].[FB_Report_OtherInformation]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
	[ProductId] int NULL,
	[SubCategory] NVARCHAR(1000) NULL,	
	[Remarks] NVARCHAR(max) NULL,
	[Result]  NVARCHAR(2000) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL	,
	[FbReportId] int NOT NULL,
	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FK_OtherInformation_ProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
)

CREATE TABLE [dbo].[FB_Report_Reviewer]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
	[ReviewerId] INT NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	[FbReportId] int NOT NULL,
	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id)
)

CREATE TABLE [dbo].[FB_Report_SampleTypes]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
	[ProductId] int NULL,
	[SampleType] NVARCHAR(1000) NULL,
	[Description] NVARCHAR(max) NULL,
	[Quantity] NVARCHAR(1000) NULL,
	[Comments] NVARCHAR(max) NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL	,
	[FbReportId] int NOT NULL,
	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FK_SampleTypeProductId
    FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
)

CREATE TABLE [dbo].[FB_Report_InspSub_Summary]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[FbReportSummaryId] int NOT NULL,	
	[Name] NVARCHAR(1000) NULL, 
	[Result] NVARCHAR(1000) NULL, 
	[ResultId] INT  NULL, 
	[Sort] INT  NULL, 
	[Remarks] NVARCHAR(MAX) NULL, 
	[CreatedOn] DATETIME NULL,
	[DeletedOn] DATETIME NULL, 
	[Active] BIT NULL,
	FOREIGN KEY(FbReportSummaryId) REFERENCES [FB_Report_InspSummary](Id),
	FOREIGN KEY ([ResultId]) REFERENCES [dbo].[FB_Report_Result](Id)
)


-----------------------------------------------------------------------------
  --FB - Report Updates end here
----------------------------------------------------------------------------


-------FB Report Result  -------------

INSERT INTO FB_Report_Result (ResultName, Active)

VALUES ('notapplicable', 1),
('Missing', 1)

-------FB Report Result  -------------

---Zoho Request Log Table start----

CREATE TABLE [dbo].ZOHO_RequestLog
(
ID INT IDENTITY(1,1) PRIMARY KEY,
CustomerID BIGINT,
RequestUrl NVARCHAR(500),
LogInformation NVARCHAR(MAX),
CreatedBy INT,
CreatedOn DATETIME
)

---Zoho Request Log Table end----

--REF_KPI_Teamplate starts--
--ESI customerid is 80
insert into REF_KPI_Teamplate values('ECIRemarkTemplate', 80,1)

--REF_KPI_Teamplate ends--
-------FB Report Result  -------------


-- Custom AQL values start -- 

CREATE TABLE [dbo].[REF_SampleType]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[SampleType] NVARCHAR(100) NULL,
	[SampleSize] NVARCHAR(100) NULL,	
	[Active] BIT NULL
)
insert into REF_LevelPick1(Value,Active)VALUES('Custom',1)
INSERT INTO [dbo].[REF_SampleType] (SampleType,SampleSize,Active)VALUES

('1pcs','1',1),

('2pcs','2',1),

('3pcs','3',1),

('5pcs','5',1),

('100%','',1),

('Other','',1)

ALTER TABLE INSP_Product_Transaction  ADD SampleType INT NULL FOREIGN KEY (SampleType) REFERENCES REF_SampleType(Id);

-- Custom AQL values end -- 

--da alter query start--
ALTER TABLE DA_UserCustomer   
DROP CONSTRAINT FK_DAUserCustomer_DeletedBy;

ALTER TABLE DA_UserCustomer
DROP COLUMN Active;

ALTER TABLE DA_UserCustomer
DROP COLUMN DeletedBy;

ALTER TABLE DA_UserCustomer
DROP COLUMN DeletedOn;

ALTER TABLE DA_UserRoleNotificationByOffice   
DROP CONSTRAINT FK_DAUserRoleNotificationByOffice_DeletedBy;

ALTER TABLE DA_UserRoleNotificationByOffice   
DROP CONSTRAINT FK_DAUserRoleNotificationByOffice_RoleId;

ALTER TABLE DA_UserRoleNotificationByOffice   
DROP CONSTRAINT FK_DAUserRoleNotificationByOffice_ServiceId;

ALTER TABLE DA_UserRoleNotificationByOffice   
DROP CONSTRAINT FK_DAUserRoleNotificationByOffice_ProductCategoryId;

ALTER TABLE DA_UserRoleNotificationByOffice
DROP COLUMN RoleId;

ALTER TABLE DA_UserRoleNotificationByOffice
DROP COLUMN ServiceId;

ALTER TABLE DA_UserRoleNotificationByOffice
DROP COLUMN ProductCategoryId;

ALTER TABLE DA_UserRoleNotificationByOffice
DROP COLUMN Active;

ALTER TABLE DA_UserRoleNotificationByOffice
DROP COLUMN DeletedBy;

ALTER TABLE DA_UserRoleNotificationByOffice
DROP COLUMN DeletedOn;

--da alter query ends


--da create query starts--
CREATE TABLE [dbo].[DA_UserByProductCategory]
(
[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),	
[DAUserCustomerId] INT NOT NULL,
[ProductCategoryId] INT NULL,
[CreatedBy] INT NULL,
[CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(),
CONSTRAINT FK_DAUserByProductCategory_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_DAUserByProductCategory_ProductCategoryId FOREIGN KEY ([ProductCategoryId]) REFERENCES [dbo].[REF_ProductCategory](Id),
CONSTRAINT FK_DAUserByProductCategory_DAUserCustomerId FOREIGN KEY ([DAUserCustomerId]) REFERENCES [dbo].[DA_UserCustomer](Id)
)


CREATE TABLE [dbo].[DA_UserByRole]
(
[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),	
[DAUserCustomerId] INT NOT NULL,
[RoleId] INT NOT NULL, 
[CreatedBy] INT NULL,
[CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(),
CONSTRAINT FK_DAUserByRole_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_DAUserByRole_RoleId FOREIGN KEY ([RoleId]) REFERENCES [dbo].[IT_Role](Id),
CONSTRAINT FK_DAUserByRole_DAUserCustomerId FOREIGN KEY ([DAUserCustomerId]) REFERENCES [dbo].[DA_UserCustomer](Id)
)

CREATE TABLE [dbo].[DA_UserByService]
(
[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),	
[DAUserCustomerId] INT NOT NULL,
[ServiceId] INT NULL,
[CreatedBy] INT NULL,
[CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(),
CONSTRAINT FK_DAUserByService_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_DAUserByService_ServiceId FOREIGN KEY ([ServiceId]) REFERENCES [dbo].[REF_Service](Id),
CONSTRAINT FK_DAUserByService_DAUserCustomerId FOREIGN KEY ([DAUserCustomerId]) REFERENCES [dbo].[DA_UserCustomer](Id)
)

CREATE TABLE [dbo].[DA_UserByBrand]
(
[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),	
[DAUserCustomerId] INT NOT NULL,
[BrandId] INT NULL, 
[CreatedBy] INT NULL,
[CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(),
CONSTRAINT FK_DAUserByBrand_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_DAUserByBrand_BrandId FOREIGN KEY ([BrandId]) REFERENCES [dbo].[CU_Brand](Id),
CONSTRAINT FK_DAUserByBrand_DAUserCustomerId FOREIGN KEY ([DAUserCustomerId]) REFERENCES [dbo].[DA_UserCustomer](Id)
)

CREATE TABLE [dbo].[DA_UserByBuyer]
(
[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),	
[DAUserCustomerId] INT NOT NULL,
[BuyerId] INT NULL, 
[CreatedBy] INT NULL,
[CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(),
CONSTRAINT FK_DAUserByBuyer_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_DAUserByBuyer_BuyerId FOREIGN KEY ([BuyerId]) REFERENCES [dbo].[CU_Buyer](Id),
CONSTRAINT FK_DAUserByBuyer_DAUserCustomerId FOREIGN KEY ([DAUserCustomerId]) REFERENCES [dbo].[DA_UserCustomer](Id)
)

CREATE TABLE [dbo].[DA_UserByDepartment]
(
[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),	
[DAUserCustomerId] INT NOT NULL,
[DepartmentId] INT NULL, 
[CreatedBy] INT NULL,
[CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(),
CONSTRAINT FK_DAUserByDepartment_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_DAUserByDepartment_DepartmentId FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[CU_Department](Id),
CONSTRAINT FK_DAUserByDepartment_DAUserCustomerId FOREIGN KEY ([DAUserCustomerId]) REFERENCES [dbo].[DA_UserCustomer](Id)
)



--da create query ends--

-- Custom AQL values end -- 

CREATE TABLE [dbo].[CU_Product_File_Attachment]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[UniqueId] nvarchar(1000) null,
	[Product_Id] [int] NOT NULL,
	[FileName] [nvarchar](500) NOT NULL,	
	[FileUrl] nvarchar(max) null,
	[UserId] [int] NOT NULL,
	[UploadDate] [datetime] NOT NULL,
	[Active] [bit] NOT NULL, 
    [Booking_Id] INT NULL, 
    [DeletedBy] INT NULL, 	
    [DeletedOn] DATETIME NULL, 
	FOREIGN KEY ([Product_Id]) REFERENCES [dbo].[CU_Products](Id),
	FOREIGN KEY ([UserId]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([Booking_Id]) REFERENCES [dbo].[INSP_TRANSACTION](Id)
)

-- file upload update drop this table and create again
CREATE TABLE [dbo].[INSP_TRAN_File_Attachment]
(
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UniqueId] nvarchar(1000) null,
    [Inspection_Id] INT NOT NULL, 
    [FileName] NVARCHAR(500) NOT NULL, 
    [FileUrl] nvarchar(max) null,
    [UserId] INT NOT NULL, 
    [UploadDate] DATETIME NOT NULL,
	[Active] BIT NOT NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
    FOREIGN KEY ([Inspection_Id]) REFERENCES [dbo].[INSP_Transaction](Id),
	FOREIGN KEY ([UserId]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)
-- Custom AQL values end -- 
-------FB Report Result  -------------


--- API Services Configuration for master data starts---
CREATE TABLE [dbo].[REF_API_Services]
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(100)
)

INSERT INTO [REF_API_Services](Name) Values ('Audit')
INSERT INTO [REF_API_Services](Name) Values ('Inspection')
INSERT INTO [REF_API_Services](Name) Values ('TCF')
INSERT INTO [REF_API_Services](Name) Values ('Lab Testing')


ALTER TABLE  IT_UserMaster ADD AuditAccess BIT NULL
ALTER TABLE  IT_UserMaster ADD InspectionAccess BIT NULL
ALTER TABLE  IT_UserMaster ADD TCFAccess BIT NULL
ALTER TABLE  IT_UserMaster ADD LabAccess BIT NULL
ALTER TABLE  IT_UserMaster ADD TCFUserID INT NULL


CREATE TABLE [dbo].[CU_API_Services]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[CustomerId] INT,
	[ServiceId] INT,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL,
	[Active] BIT,
	FOREIGN KEY([CreatedBy]) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY([DeletedBy]) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY([CustomerId]) REFERENCES [CU_Customer](Id),
	CONSTRAINT FK_CU_API_Service FOREIGN KEY (ServiceId)  REFERENCES [dbo].[REF_Service](Id)
)

CREATE TABLE [dbo].[SU_API_Services]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[SupplierId] INT,
	[ServiceId] INT,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL,
	[Active] BIT,
	FOREIGN KEY([CreatedBy]) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY([DeletedBy]) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY([SupplierId]) REFERENCES [SU_Supplier](Id),
	CONSTRAINT FK_SU_API_Service FOREIGN KEY (ServiceId)  REFERENCES [dbo].[REF_Service](Id)
)

CREATE TABLE [dbo].[CU_Buyer_API_Services]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[BuyerId] INT NOT NULL,
	[ServiceId] INT NOT NULL,
	[Active] BIT NOT NULL,
	[CreatedBy] INT NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedBy] INT NULL,
	[DeletedOn] DATETIME NULL,
	FOREIGN KEY([BuyerId]) REFERENCES [dbo].[CU_Buyer](Id),
	CONSTRAINT FK_CU_Buyer_API_Service FOREIGN KEY (ServiceId)  REFERENCES [dbo].[REF_Service](Id),
	FOREIGN KEY(CreatedBy) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY(DeletedBy) REFERENCES [IT_UserMaster](Id)
)


CREATE TABLE [dbo].[CU_Product_API_Services]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[ProductId] INT NOT NULL,
	[ServiceId] INT NOT NULL,
	[Active] BIT NOT NULL,
	[CreatedBy] INT NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedBy] INT NULL,
	[DeletedOn] DATETIME NULL,
	FOREIGN KEY([ProductId]) REFERENCES [dbo].[CU_Products](Id),
	CONSTRAINT FK_CU_Product_API_Service FOREIGN KEY (ServiceId)  REFERENCES [dbo].[REF_Service](Id),
	FOREIGN KEY(CreatedBy) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY(DeletedBy) REFERENCES [IT_UserMaster](Id)
)

CREATE TABLE [dbo].[SU_Contact_API_Services]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[ContactId] INT NOT NULL,
	[ServiceId] INT NOT NULL,
	[Active] BIT NOT NULL,
	[CreatedBy] INT NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedBy] INT NULL,
	[DeletedOn] DATETIME NULL,
	FOREIGN KEY([ContactId]) REFERENCES [dbo].[SU_Contact](Id),
	CONSTRAINT FK_SU_Contact_API_Service FOREIGN KEY (ServiceId)  REFERENCES [dbo].[REF_Service](Id),
	FOREIGN KEY(CreatedBy) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY(DeletedBy) REFERENCES [IT_UserMaster](Id)
)



INSERT INTO MID_TaskType(Id,Label,EntityId) Values (15,'UpdateCustomerToFB',1)
INSERT INTO MID_TaskType(Id,Label,EntityId) Values (16,'UpdateSupplierToFB',1)
INSERT INTO MID_TaskType(Id,Label,EntityId) Values (17,'UpdateFactoryToFB',1)
INSERT INTO MID_TaskType(Id,Label,EntityId) Values (18,'UpdateProductToFB',1)

--Configuration Data in appSettings--
  --"AccountQueue": "AccountProcessQueue",
  --"ITUserList": "2,5",

--- API Services Configuration for master data ends---

declare @IdItem as int

--menu for utilization start--

INSERT [dbo].[IT_Right] ([ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking],
[MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES ( 87, N'Manday Utilization Dashboard', N'Manday Utilization Dashboard', NULL,
1, 1, N'fa fa-sign-in', 3, NULL, 3, NULL, 1, 2)

SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team
		  ,(1, @IdItem) -- CS
GO

--menu for utilization end--

--Invoice Bank Module start 

CREATE TABLE [dbo].[INV_REF_Bank]
(
	[Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[AccountName] [nvarchar](500) NULL,
	[AccountNumber] [nvarchar](500) NULL,
	[BankName] [nvarchar](500) NULL,
	[SwiftCode] [nvarchar](500) NULL,
	[BankAddress] [nvarchar](1000) NULL,
	[AccountCurrency] [int] NULL,
	[Remarks] [nvarchar](2000) NULL,
	[Active] [bit] NULL,

	[ChopFileUniqueId] [nvarchar](255) NULL,
	[SignatureFileUniqueId] [nvarchar](255) NULL,

	[ChopFilename] [nvarchar](255) NULL,
	[SignatureFilename] [nvarchar](255) NULL,

	[ChopFileUrl] [nvarchar](max) NULL, 
	[SignatureFileUrl] [nvarchar](max) NULL, 

	[CreatedBy] [INT] NULL, 
    [CreatedOn] [DATETIME] NULL, 

    [UpdatedBy] [INT] NULL, 
    [UpdatedOn] [DATETIME] NULL,   

	[DeletedBy] [INT] NULL, 
    [DeletedOn] [DATETIME] NULL,  

	[EntityId] [INT] NULL, 

	CONSTRAINT FK_INV_Bank_AccountCurrency   FOREIGN KEY ([AccountCurrency]) REFERENCES [dbo].[REF_Currency](Id),
	CONSTRAINT FK_INV_Bank_CreatedBy	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_INV_Bank_UpdatedBy	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_INV_Bank_DeletedBy	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)


CREATE TABLE [dbo].[INV_TRAN_Bank_Tax]
(
	[Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Tax_name] [nvarchar](500) NOT NULL,
	[Tax_Value] [decimal](18,2) NOT NULL, 
	[Active] [bit] NOT NULL,
	[BankId] [int] NOT NULL,
	[From_Date] [DATETIME] NOT NULL,
	[To_Date] [DATETIME] NULL,

	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 

    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
	
	[DeletedBy] [INT] NULL, 
    [DeletedOn] [DATETIME] NULL,  
	
	CONSTRAINT FK_INV_Bank_Tax_BankId	FOREIGN KEY ([BankId]) REFERENCES [dbo].[INV_REF_Bank](Id),
	CONSTRAINT FK_INV_Bank_Tax_CreatedBy	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_INV_Bank_Tax_UpdatedBy	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_INV_Bank_Tax_DeletedBy	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)

)


declare @IdTran as int  ,@ParentIdLvl2 as int
INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Bank Account', NULL,11)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[IT_Right] ([ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], TitleName_IdTran)
VALUES (19, N'Bank Account', 'Bank Account', 'invoicebank/invoice-bank-summary', 0, 1, N'fa fa-sign-in', 8,@IdTran)
SELECT @ParentIdLvl2 = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @ParentIdLvl2) -- IT-Team


ALTER TABLE  INV_REF_Bank ADD BillingEntity INT NULL
CONSTRAINT FK_INV_Bank_BillingEntity   FOREIGN KEY ([BillingEntity]) REFERENCES [dbo].[REF_Billing_Entity](Id)

--Invoice Bank Module end 


-- Price card updates start here --
	
	ALTER TABLE CU_PR_Details ADD InvoiceRequestSelectAll BIT NULL

	ALTER TABLE CU_PR_Details ADD IsInvoiceConfigured BIT NULL
	
	ALTER TABLE CU_PR_Details ADD InvoiceRequestType INT NULL
	CONSTRAINT CU_PR_Details_InvoiceRequestType FOREIGN KEY ([InvoiceRequestType]) REFERENCES [dbo].[INV_REF_Request_Type](Id)

	ALTER TABLE CU_PR_Details ADD InvoiceRequestAddress NVARCHAR(max) NULL

	ALTER TABLE CU_PR_Details ADD InvoiceRequestBilledName NVARCHAR(2000) NULL

	ALTER TABLE CU_PR_Details ADD BillingEntity INT NULL
	CONSTRAINT CU_PR_Details_BillingEntity FOREIGN KEY ([BillingEntity]) REFERENCES [dbo].[REF_Billing_Entity](Id)

	ALTER TABLE CU_PR_Details ADD BankAccount INT NULL
	CONSTRAINT CU_PR_Details_BankAccount FOREIGN KEY ([BankAccount]) REFERENCES [dbo].[INV_REF_Bank](Id)

    ALTER TABLE CU_PR_Details ADD PaymentDuration INT NULL

	ALTER TABLE CU_PR_Details ADD PaymentTerms NVARCHAR(100) null

	ALTER TABLE CU_PR_Details ADD InvoiceNoDigit NVARCHAR(100) NULL	

    ALTER TABLE CU_PR_Details ADD InvoiceNoPrefix NVARCHAR(100) NULL	

	ALTER TABLE CU_PR_Details ADD InvoiceInspFeeFrom INT NULL
	CONSTRAINT CU_PR_Details_InvoiceTMFeeFrom FOREIGN KEY ([InvoiceInspFeeFrom]) REFERENCES [dbo].[INV_REF_Fees_From](Id)

	ALTER TABLE CU_PR_Details ADD InvoiceTMFeeFrom  INT NULL
	CONSTRAINT CU_PR_Details_InvoiceTravleExpenseFrom FOREIGN KEY ([InvoiceTMFeeFrom]) REFERENCES [dbo].[INV_REF_Fees_From](Id)

	ALTER TABLE CU_PR_Details ADD InvoiceHotelFeeFrom  INT NULL
	CONSTRAINT CU_PR_Details_InvoiceHotelFeeFrom FOREIGN KEY ([InvoiceHotelFeeFrom]) REFERENCES [dbo].[INV_REF_Fees_From](Id)

	ALTER TABLE CU_PR_Details ADD  InvoiceOtherFeeFrom  INT NULL
	CONSTRAINT CU_PR_Details_InvoiceOtherFeeFrom FOREIGN KEY ([InvoiceOtherFeeFrom]) REFERENCES [dbo].[INV_REF_Fees_From](Id)

	ALTER TABLE CU_PR_Details ADD  InvoiceDiscountFeeFrom INT NULL
	CONSTRAINT CU_PR_Details_InvoiceDiscountFeeFrom FOREIGN KEY ([InvoiceDiscountFeeFrom]) REFERENCES [dbo].[INV_REF_Fees_From](Id)

	ALTER TABLE CU_PR_Details ADD InvoiceOffice INT NULL
    CONSTRAINT CU_PR_Details_InvoiceOffice FOREIGN KEY ([InvoiceOffice]) REFERENCES [dbo].[INV_REF_Office](Id)



	CREATE TABLE INV_REF_Request_Type
	(
		[Id] INT IDENTITY(1,1) PRIMARY KEY,
		[Name] NVARCHAR(100),
		[Active]  Bit
	)

   CREATE TABLE INV_TRAN_Invoice_Request
   (
      [Id] INT IDENTITY(1,1) PRIMARY KEY,	  
	  [CuPriceCardId] INT NULL,
	  [BilledName] NVARCHAR(100),
	  [BilledAddress] NVARCHAR(max),
	  [DepartmentId] INT NULL,
	  [BrandId] INT NULL,
	  [BuyerId] INT NULL,
	  [Active] BIT,
	  [CreatedBy] INT NULL, 
      [CreatedOn] DATETIME NULL, 
      [UpdatedBy] INT NULL, 
      [UpdatedOn] DATETIME NULL, 	
	  [DeletedBy] [INT] NULL, 
      [DeletedOn] [DATETIME] NULL,

	  CONSTRAINT INV_TRAN_Invoice_Request_CuPriceCardId FOREIGN KEY ([CuPriceCardId]) REFERENCES [dbo].[CU_PR_Details](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_DepartmentId FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[CU_Department](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_BrandId FOREIGN KEY ([BrandId]) REFERENCES [dbo].[CU_Brand](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_BuyerId FOREIGN KEY ([BuyerId]) REFERENCES [dbo].[CU_Buyer](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_CreatedBy	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_UpdatedBy	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_DeletedBy	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
   )

   CREATE TABLE INV_TRAN_Invoice_Request_Contact
   (
      [Id] INT IDENTITY(1,1) PRIMARY KEY,	  
	  [CuPriceCardId] INT NULL,
	  [InvoiceRequestId] INT NULL,	 
	  [ContactId] INT NULL,
	  [IsCommon] BIT,
	  [Active] BIT,
	  [CreatedBy] INT NULL, 
      [CreatedOn] DATETIME NULL, 
      [UpdatedBy] INT NULL, 
      [UpdatedOn] DATETIME NULL, 	
	  [DeletedBy] [INT] NULL, 
      [DeletedOn] [DATETIME] NULL,

	  CONSTRAINT INV_TRAN_Invoice_Request_Contact_CuPriceCardId FOREIGN KEY ([CuPriceCardId]) REFERENCES [dbo].[CU_PR_Details](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_Contact_InvoiceRequestId FOREIGN KEY ([InvoiceRequestId]) REFERENCES [dbo].[INV_TRAN_Invoice_Request](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_Contact_ContactId FOREIGN KEY ([ContactId]) REFERENCES [dbo].[CU_Contact](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_Contact_CreatedBy	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_Contact_UpdatedBy	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),

	  CONSTRAINT INV_TRAN_Invoice_Request_Contact_DeletedBy	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
   )

   CREATE TABLE INV_REF_PaymentTerms
   (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
		[Name] NVARCHAR(1000),
		[Duration] INT NULL,
		[Active]  Bit
   )

  CREATE TABLE INV_REF_Fees_From
  (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
		[Name] NVARCHAR(1000),
		[Active]  Bit  
  )

  CREATE TABLE INV_REF_Office
	(
		[Id] INT IDENTITY(1,1) PRIMARY KEY,
		[Name] NVARCHAR(100),		
		[Address] NVARCHAR(max),
		[Phone] NVARCHAR(100),
		[Fax] NVARCHAR(100),
		[Website] NVARCHAR(100),
		[Mail] NVARCHAR(100),
		[Active]  Bit
	)


	INSERT INTO [dbo].[INV_REF_Request_Type] ([Name], [Active])
    VALUES ('Brand', 1),
		   ('Department', 1),
		   ('Buyer', 1),
		   ('N/A', 1)
	

	select * from [dbo].[INV_REF_Request_Type]

   INSERT INTO	[dbo].[INV_REF_PaymentTerms] ([Name], [Duration],[Active])
   VALUES  ('Payment is due 60 days from receipt of invoice',60, 1),
        	('Payment is due 30 days from receipt of invoice',30, 1)

	select * from [dbo].[INV_REF_PaymentTerms]


	INSERT INTO [dbo].[INV_REF_Fees_From]([Name],[Active])
	VALUES ('Invoice', 1),
	 ('Quotation', 1),	
	 ('Carrefour ', 1),
     ('N/A ', 1)

  select * from [dbo].[INV_REF_Fees_From]


  	INSERT INTO [dbo].[INV_REF_Office]([Name],[Address],[Phone],[Mail],[Website],[Active])
	VALUES ('SGT Office','China','901929929','test@gmail.com','https://sgtlink.net/', 1),
	 ('API Office', 'China','901929929','test@gmail.com','https://sgtlink.net/', 1),
	 ('Carrefour Office ','China','901929929','test@gmail.com','https://sgtlink.net/', 1)
    

  

-- Price card updates end here -- 
--Invoice Bank Module end 

-------------Account Id in FB log table-----------
ALTER TABLE FB_Booking_RequestLog ADD AccountId INT
-------------Account Id in FB log table-----------

-----eventlog-----
ALTER TABLE EventBookingLog
ADD Quotation_Id INT

---------------Display Product Scripts added--------------

ALTER TABLE INSP_Product_Transaction DROP COLUMN IsDisplay

ALTER TABLE INSP_Product_Transaction ADD IsDisplayMaster BIT NULL

ALTER TABLE  INSP_Product_Transaction ADD [Parent_Product_Id] INT NULL
CONSTRAINT FK_INSP_Prd_Trans_ParentProduct FOREIGN KEY ([Parent_Product_Id]) REFERENCES [dbo].[CU_Products](Id)

---------------Display Product Scripts ends--------------
-----kpi ----------
alter table KPI_TemplateColumn alter column idcolumn int null 
alter table KPI_TemplateColumn Add  Valuecolumn NVARCHAR(300) NULL 

-------------Account Id in FB log table-----------


ALTER TABLE REF_Province ADD Longitude decimal(12, 9) NULL

ALTER TABLE REF_Province ADD Latitude decimal(12, 9) NULL

---------------------------------------------------------------------

----------IsForeCastApplicable Starts ---------------
ALTER TABLE HR_Staff ADD IsForecastApplicable BIT NULL
----------IsForeCastApplicable ends ---------------


ALTER TABLE CU_PR_Details DROP COLUMN InspRateBySamplingProRate 

drop table  CU_PR_Sampling

ALTER TABLE CU_PR_Details ADD	[MaxProductCount] INT NULL
ALTER TABLE CU_PR_Details ADD	[SampleSizeBySet] BIT NULL
ALTER TABLE CU_PR_Details ADD	[MinBillingDay] FLOAT NULL
ALTER TABLE CU_PR_Details ADD	[MaxSampleSize] INT NULL
ALTER TABLE CU_PR_Details ADD	[AdditionalSampleSize] INT NULL
ALTER TABLE CU_PR_Details ADD	[AdditionalSamplePrice] FLOAT NULL
ALTER TABLE CU_PR_Details ADD	[Quantity8] FLOAT NULL
ALTER TABLE CU_PR_Details ADD	[Quantity13] FLOAT NULL
ALTER TABLE CU_PR_Details ADD	[Quantity20] FLOAT NULL
ALTER TABLE CU_PR_Details ADD	[Quantity32] FLOAT NULL
ALTER TABLE CU_PR_Details ADD	[Quantity50] FLOAT NULL
ALTER TABLE CU_PR_Details ADD	[Quantity80] FLOAT NULL
ALTER TABLE CU_PR_Details ADD	[Quantity125] FLOAT NULL
ALTER TABLE CU_PR_Details ADD	[Quantity200] FLOAT NULL
ALTER TABLE CU_PR_Details ADD	[Quantity315] FLOAT NULL
ALTER TABLE CU_PR_Details ADD	[Quantity500] FLOAT NULL
ALTER TABLE CU_PR_Details ADD	[Quantity800] FLOAT NULL
ALTER TABLE CU_PR_Details ADD	[Quantity1250] FLOAT NULL
----------IsForeCastApplicable ends ---------------



-- Invoice Generation start -- 



CREATE TABLE [dbo].[INV_Status]
(
        [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Name] NVARCHAR(200) NOT NULL, 
	[Active] BIT  NOT NULL
)

CREATE TABLE [dbo].[INV_Payment_Status]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Name] NVARCHAR(200) NOT NULL, 
	[Active] BIT  NOT NULL
)



CREATE TABLE [dbo].[INV_AUT_TRAN_Details]
(
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
	[InvoiceNo] nvarchar(1000),
	[InvoiceDate] DateTime null,
	[PostedDate] DateTime null,
	[UnitPrice] float null,
	[InspectionFees] float null,
	[TravelAirFees] float null,
	[TravelLandFees] float null,
	[HotelFees] float null,
	[OtherFees] float null,
	[Discount] float null,
	[TotalTaxAmount] float null,
	[TaxValue] float null,
	[TotalInvoiceFees] float null,
	[TotalSampleSize] int null,
	[PriceCardCurrency] int null,
	[InvoiceCurrency] int null,
	[ExchangeRate] float null,
	[RuleExchangeRate] float null,
	[InvoiceTo] int null,
	[InvoiceMethod] int null,
	[ManDays] float null,
	[TravelMatrixType] int null,
	[InvoicedName] nvarchar(1000) null,
	[InvoicedAddress] nvarchar(2000) null,
	[Office] int null,
	[PaymentTerms] nvarchar(2000),
	[PaymentDuration] nvarchar(1000),
	[BankId] int null,
	[IsAutomation] Bit null,
	[IsInspection] Bit null,
	[IsTravelExpense] bit null,
	[InspectionId] int null,
	[InvoiceStatus] int null,
	[InvoicePaymentStatus] int null,
	[InvoicePaymentDate] DateTime null,
	[RuleId] int null,
	[CalculateInspectionFee] int null,
	[CalculateTravelExpense] int null,
	[CalculateHotelFee] int null,
	[CalculateDiscountFee] int null,
	[CalculateOtherFee] int null,
	[Remarks] nvarchar(max) null,
	[Subject] nvarchar(max) null,
	[CreatedBy] int null,
	[CreatedOn] DateTime null,	

	CONSTRAINT INV_AUT_TRAN_Details_RuleId FOREIGN KEY ([RuleId]) REFERENCES [dbo].[CU_PR_Details](Id),

	CONSTRAINT INV_AUT_TRAN_Details_InspectionId FOREIGN KEY ([InspectionId]) REFERENCES [dbo].[INSP_Transaction](Id),

	CONSTRAINT INV_AUT_TRAN_Details_BankId FOREIGN KEY ([BankId]) REFERENCES [dbo].[INV_REF_Bank](Id),

	CONSTRAINT INV_AUT_TRAN_Details_Office FOREIGN KEY ([Office]) REFERENCES [dbo].[INV_REF_Office](Id),

	CONSTRAINT INV_AUT_TRAN_Details_TravelMatrixType FOREIGN KEY ([TravelMatrixType]) REFERENCES [dbo].[INV_TM_Type](Id),

	CONSTRAINT INV_AUT_TRAN_Details_InvoiceMethod FOREIGN KEY ([InvoiceMethod]) REFERENCES [dbo].[QU_BillMethod](Id),

	CONSTRAINT INV_AUT_TRAN_Details_InvoiceTo FOREIGN KEY ([InvoiceTo]) REFERENCES [dbo].[QU_PaidBy](Id),

	CONSTRAINT INV_AUT_TRAN_Details_InvoiceCurrency FOREIGN KEY ([InvoiceCurrency]) REFERENCES [dbo].[REF_Currency](Id),

	CONSTRAINT INV_AUT_TRAN_Details_PriceCardCurrency FOREIGN KEY ([PriceCardCurrency]) REFERENCES [dbo].[REF_Currency](Id),

	CONSTRAINT INV_AUT_TRAN_Details_InvoiceStatus FOREIGN KEY ([InvoiceStatus]) REFERENCES [dbo].[INV_Status](Id),

	CONSTRAINT INV_AUT_TRAN_Details_InvoicePaymentStatus FOREIGN KEY ([InvoicePaymentStatus]) REFERENCES [dbo].[INV_Payment_Status](Id),

	CONSTRAINT INV_AUT_TRAN_Details_CalculateInspectionFee FOREIGN KEY ([CalculateInspectionFee]) REFERENCES [dbo].[INV_REF_Fees_From](Id),

	CONSTRAINT INV_AUT_TRAN_Details_CalculateTravelExpense FOREIGN KEY ([CalculateTravelExpense]) REFERENCES [dbo].[INV_REF_Fees_From](Id),

	CONSTRAINT INV_AUT_TRAN_Details_CalculateHotelFee FOREIGN KEY ([CalculateHotelFee]) REFERENCES [dbo].[INV_REF_Fees_From](Id),

	CONSTRAINT INV_AUT_TRAN_Details_CalculateDiscountFee FOREIGN KEY ([CalculateDiscountFee]) REFERENCES [dbo].[INV_REF_Fees_From](Id),

    CONSTRAINT INV_AUT_TRAN_Details_CalculateOtherFee FOREIGN KEY ([CalculateOtherFee]) REFERENCES [dbo].[INV_REF_Fees_From](Id),

	CONSTRAINT INV_AUT_TRAN_Details_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)	

)


CREATE TABLE [dbo].[INV_AUT_TRAN_Status_Log]
(
	 [Id] INT IDENTITY(1,1) PRIMARY KEY,	 
	 [Invoice_Id] INT NULL,
	 [Inspection_Id] INT NULL, 
         [Audit_Id] INT NULL,
	 [Status_Id] INT NULL,
         [CreatedBy] INT NULL,
         [CreatedOn] INT NULL,

	 CONSTRAINT INV_AUT_TRAN_Status_Log_Invoice_Id FOREIGN KEY ([Invoice_Id]) REFERENCES [dbo].[INV_AUT_TRAN_Details](Id),

	 CONSTRAINT INV_AUT_TRAN_Status_Log_Inspection_Id FOREIGN KEY ([Inspection_Id]) REFERENCES [dbo].[INSP_Transaction](Id),

	 CONSTRAINT INV_AUT_TRAN_Status_Log_Audit_Id FOREIGN KEY ([Audit_Id]) REFERENCES [dbo].[AUD_Transaction](Id),

	 CONSTRAINT INV_AUT_TRAN_Status_Log_Status_Id FOREIGN KEY ([Status_Id]) REFERENCES [dbo].[INV_Status](Id),

	 CONSTRAINT INV_AUT_TRAN_Status_Log_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)


CREATE TABLE [dbo].[INV_AUT_TRAN_ContactDetails]
(
	  [Id] INT IDENTITY(1,1) PRIMARY KEY,	  
	  [Invoice_Id] INT NULL,
	  [Customer_Contact_Id] INT NULL,	
	  [Supplier_Contact_Id] INT NULL,
	  [Factory_Contact_Id] INT NULL, 
	  
	  CONSTRAINT INV_AUT_TRAN_ContactDetails_Invoice_Id FOREIGN KEY ([Invoice_Id]) REFERENCES [dbo].[INV_AUT_TRAN_Details](Id),

	  CONSTRAINT INV_AUT_TRAN_ContactDetails_Customer_Contact_Id FOREIGN KEY ([Customer_Contact_Id]) REFERENCES [dbo].[CU_Contact](Id),

	  CONSTRAINT INV_AUT_TRAN_ContactDetails_Supplier_Contact_Id FOREIGN KEY ([Supplier_Contact_Id]) REFERENCES [dbo].[SU_Contact](Id),

	  CONSTRAINT INV_AUT_TRAN_ContactDetails_Factory_Contact_Id	 FOREIGN KEY ([Factory_Contact_Id]) REFERENCES [dbo].[SU_Contact](Id)
)


Insert into INV_Status (Name,Active)
values('Created',1)
Insert into INV_Status (Name,Active)
values('Modified',1)
Insert into INV_Status (Name,Active)
values('Approved',1)
Insert into INV_Status (Name,Active)
values('Cancelled',1)


insert into INV_Payment_Status (Name,Active)
Values('Not Paid',1)
insert into INV_Payment_Status (Name,Active)
Values('Half Paid',1)
insert into INV_Payment_Status (Name,Active)
Values('Paid',1)





-- alter query cu_collection starts--
Alter table CU_Collection add CreatedOn Datetime default getdate()
Alter table CU_Collection add UpdatedOn Datetime
Alter table CU_Collection add DeletedOn Datetime

Alter table CU_Collection add DeletedBy int null 
Alter table CU_Collection add UpdatedBy int null 
Alter table CU_Collection add CreatedBy int null 


ALTER TABLE CU_Collection ADD CONSTRAINT FK_CU_Collection_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
ALTER TABLE CU_Collection ADD CONSTRAINT FK_CU_Collection_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
ALTER TABLE CU_Collection ADD CONSTRAINT FK_CU_Collection_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)		
	-- alter query cu_collection ends--

--insert query INSP_Status starts--
	insert into INSP_Status(Status,Active,Entity_Id,Priority) values('ReportSent', 1,1,11)
	--insert query INSP_Status ends--

	--column INSP_TRAN_Status_Log name change and column added
	--sp_rename 'INSP_TRAN_Status_Log.CreatedOn', 'StatusChangeDate', 'COLUMN';
	
	Alter table INSP_TRAN_Status_Log alter column StatusChangeDate Datetime null 

	Alter table INSP_TRAN_Status_Log add CreatedOn Datetime default getdate()
	--column INSP_TRAN_Status_Log name change

	
	-- alter query cu_collection ends--



ALTER TABLE INV_AUT_TRAN_Details ADD [TravelOtherFees] FLOAT NULL

ALTER TABLE INV_AUT_TRAN_Details ADD [TravelTotalFees] FLOAT NULL

ALTER TABLE INV_AUT_TRAN_Details ADD UpdatedBy INT

ALTER TABLE INV_AUT_TRAN_Details ADD UpdatedOn DATETIME


CREATE TABLE [dbo].[INV_AUT_TRAN_Tax]
(
	  [Id] INT IDENTITY(1,1) PRIMARY KEY,	  
	  [Invoice_Id] INT NULL,
	  [TaxId] INT NULL,	
	  [CreatedBy] INT NULL,
	  [CreatedOn] Datetime NULL,		  
	  CONSTRAINT INV_AUT_TRAN_Tax_Invoice_Id FOREIGN KEY ([Invoice_Id]) REFERENCES [dbo].[INV_AUT_TRAN_Details](Id),
	  CONSTRAINT INV_AUT_TRAN_Tax_TaxId FOREIGN KEY ([TaxId]) REFERENCES [dbo].[INV_TRAN_Bank_Tax](Id),
	  CONSTRAINT INV_AUT_TRAN_Tax_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)

ALTER TABLE INV_AUT_TRAN_Status_Log DROP COLUMN CreatedOn 

ALTER TABLE INV_AUT_TRAN_Status_Log ADD CreatedOn DATETIME

-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[REF_KPI_Template_Type]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Active] BIT NOT NULL
)

ALTER TABLE  REF_KPI_TEAMPLATE ADD TypeId INT NULL
CONSTRAINT FK_REF_KPI_TEAMPLATE_TypeId FOREIGN KEY (TypeId) REFERENCES [dbo].[REF_KPI_Template_Type](Id)

INSERT INTO [dbo].[REF_KPI_Template_Type]
           ([Name],[Active])
     VALUES
           ('Statistics',1),
		   ('Invoice',1)
---------------------------------------------------------------------------------------------------------------------------

ALTER TABLE INV_AUT_TRAN_Details ADD [ProrateBookingNumbers] nvarchar(1000) NULL



--[REF_KPI_Teamplate] starts --
INSERT INTO [dbo].[REF_KPI_Teamplate]
           ([Name]
           ,[Active],CustomerId)
     VALUES
           ('Carrefour Invoice Template', 1, 90)

--[REF_KPI_Teamplate] ends--


-- Fb Enhancement updates start here --

	CREATE TABLE [dbo].[FB_Report_Defect_Photos]
	(
		[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
		[DefectId] int not null,
		[Description] NVARCHAR(1500) NULL, 
		[Path] NVARCHAR(1000) NULL, 	
		[CreatedOn] DATETIME NULL,
		[DeletedOn] DATETIME NULL, 
		[Active] BIT NULL,	
		CONSTRAINT FK_FB_Report_Defect_Photos_FbDefectId
		FOREIGN KEY(DefectId) REFERENCES [FB_Report_InspDefects](Id)
	)

	ALTER TABLE FB_Report_Additional_Photos ADD [Description] NVARCHAR(1500) NULL
	ALTER TABLE FB_Report_Additional_Photos ADD [ProductId] INT NULL
	ALTER TABLE FB_Report_Additional_Photos ADD CONSTRAINT FK_InspSummary_Additional_Photos_ProductId  FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)

	ALTER TABLE FB_Report_InspSummary_Photo ADD [Description] NVARCHAR(1500) NULL
	ALTER TABLE FB_Report_InspSummary_Photo ADD [ProductId] INT NULL
	ALTER TABLE FB_Report_InspSummary_Photo ADD CONSTRAINT FK_InspSummary_Photo_ProductId FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)

	-- Fb Enhancement updates end here --


	--insp_transtion start here
	ALTER TABLE INSP_Transaction ADD CompassBookingNo NVARCHAR(1500) NULL	
	--insp_transtion end here

-- Added Log table for Login and Logut start

CREATE TABLE [dbo].[IT_login_Log]
(
		[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,	
		[UserItId] int not null,
		[IpAddress] NVARCHAR(1000) NULL, 
		[BrowserType] NVARCHAR(1000) NULL, 	
		[DeviceType] NVARCHAR(1000) NULL, 	
		[LogInTime] DATETIME NULL,
		[LogOutTime] DATETIME NULL,
		[Latitude] DECIMAL(12, 9) NULL, 
		[Longitude] DECIMAL(12, 9) NULL, 	
		FOREIGN KEY(UserItId) REFERENCES IT_UserMaster(Id)
)

-- Added Log table for Login and Logut end

-- Add booking form serail number for invoice sorting start

ALTER TABLE INSP_Product_Transaction ADD BookingFormSerial NVARCHAR(1000) NULL

Alter table INSP_Product_Transaction drop column BookingFormSerial

ALTER TABLE INSP_Product_Transaction ADD BookingFormSerial INT NULL

-- Add booking form serail number for invoice sorting end



--Extra Fee Booking starts


create table  [dbo].[INV_EXF_Status] ([Id] int not null primary key identity(1,1),
[Name] nvarchar(max)
,[Active] bit,
[Sort] Bit, 
[CreatedOn] datetime not null default getdate()
)



create table  [dbo].[INV_EXF_Type] ([Id] int not null primary key identity(1,1),
[Name] nvarchar(max)
,[Active] bit,
[Sort] Bit, 
[CreatedOn] datetime not null default getdate()
)


CREATE TABLE [dbo].[INV_EXF_Transaction]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	InspectionId int null,
	AuditId int null,
	CustomerId int,
	StatusId int null,  SupplierId  int null,  FactoryId int null, BilledTo int,
	CurrencyId int, ServiceId int, Tax float, BankId int, [PaymentDate] datetime, Remarks nvarchar(max),
	BillingEntityId int, InvoiceId int, ExtraFeeInvoiceNo nvarchar(max), [PaymentStatus] int,
	CreatedOn datetime not null default getdate(),  CreatedBy int null, UpdatedBy int null, 
	UpdatedOn datetime  null, DeletedBy int null, DeletedOn datetime  null,  Active bit,
	ExtraFeeSubTotal float, TaxAmount float, TotalExtraFee float
	CONSTRAINT INV_EXF_Transaction_InspectionId FOREIGN KEY (InspectionId) REFERENCES [dbo].[INSP_Transaction](Id),
	CONSTRAINT INV_EXF_Transaction_AuditId FOREIGN KEY (AuditId) REFERENCES [dbo].[AUD_Transaction](Id),
	CONSTRAINT INV_EXF_Transaction_CustomerId FOREIGN KEY (CustomerId) REFERENCES [dbo].[CU_Customer](Id),
	CONSTRAINT INV_EXF_Transaction_StatusId FOREIGN KEY (StatusId) REFERENCES [dbo].[INV_EXF_Status](Id),
	CONSTRAINT INV_EXF_Transaction_FactoryId FOREIGN KEY (FactoryId) REFERENCES [dbo].[SU_Supplier](Id),
	CONSTRAINT INV_EXF_Transaction_BilledTo FOREIGN KEY (BilledTo) REFERENCES [dbo].[QU_PaidBy](Id),
	CONSTRAINT INV_EXF_Transaction_CurrencyId FOREIGN KEY (CurrencyId) REFERENCES [dbo].[REF_CURRENCY](Id),
	CONSTRAINT INV_EXF_Transaction_ServiceId FOREIGN KEY (ServiceId) REFERENCES [dbo].[REF_SERVICE](Id),
	CONSTRAINT INV_EXF_Transaction_InvoiceId FOREIGN KEY (InvoiceId) REFERENCES [dbo].[INV_AUT_TRAN_Details](Id),
	CONSTRAINT INV_EXF_Transaction_BillingEntityId FOREIGN KEY (BillingEntityId) REFERENCES [dbo].[REF_Billing_Entity](Id),
	CONSTRAINT INV_EXF_Transaction_BankId FOREIGN KEY (BankId) REFERENCES [dbo].[INV_REF_Bank](Id),
	CONSTRAINT INV_EXF_Transaction_SupplierId FOREIGN KEY (SupplierId) REFERENCES [dbo].[SU_Supplier](Id),
	CONSTRAINT INV_EXF_Transaction_PaymentStatus FOREIGN KEY ([PaymentStatus]) REFERENCES [dbo].[INV_Payment_Status](Id),
	CONSTRAINT INV_EXF_Transaction_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT INV_EXF_Transaction_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT INV_EXF_Transaction_DeletedBy FOREIGN KEY (DeletedBy) REFERENCES [dbo].[IT_UserMaster](Id)
)


CREATE TABLE [dbo].[INV_EXF_TRAN_Details]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	[EXFTransactionId] int null,
	ExtraFeeType int null, 
	ExtraFees float null,
	Remarks nvarchar(max),
	CreatedOn datetime not null default getdate(), 
	CreatedBy int null,
	UpdatedBy int null, 
	UpdatedOn datetime  null,
	DeletedBy int null,
	DeletedOn datetime  null, 
	Active bit
	CONSTRAINT INV_EXF_TRAN_Details_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT INV_EXF_TRAN_Details_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT INV_EXF_TRAN_Details_DeletedBy FOREIGN KEY (DeletedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT INV_EXF_TRAN_Details_ExtraFeeType FOREIGN KEY (ExtraFeeType) REFERENCES [dbo].[INV_EXF_Type](Id),
	CONSTRAINT INV_EXF_TRAN_Details_EXFTransactionId FOREIGN KEY (EXFTransactionId) REFERENCES [dbo].[inv_exf_transaction](Id),
)



CREATE TABLE [dbo].[INV_EXT_TRAN_Tax]
(
[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
[ExtraFee_Id] INT NULL,
[TaxId] INT NULL,
[CreatedBy] INT NULL,
[CreatedOn] DateTime default getdate()
CONSTRAINT INV_EXT_TRAN_Tax_ExtraFee_Id FOREIGN KEY ([ExtraFee_Id]) REFERENCES [dbo].[INV_EXF_Transaction](Id),
CONSTRAINT INV_EXT_TRAN_Tax_TaxId FOREIGN KEY ([TaxId]) REFERENCES [dbo].[INV_TRAN_Bank_Tax](Id),
CONSTRAINT INV_EXT_TRAN_Tax_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)

CREATE TABLE [dbo].[INV_EXF_TRAN_Status_Log]
(
	 [Id] INT IDENTITY(1,1) PRIMARY KEY not null,	 
	 [ExtraFee_Id] INT NULL,
	 [Inspection_Id] INT NULL, 
     [Audit_Id] INT NULL,
	 [Status_Id] INT NULL,
     [CreatedBy] INT NULL,
     [CreatedOn] DATETIME default getdate()
	 CONSTRAINT INV_EXF_TRAN_Status_Log_ExtraFee_Id FOREIGN KEY ([ExtraFee_Id]) REFERENCES [dbo].[INV_EXF_Transaction](Id),
	 CONSTRAINT INV_EXF_TRAN_Status_Log_Inspection_Id FOREIGN KEY ([Inspection_Id]) REFERENCES [dbo].[INSP_Transaction](Id),
	 CONSTRAINT INV_EXF_TRAN_Status_Log_Audit_Id FOREIGN KEY ([Audit_Id]) REFERENCES [dbo].[AUD_Transaction](Id),
	 CONSTRAINT INV_EXF_TRAN_Status_Log_Status_Id FOREIGN KEY ([Status_Id]) REFERENCES [dbo].[INV_EXF_Status](Id),
	 CONSTRAINT INV_EXF_TRAN_Status_Log_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)



insert into INV_EXF_Status(name,active,sort) values('Pending',1,1)
insert into INV_EXF_Status(name,active,sort) values('Invoiced',1,2)
insert into INV_EXF_Status(name,active,sort) values('Cancelled',1,3)

insert into INV_EXF_Type(name,active,sort) values('Penalty',1,1)
insert into INV_EXF_Type(name,active,sort) values('Modify Fee',1,2)
insert into INV_EXF_Type(name,active,sort) values('OT Fee',1,3)

--Extra Fee Booking query  ends

declare @IdTran as int  ,@ParentIdLvl2 as int
INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Extra Fees', NULL,11)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[IT_Right] ([ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], TitleName_IdTran)
VALUES (19, N'Extra Fees', 'Extra Fees', 'extrafees/edit-extra-fees', 0, 1, N'fa fa-sign-in', 8,@IdTran)
SELECT @ParentIdLvl2 = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @ParentIdLvl2) -- IT-Team


ALTER TABLE INV_AUT_TRAN_Details ADD InvoiceType INT Null;

ALTER TABLE INV_AUT_TRAN_Details ADD CONSTRAINT FK_InvoiceType 
FOREIGN KEY (InvoiceType) REFERENCES REF_InvoiceType(Id);


ALTER TABLE FB_Report_Quantity_Details ALTER column SelectCtnNO [nvarchar](MAX)



---- adding claimtype foreign key constraint start-------

ALTER TABLE EC_ExpencesClaims
ADD CONSTRAINT FK_ExpenseClaimType
FOREIGN KEY (ClaimTypeId) REFERENCES EC_ExpenseClaimtype(Id)

---- adding claimtype foreign key constraint end-------

------ Checkpoint Tables Start -----------------

CREATE TABLE [dbo].[CU_CheckPoints_ServiceType]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[CheckpointId] INT NOT NULL, 
    [ServiceTypeId] INT NOT NULL, 
    [Active] BIT NOT NULL, 
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL,
	FOREIGN KEY([CheckpointId]) REFERENCES CU_CheckPoints(Id),
	FOREIGN KEY([CreatedBy]) REFERENCES It_UserMaster(Id),
	FOREIGN KEY([UpdatedBy]) REFERENCES It_UserMaster(Id),
	FOREIGN KEY([ServiceTypeId]) REFERENCES CU_ServiceType(Id)
)

CREATE TABLE [dbo].[CU_CheckPoints_Department]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[CheckpointId] INT NOT NULL, 
    [DeptId] INT NOT NULL, 
    [Active] BIT NOT NULL, 
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL,
	FOREIGN KEY([CheckpointId]) REFERENCES CU_CheckPoints(Id),
	FOREIGN KEY([CreatedBy]) REFERENCES It_UserMaster(Id),
	FOREIGN KEY([UpdatedBy]) REFERENCES It_UserMaster(Id),
	FOREIGN KEY([DeptId]) REFERENCES CU_Department(Id)
)

CREATE TABLE [dbo].[CU_CheckPoints_Brand]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [CheckpointId] INT NOT NULL, 
    [BrandId] INT NOT NULL, 
    [Active] BIT NOT NULL, 
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL,
	FOREIGN KEY([CheckpointId]) REFERENCES CU_CheckPoints(Id),
	FOREIGN KEY([CreatedBy]) REFERENCES It_UserMaster(Id),
	FOREIGN KEY([UpdatedBy]) REFERENCES It_UserMaster(Id),
	FOREIGN KEY([BrandId]) REFERENCES CU_Brand(Id)
)
------------- Checkpoint tables end ------------
---- adding claimtype foreign key constraint end-------

-- Audit Invoice start

ALTER TABLE INV_AUT_TRAN_Details ADD [AuditId] INT NULL
ALTER TABLE INV_AUT_TRAN_Details ADD CONSTRAINT INV_AUT_TRAN_Details_AuditId  FOREIGN KEY(AuditId) REFERENCES [AUD_Transaction](Id)

ALTER TABLE INV_EXF_Transaction ADD [AuditId] INT NULL
ALTER TABLE INV_EXF_Transaction ADD CONSTRAINT INV_EXF_Transaction_AuditId  FOREIGN KEY(AuditId) REFERENCES [AUD_Transaction](Id)

ALTER TABLE INV_AUT_TRAN_Details ADD [ServiceId] INT NULL
ALTER TABLE INV_AUT_TRAN_Details ADD CONSTRAINT INV_AUT_TRAN_Details_ServiceId  FOREIGN KEY(ServiceId) REFERENCES [REF_Service](Id)

-- After impementation of audit invoice we have to update existing invoice as inspection invoice

update INV_AUT_TRAN_Details set ServiceId=1

-- Audit Invoice end

----- Extra Fee new columns start ---------------------------

ALTER TABLE INV_EXF_Transaction ADD [OfficeId] INT NULL
ALTER TABLE INV_EXF_Transaction ADD CONSTRAINT INV_EXF_Transaction_OfficeId  FOREIGN KEY(OfficeId) REFERENCES [INV_REF_Office](Id)

CREATE TABLE [dbo].[INV_EXF_ContactDetails]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
    [ExtraFeeId] INT NULL, 
    [CustomerContactId] INT NULL, 
    [SupplierContactId] INT NULL, 
    [FactoryContactId] INT NULL,
	CONSTRAINT INV_EXF_ContactDetails_ExtraFeeId FOREIGN KEY ([ExtraFeeId]) REFERENCES [dbo].[INV_EXF_Transaction](Id),
	CONSTRAINT INV_EXF_ContactDetails_CustomerContactId FOREIGN KEY ([CustomerContactId]) REFERENCES [dbo].[CU_Contact](Id),
	CONSTRAINT INV_EXF_ContactDetails_SupContactId FOREIGN KEY ([SupplierContactId]) REFERENCES [dbo].[SU_Contact](Id),
	CONSTRAINT INV_EXF_ContactDetails_FactContactId FOREIGN KEY ([FactoryContactId]) REFERENCES [dbo].[SU_Contact](Id)
)

----------- Extra Fee new columns End ------------------------------

----------- #1 change set start------------------------------

--FB_Report_Quantity_Details column data type changed
ALTER TABLE FB_Report_Quantity_Details ADD SelectCtnNO NVARCHAR(max) NULL;

--INV_EXF_Transaction column added
ALTER TABLE INV_EXF_Transaction ADD ExtraFeeInvoiceDate Datetime NULL;

----------- #1 change set end------------------------------


--------------#2 change set TCF Related Table Starts------------------------------

CREATE TABLE [dbo].TCF_Master_DataLog
(
 ID INT IDENTITY(1,1) PRIMARY KEY,
 AccountId INT,
 DataType INT,
 RequestUrl NVARCHAR(500),
 LogInformation NVARCHAR(MAX),
 ResponseMessage NVARCHAR(MAX),
 CreatedOn DATETIME,
 CreatedBy INT
)

CREATE TABLE [dbo].[APIGateway_Log]
(
ID INT IDENTITY(1,1) PRIMARY KEY,
RequestUrl NVARCHAR(500),
LogInformation NVARCHAR(MAX),
ResponseMessage NVARCHAR(MAX),
RequestBaseUrl NVARCHAR(500),
CreatedOn DATETIME,
CreatedBy INT
)


CREATE TABLE [dbo].[REF_Product_Category_API_Services]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[Product_Category_Id] INT NOT NULL,
	[ServiceId] INT NOT NULL,
	[CustomName] NVARCHAR(50) NULL,
	[Active] BIT NOT NULL,
	[CreatedBy] INT NULL,
	[CreatedOn] DATETIME NULL,
	[DeletedBy] INT NULL,
	[DeletedOn] DATETIME NULL,
	FOREIGN KEY([Product_Category_Id]) REFERENCES [dbo].[REF_ProductCategory](Id),
	FOREIGN KEY([ServiceId]) REFERENCES [dbo].[REF_API_Services](Id),
	FOREIGN KEY(CreatedBy) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY(DeletedBy) REFERENCES [IT_UserMaster](Id)
)

Insert into [REF_Product_Category_API_Services]([Product_Category_Id],[ServiceId],[CustomName],[Active],[CreatedBy],
		[CreatedOn]) Values
		(1,3,'',1,1,getDate())

Insert into [REF_Product_Category_API_Services]([Product_Category_Id],[ServiceId],[CustomName],[Active],[CreatedBy],
		[CreatedOn]) Values
		(2,3,'',1,1,getDate())

Insert into [REF_Product_Category_API_Services]([Product_Category_Id],[ServiceId],[CustomName],[Active],[CreatedBy],
		[CreatedOn]) Values
		(3,3,'',1,1,getDate())

Insert into [REF_Product_Category_API_Services]([Product_Category_Id],[ServiceId],[CustomName],[Active],[CreatedBy],
		[CreatedOn]) Values
		(4,3,'',1,1,getDate())

Insert into [REF_Product_Category_API_Services]([Product_Category_Id],[ServiceId],[CustomName],[Active],[CreatedBy],
		[CreatedOn]) Values
		(8,3,'',1,1,getDate())


Insert into [REF_Product_Category_API_Services]([Product_Category_Id],[ServiceId],[CustomName],[Active],[CreatedBy],
		[CreatedOn]) Values
		(13,3,'',1,1,getDate())

ALTER TABLE MID_Task ADD TaskReason NVARCHAR(MAX) NULL


INSERT INTO MID_TaskType(Id,Label,EntityId) Values (19,'UpdateCustomerContactToTCF',1)
INSERT INTO MID_TaskType(Id,Label,EntityId) Values (20,'UpdateSupplierToTCF',1)
INSERT INTO MID_TaskType(Id,Label,EntityId) Values (21,'UpdateUserToTCF',1)
INSERT INTO MID_TaskType(Id,Label,EntityId) Values (22,'UpdateBuyerToTCF',1)
INSERT INTO MID_TaskType(Id,Label,EntityId) Values (23,'UpdateProductToTCF',1)

Alter table Mid_Task drop column TaskReason

--------------#2 change set TCF Related Table Ends------------------------------

---------------#3 Add 2 columns to It User Master Starts --------------------------------

ALTER TABLE IT_UserMaster ADD FileUrl NVARCHAR(MAX) NULL

ALTER TABLE IT_UserMaster ADD [FileName] NVARCHAR(255) NULL

---------------#3 Add 2 columns to It User Master Ends --------------------------------


----------- #4 change set start------------------------------

CREATE TABLE [dbo].[QC_BlockList]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[QCId] INT NOT NULL, 
	[Active] BIT,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME DEFAULT GETDATE(), 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL,
	CONSTRAINT FK_QC_BlockList_QCId FOREIGN KEY (QCId) REFERENCES [dbo].[HR_STAFF](Id),
    CONSTRAINT FK_QC_BlockList_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_QC_BlockList_DeletedBy FOREIGN KEY (DeletedBy) REFERENCES [dbo].[IT_UserMaster](Id)
)

CREATE TABLE [dbo].[QC_BL_Customer]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[QCBLId] INT NOT NULL,
	[Customer_Id] INT,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME DEFAULT GETDATE(), 
	CONSTRAINT FK_QC_BL_Customer_Customer_Id FOREIGN KEY([Customer_Id]) REFERENCES [dbo].[CU_Customer](Id),
	CONSTRAINT FK_QC_BL_Customer_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_QC_BL_Customer_QCBLId FOREIGN KEY (QCBLId) REFERENCES [dbo].[QC_BlockList](Id)
)

CREATE TABLE [dbo].[QC_BL_ProductCatgeory]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[QCBLId] INT NOT NULL,
	[ProductCategoryId] INT,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME DEFAULT GETDATE()
	CONSTRAINT FK_QC_BL_ProductCatgeory_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_QC_BL_ProductCatgeory_ProductCategoryId FOREIGN KEY ([ProductCategoryId]) REFERENCES [REF_ProductCategory](Id), 
	CONSTRAINT FK_QC_BL_ProductCatgeory_QCBLId FOREIGN KEY (QCBLId) REFERENCES [dbo].[QC_BlockList](Id)
)

CREATE TABLE [dbo].[QC_BL_ProductSubCategory]
(
		[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[QCBLId] INT NOT NULL,
	[ProductSubCategoryId] INT,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME DEFAULT GETDATE()
	CONSTRAINT FK_QC_BL_ProductSubCatgeory_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_QC_BL_ProductSubCatgeory_ProductSubCategoryId FOREIGN KEY ([ProductSubCategoryId]) REFERENCES [REF_ProductCategory_Sub](Id), 
	CONSTRAINT FK_QC_BL_ProductSubCatgeory_QCBLId FOREIGN KEY (QCBLId) REFERENCES [dbo].[QC_BlockList](Id)
)

CREATE TABLE [dbo].[QC_BL_ProductSubCategory2]
(
		[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[QCBLId] INT NOT NULL,
	[ProductSubCategory2Id] INT,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME DEFAULT GETDATE()
	CONSTRAINT FK_QC_BL_ProductSubCategory2_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_QC_BL_ProductSubCategory2_ProductCategoryId FOREIGN KEY ([ProductSubCategory2Id]) REFERENCES [REF_ProductCategory_Sub2](Id),
	CONSTRAINT FK_QC_BL_ProductSubCategory2_QCBLId FOREIGN KEY (QCBLId) REFERENCES [dbo].[QC_BlockList](Id)
)


CREATE TABLE [dbo].[QC_BL_Supplier_Factory]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[QCBLId] INT NOT NULL,
	[Supplier_FactoryId] INT,
	[TypeId] INT,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME DEFAULT GETDATE(), 
	CONSTRAINT FK_QC_BL_Supplier_Factory_Supplier_Id FOREIGN KEY([Supplier_FactoryId]) REFERENCES [dbo].[SU_Supplier](Id),
	CONSTRAINT FK_QC_BL_Supplier_Factory_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_QC_BL_Supplier_Factory_QCBLId FOREIGN KEY (QCBLId) REFERENCES [dbo].[QC_BlockList](Id)
)


declare @IdTran as int, @IdItem as int
INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'QC Block', NULL, 11)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[IT_Right] ([ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], MenuName_IdTran) 
VALUES (96, NULL, N'QC Block', N'qcblock/edit-qc-block', 0, 1, N'fa fa-plus-square', 40,@IdTran)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team	

----------- #4 change set end------------------------------

----------- #5 change set (API Service Reference Changed:Please change the foreign key as per your local db) start------------------------------

ALTER TABLE CU_API_Services DROP CONSTRAINT FK__CU_API_Se__Servi__62458BBE

ALTER TABLE CU_API_Services ADD CONSTRAINT FK_CU_API_Service FOREIGN KEY (ServiceId) REFERENCES REF_SERVICE(Id)

ALTER TABLE SU_API_Services DROP CONSTRAINT FK__SU_API_Se__Servi__48FABB07

ALTER TABLE SU_API_Services ADD CONSTRAINT FK_SU_API_Service FOREIGN KEY (ServiceId) REFERENCES REF_SERVICE(Id)

ALTER TABLE CU_Buyer_API_Services DROP CONSTRAINT FK__CU_Buyer___Servi__6BCEF5F8

ALTER TABLE CU_Buyer_API_Services ADD CONSTRAINT FK_CU_Buyer_API_Service FOREIGN KEY (ServiceId) REFERENCES REF_SERVICE(Id)

DROP TABLE CU_Contact_API_Services

DROP TABLE ExternalClientDataLog

ALTER TABLE CU_Product_API_Services DROP CONSTRAINT FK__CU_Produc__Servi__77FFC2B3

ALTER TABLE CU_Product_API_Services ADD CONSTRAINT FK_CU_Product_API_Service FOREIGN KEY (ServiceId) REFERENCES REF_SERVICE(Id)

ALTER TABLE SU_Contact_API_Services DROP CONSTRAINT FK__SU_Produc__Servi__77FFC2B3

ALTER TABLE SU_Contact_API_Services ADD CONSTRAINT FK_SU_Contact_API_Service FOREIGN KEY (ServiceId) REFERENCES REF_SERVICE(Id)

INSERT INTO  REF_Service(Name,Active) Values ('TCF',1)

insert into IT_Role (RoleName,Active,EntityId,PrimaryRole,SecondaryRole) values ('TCF User',1,1,1,1)

insert into IT_Role (RoleName,Active,EntityId,PrimaryRole,SecondaryRole) values ('TCF Customer',1,1,1,1)

insert into IT_Role (RoleName,Active,EntityId,PrimaryRole,SecondaryRole) values ('TCF Supplier',1,1,1,1)

--creating menus for the tcf document page(please make the menus to create under the Report Main Menu)----

declare @IdTran as int  ,@ParentIdLvl2 as int
INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Tcf Summary', NULL,11)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[IT_Right] ([ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], TitleName_IdTran)
VALUES (19, N'Tcf Summary', 'Tcf Summary', 'tcfsummary/tcf-summary', 0, 1, N'fa fa-sign-in', 8,@IdTran)
SELECT @ParentIdLvl2 = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @ParentIdLvl2) -- IT-Team

declare @IdTran as int  ,@ParentIdLvl2 as int
INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Tcf Document', NULL,11)
SELECT @IdTran =  SCOPE_IDENTITY()
INSERT [dbo].[IT_Right] ([ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], TitleName_IdTran)
VALUES (19, N'Tcf Document', 'Tcf Document', 'tcfdocument/tcf-document', 0, 1, N'fa fa-sign-in', 8,@IdTran)
SELECT @ParentIdLvl2 = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @ParentIdLvl2) -- IT-Team

----------- #5 change set (API Service Reference Changed) end------------------------------
 

-----  #6 change set Added Zone in county start ---------------------------

 ALTER TABLE REF_County ADD [ZoneId] INT NULL
ALTER TABLE REF_County ADD CONSTRAINT REF_County_ZoneId  FOREIGN KEY(ZoneId) REFERENCES [REF_Zone](Id)

 ALTER TABLE HR_Staff ADD [Current_CountyId] INT NULL
ALTER TABLE HR_Staff ADD CONSTRAINT HR_Staff_Current_CountyId  FOREIGN KEY(Current_CountyId) REFERENCES [REF_County](Id)
-----#6 change set  Added Zone in county End ---------------------------

-------------#7 Insert script Start--------------------------------------------------
INSERT INTO CU_CheckPointType ([Name], Active, [Entity_Id])
VALUES ('Skip Quotation Sent To Client', 1, 1)

INSERT INTO [dbo].[REF_KPI_Teamplate]
           ([Name]
           ,[Active])
     VALUES
           ('CarrefourDailyResultTemplate', 1)

--------------#7 Insert script End ---------------------------------------------------

----- #8 Configuration for email sending process in the report level start here


CREATE TABLE [dbo].[ES_Type]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Name] NVARCHAR(200) NOT NULL, 
    [Active] BIT NOT NULL	
)

INSERT INTO ES_Type ([Name], Active)
VALUES ('Customer Decision', 1)

INSERT INTO ES_Type ([Name], Active)
VALUES ('Report Send', 1)

CREATE TABLE ES_Details
(
  [Id] INT NOT NULL PRIMARY KEY identity(1,1),  
  [CustomerId] int NOT NULL,
  [ServiceId] int NOT NULL,
  [TypeId] int NOT NULL,
  [Is_Include_CC] BIT,
  [Is_Include_SC] BIT,
  [Is_Include_FC] BIT, 
  [Active] BIT,
  [CreatedOn] DATETIME NOT NULL default getdate(),  
  [CreatedBy] int NULL, 
  CONSTRAINT ES_Details_CustomerId FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[CU_Customer](Id),
  CONSTRAINT ES_Details_ServiceId FOREIGN KEY ([ServiceId]) REFERENCES [dbo].[REF_Service](Id),
  CONSTRAINT ES_Details_TypeId FOREIGN KEY ([TypeId]) REFERENCES [dbo].[ES_Type](Id),
  CONSTRAINT ES_Details_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)

CREATE TABLE [dbo].[ES_Office_Config]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [OfficeId] int NOT NULL,
	[EsDetailsId] int NOT NULL,
	CONSTRAINT ES_Office_Config_EsDetailsId FOREIGN KEY ([EsDetailsId]) REFERENCES [dbo].[ES_Details](Id)
)

CREATE TABLE [dbo].[ES_CU_Config]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [DepartmentId] int  NULL,
	[BrandId] int  NULL,
	[CollectionId] int  NULL,
	[BuyerId] int  NULL,
	[EsDetailsId] int NOT NULL,
	CONSTRAINT ES_CU_Config_DepartmentId FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[CU_Department](Id),
	CONSTRAINT ES_CU_Config_BrandId FOREIGN KEY ([BrandId]) REFERENCES [dbo].[CU_Brand](Id),
	CONSTRAINT ES_CU_Config_CollectionId FOREIGN KEY ([CollectionId]) REFERENCES [dbo].[CU_Collection](Id),
	CONSTRAINT ES_CU_Config_BuyerId FOREIGN KEY ([BuyerId]) REFERENCES [dbo].[CU_Buyer](Id),
	CONSTRAINT ES_CU_Config_EsDetailsId FOREIGN KEY ([EsDetailsId]) REFERENCES [dbo].[ES_Details](Id)
)

CREATE TABLE [dbo].[ES_ServiceType_Config]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [ServiceTypeId] int NOT NULL,
	[EsDetailsId] int NOT NULL,
	CONSTRAINT ES_ServiceType_Config_ServiceTypeId FOREIGN KEY ([ServiceTypeId]) REFERENCES [dbo].[REF_ServiceType](Id),
	CONSTRAINT ES_ServiceType_Config_EsDetailsId FOREIGN KEY ([EsDetailsId]) REFERENCES [dbo].[ES_Details](Id)
)

CREATE TABLE [dbo].[ES_Product_Category_Config]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [ProductCategoryId] int NOT NULL,
	[EsDetailsId] int NOT NULL,
	CONSTRAINT ES_Product_Category_Config_ProductCategoryId FOREIGN KEY ([ProductCategoryId]) REFERENCES [dbo].[REF_ProductCategory](Id),
	CONSTRAINT ES_Product_Category_Config_EsDetailsId FOREIGN KEY ([EsDetailsId]) REFERENCES [dbo].[ES_Details](Id)
)

CREATE TABLE [dbo].[ES_FA_Country_Config]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Factory_CountryId] int NOT NULL,
	[EsDetailsId] int NOT NULL,
	CONSTRAINT ES_FA_Country_Config_Factory_CountryId FOREIGN KEY ([Factory_CountryId]) REFERENCES [dbo].[REF_Country](Id),
	CONSTRAINT ES_FA_Country_Config_EsDetailsId FOREIGN KEY ([EsDetailsId]) REFERENCES [dbo].[ES_Details](Id)
)

CREATE TABLE [dbo].[ES_Result_Config]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Customer_Result_Id] int NOT NULL,
	[EsDetailsId] int NOT NULL,
	CONSTRAINT ES_Result_Config_Customer_Result_Id FOREIGN KEY  ([Customer_Result_Id]) REFERENCES [dbo].[REF_INSP_CUS_Decision_Config](Id),
	CONSTRAINT ES_Result_Config_EsDetailsId FOREIGN KEY ([EsDetailsId]) REFERENCES [dbo].[ES_Details](Id)
)

CREATE TABLE [dbo].[ES_Sup_Fact_Config]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Supplier_OR_Factory_Id] int NOT NULL,
	[EsDetailsId] int NOT NULL,
	CONSTRAINT ES_Sup_Fact_Config_Supplier_OR_Factory_Id FOREIGN KEY  ([Supplier_OR_Factory_Id]) REFERENCES [dbo].[Su_Supplier](Id),
	CONSTRAINT ES_Sup_Fact_Config_EsDetailsId FOREIGN KEY ([EsDetailsId]) REFERENCES [dbo].[ES_Details](Id)
)


CREATE TABLE [dbo].[ES_CU_Contacts]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Customer_Contact_Id] int NOT NULL,
	[EsDetailsId] int NOT NULL,
	CONSTRAINT ES_CU_Contacts_Customer_Contact_Id FOREIGN KEY  ([Customer_Contact_Id]) REFERENCES [dbo].[CU_Contact](Id),
	CONSTRAINT ES_CU_Contacts_EsDetailsId FOREIGN KEY ([EsDetailsId]) REFERENCES [dbo].[ES_Details](Id)
)

CREATE TABLE [dbo].[ES_API_Contacts]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Api_Contact_Id] int NOT NULL,
	[EsDetailsId] int NOT NULL,
	CONSTRAINT ES_API_Contacts_Customer_Api_Contact_Id FOREIGN KEY  ([Api_Contact_Id]) REFERENCES [dbo].[HR_Staff](Id),
	CONSTRAINT ES_API_Contacts_EsDetailsId FOREIGN KEY ([EsDetailsId]) REFERENCES [dbo].[ES_Details](Id)
)

CREATE TABLE [dbo].[ES_API_Default_Contacts]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Api_Contact_Id] int NOT NULL,
	[OfficeId] int NOT NULL,	
	CONSTRAINT ES_API_Default_Contacts_Api_Contact_Id FOREIGN KEY  ([Api_Contact_Id]) REFERENCES [dbo].[HR_Staff](Id)	
)


insert into [dbo].[ES_Details] (CustomerId,ServiceId,TypeId,Is_Include_CC,Is_Include_SC,Is_Include_FC,Active) values(1,1,1,1,1,1,1)
insert into [dbo].[ES_Details] (CustomerId,ServiceId,TypeId,Is_Include_CC,Is_Include_SC,Is_Include_FC,Active) values(2,1,1,1,1,1,1)

insert into [dbo].[ES_API_Default_Contacts] (Api_Contact_Id,OfficeId) values(1,1)
insert into [dbo].[ES_API_Default_Contacts] (Api_Contact_Id,OfficeId) values(2,1)
insert into [dbo].[ES_API_Default_Contacts] (Api_Contact_Id,OfficeId) values(3,1)

----- #8 Configuration for email sending process in the report level end here


----- #9 Report information update start here

ALTER TABLE FB_Report_OtherInformation ADD [Sub_Category2] nvarchar(1000) NULL

ALTER TABLE FB_Report_Problematic_Remarks ADD [Sub_Category] nvarchar(1000) NULL
ALTER TABLE FB_Report_Problematic_Remarks ADD [Sub_Category2] nvarchar(1000) NULL

ALTER TABLE FB_Report_InspDefects ADD [Qty_Reworked] INT NULL
ALTER TABLE FB_Report_InspDefects ADD [Qty_Replaced] INT NULL
ALTER TABLE FB_Report_InspDefects ADD [Qty_Rejected] INT NULL

----- #9 Report information update end here

--#10 email send subject config table starts
CREATE TABLE [dbo].[ES_SU_PreDefined_Fields]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	[Field_Name] nvarchar(max),  
	[Field_Alias_Name] nvarchar(max),
	[Max_Char] int,
	Active bit,
	CreatedBy int,
	CreatedOn datetime default getdate(),
	DeletedBy int,
	DeletedOn datetime,
	UpdatedOn datetime,
	UpdatedBy int
	CONSTRAINT ES_SU_PreDefined_Fields_DeletedBy FOREIGN KEY (DeletedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT ES_SU_PreDefined_Fields_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT ES_SU_PreDefined_Fields_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES [dbo].[IT_UserMaster](Id)
)



CREATE TABLE [dbo].[ES_SU_Template_Master]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	Template_Name nvarchar(max),
	Template_Display_Name nvarchar(max),
	Customer_Id int null,
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
)

CREATE TABLE [dbo].[ES_SU_Template_Details]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	 Field_Id int,
	 Template_Id int,
	 Sort int,
	 Max_Char int,
	 CreatedBy int,
	CreatedOn datetime default getdate(),
	 CONSTRAINT ES_SU_Template_Details_Field_Id FOREIGN KEY (Field_Id) REFERENCES [dbo].[ES_SU_PreDefined_Fields](Id),
	 CONSTRAINT ES_SU_Template_Details_Template_Id FOREIGN KEY (Template_Id) REFERENCES [dbo].[ES_SU_Template_Master](Id),
	 CONSTRAINT ES_SU_Template_Details_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id)
)

	insert into [ES_SU_PreDefined_Fields](field_name,field_alias_Name,Active) values('Supplier','Supplier',1)
	insert into [ES_SU_PreDefined_Fields](field_name,field_alias_Name,Active) values('ServiceDate','Service Date',1)
	insert into [ES_SU_PreDefined_Fields](field_name,field_alias_Name,Active) values('Factory','Factory',1)
	insert into [ES_SU_PreDefined_Fields](field_name,field_alias_Name,Active) values('InspectionID','Inspection Id',1)
	insert into [ES_SU_PreDefined_Fields](field_name,field_alias_Name,Active) values('Customer','Customer',1)
	insert into [ES_SU_PreDefined_Fields](field_name,field_alias_Name,Active) values('ServiceType','Service Type',1)
	insert into [ES_SU_PreDefined_Fields](field_name,field_alias_Name,Active) values('Office','Office',1)
	insert into [ES_SU_PreDefined_Fields](field_name,field_alias_Name,Active) values('ServiceTypeCode','ServiceType Code',1)
	insert into [ES_SU_PreDefined_Fields](field_name,field_alias_Name,Active) values('CustomerBookingNo','Customer Booking No',1)
	insert into [ES_SU_PreDefined_Fields](field_name,field_alias_Name,Active) values('Department','Department',1)
	insert into [ES_SU_PreDefined_Fields](field_name,field_alias_Name,Active) values('DepartmentCode','Department Code',1)
	insert into [ES_SU_PreDefined_Fields](field_name,field_alias_Name,Active) values('Brand','Brand',1)
	insert into [ES_SU_PreDefined_Fields](field_name,field_alias_Name,Active) values('Buyer','Buyer',1)	
	insert into [ES_SU_PreDefined_Fields](field_name,field_alias_Name,Active) values('Collection','Collection',1)
	insert into [ES_SU_PreDefined_Fields](field_name,field_alias_Name,Active) values('Departmentcode','Departmentcode',1)

	insert into [ES_SU_PreDefined_Fields](field_name,field_alias_Name,Active) values('PoNumber','Po Number',1)
	insert into [ES_SU_PreDefined_Fields](field_name,field_alias_Name,Active) values('ProductName','Product Code',1)
	insert into [ES_SU_PreDefined_Fields](field_name,field_alias_Name,Active) values('ProductDesc','ProductDesc',1)
	insert into [ES_SU_PreDefined_Fields](field_name,field_alias_Name,Active) values('Etd','Etd',1)
	insert into [ES_SU_PreDefined_Fields](field_name,field_alias_Name,Active) values('DestinationCountry','Destination Country',1)

	insert into [ES_SU_PreDefined_Fields](field_name,field_alias_Name,Active) values('ReportResult','Report Result',1)

	--menu
declare @IdItem1 as int
INSERT [dbo].[IT_Right] ([ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], MenuName_IdTran) 
VALUES (11, N'Report Sending', N'Report Sending', null, 0, 1, N'fa fa-plus-square', 40,null)
SELECT @IdItem1 = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem1) -- IT-Team	




declare @IdItem as int
INSERT [dbo].[IT_Right] ([ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], MenuName_IdTran) 
VALUES (@IdItem1, NULL, N'Email Subject Config', N'email/sub-config', 0, 1, N'fa fa-plus-square', 40,null)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team	

INSERT [dbo].[IT_Right] ([ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], MenuName_IdTran) 
VALUES (@IdItem1, NULL, N'Email Subject Config Summary', N'email/sub-config-summary', 0, 1, N'fa fa-plus-square', 40,null)
SELECT @IdItem = SCOPE_IDENTITY()
INSERT INTO [dbo].[IT_Role_Right](RoleId,RightId) 
	VALUES(4, @IdItem) -- IT-Team	


--#10 email send subject config table ends

------#11 REF_Budget_Forecast start ----------------
CREATE TABLE [dbo].[REF_Budget_Forecast]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Year] INT NOT NULL, 
    [Month] INT NOT NULL, 
    [CountryId] INT NOT NULL, 
    [ManDay] INT NOT NULL, 
    [Active] BIT NOT NULL, 
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [UpdatedOn] DATETIME NULL, 
    [UpdatedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
    [DeletedBy] INT NULL,
	FOREIGN KEY ([CountryId]) REFERENCES [dbo].[REF_Country](Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)
------#11 REF_Budget_Forecast end ----------------
------#12 Zone-location mapping start ----------------
ALTER TABLE REF_Zone ADD [LocationId] INT NULL
ALTER TABLE REF_Zone ADD CONSTRAINT REF_Zone_LocationId  FOREIGN KEY(LocationId) REFERENCES [REF_Location](Id)
------#12 Zone-location mapping end ----------------


--#13 Report map column added start

ALTER TABLE FB_Report_Details ADD [FB_Report_Map_Id] INT NULL
-- change column value from id to FB_Report_Map_Id
UPDATE FB_Report_Details SET FB_Report_Map_Id = Id;

-- update id column as auto increment 

--#13 Report map column added end


--#14 email config table starts-- 

CREATE TABLE [dbo].[ES_REF_Special_Rule]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1),
	[Name] nvarchar(max),
	[Active] bit,
	[CreatedOn] DATETIME NOT NULL default getdate(),  
  [CreatedBy] int NULL
  CONSTRAINT FK_ES_REF_Special_Rule_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)

CREATE TABLE [dbo].[ES_Special_Rule]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	[Special_Rule_Id] int null,
	[Es_Details_Id] int null,
	[CreatedOn] DATETIME NOT NULL default getdate(),  
  [CreatedBy] int NULL
	CONSTRAINT FK_ES_Special_Rule_Es_Details_Id FOREIGN KEY ([Es_Details_Id]) REFERENCES [dbo].[ES_Details](Id),
	CONSTRAINT FK_ES_Special_Rule_Special_Rule_Id FOREIGN KEY ([Special_Rule_Id]) REFERENCES [dbo].[ES_REF_Special_Rule](Id),
	CONSTRAINT FK_ES_Special_Rule_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)

CREATE TABLE [dbo].[ES_REF_Report_In_Email]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1),
	[Name] nvarchar(max),
	[Active] bit,
	[CreatedOn] DATETIME NOT NULL default getdate(),  
  [CreatedBy] int NULL
  CONSTRAINT FK_ES_REF_Report_In_Email_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)

CREATE TABLE [dbo].[ES_REF_Email_Size]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1),
	[Name] nvarchar(max),
	[Active] bit,
	[CreatedOn] DATETIME NOT NULL default getdate(),  
  [CreatedBy] int NULL
  CONSTRAINT FK_ES_REF_Email_Size_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)

CREATE TABLE [dbo].[ES_REF_Report_Send_Type]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1),
	[Name] nvarchar(max),
	[Active] bit,
	[CreatedOn] DATETIME NOT NULL default getdate(),  
  [CreatedBy] int NULL
  CONSTRAINT FK_ES_REF_Report_Send_Type_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)


Alter table [ES_Result_Config] add [API_Result_Id] INT Null;
Alter table [ES_Result_Config] ADD CONSTRAINT FK_ES_Result_Config_API_Result_Id FOREIGN KEY ([API_Result_Id]) REFERENCES  [dbo].[FB_Report_Result](Id);

Alter table [ES_Result_Config] alter column Customer_Result_Id INT Null;

Alter table [ES_Office_Config] alter column [OfficeId] INT Null;
Alter table [ES_Office_Config] ADD CONSTRAINT ES_Office_Config_Office_Id FOREIGN KEY (OfficeId) REFERENCES  [dbo].[Ref_Location](Id);

Alter table [ES_Details] add Recipient_Name nvarchar(max) Null;
Alter table ES_Details add No_Of_Reports int Null;

Alter table ES_Details add Report_In_Email int Null;
Alter table ES_Details ADD CONSTRAINT FK_ES_Details_Report_In_Email FOREIGN KEY (Report_In_Email) REFERENCES  [dbo].[ES_REF_Report_In_Email](Id);

Alter table ES_Details add Email_Size int Null;
Alter table ES_Details ADD CONSTRAINT FK_ES_Details_Email_Size FOREIGN KEY (Email_Size) REFERENCES  [dbo].[ES_REF_Email_Size](Id);

Alter table ES_Details add Email_Subject int Null;
Alter table ES_Details ADD CONSTRAINT FK_ES_Details_Email_Subject FOREIGN KEY (Email_Subject) REFERENCES  [dbo].[ES_SU_Template_Master](Id);

Alter table ES_Details add Report_Send_Type int Null;
Alter table ES_Details ADD CONSTRAINT FK_ES_Details_Report_Send_Type FOREIGN KEY (Report_Send_Type) REFERENCES  [dbo].[ES_REF_Report_Send_Type](Id);

insert into ES_REF_Special_Rule(name,Active) values('special rule1',1)
insert into ES_REF_Special_Rule(name,Active) values('special rule 2',1)

insert into [ES_REF_Report_In_Email](name,Active) values('Link',1)
insert into [ES_REF_Report_In_Email](name,Active) values('Attachment',1)

insert into [ES_REF_Email_Size](name,Active,Value) values('5',1,5)
insert into [ES_REF_Email_Size](name,Active,Value) values('10',1,10)

insert into [ES_REF_Report_Send_Type](name,Active) values('All Reports',1)
insert into [ES_REF_Report_Send_Type](name,Active) values('All Reports with same result',1)


--#14 email config table ends--

--#15 email config entity table starts--

ALTER TABLE [dbo].[ES_API_Default_Contacts] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[ES_API_Default_Contacts] ADD CONSTRAINT FK_ES_API_Default_Contacts_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

ALTER TABLE [dbo].[ES_Details] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[ES_Details] ADD CONSTRAINT FK_ES_Details_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

ALTER TABLE [dbo].[ES_SU_Template_Master] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[ES_SU_Template_Master] ADD CONSTRAINT FK_ES_SU_Template_Master_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

ALTER TABLE [dbo].[ES_REF_Special_Rule] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[ES_REF_Special_Rule] ADD CONSTRAINT FK_ES_REF_Special_Rule_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

ALTER TABLE [dbo].[ES_REF_Report_Send_Type] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[ES_REF_Report_Send_Type] ADD CONSTRAINT FK_ES_REF_Report_Send_Type_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

--#15 email config entity table ends--



--#16 email subject and config starts----

CREATE TABLE [dbo].[Ref_Delimiter]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1),
	[Name] nvarchar(max),
	[Active] bit,
	[CreatedOn] DATETIME NOT NULL default getdate(),  
  [CreatedBy] int NULL
  CONSTRAINT FK_Ref_Delimiter_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)

CREATE TABLE [dbo].[ES_SU_Module]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1),
	[Name] nvarchar(max),
	[Active] bit,
	[CreatedOn] DATETIME NOT NULL default getdate(),  
  [CreatedBy] int NULL
  CONSTRAINT FK_ES_SU_Module_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)

ALTER TABLE [dbo].[ES_SU_Template_Master] ADD [Email_Type_Id] INT NULL
ALTER TABLE [dbo].[ES_SU_Template_Master] ADD [Module_Id] INT NULL
ALTER TABLE [dbo].[ES_SU_Template_Master] ADD [Delimiter_Id] INT NULL

ALTER TABLE [dbo].[ES_Details] ADD [File_Name_Id] INT NULL

ALTER TABLE [dbo].[ES_SU_Template_Master] ADD CONSTRAINT FK_ES_SU_Template_Master_Module_Id FOREIGN KEY([Module_Id]) REFERENCES [dbo].[ES_SU_Module](Id)
ALTER TABLE [dbo].[ES_SU_Template_Master] ADD CONSTRAINT FK_ES_SU_Template_Master_Email_Type_Id FOREIGN KEY([Email_Type_Id]) REFERENCES [dbo].[ES_Type](Id)
ALTER TABLE [dbo].[ES_SU_Template_Master] ADD CONSTRAINT FK_ES_SU_Template_Master_Delimiter_Id FOREIGN KEY([Delimiter_Id]) REFERENCES [dbo].[Ref_Delimiter](Id)

ALTER TABLE [dbo].[ES_Details] ADD CONSTRAINT FK_ES_Details_File_Name_Id FOREIGN KEY([File_Name_Id]) REFERENCES [dbo].[ES_SU_Template_Master](Id)


insert into [ES_SU_Module](name,active) values('Subject',1)
insert into [ES_SU_Module](name,active) values('File Name',1)

insert into Ref_Delimiter(name,active) values('/',1)
insert into Ref_Delimiter(name,active) values('|',1)
insert into Ref_Delimiter(name,active) values(',',1)
insert into Ref_Delimiter(name,active) values(';',1)

----#16 email subject and config ends------

-----#17 Remark Reference from FB to API Link starts--------

ALTER TABLE FB_Report_Problematic_Remarks ADD CustomerRemarkCode NVARCHAR(1000)

-----#17 Remark Reference from FB to API Link ends--------

--==#18  email send starts --

CREATE TABLE [dbo].[ES_REF_File_Type]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1,1),
	[Name] nvarchar(max),
	[Active] bit,
	[CreatedOn] DATETIME NOT NULL default getdate(),  
  [CreatedBy] int NULL
  CONSTRAINT FK_ES_REF_File_Type_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)

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

--#18 email send  ends

-------#20 Email send table details Starts-------
create table ES_Send_Log(
 Id int primary key identity(1,1),
 Booking_Id int null,
 Report_Id nvarchar(2500),
 Audit_Id int null,
 To_Recipient nvarchar(1500),
 CC_Recipient nvarchar(1500),
 Subject nvarchar(2500),
 Body nvarchar(max),
 Attachment nvarchar(max),
 Created_By int null,
 Created_On DATETIME NOT NULL DEFAULT GETDATE(),
 IsSend bit
 CONSTRAINT FK_ES_Send_Log_Created_By FOREIGN KEY (Created_By) REFERENCES [dbo].[IT_UserMaster](Id),	
 CONSTRAINT FK_ES_Send_Log_Booking_Id FOREIGN KEY (Booking_Id) REFERENCES [dbo].[Insp_transaction](Id),
 CONSTRAINT FK_ES_Send_Log_Auidt_Id FOREIGN KEY (Audit_Id) REFERENCES [dbo].[AUD_Transaction](Id),
 )


 create table ES_REF_RecipientType 
(
Id int primary key identity(1,1),
Name nvarchar(300),
Active bit,
Created_By int null,
Created_On DATETIME NOT NULL DEFAULT GETDATE()
CONSTRAINT ES_REF_RecipientType_Created_By FOREIGN KEY (Created_By) REFERENCES [dbo].[IT_UserMaster](Id)
)


create table ES_Recipient_Type 
(
Id int primary key identity(1,1),
Es_Details_Id int null,
Recipient_Type_Id int null,
IsTo bit,
IsCC bit,
Active bit,
Created_By int null,
Created_On DATETIME NOT NULL DEFAULT GETDATE()
CONSTRAINT ES_Recipient_Type_Created_By FOREIGN KEY (Created_By) REFERENCES [dbo].[IT_UserMaster](Id),
CONSTRAINT ES_Recipient_Type_Es_Details_Id FOREIGN KEY (Es_Details_Id) REFERENCES [dbo].[ES_Details](Id),
CONSTRAINT ES_Recipient_Type_Recipient_Type_Id FOREIGN KEY (Recipient_Type_Id) REFERENCES [dbo].[ES_REF_RecipientType](Id)
)

insert into ES_REF_RecipientType (name,active) values('Customer',1)
insert into ES_REF_RecipientType (name,active) values('Supplier',1)
insert into ES_REF_RecipientType (name,active) values('Factory',1)
insert into ES_REF_RecipientType (name,active) values('API Team',1)

-------#20 Email send table details ends-------

---------------#22 Booking Cancel User Role Starts-------------------

INSERT INTO IT_Role (RoleName, Active, EntityId, PrimaryRole, SecondaryRole)
	VALUES('Accounting-Inspection Cancel', 1, 1, 1, 1)

---------------#22 Booking Cancel User Role Ends---------------

-----------------#23 Updated Custom KPI sp for MDM Starts------------------------

/****** Object:  UserDefinedTableType [dbo].[IntList]    Script Date: 5/7/2021 12:07:30 PM ******/
CREATE TYPE [dbo].[IntList] AS TABLE(
	[Id] [int] NULL
)
GO


/****** Object:  StoredProcedure [dbo].[Defect_KPI_MDM]    Script Date: 5/7/2021 10:20:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Binny>
-- Create date: <5-5-2021>
-- Description:	<MDM KPI>
-- =============================================


CREATE PROCEDURE [dbo].[sp_Defect_KPI_MDM] (
	@CustomerId INT NULL,
	@ServiceDateFrom DATETIME,
	@ServiceDateTo DATETIME,
	@TemplateId INT NULL,
	@OfficeList IntList NULL READONLY ,
	@ServiceTypeList IntList NULL READONLY,
	@InvoiceNo nvarchar(100) NULL)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN  TRY

			DECLARE @tblresult TABLE
			(
			bookingNo int,
			serviceDate nvarchar(100),
			customerName nvarchar(1000),
			supplierName nvarchar(1000),
			factoryName nvarchar(1000),
			serviceType nvarchar(1000),
			inspectionStatus nvarchar(1000),
			productRef nvarchar(1000),
			productDesc nvarchar(1000),
			poNumber nvarchar(max),
			poQty int,
			inspectedQty int,
			totalDefects int,
			totalCritical int,
			totalMajor int,
			totalMinor int,
			totalQtyReworked int,
			totalQtyReplaced int,
			totalQtyRejected int,
			finalResult nvarchar(1000),
			productid int,
			reportid int,
			department nvarchar(1000),
			FactoryCountry nvarchar(1000)
			)
			DECLARE @tbldefects Table
			(
			bookingno int,
			productid int,
			reportid int,
			totalCritical int,
			totalMajor int,
			totalMinor int,
			totalQtyReworked int,
			totalQtyReplaced int,
			totalQtyRejected int
			)

			insert into @tblresult(bookingNo,serviceDate,customerName,supplierName,factoryName,serviceType,inspectionStatus,productRef,productDesc,
			poQty,poNumber,inspectedQty,finalResult,productid,reportid, department,FactoryCountry)
			SELECT insp.Id 'Ins#',case WHEN insp.ServiceDate_From=insp.ServiceDate_To THEN CONVERT(VARCHAR(100), insp.ServiceDate_From,103) else 
			CONVERT(VARCHAR(100), insp.ServiceDate_From,103)+'-'+CONVERT(VARCHAR(100), insp.ServiceDate_To,103) END 'ins date',cu.Customer_Name 'Customer Name',su.Supplier_Name 'Supplier Name'
			,suf.Supplier_Name 'Factory Name' , refs.Name 'Service Type',insps.Status 'Inspection Status',cup.ProductID 'Product Code',
			cup.[Product Description] 'Product Desc', ipt.TotalBookingQuantity 'Po Qty',
			(SELECT STUFF((SELECT   ',' + convert(varchar(1000),cupo.PONO)  from INSP_PurchaseOrder_Transaction ito 
			join CU_PurchaseOrder cupo on cupo.Id=ito.PO_Id
			where cupo.Active=1 AND ito.Inspection_Id=insp.Id and ito.Product_Ref_Id=ipt.Id FOR XML PATH ('')),1,1,''))'PO Number',
			(SELECT sum(ISNULL(fbq.InspectedQuantity,0)) from FB_Report_Quantity_Details fbq 
			where fbq.FbReportDetailId=ipt.Fb_Report_Id and fbq.InspPoTransactionId in (SELECT ipt2.Id from INSP_PurchaseOrder_Transaction ipt2
			where ipt2.Active=1 and ipt2.Inspection_Id=ipt.Inspection_Id and ipt2.Product_Ref_Id=ipt.Id) and fbq.Active=1)'Inspected Qty' ,
			isnull(fb.OverAllResult,'')'Final Result',ipt.Product_Id,ipt.Fb_Report_Id, dept.[Name],con.Country_Name
			from INSP_Transaction insp
			join INSP_Product_Transaction ipt on insp.Id=ipt.Inspection_Id
			left join FB_Report_Details fb on fb.Id=ipt.Fb_Report_Id
			join CU_Products cup on cup.Id=ipt.Product_Id and cup.Active=1
			join CU_Customer cu on cu.Id=insp.Customer_Id
			join SU_Supplier su on su.Id=insp.Supplier_Id
			join SU_Supplier suf on suf.Id=insp.Factory_Id
			JOIN SU_Address sud on sud.Supplier_Id=suf.Id and sud.AddressTypeId=1
			JOIN REF_Country con on con.Id=sud.CountryId
			join INSP_TRAN_ServiceType ser on ser.Inspection_Id=insp.Id and ser.Active=1
			join REF_ServiceType refs on refs.Id=ser.ServiceType_Id 
			join INSP_Status insps on insps.Id=insp.Status_Id
			LEFT join INSP_TRAN_CU_Brand insp_brand on insp_brand.Inspection_Id=insp.Id and insp_brand.Active = 1
			LEFT join INSP_TRAN_CU_Department insp_dept on insp_dept.Inspection_Id=insp.Id and insp_dept.Active = 1
			LEFT join CU_Department dept on dept.id = insp_dept.Department_Id

			where insp.Customer_Id = COALESCE(NULLIF(@CustomerId, ''), insp.Customer_Id) and insp.Status_Id!=4 and ipt.Active=1 and
			insp.ServiceDate_To BETWEEN  @FromDate and @ToDate --mm/dd/yyyy 
			AND (NOT EXISTS(SELECT 1 FROM @OfficeIdList) OR insp.Office_Id IN (SELECT * FROM @OfficeIdList))
			AND (NOT EXISTS(SELECT 1 FROM @ServiceTypeIdList) OR refs.Id IN (SELECT * FROM @ServiceTypeIdList))
			AND (NOT EXISTS(SELECT 1 FROM @BrandIdList) OR insp_brand.Brand_Id IN (SELECT * FROM @BrandIdList))
			AND (NOT EXISTS(SELECT 1 FROM @DepartmentIdList) OR insp_dept.Department_Id IN (SELECT * FROM @DepartmentIdList))
			order by Ins#

			--SELECT * from @tblresult

			INSERT INTO @tbldefects(productid,reportid,totalCritical,totalMajor,totalMinor,totalQtyReworked,totalQtyReplaced,totalQtyRejected,bookingno)
			SELECT ipt.Product_Id,ipt.Fb_Report_Id,sum(isnull( fbq.Critical,0))'critical',
			sum(isnull( fbq.Major,0))'Major',sum(isnull( fbq.Minor,0))'Minor',
			sum(isnull( fbq.Qty_Reworked,0))'Qc Reworked',sum(isnull( fbq.Qty_Replaced,0))'QC Replaced',
			sum(isnull( fbq.Qty_Rejected,0))'QC Rejected' , ipt.Inspection_Id
			from FB_Report_InspDefects fbq
			join FB_Report_Details fbr on fbr.Id=fbq.FbReportDetailId
			join INSP_PurchaseOrder_Transaction  inpo on inpo.Id=fbq.InspPoTransactionId
			join INSP_Product_Transaction ipt on ipt.Id=inpo.Product_Ref_Id
			where ipt.Inspection_Id in (SELECT bookingNo from @tblresult) and fbq.Active=1 and inpo.Active=1 and ipt.Active=1 and fbr.Active=1
			group by ipt.Product_Id,ipt.Fb_Report_Id,ipt.Inspection_Id

			--SELECT * from @tbldefects

			UPDATE r SET  r.totalDefects=(d.totalCritical+d.totalMajor+d.totalMinor),
			r.totalCritical=d.totalCritical,r.totalMajor=d.totalMajor,r.totalMinor=d.totalMinor,
			r.totalQtyReworked=d.totalQtyReworked,r.totalQtyReplaced=d.totalQtyReplaced,r.totalQtyRejected=d.totalQtyRejected
			from @tblresult r join @tbldefects d on r.productid=d.productid and r.reportid=d.reportid and r.bookingNo=d.bookingno 


			SELECT bookingNo ,
			serviceDate ,
			customerName ,
			supplierName ,
			factoryName ,
			serviceType ,
			inspectionStatus ,
			productRef ,
			productDesc ,
			poNumber ,
			poQty ,
			ISNULL(inspectedQty,0)AS inspectedQty ,
			ISNULL(totalDefects,0)AS totalDefects ,
			ISNULL(totalCritical,0)AS totalCritical ,
			ISNULL(totalMajor,0)AS totalMajor ,
			ISNULL(totalMinor,0)AS totalMinor ,
			ISNULL(totalQtyReworked,0)AS totalQtyReworked ,
			ISNULL(totalQtyReplaced,0)AS totalQtyReplaced ,
			ISNULL(totalQtyRejected,0)AS totalQtyRejected ,
			finalResult ,
			productid ,
			--reportid ,
			ISNULL(Department,'') AS Department,
			FactoryCountry
			
			from @tblresult order by bookingNo


		END TRY

		BEGIN CATCH
			
			THROW

		END CATCH


END
GO
-----------------#23 Updated Custom KPI sp for MDM Starts------------------------

---------------#24 ES_REF_Email_Size column  starts ---------------
alter table ES_REF_Email_Size add Value float Null;
---------------#24 ES_REF_Email_Size column Ends---------------


ALTER TABLE LOG_Email_Queue_Attachments add FileStorageType INT null;
ALTER TABLE LOG_Email_Queue_Attachments add FileUniqueId nvarchar(2000) null;
ALTER TABLE LOG_Email_Queue_Attachments add FileLink nvarchar(2000) null;

 CREATE TABLE LOG_Booking_Report_Email_Queue
 (
     [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
	 Inspection_Id int null,
	 Report_Id int null,
	 Audit_Id int null,
	 Es_Type_Id int null,
	 Email_Log_Id int null,
     CONSTRAINT FK_LOG_Booking_Report_Email_Queue_Inspection_Id FOREIGN KEY (Inspection_Id) REFERENCES [dbo].[Insp_transaction](Id),
     CONSTRAINT FK_LOG_Booking_Report_Email_Queue_Report_Id FOREIGN KEY (Report_Id) REFERENCES [dbo].[FB_Report_Details](Id),
     CONSTRAINT FK_LOG_Booking_Report_Email_Queue_Audit_Id FOREIGN KEY (Audit_Id) REFERENCES [dbo].[AUD_transaction](Id),
	 CONSTRAINT FK_LOG_Booking_Report_Email_Queue_Email_Log_Id FOREIGN KEY (Email_Log_Id) REFERENCES [dbo].[LOG_Email_Queue](Id),
	 CONSTRAINT FK_LOG_Booking_Report_Email_Queue_Es_Type_Id FOREIGN KEY (Es_Type_Id) REFERENCES [dbo].[ES_Type](Id)
 )

 -------------#26 Add Buyer Name in IC Transaction starts-------------------

 ALTER TABLE INSP_IC_Transaction ADD Buyer_Name NVARCHAR(1000)
  -------------#26 Add Buyer Name in IC Transaction Ends-------------------

-----------------#27  Edit Inspection Booking Starts------------------------

ALTER TABLE INSP_Transaction ADD IsCombineRequired BIT

-----------------#27  Edit Inspection Booking Ends------------------------

---------------#28 table for onsite CS email starts-------------------------
CREATE TABLE [dbo].[CU_CS_Onsite_Email]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [UserId] INT NOT NULL, 
    [CustomerId] INT NOT NULL, 
    [EmailId] NVARCHAR(MAX) NOT NULL, 
    [Active] BIT NOT NULL,
	FOREIGN KEY ([UserId]) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY ([CustomerId]) REFERENCES Cu_Customer(Id)
)
---------------#28 table for onsite CS email ends-------------------------

-----------------#29  Customer Product script starts------------------------

ALTER TABLE cu_products ADD IsNewProduct BIT

-----------------#29  Customer Product script ends------------------------


-----------------#30  Email Subject Enhancement Starts------------------------

CREATE TABLE [dbo].[ES_SU_DataType]
(
Id INT PRIMARY KEY IDENTITY(1,1),
Data NVARCHAR(20)
)

INSERT INTO [dbo].[ES_SU_DataType] (Data) VALUES ('Date')
INSERT INTO [dbo].[ES_SU_DataType] (Data) VALUES ('List')

ALTER TABLE [dbo].[ES_SU_PreDefined_Fields] ADD IsText BIT

ALTER TABLE [dbo].[ES_SU_PreDefined_Fields] ADD [DataType] INT NULL
ALTER TABLE [dbo].[ES_SU_PreDefined_Fields] ADD CONSTRAINT FK_SU_DataType FOREIGN KEY(DataType) REFERENCES [dbo].[ES_SU_DataType](Id)

CREATE TABLE [dbo].[REF_DateFormat]
(
Id INT PRIMARY KEY IDENTITY(1,1),
DateFormat NVARCHAR(20)
)

INSERT INTO [dbo].[REF_DateFormat] (DateFormat) Values ('dd/MM/yyyy')
INSERT INTO [dbo].[REF_DateFormat] (DateFormat) Values ('MM/dd/yyyy')
INSERT INTO [dbo].[REF_DateFormat] (DateFormat) Values ('dd/MMM/yyyy')


ALTER TABLE ES_SU_Template_Details ADD IsTitle BIT

ALTER TABLE ES_SU_Template_Details ADD TitleCustomName NVARCHAR(400)

ALTER TABLE [dbo].[ES_SU_Template_Details] ADD Max_Items INT

ALTER TABLE [dbo].[ES_SU_Template_Details] ADD DateFormat INT
ALTER TABLE [dbo].[ES_SU_Template_Details] ADD CONSTRAINT FK_Date_Format FOREIGN KEY(DateFormat) REFERENCES [dbo].[REF_DateFormat](Id)

ALTER TABLE [dbo].[ES_SU_Template_Details] ADD IsDateSeperator BIT

ALTER TABLE [dbo].[Ref_Delimiter] ADD Is_File BIT

-----------------#30  Email Subject Enhancement Ends------------------------

---------------#31 Finance Dashboard sp ends-------------------------

ALTER TABLE REF_Budget_Forecast ADD Fees float 

ALTER TABLE REF_Budget_Forecast ADD CurrencyId int
ALTER TABLE REF_Budget_Forecast ADD CONSTRAINT budget_forecast_currency_fk FOREIGN KEY (CurrencyId) REFERENCES dbo.REF_Currency (Id)


/****** Object:  StoredProcedure [dbo].[Usp_FinanceKPI_GetBilledMandayDetails]    Script Date: 6/23/2021 12:38:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Binny
-- Create date: 22-06-2021
-- Description: Billed Man day data
-- =============================================
CREATE PROCEDURE [dbo].[Usp_FinanceKPI_GetBilledMandayDetails] 
		--@BookingIdList IntList READONLY,
		@ServiceDateFrom DATETIME,
		@ServiceDateTo DATETIME,
		@EntityId INT NULL
AS
BEGIN

	SELECT Month(ServiceDate_To) AS [Month], FORMAT(ServiceDate_To, 'MMM') AS [MonthName],
	Year(ServiceDate_To) AS [Year], 
	SUM(NoOfManday) AS [MonthManDay] 
	FROM INSP_Transaction insp
	INNER JOIN QU_Quotation_Insp quot ON quot.IdBooking = insp.Id
	INNER JOIN QU_QUOTATION q on q.id = quot.IdQuotation
	WHERE ServiceDate_To BETWEEN  @ServiceDateFrom and @ServiceDateTo
	AND insp.Status_Id != 4 AND q.IdStatus != 5
	GROUP BY YEAR(ServiceDate_To), MONTH(ServiceDate_To), FORMAT(ServiceDate_To, 'MMM')
	 UNION 

	SELECT Month(ServiceDate_To) AS [Month], FORMAT(ServiceDate_To, 'MMM') AS [MonthName],
	Year(ServiceDate_To) AS [Year], 
	SUM(NoOfManday) AS [MonthManDay] 
	FROM INSP_Transaction insp
	INNER JOIN QU_Quotation_Insp quot ON quot.IdBooking = insp.Id
	INNER JOIN QU_QUOTATION q on q.id = quot.IdQuotation
	WHERE ServiceDate_To BETWEEN DATEADD(year, -1,  @ServiceDateFrom) and DATEADD(year, -1,  @ServiceDateTo) 
	AND insp.Status_Id != 4 AND q.IdStatus != 5
	GROUP BY YEAR(ServiceDate_To), MONTH(ServiceDate_To), FORMAT(ServiceDate_To, 'MMM')

	--select [Month], [MonthName], [Year], [MonthManDay] FROM  #CurrentYearMandayTable UNION select [Month], [MonthName], [Year], [MonthManDay] FROM  #LastYearMandayTable

	SELECT [Month], FORMAT([Month], 'MMM') AS [MonthName], 
	SUM(ManDay) AS [MonthManDay]
	FROM REF_Budget_Forecast 
	WHERE [Year] = YEAR(GETDATE()) AND Active = 1
	GROUP BY [Month]

END
GO

/****** Object:  StoredProcedure [dbo].[Usp_FinanceKPI_GetMandayRateDetails]    Script Date: 6/23/2021 12:39:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Binny
-- Create date: 22-06-2021
-- Description:	get man day rate
-- =============================================
CREATE PROCEDURE [dbo].[Usp_FinanceKPI_GetMandayRateDetails] 
		--@BookingIdList IntList READONLY,
		@ServiceDateFrom DATETIME,
		@ServiceDateTo DATETIME,
		@EntityId INT NULL
AS
BEGIN

	SELECT CAST(Row_number()
         OVER(
           ORDER BY (SELECT NULL)) AS INT)AS Id,
	Month(insp.ServiceDate_To) AS [Month], FORMAT(insp.ServiceDate_To, 'MMM') AS [MonthName],
	Year(insp.ServiceDate_To) AS [Year], 
	NoOfManday AS [MonthManDay], inv.inspectionFees AS [InspFees], inv.InvoiceCurrency AS [CurrencyId] 
	FROM INSP_Transaction insp
	INNER JOIN QU_Quotation_Insp quot ON quot.IdBooking = insp.Id
	INNER JOIN QU_QUOTATION q ON q.Id = quot.IdQuotation
	INNER JOIN INV_AUT_TRAN_Details inv ON inv.InspectionId = insp.Id	
	WHERE ServiceDate_To BETWEEN  @ServiceDateFrom and @ServiceDateTo
	AND inv.Active = 1 AND q.IdStatus != 5 AND insp.Status_Id != 4
	GROUP BY YEAR(ServiceDate_To), MONTH(ServiceDate_To), FORMAT(ServiceDate_To, 'MMM'), NoOfManday, inv.InspectionFees, InvoiceCurrency

	UNION

	SELECT CAST(Row_number()
         OVER(
           ORDER BY (SELECT @@ROWCOUNT)) AS INT)AS Id,
	Month(ServiceDate_To) AS [Month], FORMAT(ServiceDate_To, 'MMM') AS [MonthName],
	Year(ServiceDate_To) AS [Year], 
	NoOfManday AS [MonthManDay], inv.inspectionFees AS [InspFees], inv.InvoiceCurrency AS [CurrencyId] 
	FROM INSP_Transaction insp
	INNER JOIN QU_Quotation_Insp quot ON quot.IdBooking = insp.Id
	INNER JOIN QU_QUOTATION q ON q.Id = quot.IdQuotation
	INNER JOIN INV_AUT_TRAN_Details inv ON inv.InspectionId = insp.Id
	WHERE ServiceDate_To BETWEEN DATEADD(year, -1,  @ServiceDateFrom) and DATEADD(year, -1,  @ServiceDateTo) 
	AND inv.Active = 1 AND q.IdStatus != 5 AND insp.Status_Id != 4
	GROUP BY YEAR(ServiceDate_To), MONTH(ServiceDate_To), FORMAT(ServiceDate_To, 'MMM'), NoOfManday, inv.InspectionFees, InvoiceCurrency



	SELECT CAST(Row_number()
         OVER(
           ORDER BY (SELECT NULL)) AS INT) AS Id, [Month], FORMAT([Month], 'MMM') AS [MonthName],
	fees AS [InspFees], ManDay AS [MonthManDay],
	CurrencyId AS [CurrencyId]
	FROM REF_Budget_Forecast 
	WHERE [Year] = YEAR(GETDATE()) AND Active = 1
	GROUP BY [Month], fees, ManDay, CurrencyId
END
GO


/****** Object:  StoredProcedure [dbo].[Usp_FinanceKPI_GetTurnOver]    Script Date: 6/23/2021 12:40:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Binny
-- Create date: 15-6-2021
-- Description:	get the invoice amount by country
-- =============================================
CREATE PROCEDURE [dbo].[Usp_FinanceKPI_GetTurnOver]
	
	@BookingIdList IntList NULL READONLY,
	@EntityId INT NULL

AS
BEGIN
	
	SELECT insp.Id as [BookingId], inv.TotalInvoiceFees as [TotalInvoiceFee], inv.InvoiceCurrency as [InvoiceCurrencyId], exf.TotalExtraFee as [TotalExtraFee], exf.CurrencyId as [ExtraFeeCurrencyId], country.Id as [CountryId], country.Country_Name as [CountryName], ser.ServiceType_Id as [ServiceTypeId], refs.[Name] as [ServiceTypeName],
	prod.ProductCategory as [ProdCategoryId], cat.[Name] as [ProdCategoryName]
	FROM INSP_TRANSACTION insp
	LEFT JOIN INV_AUT_TRAN_Details inv on inv.InspectionId = insp.Id
	LEFT JOIN INV_EXF_Transaction exf on exf.InspectionId = insp.Id
	INNER JOIN SU_Address sup ON sup.Supplier_Id = insp.Factory_Id AND sup.AddressTypeId = 1
	INNER JOIN REF_Country country ON country.Id = sup.CountryId
	INNER JOIN INSP_TRAN_ServiceType ser on ser.Inspection_Id=insp.Id and ser.Active=1
	INNER JOIN REF_ServiceType refs on refs.Id=ser.ServiceType_Id 

	 

	INNER JOIN (select Inspection_id, Product_id, Active, row_number() over (
            partition by inspection_id
			ORDER BY inspection_id
        ) as row_num
		FROM INSP_Product_Transaction) as inspProd
		ON inspProd.Inspection_Id = insp.Id AND row_num = 1 AND inspProd.Active = 1

		--INNER JOIN (select top 1 Inspection_id, Product_id, Active from INSP_Product_Transaction)  inspProd ON inspProd.Inspection_Id = insp.Id AND inspProd.Active = 1
	
	
	--INSP_Product_Transaction inspProd ON inspProd.Inspection_Id = insp.Id
	INNER JOIN CU_Products prod ON prod.Id = inspProd.Product_Id
	INNER JOIN REF_ProductCategory cat ON cat.Id = prod.ProductCategory

	WHERE insp.Id IN (SELECT Id FROM @BookingIdList)

END
GO

/****** Object:  StoredProcedure [dbo].[Usp_FinanceKPI_GetChargeBack]    Script Date: 6/23/2021 12:40:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Binny
-- Create date: 17-6-2021
-- Description:	get the total travel expense claim and total travel invoice amount
-- =============================================
CREATE PROCEDURE [dbo].[Usp_FinanceKPI_GetChargeBack]

	@BookingIdList IntList NULL READONLY,
	@EntityId INT NULL
AS
BEGIN
	
	SELECT SUM(Amount) FROM EC_ExpenseClaimsInspection expInsp
	INNER JOIN EC_ExpensesClaimDetais expDetails ON expDetails.Id = expInsp.ExpenseClaimDetailId
	INNER JOIN EC_ExpensesTypes expType ON expType.Id = expDetails.ExpenseTypeId
	WHERE expType.IsTravel = 1
	AND  expInsp.BookingId IN (SELECT Id FROM @BookingIdList)

	SELECT insp.id AS BookingId, InvoiceCurrency AS [InvoiceCurrencyId], TotalInvoiceFees AS [TotalInvoiceFee], TravelTotalFees , 
	TotalExtraFee AS [TotalExtraFee], exf.CurrencyId AS [ExtraFeeCurrencyId], invoice.TravelIncluded AS [PriceCardTravelIncluded]
	FROM INSP_TRANSACTION insp
	LEFT JOIN (select ruleId, InvoiceCurrency, TotalInvoiceFees, TravelTotalFees, InspectionId, TravelIncluded from INV_AUT_TRAN_Details inv 
	INNER JOIN CU_PR_Details pc ON pc.Id = inv.RuleId) invoice on invoice.InspectionId = insp.Id
	LEFT JOIN INV_EXF_Transaction exf on exf.InspectionId = insp.Id

	WHERE insp.Id IN (SELECT Id FROM @BookingIdList)
	-- AND pc.TravelIncluded = 0
END
GO

/****** Object:  StoredProcedure [dbo].[Usp_FinanceKPI_GetRejectQuotationDetails]    Script Date: 6/23/2021 12:41:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Binny
-- Create date: 23-06-2021
-- Description:	quotation count and customer rejected quotation count
-- =============================================
CREATE PROCEDURE [dbo].[Usp_FinanceKPI_GetRejectQuotationDetails]
	
	@BookingIdList IntList NULL READONLY,
	@EntityId INT NULL

AS
BEGIN
	DECLARE @TotalQuotation INT
	DECLARE @RejectedQuotation INT

	SELECT @TotalQuotation = COUNT(DISTINCT(quotInsp.IdQuotation))  FROM QU_Quotation_Insp quotInsp
	INNER JOIN QU_QUOTATION quot ON quot.id = quotInsp.IdQuotation
	WHERE quotInsp.IdBooking IN (SELECT * FROM @BookingIdList)
	AND quot.IdStatus != 5

	SELECT @RejectedQuotation = COUNT(DISTINCT(quotInsp.IdQuotation)) FROM QU_Quotation_Insp quotInsp
	--INNER JOIN QU_QUOTATION quot ON quot.id = quotInsp.IdQuotation
	INNER JOIN QU_TRAN_Status_Log quotLog ON quotLog.QuotationId = quotInsp.IdQuotation
	WHERE quotInsp.IdBooking IN (SELECT * FROM @BookingIdList)
	AND quotlog.StatusId = 6

	SELECT @TotalQuotation AS [QuotationCount], @RejectedQuotation AS [RejectedQuotationCount]
END
GO

---------------#31 table for Finance Dashboard sp ends-------------------------

-----------------#32 Customer booking no in Audit Starts------------------------

 ALTER TABLE AUD_Transaction ADD CustomerBookingNo NVARCHAR(1000)

-----------------#32 Customer booking no in Audit End------------------------

-----------------#33  changing PoNumber as PoNumberList start------------------------
update ES_SU_PreDefined_Fields set Field_Name='PoNumberList' where id=16
-----------------#33  changing PoNumber as PoNumberList end------------------------

-----------------#33  Validate info in Quotation Starts------------------------
ALTER TABLE [dbo].[QU_QUOTATION] ADD [ValidatedBy] INT NULL
ALTER TABLE [dbo].[QU_QUOTATION] ADD CONSTRAINT FK_QU_QUOTATION_ValidatedBy FOREIGN KEY(ValidatedBy) REFERENCES [dbo].[IT_UserMaster](Id)

ALTER TABLE [dbo].[QU_QUOTATION] ADD [ValidatedOn] DATETIME NULL

-----------------#33  Validate info in Quotation Ends------------------------

---------#35 CS Dashboard starts----------------

/****** Object:  StoredProcedure [dbo].[Usp_CSDashboard_GetMandayByOffice]    Script Date: 30/06/2021 12:36:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ============================================= 
-- Author:  Ronika 
-- Create date: 16-6-2021 
-- Description: get the office list with manday count by booking id list
-- =============================================  
CREATE PROCEDURE [dbo].[Usp_CSDashboard_GetMandayByOffice] (   
 @BookingIdList IntList NULL READONLY,  
 @EntityId INT NULL)  
AS  
BEGIN  
select refloc.id,refloc.location_name as Name, sum(quinsp.NoOfManday) as [Count] 
from qu_quotation_insp quinsp inner join insp_transaction insptran  on insptran.id = quinsp.idbooking
inner join ref_location refloc on insptran.office_id = refloc.id
WHERE 
(NOT EXISTS(SELECT 1 FROM @bookingIdList) OR insptran.id IN (SELECT * FROM @bookingIdList))
AND (refloc.Active = 1)
GROUP BY refloc.id, refloc.Location_Name
END  

GO




/****** Object:  StoredProcedure [dbo].[Usp_CSDashboard_GetModuleStatus]    Script Date: 30/06/2021 12:36:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================           
-- Author:  Ronika           
-- Create date: 18-6-2021           
-- Description: get the booking, quotation, allocation, report status list by booking id list          
-- =============================================            
CREATE PROCEDURE [dbo].[Usp_CSDashboard_GetModuleStatus] (             
 @BookingIdList IntList NULL READONLY,            
 @RoleIdList IntList NULL READONLY,            
 @UserId int null,          
 @EntityId INT NULL)            
AS            
        
BEGIN          
        
Declare @StatusTable TABLE(            
Id int,          
Name nvarchar(100),        
[Count] int,        
ActionType int        
)            
        
--inpection status           
Insert into @StatusTable (Id,Name,[Count],ActionType)         
select inspstatus.Id as Id , inspstatus.Status as Name , count(insptran.id) as Count, 1 as ActionType from          
insp_status inspstatus left join insp_transaction insptran            
on insptran.Status_Id = inspstatus.Id      
and (NOT EXISTS(SELECT 1 FROM @BookingIdList) OR insptran.id IN (SELECT * FROM @BookingIdList))          
where inspstatus.Active = 1 and  inspstatus.id not in (9)  --9 - allocated status    
GROUP BY inspstatus.Id, inspstatus.Status        
            
-- inspection status task count          
Insert into @StatusTable (Id,Name,[Count],ActionType)         
select midtask.tasktypeid as Id, '' as Name, count(midtask.tasktypeid) as Count, 2 as ActionType from          
it_userrole userrole left join mid_task midtask  on userrole.UserId = midtask.UserId          
where (          
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR midtask.LinkId IN (SELECT * FROM @BookingIdList)) and          
(userrole.UserId = @UserId and midtask.isdone = 0)           
 --and  (userrole.RoleId in(24) and midtask.TaskTypeId in(6))          
 and           
 (          
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 25)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 25))  and midtask.TaskTypeId in(5,8))          
 or          
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 24)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 24))  and midtask.TaskTypeId in(6))          
 ))          
 --25 - Inspection Verified, 24 - Inspection Confirmed, 5 -Verify Inspection   , 6-Confirm Inspection   , 8 -SplitInspectionBooking.          
group by midtask.tasktypeid           
          
--add quotation pending status      
      
--booking --8- verified, 2 - confirmed      
--quotation required - 1 - customer check point      
      
Insert into @StatusTable (Id,Name,[Count],ActionType)         
 select 0 as Id , 'Pending' as Name , count(insptran.id) as [Count], 3 as ActionType from insp_transaction insptran       
inner join CU_CheckPoints cucheckpoint on cucheckpoint.CustomerId = insptran.Customer_Id      
where insptran.Status_Id in (8,2) and cucheckpoint.CheckpointTypeId = 1       
      
      
-- quotation status count      
Insert into @StatusTable (Id,Name,[Count],ActionType)         
select qustatus.Id as Id , qustatus.Label as Name , count(ququotation.id) as [Count], 3 as ActionType from          
qu_status qustatus left join qu_quotation ququotation         
on ququotation.IdStatus = qustatus.Id          
left join qu_quotation_insp quinsp on quinsp.IdQuotation = ququotation.Id      
and (NOT EXISTS(SELECT 1 FROM @BookingIdList) OR quinsp.IdQuotation IN (SELECT * FROM @BookingIdList))          
where qustatus.Active = 1      
GROUP BY qustatus.Id , qustatus.Label        
            
-- quotation status task count         
Insert into @StatusTable (Id,Name,[Count],ActionType)         
select midtask.tasktypeid as Id, '' as Name, count(midtask.tasktypeid) as [Count], 4 as ActionType from          
it_userrole userrole left join mid_task midtask  on userrole.UserId = midtask.UserId          
where (          
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR midtask.LinkId IN (SELECT * FROM @BookingIdList)) and          
(userrole.UserId = @UserId and midtask.isdone = 0)           
 and       
 (          
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 20)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 20))  and midtask.TaskTypeId in(10,14))          
 or          
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 21)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 21))  and midtask.TaskTypeId in(7))      
 or      
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 22)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 22))  and midtask.TaskTypeId in(12,13))      
 or      
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 27)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 27))  and midtask.TaskTypeId in(11))      
 ))          
group by midtask.tasktypeid               
-- role - 20-Quotation Request, 21 Quotation Manager, 22 Quotation Confirmation,27 Quotation Send      
--task 14 Quotation Pending,10 Quotation Modify,7 quotation approve, 12 Quotation Customer Confirmed,13 Quotation Customer Reject,11 Quotation Sent      
      
--inpection allocation status         
Insert into @StatusTable (Id,Name,[Count],ActionType)         
select insptran.Status_Id as Id , inspstatus.Status as Name , count(insptran.id) as [Count], 5 as ActionType from          
insp_status inspstatus left join insp_transaction insptran            
on insptran.Status_Id = inspstatus.Id          
and (NOT EXISTS(SELECT 1 FROM @BookingIdList) OR insptran.id IN (SELECT * FROM @BookingIdList))          
where inspstatus.Active = 1 and  inspstatus.id in (2,9)  --9 - allocated status , 2 - confirmed        
GROUP BY insptran.Status_Id, inspstatus.Status             
          
-- inspection allocation status task count         
Insert into @StatusTable (Id,Name,[Count],ActionType)         
select midtask.tasktypeid as Id, '' as Name, count(midtask.tasktypeid) as [Count], 6 as ActionType from          
it_userrole userrole left join mid_task midtask  on userrole.UserId = midtask.UserId          
where (          
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR midtask.LinkId IN (SELECT * FROM @BookingIdList)) and          
(userrole.UserId = @UserId and midtask.isdone = 0)           
 and           
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 26)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 26))  and midtask.TaskTypeId in(9))            
 )          
 --26 Inspection Schedule - role --9 Inspection Schedule - task type        
group by midtask.tasktypeid           
            
  --product tran fb report filling status count        
Insert into @StatusTable (Id,Name,[Count],ActionType)  
select fbstatus.id,'QC-' +fbstatus.StatusName,count(tbl.fbreportId) as [Count],7 as ActionType from fb_status fbstatus  
left join   
(select fbreport.Fb_Filling_Status,fbreport.id as fbreportId from fb_report_details fbreport 
inner join insp_product_transaction inspprod on inspprod.Fb_Report_Id = fbreport.Id 
where 
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR inspprod.Inspection_Id  IN (SELECT * FROM @BookingIdList))          
and fbreport.Active = 1 and inspprod.Active = 1) as tbl   
on tbl.Fb_Filling_Status = fbstatus.Id
where fbstatus.Type=3  and fbstatus.Active = 1 
GROUP BY fbstatus.id,fbstatus.StatusName

--container tran fb report filling status count        
  Insert into @StatusTable (Id,Name,[Count],ActionType)   
  select fbstatus.id,'QC-' +fbstatus.StatusName,count(tbl.fbreportId) as cnt,7 as ActionType from fb_status fbstatus  
left join   
(select fbreport.Fb_Filling_Status,fbreport.id as fbreportId from fb_report_details fbreport 
inner join insp_container_transaction inspcont on inspcont.Fb_Report_Id = fbreport.Id 
where inspcont.Inspection_Id in (4,5) and fbreport.Active = 1 and inspcont.Active = 1  )as tbl   
on tbl.Fb_Filling_Status = fbstatus.Id
where fbstatus.Type=3  and fbstatus.Active = 1 
GROUP BY fbstatus.id,fbstatus.StatusName


--product tran fb report reivew status count        
Insert into @StatusTable (Id,Name,[Count],ActionType)   
select fbstatus.id,'CS-' +fbstatus.StatusName,count(tbl.fbreportId) as [Count],7 as ActionType 
from fb_status fbstatus  
left join   
(select fbreport.Fb_Review_Status,fbreport.id as fbreportId from fb_report_details fbreport 
inner join insp_product_transaction inspprod on inspprod.Fb_Report_Id = fbreport.Id 
where 
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR inspprod.Inspection_Id  IN (SELECT * FROM @BookingIdList))          
and fbreport.Active = 1 and inspprod.Active = 1) as tbl   
on tbl.Fb_Review_Status = fbstatus.Id
where fbstatus.Type=4  and fbstatus.Active = 1 
GROUP BY fbstatus.id,fbstatus.StatusName

--container tran fb report reivew status count        
Insert into @StatusTable (Id,Name,[Count],ActionType)   
select fbstatus.id,'CS-' +fbstatus.StatusName,count(tbl.fbreportId) as [Count],7 as ActionType 
from fb_status fbstatus  
left join   
(select fbreport.Fb_Review_Status,fbreport.id as fbreportId from fb_report_details fbreport 
inner join insp_container_transaction inspcont on inspcont.Fb_Report_Id = fbreport.Id 
where 
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR inspcont.Inspection_Id  IN (SELECT * FROM @BookingIdList))          
and fbreport.Active = 1 and inspcont.Active = 1) as tbl   
on tbl.Fb_Review_Status = fbstatus.Id
where fbstatus.Type=4  and fbstatus.Active = 1 
GROUP BY fbstatus.id,fbstatus.StatusName
        
-- product tran fb report status count        
Insert into @StatusTable (Id,Name,[Count],ActionType) 
select fbstatus.id,'FR-' +fbstatus.StatusName,count(tbl.fbreportId) as [Count],7 as ActionType 
from fb_status fbstatus  
left join   
(select fbreport.Fb_Report_Status,fbreport.id as fbreportId from fb_report_details fbreport 
inner join insp_product_transaction inspprod on inspprod.Fb_Report_Id = fbreport.Id 
where 
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR inspprod.Inspection_Id  IN (SELECT * FROM @BookingIdList))          
and fbreport.Active = 1 and inspprod.Active = 1) as tbl   
on tbl.Fb_Report_Status = fbstatus.Id
where fbstatus.Id in(13,15)   and fbstatus.Active = 1 
GROUP BY fbstatus.id,fbstatus.StatusName

-- container tran fb report status count        
Insert into @StatusTable (Id,Name,[Count],ActionType) 
select fbstatus.id,'FR-' +fbstatus.StatusName,count(tbl.fbreportId) as [Count],7 as ActionType 
from fb_status fbstatus  
left join   
(select fbreport.Fb_Report_Status,fbreport.id as fbreportId from fb_report_details fbreport 
inner join insp_container_transaction inspcont on inspcont.Fb_Report_Id = fbreport.Id 	
where 
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR inspcont.Inspection_Id  IN (SELECT * FROM @BookingIdList))          
and fbreport.Active = 1 and inspcont.Active = 1) as tbl   
on tbl.Fb_Report_Status = fbstatus.Id
where fbstatus.Id in(13,15)   and fbstatus.Active = 1 
GROUP BY fbstatus.id,fbstatus.StatusName


-- report task count   - user role check        
declare @roleId as int = (select userrole.RoleId from it_userrole userrole         
where ((userrole.UserId = @UserId) and           
((EXISTS(SELECT * FROM @RoleIdList WHERE id = 35)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 35)))           
))        
        
-- 35 - report checker        
if(@roleid =35)        
begin        
--report task count        
Insert into @StatusTable (Id,Name,[Count],ActionType)         
select fbreport.Fb_Filling_Status as Id, 'to be review' as name, count(fbreport.Id) as [Count], 8 as ActionType from fb_report_details fbreport        
inner join fb_status fbstatus on fbstatus.Id = fbreport.Fb_Filling_Status        
where fbreport.Fb_Filling_Status = 9 and fbreport.Fb_Review_Status = 10 -- 9 - filling validated, 10 CS not started        
group by fbreport.Fb_Filling_Status , fbstatus.StatusName         
--fbstatus.StatusName as Name        
end        
        
  select Id,Name,sum([Count]) as [count],ActionType from @StatusTable
group by Id,Name ,ActionType
  
END 
GO


/****** Object:  StoredProcedure [dbo].[Usp_CSDashboard_GetNewDetails]    Script Date: 30/06/2021 12:37:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ronika
-- Create date: 15-6-2021
-- Description:	get the new count - customer, supplier, factory, product, po, booking
-- =============================================
CREATE PROCEDURE [dbo].[Usp_CSDashboard_GetNewDetails] (	
	--@BookingIdList IntList NULL READONLY,
	@FromDate DATETIME,
	@ToDate DATETIME,
	@EntityId INT NULL)

AS
BEGIN
			--DECLARE @tblresult TABLE (NewCount int)		

			--customer count
			declare @customerCount as int = (select count(*) from cu_customer cus where 
			cus.CreatedOn BETWEEN  @FromDate and @ToDate)
			
			--supplier count  and -- where 2 - supplier type
			declare @supCount as int = (select count(*) from su_supplier sup where sup.type_id  = 2 and
			sup.CreatedDate BETWEEN @FromDate and @ToDate)

			--factory Count  and where 1 - factory type
			declare @factCount as int = (select count(*) from su_supplier fact where fact.type_id = 1 and 			
			fact.CreatedDate BETWEEN  @FromDate and @ToDate)			
			
			--booking count
			declare @bookingCount as int =  (select count(*) from insp_transaction insp where
			insp.CreatedOn BETWEEN  @FromDate and @ToDate)


			--po count
			declare @poCount as int = (select count(*) from cu_purchaseorder purchaseorder where purchaseorder.CreatedOn
			BETWEEN  @FromDate and @ToDate)


			--product count
			declare @productCount as int =(select count(*) from cu_products product where product.CreatedTime
			BETWEEN  @FromDate and @ToDate)						


			SELECT @customerCount as CustomerCount, @supCount as SupplierCount, 
			@factCount as FactoryCount, @bookingCount as BookingCount,
			 @poCount as POCount,@productCount as ProductCount

END
GO


/****** Object:  StoredProcedure [dbo].[Usp_CSDashboard_GetReports]    Script Date: 30/06/2021 12:37:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================   
-- Author:  Ronika   
-- Create date: 17-6-2021   
-- Description: get the report count with date by booking id list  
-- =============================================    
CREATE PROCEDURE [dbo].[Usp_CSDashboard_GetReports] (     
 @BookingIdList IntList NULL READONLY,    
 @EntityId INT NULL)    
AS    
BEGIN    
select insptran.ServiceDate_To as ServiceToDate, count(fbreport.id) as FbReportCount from insp_product_transaction inspproducttran inner join insp_transaction insptran  
on insptran.id = inspproducttran.Inspection_Id
inner join fb_report_details fbreport on fbreport.id = inspproducttran.Fb_Report_Id
WHERE
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR insptran.id IN (SELECT * FROM @BookingIdList))   and
(fbreport.Active = 1 and inspproducttran.Active = 1)  
GROUP BY insptran.ServiceDate_To order by insptran.ServiceDate_To asc

END    
  
GO


/****** Object:  StoredProcedure [dbo].[Usp_CSDashboard_GetServiceType]    Script Date: 30/06/2021 12:37:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ============================================= 
-- Author:  Ronika 
-- Create date: 15-6-2021 
-- Description: get the service type list by booking id list
-- =============================================  
CREATE PROCEDURE [dbo].[Usp_CSDashboard_GetServiceType] (   
 @BookingIdList IntList NULL READONLY,  
 @EntityId INT NULL)  
  
AS  
BEGIN  
  SELECT Insp_Service_Type.ServiceType_Id AS [ServiceTypeId], Ref_Service_type.Name AS [Name], COUNT(*) AS [Count]

FROM [INSP_TRAN_ServiceType] AS Insp_Service_Type

INNER JOIN [REF_ServiceType] AS Ref_Service_type 

ON Insp_Service_Type.ServiceType_Id = Ref_Service_type.Id

WHERE 

(NOT EXISTS(SELECT 1 FROM @bookingIdList) OR Insp_Service_Type.[Inspection_Id] IN (SELECT * FROM @bookingIdList))

AND (Insp_Service_Type.Active = 1)

GROUP BY Insp_Service_Type.ServiceType_Id, Ref_Service_type.Name
END  


GO


---------#35 CS Dashboard ends----------------

---------#36 REF_KPI_Template starts---------------- 

INSERT INTO REF_KPI_Teamplate(Name,Active,TypeId) VALUES ('Insp Picking Summary',1,1)

---------#36 REF_KPI_Template ends---------------- 

--------#37 User Guide Script starts-----------------

CREATE TABLE [dbo].[UG_UserGuide_Details]
(
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,
	[Name] [nvarchar](50),
	[FileUrl] [nvarchar](500),
	[VideoUrl] [nvarchar](500),
	[TotalPage] [int],
	[Is_Customer] [bit],
	[Is_Supplier] [bit],
	[Is_Factory] [bit],
	[Is_Internal] [bit],
	[EntityId] [int],
	[Active] [bit]
)

CREATE TABLE [dbo].[UG_Role](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[RoleId] [int] NULL,
	[UserGuideId] [int] NULL,
	[Active] BIT NULL
	CONSTRAINT FK_UG_USER_GUIDE FOREIGN KEY(UserGuideId) REFERENCES [dbo].[UG_UserGuide_Details](Id),
	CONSTRAINT FK_UG_ROLE_ID FOREIGN KEY(RoleId) REFERENCES [dbo].[IT_Role](Id)
)


INSERT INTO [dbo].UG_UserGuide_Details(Name,FileUrl,VideoUrl,TotalPage,Is_Customer,Is_Supplier,Is_Factory,Is_Internal,EntityId,Active)
Values ('Booking','\\Documents\\User_Guide\\API_Insp_Booking.pdf','',3,1,1,1,1,1,1)

INSERT INTO [dbo].UG_UserGuide_Details(Name,FileUrl,VideoUrl,TotalPage,Is_Customer,Is_Supplier,Is_Factory,Is_Internal,EntityId,Active)
Values ('Quotation','\\Documents\\User_Guide\\API_Insp_Quotation.pdf','',4,1,0,0,1,1,1)

INSERT INTO [dbo].UG_UserGuide_Details(Name,FileUrl,VideoUrl,TotalPage,Is_Customer,Is_Supplier,Is_Factory,Is_Internal,EntityId,Active)
Values ('Scheduling','\\Documents\\User_Guide\\API_Insp_Scheduling.pdf','',5,0,0,0,1,1,1)

INSERT INTO [dbo].UG_UserGuide_Details(Name,FileUrl,VideoUrl,TotalPage,Is_Customer,Is_Supplier,Is_Factory,Is_Internal,EntityId,Active)
Values ('Report','\\Documents\\User_Guide\\API_Insp_Booking.pdf','',3,1,1,1,1,1,1)

INSERT INTO [dbo].UG_UserGuide_Details(Name,FileUrl,VideoUrl,TotalPage,Is_Customer,Is_Supplier,Is_Factory,Is_Internal,EntityId,Active)
Values ('Invoice','\\Documents\\User_Guide\\API_Insp_Booking.pdf','',3,1,1,1,1,1,1)

INSERT INTO [dbo].UG_UserGuide_Details(Name,FileUrl,VideoUrl,TotalPage,Is_Customer,Is_Supplier,Is_Factory,Is_Internal,EntityId,Active)
Values ('Human Resource','\\Documents\\User_Guide\\API_Insp_Booking.pdf','',3,1,1,1,1,1,1)

INSERT INTO [dbo].UG_UserGuide_Details(Name,FileUrl,VideoUrl,TotalPage,Is_Customer,Is_Supplier,Is_Factory,Is_Internal,EntityId,Active)
Values ('Area Management','\\Documents\\User_Guide\\API_Insp_Booking.pdf','',3,1,1,1,1,1,1)

INSERT INTO [dbo].UG_UserGuide_Details(Name,FileUrl,VideoUrl,TotalPage,Is_Customer,Is_Supplier,Is_Factory,Is_Internal,EntityId,Active)
Values ('Expense Claim','\\Documents\\User_Guide\\API_Insp_Booking.pdf','',3,1,1,1,1,1,1)

INSERT INTO [dbo].UG_UserGuide_Details(Name,FileUrl,VideoUrl,TotalPage,Is_Customer,Is_Supplier,Is_Factory,Is_Internal,EntityId,Active)
Values ('Product Management','\\Documents\\User_Guide\\API_Insp_Booking.pdf','',3,1,1,1,1,1,1)

INSERT INTO [dbo].UG_UserGuide_Details(Name,FileUrl,VideoUrl,TotalPage,Is_Customer,Is_Supplier,Is_Factory,Is_Internal,EntityId,Active)
Values ('Lab Details','\\Documents\\User_Guide\\API_Insp_Booking.pdf','',3,1,1,1,1,1,1)

INSERT INTO [dbo].[UG_Role] (RoleId,UserGuideId,Active) Values
(23,1,1)

INSERT INTO [dbo].[UG_Role] (RoleId,UserGuideId,Active) Values
(24,1,1)

INSERT INTO [dbo].[UG_Role] (RoleId,UserGuideId,Active) Values
(25,1,1)

INSERT INTO [dbo].[UG_Role] (RoleId,UserGuideId,Active) Values
(26,2,1)

INSERT INTO [dbo].[UG_Role] (RoleId,UserGuideId,Active) Values
(20,3,1)

INSERT INTO [dbo].[UG_Role] (RoleId,UserGuideId,Active) Values
(21,3,1)

INSERT INTO [dbo].[UG_Role] (RoleId,UserGuideId,Active) Values
(22,3,1)

INSERT INTO [dbo].[UG_Role] (RoleId,UserGuideId,Active) Values
(27,3,1)

INSERT INTO [dbo].[UG_Role] (RoleId,UserGuideId,Active) Values
(23,4,1)

INSERT INTO [dbo].[UG_Role] (RoleId,UserGuideId,Active) Values
(23,5,1)

-----#37 User Guide Script ends-----------------

-----#38 Report details Inspection_Id added start---------------

	  ALTER  TABLE [FB_Report_Details] ADD [Inspection_Id] INT NULL
	  ALTER TABLE [FB_Report_Details] ADD CONSTRAINT FB_Report_Details_Inspection_Id FOREIGN KEY ([Inspection_Id]) REFERENCES [dbo].[INSP_Transaction](Id)

	  UPDATE reportData set reportData.Inspection_Id=inspprod.Inspection_Id 	   
	  FROM
	  FB_Report_Details reportData
	  INNER JOIN
	  INSP_Product_Transaction inspprod 
	  on reportData.Id=inspprod.Fb_Report_Id
	  ---------------------------------------------------------

	  UPDATE reportData set reportData.Inspection_Id=container.Inspection_Id 	   
	  FROM
	  FB_Report_Details reportData
	  INNER JOIN 
	  INSP_Container_Transaction container 
	  on reportData.Id=container.Fb_Report_Id

-----#38 Report details Inspection_Id added end-----------------

------------------#39 Finance Dashboard Chageback sp  starts------------------------
/****** Object:  StoredProcedure [dbo].[Usp_FinanceKPI_GetChargeBack]    Script Date: 7/2/2021 7:10:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Binny
-- Create date: 17-6-2021
-- Description:	get the total travel expense claim and total travel invoice amount
-- =============================================
ALTER PROCEDURE [dbo].[Usp_FinanceKPI_GetChargeBack]

	@BookingIdList IntList NULL READONLY,
	@EntityId INT NULL
AS
BEGIN
	DECLARE @ExpenseClaimAmount float = 0

	SELECT CAST(Row_number()
         OVER(
           ORDER BY (SELECT NULL)) AS INT) AS Id,
	SUM(expDetails.Ammount_HK) AS [Fee], expDetails.CurrencyId AS [CurrencyId] FROM EC_ExpenseClaimsInspection expInsp
	INNER JOIN EC_ExpensesClaimDetais expDetails ON expDetails.Id = expInsp.ExpenseClaimDetailId
	INNER JOIN EC_ExpensesTypes expType ON expType.Id = expDetails.ExpenseTypeId
	INNER JOIN EC_ExpencesClaims claims ON claims.Id = expDetails.ExpenseId
	WHERE expType.IsTravel = 1 AND claims.active = 1 AND claims.StatusId = 4
	AND  expInsp.BookingId IN (SELECT Id FROM @BookingIdList)
	GROUP BY expDetails.CurrencyId

	--SELECT COALESCE(@ExpenseClaimAmount, 0)

	SELECT insp.id AS BookingId, InvoiceCurrency AS [InvoiceCurrencyId], TotalInvoiceFees AS [TotalInvoiceFee], TravelTotalFees , 
	TotalExtraFee AS [TotalExtraFee], exf.CurrencyId AS [ExtraFeeCurrencyId], invoice.TravelIncluded AS [PriceCardTravelIncluded]
	FROM INSP_TRANSACTION insp
	LEFT JOIN (select ruleId, InvoiceCurrency, TotalInvoiceFees, TravelTotalFees, InspectionId, TravelIncluded from INV_AUT_TRAN_Details inv 
	INNER JOIN CU_PR_Details pc ON pc.Id = inv.RuleId) invoice on invoice.InspectionId = insp.Id
	LEFT JOIN INV_EXF_Transaction exf on exf.InspectionId = insp.Id

	WHERE insp.Id IN (SELECT Id FROM @BookingIdList)
	-- AND pc.TravelIncluded = 0
END
GO
------------------#39 Finance Dashboard Chageback sp  ends------------------------

-----#40 Leave Module Script starts-----------------

ALTER TABLE HR_Leave ADD ApprovedOn DATETIME

ALTER TABLE HR_Leave ADD CancelledOn DATETIME

ALTER TABLE HR_Leave ADD RejectedOn DATETIME

-----#40 Leave Module Script ends-----------------

-----#41 CS Dashboard SP starts-----------------

-- =============================================             
-- Author:  Ronika             
-- Create date: 18-6-2021             
-- Description: get the booking, quotation, allocation, report status list by booking id list            
-- =============================================              
alter PROCEDURE [dbo].[Usp_CSDashboard_GetModuleStatus] (               
 @BookingIdList IntList NULL READONLY,              
 @RoleIdList IntList NULL READONLY,              
 @UserId int null,            
 @EntityId INT NULL)              
AS              
          
BEGIN            
          
Declare @StatusTable TABLE(              
Id int,            
Name nvarchar(100),          
[Count] int,          
ActionType int          
)              
          
--inpection status             
Insert into @StatusTable (Id,Name,[Count],ActionType)           
select inspstatus.Id as Id , inspstatus.Status as Name , count(insptran.id) as Count, 1 as ActionType from            
insp_status inspstatus left join insp_transaction insptran              
on insptran.Status_Id = inspstatus.Id        
and (NOT EXISTS(SELECT 1 FROM @BookingIdList) OR insptran.id IN (SELECT * FROM @BookingIdList))            
where inspstatus.Active = 1 and  inspstatus.id not in (9)  --9 - allocated status      
GROUP BY inspstatus.Id, inspstatus.Status          
              
-- inspection status task count            
Insert into @StatusTable (Id,Name,[Count],ActionType)           
select midtask.tasktypeid as Id, '' as Name, count(midtask.tasktypeid) as Count, 2 as ActionType from            
it_userrole userrole left join mid_task midtask  on userrole.UserId = midtask.UserId            
where (            
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR midtask.LinkId IN (SELECT * FROM @BookingIdList)) and            
(userrole.UserId = @UserId and midtask.isdone = 0)             
 --and  (userrole.RoleId in(24) and midtask.TaskTypeId in(6))            
 and             
 (            
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 25)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 25))  and midtask.TaskTypeId in(5,8))            
 or            
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 24)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 24))  and midtask.TaskTypeId in(6))            
 ))            
 --25 - Inspection Verified, 24 - Inspection Confirmed, 5 -Verify Inspection   , 6-Confirm Inspection   , 8 -SplitInspectionBooking.            
group by midtask.tasktypeid             
            
--add quotation pending status        
        
--booking --8- verified, 2 - confirmed        
--quotation required - 1 - customer check point        
        
Insert into @StatusTable (Id,Name,[Count],ActionType)           
 select 0 as Id , 'Pending' as Name , count(insptran.id) as [Count], 3 as ActionType from insp_transaction insptran         
inner join CU_CheckPoints cucheckpoint on cucheckpoint.CustomerId = insptran.Customer_Id        
where insptran.Status_Id in (8,2) and cucheckpoint.CheckpointTypeId = 1         
        
        
-- quotation status count        
Insert into @StatusTable (Id,Name,[Count],ActionType)           
select qustatus.Id as Id , qustatus.Label as Name , count(ququotation.id) as [Count], 3 as ActionType from            
qu_status qustatus left join qu_quotation ququotation           
on ququotation.IdStatus = qustatus.Id            
left join qu_quotation_insp quinsp on quinsp.IdQuotation = ququotation.Id        
and (NOT EXISTS(SELECT 1 FROM @BookingIdList) OR quinsp.IdQuotation IN (SELECT * FROM @BookingIdList))            
where qustatus.Active = 1        
GROUP BY qustatus.Id , qustatus.Label          
              
-- quotation status task count           
Insert into @StatusTable (Id,Name,[Count],ActionType)           
select midtask.tasktypeid as Id, '' as Name, count(midtask.tasktypeid) as [Count], 4 as ActionType from            
it_userrole userrole left join mid_task midtask  on userrole.UserId = midtask.UserId            
where (            
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR midtask.LinkId IN (SELECT * FROM @BookingIdList)) and            
(userrole.UserId = @UserId and midtask.isdone = 0)             
 and         
 (            
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 20)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 20))  and midtask.TaskTypeId in(10,14))            
 or            
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 21)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 21))  and midtask.TaskTypeId in(7))        
 or        
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 22)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 22))  and midtask.TaskTypeId in(12,13))        
 or        
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 27)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 27))  and midtask.TaskTypeId in(11))        
 ))            
group by midtask.tasktypeid                 
-- role - 20-Quotation Request, 21 Quotation Manager, 22 Quotation Confirmation,27 Quotation Send        
--task 14 Quotation Pending,10 Quotation Modify,7 quotation approve, 12 Quotation Customer Confirmed,13 Quotation Customer Reject,11 Quotation Sent        
        
--inpection allocation status           
Insert into @StatusTable (Id,Name,[Count],ActionType)           
select insptran.Status_Id as Id , inspstatus.Status as Name , count(insptran.id) as [Count], 5 as ActionType from            
insp_status inspstatus left join insp_transaction insptran              
on insptran.Status_Id = inspstatus.Id            
and (NOT EXISTS(SELECT 1 FROM @BookingIdList) OR insptran.id IN (SELECT * FROM @BookingIdList))            
where inspstatus.Active = 1 and  inspstatus.id in (2,9)  --9 - allocated status , 2 - confirmed          
GROUP BY insptran.Status_Id, inspstatus.Status               
            
-- inspection allocation status task count           
Insert into @StatusTable (Id,Name,[Count],ActionType)           
select midtask.tasktypeid as Id, '' as Name, count(midtask.tasktypeid) as [Count], 6 as ActionType from            
it_userrole userrole left join mid_task midtask  on userrole.UserId = midtask.UserId            
where (            
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR midtask.LinkId IN (SELECT * FROM @BookingIdList)) and            
(userrole.UserId = @UserId and midtask.isdone = 0)             
 and             
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 26)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 26))  and midtask.TaskTypeId in(9))              
 )            
 --26 Inspection Schedule - role --9 Inspection Schedule - task type          
group by midtask.tasktypeid             
              
  --product tran fb report filling status count          
Insert into @StatusTable (Id,Name,[Count],ActionType)    
select fbstatus.id,'QC-' +fbstatus.StatusName,count(tbl.fbreportId) as [Count],7 as ActionType from fb_status fbstatus    
left join     
(select fbreport.Fb_Filling_Status,fbreport.id as fbreportId from fb_report_details fbreport   
inner join insp_product_transaction inspprod on inspprod.Fb_Report_Id = fbreport.Id   
where   
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR inspprod.Inspection_Id  IN (SELECT * FROM @BookingIdList))            
and fbreport.Active = 1 and inspprod.Active = 1) as tbl     
on tbl.Fb_Filling_Status = fbstatus.Id  
where fbstatus.Type=3  and fbstatus.Active = 1   
GROUP BY fbstatus.id,fbstatus.StatusName  
  
--container tran fb report filling status count          
  Insert into @StatusTable (Id,Name,[Count],ActionType)     
  select fbstatus.id,'QC-' +fbstatus.StatusName,count(tbl.fbreportId) as cnt,7 as ActionType from fb_status fbstatus   left join    (select fbreport.Fb_Filling_Status,fbreport.id as fbreportId from fb_report_details fbreport  inner join 
  insp_container_transaction inspcont on inspcont.Fb_Report_Id = fbreport.Id  where inspcont.Inspection_Id in (4,5) and fbreport.Active = 1 and inspcont.Active = 1  )as tbl    on tbl.Fb_Filling_Status = fbstatus.Id where fbstatus.Type=3  and fbstatus.Active = 1  GROUP BY 
  fbstatus.id,fbstatus.StatusName  
  
  
--product tran fb report reivew status count          
Insert into @StatusTable (Id,Name,[Count],ActionType)     
select fbstatus.id,'CS-' +fbstatus.StatusName,count(tbl.fbreportId) as [Count],7 as ActionType   
from fb_status fbstatus    
left join     
(select fbreport.Fb_Review_Status,fbreport.id as fbreportId from fb_report_details fbreport   
inner join insp_product_transaction inspprod on inspprod.Fb_Report_Id = fbreport.Id   
where   
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR inspprod.Inspection_Id  IN (SELECT * FROM @BookingIdList))            
and fbreport.Active = 1 and inspprod.Active = 1) as tbl     
on tbl.Fb_Review_Status = fbstatus.Id  
where fbstatus.Type=4  and fbstatus.Active = 1   
GROUP BY fbstatus.id,fbstatus.StatusName  
  
--container tran fb report reivew status count          
Insert into @StatusTable (Id,Name,[Count],ActionType)     
select fbstatus.id,'CS-' +fbstatus.StatusName,count(tbl.fbreportId) as [Count],7 as ActionType   
from fb_status fbstatus    
left join     
(select fbreport.Fb_Review_Status,fbreport.id as fbreportId from fb_report_details fbreport   
inner join insp_container_transaction inspcont on inspcont.Fb_Report_Id = fbreport.Id   
where   
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR inspcont.Inspection_Id  IN (SELECT * FROM @BookingIdList))            
and fbreport.Active = 1 and inspcont.Active = 1) as tbl     
on tbl.Fb_Review_Status = fbstatus.Id  
where fbstatus.Type=4  and fbstatus.Active = 1   
GROUP BY fbstatus.id,fbstatus.StatusName  
          
-- product tran fb report status count          
Insert into @StatusTable (Id,Name,[Count],ActionType)   
select fbstatus.id,'FR-' +fbstatus.StatusName,count(tbl.fbreportId) as [Count],7 as ActionType   
from fb_status fbstatus    
left join     
(select fbreport.Fb_Report_Status,fbreport.id as fbreportId from fb_report_details fbreport   
inner join insp_product_transaction inspprod on inspprod.Fb_Report_Id = fbreport.Id   
where   
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR inspprod.Inspection_Id  IN (SELECT * FROM @BookingIdList))            
and fbreport.Active = 1 and inspprod.Active = 1) as tbl     
on tbl.Fb_Report_Status = fbstatus.Id  
where fbstatus.Id in(13,15)   and fbstatus.Active = 1   
GROUP BY fbstatus.id,fbstatus.StatusName  
  
-- container tran fb report status count          
Insert into @StatusTable (Id,Name,[Count],ActionType)   
select fbstatus.id,'FR-' +fbstatus.StatusName,count(tbl.fbreportId) as [Count],7 as ActionType   
from fb_status fbstatus    
left join     
(select fbreport.Fb_Report_Status,fbreport.id as fbreportId from fb_report_details fbreport   
inner join insp_container_transaction inspcont on inspcont.Fb_Report_Id = fbreport.Id    
where   
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR inspcont.Inspection_Id  IN (SELECT * FROM @BookingIdList))            
and fbreport.Active = 1 and inspcont.Active = 1) as tbl     
on tbl.Fb_Report_Status = fbstatus.Id  
where fbstatus.Id in(13,15)   and fbstatus.Active = 1   
GROUP BY fbstatus.id,fbstatus.StatusName  
  
  
-- report task count   - user role check          
declare @roleId as int = (select userrole.RoleId from it_userrole userrole           
where ((userrole.UserId = @UserId) and             
((EXISTS(SELECT * FROM @RoleIdList WHERE id = 35)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 35)))             
))          
          
-- 35 - report checker          
if(@roleid =35)          
begin          
--report task count          
Insert into @StatusTable (Id,Name,[Count],ActionType)           
select fbreport.Fb_Review_Status as Id, 'to be review' as name, count(fbreport.Id) as [Count], 8 as ActionType from fb_report_details fbreport          
inner join fb_status fbstatus on fbstatus.Id = fbreport.Fb_Filling_Status          
where fbreport.Fb_Filling_Status = 9 and fbreport.Fb_Review_Status = 10 -- 9 - filling validated, 10 CS not started          
group by fbreport.Fb_Review_Status , fbstatus.StatusName           
--fbstatus.StatusName as Name          
end          
          
  select Id,Name,sum([Count]) as [count],ActionType from @StatusTable  
group by Id,Name ,ActionType  
    
END   


-- =============================================   
-- Author:  Ronika   
-- Create date: 16-6-2021   
-- Description: get the office list with manday count by booking id list  
-- =============================================    
alter PROCEDURE [dbo].[Usp_CSDashboard_GetMandayByOffice] (     
 @BookingIdList IntList NULL READONLY,    
 @EntityId INT NULL)    
AS    
BEGIN    
--, quinsp.Idbooking  
--, quinsp.idbooking,quinsp.NoOfManday  
--select refloc.id,refloc.location_name, sum(quinsp.NoOfManday) as mandaycount from qu_quotation_insp quinsp inner join insp_transaction insptran  on insptran.id = quinsp.idbooking  
--inner join ref_location refloc on insptran.office_id = refloc.id  
--WHERE (refloc.Active = 1)  
--GROUP BY refloc.id, refloc.Location_Name  
select refloc.id,refloc.location_name as Name, sum(quinsp.NoOfManday) as [Count]   
from qu_quotation_insp quinsp inner join insp_transaction insptran  on insptran.id = quinsp.idbooking  
inner join ref_location refloc on insptran.office_id = refloc.id  
WHERE   
(NOT EXISTS(SELECT 1 FROM @bookingIdList) OR insptran.id IN (SELECT * FROM @bookingIdList))  
AND (refloc.Active = 1)  
GROUP BY refloc.id, refloc.Location_Name  
order by [Count] desc
END    
  

  -- =============================================   
-- Author:  Ronika   
-- Create date: 15-6-2021   
-- Description: get the service type list by booking id list  
-- =============================================    
alter PROCEDURE [dbo].[Usp_CSDashboard_GetServiceType] (     
 @BookingIdList IntList NULL READONLY,    
 @EntityId INT NULL)    
    
AS    
BEGIN    
  SELECT Insp_Service_Type.ServiceType_Id AS [ServiceTypeId], Ref_Service_type.Name AS [Name], COUNT(*) AS [Count]  
  
FROM [INSP_TRAN_ServiceType] AS Insp_Service_Type  
  
INNER JOIN [REF_ServiceType] AS Ref_Service_type   
  
ON Insp_Service_Type.ServiceType_Id = Ref_Service_type.Id  
  
WHERE   
  
(NOT EXISTS(SELECT 1 FROM @bookingIdList) OR Insp_Service_Type.[Inspection_Id] IN (SELECT * FROM @bookingIdList))  
  
AND (Insp_Service_Type.Active = 1)  
  
GROUP BY Insp_Service_Type.ServiceType_Id, Ref_Service_type.Name 
order by [Count] desc
END    
  
  

-----#41 CS Dashboard SP ends-----------------





-----#42 CS Dashboard  Usp_CSDashboard_GetModuleStatus SP starts-----------------


-- =============================================               
-- Author:  Ronika               
-- Create date: 18-6-2021               
-- Description: get the booking, quotation, allocation, report status list by booking id list              
-- =============================================                
CREATE PROCEDURE [dbo].[Usp_CSDashboard_GetModuleStatus] (                 
 @BookingIdList IntList NULL READONLY,                
 @RoleIdList IntList NULL READONLY,                
 @UserId int null,              
 @EntityId INT NULL)                
AS                
         
BEGIN              
            
Declare @StatusTable TABLE(                
Id int,              
Name nvarchar(100),            
[Count] int,            
ActionType int            
)                
            
--declare @QuotationIdList IntList NULL;  
  
--inpection status               
Insert into @StatusTable (Id,Name,[Count],ActionType)             
select inspstatus.Id as Id , inspstatus.Status as Name , count(insptran.id) as Count, 1 as ActionType from              
insp_status inspstatus left join insp_transaction insptran                
on insptran.Status_Id = inspstatus.Id          
and (NOT EXISTS(SELECT 1 FROM @BookingIdList) OR insptran.id IN (SELECT * FROM @BookingIdList))              
where inspstatus.Active = 1 and  inspstatus.id not in (9)  --9 - allocated status        
GROUP BY inspstatus.Id, inspstatus.Status            
                
-- inspection status task count              
Insert into @StatusTable (Id,Name,[Count],ActionType)             
select midtask.tasktypeid as Id, '' as Name, count(midtask.tasktypeid) as Count, 2 as ActionType from              
it_userrole userrole left join mid_task midtask  on userrole.UserId = midtask.UserId              
where (              
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR midtask.LinkId IN (SELECT * FROM @BookingIdList)) and              
(userrole.UserId = @UserId and midtask.isdone = 0)               
 --and  (userrole.RoleId in(24) and midtask.TaskTypeId in(6))              
 and               
 (              
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 25)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 25))  and midtask.TaskTypeId in(5,8))              
 or              
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 24)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 24))  and midtask.TaskTypeId in(6))              
 ))              
 --25 - Inspection Verified, 24 - Inspection Confirmed, 5 -Verify Inspection   , 6-Confirm Inspection   , 8 -SplitInspectionBooking.              
group by midtask.tasktypeid               
              
--add quotation pending status          
          
--booking --8- verified, 2 - confirmed          
--quotation required - 1 - customer check point          
          
Insert into @StatusTable (Id,Name,[Count],ActionType)             
 select 0 as Id , 'Pending' as Name , count(insptran.id) as [Count], 3 as ActionType from insp_transaction insptran           
inner join CU_CheckPoints cucheckpoint on cucheckpoint.CustomerId = insptran.Customer_Id          
where insptran.Status_Id in (8,2) and cucheckpoint.CheckpointTypeId = 1       
and (NOT EXISTS(SELECT 1 FROM @BookingIdList) OR insptran.Id IN (SELECT * FROM @BookingIdList))              
          
          
-- quotation status count          
Insert into @StatusTable (Id,Name,[Count],ActionType)             
select qustatus.Id as Id , qustatus.Label as Name , count(ququotation.id) as [Count], 3 as ActionType from              
qu_status qustatus left join qu_quotation ququotation             
on ququotation.IdStatus = qustatus.Id              
left join qu_quotation_insp quinsp on quinsp.IdQuotation = ququotation.Id          
and (NOT EXISTS(SELECT 1 FROM @BookingIdList) OR quinsp.IdQuotation IN (SELECT * FROM @BookingIdList))              
where qustatus.Active = 1          
GROUP BY qustatus.Id , qustatus.Label            
                
  
  
-- quotation status task count             
Insert into @StatusTable (Id,Name,[Count],ActionType)             
select midtask.tasktypeid as Id, '' as Name, count(midtask.tasktypeid) as [Count], 4 as ActionType from              
it_userrole userrole left join mid_task midtask  on userrole.UserId = midtask.UserId              
where (              
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR midtask.LinkId IN (SELECT * FROM @BookingIdList)) and              
(userrole.UserId = @UserId and midtask.isdone = 0)               
 and           
 (              
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 20)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 20))  and midtask.TaskTypeId in(10,14))              
 or              
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 21)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 21))  and midtask.TaskTypeId in(7))          
 or          
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 22)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 22))  and midtask.TaskTypeId in(12,13))          
 or          
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 27)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 27))  and midtask.TaskTypeId in(11))          
 ))              
group by midtask.tasktypeid                   
-- role - 20-Quotation Request, 21 Quotation Manager, 22 Quotation Confirmation,27 Quotation Send          
--task 14 Quotation Pending,10 Quotation Modify,7 quotation approve, 12 Quotation Customer Confirmed,13 Quotation Customer Reject,11 Quotation Sent          
          
--inpection allocation status             
Insert into @StatusTable (Id,Name,[Count],ActionType)             
select insptran.Status_Id as Id , inspstatus.Status as Name , count(insptran.id) as [Count], 5 as ActionType from              
insp_status inspstatus left join insp_transaction insptran                
on insptran.Status_Id = inspstatus.Id              
and (NOT EXISTS(SELECT 1 FROM @BookingIdList) OR insptran.id IN (SELECT * FROM @BookingIdList))              
where inspstatus.Active = 1 and  inspstatus.id in (2,9)  --9 - allocated status , 2 - confirmed            
GROUP BY insptran.Status_Id, inspstatus.Status                 
              
-- inspection allocation status task count             
Insert into @StatusTable (Id,Name,[Count],ActionType)             
select midtask.tasktypeid as Id, '' as Name, count(midtask.tasktypeid) as [Count], 6 as ActionType from              
it_userrole userrole left join mid_task midtask  on userrole.UserId = midtask.UserId              
where (              
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR midtask.LinkId IN (SELECT * FROM @BookingIdList)) and              
(userrole.UserId = @UserId and midtask.isdone = 0)               
 and               
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 26)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 26))  and midtask.TaskTypeId in(9))                
 )              
 --26 Inspection Schedule - role --9 Inspection Schedule - task type            
group by midtask.tasktypeid               
                
  --product tran fb report filling status count            
Insert into @StatusTable (Id,Name,[Count],ActionType)      
select fbstatus.id,'QC-' +fbstatus.StatusName,count(tbl.fbreportId) as [Count],7 as ActionType from fb_status fbstatus      
left join       
(select fbreport.Fb_Filling_Status,fbreport.id as fbreportId from fb_report_details fbreport     
inner join insp_product_transaction inspprod on inspprod.Fb_Report_Id = fbreport.Id     
where     
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR inspprod.Inspection_Id  IN (SELECT * FROM @BookingIdList))              
and fbreport.Active = 1 and inspprod.Active = 1) as tbl       
on tbl.Fb_Filling_Status = fbstatus.Id    
where fbstatus.Type=3  and fbstatus.Active = 1     
GROUP BY fbstatus.id,fbstatus.StatusName    
    
--container tran fb report filling status count            
  Insert into @StatusTable (Id,Name,[Count],ActionType)       
  select fbstatus.id,'QC-' +fbstatus.StatusName,count(tbl.fbreportId) as cnt,7 as ActionType from fb_status fbstatus   left join    (select fbreport.Fb_Filling_Status,fbreport.id as fbreportId from fb_report_details fbreport  inner join   
  insp_container_transaction inspcont on inspcont.Fb_Report_Id = fbreport.Id  where inspcont.Inspection_Id in (4,5) and fbreport.Active = 1 and inspcont.Active = 1  )as tbl    on tbl.Fb_Filling_Status = fbstatus.Id where fbstatus.Type=3  and fbstatus.Active = 1  GROUP BY   
  fbstatus.id,fbstatus.StatusName    
    
    
--product tran fb report reivew status count            
Insert into @StatusTable (Id,Name,[Count],ActionType)       
select fbstatus.id,'CS-' +fbstatus.StatusName,count(tbl.fbreportId) as [Count],7 as ActionType     
from fb_status fbstatus      
left join       
(select fbreport.Fb_Review_Status,fbreport.id as fbreportId from fb_report_details fbreport     
inner join insp_product_transaction inspprod on inspprod.Fb_Report_Id = fbreport.Id     
where     
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR inspprod.Inspection_Id  IN (SELECT * FROM @BookingIdList))              
and fbreport.Active = 1 and inspprod.Active = 1) as tbl       
on tbl.Fb_Review_Status = fbstatus.Id    
where fbstatus.Type=4  and fbstatus.Active = 1     
GROUP BY fbstatus.id,fbstatus.StatusName    
    
--container tran fb report reivew status count            
Insert into @StatusTable (Id,Name,[Count],ActionType)       
select fbstatus.id,'CS-' +fbstatus.StatusName,count(tbl.fbreportId) as [Count],7 as ActionType     
from fb_status fbstatus      
left join       
(select fbreport.Fb_Review_Status,fbreport.id as fbreportId from fb_report_details fbreport     
inner join insp_container_transaction inspcont on inspcont.Fb_Report_Id = fbreport.Id     
where     
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR inspcont.Inspection_Id  IN (SELECT * FROM @BookingIdList))              
and fbreport.Active = 1 and inspcont.Active = 1) as tbl       
on tbl.Fb_Review_Status = fbstatus.Id    
where fbstatus.Type=4  and fbstatus.Active = 1     
GROUP BY fbstatus.id,fbstatus.StatusName    
            
-- product tran fb report status count            
Insert into @StatusTable (Id,Name,[Count],ActionType)     
select fbstatus.id,'FR-' +fbstatus.StatusName,count(tbl.fbreportId) as [Count],7 as ActionType     
from fb_status fbstatus      
left join       
(select fbreport.Fb_Report_Status,fbreport.id as fbreportId from fb_report_details fbreport     
inner join insp_product_transaction inspprod on inspprod.Fb_Report_Id = fbreport.Id     
where     
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR inspprod.Inspection_Id  IN (SELECT * FROM @BookingIdList))              
and fbreport.Active = 1 and inspprod.Active = 1) as tbl       
on tbl.Fb_Report_Status = fbstatus.Id    
where fbstatus.Id in(13,15)   and fbstatus.Active = 1     
GROUP BY fbstatus.id,fbstatus.StatusName    
    
-- container tran fb report status count            
Insert into @StatusTable (Id,Name,[Count],ActionType)     
select fbstatus.id,'FR-' +fbstatus.StatusName,count(tbl.fbreportId) as [Count],7 as ActionType     
from fb_status fbstatus      
left join       
(select fbreport.Fb_Report_Status,fbreport.id as fbreportId from fb_report_details fbreport     
inner join insp_container_transaction inspcont on inspcont.Fb_Report_Id = fbreport.Id      
where     
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR inspcont.Inspection_Id  IN (SELECT * FROM @BookingIdList))              
and fbreport.Active = 1 and inspcont.Active = 1) as tbl       
on tbl.Fb_Report_Status = fbstatus.Id    
where fbstatus.Id in(13,15)   and fbstatus.Active = 1     
GROUP BY fbstatus.id,fbstatus.StatusName    
    
-- report task count   - user role check            
declare @roleId as int = (select userrole.RoleId from it_userrole userrole             
where ((userrole.UserId = @UserId) and               
((EXISTS(SELECT * FROM @RoleIdList WHERE id = 35)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 35)))               
))            
            
-- 35 - report checker            
if(@roleid =35)            
begin            
--report task count            
Insert into @StatusTable (Id,Name,[Count],ActionType)             
select fbreport.Fb_Review_Status as Id, 'to be review' as name, count(fbreport.Id) as [Count], 8 as ActionType  
from fb_report_details fbreport            
inner join fb_status fbstatus on fbstatus.Id = fbreport.Fb_Filling_Status    
inner join insp_container_transaction inspcont on inspcont.Fb_Report_Id = fbreport.Id      
where fbreport.Fb_Filling_Status = 9 and fbreport.Fb_Review_Status = 10 -- 9 - filling validated, 10 CS not started      
and (NOT EXISTS(SELECT 1 FROM @BookingIdList) OR inspcont.Inspection_Id  IN (SELECT * FROM @BookingIdList))   
and fbreport.Active = 1 and inspcont.Active = 1 and fbstatus.Active = 1    
group by fbreport.Fb_Review_Status , fbstatus.StatusName    
  
  
Insert into @StatusTable (Id,Name,[Count],ActionType)             
select fbreport.Fb_Review_Status as Id, 'to be review' as name, count(fbreport.Id) as [Count], 8 as ActionType  
from fb_report_details fbreport            
inner join fb_status fbstatus on fbstatus.Id = fbreport.Fb_Filling_Status    
inner join insp_product_transaction inspprod on inspprod.Fb_Report_Id = fbreport.Id     
where fbreport.Fb_Filling_Status = 9 and fbreport.Fb_Review_Status = 10 -- 9 - filling validated, 10 CS not started      
and (NOT EXISTS(SELECT 1 FROM @BookingIdList) OR inspprod.Inspection_Id  IN (SELECT * FROM @BookingIdList))   
and fbreport.Active = 1 and inspprod.Active = 1 and fbstatus.Active = 1    
group by fbreport.Fb_Review_Status , fbstatus.StatusName    
end            
            
  select Id,Name,sum([Count]) as [count],ActionType from @StatusTable    
group by Id,Name ,ActionType    
      
END 
-----#42 CS Dashboard  Usp_CSDashboard_GetModuleStatus SP ends-----------------

-- # 43 Report send enhancement start ---

INSERT INTO [dbo].[REF_DateFormat] (DateFormat) Values ('dd_MMM_yyyy')
INSERT INTO [ES_SU_PreDefined_Fields](field_name,field_alias_Name,Active) values('SupplierCode','Supplier Code',1)

-- # 43 Report send enhancement end ---

------- #44 Upload Audit Report to Cloud starts------------

DELETE FROM AUD_TRAN_File_Attachment

DROP TABLE AUD_TRAN_File_Attachment

CREATE TABLE [dbo].[AUD_TRAN_File_Attachment]
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Audit_Id] INT NOT NULL,
	[FileName] [nvarchar](500) NOT NULL,
	[UserId] [int] NOT NULL,
	[UploadDate] [datetime] NOT NULL,
	[Active] BIT NOT NULL, 
    [FileUrl] NVARCHAR(MAX) NULL, 
    [UniqueId] NVARCHAR(1000) NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
    FOREIGN KEY(Audit_Id) REFERENCES [AUD_Transaction](Id),
	FOREIGN KEY ([UserId]) REFERENCES [dbo].[IT_UserMaster](Id),	
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)	
)

CREATE TABLE [dbo].[AUD_TRAN_Report]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [UniqueId] NVARCHAR(1000) NULL, 
    [AuditId] INT NOT NULL, 
    [FileName] NVARCHAR(500) NULL, 
    [FileUrl] NVARCHAR(MAX) NULL, 
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
    [Active] BIT NOT NULL,
	FOREIGN KEY ([AuditId]) REFERENCES [dbo].[AUD_Transaction](Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)


CREATE TABLE [dbo].[EC_ReceiptFileAttachment]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [ExpenseId] INT NOT NULL, 
	[UniqueId] [nvarchar](1000) NULL,
    [FileName] NVARCHAR(500) NULL, 
    [FileUrl] NVARCHAR(MAX) NULL, 
    [Createdby] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
    [Active] BIT NOT NULL,
	FOREIGN KEY ([ExpenseId]) REFERENCES [dbo].EC_ExpensesClaimDetais (Id),
	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)

ALTER TABLE [REF_Budget_Forecast] ALTER COLUMN CountryId INT NULL
------- #44 Upload Audit Report to Cloud ends------------

--#45 Style creation start here

ALTER TABLE cu_products ADD IsMS_Chart BIT
ALTER TABLE cu_products ADD IsStyle BIT

CREATE TABLE CU_Product_FileType
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(200) NOT NULL, 
	[Sort] INT NULL,
	[Active] BIT
)

INSERT INTO CU_Product_FileType (NAME,Sort,Active) VALUES('MS chart',1,1)


CREATE TABLE CU_Product_MSChart
(
  [Id] INT NOT NULL PRIMARY KEY IDENTITY, 

  [Product_Id] int NOT NULL, 
 
  [Product_File_Id] int NULL,
 
  [Code] nvarchar(1000) NULL,
 
  [Description] nvarchar(2000) NULL,
 
  [MPCode] nvarchar(500) NULL,
 
  [Required] float NULL,
 
  [Tolerance1_Up] float NULL,
 
  [Tolerance1_Down] float NULL,
 
  [Tolerance2_Up] float NULL,
 
  [Tolerance2_Down] float NULL,
 
  [Sort] INT NULL,
 
  [CreatedOn] DATETIME NULL,
 
  [CreatedBy] INT NULL,
 
  [UpdatedBy] INT NULL,
 
  [UpdatedOn] DATETIME NULL,

  CONSTRAINT FK_CU_Product_MSChart_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	

  CONSTRAINT FK_CU_Product_MSChart_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	

  CONSTRAINT FK_CU_Product_MSChart_Product_Id FOREIGN KEY ([Product_Id]) REFERENCES [dbo].[CU_Products](Id),

  CONSTRAINT FK_CU_Product_MSChart_Product_File_Id FOREIGN KEY ([Product_File_Id]) REFERENCES [dbo].[CU_Product_File_Attachment](Id)

)

ALTER TABLE CU_Product_File_Attachment ADD FileType_Id int null 
ALTER TABLE CU_Product_File_Attachment ADD CONSTRAINT FK_CU_Product_File_Attachment_FileType_Id FOREIGN KEY ([FileType_Id]) REFERENCES [dbo].[CU_Product_FileType](Id)

--#45 Style creation end here
------- #44 Upload Audit Report to Cloud ends------------


-------------------------------45 Remove API Service Starts----------------------------------

ALTER TABLE REF_Product_Category_API_Services DROP CONSTRAINT FK__REF_Product_Category_Service

ALTER TABLE REF_Product_Category_API_Services ADD CONSTRAINT FK__REF_Product_Category_Service
FOREIGN KEY([ServiceId]) REFERENCES [dbo].[REF_Service](Id)

ALTER TABLE IT_UserMaster DROP COLUMN InspectionAccess

ALTER TABLE IT_UserMaster DROP COLUMN AuditAccess

ALTER TABLE IT_UserMaster DROP COLUMN TcfAccess

ALTER TABLE IT_UserMaster DROP COLUMN LabAccess

DROP TABLE REF_API_Services

-------------------------------45 Remove API Service Ends----------------------------------


----------------------------46 Hold Booking Reason Type Starts-------------------------------

CREATE TABLE [dbo].[INSP_REF_Hold_Reasons]
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Reason NVARCHAR(500),
	Active BIT,
	Sort INT,
	EntityId INT
)


CREATE TABLE [dbo].[INSP_TRAN_Hold_reason]
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	ReasonType INT,
	Comment NVARCHAR(MAX),
	Inspection_Id INT,
	Active BIT,
	CreatedOn DATETIME,
	CreatedBy INT,
	CONSTRAINT FK_HOLD_REASON FOREIGN KEY (ReasonType) REFERENCES [INSP_REF_Hold_Reasons],
	CONSTRAINT FK_INSPECTION_ID FOREIGN KEY (Inspection_Id) REFERENCES [INSP_TRANSACTION]
)

----insert queries for reason type master
insert into [INSP_REF_Hold_Reasons](Reason,Active,Sort,EntityId) Values
('Product Data Required',1,1,1)

insert into [INSP_REF_Hold_Reasons](Reason,Active,Sort,EntityId) Values
('Po Data Required',1,2,1)

insert into [INSP_REF_Hold_Reasons](Reason,Active,Sort,EntityId) Values
('Collection Data Required',1,3,1)

insert into [INSP_REF_Hold_Reasons](Reason,Active,Sort,EntityId) Values
('Price Category Data Required',1,4,1)

insert into [INSP_REF_Hold_Reasons](Reason,Active,Sort,EntityId) Values
('Others',1,5,1)

----insert query to move hold data from insp_transaction to INSP_TRAN_Hold_reason table
insert into INSP_TRAN_Hold_reason (ReasonType,comment,Inspection_Id,Active,CreatedOn)
select 5,holdreason,Id,1,getDate() from insp_transaction where holdreason is not null and HoldReason!=''

ALTER TABLE INSP_Transaction DROP COLUMN HoldReason

----------------------------46 Hold Booking Reason Type Ends-------------------------------

--------------------------------47 Booking Enhancement V2 Starts----------------------------------

CREATE TABLE [dbo].[REF_BUSINESS_LINE]
(
ID INT IDENTITY(1,1) PRIMARY KEY,
BusinessLine NVARCHAR(200),
Active BIT DEFAULT 1
)

INSERT INTO [dbo].[REF_BUSINESS_LINE](BusinessLine,Active) VALUES ('HardLine',1)
INSERT INTO [dbo].[REF_BUSINESS_LINE](BusinessLine,Active) VALUES ('SoftLine',1)

ALTER TABLE INSP_TRANSACTION ADD BusinessLine INT CONSTRAINT FK_INSP_Business_Line FOREIGN KEY(BusinessLine) REFERENCES [dbo].[REF_BUSINESS_LINE]

ALTER TABLE REF_ServiceType ADD ServiceId INT CONSTRAINT FK_SERVICE FOREIGN KEY(ServiceId) REFERENCES [dbo].[REF_SERVICE]

ALTER TABLE REF_ServiceType ADD BusinessLineId INT CONSTRAINT FK_BUSSINESS_LINE FOREIGN KEY(BusinessLineId) REFERENCES [dbo].[REF_BUSINESS_LINE]

ALTER TABLE REF_ServiceType ADD ShowServiceDateTo BIT

CREATE TABLE [dbo].[CU_ProductCategory]
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(500),
	Code NVARCHAR(500),
	Active BIT,
	CustomerId INT,
	Sector NVARCHAR(500),
	Sort INT,
	EntityId INT,
	CONSTRAINT FK_CUSTOMER_CU_PRODUCT_CATEGORY FOREIGN KEY(CustomerId) REFERENCES [dbo].[CU_Customer],
	CONSTRAINT FK_CU_PRODUCT_CATEGORY_ENTITY_ID FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity]
)


INSERT INTO [dbo].[CU_ProductCategory](Name,Code,Active,CustomerId,Sector,Sort,EntityId)
VALUES ('C-Electronics','CE',1,10,'Test Sector',1,1)


INSERT INTO [dbo].[CU_ProductCategory](Name,Code,Active,CustomerId,Sector,Sort,EntityId)
VALUES ('C-Furniture','CF',1,10,'Test Sector',1,1)

INSERT INTO [dbo].[CU_ProductCategory](Name,Code,Active,CustomerId,Sector,Sort,EntityId)
VALUES ('C-Toys','CT',1,10,'Test Sector',1,1)

ALTER TABLE INSP_TRANSACTION ADD CuProductCategory INT 
CONSTRAINT FK_INSP_CU_PRODUCT_CATEGORY FOREIGN KEY(CuProductCategory) REFERENCES [dbo].[CU_ProductCategory]

CREATE TABLE [dbo].[CU_Season_Config]
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	SeasonId INT,
	CustomerId INT,
	Active BIT,
	IsDefault BIT,
	EntityId INT,
	CONSTRAINT FK_CUSTOMER_CU_SEASON_CONFIG FOREIGN KEY(CustomerId) REFERENCES [dbo].[CU_Customer],
	CONSTRAINT FK_SEASON_CU_SEASON_CONFIG FOREIGN KEY(SeasonId) REFERENCES [dbo].[REF_Season],
	CONSTRAINT FK_CU_SEASON_CONFIG_ENTITY_ID FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity]
)

INSERT INTO [dbo].[CU_Season_Config](SeasonId,CustomerId,Active,IsDefault,EntityId)
VALUES (1,10,1,0,1)

INSERT INTO [dbo].[CU_Season_Config](SeasonId,CustomerId,Active,IsDefault,EntityId)
VALUES (2,10,1,0,1)

INSERT INTO [dbo].[CU_Season_Config](SeasonId,CustomerId,Active,IsDefault,EntityId)
VALUES (3,null,1,1,1)


ALTER TABLE INSP_TRANSACTION ADD Season INT CONSTRAINT FK_INSP_SEASON FOREIGN KEY(Season) REFERENCES [dbo].[REF_Season]

CREATE TABLE [dbo].[INSP_REF_InspectionLocation]
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(500),
	Active BIT,
	Sort INT
)

INSERT INTO [dbo].[INSP_REF_InspectionLocation] (Name,Active,Sort) VALUES ('Factory',1,1)

INSERT INTO [dbo].[INSP_REF_InspectionLocation] (Name,Active,Sort) VALUES ('Platform',1,2)

ALTER TABLE INSP_TRANSACTION ADD InspectionLocation INT CONSTRAINT FK_INSP_INSPECTION_LOCATION FOREIGN KEY(InspectionLocation) REFERENCES [dbo].[INSP_REF_InspectionLocation]

CREATE TABLE [dbo].[INSP_REF_ShipmentType]
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(500),
	Active BIT,
	Sort INT
)

INSERT INTO [dbo].[INSP_REF_ShipmentType](Name,Active,Sort) Values ('Shipment1',1,1)
INSERT INTO [dbo].[INSP_REF_ShipmentType](Name,Active,Sort) Values ('Shipment2',1,2)

CREATE TABLE [dbo].[INSP_TRAN_ShipmentType](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,
	[ShipmentTypeId] [int] NULL,
	[InspectionId] [int] NULL,
	[Active] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	CONSTRAINT [FK_INSP_SHIPMENT_TYPE] FOREIGN KEY([ShipmentTypeId]) REFERENCES [dbo].[INSP_REF_ShipmentType] ([Id]),
	CONSTRAINT [FK_INSP_TRANSACTION] FOREIGN KEY([InspectionId]) REFERENCES [dbo].[INSP_Transaction] ([Id]),
	CONSTRAINT [FK_INSP_SHIPMENT_CREATED_BY] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_INSP_SHIPMENT_DELETED_BY] FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id])
)

ALTER TABLE INSP_TRANSACTION ADD IsASReceived BIT

ALTER TABLE INSP_TRANSACTION ADD AsReceivedDate DATETIME

ALTER TABLE INSP_TRANSACTION ADD IsTFReceived BIT

ALTER TABLE INSP_TRANSACTION ADD TfReceivedDate DATETIME

ALTER TABLE INSP_TRANSACTION ADD ShipmentPort NVARCHAR(500)

ALTER TABLE INSP_TRANSACTION ADD ShipmentDate DATETIME

ALTER TABLE INSP_TRANSACTION ADD EAN NVARCHAR(500)

--------------------------------47 Booking Enhancement V2 Ends----------------------------------








-------------------------- 47 FB_Report_Comments new columns starts -------------------------

ALTER TABLE FB_Report_Comments ADD  Category NVARCHAR(2000) NULL

ALTER TABLE FB_Report_Comments ADD  Sub_Category NVARCHAR(2000) NULL

ALTER TABLE FB_Report_Comments ADD  Sub_Category2 NVARCHAR(2000) NULL

ALTER TABLE FB_Report_Comments ADD  CustomerReferenceCode NVARCHAR(2000) NULL

-------------------------- 47 FB_Report_Comments new columns ends -------------------------

-------------------------- 48 Email type and report type map Starts -------------------------
CREATE TABLE [dbo].[ES_Email_Report_Type_Map] 
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[EmailType] INT NOT NULL,
	[ReportType] INT NOT NULL,
	[Active] BIT NOT NULL, 
    CONSTRAINT FK_ES_Email_Report_Type_Map_EmailType FOREIGN KEY([EmailType]) REFERENCES ES_Type(Id),
	CONSTRAINT FK_ES_Email_Report_Type_Map_ReportType FOREIGN KEY([ReportType]) REFERENCES ES_REF_Report_Send_Type(Id)
)

-------------------------- 48 Email type and report type map ends -------------------------
-------------------------- 49 Visible To QC in Schedule booking Starts -------------------------
Alter table SCH_Schedule_QC add IsVisibleToQC bit NULL
-------------------------- 49 Visible To QC in Schedule booking ends -------------------------
-------------------------- 50 Upload Custom Report starts -------------------------

ALTER TABLE FB_REPORT_DETAILS ADD FinalManualReportPath NVARCHAR(1000)

CREATE TABLE [dbo].[FB_Report_Manual_Log](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[FbReportId] [int] NULL,
	[FileUrl] [nvarchar](max) NULL,
	[UniqueId] [nvarchar](1000) NULL,
	[FileName] [nvarchar](500) NOT NULL,
	[Active] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	CONSTRAINT [FK_FB_Report_Manual_Log_Created_By] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_FB_Report_Manual_Log_Deleted_By] FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_FB_Report_Manual_Log_Fb_Report_Id] FOREIGN KEY([FbReportId]) REFERENCES [dbo].[FB_Report_Details] ([Id])
)

ALTER TABLE [dbo].[FB_Report_Details] ADD CreatedBy INT CONSTRAINT FB_REPORT_DETAIL_CREATED_BY FOREIGN KEY(CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id)
ALTER TABLE [dbo].[FB_Report_Details] ADD UpdatedBy INT CONSTRAINT FB_REPORT_DETAIL_UPDATED_BY FOREIGN KEY(UpdatedBy) REFERENCES [dbo].[IT_UserMaster](Id)
ALTER TABLE [dbo].[FB_Report_Details] ADD UpdatedOn DATETIME
ALTER TABLE [dbo].[FB_Report_Details] ADD DeletedBy INT CONSTRAINT FB_REPORT_DETAIL_DELETED_BY FOREIGN KEY(DeletedBy) REFERENCES [dbo].[IT_UserMaster](Id)


-------------------------- 50 Upload Custom Report ends -------------------------


-------------------------- 51 Staff table change starts -------------------------
ALTER TABLE HR_Staff ADD StatusId INT 
CONSTRAINT FK_HR_Staff_StatusId FOREIGN KEY(StatusId) REFERENCES [dbo].[HR_REF_Status]

ALTER TABLE HR_Staff ADD BandId INT 
CONSTRAINT FK_HR_Staff_BandId FOREIGN KEY(BandId) REFERENCES [dbo].[HR_REF_Band]

ALTER TABLE HR_Staff ADD SocialInsuranceTypeId INT 
CONSTRAINT FK_HR_Staff_SocialInsuranceTypeId FOREIGN KEY(SocialInsuranceTypeId) REFERENCES [dbo].[HR_REF_Social_Insurance_type]

ALTER TABLE HR_Staff ADD HukoLocationId INT 
CONSTRAINT FK_HR_Staff_HukoLocationId FOREIGN KEY(HukoLocationId) REFERENCES [dbo].[Ref_City]

ALTER TABLE HR_Staff ADD MajorSubject nvarchar(500)
ALTER TABLE HR_Staff ADD EmergencyContactRelationship nvarchar(500)
ALTER TABLE HR_Staff ADD GlobalGrading nvarchar(500)
ALTER TABLE HR_Staff ADD NoticePeriod int


CREATE TABLE [dbo].[HR_REF_Band]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] nvarchar(200) NOT NULL,
	[Active] BIT NOT NULL,
	[Sort] INT  NULL, 	
)

CREATE TABLE [dbo].[HR_REF_Social_Insurance_type]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] nvarchar(200) NOT NULL,
	[Active] BIT NOT NULL,
	[Sort] INT  NULL, 	
)

CREATE TABLE [dbo].[HR_REF_Status]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] nvarchar(200) NOT NULL,
	[Active] BIT NOT NULL,
	[Sort] INT  NULL, 	
)

insert into HR_REF_Status values('On-job',1,1)
insert into HR_REF_Status values('Retired',1,2)
insert into HR_REF_Status values('Left',1,3)

insert into HR_REF_Band values('band1',1,1)
insert into HR_REF_Band values('band2',1,2)
insert into HR_REF_Band values('band3',1,3)

insert into HR_REF_Social_Insurance_type values('SocialInsurancetype1',1,1)
insert into HR_REF_Social_Insurance_type values('SocialInsurancetype2',1,2)
insert into HR_REF_Social_Insurance_type values('SocialInsurancetype3',1,3)

-------------------------- 51 Staff table change ends -------------------------

-------------------------- 52 EventLog starts -------------------------
Alter table EventLog add ResponseTime DateTime Null;
-------------------------- 52 EventLog ends -------------------------

-------------------------- 53 Complaint module starts -------------------------
INSERT [dbo].[IT_Right] ( [ParentId], [TitleName], [MenuName], [Path], [IsHeading], [Active], [Glyphicons], [Ranking], 
[MenuName_IdTran], [TitleName_IdTran], [EntityId], [ShowMenu], [RightType]) VALUES (36, NULL, 'Complaint','cuscomplaintsummary/customer-complaint-summary', 0, 1, N'fa fa-search', 
2, NULL, NULL,NULL,1,NULL)
 

CREATE TABLE [dbo].[COMP_REF_Type] 
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] NVARCHAR(50) NOT NULL, 
	[Active] BIT NOT NULL,
	[Sort] INT NOT NULL 
)

INSERT INTO  [dbo].[COMP_REF_Type]([Name],[Active],[Sort]) VALUES('Booking',1,1)
INSERT INTO  [dbo].[COMP_REF_Type]([Name],[Active],[Sort]) VALUES('General',1,2)

CREATE TABLE [dbo].[COMP_REF_Category] 
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] NVARCHAR(50) NOT NULL, 
	[Active] BIT NOT NULL,
	[Sort] INT NOT NULL 
)
INSERT INTO  [dbo].[COMP_REF_Category]([Name],[Active],[Sort]) VALUES('Category1',1,1)
INSERT INTO  [dbo].[COMP_REF_Category]([Name],[Active],[Sort]) VALUES('Category2',1,2)

CREATE TABLE [dbo].[COMP_REF_Recipient_Type] 
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] NVARCHAR(50) NOT NULL, 
	[Active] BIT NOT NULL,
	[Sort] INT NOT NULL 
)
INSERT INTO  [dbo].[COMP_REF_Recipient_Type]([Name],[Active],[Sort]) VALUES('RecipientType1',1,1)
INSERT INTO  [dbo].[COMP_REF_Recipient_Type]([Name],[Active],[Sort]) VALUES('RecipientType2',1,2)

CREATE TABLE [dbo].[COMP_REF_Department] 
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] NVARCHAR(50) NOT NULL, 
	[Active] BIT NOT NULL,
	[Sort] INT NOT NULL 
)
INSERT INTO  [dbo].[COMP_REF_Department]([Name],[Active],[Sort]) VALUES('Department1',1,1)
INSERT INTO  [dbo].[COMP_REF_Department]([Name],[Active],[Sort]) VALUES('Department2',1,2)

CREATE TABLE [dbo].[COMP_Complaints] 
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Type] INT NOT NULL,
	[Service] INT NULL,
	[Inspection_Id] INT  NULL, 
	[Audit_Id] INT NULL,
	[Complaint_Date] Datetime NULL,	
	[Recipient_Type] INT NOT NULL,
	[Department] INT NOT NULL,
	[Remarks] NVARCHAR(MAX),
	[CustomerId] INT NULL,
	[Country] INT NULL,
	[Office] INT NULL,
	[CreatedBy] INT NULL,
	[CreatedOn] Datetime NULL,	
	[DeletedBy] INT NULL,
	[DeletedOn] Datetime NULL,	
	Active bit,
    	CONSTRAINT FK_COMP_REF_Type_Type FOREIGN KEY([Type]) REFERENCES COMP_REF_Type(Id),
	CONSTRAINT FK_REF_Service_Service FOREIGN KEY([Service]) REFERENCES REF_Service(Id),
	CONSTRAINT FK_INSP_Transaction_Inspection_Id FOREIGN KEY([Inspection_Id]) REFERENCES [dbo].[INSP_Transaction](Id),
	CONSTRAINT FK_AUD_Transaction_Audit_Id FOREIGN KEY ([Audit_Id]) REFERENCES [dbo].[AUD_Transaction](Id),
	CONSTRAINT FK_COMP_REF_Recipient_Type_Recipient_Type FOREIGN KEY ([Recipient_Type]) REFERENCES [dbo].[COMP_REF_Recipient_Type](Id),
	CONSTRAINT FK_COMP_REF_Department_Department FOREIGN KEY ([Department]) REFERENCES [dbo].[COMP_REF_Department](Id),
	CONSTRAINT FK_CU_Customer_CustomerId FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[CU_Customer](Id),
	CONSTRAINT FK_REF_Country_Country FOREIGN KEY ([Country]) REFERENCES [dbo].[REF_Country](Id),
	CONSTRAINT FK_REF_Location_Office FOREIGN KEY ([Office]) REFERENCES [dbo].[REF_Location](Id),
	CONSTRAINT FK_IT_UserMaster_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_IT_UserMaster_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)


CREATE TABLE [dbo].[COMP_TRAN_ComplaintsDetails](
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[ComplaintId] [int] NOT NULL,
	[ProductId] [int] NULL,
	[Complaint_Category] [int] NOT NULL,
	[Complaint_Description] [nvarchar](max) NULL,
	[CorrectiveAction] [nvarchar](max) NULL,
	[AnswerDate] [datetime] NULL,
	[Remarks] [nvarchar](max) NULL,
	[Title] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[Active] [bit] NULL,
	CONSTRAINT [FK_COMP_Complaints_ComplaintId] FOREIGN KEY([ComplaintId]) REFERENCES [dbo].[COMP_Complaints] ([Id]),
	CONSTRAINT [FK_COMP_REF_Category_Complaint_Category] FOREIGN KEY([Complaint_Category]) REFERENCES [dbo].[COMP_REF_Category] ([Id]),
	CONSTRAINT [FK_CU_Products_ProductId] FOREIGN KEY([ProductId]) REFERENCES [dbo].[CU_Products] ([Id]),
	CONSTRAINT [FK_TRAN_ComplaintsDetails_CreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_TRAN_ComplaintsDetails_DeletedBy] FOREIGN KEY([DeletedBy])REFERENCES [dbo].[IT_UserMaster] ([Id])
 )

 CREATE TABLE [dbo].[COMP_TRAN_PersonInCharge](
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[ComplaintId] [int] NOT NULL,
	[PsersonInCharge] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[Active] [bit] NOT NULL,
	CONSTRAINT [FK_COMP_Complaints_ComplaintsId] FOREIGN KEY([ComplaintId]) REFERENCES [dbo].[COMP_Complaints] ([Id]),
	CONSTRAINT [FK_HR_Staff_PsersonInCharge] FOREIGN KEY([PsersonInCharge]) REFERENCES [dbo].[HR_Staff] ([Id]),
	CONSTRAINT [FK_IT_UserMaster_Comp_CreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_IT_UserMaster_Comp_DeletedBy] FOREIGN KEY([DeletedBy])
	REFERENCES [dbo].[IT_UserMaster] ([Id])
 ) 
 -------------------------- 53 Complaint module ends -------------------------
 
 -------------------------- 54 Logging start -------------------------

 
CREATE TABLE [dbo].[RestApiLog]
(
	[ID] int identity primary key,  
	[RequestMethod] NVARCHAR(200),
    [RequestPath] NVARCHAR(2000),
	[RequestQuery] NVARCHAR(2000),   
	[RequestBody] NVARCHAR(max),   
    [RequestTime] datetime,
	[ResponseTime] datetime,
	[ResponseInMilliSeconds] INT,
	[ResponseStatus] NVARCHAR(2000),   
	[EntityId] INT NULL,
	[CreatedBy] INT NULL,
	CONSTRAINT [FK_RestApiLog_CreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_RestApiLog_EntityId] FOREIGN KEY([EntityId]) REFERENCES [dbo].[Ap_Entity] ([Id])
)
 -------------------------- 54 Logging start -------------------------
-----------------#27  Edit Inspection Booking Ends------------------------


--# Link 2.0 updates start here

ALTER TABLE [dbo].[CU_Address] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[CU_Address] ADD CONSTRAINT CU_Address_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[CU_Address] set EntityId=1

ALTER TABLE [dbo].[CU_API_Services] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[CU_API_Services] ADD CONSTRAINT CU_API_Services_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[CU_API_Services] set EntityId=1

ALTER TABLE [dbo].[CU_Brand] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[CU_Brand] ADD CONSTRAINT CU_Brand_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[CU_Brand] set EntityId=1

ALTER TABLE [dbo].[CU_Buyer] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[CU_Buyer] ADD CONSTRAINT CU_Buyer_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[CU_Buyer] set EntityId=1

ALTER TABLE [dbo].[CU_Buyer_API_Services] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[CU_Buyer_API_Services] ADD CONSTRAINT CU_Buyer_API_Services_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[CU_Buyer_API_Services] set EntityId=1

ALTER TABLE [dbo].[CU_CheckPoints] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[CU_CheckPoints] ADD CONSTRAINT CU_CheckPoints_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[CU_CheckPoints] set EntityId=1

ALTER TABLE [dbo].[CU_CheckPoints_Brand] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[CU_CheckPoints_Brand] ADD CONSTRAINT CU_CheckPoints_Brand_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[CU_CheckPoints_Brand] set EntityId=1

ALTER TABLE [dbo].[CU_CheckPoints_Department] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[CU_CheckPoints_Department] ADD CONSTRAINT CU_CheckPoints_Department_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[CU_CheckPoints_Department] set EntityId=1

ALTER TABLE [dbo].[CU_CheckPoints_ServiceType] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[CU_CheckPoints_ServiceType] ADD CONSTRAINT CU_CheckPoints_ServiceType_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[CU_CheckPoints_ServiceType] set EntityId=1

ALTER TABLE [dbo].[CU_Collection] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[CU_Collection] ADD CONSTRAINT CU_Collection_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[CU_Collection] set EntityId=1

ALTER TABLE [dbo].[CU_CS_Configuration] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[CU_CS_Configuration] ADD CONSTRAINT CU_CS_Configuration_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[CU_CS_Configuration] set EntityId=1

ALTER TABLE [dbo].[CU_CustomerGroup] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[CU_CustomerGroup] ADD CONSTRAINT CU_CustomerGroup_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[CU_CustomerGroup] set EntityId=1

ALTER TABLE [dbo].[CU_Department] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[CU_Department] ADD CONSTRAINT CU_Department_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[CU_Department] set EntityId=1


ALTER TABLE [dbo].[CU_PR_Details] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[CU_PR_Details] ADD CONSTRAINT CU_PR_Details_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[CU_PR_Details] set EntityId=1

ALTER TABLE [dbo].[CU_PriceCategory] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[CU_PriceCategory] ADD CONSTRAINT CU_PriceCategory_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[CU_PriceCategory] set EntityId=1

ALTER TABLE [dbo].[CU_PurchaseOrder] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[CU_PurchaseOrder] ADD CONSTRAINT CU_PurchaseOrder_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[CU_PurchaseOrder] set EntityId=1

--ALTER TABLE [dbo].[CU_PurchaseOrder_Attachment] ADD [EntityId] INT NULL
--ALTER TABLE [dbo].[CU_PurchaseOrder_Attachment] ADD CONSTRAINT CU_PurchaseOrder_Attachment_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

--update [dbo].[CU_PurchaseOrder_Attachment] set EntityId=1

ALTER TABLE [dbo].[CU_PurchaseOrder_Details] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[CU_PurchaseOrder_Details] ADD CONSTRAINT CU_PurchaseOrder_Details_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[CU_PurchaseOrder_Details] set EntityId=1

ALTER TABLE [dbo].[CU_Season] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[CU_Season] ADD CONSTRAINT CU_Season_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[CU_Season] set EntityId=1

ALTER TABLE [dbo].[CU_ServiceType] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[CU_ServiceType] ADD CONSTRAINT CU_ServiceType_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[CU_ServiceType] set EntityId=1

ALTER TABLE [dbo].[CU_Products] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[CU_Products] ADD CONSTRAINT CU_Products_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)


update [dbo].[CU_Products] set EntityId=1

ALTER TABLE [dbo].[CU_Product_File_Attachment] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[CU_Product_File_Attachment] ADD CONSTRAINT CU_Product_File_Attach_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[CU_Product_File_Attachment] set EntityId=1

ALTER TABLE [dbo].[DF_CU_Configuration] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[DF_CU_Configuration] ADD CONSTRAINT DF_CU_CONFIG_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[DF_CU_Configuration] set EntityId=1

ALTER TABLE [dbo].[INV_REF_Office] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[INV_REF_Office] ADD CONSTRAINT INV_REF_Office_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[INV_REF_Office] set EntityId=1

--insert into REF_MarketSegment (Name,Active,EntityId)values('Sgt-Home',1,2)
--insert into REF_MarketSegment (Name,Active,EntityId)values('Sgt-Furniture',1,2)

select * from EM_ExchangeRateType
update EM_ExchangeRateType set EntityId=1 where id in(1,2)

--INSERT INTO EM_ExchangeRateType VALUES('Customer',47,1,2)
--INSERT INTO EM_ExchangeRateType VALUES('Expense Claim',48,1,2)

--INSERT INTO REF_ServiceType values('SGT Social Audit',1,2,0,null,null)
--INSERT INTO REF_ServiceType values('SGT Chemical Audit',1,2,0,null,null)
--INSERT INTO REF_ServiceType values('SGT Technical Audit',1,2,0,null,null)
--INSERT INTO REF_ServiceType values('SGT Final Random Inspection',1,2,0,null,null)
--INSERT INTO REF_ServiceType values('SGT Sample Check',1,2,0,null,null)
--INSERT INTO REF_ServiceType values('SGT Self Inspection Training',1,2,0,null,null)

--insert into EM_ExchangeRateType (Label,TypeTransId, Active,EntityId)values('SGT-Customer',47,1,2)
--insert into EM_ExchangeRateType (Label,TypeTransId,Active,EntityId)values('SGT-Expense',48,1,2)

select * from CU_PriceCategory

update CU_PriceCategory set EntityId=1

--INSERT INTO CU_PriceCategory(Name,CustomerId,Active,EntityId) values ('SGT Toys',2,1,2)
--INSERT INTO CU_PriceCategory(Name,CustomerId,Active,EntityId) values ('SGT HardGoods',2,1,2)

--select * from REF_Billing_Entity
--update REF_Billing_Entity set EntityId=1

--INSERT INTO REF_Billing_Entity(Name,Active,EntityId) values ('SGT Asia Pacific Inspection Ltd - HONG KONG',1,2)
--INSERT INTO REF_Billing_Entity(Name,Active,EntityId) values ('SGT Guangzhou Ouyatai - CHINA',1,2)
--INSERT INTO REF_Billing_Entity(Name,Active,EntityId) values ('SGT Asia Pacific Inspection Vietnam Company Ltd - VIETNAM',1,2)
--INSERT INTO REF_Billing_Entity(Name,Active,EntityId) values ('SGT API Audit Limited - HONG KONG (Audit)',1,2)

--update INV_REF_PaymentTerms set EntityId=1


update INV_REF_Office set EntityId=1

--INSERT INTO INV_REF_Office(Name,Address,Phone,Fax,Website,Mail,Active,EntityId) 
--	values('SGT New Office','China','901929929',null,'https://sgtlink.net/','test@gmail.com',1,2)
--INSERT INTO INV_REF_Office(Name,Address,Phone,Fax,Website,Mail,Active,EntityId) 
--	values('SGT Branch Office','China','901929929',null,'https://sgtlink.net/','test@gmail.com',1,2)

ALTER TABLE [dbo].[INV_TRAN_Invoice_Request_Contact] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[INV_TRAN_Invoice_Request_Contact] ADD CONSTRAINT INV_TRAN_Invoice_Request_Contact_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)


update [dbo].[INV_TRAN_Invoice_Request_Contact] set EntityId=1

--Update SU_Level set EntityId=1

update HR_Department set EntityId=1

--DECLARE @parentId INT
	
--INSERT INTO HR_Department(Department_Name,Active,Department_Code,DeptParentId,EntityId)
--VALUES('SGT HR',1,'SGTHRA',null,2)
--SET @parentId =@@IDENTITY
--INSERT INTO HR_Department(Department_Name,Active,Department_Code,DeptParentId,EntityId)
--VALUES('SGT HQ-HR & Admin',1,null,@parentId,2)

--INSERT INTO HR_Department(Department_Name,Active,Department_Code,DeptParentId,EntityId)
--VALUES('SGT Finance',1,'SGTFA',null,2)
--SET @parentId =@@IDENTITY

--INSERT INTO HR_Department(Department_Name,Active,Department_Code,DeptParentId,EntityId)
--VALUES('SGT HQ-Accounting & Finance',1,null,@parentId,2)


--INSERT INTO HR_Department(Department_Name,Active,Department_Code,DeptParentId,EntityId)
--VALUES('SGT IT',1,'SGTIT',null,2)
--	SET @parentId =@@IDENTITY
--INSERT INTO HR_Department(Department_Name,Active,Department_Code,DeptParentId,EntityId)
--VALUES('SGT HQ-IT & Business Analytics',1,null,@parentId,2)

--INSERT INTO HR_Department(Department_Name,Active,Department_Code,DeptParentId,EntityId)
--VALUES('SGT Quality & Internal Audit',1,'SGTQIA',null,2)
--SET @parentId =@@IDENTITY
--INSERT INTO HR_Department(Department_Name,Active,Department_Code,DeptParentId,EntityId)
--VALUES('SGT HQ-Quality and Technical',1,null,@parentId,2)

--INSERT INTO HR_Department(Department_Name,Active,Department_Code,DeptParentId,EntityId)
--VALUES('SGT Sales',1,'SGTSAL',null,2)
--SET @parentId =@@IDENTITY
--INSERT INTO HR_Department(Department_Name,Active,Department_Code,DeptParentId,EntityId)
--VALUES('SGT HQ - Sales',1,null,@parentId,2)
--INSERT INTO HR_Department(Department_Name,Active,Department_Code,DeptParentId,EntityId)
--VALUES('SGT HQ - Marketing',1,null,@parentId,2)

update REF_Expertise set EntityId=1

--INSERT INTO REF_Expertise(Name,Active,EntityId)VALUES('SGT Mass Market',1,2)
--INSERT INTO REF_Expertise(Name,Active,EntityId)VALUES('SGT Toys',1,2)
--INSERT INTO REF_Expertise(Name,Active,EntityId)VALUES('SGT Infant Toys',1,2)
--INSERT INTO REF_Expertise(Name,Active,EntityId)VALUES('SGT Electrical',1,2)
--INSERT INTO REF_Expertise(Name,Active,EntityId)VALUES('SGT Wood Items',1,2)
--INSERT INTO REF_Expertise(Name,Active,EntityId)VALUES('SGT Festive decorations',1,2)
--INSERT INTO REF_Expertise(Name,Active,EntityId)VALUES('SGT DIY',1,2)
--INSERT INTO REF_Expertise(Name,Active,EntityId)VALUES('SGT Cookware',1,2)
--INSERT INTO REF_Expertise(Name,Active,EntityId)VALUES('SGT Kitchen Ware',1,2)
--INSERT INTO REF_Expertise(Name,Active,EntityId)VALUES('SGT Outdoor furniture',1,2)
--INSERT INTO REF_Expertise(Name,Active,EntityId)VALUES('SGT Indoor furniture',1,2)


CREATE TABLE [dbo].[CU_Contact_Entity_Map]
(
	ContactId  INT NOT NULL,
	EntityId INT NOT NULL, 
	PRIMARY KEY(ContactId, EntityId),
    CONSTRAINT FK_CU_Contact_Entity_Map_ContactId FOREIGN KEY(ContactId) REFERENCES Cu_Contact(Id),
	CONSTRAINT FK_CU_Contact_Entity_Map_EntityId FOREIGN KEY(EntityId) REFERENCES AP_Entity(Id)
)


CREATE TABLE [dbo].[SU_Contact_Entity_Map]
(
	ContactId  INT NOT NULL,
	EntityId INT NOT NULL, 
	PRIMARY KEY(ContactId, EntityId),
    CONSTRAINT FK_SU_Contact_Entity_Map_ContactId FOREIGN KEY(ContactId) REFERENCES Su_Contact(Id),
	CONSTRAINT FK_SU_Contact_Entity_Map_EntityId FOREIGN KEY(EntityId) REFERENCES AP_Entity(Id)
)


CREATE TABLE [dbo].[HR_Entity_Map]
(
	StaffId  INT NOT NULL,
	EntityId INT NOT NULL, 
	PRIMARY KEY(StaffId, EntityId),
    CONSTRAINT FK_HR_Entity_Map_ContactId FOREIGN KEY(StaffId) REFERENCES HR_staff(Id),
	CONSTRAINT FK_HR_Entity_Map_EntityId FOREIGN KEY(EntityId) REFERENCES AP_Entity(Id)
)

select * from [dbo].[HR_Entity_Map]

insert into [HR_Entity_Map] (StaffId,EntityId)values(334,1)
insert into [HR_Entity_Map] (StaffId,EntityId)values(334,2)


ALTER TABLE cu_customer
DROP CONSTRAINT FK__CU_Custom__Entit__1181FF99;

ALTER TABLE cu_customer DROP COLUMN EntityId

ALTER TABLE su_supplier
DROP CONSTRAINT FK__SU_Suppli__Entit__6ED7E1DA;

ALTER TABLE su_supplier DROP COLUMN Entity_Id

ALTER TABLE hr_staff
DROP CONSTRAINT FK__HR_Staff__Entity__7A7E9EB0;

select * from hr_staff

ALTER TABLE hr_staff DROP COLUMN EntityId



ALTER TABLE [dbo].[EC_ExpensesClaimDetais] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[EC_ExpensesClaimDetais] ADD CONSTRAINT EC_ExpensesClaimDetais_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)


--ALTER TABLE IT_UserMaster add  PrimaryEntity int null
--ALTER TABLE IT_UserMaster add CONSTRAINT FK_IT_USER_Entity_PrimaryEntity FOREIGN KEY(PrimaryEntity) REFERENCES AP_Entity(Id)


ALTER TABLE [dbo].[HR_Leave] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[HR_Leave] ADD CONSTRAINT HR_Leave_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update HR_Leave set EntityId=1

ALTER TABLE REF_Product_Category_API_Services DROP CONSTRAINT PK__REF_Prod__3214EC07F5573F5C
ALTER TABLE REF_Product_Category_API_Services ADD CONSTRAINT FK_REF_Product_Category_Service FOREIGN KEY (ServiceId) REFERENCES REF_SERVICE(Id)

ALTER TABLE IT_UserMaster
DROP CONSTRAINT FK__IT_UserMa__Entit__33B71C0C

ALTER TABLE IT_UserMaster DROP COLUMN EntityId

ALTER TABLE [dbo].[DA_UserCustomer] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[DA_UserCustomer] ADD CONSTRAINT FK_DAUserCustomer_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[DA_UserCustomer] set EntityId=1


ALTER TABLE [dbo].[DA_UserRoleNotificationByOffice] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[DA_UserRoleNotificationByOffice] ADD CONSTRAINT FK_DAUserRoleNotificationByOffice_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[DA_UserRoleNotificationByOffice] set EntityId=1

ALTER TABLE [dbo].[DA_UserByService] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[DA_UserByService] ADD CONSTRAINT FK_DAUserByService_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[DA_UserByService] set EntityId=1

ALTER TABLE [dbo].[DA_UserByProductCategory] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[DA_UserByProductCategory] ADD CONSTRAINT FK_DAUserByProductCategory_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[DA_UserByProductCategory] set EntityId=1

ALTER TABLE [dbo].[DA_UserByRole] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[DA_UserByRole] ADD CONSTRAINT FK_DAUserByRole_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)


update [dbo].[DA_UserByRole] set EntityId=1

ALTER TABLE [dbo].[DA_UserByDepartment] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[DA_UserByDepartment] ADD CONSTRAINT FK_DAUserByDepartment_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[DA_UserByDepartment] set EntityId=1

ALTER TABLE [dbo].[DA_UserByBrand] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[DA_UserByBrand] ADD CONSTRAINT FK_DAUserByBrand_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[DA_UserByBrand] set EntityId=1

ALTER TABLE IT_UserRole ADD [EntityId] INT not null  DEFAULT 1;
ALTER TABLE IT_UserRole ADD CONSTRAINT FK_IT_UserRole_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[IT_UserRole] set EntityId=1

 ALTER TABLE  IT_UserRole 
    DROP CONSTRAINT PK__IT_UserR__AF2760ADDFC617FD

ALTER TABLE IT_UserRole
ADD CONSTRAINT pk_user_role_entity PRIMARY KEY (UserId, RoleId,EntityId)

ALTER TABLE [dbo].[QC_BlockList] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[QC_BlockList] ADD CONSTRAINT FK_QC_BlockList_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update QC_BlockList set EntityId=1

 --ALTER TABLE  CU_PurchaseOrder_Attachment DROP CONSTRAINT CU_PurchaseOrder_Attachment_EntityId
 --ALTER TABLE  CU_PurchaseOrder_Attachment DROP COLUMN EntityId


ALTER TABLE [dbo].[INSP_Product_Transaction] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[INSP_Product_Transaction] ADD CONSTRAINT FK_INSP_Product_Transaction_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[INSP_Product_Transaction] set EntityId=1

ALTER TABLE [dbo].[INSP_PurchaseOrder_Transaction] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[INSP_PurchaseOrder_Transaction] ADD CONSTRAINT FK_INSP_PurchaseOrder_Transaction_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[INSP_PurchaseOrder_Transaction] set EntityId=1

ALTER TABLE [dbo].[INSP_Container_Transaction] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[INSP_Container_Transaction] ADD CONSTRAINT FK_INSP_Container_Transaction_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[INSP_Container_Transaction] set EntityId=1

--insert into REF_ServiceType(Name,Active,EntityId,IsReInspectedService) Values ('SGT Container Loading Supervision',1,2,0)

ALTER TABLE [dbo].[QU_QUOTATION] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[QU_QUOTATION] ADD CONSTRAINT FK_QU_QUOTATION_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update QU_QUOTATION set EntityId=1


ALTER TABLE [dbo].[INV_TM_Details] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[INV_TM_Details] ADD CONSTRAINT FK_INV_TM_Details_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update INV_TM_Details set EntityId=1

ALTER TABLE [dbo].[INV_AUT_TRAN_Details] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[INV_AUT_TRAN_Details] ADD CONSTRAINT FK_INV_AUT_TRAN_Details_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update INV_AUT_TRAN_Details set EntityId=1

ALTER TABLE [dbo].[INV_EXF_Transaction] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[INV_EXF_Transaction] ADD CONSTRAINT FK_INV_EXF_Transaction_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update INV_EXF_Transaction set EntityId=1

--update IT_UserMaster set PrimaryEntity=1
 
ALTER TABLE [dbo].[INSP_IC_Transaction] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[INSP_IC_Transaction] ADD CONSTRAINT FK_INSP_IC_Transaction_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update INSP_IC_Transaction set EntityId=1

select * from [dbo].[AP_Entity]

Insert into [dbo].[AP_Entity](Name,Active)values('SGT',1)
Insert into [dbo].[AP_Entity](Name,Active)values('AQF',1)

ALTER TABLE [dbo].[AP_Entity] ADD [FB_ID] INT

UPDATE [dbo].[AP_Entity] SET [FB_ID]=2727 where Id=1

UPDATE [dbo].[AP_Entity] SET [FB_ID]=14904 where Id=2

UPDATE [dbo].[AP_Entity] SET [FB_ID]=14905 where Id=3

CREATE TABLE [dbo].[HR_Staff_Services]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[StaffId] INT,
	[ServiceId] INT,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL,
	[Active] BIT,
	CONSTRAINT FK_HR_Staff_Services_CreatedBy	FOREIGN KEY([CreatedBy]) REFERENCES [IT_UserMaster](Id),
	CONSTRAINT FK_HR_Staff_Services_DeletedBy	FOREIGN KEY([DeletedBy]) REFERENCES [IT_UserMaster](Id),
	CONSTRAINT FK_HR_Staff_Services_StaffId	FOREIGN KEY([StaffId]) REFERENCES [HR_Staff](Id),
	CONSTRAINT FK_Staff_API_ServiceId FOREIGN KEY (ServiceId)  REFERENCES [dbo].[REF_Service](Id)
)

-- username is sabari
insert into [dbo].[HR_Staff_Services] (StaffId,ServiceId,Active) values(334,1,1)

CREATE TABLE [dbo].[HR_Staff_Entity_Service_Map]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[StaffId] INT,
	[ServiceId] INT,
	[EntityId] INT,
	CONSTRAINT FK_HR_Staff_Entity_Service_Map_StaffId	FOREIGN KEY([StaffId]) REFERENCES [HR_Staff](Id),
	CONSTRAINT FK_HR_Staff_Entity_Service_Map_ServiceId FOREIGN KEY (ServiceId)  REFERENCES [dbo].[REF_Service](Id),
	CONSTRAINT FK_HR_Staff_Entity_Service_Map_EntityId FOREIGN KEY (EntityId)  REFERENCES [dbo].[AP_Entity](Id)
)
-- username is sabari
insert into[dbo].[HR_Staff_Entity_Service_Map] (StaffId,ServiceId,EntityId) values(334,1,1)
insert into[dbo].[HR_Staff_Entity_Service_Map] (StaffId,ServiceId,EntityId) values(334,1,2)

select * from HR_Staff_Entity_Service_Map



CREATE TABLE [dbo].[SU_Contact_Entity_Service_Map]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ContactId] INT,
	[ServiceId] INT,
	[EntityId] INT,
	CONSTRAINT FK_SU_Contact_Entity_Service_Map_ContactId	FOREIGN KEY([ContactId]) REFERENCES [dbo].[SU_Contact](Id),
	CONSTRAINT FK_SU_Contact_Entity_Service_Map_ServiceId FOREIGN KEY (ServiceId)  REFERENCES [dbo].[REF_Service](Id),
	CONSTRAINT FK_SU_Contact_Entity_Service_Map_EntityId FOREIGN KEY (EntityId)  REFERENCES [dbo].[AP_Entity](Id)
)

CREATE TABLE [dbo].[CU_Contact_Entity_Service_Map]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ContactId] INT,
	[ServiceId] INT,
	[EntityId] INT,
	CONSTRAINT FK_CU_Contact_Entity_Service_Map_ContactId	FOREIGN KEY([ContactId]) REFERENCES [dbo].[Cu_Contact](Id),
	CONSTRAINT FK_CU_Contact_Entity_Service_Map_ServiceId FOREIGN KEY (ServiceId)  REFERENCES [dbo].[REF_Service](Id),
	CONSTRAINT FK_CU_Contact_Entity_Service_Map_EntityId FOREIGN KEY (EntityId)  REFERENCES [dbo].[AP_Entity](Id)
)


--Alter table Hr_Staff drop column EntityId;
--Alter table It_usermaster drop column EntityId;
--Alter table It_usermaster drop constraint FK_IT_USER_Entity_PrimaryEntity;
--Alter table It_usermaster drop column primaryentity;

ALTER TABLE Hr_Staff add  PrimaryEntity int null
ALTER TABLE Hr_Staff add CONSTRAINT FK_Hr_Staff_PrimaryEntity FOREIGN KEY(PrimaryEntity) REFERENCES AP_Entity(Id)

ALTER TABLE SU_Contact add  PrimaryEntity int null
ALTER TABLE SU_Contact add CONSTRAINT FK_SU_Contact_PrimaryEntity FOREIGN KEY(PrimaryEntity) REFERENCES AP_Entity(Id)

ALTER TABLE CU_Contact add  PrimaryEntity int null
ALTER TABLE CU_Contact add CONSTRAINT FK_CU_Contact_PrimaryEntity FOREIGN KEY(PrimaryEntity) REFERENCES AP_Entity(Id)
select * from hr_staff where id=334

update Hr_Staff set PrimaryEntity=1
update SU_Contact set PrimaryEntity=1
update CU_Contact set PrimaryEntity=1

Update REF_ServiceType Set ServiceId=2 where id in (1,2,3,4)

Update REF_ServiceType Set ServiceId=1 where id not in (1,2,3,4)

Update REF_ServiceType Set ShowServiceDateTo=1

--Update HardLine as BusinessLine for the service types which it belongs
Update REF_ServiceType Set BusinessLineId=1 where id <=42

ALTER TABLE LOG_Email_Queue add EntityId INT null;

ALTER TABLE LOG_Email_Queue add	CONSTRAINT FK_LOG_Email_Queue_ENTITY_ID FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity]

update LOG_Email_Queue set EntityId=1

--Update SoftLine as BusinessLine for the service types which belongs
--Update REF_ServiceType Set BusinessLineId=2 where id >42



-- Customer contact and entity and service mapping update start here

update Cu_Contact set PrimaryEntity=1

truncate table CU_Contact_Entity_Map

Insert into [dbo].[CU_Contact_Entity_Map] 
 (ContactId,EntityId)
(
   select distinct  CustomerContactId,1  from IT_UserMaster with(nolock) where UserTypeId=2 and CustomerContactId is not null 
);

truncate table CU_Contact_Entity_Service_Map

 -- Service and entity mapping
 Insert into [dbo].[CU_Contact_Entity_Service_Map](ContactId,ServiceId,EntityId) ( 
 select ContactId,ServiceId,1 from [dbo].[Cu_Contact_Service] where ContactId in(select CustomerContactId  from IT_UserMaster with(nolock) where UserTypeId=2 and CustomerContactId is not null) 
 )

 -- Customer contact and entity and service mapping update end here

 -- Supplier contact and entity and service mapping update start here
 update SU_Contact set PrimaryEntity=1

truncate table SU_Contact_Entity_Map

Insert into [dbo].[SU_Contact_Entity_Map] 
 (ContactId,EntityId)
(
   select SupplierContactId,1  from IT_UserMaster with(nolock) where UserTypeId=3 and SupplierContactId is not null
);

truncate table SU_Contact_Entity_Service_Map
 
 Insert into [dbo].[SU_Contact_Entity_Service_Map](ContactId,ServiceId,EntityId) ( 
 select ContactId,ServiceId,1 from [dbo].[SU_Contact_API_Services] where ContactId in(select SupplierContactId  from IT_UserMaster with(nolock) where UserTypeId=3 and SupplierContactId is not null)
 )
 -- Supplier contact and entity and service mapping update end here

  -- Factory contact and entity and service mapping update start here

 truncate table SU_Contact_Entity_Map

Insert into [dbo].[SU_Contact_Entity_Map] 
 (ContactId,EntityId)
(
   select FactoryContactId,1  from IT_UserMaster with(nolock) where UserTypeId=4 and FactoryContactId is not null
);

truncate table SU_Contact_Entity_Service_Map

 Insert into [dbo].[SU_Contact_Entity_Service_Map](ContactId,ServiceId,EntityId) ( 
 select ContactId,ServiceId,1 from [dbo].[SU_Contact_API_Services] where ContactId in(select FactoryContactId  from IT_UserMaster with(nolock) where UserTypeId=4 and FactoryContactId is not null)
 )

  -- Factory contact and entity and service mapping update end here

-- Internal user and entity and service mapping update start here

update Hr_Staff set PrimaryEntity=1

truncate table [dbo].[HR_Entity_Map]

Insert into [dbo].[HR_Entity_Map] 
 (StaffId,EntityId)
(
   select distinct StaffId,1  from IT_UserMaster with(nolock) where UserTypeId=1 and StaffId is not null
);

truncate table [dbo].[HR_Staff_Services]
-- inspection service
 Insert into [dbo].[HR_Staff_Services] (StaffId,ServiceId,Active)(
   select StaffId,1,1  from IT_UserMaster with(nolock) where UserTypeId=1 and StaffId is not null
);

-- Audit service
 Insert into [dbo].[HR_Staff_Services] (StaffId,ServiceId,Active)(
   select StaffId,2,1  from IT_UserMaster with(nolock) where UserTypeId=1 and StaffId is not null
 );

-- TCF service
 Insert into [dbo].[HR_Staff_Services] (StaffId,ServiceId,Active)(
 select StaffId,3,1 from IT_UserMaster where id in (
 select UserId from IT_UserRole where RoleId=37 and userid in(select Id from IT_UserMaster with (nolock) where UserTypeId=1 and StaffId is not null))
 );

 truncate table [dbo].[HR_Staff_Entity_Service_Map]
 -- inspection service
 Insert into [dbo].[HR_Staff_Entity_Service_Map](StaffId,ServiceId,EntityId) ( 
  select StaffId,1,1  from IT_UserMaster with (nolock) where UserTypeId=1 and StaffId is not null
 )

 -- Audit service
  Insert into [dbo].[HR_Staff_Entity_Service_Map](StaffId,ServiceId,EntityId) ( 
  select StaffId,2,1  from IT_UserMaster with (nolock) where UserTypeId=1 and StaffId is not null
 )
 -- Tcf service
 Insert into [dbo].[HR_Staff_Entity_Service_Map](StaffId,ServiceId,EntityId) ( 
  select StaffId,3,1 from IT_UserMaster where id in (
  select UserId from IT_UserRole where RoleId=37 and userid in(select Id from IT_UserMaster with (nolock) where UserTypeId=1 and StaffId is not null))
 )

-- Internal user and entity and service mapping update end here

ALTER TABLE [dbo].[REF_Budget_Forecast] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[REF_Budget_Forecast] ADD CONSTRAINT FK_REF_Budget_Forecast_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

ALTER TABLE [dbo].[MID_Task] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[MID_Task] ADD CONSTRAINT FK_MID_Task_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

ALTER TABLE [dbo].[MID_Notification] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[MID_Notification] ADD CONSTRAINT FK_MID_Notification_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

ALTER TABLE [dbo].[REF_Zone] ADD [EntityId] INT NULL
ALTER TABLE [dbo].[REF_Zone] ADD CONSTRAINT FK_REF_Zone_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)

update [dbo].[REF_Budget_Forecast] set EntityId=1
update [dbo].[MID_Task] set EntityId=1
update [dbo].[MID_Notification] set EntityId=1
update [dbo].[REF_Zone] set EntityId=1

--#  Link 2.0 updates end here


--------- As Received and TF Received Date scripst starts----------------------


Alter Table insp_product_transaction ADD AsReceivedDate DATETIME

Alter Table insp_product_transaction ADD TfReceivedDate DATETIME

Alter Table insp_transaction DROP COLUMN IsAsReceived

Alter Table insp_transaction DROP COLUMN IsTfReceived

Alter Table insp_transaction DROP COLUMN AsReceivedDate 

Alter Table insp_transaction DROP COLUMN TfReceivedDate 

--------- As Received and TF Received Date scripst ends----------------------


---- FB bar code info updates here start --- 
Create TABLE FB_Report_ProductBarcodesInfo
(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ProductId] INT NULL,
	[FbReportId] INT NOT NULL,
	[BarCode] NVARCHAR(500) NULL,	
	[Description] NVARCHAR(2000) NULL,
    CONSTRAINT FK_FB_Report_ProductBarcodesInfo_FbReportId	FOREIGN KEY(FbReportId) REFERENCES [FB_Report_Details](Id),
	CONSTRAINT FB_Report_ProductBarcodesInfo_ProductId   FOREIGN KEY(ProductId) REFERENCES [CU_Products](Id)
)

ALTER TABLE FB_Report_Product_Dimention  ADD [Description] NVARCHAR(1000) NULL;
ALTER TABLE FB_Report_Product_Dimention  ADD [Unit] NVARCHAR(100) NULL;

ALTER TABLE FB_Report_Product_Weight ADD [Description] NVARCHAR(1000) NULL;
ALTER TABLE FB_Report_Product_Weight ADD [Unit] NVARCHAR(100) NULL;

ALTER TABLE FB_Report_Packing_Dimention  ADD [Unit] NVARCHAR(100) NULL;


ALTER TABLE FB_Report_Packing_Weight ADD [Unit] NVARCHAR(100) NULL;
ALTER TABLE FB_Report_Packing_Weight ADD [PackingType] NVARCHAR(500) NULL;

ALTER TABLE FB_Report_Details ADD [Aql_Level] INT NULL CONSTRAINT FK_FB_Report_Details_Aql_Level FOREIGN KEY(Aql_Level) REFERENCES [REF_LevelPick1](Id) 
ALTER TABLE FB_Report_Details ADD [SampleSize] INT NULL

ALTER TABLE FB_Report_Details ADD [Found_Critical] INT NULL
ALTER TABLE FB_Report_Details ADD [Found_Major] INT NULL
ALTER TABLE FB_Report_Details ADD [Found_Minor] INT NULL

ALTER TABLE FB_Report_Details ADD [Aql_Critical] FLOAT NULL
ALTER TABLE FB_Report_Details ADD [Aql_Major] FLOAT NULL
ALTER TABLE FB_Report_Details ADD [Aql_Minor] FLOAT NULL

---- FB bar code info updates here end --- 

ALTER TABLE HR_OfficeControl add EntityId INT null;

ALTER TABLE HR_OfficeControl add	CONSTRAINT FK_HR_OfficeControl_ENTITY_ID FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity]


----------------Email Send Starts----------------
INSERT INTO ES_REF_RecipientType(Name,Active) values('Merchandiser',1)
ALTER TABLE es_details ADD Is_Include_MC bit

----------------Email Send ends----------------

-- Travel tarif start --

CREATE TABLE EC_AUT_REF_StartPort
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

insert into EC_AUT_REF_StartPort(StartPortName,Entity_Id,Active,Sort,CreatedBy)values('StartPort1',1,1,1,273)
insert into EC_AUT_REF_StartPort(StartPortName,Entity_Id,Active,Sort,CreatedBy)values('StartPort2',1,1,1,273)


CREATE TABLE EC_AUT_TravelTariff
(
	[Id] int not null primary key identity(1,1),
	[StartPort] int not null,
	[TownId] int not null,	
	[TravelTariff] float null,
	[TravelCurrency] int null,
	[StartDate] datetime,
	[EndDate] datetime,
	[Entity_Id] INT , 
	[Active] bit, 	
	[CreatedOn] datetime,
	[UpdatedOn] datetime,
	[DeletedOn] datetime,
	[UpdatedBy] int null,
	[DeletedBy] int null,
	[CreatedBy] int null,
	CONSTRAINT FK_EC_AUT_TravelTariff_StartPort FOREIGN KEY ([StartPort]) REFERENCES [dbo].[EC_AUT_REF_StartPort](Id),	
	CONSTRAINT FK_EC_AUT_TravelTariff_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
	CONSTRAINT FK_EC_AUT_TravelTariff_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_EC_AUT_TravelTariff_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
	CONSTRAINT FK_EC_AUT_TravelTariff_TownId FOREIGN KEY ([TownId]) REFERENCES [dbo].[Ref_Town](Id),
    CONSTRAINT FK_EC_AUT_TravelTariff_Entity_Id FOREIGN KEY([Entity_Id]) REFERENCES [dbo].[AP_Entity](Id),
	CONSTRAINT FK_EC_AUT_TravelTariff_TravelCurrency FOREIGN KEY([TravelCurrency]) REFERENCES [dbo].[Ref_Currency](Id)
)


-- Travel tarif end --

---------------------------Purchase Order Color Transactions Starts--------------------------------

CREATE TABLE [dbo].[INSP_PurchaseOrder_Color_Transaction](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ColorCode] [nvarchar](50) NULL,
	[ColorName] [nvarchar](50) NULL,
	[PoTransId] [int] NULL,
	[ProductRefId] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[Active] [bit] NULL,
	[EntityId] [int] NULL,
	CONSTRAINT [FK_Color_Trans_Created_By] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_Color_Trans_Deleted_By] FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_Color_Trans_EntityId] FOREIGN KEY([EntityId]) REFERENCES [dbo].[AP_Entity] ([Id]),
	CONSTRAINT [FK_Color_Trans_Po_Trans_Id] FOREIGN KEY([PoTransId]) REFERENCES [dbo].[INSP_PurchaseOrder_Transaction] ([Id]),
	CONSTRAINT [FK_Color_Trans_Product_Trans_Id] FOREIGN KEY([ProductRefId]) REFERENCES [dbo].[INSP_Product_Transaction] ([Id]),
	CONSTRAINT [FK_Color_Trans_Updated_By] FOREIGN KEY([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id])
)

---------------------------Purchase Order Color Transactions Ends--------------------------------



ALTER TABLE HR_OfficeControl add	CONSTRAINT FK_HR_OfficeControl_ENTITY_ID FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity]

----------------------------Food Allowance new columns starts ------------------

Alter table EC_FoodAllowance add Active Bit Default  1
Alter table EC_FoodAllowance add CreatedOn DateTime Null

Alter table EC_FoodAllowance add UpdatedBy INT Null;
ALTER TABLE EC_FoodAllowance ADD CONSTRAINT FK_EC_FoodAllowance_UpdatedById FOREIGN KEY (UpdatedBy) REFERENCES [IT_UserMaster](Id)

Alter table EC_FoodAllowance add UpdatedOn DateTime Null

Alter table EC_FoodAllowance add DeletedBy INT Null;
ALTER TABLE EC_FoodAllowance ADD CONSTRAINT FK_EC_FoodAllowance_DeletedById FOREIGN KEY (DeletedBy) REFERENCES [IT_UserMaster](Id)

Alter table EC_FoodAllowance add DeletedOn DateTime Null

----------------------------Food Allowance new columns ends here-----------------------

----------------------------Starting Port add columns starts ---------------------------------
Alter table EC_AUT_REF_StartPort add CityId INT Null
ALTER TABLE EC_AUT_REF_StartPort ADD CONSTRAINT FK_EC_AUT_REF_StartPort_CityId FOREIGN KEY (CityId) REFERENCES [Ref_City](Id)

Alter table HR_Staff add StartPortId INT Null
ALTER TABLE HR_Staff ADD CONSTRAINT FK_HR_Staff_StartPortId FOREIGN KEY (StartPortId) REFERENCES [EC_AUT_REF_StartPort](Id)
----------------------------Starting Port add columns ends---------------------------------

----------------------------Food Allowance new columns ends here-----------------------

-- Schedule job start -- 

CREATE TABLE JOB_Schedule_Log
(
    [Id] int not null primary key identity(1,1),
	[Booking_Id] INT, 
	[Report_Id] INT ,	
	[Schedule_Type] INT ,
	[FileName] nvarchar(500), 
	[CreatedOn] datetime
    CONSTRAINT FK_JOB_Schedule_Log_Booking_Id FOREIGN KEY([Booking_Id]) REFERENCES [dbo].[INSP_Transaction](Id),
	CONSTRAINT FK_JOB_Schedule_Log_Report_Id FOREIGN KEY([Report_Id]) REFERENCES [dbo].[FB_Report_Details](Id)
)

-- Schedule job end -- 


--QC Auto expense start --

CREATE TABLE [dbo].[ENT_REF_Features]
(
    [Id] int not null primary key identity(1,1),
	[Name] nvarchar(500),	
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,	
	[Active] [bit] NULL,
	CONSTRAINT [FK_ENT_REF_Features_Created_By] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_ENT_REF_Features_Deleted_By] FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id])
)

INSERT INTO [dbo].[ENT_REF_Features] (Name,Active) VALUES('Auto QC Expense Claims',1)

CREATE TABLE [dbo].[EC_AUT_REF_TripType]
(
    [Id] int not null primary key identity(1,1),
	[Name] nvarchar(500),	
	[Sort] int,
	[Active] [bit] NULL
)

INSERT INTO [dbo].[EC_AUT_REF_TripType] (Name,Active) VALUES('No Trip',1)
INSERT INTO [dbo].[EC_AUT_REF_TripType] (Name,Active) VALUES('Single Trip',1)
INSERT INTO [dbo].[EC_AUT_REF_TripType] (Name,Active) VALUES('Round Trip',1)

CREATE TABLE [dbo].[ENT_Feature_Details]
(
    [Id] int not null primary key identity(1,1),
	[FeatureId] [int],	
	[EntityId] [int],	
	[CountryId] [int],	
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,	
	[Active] [bit] NULL,
	CONSTRAINT [FK_ENT_Feature_Details_FeatureId] FOREIGN KEY([FeatureId]) REFERENCES [dbo].[ENT_REF_Features] ([Id]),
	CONSTRAINT [FK_ENT_Feature_Details_EntityId] FOREIGN KEY([EntityId]) REFERENCES [dbo].[AP_Entity] ([Id]),
	CONSTRAINT [FK_ENT_Feature_Details_CountryId] FOREIGN KEY([CountryId]) REFERENCES [dbo].[REF_Country] ([Id]),
	CONSTRAINT [FK_ENT_Feature_Details_Created_By] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_ENT_Feature_Details_Deleted_By] FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
)

ALTER TABLE REF_ServiceType ADD [IsAutoQCExpenseClaim] BIT NULL

ALTER TABLE EC_AUT_TravelTariff ADD [Status] BIT NULL

--CREATE TABLE [dbo].[EC_AUT_QC_Expense]
--(
--    [Id] int not null primary key identity(1,1),
--	[InspectionId] [int],	
--	[QcId] [int],	
--	[StartPort] [int],	
--	[FactoryTown] [int],	
--	[TripType] [int],	
--	[ServiceDate] [DateTime],	
--	[TravelTariff] [float],	
--	[TravelTariffCurrency] [int],	
--	[FoodAllowance] [float],	
--	[FoodAllowanceCurrency] [int],	
--	[EntityId] [int],
--	[Active] [bit] NULL,
--	[IsExpenseCreated] [bit] NULL,
--	[IsTravelAllowanceConfigured] [bit] NULL,
--	[IsFoodAllowanceConfigured] [bit] NULL,
--	[Comments] nvarchar(2500),
--	[CreatedBy] [int] NULL,
--	[CreatedOn] [datetime] NULL,
--	[UpdatedBy] [int] NULL,
--	[UpdatedOn] [datetime] NULL,
--	[DeletedBy] [int] NULL,
--	[DeletedOn] [datetime] NULL,	
	
--	CONSTRAINT [FK_EC_AUT_QC_Expense_Created_By] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),

--	CONSTRAINT [FK_EC_AUT_QC_Expense_Deleted_By] FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),

--	CONSTRAINT [FK_EC_AUT_QC_Expense_InspectionId] FOREIGN KEY([InspectionId]) REFERENCES [dbo].[Insp_Transaction] ([Id]),

--	CONSTRAINT [FK_EC_AUT_QC_Expense_QcId] FOREIGN KEY([QcId]) REFERENCES [dbo].[Hr_Staff] ([Id]),

--	CONSTRAINT [FK_EC_AUT_QC_Expense_StartPort] FOREIGN KEY([StartPort]) REFERENCES [dbo].[EC_AUT_REF_StartPort] ([Id]),

--	CONSTRAINT [FK_EC_AUT_QC_Expense_FactoryTown] FOREIGN KEY([FactoryTown]) REFERENCES [dbo].[Ref_Town] ([Id]),

--	CONSTRAINT [FK_EC_AUT_QC_Expense_TripType] FOREIGN KEY([TripType]) REFERENCES [dbo].[EC_AUT_REF_TripType] ([Id]),

--	CONSTRAINT [FK_EC_AUT_QC_Expense_TravelTariffCurrency] FOREIGN KEY([TravelTariffCurrency]) REFERENCES [dbo].[Ref_Currency] ([Id]),

--	CONSTRAINT [FK_EC_AUT_QC_Expense_FoodAllowanceCurrency] FOREIGN KEY([FoodAllowanceCurrency]) REFERENCES [dbo].[Ref_Currency] ([Id])
--)

insert into IT_Role (RoleName,Active,EntityId,PrimaryRole,SecondaryRole) values ('Auto QC Expense-Accounting',1,1,1,1)

INSERT INTO MID_TaskType(Id,Label,EntityId) Values (24,'UpdateCustomerToTCF',1)

INSERT INTO MID_TaskType(Id,Label,EntityId) Values (25,'TravelTariffUpdate',1)



ALTER TABLE [dbo].[EC_ExpensesClaimDetais] ADD [TripType] INT NULL

ALTER TABLE [dbo].[EC_ExpensesClaimDetais] ADD CONSTRAINT EC_ExpensesClaimDetais_TripType  FOREIGN KEY(TripType) REFERENCES [dbo].[EC_AUT_REF_TripType](Id)

--ALTER TABLE [dbo].[EC_ExpensesClaimDetais] ADD [Qc_Auto_ExpenseId] INT NULL

--ALTER TABLE [dbo].[EC_ExpensesClaimDetais] ADD CONSTRAINT EC_ExpensesClaimDetais_Qc_Auto_ExpenseId  FOREIGN KEY(Qc_Auto_ExpenseId) REFERENCES [dbo].[EC_AUT_QC_Expense](Id)



CREATE TABLE [dbo].[EC_AUT_QC_FoodExpense]
(
    [Id] int not null primary key identity(1,1),
	[QcId] [int],	
	[InspectionId] [int],		
	[FactoryCountry] [int],		
	[ServiceDate] [DateTime],	

	[FoodAllowance] [float],	
	[FoodAllowanceCurrency] [int],	

	
	[Active] [bit] NULL,
	[EntityId] [int],

	[IsExpenseCreated] [bit] NULL,
	[IsFoodAllowanceConfigured] [bit] NULL,

	[Comments] nvarchar(2500),

	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,	
	
	CONSTRAINT [FK_EC_AUT_QC_FoodExpense_Created_By] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_FoodExpense_Deleted_By] FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_FoodExpense_UpdatedBy] FOREIGN KEY([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_FoodExpense_InspectionId] FOREIGN KEY([InspectionId]) REFERENCES [dbo].[Insp_Transaction] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_FoodExpense_QcId] FOREIGN KEY([QcId]) REFERENCES [dbo].[Hr_Staff] ([Id]),	

	CONSTRAINT [FK_EC_AUT_QC_FoodExpense_FactoryCountry] FOREIGN KEY([FactoryCountry]) REFERENCES [dbo].[Ref_Country] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_FoodExpense_FoodAllowanceCurrency] FOREIGN KEY([FoodAllowanceCurrency]) REFERENCES [dbo].[Ref_Currency] ([Id])
)


CREATE TABLE [dbo].[EC_AUT_QC_TravelExpense]
(
    [Id] int not null primary key identity(1,1),
	[QcId] [int],		
	[InspectionId] [int],	
	[ServiceDate] [DateTime],	
	
	[StartPort] [int],	
	[FactoryTown] [int],	
	[TripType] [int],	
	[TravelTariff] [float],	
	[TravelTariffCurrency] [int],

	[Active] [bit] NULL,
	[EntityId] [int],
	
	[IsExpenseCreated] [bit] NULL,
	[IsTravelAllowanceConfigured] [bit] NULL,

	[Comments] nvarchar(2500),

	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,	
	
	CONSTRAINT [FK_EC_AUT_QC_TravelExpense_Created_By] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_TravelExpense_Deleted_By] FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_TravelExpense_UpdatedBy] FOREIGN KEY([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_TravelExpense_InspectionId] FOREIGN KEY([InspectionId]) REFERENCES [dbo].[Insp_Transaction] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_TravelExpense_QcId] FOREIGN KEY([QcId]) REFERENCES [dbo].[Hr_Staff] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_TravelExpense_StartPort] FOREIGN KEY([StartPort]) REFERENCES [dbo].[EC_AUT_REF_StartPort] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_TravelExpense_FactoryTown] FOREIGN KEY([FactoryTown]) REFERENCES [dbo].[Ref_Town] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_TravelExpense_TripType] FOREIGN KEY([TripType]) REFERENCES [dbo].[EC_AUT_REF_TripType] ([Id]),

	CONSTRAINT [FK_EC_AUT_QC_TravelExpense_TravelTariffCurrency] FOREIGN KEY([TravelTariffCurrency]) REFERENCES [dbo].[Ref_Currency] ([Id])	
)




ALTER TABLE [dbo].[EC_ExpensesClaimDetais] ADD [Qc_Travel_ExpenseId] INT NULL
ALTER TABLE [dbo].[EC_ExpensesClaimDetais] ADD CONSTRAINT EC_ExpensesClaimDetais_Qc_Travel_ExpenseId  FOREIGN KEY(Qc_Travel_ExpenseId) REFERENCES [dbo].[EC_AUT_QC_TravelExpense](Id)

ALTER TABLE [dbo].[EC_ExpensesClaimDetais] ADD [Qc_Food_ExpenseId] INT NULL
ALTER TABLE [dbo].[EC_ExpensesClaimDetais] ADD CONSTRAINT EC_ExpensesClaimDetais_Qc_Food_ExpenseId  FOREIGN KEY(Qc_Food_ExpenseId) REFERENCES [dbo].[EC_AUT_QC_FoodExpense](Id)

ALTER TABLE [dbo].[EC_ExpensesClaimDetais] ADD [IsAutoExpense] BIT NULL

ALTER TABLE [dbo].[EC_ExpensesClaimDetais] ADD [IsManagerApproved ] BIT NULL

-- QC Auto expense end --

----- Product sub category 3, workload matrix starts ----

CREATE TABLE [dbo].[REF_ProductCategory_Sub3]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(1000) NOT NULL, 
    [ProductSubCategory2Id] INT NOT NULL, 
    [Active] BIT NOT NULL, 
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
    [EntityId] INT NULL,
    FOREIGN KEY([ProductSubCategory2Id]) REFERENCES [REF_ProductCategory_Sub2](Id),
    FOREIGN KEY([CreatedBy]) REFERENCES [IT_UserMaster](Id),
    FOREIGN KEY([UpdatedBy]) REFERENCES [IT_UserMaster](Id),
    FOREIGN KEY([DeletedBy]) REFERENCES [IT_UserMaster](Id),
    FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
)

CREATE TABLE [dbo].[QU_WorkLoadMatrix]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [ProductSubCategory3Id] INT NULL, 
    [PreparationTime] FLOAT NULL, 
    [SampleSize_8h] INT NULL, 
    [Active] BIT NULL,
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
    [EntityId] INT NULL,
    FOREIGN KEY([ProductSubCategory3Id]) REFERENCES [REF_ProductCategory_Sub3](Id),
    FOREIGN KEY([CreatedBy]) REFERENCES [IT_UserMaster](Id),
    FOREIGN KEY([UpdatedBy]) REFERENCES [IT_UserMaster](Id),
    FOREIGN KEY([DeletedBy]) REFERENCES [IT_UserMaster](Id),
    FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
)

----- Product sub category 3, workload matrix end ----

-------- Map Work Load Data to Cu Product start --------------------------
ALTER TABLE CU_Products ADD ProductCategorySub3 INT NULL
ALTER TABLE CU_Products ADD CONSTRAINT FK_CuProducts_ProductCategorySub3Id FOREIGN KEY (ProductCategorySub3) REFERENCES REF_ProductCategory_Sub3(Id);

ALTER TABLE CU_Products ADD SampleSize_8h INT NULL
ALTER TABLE CU_Products ADD TimePreparation FLOAT NULL
ALTER TABLE CU_Products ADD Tp_AdjustmentReason NVARCHAR(1000) NULL
ALTER TABLE CU_Products ADD Unit INT NULL
ALTER TABLE CU_Products ADD TechnicalComments NVARCHAR(1000) NULL
-------- Map Work Load Data to Cu Product end --------------------------

------------ KeepQCForTravelExpense for cancel booking start----------
ALTER TABLE  SCH_Schedule_QC ADD KeepQCForTravelExpense BIT
------------ KeepQCForTravelExpense for cancel booking end----------

--------------Qu_quotation_Insp 2 new columns start --------------

ALTER TABLE QU_Quotation_Insp ADD CalculatedWorkingHrs FLOAT NULL 

ALTER TABLE QU_Quotation_Insp ADD CalculatedWorkingManDay FLOAT NULL 

--------------Qu_quotation_Insp 2 new columns end --------------

--------------- Other Man day tables start -------------------------

CREATE TABLE [dbo].[OM_REF_Purpose]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] [nvarchar](200) NOT NULL, 
    [Active] BIT NOT NULL, 
    [Sort] INT NULL
)

CREATE TABLE [dbo].[OM_Details]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [CustomerId] INT NULL, 
    [OfficeCountryId] INT NULL, 
    [QcId] INT NULL, 
    [OperationalCountryId] INT NULL, 
    [PurposeId] INT NULL, 
    [ServiceDate] DATETIME NULL, 
    [ManDay] FLOAT NULL, 
    [Remarks] NVARCHAR(1000) NULL, 	
    [Active] BIT,
    [CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL,
    [EntityId] INT NULL, 
    FOREIGN KEY (CustomerId) REFERENCES CU_Customer(Id),
    FOREIGN KEY (OfficeCountryId) REFERENCES REF_Country(Id),
    FOREIGN KEY (QcId) REFERENCES HR_Staff(Id),
    FOREIGN KEY (OperationalCountryId) REFERENCES REF_Country(Id),
    FOREIGN KEY (PurposeId) REFERENCES [OM_REF_Purpose](Id),
    FOREIGN KEY([CreatedBy]) REFERENCES [IT_UserMaster](Id),
    FOREIGN KEY([UpdatedBy]) REFERENCES [IT_UserMaster](Id),
    FOREIGN KEY([DeletedBy]) REFERENCES [IT_UserMaster](Id),
    FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
)

ALTER TABLE CU_Contact ADD ReportTo INT NULL
ALTER TABLE CU_Contact ADD CONSTRAINT FK_CU_Contact_CU_Contact FOREIGN KEY (ReportTo) REFERENCES CU_Contact(Id);

--------------- Other Man day tables end -------------------------

----------------Update Product Category Starts---------------------------

Update inspection Set 
inspection.ProductCategoryId=inspectionProduct.ProductCategory,
inspection.ProductSubCategoryId=inspectionProduct.ProductSubCategory,
inspection.ProductSubCategory2Id=inspectionProduct.ProductCategorySub2
from insp_transaction inspection join
        (
        Select insp.Id inspectionId,inspProduct.Id ProductRefId,inspProduct.Product_Id ProductId,
		Product.ProductCategory,Product.ProductSubCategory,Product.ProductCategorySub2
            ,ROW_NUMBER() OVER(PARTITION BY insp.Id ORDER BY inspProduct.Id) AS RN
        From INSP_Product_Transaction inspProduct
		inner join CU_Products Product
		on inspProduct.Product_Id=Product.Id
        inner join INSP_Transaction insp on insp.Id=inspProduct.Inspection_Id
		and inspProduct.Active=1
        ) inspectionProduct on inspection.Id=inspectionProduct.inspectionId
WHERE inspectionProduct.RN = 1 

----------------Update Product Category Ends---------------------------

----------------Price Card Enhancement Starts--------------------------

CREATE TABLE [dbo].[CU_PR_InspectionLocation]
(
    [Id] int not null primary key identity(1,1),
	[Cu_Price_Id] [int],	
	[InspectionLocationId] [int],		
	[Active] [bit] NULL,	
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,	
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,	
	CONSTRAINT [FK_CU_PR_InspectionLocation_Cu_Price_Id] FOREIGN KEY([Cu_Price_Id]) REFERENCES [dbo].[CU_PR_Details] ([Id]),
	CONSTRAINT [FK_CU_PR_InspectionLocation_InspectionLocationId] FOREIGN KEY([InspectionLocationId]) REFERENCES [dbo].[INSP_REF_InspectionLocation] ([Id]),	
	CONSTRAINT [FK_CU_PR_InspectionLocation_Created_By] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_CU_PR_InspectionLocation_Deleted_By] FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id])	
)

CREATE TABLE [dbo].[INSP_REF_QuantityType]
(
	[Id] int identity(1,1) primary key,
	[Name] NVARCHAR(50),
	[Active] BIT,
	[Sort] int
)

INSERT INTO INSP_REF_QuantityType(Name,Active,Sort) Values ('Ordered Qty',1,1)
INSERT INTO INSP_REF_QuantityType(Name,Active,Sort) Values ('Inspected Qty',1,2)
INSERT INTO INSP_REF_QuantityType(Name,Active,Sort) Values ('Presented Qty',1,3)

CREATE TABLE [dbo].[INV_REF_BillingFreequency]
(
	[Id] int identity(1,1) primary key,
	[Name] NVARCHAR(50),
	[Active] BIT,
	[Sort] int
)

INSERT INTO INV_REF_BillingFreequency(Name,Active,Sort) Values ('Daily',1,1)
INSERT INTO INV_REF_BillingFreequency(Name,Active,Sort) Values ('Weekly',1,2)
INSERT INTO INV_REF_BillingFreequency(Name,Active,Sort) Values ('Monthly',1,3)


CREATE TABLE [dbo].[INV_REF_InterventionType]
(
	[Id] int identity(1,1) primary key,
	[Name] NVARCHAR(50),
	[Active] BIT,
	[Sort] int
)

INSERT INTO INV_REF_InterventionType(Name,Active,Sort) Values ('Range',1,1)
INSERT INTO INV_REF_InterventionType(Name,Active,Sort) Values ('Per Style',1,2)

CREATE TABLE [dbo].[CU_PR_TRAN_Subcategory]
   (
      [Id] INT IDENTITY(1,1) PRIMARY KEY,	  
	  [Cu_Price_Id] [int],	
	  [Sub_CategoryId] [int],	
	  [MandayProductivity] [int],	
	  [MandayReports] [int],	
	  [MandayBuffer] [float],	
	  [UnitPrice] [float],	
	  [AQL_QTY_8] [int],		   
	  [AQL_QTY_13] [int],	   
	  [AQL_QTY_20] [int],	   
	  [AQL_QTY_32] [int],
	  [AQL_QTY_50] [int],
	  [AQL_QTY_80] [int],
	  [AQL_QTY_125] [int],
	  [AQL_QTY_200] [int],	
	  [AQL_QTY_315] [int],	
	  [AQL_QTY_500] [int],	
	  [AQL_QTY_800] [int],	
	  [AQL_QTY_1250] [int],
	  [Active] BIT,
	  [CreatedBy] INT NULL, 
      [CreatedOn] DATETIME NULL,     
	  [DeletedBy] [INT] NULL, 
      [DeletedOn] [DATETIME] NULL,
	  CONSTRAINT CU_PR_TRAN_Subcategory_Cu_Price_Id FOREIGN KEY ([Cu_Price_Id]) REFERENCES [dbo].[CU_PR_Details](Id),
	  CONSTRAINT CU_PR_TRAN_Subcategory_Sub_CategoryId FOREIGN KEY ([Sub_CategoryId]) REFERENCES [dbo].[REF_ProductCategory_Sub](Id),
	  CONSTRAINT CU_PR_TRAN_Subcategory_CreatedBy	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	  CONSTRAINT CU_PR_TRAN_Subcategory_DeletedBy	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
 )


 CREATE TABLE [dbo].[CU_PR_TRAN_SpecialRule]
   (
      [Id] INT IDENTITY(1,1) PRIMARY KEY,	  
	  [Cu_Price_Id] [int],	
	  [MandayProductivity] [int],	
	  [MandayReports] [int],	
	  [UnitPrice] [Float],	
	  [PieceRate_Billing_Q_Start] [int],		   
	  [Piecerate_Billing_Q_End] [int],	   
	  [AdditionalFee] [Float],	   
	  [Piecerate_MinBilling] [Float],
	  [PerInterventionRange1] [int],
	  [PerInterventionRange2] [int],
	  [Max_Style_Per_Day] [Float],
	  [Max_Style_Per_Week] [Float],	
	  [Max_Style_Per_Month] [Float],	
	  [InterventionFee] [Float],
	  [Active] BIT,
	  [CreatedBy] [INT] NULL, 
      [CreatedOn] [DATETIME] NULL,     
	  [DeletedBy] [INT] NULL, 
      [DeletedOn] [DATETIME] NULL,
	  CONSTRAINT CU_PR_TRAN_SpecialRule_Cu_Price_Id FOREIGN KEY ([Cu_Price_Id]) REFERENCES [dbo].[CU_PR_Details](Id),
	  CONSTRAINT CU_PR_TRAN_SpecialRule_CreatedBy	FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	  CONSTRAINT CU_PR_TRAN_SpecialRule_DeletedBy	FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
 )




ALTER TABLE CU_PR_Details ADD BilledQuantityType INT NULL
CONSTRAINT CU_PR_Details_BilledQuantityType FOREIGN KEY ([BilledQuantityType]) REFERENCES [dbo].[INSP_REF_QuantityType](Id)

ALTER TABLE CU_PR_Details ADD MaxFeeStyle Float NULL
ALTER TABLE CU_PR_Details ADD InvoiceSubject NVARCHAR(1000) NULL

ALTER TABLE CU_PR_Details ADD BillingFreequency INT NULL
CONSTRAINT CU_PR_Details_BillingFreequency FOREIGN KEY ([BillingFreequency]) REFERENCES [dbo].[INV_REF_BillingFreequency](Id)

ALTER TABLE CU_PR_Details ADD ManDay_Productivity Float NULL

ALTER TABLE CU_PR_Details ADD Manday_ReportCount int NULL

ALTER TABLE CU_PR_Details ADD Manday_Buffer Float NULL

ALTER TABLE CU_PR_Details ADD SubCategorySelectAll Bit NULL

ALTER TABLE CU_PR_Details ADD IsSpecial Bit NULL

ALTER TABLE CU_PR_Details ADD InterventionType INT NULL
CONSTRAINT CU_PR_Details_InterventionType FOREIGN KEY ([InterventionType]) REFERENCES [dbo].[INV_REF_InterventionType](Id)

INSERT INTO [dbo].[INV_REF_Request_Type] ([Name], [Active]) VALUES ('Product Category', 1)

ALTER TABLE INV_TRAN_Invoice_Request ADD ProductCategoryId INT NULL
CONSTRAINT INV_TRAN_Invoice_Request_ProductCategoryId FOREIGN KEY ([ProductCategoryId]) REFERENCES [dbo].[CU_ProductCategory](Id)

----------------Price Card Enhancement Ends--------------------------

ALTER  TABLE [FB_Report_Details] ADD [OrderQty] INT NULL

ALTER  TABLE [FB_Report_Details] ADD [InspectedQty] INT NULL

ALTER  TABLE [FB_Report_Details] ADD [PresentedQty] INT NULL

----------------------Travel Matrix Changes-------------------------------

ALTER TABLE [dbo].[INV_TM_Details] ALTER COLUMN CityId INT NULL

ALTER TABLE [dbo].[INV_TM_Details] ALTER COLUMN [CountyId] INT NULL

ALTER TABLE [dbo].[INV_TM_Details] ALTER COLUMN [InspPortCountyId] INT NULL

ALTER TABLE [dbo].[INV_TM_Details] ADD [InspPortCityId] INT NULL

ALTER TABLE [dbo].[INV_TM_Details] ADD CONSTRAINT FK_TRAVEL_MATRIX_PORT_CITY_ID FOREIGN KEY ([InspPortCityId]) REFERENCES [dbo].[REF_CITY]

----------------------Travel Matrix Changes Ends-------------------------------



ALTER TABLE INSP_PurchaseOrder_Color_Transaction ADD PickingQuantity INT

ALTER TABLE INSP_PurchaseOrder_Color_Transaction ADD BookingQuantity INT
----------------------Travel Matrix Changes Ends-------------------------------


INSERT INTO [dbo].[ENT_REF_Features] (Name,Active) VALUES('Billing method manday complex',1)

INSERT INTO [dbo].[ENT_REF_Features] (Name,Active) VALUES('Billing method sampling complex',1)

INSERT INTO [dbo].[ENT_REF_Features] (Name,Active) VALUES('Billing method piecerate complex',1)

INSERT INTO [dbo].[ENT_REF_Features] (Name,Active) VALUES('Billing method per intervention complex',1)


INSERT INTO [dbo].[ENT_Feature_Details] (FeatureId,EntityId,Active) VALUES(2,2,1)
INSERT INTO [dbo].[ENT_Feature_Details] (FeatureId,EntityId,Active) VALUES(3,2,1)
INSERT INTO [dbo].[ENT_Feature_Details] (FeatureId,EntityId,Active) VALUES(4,2,1)
INSERT INTO [dbo].[ENT_Feature_Details] (FeatureId,EntityId,Active) VALUES(5,2,1)
----------------------Travel Matrix Changes Ends-------------------------------



ALTER TABLE INSP_PurchaseOrder_Color_Transaction ADD PickingQuantity INT

ALTER TABLE INSP_PurchaseOrder_Color_Transaction ADD BookingQuantity INT

---------------------- DP Point Enhancement Start-------------------------------
CREATE TABLE [dbo].[INSP_REF_DP_Point]
(
	[Id] int IDENTITY(1,1) PRIMARY KEY,
	[Name] varchar(250),
	[Active] BIT,
	[Sort] int
 )

INSERT INTO INSP_REF_DP_Point([Name],[Active],[Sort])values('4 points',1,1),('5 points',1,2)

ALTER TABLE [dbo].[CU_ServiceType] ADD [DP_Point] INT NULL

ALTER TABLE [dbo].[CU_ServiceType] ADD  CONSTRAINT [FK_CU_ServiceType_INSP_REF_DP_Point] FOREIGN KEY([DP_Point]) REFERENCES [dbo].[INSP_REF_DP_Point] ([Id])
---------------------- DP Point Enhancement End -------------------------------
GO
/****** Object:  Table [dbo].[CLM_REF_CustomerRequest]    Script Date: 15/03/2022 23:15:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLM_REF_CustomerRequest](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Active] [bit] NOT NULL,
	[Sort] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CLM_REF_DefectDistribution]    Script Date: 15/03/2022 23:15:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLM_REF_DefectDistribution](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Active] [bit] NOT NULL,
	[Sort] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CLM_REF_DefectFamily]    Script Date: 15/03/2022 23:15:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLM_REF_DefectFamily](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Active] [bit] NOT NULL,
	[Sort] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CLM_REF_Department]    Script Date: 15/03/2022 23:15:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLM_REF_Department](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Active] [bit] NOT NULL,
	[Sort] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CLM_REF_FileType]    Script Date: 15/03/2022 23:15:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLM_REF_FileType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Active] [bit] NOT NULL,
	[Sort] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CLM_REF_FROM]    Script Date: 15/03/2022 23:15:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLM_REF_FROM](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Active] [bit] NOT NULL,
	[Sort] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CLM_REF_Priority]    Script Date: 15/03/2022 23:15:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLM_REF_Priority](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Active] [bit] NOT NULL,
	[Sort] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CLM_REF_ReceivedFrom]    Script Date: 15/03/2022 23:15:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLM_REF_ReceivedFrom](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Active] [bit] NOT NULL,
	[Sort] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CLM_REF_RefundType]    Script Date: 15/03/2022 23:15:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLM_REF_RefundType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Active] [bit] NOT NULL,
	[Sort] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CLM_REF_Result]    Script Date: 15/03/2022 23:15:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLM_REF_Result](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Active] [bit] NOT NULL,
	[Sort] [int] NULL,
	[IsValidate] [bit] NULL,
	[IsFinal] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CLM_REF_Source]    Script Date: 15/03/2022 23:15:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLM_REF_Source](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Active] [bit] NOT NULL,
	[Sort] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CLM_REF_Status]    Script Date: 15/03/2022 23:15:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLM_REF_Status](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Active] [bit] NOT NULL,
	[Sort] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CLM_TRAN_Attachments]    Script Date: 15/03/2022 23:15:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLM_TRAN_Attachments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UniqueId] [nvarchar](2000) NULL,
	[ClaimId] [int] NULL,
	[FileType] [int] NULL,
	[FileName] [nvarchar](1000) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[FileUrl] [nvarchar](max) NULL,
	[EntityId] [int] NULL,
	[Active] [bit] NULL,
	[FileDesc] [nvarchar](max) NULL,
 CONSTRAINT [PK__CLM_TRAN__3214EC07C874F54C] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CLM_TRAN_ClaimRefund]    Script Date: 15/03/2022 23:15:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLM_TRAN_ClaimRefund](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Claimid] [int] NULL,
	[RefundTypeId] [int] NULL,
	[Active] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
 CONSTRAINT [PK_CLM_TRAN_ClaimRefund] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CLM_TRAN_CustomerRequest]    Script Date: 15/03/2022 23:15:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLM_TRAN_CustomerRequest](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Claimid] [int] NULL,
	[CustomerRequestId] [int] NULL,
	[Active] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CLM_TRAN_CustomerRequestRefund]    Script Date: 15/03/2022 23:15:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLM_TRAN_CustomerRequestRefund](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Claimid] [int] NULL,
	[RefundTypeId] [int] NULL,
	[Active] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CLM_TRAN_DefectFamily]    Script Date: 15/03/2022 23:15:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLM_TRAN_DefectFamily](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimId] [int] NULL,
	[DefectFamilyId] [int] NULL,
	[Active] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
 CONSTRAINT [PK__CLM_TRAN__3214EC07361F3FE1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CLM_TRAN_Department]    Script Date: 15/03/2022 23:15:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLM_TRAN_Department](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Claimid] [int] NULL,
	[DepartmentId] [int] NULL,
	[Active] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CLM_TRAN_FinalDecision]    Script Date: 15/03/2022 23:15:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLM_TRAN_FinalDecision](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Claimid] [int] NULL,
	[FinalDecision] [int] NULL,
	[Active] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CLM_TRAN_Reports]    Script Date: 15/03/2022 23:15:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLM_TRAN_Reports](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimId] [int] NULL,
	[ReportId] [int] NULL,
	[Active] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
 CONSTRAINT [PK__CLM_TRAN__3214EC07F9AAAD1B] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CLM_Transaction]    Script Date: 15/03/2022 23:15:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CLM_Transaction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimNo] [nvarchar](50) NULL,
	[InspectionNo] [int] NULL,
	[StatusId] [int] NULL,
	[ClaimDate] [datetime] NULL,
	[RequestedContactName] [nvarchar](100) NULL,
	[ClaimForm] [int] NULL,
	[ReceivedFrom] [int] NULL,
	[ClaimSource] [int] NULL,
	[ClaimDescription] [nvarchar](max) NULL,
	[CustomerPriority] [int] NULL,
	[CustomerReqRefundAmount] [float] NULL,
	[CustomerReqRefundCurrency] [int] NULL,
	[CustomerComments] [nvarchar](max) NULL,
	[QCControl_100Goods] [bit] NULL,
	[DefectPercentage] [float] NULL,
	[NoOfPieces] [int] NULL,
	[CompareToAQL] [float] NULL,
	[DefectDistribution] [int] NULL,
	[Color] [nvarchar](1000) NULL,
	[DefectCartonInspected] [nvarchar](500) NULL,
	[FOB_Price] [float] NULL,
	[FOB_Currency] [int] NULL,
	[Retail_Price] [float] NULL,
	[Retail_Currency] [int] NULL,
	[ClaimValidateResult] [int] NULL,
	[ClaimRemarks] [nvarchar](2000) NULL,
	[ClaimRecommendation] [nvarchar](2000) NULL,
	[ClaimRefundAmount] [float] NULL,
	[ClaimRefundCurrency] [int] NULL,
	[ClaimRefundRemarks] [nvarchar](2000) NULL,
	[RealInspectionFees] [float] NULL,
	[RealInspectionFeesCurrency] [int] NULL,
	[AnalyzerFeedback] [varchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[EntityId] [int] NULL,
	[AnalyzedOn] [datetime] NULL,
	[AnalyzedBy] [int] NULL,
	[ValidatedOn] [datetime] NULL,
	[ValidatedBy] [int] NULL,
	[ClosedOn] [datetime] NULL,
	[ClosedBy] [int] NULL,
 CONSTRAINT [PK__CLM_Tran__3214EC0710E028DB] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[CLM_TRAN_Attachments]  WITH CHECK ADD  CONSTRAINT [FK__CLM_TRAN___Claim__25A9BF2E] FOREIGN KEY([ClaimId])
REFERENCES [dbo].[CLM_Transaction] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_Attachments] CHECK CONSTRAINT [FK__CLM_TRAN___Claim__25A9BF2E]
GO
ALTER TABLE [dbo].[CLM_TRAN_Attachments]  WITH CHECK ADD  CONSTRAINT [FK__CLM_TRAN___Creat__22CD5283] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_Attachments] CHECK CONSTRAINT [FK__CLM_TRAN___Creat__22CD5283]
GO
ALTER TABLE [dbo].[CLM_TRAN_Attachments]  WITH CHECK ADD  CONSTRAINT [FK__CLM_TRAN___Delet__24B59AF5] FOREIGN KEY([DeletedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_Attachments] CHECK CONSTRAINT [FK__CLM_TRAN___Delet__24B59AF5]
GO
ALTER TABLE [dbo].[CLM_TRAN_Attachments]  WITH CHECK ADD  CONSTRAINT [FK__CLM_TRAN___Entit__279207A0] FOREIGN KEY([EntityId])
REFERENCES [dbo].[AP_Entity] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_Attachments] CHECK CONSTRAINT [FK__CLM_TRAN___Entit__279207A0]
GO
ALTER TABLE [dbo].[CLM_TRAN_Attachments]  WITH CHECK ADD  CONSTRAINT [FK__CLM_TRAN___FileT__269DE367] FOREIGN KEY([FileType])
REFERENCES [dbo].[CLM_REF_FileType] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_Attachments] CHECK CONSTRAINT [FK__CLM_TRAN___FileT__269DE367]
GO
ALTER TABLE [dbo].[CLM_TRAN_Attachments]  WITH CHECK ADD  CONSTRAINT [FK__CLM_TRAN___Updat__23C176BC] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_Attachments] CHECK CONSTRAINT [FK__CLM_TRAN___Updat__23C176BC]
GO
ALTER TABLE [dbo].[CLM_TRAN_ClaimRefund]  WITH CHECK ADD  CONSTRAINT [FK_CLM_TRAN_ClaimRefund_CLM_REF_RefundType] FOREIGN KEY([RefundTypeId])
REFERENCES [dbo].[CLM_REF_RefundType] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_ClaimRefund] CHECK CONSTRAINT [FK_CLM_TRAN_ClaimRefund_CLM_REF_RefundType]
GO
ALTER TABLE [dbo].[CLM_TRAN_ClaimRefund]  WITH CHECK ADD  CONSTRAINT [FK_CLM_TRAN_ClaimRefund_CLM_Transaction] FOREIGN KEY([Claimid])
REFERENCES [dbo].[CLM_Transaction] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_ClaimRefund] CHECK CONSTRAINT [FK_CLM_TRAN_ClaimRefund_CLM_Transaction]
GO
ALTER TABLE [dbo].[CLM_TRAN_ClaimRefund]  WITH CHECK ADD  CONSTRAINT [FK_CLM_TRAN_ClaimRefund_IT_UserMaster] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_ClaimRefund] CHECK CONSTRAINT [FK_CLM_TRAN_ClaimRefund_IT_UserMaster]
GO
ALTER TABLE [dbo].[CLM_TRAN_ClaimRefund]  WITH CHECK ADD  CONSTRAINT [FK_CLM_TRAN_ClaimRefund_IT_UserMaster1] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_ClaimRefund] CHECK CONSTRAINT [FK_CLM_TRAN_ClaimRefund_IT_UserMaster1]
GO
ALTER TABLE [dbo].[CLM_TRAN_ClaimRefund]  WITH CHECK ADD  CONSTRAINT [FK_CLM_TRAN_ClaimRefund_IT_UserMaster2] FOREIGN KEY([DeletedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_ClaimRefund] CHECK CONSTRAINT [FK_CLM_TRAN_ClaimRefund_IT_UserMaster2]
GO
ALTER TABLE [dbo].[CLM_TRAN_CustomerRequest]  WITH CHECK ADD  CONSTRAINT [FK__CLM_TRAN___Claim__0CDE1164] FOREIGN KEY([Claimid])
REFERENCES [dbo].[CLM_Transaction] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_CustomerRequest] CHECK CONSTRAINT [FK__CLM_TRAN___Claim__0CDE1164]
GO
ALTER TABLE [dbo].[CLM_TRAN_CustomerRequest]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_CustomerRequest]  WITH CHECK ADD FOREIGN KEY([CustomerRequestId])
REFERENCES [dbo].[CLM_REF_CustomerRequest] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_CustomerRequest]  WITH CHECK ADD FOREIGN KEY([DeletedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_CustomerRequest]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_CustomerRequestRefund]  WITH CHECK ADD  CONSTRAINT [FK__CLM_TRAN___Claim__138B0EF3] FOREIGN KEY([Claimid])
REFERENCES [dbo].[CLM_Transaction] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_CustomerRequestRefund] CHECK CONSTRAINT [FK__CLM_TRAN___Claim__138B0EF3]
GO
ALTER TABLE [dbo].[CLM_TRAN_CustomerRequestRefund]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_CustomerRequestRefund]  WITH CHECK ADD FOREIGN KEY([DeletedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_CustomerRequestRefund]  WITH CHECK ADD FOREIGN KEY([RefundTypeId])
REFERENCES [dbo].[CLM_REF_RefundType] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_CustomerRequestRefund]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_DefectFamily]  WITH CHECK ADD  CONSTRAINT [FK__CLM_TRAN___Claim__78D718B7] FOREIGN KEY([ClaimId])
REFERENCES [dbo].[CLM_Transaction] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_DefectFamily] CHECK CONSTRAINT [FK__CLM_TRAN___Claim__78D718B7]
GO
ALTER TABLE [dbo].[CLM_TRAN_DefectFamily]  WITH CHECK ADD  CONSTRAINT [FK__CLM_TRAN___Creat__7ABF6129] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_DefectFamily] CHECK CONSTRAINT [FK__CLM_TRAN___Creat__7ABF6129]
GO
ALTER TABLE [dbo].[CLM_TRAN_DefectFamily]  WITH CHECK ADD  CONSTRAINT [FK__CLM_TRAN___Defec__79CB3CF0] FOREIGN KEY([DefectFamilyId])
REFERENCES [dbo].[CLM_REF_DefectFamily] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_DefectFamily] CHECK CONSTRAINT [FK__CLM_TRAN___Defec__79CB3CF0]
GO
ALTER TABLE [dbo].[CLM_TRAN_DefectFamily]  WITH CHECK ADD  CONSTRAINT [FK__CLM_TRAN___Delet__7CA7A99B] FOREIGN KEY([DeletedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_DefectFamily] CHECK CONSTRAINT [FK__CLM_TRAN___Delet__7CA7A99B]
GO
ALTER TABLE [dbo].[CLM_TRAN_DefectFamily]  WITH CHECK ADD  CONSTRAINT [FK__CLM_TRAN___Updat__7BB38562] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_DefectFamily] CHECK CONSTRAINT [FK__CLM_TRAN___Updat__7BB38562]
GO
ALTER TABLE [dbo].[CLM_TRAN_Department]  WITH CHECK ADD  CONSTRAINT [FK__CLM_TRAN___Claim__063113D5] FOREIGN KEY([Claimid])
REFERENCES [dbo].[CLM_Transaction] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_Department] CHECK CONSTRAINT [FK__CLM_TRAN___Claim__063113D5]
GO
ALTER TABLE [dbo].[CLM_TRAN_Department]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_Department]  WITH CHECK ADD FOREIGN KEY([DeletedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_Department]  WITH CHECK ADD FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[CLM_REF_Department] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_Department]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_FinalDecision]  WITH CHECK ADD  CONSTRAINT [FK__CLM_TRAN___Claim__1C2054F4] FOREIGN KEY([Claimid])
REFERENCES [dbo].[CLM_Transaction] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_FinalDecision] CHECK CONSTRAINT [FK__CLM_TRAN___Claim__1C2054F4]
GO
ALTER TABLE [dbo].[CLM_TRAN_FinalDecision]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_FinalDecision]  WITH CHECK ADD FOREIGN KEY([DeletedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_FinalDecision]  WITH CHECK ADD FOREIGN KEY([FinalDecision])
REFERENCES [dbo].[CLM_REF_Result] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_FinalDecision]  WITH CHECK ADD FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_Reports]  WITH CHECK ADD  CONSTRAINT [FK__CLM_TRAN___Claim__722A1B28] FOREIGN KEY([ClaimId])
REFERENCES [dbo].[CLM_Transaction] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_Reports] CHECK CONSTRAINT [FK__CLM_TRAN___Claim__722A1B28]
GO
ALTER TABLE [dbo].[CLM_TRAN_Reports]  WITH CHECK ADD  CONSTRAINT [FK__CLM_TRAN___Creat__7412639A] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_Reports] CHECK CONSTRAINT [FK__CLM_TRAN___Creat__7412639A]
GO
ALTER TABLE [dbo].[CLM_TRAN_Reports]  WITH CHECK ADD  CONSTRAINT [FK__CLM_TRAN___Delet__75FAAC0C] FOREIGN KEY([DeletedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_Reports] CHECK CONSTRAINT [FK__CLM_TRAN___Delet__75FAAC0C]
GO
ALTER TABLE [dbo].[CLM_TRAN_Reports]  WITH CHECK ADD  CONSTRAINT [FK__CLM_TRAN___Repor__731E3F61] FOREIGN KEY([ReportId])
REFERENCES [dbo].[FB_Report_Details] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_Reports] CHECK CONSTRAINT [FK__CLM_TRAN___Repor__731E3F61]
GO
ALTER TABLE [dbo].[CLM_TRAN_Reports]  WITH CHECK ADD  CONSTRAINT [FK__CLM_TRAN___Updat__750687D3] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_TRAN_Reports] CHECK CONSTRAINT [FK__CLM_TRAN___Updat__750687D3]
GO
ALTER TABLE [dbo].[CLM_Transaction]  WITH CHECK ADD  CONSTRAINT [FK__CLM_Trans__Claim__66B8687C] FOREIGN KEY([ClaimSource])
REFERENCES [dbo].[CLM_REF_Source] ([Id])
GO
ALTER TABLE [dbo].[CLM_Transaction] CHECK CONSTRAINT [FK__CLM_Trans__Claim__66B8687C]
GO
ALTER TABLE [dbo].[CLM_Transaction]  WITH CHECK ADD  CONSTRAINT [FK__CLM_Trans__Claim__6B7D1D99] FOREIGN KEY([ClaimRefundCurrency])
REFERENCES [dbo].[REF_Currency] ([Id])
GO
ALTER TABLE [dbo].[CLM_Transaction] CHECK CONSTRAINT [FK__CLM_Trans__Claim__6B7D1D99]
GO
ALTER TABLE [dbo].[CLM_Transaction]  WITH CHECK ADD  CONSTRAINT [FK__CLM_Trans__Creat__6D65660B] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_Transaction] CHECK CONSTRAINT [FK__CLM_Trans__Creat__6D65660B]
GO
ALTER TABLE [dbo].[CLM_Transaction]  WITH CHECK ADD  CONSTRAINT [FK__CLM_Trans__Custo__67AC8CB5] FOREIGN KEY([CustomerPriority])
REFERENCES [dbo].[CLM_REF_Priority] ([Id])
GO
ALTER TABLE [dbo].[CLM_Transaction] CHECK CONSTRAINT [FK__CLM_Trans__Custo__67AC8CB5]
GO
ALTER TABLE [dbo].[CLM_Transaction]  WITH CHECK ADD  CONSTRAINT [FK__CLM_Trans__Custo__68A0B0EE] FOREIGN KEY([CustomerReqRefundCurrency])
REFERENCES [dbo].[REF_Currency] ([Id])
GO
ALTER TABLE [dbo].[CLM_Transaction] CHECK CONSTRAINT [FK__CLM_Trans__Custo__68A0B0EE]
GO
ALTER TABLE [dbo].[CLM_Transaction]  WITH CHECK ADD  CONSTRAINT [FK__CLM_Trans__Delet__6F4DAE7D] FOREIGN KEY([DeletedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_Transaction] CHECK CONSTRAINT [FK__CLM_Trans__Delet__6F4DAE7D]
GO
ALTER TABLE [dbo].[CLM_Transaction]  WITH CHECK ADD  CONSTRAINT [FK__CLM_Trans__FOB_C__6994D527] FOREIGN KEY([FOB_Currency])
REFERENCES [dbo].[REF_Currency] ([Id])
GO
ALTER TABLE [dbo].[CLM_Transaction] CHECK CONSTRAINT [FK__CLM_Trans__FOB_C__6994D527]
GO
ALTER TABLE [dbo].[CLM_Transaction]  WITH CHECK ADD  CONSTRAINT [FK__CLM_Trans__Inspe__63DBFBD1] FOREIGN KEY([InspectionNo])
REFERENCES [dbo].[INSP_Transaction] ([Id])
GO
ALTER TABLE [dbo].[CLM_Transaction] CHECK CONSTRAINT [FK__CLM_Trans__Inspe__63DBFBD1]
GO
ALTER TABLE [dbo].[CLM_Transaction]  WITH CHECK ADD  CONSTRAINT [FK__CLM_Trans__RealI__6C7141D2] FOREIGN KEY([RealInspectionFeesCurrency])
REFERENCES [dbo].[REF_Currency] ([Id])
GO
ALTER TABLE [dbo].[CLM_Transaction] CHECK CONSTRAINT [FK__CLM_Trans__RealI__6C7141D2]
GO
ALTER TABLE [dbo].[CLM_Transaction]  WITH CHECK ADD  CONSTRAINT [FK__CLM_Trans__Recei__65C44443] FOREIGN KEY([ReceivedFrom])
REFERENCES [dbo].[CLM_REF_ReceivedFrom] ([Id])
GO
ALTER TABLE [dbo].[CLM_Transaction] CHECK CONSTRAINT [FK__CLM_Trans__Recei__65C44443]
GO
ALTER TABLE [dbo].[CLM_Transaction]  WITH CHECK ADD  CONSTRAINT [FK__CLM_Trans__Retai__6A88F960] FOREIGN KEY([Retail_Currency])
REFERENCES [dbo].[REF_Currency] ([Id])
GO
ALTER TABLE [dbo].[CLM_Transaction] CHECK CONSTRAINT [FK__CLM_Trans__Retai__6A88F960]
GO
ALTER TABLE [dbo].[CLM_Transaction]  WITH CHECK ADD  CONSTRAINT [FK__CLM_Trans__Statu__64D0200A] FOREIGN KEY([StatusId])
REFERENCES [dbo].[CLM_REF_Status] ([Id])
GO
ALTER TABLE [dbo].[CLM_Transaction] CHECK CONSTRAINT [FK__CLM_Trans__Statu__64D0200A]
GO
ALTER TABLE [dbo].[CLM_Transaction]  WITH CHECK ADD  CONSTRAINT [FK__CLM_Trans__Updat__6E598A44] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_Transaction] CHECK CONSTRAINT [FK__CLM_Trans__Updat__6E598A44]
GO
ALTER TABLE [dbo].[CLM_Transaction]  WITH CHECK ADD  CONSTRAINT [FK_CLM_Transaction_EntityId] FOREIGN KEY([EntityId])
REFERENCES [dbo].[AP_Entity] ([Id])
GO
ALTER TABLE [dbo].[CLM_Transaction] CHECK CONSTRAINT [FK_CLM_Transaction_EntityId]
GO
ALTER TABLE [dbo].[CLM_Transaction]  WITH CHECK ADD  CONSTRAINT [FK_CLM_Transaction_IT_UserMaster] FOREIGN KEY([AnalyzedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_Transaction] CHECK CONSTRAINT [FK_CLM_Transaction_IT_UserMaster]
GO
ALTER TABLE [dbo].[CLM_Transaction]  WITH CHECK ADD  CONSTRAINT [FK_CLM_Transaction_IT_UserMaster1] FOREIGN KEY([ValidatedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_Transaction] CHECK CONSTRAINT [FK_CLM_Transaction_IT_UserMaster1]
GO
ALTER TABLE [dbo].[CLM_Transaction]  WITH CHECK ADD  CONSTRAINT [FK_CLM_Transaction_IT_UserMaster2] FOREIGN KEY([ClosedBy])
REFERENCES [dbo].[IT_UserMaster] ([Id])
GO
ALTER TABLE [dbo].[CLM_Transaction] CHECK CONSTRAINT [FK_CLM_Transaction_IT_UserMaster2]
GO


-------create CU_REF_AccountingLeader starts------------
CREATE TABLE [dbo].[CU_REF_AccountingLeader](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Active] [int] NULL,
	[Sort] [int] NULL,
 CONSTRAINT [PK_CU_REF_AccountingLeader] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CU_REF_AccountingLeader] ON 
GO
INSERT [dbo].[CU_REF_AccountingLeader] ([Id], [Name], [Active], [Sort]) VALUES (1, N'KAM', 1, 1)
GO
INSERT [dbo].[CU_REF_AccountingLeader] ([Id], [Name], [Active], [Sort]) VALUES (2, N'CS', 1, 2)
GO
SET IDENTITY_INSERT [dbo].[CU_REF_AccountingLeader] OFF
GO
ALTER TABLE [dbo].[CU_REF_AccountingLeader] ADD  CONSTRAINT [DF_CU_REF_AccountingLeader_Active]  DEFAULT ((1)) FOR [Active]

-------create CU_REF_AccountingLeader Ends------------

-------create Cu_SalesIncharge starts------------
CREATE TABLE [dbo].[Cu_SalesIncharge](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[StaffId] [int] NULL,
	[Active] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
 CONSTRAINT [PK_Cu_SalesIncharge] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cu_SalesIncharge]  WITH CHECK ADD  CONSTRAINT [FK_Cu_SalesIncharge_CU_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[CU_Customer] ([Id])
GO
ALTER TABLE [dbo].[Cu_SalesIncharge] CHECK CONSTRAINT [FK_Cu_SalesIncharge_CU_Customer]
GO
ALTER TABLE [dbo].[Cu_SalesIncharge]  WITH CHECK ADD  CONSTRAINT [FK_Cu_SalesIncharge_HR_Staff] FOREIGN KEY([StaffId])
REFERENCES [dbo].[HR_Staff] ([Id])
GO
ALTER TABLE [dbo].[Cu_SalesIncharge] CHECK CONSTRAINT [FK_Cu_SalesIncharge_HR_Staff]
-------create Cu_SalesIncharge Ends------------

-------create CU_KAM starts------------
CREATE TABLE [dbo].[CU_KAM](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[KAM_Id] [int] NULL,
	[Active] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
 CONSTRAINT [PK_CU_KAM] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CU_KAM]  WITH CHECK ADD  CONSTRAINT [FK_CU_KAM_CU_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[CU_Customer] ([Id])
GO
ALTER TABLE [dbo].[CU_KAM] CHECK CONSTRAINT [FK_CU_KAM_CU_Customer]
GO
ALTER TABLE [dbo].[CU_KAM]  WITH CHECK ADD  CONSTRAINT [FK_CU_KAM_HR_Staff] FOREIGN KEY([KAM_Id])
REFERENCES [dbo].[HR_Staff] ([Id])
GO
ALTER TABLE [dbo].[CU_KAM] CHECK CONSTRAINT [FK_CU_KAM_HR_Staff]
-------create CU_KAM Ends------------

-------create CU_REF_ActivitiesLevel starts------------
CREATE TABLE [dbo].[CU_REF_ActivitiesLevel](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Active] [int] NULL,
	[Sort] [int] NULL,
 CONSTRAINT [PK_CU_REF_ActivitiesLevel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CU_REF_ActivitiesLevel] ON 
GO
INSERT [dbo].[CU_REF_ActivitiesLevel] ([Id], [Name], [Active], [Sort]) VALUES (1, N'Key', 1, 1)
GO
INSERT [dbo].[CU_REF_ActivitiesLevel] ([Id], [Name], [Active], [Sort]) VALUES (2, N'New', 1, 2)
GO
INSERT [dbo].[CU_REF_ActivitiesLevel] ([Id], [Name], [Active], [Sort]) VALUES (3, N'Regular', 1, 3)
GO
INSERT [dbo].[CU_REF_ActivitiesLevel] ([Id], [Name], [Active], [Sort]) VALUES (4, N'Small', 1, 4)
GO
INSERT [dbo].[CU_REF_ActivitiesLevel] ([Id], [Name], [Active], [Sort]) VALUES (5, N'Inactive', 1, 5)
GO
SET IDENTITY_INSERT [dbo].[CU_REF_ActivitiesLevel] OFF
GO
ALTER TABLE [dbo].[CU_REF_ActivitiesLevel] ADD  CONSTRAINT [DF_CU_REF_ActivitiesLevel_Active]  DEFAULT ((1)) FOR [Active]
-------create CU_REF_ActivitiesLevel Ends------------

-------create CU_REF_RelationshipStatus starts------------
CREATE TABLE [dbo].[CU_REF_RelationshipStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Active] [int] NULL,
	[Sort] [int] NULL,
 CONSTRAINT [PK_CU_REF_RelationshipStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CU_REF_RelationshipStatus] ON 
GO
INSERT [dbo].[CU_REF_RelationshipStatus] ([Id], [Name], [Active], [Sort]) VALUES (1, N'Stable', 1, 1)
GO
INSERT [dbo].[CU_REF_RelationshipStatus] ([Id], [Name], [Active], [Sort]) VALUES (2, N'Cautious', 1, 2)
GO
SET IDENTITY_INSERT [dbo].[CU_REF_RelationshipStatus] OFF
GO
ALTER TABLE [dbo].[CU_REF_RelationshipStatus] ADD  CONSTRAINT [DF_CU_REF_RelationshipStatus_Active]  DEFAULT ((1)) FOR [Active]
-------create CU_REF_RelationshipStatus Ends------------

-------create Cu_REF_BrandPriority starts------------
CREATE TABLE [dbo].[Cu_REF_BrandPriority](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Active] [int] NULL,
	[Sort] [int] NULL,
 CONSTRAINT [PK_Cu_REF_BrandPriority] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Cu_REF_BrandPriority] ON 
GO
INSERT [dbo].[Cu_REF_BrandPriority] ([Id], [Name], [Active], [Sort]) VALUES (1, N'Price', 1, 1)
GO
INSERT [dbo].[Cu_REF_BrandPriority] ([Id], [Name], [Active], [Sort]) VALUES (2, N'LeadTime', 1, 2)
GO
INSERT [dbo].[Cu_REF_BrandPriority] ([Id], [Name], [Active], [Sort]) VALUES (3, N'Quality', 1, 3)
GO
INSERT [dbo].[Cu_REF_BrandPriority] ([Id], [Name], [Active], [Sort]) VALUES (4, N'Speed', 1, 4)
GO
SET IDENTITY_INSERT [dbo].[Cu_REF_BrandPriority] OFF
GO
ALTER TABLE [dbo].[Cu_REF_BrandPriority] ADD  CONSTRAINT [DF_Cu_REF_BrandPriority_Active]  DEFAULT ((1)) FOR [Active]
-------create Cu_REF_BrandPriority Ends------------

-------create CU_Brandpriority starts------------
CREATE TABLE [dbo].[CU_Brandpriority](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[BrandpriorityId] [int] NULL,
	[Active] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
 CONSTRAINT [PK_CU_Brandpriority] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CU_Brandpriority]  WITH CHECK ADD  CONSTRAINT [FK_CU_Brandpriority_CU_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[CU_Customer] ([Id])
GO
ALTER TABLE [dbo].[CU_Brandpriority] CHECK CONSTRAINT [FK_CU_Brandpriority_CU_Customer]
GO
ALTER TABLE [dbo].[CU_Brandpriority]  WITH CHECK ADD  CONSTRAINT [FK_CU_Brandpriority_Cu_REF_BrandPriority] FOREIGN KEY([BrandpriorityId])
REFERENCES [dbo].[Cu_REF_BrandPriority] ([Id])
GO
ALTER TABLE [dbo].[CU_Brandpriority] CHECK CONSTRAINT [FK_CU_Brandpriority_Cu_REF_BrandPriority]
-------create CU_Brandpriority Ends------------


-------add four columns for  Cu_Customer starts------------
ALTER TABLE [dbo].[Cu_Customer] ADD [AccountingLeaderId] int Null
ALTER TABLE [dbo].[Cu_Customer] ADD [ActvitiesLevelId] int Null
ALTER TABLE [dbo].[Cu_Customer] ADD [RelationshipStatusId] int Null
ALTER TABLE [dbo].[Cu_Customer] ADD [DirectCompetitor] nvarchar(2000) Null
-------create CU_Brandpriority Ends------------
INSERT INTO [dbo].[ENT_Feature_Details] (FeatureId,EntityId,Active) VALUES(5,2,1)

-------------------------Data Management Starts -----------------------------------

CREATE TABLE [dbo].[DM_File](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[DMDetailsId] [int] NULL,
	[FileId] [nvarchar](200) NULL,
	[FileName] [nvarchar](200) NULL,
	[FileType] [nvarchar](200) NULL,
	[FileUrl] [nvarchar](max) NULL,
	[FileSize] [float] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[Active] [bit] NULL,
	CONSTRAINT [FK_DM_DETAILS_ID] FOREIGN KEY([DMDetailsId]) REFERENCES [dbo].[DM_details] ([Id])
	)

ALTER TABLE DM_details DROP COLUMN FilePath

update IT_Right set Path='data-management/data-management-add' where id=129

-------------------------Data Management Ends -----------------------------------

CREATE TABLE [dbo].[ENT_Master_Config](
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[EntityId] [int] NULL,
	[CountryId] [int] NULL,
	[Type] [nvarchar](300) NULL,
	[Value] [nvarchar](max) NULL,
	[Active] [bit] NULL,
	CONSTRAINT [FK_ENT_Master_Config_Ent_Master_Type] FOREIGN KEY([Type]) REFERENCES [dbo].[ENT_Master_Type] ([Id])
)

CREATE TABLE [dbo].[ENT_Master_Type](
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] [nvarchar](300) NULL,
	[Active] [bit] NULL,
	[Sort] [int] NULL
	)

-------------------------ENT_Master_Type -----------------------------------	
INSERT INTO ENT_Master_Type(Name,Active,sort)Values('Inspection Booking Terms',1,1)
INSERT INTO ENT_Master_Type(Name,Active,sort)Values('Inspection  Confirmation Terms English',1,2)
INSERT INTO ENT_Master_Type(Name,Active,sort)Values('Inspection  Confirmation Terms Chinese',1,3)
INSERT INTO ENT_Master_Type(Name,Active,sort)Values('Booking Team Group Email',1,4)
INSERT INTO ENT_Master_Type(Name,Active,sort)Values('Entity Short Name',1,5)
INSERT INTO ENT_Master_Type(Name,Active,sort)Values('Logo path 394x189',1,6)
INSERT INTO ENT_Master_Type(Name,Active,sort)Values('IC Chop',1,7)
INSERT INTO ENT_Master_Type(Name,Active,sort)Values('IC Sign',1,8)
INSERT INTO ENT_Master_Type(Name,Active,sort)Values('Head Office Address',1,9)
INSERT INTO ENT_Master_Type(Name,Active,sort)Values('Head Office Wrap Address',1,10)
INSERT INTO ENT_Master_Type(Name,Active,sort)Values('Company Registered Name for Inspection',1,11)
INSERT INTO ENT_Master_Type(Name,Active,sort)Values('Company Registered Name for Audit',1,12)

-------------------------ENT_Master_Config -----------------------------------
INSERT INTO ENT_Master_Config(EntityId,Type,Value,Active)Values(1,1,'\\Documents\\Insp_Terms\\API_Insp_Book_Terms12112019.pdf',1)
INSERT INTO ENT_Master_Config(EntityId,Type,Value,Active)Values(1,2,'\\Documents\\Insp_Terms\\API_ConfirmTermsEg11092020.pdf',1)
INSERT INTO ENT_Master_Config(EntityId,Type,Value,Active)Values(1,3,'\\Documents\\Insp_Terms\\API_ConfirmTermsCn11092020.pdf',1)
INSERT INTO ENT_Master_Config(EntityId,Type,Value,Active)Values(1,4,'booking@api-hk.com',1)
INSERT INTO ENT_Master_Config(EntityId,Type,Value,Active)Values(1,5,'API',1)
INSERT INTO ENT_Master_Config(EntityId,Type,Value,Active)Values(1,6,'ClientApp\\src\\assets\\images\\logo-header.png',1)
INSERT INTO ENT_Master_Config(EntityId,Type,Value,Active)Values(1,7,'ClientApp\\src\\assets\\images\\ICchop.jpg',1)
INSERT INTO ENT_Master_Config(EntityId,Type,Value,Active)Values(1,8,'ClientApp\\src\\assets\\images\\ICsign.png',1)
INSERT INTO ENT_Master_Config(EntityId,Type,Value,Active)Values(1,9,'Rooms 803-5, 8/F, Sing Shun Centre, 495 Castle Peak Road, Lai Chi Kok, Kowloon, Hong Kong Tel: +852 2741 6601, Fax: +852 2377 0028',1)
INSERT INTO ENT_Master_Config(EntityId,Type,Value,Active)Values(1,10,'Rooms 803-5, 8/F, Sing Shun Centre, 495 Castle Peak Road, \n Lai Chi Kok, Kowloon, Hong Kong, \n Tel: +852-3719 8611    Fax: +852-2377 0028 \n E-mail: api@api-hk.com     Website: http://www.api-hk.com',1)
INSERT INTO ENT_Master_Config(EntityId,Type,Value,Active)Values(1,11,'ASIA PACIFIC INSPECTION LTD',1)
INSERT INTO ENT_Master_Config(EntityId,Type,Value,Active)Values(1,12,'API AUDIT LTD',1)


----------------------------------------- Outsource QC Starts-----------------------------------------------
CREATE TABLE [dbo].[HR_OutSource_Company]
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(500),
	Active BIT,
	EntityId INT,
	CreatedBy INT,
	CreatedOn DATETIME,
	UpdatedBy INT,
	UpdatedOn DATETIME,
	DeletedBy INT,
	DeletedOn DATETIME,
	CONSTRAINT FK_HR_OUTSOURCE_COMPANY_ENTITYID FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_ENTITY] (Id),
	CONSTRAINT FK_HR_OUTSOURCE_COMPANY_CREATEDBY FOREIGN KEY(CreatedBy) REFERENCES [dbo].[IT_USERMASTER] (Id),
	CONSTRAINT FK_HR_OUTSOURCE_COMPANY_UPDATEDBY FOREIGN KEY(UpdatedBy) REFERENCES [dbo].[IT_USERMASTER] (Id),
	CONSTRAINT FK_HR_OUTSOURCE_COMPANY_DELETEDBY FOREIGN KEY(DeletedBy) REFERENCES [dbo].[IT_USERMASTER] (Id)
)

ALTER TABLE HR_Staff ADD HROutSourceCompanyId INT

ALTER TABLE HR_Staff ADD Constraint FK_HR_STaff_Outsource_Company_Id FOREIGN KEY(HROutSourceCompanyId) REFERENCES [dbo].[HR_OutSource_Company] (Id)

INSERT INTO IT_UserType(Id,Label) Values (5,'OutSource')

INSERT INTO IT_Role(RoleName,Active,EntityId,PrimaryRole,SecondaryRole)
VALUES
('Outsource Accounting',1,1,1,0)

----------------------------------------- Outsource QC Ends-----------------------------------------------


---------------------------------------FB_Report_Quantity_Details Starts---------------------------------------------------------------

ALTER TABLE [FB_Report_Quantity_Details] ADD [InspColorTransactionId] INT

ALTER TABLE [FB_Report_Quantity_Details] ADD CONSTRAINT FK_FB_RPT_DTL_COLOR_TRANSACTION_ID FOREIGN KEY (InspColorTransactionId) REFERENCES
[INSP_PurchaseOrder_Color_Transaction](Id)

ALTER TABLE [FB_Report_InspDefects] ADD InspColorTransactionId INT

ALTER TABLE [FB_Report_InspDefects] ADD CONSTRAINT FK_FB_RPT_DEFECT_COLOR_TRANSACTION_ID FOREIGN KEY (InspColorTransactionId) REFERENCES
[INSP_PurchaseOrder_Color_Transaction](Id)

ALTER TABLE [FB_Report_Details] ADD Main_Observations NVARCHAR(MAX)

ALTER TABLE [FB_Report_InspDefects] ADD Code NVARCHAR(100)

ALTER TABLE [FB_Report_InspDefects] ADD Size NVARCHAR(100)

ALTER TABLE [FB_Report_InspDefects] ADD Reparability NVARCHAR(100)

ALTER TABLE [FB_Report_InspDefects] ADD Garment_Grade NVARCHAR(100)

ALTER TABLE [FB_Report_InspDefects] ADD Zone NVARCHAR(100)


---------------------------------------FB_Report_Quantity_Details ends---------------------------------------------------------------

---Invoice price calculation types start -----------------

CREATE TABLE [dbo].[INV_REF_PriceCalculationType]
(
	[Id] int IDENTITY(1,1) PRIMARY KEY,
	[Name] varchar(250),
	[Active] BIT
)

ALTER TABLE [INV_AUT_TRAN_Details] ADD [PriceCalculationType] INT

ALTER TABLE [INV_AUT_TRAN_Details] ADD CONSTRAINT INV_AUT_TRAN_Details_PriceCalculationType 

FOREIGN KEY ([PriceCalculationType]) REFERENCES [dbo].[INV_REF_PriceCalculationType](Id)

Insert into INV_REF_PriceCalculationType(Name,Active)values('Normal',1)
Insert into INV_REF_PriceCalculationType(Name,Active)values('MinFee',1)
Insert into INV_REF_PriceCalculationType(Name,Active)values('MaxFee',1)
Insert into INV_REF_PriceCalculationType(Name,Active)values('SpecialPrice',1)

--- Invoice price calculation types end -----------------

---------------------------------------------- Booking Enhancement v3 Starts --------------------------------------------------------

CREATE TABLE [dbo].[INSP_TRAN_CS]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[InspectionId] INT,
	[CsId] INT,
	[Active] BIT,
	[CreatedBy] INT,
	[CreatedOn] DATETIME,
	[UpdatedBy] INT,
	[UpdatedOn] DATETIME,
	CONSTRAINT FK_INSP_TRAN_CS_InspectionId FOREIGN KEY ([InspectionId]) REFERENCES [dbo].[INSP_TRANSACTION](Id),
	CONSTRAINT FK_INSP_TRAN_CS_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_INSP_TRAN_CS_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_INSP_TRAN_CS_CsId FOREIGN KEY ([CsId]) REFERENCES [dbo].[IT_UserMaster](Id)
)

DROP TABLE EC_AUT_QC_Expense

INSERT INTO ENT_Master_Type(Name,Active,Sort) Values ('Inspection Service Date Lead Time Message',1,13)

INSERT INTO ENT_Master_Config(EntityId,Type,Value,Active) VALUES
(1,13,'Service date can be selected at least {0} working days before service date, if less than {1} working days, please contact our customer service',1),
(2,13,'Service date can be selected at least {0} working days before service date, if less than {1} working days, please contact our customer service',1)
---------------------------------------------- Booking Enhancement v3 Ends --------------------------------------------------------

-- ALD-1980 start --

ALTER  TABLE [FB_Report_Details] ADD [Report_Picture_Path] NVARCHAR(1000) NULL

---------------------------------------------- Booking Enhancement v3 Ends --------------------------------------------------------

INSERT INTO EM_ExchangeRateType(Label,TypeTransId,Active,EntityId)values('Customer',47,1,2)
---------------------------------------------- Booking Enhancement v3 Ends --------------------------------------------------------

---Invoice price complex type start ---

CREATE TABLE [dbo].[CU_PR_RefComplexType]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[Name] [nvarchar](300) NULL,
	[Active] [bit] NULL	
)

INSERT INTO CU_PR_RefComplexType(Name,Active)Values('Simple',1)
INSERT INTO CU_PR_RefComplexType(Name,Active)Values('Complex',1)


ALTER TABLE [dbo].[CU_PR_Details] ADD [PriceComplexType] INT NULL
ALTER TABLE [dbo].[CU_PR_Details] ADD CONSTRAINT CU_PR_Details_PriceComplexType  FOREIGN KEY(PriceComplexType) REFERENCES [dbo].[CU_PR_RefComplexType](Id)
---Invoice price complex type end ---
ALTER TABLE [ES_Details] ADD [IsPictureFileInEmail] BIT NULL;

---------------------------[INSP_Transaction_Draft] Starts ---------------------------------------

CREATE TABLE [dbo].[INSP_Transaction_Draft](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[CustomerId] [int] NULL,
	[SupplierId] [int] NULL,
	[FactoryId] [int] NULL,
	[ServiceDateFrom] [datetime] NULL,
	[ServiceDateTo] [datetime] NULL,
	[BrandId] [int] NULL,
	[DepartmentId] [int] NULL,
	[InspectionId] [int] NULL,
	[BookingInfo] [nvarchar](max) NULL,
	[Active] [BIT] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	CONSTRAINT [FK_INSP_Transaction_Draft_CustomerId] FOREIGN KEY([CustomerId])REFERENCES [dbo].[CU_Customer] ([Id]),
	CONSTRAINT [FK_INSP_Transaction_Draft_SupplierId] FOREIGN KEY([SupplierId]) REFERENCES [dbo].[SU_Supplier] ([Id]),
	CONSTRAINT [FK_INSP_Transaction_Draft_FactoryId] FOREIGN KEY([FactoryId]) REFERENCES [dbo].[SU_Supplier] ([Id]),
	CONSTRAINT [FK_INSP_Transaction_Draft_BrandId] FOREIGN KEY([BrandId]) REFERENCES [dbo].[CU_Brand] ([Id]),
	CONSTRAINT [FK_INSP_Transaction_Draft_DepartmentId] FOREIGN KEY([DepartmentId]) REFERENCES [dbo].[CU_Department] ([Id]),
	CONSTRAINT [FK_INSP_Transaction_Draft_InspectionId] FOREIGN KEY([InspectionId]) REFERENCES [dbo].[INSP_Transaction] ([Id]),
	CONSTRAINT [FK_INSP_Transaction_Draft_CreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_INSP_Transaction_Draft_UpdatedBy] FOREIGN KEY([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_INSP_Transaction_Draft_DeletedBy] FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id])
	)

	ALTER TABLE INSP_Transaction_Draft ADD IsReInspectionDraft BIT NULL

	ALTER TABLE INSP_Transaction_Draft ADD IsReBookingDraft BIT NULL

	ALTER TABLE INSP_Transaction_Draft ADD PreviousBookingNo INT NULL

	---------------------------[INSP_Transaction_Draft] Ends ---------------------------------------
	
CREATE TABLE [dbo].[IT_Right_Entity]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[RightId] INT NULL,
	[EntityId] INT NULL,
	[CustomerId] INT NULL,
	[Active] BIT NULL,
	CONSTRAINT FK_IT_Right_Entity_IT_Right FOREIGN KEY(RightId) REFERENCES [dbo].[IT_Right],
	CONSTRAINT FK_IT_RIGHT_ENTITY_CU_CUSTOMER FOREIGN KEY(CustomerId) REFERENCES [dbo].[CU_Customer],
	CONSTRAINT FK_IT_RIGHT_ENTITY_AP_ENTITY FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity]
)
-- ALD-1980 end --

ALTER TABLE [dbo].[CU_PR_TRAN_Subcategory] DROP column [Sub_CategoryId]
ALTER TABLE [dbo].[CU_PR_TRAN_Subcategory] DROP constraint CU_PR_TRAN_Subcategory_Sub_CategoryId

ALTER TABLE [dbo].[CU_PR_TRAN_Subcategory] ADD [Sub_Category2Id] INT NULL
ALTER TABLE [dbo].[CU_PR_TRAN_Subcategory] ADD CONSTRAINT CU_PR_TRAN_Subcategory_Sub_Category2Id  FOREIGN KEY(Sub_Category2Id) REFERENCES [dbo].[REF_ProductCategory_Sub2](Id)



-------------------------[CU_Entity] Starts  Jira ALD-1974  ----------------------------------------------------------------
CREATE TABLE [Cu_Entity]
(
	[Id] INT IDENTITY(1,1)  PRIMARY KEY,
	[CustomerId] INT NOT NULL,
	[EntityId] INT NOT NULL,
	[Active] BIT,
	[CreatedOn] DATETIME, 
	[CreatedBy] INT, 
	[DeletedOn] DATETIME,
	[DeletedBy] INT, 

	CONSTRAINT Cu_Entity_CustomerId  FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Cu_Customer](Id),
	CONSTRAINT Cu_Entity_EntityId  FOREIGN KEY ([EntityId]) REFERENCES [dbo].[AP_Entity](Id),
	CONSTRAINT Cu_Entity_CreatedBy Foreign Key (CreatedBy) References IT_UserMaster(Id),
	CONSTRAINT Cu_Entity_DeletedBy Foreign Key (DeletedBy) References IT_UserMaster(Id)
)

--------------------------[CU_Entity] ends ----------------------------------------------------------------

CREATE TABLE [dbo].[ENT_Pages]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[RightId] INT,
	[ServiceId] INT,
	[Active] BIT,
	[Remarks] NVARCHAR(500),
	CONSTRAINT FK_ENT_PAGE_SERVICE FOREIGN KEY(ServiceId) REFERENCES [dbo].[REF_Service]
)

CREATE TABLE [dbo].[ENT_Pages_Fields]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[FieldName] NVARCHAR(50),
	[ENTPageId] INT,
	[EntityId] INT,
	[CustomerId] INT,
	[Active] BIT,
	CONSTRAINT FK_ENT_PAGE_FIELD_PAGEID FOREIGN KEY([ENTPageId]) REFERENCES [dbo].[ENT_Pages],
	CONSTRAINT FK_ENT_PAGE_FIELD_ENTITY_ID FOREIGN KEY([EntityId]) REFERENCES [dbo].[AP_ENTITY],
	CONSTRAINT FK_ENT_PAGE_FIELD_CUSTOMER_ID FOREIGN KEY([CustomerId]) REFERENCES [dbo].[CU_CUSTOMER]
)

INSERT INTO [dbo].[ENT_Pages] (RightId,ServiceId,Active,Remarks) Values (1,1,1,'Booking Page')

INSERT INTO [dbo].[ENT_Pages] (RightId,ServiceId,Active,Remarks) Values (2,1,1,'Customer Product')

INSERT INTO [dbo].[ENT_Pages_Fields](FieldName,[ENTPageId],[EntityId],[CustomerId],[Active]) VALUES
('Booking_Edit_POQuantity',1,1,NULL,1)

INSERT INTO  ENT_Pages_Fields(FieldName,ENTPageId,EntityId,CustomerId,Active) VALUES
('Booking_Edit_DisplayMaster',1,1,10,1)

INSERT INTO  ENT_Pages_Fields(FieldName,ENTPageId,EntityId,CustomerId,Active) VALUES
('Booking_Edit_DisplayChild',1,1,10,1)

INSERT INTO  ENT_Pages_Fields(FieldName,ENTPageId,EntityId,CustomerId,Active) VALUES
('Booking_Edit_EcoPack',1,1,10,1)

INSERT INTO  ENT_Pages_Fields(FieldName,ENTPageId,EntityId,CustomerId,Active) VALUES
('Booking_Edit_CustomerRefPo',1,1,140,1)

INSERT INTO  ENT_Pages_Fields(FieldName,ENTPageId,EntityId,CustomerId,Active) VALUES
('Customer_Product_SubCategory2',2,1,NULL,1)


ALTER TABLE INSP_TRAN_File_Attachment ADD FileDescription NVARCHAR(MAX)

-----------------------------Booking Enhancement v4 Ends---------------------------------------


--------------------------[SU_Entity] Starts   Jira ALD-1976 --------------------------------------------------------------
CREATE TABLE [SU_Entity]
(
	[Id] INT IDENTITY(1,1)  PRIMARY KEY,
	[SupplierId] INT NOT NULL,
	[EntityId] INT NOT NULL,
	[Active] BIT,
	[CreatedOn] DATETIME, 
	[CreatedBy] INT, 
	[DeletedOn] DATETIME,
	[DeletedBy] INT, 

	CONSTRAINT Su_Entity_Supplierid  FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Su_Supplier](Id),
    CONSTRAINT Su_Entity_EntityId  FOREIGN KEY ([EntityId]) REFERENCES [dbo].[AP_Entity](Id),
	CONSTRAINT Su_Entity_CreatedBy Foreign Key (CreatedBy) References IT_UserMaster(Id),
	CONSTRAINT Su_Entity_DeletedBy Foreign Key (DeletedBy) References IT_UserMaster(Id)
)

-------------------------[SU_Entity] ends ----------------------------------------------------------------------


ALTER TABLE [dbo].[INSP_TRANSACTION_DRAFT] ADD EntityId INT

ALTER TABLE [dbo].[INSP_TRANSACTION_DRAFT] ADD CONSTRAINT [FK_INSP_TRANSACTION_DRAFT_ENTITYId] FOREIGN KEY([EntityId]) REFERENCES [dbo].[AP_Entity]([Id])




----------------------- Customer And Customer Contact Entity data scripts Starts  ----------------------
--CU_Entity
Insert Into CU_Entity (CustomerId,EntityId,Active,CreatedOn)
Select CU.Id,1,1,GetDate() From CU_Customer CU
LEFT JOIN CU_Entity CUE ON CU.Id=CUE.CustomerId
Where CUE.EntityId IS NULL And CUE.CustomerId IS NULL
Group By CU.Id


--CU_Contact_Entity_Map
Insert Into CU_Contact_Entity_Map (ContactId,EntityId)
Select CUC.Id,1 From CU_Contact CUC
LEFT JOIN CU_Contact_Entity_Map CUCEM ON CUC.Id=CUCEM.ContactId
WHERE CUCEM.EntityId IS NULL AND CUCEM.ContactId IS NULL
Group By CUC.Id

--Update Primary Entity in CU_Contact_Entity_Map
Update CU_Contact Set PrimaryEntity=1 Where PrimaryEntity IS NULL

----------------------- Customer And Customer Contact Entity data scripts Ends ----------------------------


----------------------- Supplier And Supplier Contact Entity data scripts Starts ----------------------------
--SU_Entity
Insert Into SU_Entity (SupplierId,EntityId,Active,CreatedOn)Select SU.Id,1,1,GETDATE() From SU_Supplier SU
LEFT JOIN SU_Entity SUE ON SU.Id = SUE.SupplierId
Where SUE.SupplierId IS NULL AND SUE.EntityId IS NULL
Group By SU.Id

--SU_Contact_Entity_Map
Insert Into SU_Contact_Entity_Map (ContactId,EntityId)
Select SUC.Id,1 From SU_Contact SUC
LEFT JOIN SU_Contact_Entity_Map SUCEM ON SUC.Id=SUCEM.ContactId
WHERE SUCEM.EntityId IS NULL AND SUCEM.ContactId IS NULL
Group By SUC.Id

--Update Primary Entity in SU_Contact_Entity_Map
Update SU_Contact Set PrimaryEntity=1 Where PrimaryEntity IS NULL

----------------------- Supplier And Supplier Contact Entity data scripts Ends ----------------------------

----------------------- HR Staff Entity data scripts Starts ----------------------------

--HR_Entity_Map
Insert Into HR_Entity_Map (StaffId,EntityId)
Select HS.Id,1 From HR_Staff HS
LEFT JOIN HR_Entity_Map HEM ON HS.Id=HEM.StaffId
Where HEM.StaffId IS NULL AND HEM.EntityId IS NULL
Group By HS.Id

-- Update Primary Entity in HR_Staff
Update HR_Staff Set PrimaryEntity=1 Where PrimaryEntity IS NULL

----------------------- HR Staff Entity data scripts Ends ----------------------------


---------------------------------------------Su Contact Starts -----------------------------------------------------------------------

ALTER TABLE [dbo].[SU_Contact] ADD CreatedBy INT

ALTER TABLE [dbo].[SU_Contact] ADD CONSTRAINT FK_SU_CONTACT_CREATED_BY FOREIGN KEY (CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id)

ALTER TABLE [dbo].[SU_Contact] ADD CreatedOn DATETIME

---------------------------------------------Su Contact Ends -----------------------------------------------------------------------

---------------------------------ALD-2060 Booking Enhancement v4 Starts ---------------------------------------------------------------------

ALTER TABLE [dbo].[REF_Billing_Entity] ADD EntityId INT

ALTER TABLE [dbo].[REF_Billing_Entity] ADD CONSTRAINT FK_REF_BILLING_ENTITY_ENTITY_ID FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_ENTITY](Id)

insert into ENT_Pages_Fields(FieldName,ENTPageId,EntityId,CustomerId,Active)
Values('Booking_Edit_Picking',1,1,null,1)

insert into ENT_Pages_Fields(FieldName,ENTPageId,EntityId,CustomerId,Active)
Values('Booking_Edit_Combine',1,1,null,1)

insert into ENT_Pages_Fields(FieldName,ENTPageId,EntityId,CustomerId,Active)
Values('Booking_Edit_Product_Barcode',1,1,null,1)

insert into ENT_Pages_Fields(FieldName,ENTPageId,EntityId,CustomerId,Active)
Values('Booking_Edit_Product_Factory_Reference',1,1,null,1)

insert into ENT_Pages_Fields(FieldName,ENTPageId,EntityId,CustomerId,Active)
Values('Booking_Edit_First_Service_Date',1,1,null,1)

---------------------------------ALD-2060 Booking Enhancement v4 Ends ---------------------------------------------------------------------

------------------ALD-2059 Email Subject Predefined Fields Starts----------------------------

ALTER TABLE [dbo].[ES_SU_PreDefined_Fields] ADD [EntityId] INT

ALTER TABLE [dbo].[ES_SU_PreDefined_Fields] ADD CONSTRAINT FK_ES_SU_PreDefined_Field_Entity_Id 
FOREIGN KEY([EntityId]) REFERENCES [dbo].[AP_Entity](Id)

------------------ALD-2059 Email Subject Predefined Fields Ends----------------------------




-------------------------------------------Usp_CSDashboard_GetModuleStatus start---------------------------------------------------
/****** Object:  StoredProcedure [dbo].[Usp_CSDashboard_GetModuleStatus]    Script Date: 03/05/2022 14:56:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================             
-- Author:  Ronika             
-- Create date: 18-6-2021             
-- Description: get the booking, quotation, allocation, report status list by booking id list            


-- Author:  Nikhil             
-- Update date: 03-5-2022
-- Description: add entity id where clause in midtask and qu_quotation table 
-- =============================================              
ALTER PROCEDURE [dbo].[Usp_CSDashboard_GetModuleStatus] (               
 @BookingIdList IntList NULL READONLY,              
 @RoleIdList IntList NULL READONLY,              
 @UserId int null,            
 @EntityId INT NULL)              
AS              
          
BEGIN           
          
Declare @StatusTable TABLE(              
Id int,            
Name nvarchar(100),          
[Count] int,          
ActionType int          
)              
          
--inpection status             
INSERT INTO @StatusTable (Id,Name,[Count],ActionType)           
SELECT inspstatus.Id as Id , inspstatus.Status as Name , count(insptran.id) as Count, 1 as ActionType FROM            
insp_status inspstatus left join insp_transaction insptran              
on insptran.Status_Id = inspstatus.Id  AND insptran.Entity_Id= @EntityId    
and (NOT EXISTS(SELECT 1 FROM @BookingIdList) OR insptran.id IN (SELECT * FROM @BookingIdList))            
WHERE inspstatus.Active = 1 and  inspstatus.id not in (9)  --9 - allocated status      
Group BY inspstatus.Id, inspstatus.Status          
              
-- inspection status task count            
INSERT INTO @StatusTable (Id,Name,[Count],ActionType)           
SELECT midtask.tasktypeid as Id, '' as Name, count(midtask.tasktypeid) as Count, 2 as ActionType FROM            
it_userrole userrole left join mid_task midtask  on userrole.UserId = midtask.ReportTo and midtask.EntityId=@EntityId
WHERE userrole.EntityId=@EntityId AND (            
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR midtask.LinkId IN (SELECT * FROM @BookingIdList)) and            
(userrole.UserId = @UserId and midtask.isdone = 0)             
 --and  (userrole.RoleId in(24) and midtask.TaskTypeId in(6))            
 and             
 (            
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 25)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 25))  and midtask.TaskTypeId in(5,8))            
 or            
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 24)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 24))  and midtask.TaskTypeId in(6))            
 ))            
 --25 - Inspection VerIFied, 24 - Inspection Confirmed, 5 -VerIFy Inspection   , 6-Confirm Inspection   , 8 -SplitInspectionBooking.            
Group BY midtask.tasktypeid             
            
--add quotation pending status        
        
--booking --8- verIFied, 2 - confirmed        
--quotation required - 1 - customer check point        
        
INSERT INTO @StatusTable (Id,Name,[Count],ActionType)               
 
SELECT 0 as Id , 'Pending' as Name , count(insptran.id) as [Count], 3 as ActionType FROM insp_transaction insptran         
inner join CU_CheckPoints cucheckpoint on cucheckpoint.CustomerId = insptran.Customer_Id AND cucheckpoint.EntityId=@EntityId 
WHERE insptran.Entity_Id=@EntityId AND insptran.Status_Id in (8,2, 9, 3) and cucheckpoint.CheckpointTypeId = 1  AND cucheckpoint.Active = 1 AND
(NOT EXISTS (SELECT * FROM QU_Quotation_Insp quInsp WHERE quInsp.IdBooking = insptran.Id) OR 
NOT EXISTS (SELECT * FROM QU_Quotation_Insp quInsp inner join QU_QUOTATION q on q.id = quInsp.IdQuotation WHERE q.EntityId=@EntityId And q.IdStatus not in (5) AND quInsp.IdBooking = insptran.Id ))
AND (NOT EXISTS(SELECT 1 FROM @BookingIdList) OR insptran.Id IN (SELECT Id FROM @BookingIdList))
        
        
-- quotation status count        
INSERT INTO @StatusTable (Id,Name,[Count],ActionType)                     
              
SELECT qustatus.Id AS Id, qustatus.Label AS [Name], COUNT(quotation.Id) AS [Count], 3 as ActionType FROM 
QU_Status qustatus 
left join (SELECT IdStatus, Id FROM QU_QUOTATION q 
INNER JOIN QU_Quotation_Insp insp on insp.IdQuotation = q.Id WHERE q.EntityId=@EntityId AND (NOT EXISTS(SELECT 1 FROM @BookingIdList) OR insp.IdBooking IN (SELECT * FROM @BookingIdList))) AS quotation ON quotation.IdStatus = qustatus.Id
WHERE qustatus.Active = 1
Group BY qustatus.Id, qustatus.Label

-- quotation status task count           
INSERT INTO @StatusTable (Id,Name,[Count],ActionType)           
SELECT midtask.tasktypeid as Id, '' as Name, count(midtask.tasktypeid) as [Count], 4 as ActionType FROM            
it_userrole userrole left join mid_task midtask  on userrole.UserId = midtask.ReportTo 
WHERE userrole.EntityId=@EntityId And midtask.EntityId=@EntityId AND (            
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR midtask.LinkId IN (SELECT IdQuotation FROM QU_Quotation_Insp WHERE IdBooking in (SELECT Id FROM @BookingIdList))) and            
(userrole.UserId = @UserId and midtask.isdone = 0)             
 and         
 (            
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 20)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 20))  and midtask.TaskTypeId in(10,14))            
 or            
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 21)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 21))  and midtask.TaskTypeId in(7))        
 or        
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 22)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 22))  and midtask.TaskTypeId in(12,13))        
 or        
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 27)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 27))  and midtask.TaskTypeId in(11))        
 ))            
Group BY midtask.tasktypeid          

--pending quotation link Id will be booking id and not quotation Id as above
INSERT INTO @StatusTable (Id,Name,[Count],ActionType)           
SELECT midtask.tasktypeid as Id, '' as Name, count(midtask.tasktypeid) as [Count], 4 as ActionType FROM            
it_userrole userrole left join mid_task midtask  on userrole.UserId = midtask.ReportTo            
WHERE userrole.EntityId=@EntityId And midtask.EntityId=@EntityId AND (            
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR midtask.LinkId IN (SELECT Id FROM @BookingIdList)) and            
(userrole.UserId = @UserId and midtask.isdone = 0)             
 and         
 (            
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 20)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 20))  and midtask.TaskTypeId in(10,14))  
 ))
Group BY midtask.tasktypeid   
-- role - 20-Quotation Request, 21 Quotation Manager, 22 Quotation Confirmation,27 Quotation Send        
--task 14 Quotation Pending,10 Quotation ModIFy,7 quotation approve, 12 Quotation Customer Confirmed,13 Quotation Customer Reject,11 Quotation Sent        
        
--inpection allocation status           
INSERT INTO @StatusTable (Id,Name,[Count],ActionType)           
SELECT insptran.Status_Id as Id , inspstatus.Status as Name , count(insptran.id) as [Count], 5 as ActionType FROM            
insp_status inspstatus left join insp_transaction insptran              
on insptran.Status_Id = inspstatus.Id  AND insptran.Entity_Id=@EntityId          
and (NOT EXISTS(SELECT 1 FROM @BookingIdList) OR insptran.id IN (SELECT * FROM @BookingIdList))            
WHERE inspstatus.Active = 1 and  inspstatus.id in (2,9)  --9 - allocated status , 2 - confirmed          
Group BY insptran.Status_Id, inspstatus.Status               
            
-- inspection allocation status task count           
INSERT INTO @StatusTable (Id,Name,[Count],ActionType)           
SELECT midtask.tasktypeid as Id, '' as Name, count(midtask.tasktypeid) as [Count], 6 as ActionType FROM            
it_userrole userrole left join mid_task midtask  on userrole.UserId = midtask.ReportTo            
WHERE userrole.EntityId=@EntityId And midtask.EntityId=@EntityId AND (            
(NOT EXISTS(SELECT 1 FROM @BookingIdList) OR midtask.LinkId IN (SELECT * FROM @BookingIdList)) and            
(userrole.UserId = @UserId and midtask.isdone = 0)             
 and             
 ((EXISTS(SELECT * FROM @RoleIdList WHERE id = 26)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 26))  and midtask.TaskTypeId in(9))              
 )            
 --26 Inspection Schedule - role --9 Inspection Schedule - task type          
Group BY midtask.tasktypeid             
              
  --product tran fb report filling status count          
INSERT INTO @StatusTable (Id,Name,[Count],ActionType)    
SELECT fbstatus.id,'QC-' +fbstatus.StatusName,count(fbreport.Id) as [Count],7 as ActionType FROM fb_status fbstatus    
left join 
fb_report_details fbreport ON  fbreport.Fb_Filling_Status = fbstatus.Id
WHERE fbstatus.Type=3  and fbstatus.Active = 1  AND  (NOT EXISTS(SELECT 1 FROM @BookingIdList) OR fbreport.Inspection_Id  IN (SELECT * FROM @BookingIdList))
Group BY fbstatus.id,fbstatus.StatusName    
  
--product tran fb report reivew status count          
INSERT INTO @StatusTable (Id,Name,[Count],ActionType)     
SELECT fbstatus.id,'CS-' +fbstatus.StatusName,count(fbreport.Id) as [Count],7 as ActionType   
FROM fb_status fbstatus    
left join     
fb_report_details fbreport ON  fbreport.Fb_Review_Status = fbstatus.Id 
WHERE fbstatus.Type=4  and fbstatus.Active = 1    AND  (NOT EXISTS(SELECT 1 FROM @BookingIdList) OR fbreport.Inspection_Id  IN (SELECT * FROM @BookingIdList)) 
Group BY fbstatus.id,fbstatus.StatusName  
          
-- product tran fb report status count          
INSERT INTO @StatusTable (Id,Name,[Count],ActionType)   
SELECT fbstatus.id,'FR-' +fbstatus.StatusName,count(fbreport.Id) as [Count],7 as ActionType   
FROM fb_status fbstatus    
left join 
fb_report_details fbreport ON  fbreport.Fb_Report_Status = fbstatus.Id 
WHERE fbstatus.Id in(13,15)   and fbstatus.Active = 1  AND  (NOT EXISTS(SELECT 1 FROM @BookingIdList) OR fbreport.Inspection_Id  IN (SELECT * FROM @BookingIdList))  
Group BY fbstatus.id,fbstatus.StatusName  
  
-- report task count   - user role check          
declare @roleId as int = (SELECT userrole.RoleId FROM it_userrole userrole           
WHERE userrole.EntityId=@EntityId AND ((userrole.UserId = @UserId) and             
((EXISTS(SELECT * FROM @RoleIdList WHERE id = 35)) and (userrole.RoleId IN (SELECT * FROM @RoleIdList WHERE id = 35)))             
))          
          
-- 35 - report checker          
IF(@roleid =35)          
BEGIN         
--report task count          
INSERT INTO @StatusTable (Id,Name,[Count],ActionType)           
SELECT fbreport.Fb_Review_Status as Id, 'to be review' as name, count(fbreport.Id) as [Count], 8 as ActionType FROM fb_report_details fbreport          
inner join fb_status fbstatus on fbstatus.Id = fbreport.Fb_Filling_Status          
WHERE fbreport.Fb_Filling_Status = 9 and fbreport.Fb_Review_Status = 10 -- 9 - filling validated, 10 CS not started      
AND (NOT EXISTS(SELECT 1 FROM @BookingIdList) OR fbreport.Inspection_Id  IN (SELECT * FROM @BookingIdList))            
Group BY fbreport.Fb_Review_Status , fbstatus.StatusName           
--fbstatus.StatusName as Name          
end          
          
  SELECT Id,Name,sum([Count]) as [count],ActionType FROM @StatusTable  
Group BY Id,Name ,ActionType  
    
END   

-------------------------------Usp_CSDashboard_GetModuleStatus End---------------------------------



-------------------------------Update Usp_CSDashboard_GetNewDetails SP starts-------------------
/****** Object:  StoredProcedure [dbo].[Usp_CSDashboard_GetNewDetails]    Script Date: 03/05/2022 12:25:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ronika
-- Create date: 15-6-2021
-- Description:	get the new count - customer, supplier, factory, product, po, booking
-- =============================================

-- =============================================
-- Author:		Nikhil
-- Create date: 03-05-2022
-- Description:	add entity id filter with the customer, supplier and factory
-- =============================================
ALTER PROCEDURE [dbo].[Usp_CSDashboard_GetNewDetails] (@FromDate DATETIME,
@ToDate DATETIME,
@EntityId INT NULL)

AS
BEGIN
	--customer count
	DECLARE @customerCount AS INT = (SELECT
			COUNT(cus.Id)
		FROM CU_Customer cus
		WHERE cus.CreatedOn BETWEEN @FromDate AND @ToDate AND cus.Active=1
		And EXISTS (Select 1 From CU_Entity cue Where cus.Id=cue.CustomerId AND cue.EntityId=@EntityId And cue.Active=1))

	--supplier count  and -- where 2 - supplier type
	DECLARE @supCount AS INT = (SELECT
			COUNT(sup.Id)
		FROM SU_Supplier sup
		WHERE sup.type_id = 2
		AND sup.CreatedDate BETWEEN @FromDate AND @ToDate AND sup.Active=1
		AND EXISTS (Select 1 From SU_Entity sue Where sup.Id=sue.SupplierId AND sue.EntityId=@EntityId And sue.Active=1))

	--factory Count  and where 1 - factory type
	DECLARE @factCount AS INT = (SELECT
			COUNT(fact.Id)
		FROM SU_Supplier fact
		WHERE fact.type_id = 1
		AND fact.CreatedDate BETWEEN @FromDate AND @ToDate AND fact.Active=1
		AND EXISTS (Select 1 From SU_Entity factentity Where fact.Id=factentity.SupplierId AND factentity.EntityId=@EntityId And factentity.Active=1))

	--booking count
	DECLARE @bookingCount AS INT = (SELECT
			COUNT(insp.Id)
		FROM INSP_Transaction insp
		WHERE insp.Entity_Id=@EntityId AND insp.CreatedOn BETWEEN @FromDate AND @ToDate AND insp.Status_Id!=4)

	--po count
	DECLARE @poCount AS INT = (SELECT
			COUNT(purchaseorder.Id)
		FROM CU_PurchaseOrder purchaseorder
		WHERE purchaseorder.EntityId=@EntityId AND purchaseorder.CreatedOn
		BETWEEN @FromDate AND @ToDate AND purchaseorder.Active=1)

	--product count
	DECLARE @productCount AS INT = (SELECT
			COUNT(product.Id)
		FROM CU_Products product
		WHERE product.EntityId=@EntityId AND product.CreatedTime
		BETWEEN @FromDate AND @ToDate AND product.Active=1)

	SELECT
		@customerCount AS CustomerCount
	   ,@supCount AS SupplierCount
	   ,@factCount AS FactoryCount
	   ,@bookingCount AS BookingCount
	   ,@poCount AS POCount
	   ,@productCount AS ProductCount

END
---------------------------------Update Usp_CSDashboard_GetNewDetails SP Ends----------------------------------




--------------------------------Usp_FinanceKPI_GetBilledMandayDetails SP start--------------------------------
/****** Object:  StoredProcedure [dbo].[Usp_FinanceKPI_GetBilledMandayDetails]    Script Date: 03/05/2022 15:00:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
  
  
-- =============================================  
-- Author:  Binny  
-- Create date: 22-06-2021  
-- Description: Billed Man day data  
-- =============================================  

-- =============================================  
-- Author:  Nikhil  
-- Create date: 03-05-2022
-- Description: Add EntityId filter in REF_Budget_Forecast table
-- =============================================  
ALTER PROCEDURE [dbo].[Usp_FinanceKPI_GetBilledMandayDetails]   
  --@BookingIdList IntList READONLY,  
  @ServiceDateFrom DATETIME,  
  @ServiceDateTo DATETIME,  
  @CustomerId INT NULL,  
  @SupplierId INT NULL,  
  @FactoryIdList Udt_Int NULL READONLY,  
  @FactoryCountryIdList Udt_Int NULL READONLY ,  
  @OfficeIdList Udt_Int NULL READONLY ,  
  @ServiceTypeIdList Udt_Int NULL READONLY ,  
  @BrandIdList Udt_Int NULL READONLY ,  
  @DepartmentIdList Udt_Int NULL READONLY ,  
  @BuyerIdList Udt_Int NULL READONLY ,  
  @EntityId INT NULL  
AS  
BEGIN  
  
 CREATE TABLE #TempInspectionIds  
 (  
  ID INT PRIMARY KEY IDENTITY(1,1),  
  InspectionId INT   
 )  
  
 Insert into #TempInspectionIds  
 SELECT Distinct insp.Id  
 FROM INSP_Transaction insp  
 INNER JOIN INSP_TRAN_ServiceType insp_service on insp_service.Inspection_Id=insp.Id and insp_service.Active=1  
 INNER JOIN REF_ServiceType ref_servicetype on ref_servicetype.Id=insp_service.ServiceType_Id   AND ref_servicetype.EntityId=@EntityId
 INNER JOIN SU_Address su_address on su_address.Supplier_Id=insp.Factory_Id 
 INNER JOIN REF_Country ref_country on ref_country.Id=su_address.CountryId and ref_country.Active=1  
 LEFT JOIN INSP_TRAN_CU_Brand insp_brand on insp_brand.Inspection_Id=insp.Id and insp_brand.Active = 1  
 LEFT JOIN INSP_TRAN_CU_Department insp_dept on insp_dept.Inspection_Id=insp.Id and insp_dept.Active = 1  
 LEFT JOIN INSP_TRAN_CU_Buyer insp_buyer on insp_buyer.Inspection_Id=insp.Id and insp_buyer.Active = 1  
 WHERE insp.Entity_Id=@EntityId AND insp.Customer_Id = COALESCE(NULLIF(@CustomerId, ''), insp.Customer_Id)  
 AND insp.Supplier_Id = COALESCE(NULLIF(@SupplierId, ''), insp.Supplier_Id)  
 AND (NOT EXISTS(SELECT 1 FROM @FactoryIdList) OR insp.Factory_Id IN (SELECT * FROM @FactoryIdList))  
 AND (NOT EXISTS(SELECT 1 FROM @FactoryCountryIdList) OR su_address.CountryId IN (SELECT * FROM @FactoryCountryIdList))  
 AND (NOT EXISTS(SELECT 1 FROM @OfficeIdList) OR insp.Office_Id IN (SELECT * FROM @OfficeIdList))  
 AND (NOT EXISTS(SELECT 1 FROM @ServiceTypeIdList) OR ref_servicetype.Id IN (SELECT * FROM @ServiceTypeIdList))  
 AND (NOT EXISTS(SELECT 1 FROM @BrandIdList) OR insp_brand.Brand_Id IN (SELECT * FROM @BrandIdList))  
 AND (NOT EXISTS(SELECT 1 FROM @DepartmentIdList) OR insp_dept.Department_Id IN (SELECT * FROM @DepartmentIdList))  
 AND (NOT EXISTS(SELECT 1 FROM @BuyerIdList) OR insp_buyer.Buyer_Id IN (SELECT * FROM @BuyerIdList))  
  
 SELECT Month(ServiceDate_To) AS [Month], FORMAT(ServiceDate_To, 'MMM') AS [MonthName],  
 Year(ServiceDate_To) AS [Year],   
 SUM(NoOfManday) AS [MonthManDay]   
 FROM INSP_Transaction insp  
 INNER JOIN QU_Quotation_Insp quot ON quot.IdBooking = insp.Id  
 INNER JOIN QU_QUOTATION q on q.id = quot.IdQuotation AND q.EntityId=@EntityId 
 WHERE insp.Entity_Id=@EntityId AND ServiceDate_To BETWEEN  @ServiceDateFrom and @ServiceDateTo  
 AND insp.Status_Id in (6,7,11) AND q.IdStatus = 7  
 AND insp.Id in (Select InspectionId from #TempInspectionIds)  
 GROUP BY YEAR(ServiceDate_To), MONTH(ServiceDate_To), FORMAT(ServiceDate_To, 'MMM')  
    
  UNION   
  
 SELECT Month(ServiceDate_To) AS [Month], FORMAT(ServiceDate_To, 'MMM') AS [MonthName],  
 Year(ServiceDate_To) AS [Year],   
 SUM(NoOfManday) AS [MonthManDay]   
 FROM INSP_Transaction insp  
 INNER JOIN QU_Quotation_Insp quot ON quot.IdBooking = insp.Id  
 INNER JOIN QU_QUOTATION q on q.id = quot.IdQuotation  AND q.EntityId=@EntityId
 WHERE insp.Entity_Id=@EntityId AND ServiceDate_To BETWEEN DATEADD(year, -1,  @ServiceDateFrom) and DATEADD(year, -1,  @ServiceDateTo)   
 AND insp.Status_Id in (6,7,11) AND q.IdStatus = 7  
 AND insp.Id in (Select InspectionId from #TempInspectionIds)  
 GROUP BY YEAR(ServiceDate_To), MONTH(ServiceDate_To), FORMAT(ServiceDate_To, 'MMM')  
  
 --select [Month], [MonthName], [Year], [MonthManDay] FROM  #CurrentYearMandayTable UNION select [Month], [MonthName], [Year], [MonthManDay] FROM  #LastYearMandayTable  
  
 SELECT [Month], FORMAT([Month], 'MMM') AS [MonthName],   
 SUM(ManDay) AS [MonthManDay]  
 FROM REF_Budget_Forecast   
 WHERE  EntityId=@EntityId AND [Year] = YEAR(GETDATE()) AND Active = 1  
 GROUP BY [Month]  
  
 Select * from #TempInspectionIds  
  
 DROP TABLE #TempInspectionIds  
  
END  
------------------------------Usp_FinanceKPI_GetBilledMandayDetails SP Ends----------------------------------



-----------------------------Usp_FinanceKPI_GetMandayRateDetails SP Start----------------------------
/****** Object:  StoredProcedure [dbo].[Usp_FinanceKPI_GetMandayRateDetails]    Script Date: 03/05/2022 16:00:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================  
-- Author:  Binny  
-- Create date: 22-06-2021  
-- Description: get man day rate  
-- =============================================  
-- =============================================  
-- Author:  Nikhil  
-- Create date: 03-05-2022
-- Description: Add EntityId filter in REF_Budget_Forecast
-- =============================================  
ALTER PROCEDURE [dbo].[Usp_FinanceKPI_GetMandayRateDetails]   
  --@BookingIdList IntList READONLY,  
  @ServiceDateFrom DATETIME,  
  @ServiceDateTo DATETIME,  
  @CustomerId INT NULL,  
  @SupplierId INT NULL,  
  @FactoryIdList Udt_Int NULL READONLY,  
  @FactoryCountryIdList Udt_Int NULL READONLY ,  
  @OfficeIdList Udt_Int NULL READONLY ,  
  @ServiceTypeIdList Udt_Int NULL READONLY ,  
  @BrandIdList Udt_Int NULL READONLY ,  
  @DepartmentIdList Udt_Int NULL READONLY ,  
  @BuyerIdList Udt_Int NULL READONLY ,  
  @EntityId INT NULL  
AS  
BEGIN  
  
 CREATE TABLE #TempInspectionIds  
 (  
  ID INT PRIMARY KEY IDENTITY(1,1),  
  InspectionId INT   
 )  
  
 Insert into #TempInspectionIds  
 SELECT Distinct insp.Id  
 FROM INSP_Transaction insp  
 INNER JOIN INSP_TRAN_ServiceType insp_service on insp_service.Inspection_Id=insp.Id and insp_service.Active=1  
 INNER JOIN REF_ServiceType ref_servicetype on ref_servicetype.Id=insp_service.ServiceType_Id   AND ref_servicetype.EntityId=@EntityId
 INNER JOIN SU_Address su_address on su_address.Supplier_Id=insp.Factory_Id  
 INNER JOIN REF_Country ref_country on ref_country.Id=su_address.CountryId and ref_country.Active=1  
 LEFT JOIN INSP_TRAN_CU_Brand insp_brand on insp_brand.Inspection_Id=insp.Id and insp_brand.Active = 1  
 LEFT JOIN INSP_TRAN_CU_Department insp_dept on insp_dept.Inspection_Id=insp.Id and insp_dept.Active = 1  
 LEFT JOIN INSP_TRAN_CU_Buyer insp_buyer on insp_buyer.Inspection_Id=insp.Id and insp_buyer.Active = 1  
 WHERE insp.Entity_Id=@EntityId AND insp.Customer_Id = COALESCE(NULLIF(@CustomerId, ''), insp.Customer_Id)  
 AND insp.Supplier_Id = COALESCE(NULLIF(@SupplierId, ''), insp.Supplier_Id)  
 AND (NOT EXISTS(SELECT 1 FROM @FactoryIdList) OR insp.Factory_Id IN (SELECT * FROM @FactoryIdList))  
 AND (NOT EXISTS(SELECT 1 FROM @FactoryCountryIdList) OR su_address.CountryId IN (SELECT * FROM @FactoryCountryIdList))  
 AND (NOT EXISTS(SELECT 1 FROM @OfficeIdList) OR insp.Office_Id IN (SELECT * FROM @OfficeIdList))  
 AND (NOT EXISTS(SELECT 1 FROM @ServiceTypeIdList) OR ref_servicetype.Id IN (SELECT * FROM @ServiceTypeIdList))  
 AND (NOT EXISTS(SELECT 1 FROM @BrandIdList) OR insp_brand.Brand_Id IN (SELECT * FROM @BrandIdList))  
 AND (NOT EXISTS(SELECT 1 FROM @DepartmentIdList) OR insp_dept.Department_Id IN (SELECT * FROM @DepartmentIdList))  
 AND (NOT EXISTS(SELECT 1 FROM @BuyerIdList) OR insp_buyer.Buyer_Id IN (SELECT * FROM @BuyerIdList))  
  
 SELECT CAST(Row_number()  
         OVER(  
           ORDER BY (SELECT NULL)) AS INT)AS Id,  
 Month(insp.ServiceDate_To) AS [Month], FORMAT(insp.ServiceDate_To, 'MMM') AS [MonthName],  
 Year(insp.ServiceDate_To) AS [Year],   
 SUM(NoOfManday) AS [MonthManDay], SUM(inv.inspectionFees) AS [InspFees], inv.InvoiceCurrency AS [CurrencyId]   
 FROM INSP_Transaction insp  
 INNER JOIN QU_Quotation_Insp quot ON quot.IdBooking = insp.Id  
 INNER JOIN QU_QUOTATION q ON q.Id = quot.IdQuotation  AND q.EntityId=@EntityId
 INNER JOIN INV_AUT_TRAN_Details inv ON inv.InspectionId = insp.Id   AND inv.EntityId=@EntityId
 WHERE insp.Entity_Id=@EntityId AND ServiceDate_To BETWEEN  @ServiceDateFrom and @ServiceDateTo  
 AND inv.InvoiceStatus != 4 AND q.IdStatus = 7 AND insp.Status_Id in (6,7,11)  
 AND insp.Id in (Select InspectionId from #TempInspectionIds)  
 GROUP BY YEAR(ServiceDate_To), MONTH(ServiceDate_To), FORMAT(ServiceDate_To, 'MMM'), InvoiceCurrency  
  
 UNION  
  
 SELECT CAST(Row_number()  
         OVER(  
           ORDER BY (SELECT @@ROWCOUNT)) AS INT)AS Id,  
 Month(ServiceDate_To) AS [Month], FORMAT(ServiceDate_To, 'MMM') AS [MonthName],  
 Year(ServiceDate_To) AS [Year],   
 SUM(NoOfManday) AS [MonthManDay], SUM(inv.inspectionFees) AS [InspFees], inv.InvoiceCurrency AS [CurrencyId]   
 FROM INSP_Transaction insp  
 INNER JOIN QU_Quotation_Insp quot ON quot.IdBooking = insp.Id  
 INNER JOIN QU_QUOTATION q ON q.Id = quot.IdQuotation AND q.EntityId=@EntityId 
 INNER JOIN INV_AUT_TRAN_Details inv ON inv.InspectionId = insp.Id  AND inv.EntityId=@EntityId
 WHERE insp.Entity_Id=@EntityId AND ServiceDate_To BETWEEN DATEADD(year, -1,  @ServiceDateFrom) and DATEADD(year, -1,  @ServiceDateTo)   
 AND inv.InvoiceStatus != 4 AND q.IdStatus = 7 AND insp.Status_Id in (6,7,11)  
 AND insp.Id in (Select InspectionId from #TempInspectionIds)  
 GROUP BY YEAR(ServiceDate_To), MONTH(ServiceDate_To), FORMAT(ServiceDate_To, 'MMM'), InvoiceCurrency  
  
  
  
 SELECT CAST(Row_number()  
         OVER(  
           ORDER BY (SELECT NULL)) AS INT) AS Id, [Month], FORMAT([Month], 'MMM') AS [MonthName],  
 SUM(fees) AS [InspFees], SUM(ManDay) AS [MonthManDay],  
 CurrencyId AS [CurrencyId]  
 FROM REF_Budget_Forecast   
 WHERE [EntityId]=@EntityId AND [Year] = YEAR(GETDATE()) AND Active = 1  
 GROUP BY [Month], fees, ManDay, CurrencyId  
  
 DROP TABLE #TempInspectionIds  
END  

---------------------------Usp_FinanceKPI_GetMandayRateDetails SP Ends-------------------------------------------------



-----------------------------Usp_FinanceKPI_GetRejectQuotationDetails starts------------------------------------------
/****** Object:  StoredProcedure [dbo].[Usp_FinanceKPI_GetRejectQuotationDetails]    Script Date: 03/05/2022 16:50:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Binny
-- Create date: 23-06-2021
-- Description:	quotation count and customer rejected quotation count
-- =============================================
-- =============================================
-- Author:		Nikhil
-- Create date: 03-05-2022
-- Description:	Add Entity Id in QU_TRAN_Status_Log
-- =============================================
ALTER PROCEDURE [dbo].[Usp_FinanceKPI_GetRejectQuotationDetails]
	
	@BookingIdList Udt_Int NULL READONLY,
	@EntityId INT NULL

AS
BEGIN
	DECLARE @TotalQuotation INT
	DECLARE @RejectedQuotation INT

	SELECT @TotalQuotation = COUNT(DISTINCT(quotInsp.IdQuotation))  FROM QU_Quotation_Insp quotInsp
	INNER JOIN QU_QUOTATION quot ON quot.id = quotInsp.IdQuotation
	WHERE quot.EntityId=@EntityId AND quotInsp.IdBooking IN (SELECT * FROM @BookingIdList)
	AND quot.IdStatus = 7

	SELECT @RejectedQuotation = COUNT(DISTINCT(quotInsp.IdQuotation)) FROM QU_Quotation_Insp quotInsp
	--INNER JOIN QU_QUOTATION quot ON quot.id = quotInsp.IdQuotation
	INNER JOIN QU_TRAN_Status_Log quotLog ON quotLog.QuotationId = quotInsp.IdQuotation -- AND quotLog.EntityId=@EntityId 
	WHERE quotInsp.IdBooking IN (SELECT * FROM @BookingIdList)
	AND quotlog.StatusId = 6

	SELECT @TotalQuotation AS [QuotationCount], @RejectedQuotation AS [RejectedQuotationCount]
END

---------------------------Usp_FinanceKPI_GetRejectQuotationDetails sp Ends---------------------------------------

----------------------------Usp_Defect_KPI_MDM sp Start-------------------------------------------
/****** Object:  StoredProcedure [dbo].[Usp_Defect_KPI_MDM]    Script Date: 04/05/2022 09:50:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Usp_Defect_KPI_MDM] (
	@CustomerId INT NULL,
	@FromDate DATETIME,
	@ToDate DATETIME,
	@TemplateId INT NULL,
	@OfficeIdList Udt_Int NULL READONLY ,
	@ServiceTypeIdList Udt_Int NULL READONLY,
	@InvoiceNo varchar(100) NULL,
	@BrandIdList Udt_Int NULL READONLY ,
	@DepartmentIdList Udt_Int NULL READONLY,
	@EntityId INT NULL )
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN  TRY

			DECLARE @tblresult TABLE
			(
			bookingNo int,
			serviceDate nvarchar(100),
			customerName nvarchar(1000),
			supplierName nvarchar(1000),
			factoryName nvarchar(1000),
			serviceType nvarchar(1000),
			inspectionStatus nvarchar(1000),
			productRef nvarchar(1000),
			productDesc nvarchar(1000),
			poNumber nvarchar(max),
			poQty int,
			inspectedQty int,
			totalDefects int,
			totalCritical int,
			totalMajor int,
			totalMinor int,
			totalQtyReworked int,
			totalQtyReplaced int,
			totalQtyRejected int,
			finalResult nvarchar(1000),
			productid int,
			reportid int,
			department nvarchar(1000),
			FactoryCountry nvarchar(1000)
			)
			DECLARE @tbldefects Table
			(
			bookingno int,
			productid int,
			reportid int,
			totalCritical int,
			totalMajor int,
			totalMinor int,
			totalQtyReworked int,
			totalQtyReplaced int,
			totalQtyRejected int
			)

			insert into @tblresult(bookingNo,serviceDate,customerName,supplierName,factoryName,serviceType,inspectionStatus,productRef,productDesc,
			poQty,poNumber,inspectedQty,finalResult,productid,reportid, department,FactoryCountry)
			SELECT insp.Id 'Ins#',case WHEN insp.ServiceDate_From=insp.ServiceDate_To THEN CONVERT(VARCHAR(100), insp.ServiceDate_From,103) else 
			CONVERT(VARCHAR(100), insp.ServiceDate_From,103)+'-'+CONVERT(VARCHAR(100), insp.ServiceDate_To,103) END 'ins date',cu.Customer_Name 'Customer Name',su.Supplier_Name 'Supplier Name'
			,suf.Supplier_Name 'Factory Name' , refs.Name 'Service Type',insps.Status 'Inspection Status',cup.ProductID 'Product Code',
			cup.[Product Description] 'Product Desc', ipt.TotalBookingQuantity 'Po Qty',
			(SELECT STUFF((SELECT   ',' + convert(varchar(1000),cupo.PONO)  from INSP_PurchaseOrder_Transaction ito 
			join CU_PurchaseOrder cupo on cupo.Id=ito.PO_Id AND cupo.EntityId=@EntityId
			where ito.EntityId=@EntityId AND cupo.Active=1 AND ito.Inspection_Id=insp.Id and ito.Product_Ref_Id=ipt.Id FOR XML PATH ('')),1,1,''))'PO Number',
			(SELECT sum(ISNULL(fbq.InspectedQuantity,0)) from FB_Report_Quantity_Details fbq 
			where fbq.FbReportDetailId=ipt.Fb_Report_Id and fbq.InspPoTransactionId in (SELECT ipt2.Id from INSP_PurchaseOrder_Transaction ipt2
			where ipt2.EntityId=@EntityId AND ipt2.Active=1 and ipt2.Inspection_Id=ipt.Inspection_Id and ipt2.Product_Ref_Id=ipt.Id) and fbq.Active=1)'Inspected Qty' ,
			isnull(fb.OverAllResult,'')'Final Result',ipt.Product_Id,ipt.Fb_Report_Id, dept.[Name],con.Country_Name
			from INSP_Transaction insp
			join INSP_Product_Transaction ipt on insp.Id=ipt.Inspection_Id AND ipt.EntityId=@EntityId
			left join FB_Report_Details fb on fb.Id=ipt.Fb_Report_Id
			join CU_Products cup on cup.Id=ipt.Product_Id and cup.Active=1 AND cup.EntityId=@EntityId
			join CU_Customer cu on cu.Id=insp.Customer_Id AND EXISTS (SELECT 1 FROM CU_Entity cue Where cue.CustomerId=cu.Id AND cue.Active=1 AND cue.EntityId=@EntityId)
			join SU_Supplier su on su.Id=insp.Supplier_Id AND EXISTS (SELECT 1 FROM SU_Entity sue Where sue.SupplierId=su.Id AND sue.Active=1 AND sue.EntityId=@EntityId)
			join SU_Supplier suf on suf.Id=insp.Factory_Id AND EXISTS (SELECT 1 FROM SU_Entity suef Where suef.SupplierId=suf.Id AND suef.Active=1 AND suef.EntityId=@EntityId)
			JOIN SU_Address sud on sud.Supplier_Id=suf.Id and sud.AddressTypeId=1
			JOIN REF_Country con on con.Id=sud.CountryId
			join INSP_TRAN_ServiceType ser on ser.Inspection_Id=insp.Id and ser.Active=1
			join REF_ServiceType refs on refs.Id=ser.ServiceType_Id AND refs.EntityId=@EntityId
			join INSP_Status insps on insps.Id=insp.Status_Id
			LEFT join INSP_TRAN_CU_Brand insp_brand on insp_brand.Inspection_Id=insp.Id and insp_brand.Active = 1
			LEFT join INSP_TRAN_CU_Department insp_dept on insp_dept.Inspection_Id=insp.Id and insp_dept.Active = 1
			LEFT join CU_Department dept on dept.id = insp_dept.Department_Id AND dept.EntityId=@EntityId

			where insp.Entity_Id=@EntityId AND insp.Customer_Id = COALESCE(NULLIF(@CustomerId, ''), insp.Customer_Id) and insp.Status_Id!=4 and ipt.Active=1 and
			insp.ServiceDate_To BETWEEN  @FromDate and @ToDate --mm/dd/yyyy 
			AND (NOT EXISTS(SELECT 1 FROM @OfficeIdList) OR insp.Office_Id IN (SELECT * FROM @OfficeIdList))
			AND (NOT EXISTS(SELECT 1 FROM @ServiceTypeIdList) OR refs.Id IN (SELECT * FROM @ServiceTypeIdList))
			AND (NOT EXISTS(SELECT 1 FROM @BrandIdList) OR insp_brand.Brand_Id IN (SELECT * FROM @BrandIdList))
			AND (NOT EXISTS(SELECT 1 FROM @DepartmentIdList) OR insp_dept.Department_Id IN (SELECT * FROM @DepartmentIdList))
			order by Ins#

			--SELECT * from @tblresult

			INSERT INTO @tbldefects(productid,reportid,totalCritical,totalMajor,totalMinor,totalQtyReworked,totalQtyReplaced,totalQtyRejected,bookingno)
			SELECT ipt.Product_Id,ipt.Fb_Report_Id,sum(isnull( fbq.Critical,0))'critical',
			sum(isnull( fbq.Major,0))'Major',sum(isnull( fbq.Minor,0))'Minor',
			sum(isnull( fbq.Qty_Reworked,0))'Qc Reworked',sum(isnull( fbq.Qty_Replaced,0))'QC Replaced',
			sum(isnull( fbq.Qty_Rejected,0))'QC Rejected' , ipt.Inspection_Id
			from FB_Report_InspDefects fbq
			join FB_Report_Details fbr on fbr.Id=fbq.FbReportDetailId
			join INSP_PurchaseOrder_Transaction  inpo on inpo.Id=fbq.InspPoTransactionId AND inpo.EntityId=@EntityId
			join INSP_Product_Transaction ipt on ipt.Id=inpo.Product_Ref_Id AND ipt.EntityId=@EntityId
			where ipt.Inspection_Id in (SELECT bookingNo from @tblresult) and fbq.Active=1 and inpo.Active=1 and ipt.Active=1 and fbr.Active=1
			group by ipt.Product_Id,ipt.Fb_Report_Id,ipt.Inspection_Id

			--SELECT * from @tbldefects

			UPDATE r SET  r.totalDefects=(d.totalCritical+d.totalMajor+d.totalMinor),
			r.totalCritical=d.totalCritical,r.totalMajor=d.totalMajor,r.totalMinor=d.totalMinor,
			r.totalQtyReworked=d.totalQtyReworked,r.totalQtyReplaced=d.totalQtyReplaced,r.totalQtyRejected=d.totalQtyRejected
			from @tblresult r join @tbldefects d on r.productid=d.productid and r.reportid=d.reportid and r.bookingNo=d.bookingno 


			SELECT bookingNo ,
			serviceDate ,
			customerName ,
			supplierName ,
			factoryName ,
			serviceType ,
			inspectionStatus ,
			productRef ,
			productDesc ,
			poNumber ,
			poQty ,
			ISNULL(inspectedQty,0)AS inspectedQty ,
			ISNULL(totalDefects,0)AS totalDefects ,
			ISNULL(totalCritical,0)AS totalCritical ,
			ISNULL(totalMajor,0)AS totalMajor ,
			ISNULL(totalMinor,0)AS totalMinor ,
			ISNULL(totalQtyReworked,0)AS totalQtyReworked ,
			ISNULL(totalQtyReplaced,0)AS totalQtyReplaced ,
			ISNULL(totalQtyRejected,0)AS totalQtyRejected ,
			finalResult ,
			productid ,
			--reportid ,
			ISNULL(Department,'') AS Department,
			FactoryCountry
			
			from @tblresult order by bookingNo


		END TRY

		BEGIN CATCH
			
			THROW

		END CATCH


END
-------------------------------------Usp_Defect_KPI_MDM sp ends----------------------------------------------------------

CREATE TABLE [dbo].[INSP_REP_CUS_Decision_Template](
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	[CustomerId] [int] NULL,
	[EntityId] [int] NULL,
	[TemplatePath] [varchar](500) NULL,
	[IsDefault] [bit] NULL,
	[ServiceTypeId] [int] NULL,
	CONSTRAINT [FK_INSP_REP_CUS_Decision_Template_AP_Entity] FOREIGN KEY([EntityId]) REFERENCES [dbo].[AP_Entity] ([Id]),
	CONSTRAINT [FK_INSP_REP_CUS_Decision_Template_CU_Customer] FOREIGN KEY([CustomerId]) REFERENCES [dbo].[CU_Customer] ([Id]),
	CONSTRAINT [FK_INSP_REP_CUS_Decision_Template_REF_ServiceType] FOREIGN KEY([ServiceTypeId]) REFERENCES [dbo].[Ref_serviceType] ([Id])
	)



-------------------------------------------- Report Fast Transaction  REP_Fast_Transaction Start     -----------------------------------------------------------------

Insert Into CU_CheckPointType (Name,Active,Entity_Id) Values  ('New Report Format',1,1) 

Insert Into MID_NotificationType (Id,Label,EntityId) Values (32,'Fast Report Generation Failed ',1)

Create Table REP_FAST_REF_Status(
Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
Name nvarchar(200) NOT NULL,
Sort int 
)

Insert Into REP_FAST_REF_Status Values ('Not Started',1)
Insert Into REP_FAST_REF_Status Values ('In Progress',1)
Insert Into REP_FAST_REF_Status Values ('Completed',1)
Insert Into REP_FAST_REF_Status Values ('Error',1)
Insert Into REP_FAST_REF_Status Values ('Cancelled',1)
Insert Into REP_FAST_REF_Status Values ('Pushed To FB, ',1)


Create Table REP_FAST_Transaction(
Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
ReportId int,
BookingId int,
Status int,
CreatedOn datetime NOT NULL DEFAULT GETDATE(),
TryCount int,
IT_Notification bit,
CONSTRAINT FK_REP_FAST_Transaction_ReportId FOREIGN KEY (ReportId) REFERENCES FB_Report_Details(Id),
CONSTRAINT FK_REP_FAST_Transaction_BookingId FOREIGN KEY (BookingId) REFERENCES INSP_Transaction(Id),
CONSTRAINT FK_REP_FAST_Transaction_Status FOREIGN KEY (Status) REFERENCES REP_FAST_REF_Status(Id)
)


Create Table REP_FAST_TRAN_Log(
Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
FastTranId int,
ReportId int,
Status int,
ErrorLog nvarchar(4000),
CreatedOn datetime NOT NULL DEFAULT GETDATE(),
CONSTRAINT FK_REP_FAST_TRAN_Log_ReportId FOREIGN KEY (ReportId) REFERENCES FB_Report_Details(Id),
CONSTRAINT FK_REP_FAST_TRAN_Log_FastTranId FOREIGN KEY (FastTranId) REFERENCES REP_FAST_Transaction(Id),
CONSTRAINT FK_REP_FAST_TRAN_Log_Status FOREIGN KEY (Status) REFERENCES REP_FAST_REF_Status(Id)
)


Create Table  MID_Notification_Message(
Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
Name nvarchar(100),
Active bit
)

Insert Into MID_Notification_Message Values('Fast Report Generation Failed For {0}',1)


Alter Table MID_Notification Add MessageId int 
Alter Table MID_Notification Add Constraint FK_MID_Notification_MessageId FOREIGN KEY (MessageId) REFERENCES MID_Notification_Message(Id)
Alter Table MID_Notification Add NotificationMessage nvarchar(1000)


----------------------------------- REP Fast Transaction Ends  ---------------------------------------------------------


----------------------------------- Add Entity Id in logs tables Start---------------------------------------------------------
Alter Table AUD_TRAN_Status_Log Add EntityId int
Alter Table AUD_TRAN_Status_Log Add Constraint FK_AUD_TRAN_Status_Log_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
Update AUD_TRAN_Status_Log Set EntityId=1 Where EntityId IS NULL

Alter Table EventBookingLog Add EntityId int
Alter Table EventBookingLog Add Constraint FK_EventBookingLog_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
Update EventBookingLog Set EntityId=1 Where EntityId IS NULL

Alter Table FB_Booking_RequestLog Add EntityId int
Alter Table FB_Booking_RequestLog Add Constraint FK_FB_Booking_RequestLog_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
Update FB_Booking_RequestLog Set EntityId=1 Where EntityId IS NULL

Alter Table FB_Report_Manual_Log Add EntityId int
Alter Table FB_Report_Manual_Log Add Constraint FK_FB_Report_Manual_Log_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
Update FB_Report_Manual_Log Set EntityId=1 Where EntityId IS NULL

Alter Table INSP_TRAN_Status_Log Add EntityId int
Alter Table INSP_TRAN_Status_Log Add Constraint FK_INSP_TRAN_Status_Log_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
Update INSP_TRAN_Status_Log Set EntityId=1 Where EntityId IS NULL

Alter Table INV_AUT_TRAN_Status_Log Add EntityId int
Alter Table INV_AUT_TRAN_Status_Log Add Constraint FK_INV_AUT_TRAN_Status_Log_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
Update INV_AUT_TRAN_Status_Log Set EntityId=1 Where EntityId IS NULL

Alter Table INV_EXF_TRAN_Status_Log Add EntityId int
Alter Table INV_EXF_TRAN_Status_Log Add Constraint FK_INV_EXF_TRAN_Status_Log_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
Update INV_EXF_TRAN_Status_Log Set EntityId=1 Where EntityId IS NULL

Alter Table JOB_Schedule_Log Add EntityId int
Alter Table JOB_Schedule_Log Add Constraint FK_JOB_Schedule_Log_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
Update JOB_Schedule_Log Set EntityId=1 Where EntityId IS NULL

Alter Table LOG_Booking_Report_Email_Queue  Add EntityId int
Alter Table LOG_Booking_Report_Email_Queue  Add Constraint FK_LOG_Booking_Report_Email_Queue_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
Update LOG_Booking_Report_Email_Queue  Set EntityId=1 Where EntityId IS NULL

Alter Table LOG_Email_Queue_Attachments Add EntityId int
Alter Table LOG_Email_Queue_Attachments Add Constraint FK_LOG_Email_Queue_Attachments_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
Update LOG_Email_Queue_Attachments Set EntityId=1 Where EntityId IS NULL

Alter Table QU_TRAN_Status_Log  Add EntityId int
Alter Table QU_TRAN_Status_Log  Add Constraint FK_QU_TRAN_Status_Log_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
Update QU_TRAN_Status_Log  Set EntityId=1 Where EntityId IS NULL



---------------------------------------ALD-2114 Add Report link add in REP_FAST_Transaction------------------------------------------------------------------
Alter Table REP_Fast_Transaction Add ReportLink nvarchar(2000)
----------------------------------------------------------------------------------------------------------------------------------------------


-----------------------------ALD-2120 Add user type logic in field visibility Starts-------------------

CREATE TABLE [dbo].[ENT_Fields]
(
    Id INT PRIMARY KEY IDENTITY(1,1),
	Name NVARCHAR(200),
	Active BIT,
	ENTPageId INT,
	CONSTRAINT FK_ENT_Page_Id FOREIGN KEY(ENTPageId) REFERENCES API_05102021_V2.dbo.ENT_Pages (Id)
)

ALTER TABLE [dbo].[ENT_Pages_Fields] ADD ENTFieldId INT
ALTER TABLE [dbo].[ENT_Pages_Fields] ADD CONSTRAINT FK_ENT_FIELD_ID FOREIGN KEY(ENTFieldId) REFERENCES [dbo].[ENT_Fields]

ALTER TABLE [dbo].[ENT_Pages_Fields] ADD UserTypeId INT
ALTER TABLE [dbo].[ENT_Pages_Fields] ADD CONSTRAINT FK_USER_TYPE_ID FOREIGN KEY(UserTypeId) REFERENCES [dbo].[IT_UserType]
ALTER TABLE [dbo].[ENT_Pages_Fields] DROP COLUMN FieldName

ALTER TABLE [dbo].[ENT_Pages_Fields] DROP CONSTRAINT FK_ENT_PAGE_FIELD_PAGEID
ALTER TABLE [dbo].[ENT_Pages_Fields] DROP COLUMN ENTPageId

-----------------------------------Ent Field Insertion Starts----------------------------------

insert into [dbo].[ENT_Fields](Name,Active,ENTPageId) Values ('Booking_Edit_POQuantity',1,1)
insert into [dbo].[ENT_Fields](Name,Active,ENTPageId) Values ('Booking_Edit_DisplayMaster',1,1)
insert into [dbo].[ENT_Fields](Name,Active,ENTPageId) Values ('Booking_Edit_DisplayChild',1,1)
insert into [dbo].[ENT_Fields](Name,Active,ENTPageId) Values ('Booking_Edit_EcoPack',1,1)
insert into [dbo].[ENT_Fields](Name,Active,ENTPageId) Values ('Booking_Edit_CustomerRefPo',1,1)
insert into [dbo].[ENT_Fields](Name,Active,ENTPageId) Values ('Customer_Product_SubCategory2',1,2)
insert into [dbo].[ENT_Fields](Name,Active,ENTPageId) Values ('Booking_Edit_Product_SubCategory2',1,1)
insert into [dbo].[ENT_Fields](Name,Active,ENTPageId) Values ('Booking_Edit_Picking',1,1)
insert into [dbo].[ENT_Fields](Name,Active,ENTPageId) Values ('Booking_Edit_Combine',1,1)
insert into [dbo].[ENT_Fields](Name,Active,ENTPageId) Values ('Booking_Edit_Product_Barcode',1,1)
insert into [dbo].[ENT_Fields](Name,Active,ENTPageId) Values ('Booking_Edit_Product_Factory_Reference',1,1)
insert into [dbo].[ENT_Fields](Name,Active,ENTPageId) Values ('Booking_Edit_First_Service_Date',1,1)

-----------------------------------Ent Field Insertion Ends----------------------------------

-----------------------------------Ent Page Field Insertion Starts----------------------------------

insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,1,1)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,1,2)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,1,3)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,1,4)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,10,1,2,2)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,10,1,3,2)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,10,1,4,2)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,140,1,5,2)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,6,1)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (2,NULL,1,6,1)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (2,NULL,1,6,2)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (2,NULL,1,6,3)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (2,NULL,1,6,4)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (2,NULL,1,7,1)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (2,NULL,1,7,2)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (2,NULL,1,7,3)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (2,NULL,1,7,4)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,8,1)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,8,2)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,8,3)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,8,4)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,9,1)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,9,2)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,9,3)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,9,4)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,10,1)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,10,2)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,10,3)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,10,4)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,11,1)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,11,2)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,11,3)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,11,4)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,12,1)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,12,2)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,12,3)
insert into [dbo].[ENT_Pages_Fields](EntityId,CustomerId,Active,ENTFieldId,UserTypeId) Values (1,NULL,1,12,4)

-----------------------------------Ent Page Field Insertion Ends----------------------------------

-----------------------------ALD-2120 Add user type logic in field visibility Ends-------------------


-- ALD-2128 Invoice Data access start here -- 

CREATE TABLE [dbo].[INV_DA_Transaction]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[StaffId] INT NOT NULL,
	[EntityId] INT,
	[Active] BIT NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
	CONSTRAINT FK_INV_DA_Transaction_StaffId FOREIGN KEY ([StaffId]) REFERENCES [dbo].[HR_Staff](Id),	
	Constraint FK_INV_DA_Transaction_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id),
    CONSTRAINT FK_INV_DA_Transaction_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
    CONSTRAINT FK_INV_DA_Transactions_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
    CONSTRAINT FK_INV_DA_Transaction_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)

CREATE TABLE [dbo].[INV_DA_Customer]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[InvDaId] INT NOT NULL,
	[CustomerId] INT NOT NULL,
	[Active] BIT NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
	CONSTRAINT FK_INV_DA_Customer_InvDaId FOREIGN KEY ([InvDaId]) REFERENCES [dbo].[INV_DA_Transaction](Id),	
	CONSTRAINT FK_INV_DA_Customer_CustomerId FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[CU_Customer](Id),	
    CONSTRAINT FK_INV_DA_Customer_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
    CONSTRAINT FK_INV_DA_Customer_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
    CONSTRAINT FK_INV_DA_Customer_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)

CREATE TABLE [dbo].[INV_DA_InvoiceType]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[InvDaId] INT NOT NULL,
	[InvoiceTypeId] INT NOT NULL,
	[Active] BIT NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
	CONSTRAINT FK_INV_DA_InvoiceType_InvDaId FOREIGN KEY ([InvDaId]) REFERENCES [dbo].[INV_DA_Transaction](Id),	
	CONSTRAINT FK_INV_DA_InvoiceType_InvoiceTypeId FOREIGN KEY ([InvoiceTypeId]) REFERENCES [dbo].[REF_InvoiceType](Id),	
    CONSTRAINT FK_INV_DA_InvoiceType_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
    CONSTRAINT FK_INV_DA_InvoiceType_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
    CONSTRAINT FK_INV_DA_InvoiceType_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)

CREATE TABLE [dbo].[INV_DA_office]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[InvDaId] INT NOT NULL,
	[OfficeId] INT NOT NULL,
	[Active] BIT NOT NULL, 
    [CreatedBy] INT NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    [UpdatedBy] INT NULL, 
    [UpdatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
	CONSTRAINT FK_INV_DA_office_InvDaId FOREIGN KEY ([InvDaId]) REFERENCES [dbo].[INV_DA_Transaction](Id),	
	CONSTRAINT FK_INV_DA_office_OfficeId FOREIGN KEY ([OfficeId]) REFERENCES [dbo].[REF_Location](Id),	
    CONSTRAINT FK_INV_DA_office_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
    CONSTRAINT FK_INV_DA_office_DeletedBy FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
    CONSTRAINT FK_INV_DA_office_UpdatedBy FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id)
)

--ALD-2128 Invoice Data access end here -- 
-----------------------------ALD-2120 Add user type logic in field visibility Ends-------------------


---------------------------- ALD-2087 Add CU Product Picture File Type start--------------------------------
Insert Into CU_Product_FileType (Name,Sort,Active) Values ('Pictures',1,1)
---------------------------- ALD-2087 Add CU Product Picture File Type end--------------------------------

-----------------------------ALD-2134 Add Credit Term in SU_Supplier_Customer start--------------------
Alter Table SU_Supplier_Customer Add Credit_Term int 
Alter Table SU_Supplier_Customer Add Constraint FK_SU_Supplier_Customer_Credit_Term FOREIGN KEY (Credit_Term) REFERENCES SU_CreditTerm(Id)
-----------------------------ALD-2134 Add Credit Term in SU_Supplier_Customer end--------------------

---------------------------------- ALD-2135 zip the attachments and download in booking start ---------------------------------

CREATE TABLE [dbo].[INSP_REF_FileAttachment_Category]
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(50),
	Sort INT,
	Active BIT
)

INSERT INTO [dbo].[INSP_REF_FileAttachment_Category](Name,Sort,Active) Values ('General',1,1)

select * from INSP_REF_FileAttachment_Category

CREATE TABLE [dbo].[INSP_TRAN_File_Attachment_zip]
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	UniqueId NVARCHAR(MAX),
	InspectionId INT,
	FileAttachmentCategoryId INT,
	FileName NVARCHAR(500),
	FileUrl NVARCHAR(500),
	CreatedBy INT,
	CreatedOn DATETIME,
	DeletedBy INT,
	DeletedOn DATETIME,
	Active BIT,
	CONSTRAINT FK_INSP_TRAN_FILE_ATTACH_ZIP_INSPECTION_ID FOREIGN KEY(InspectionId) REFERENCES [dbo].[INSP_TRANSACTION](Id),
	CONSTRAINT FK_INSP_TRAN_FILE_ATTACH_CATEGORY_ID FOREIGN KEY(FileAttachmentCategoryId) REFERENCES [dbo].[INSP_REF_FileAttachment_Category](Id),
	CONSTRAINT FK_INSP_TRAN_FILE_ATTACH_ZIP_CREATED_BY FOREIGN KEY(CreatedBy) REFERENCES [dbo].[IT_UserMaster](Id),
	CONSTRAINT FK_INSP_TRAN_FILE_ATTACH_ZIP_DELETED_BY FOREIGN KEY(DeletedBy) REFERENCES [dbo].[IT_UserMaster](Id)
)

ALTER TABLE INSP_TRAN_File_Attachment ADD ZipStatus INT

ALTER TABLE INSP_TRAN_File_Attachment ADD ZipTryCount INT

ALTER TABLE INSP_TRAN_File_Attachment ADD FileAttachment_CategoryId INT

ALTER TABLE INSP_TRAN_File_Attachment ADD CONSTRAINT FK_INSP_TRAN_FILE_ATTACHMENT_CATEGORY_ID FOREIGN KEY(FileAttachment_CategoryId) REFERENCES [dbo].[INSP_REF_FileAttachment_Category](Id)

---------------------------------- ALD-2135 zip the attachments and download in booking end ---------------------------------


ALTER TABLE [dbo].[CU_ServiceType] ADD IgnoreAcceptanceLevel BIT

------------------------- ALD-2246 Data Management not loading by Entity Starts -------------------------
ALTER TABLE [DM_File] ALTER COLUMN [DMDetailsId] INT NOT NULL

ALTER TABLE DM_File ADD EntityId INT

ALTER TABLE DM_File ADD CONSTRAINT FK_DM_ENTITY_ID FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_ENTITY](Id)

------------------------- ALD-2246 Data Management not loading by Entity Ends -------------------------

--------V2_FastReport_100 Report Template stat-----------------
--1
CREATE TABLE [dbo].[REP_FAST_Template](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Active] [bit] NULL,
	[Sort] [int] NULL,
	[EntityId] [int] NULL,
 CONSTRAINT [PK_REP_FAST_Template] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[REP_FAST_Template] ON 
GO
INSERT [dbo].[REP_FAST_Template] ([Id], [Name], [Active], [Sort], [EntityId]) VALUES (1, N'StandardReport_Final.frx', 1, 1, 2)
GO
INSERT [dbo].[REP_FAST_Template] ([Id], [Name], [Active], [Sort], [EntityId]) VALUES (2, N'StandardReport_Inline.frx', 1, 2, 2)
GO
INSERT [dbo].[REP_FAST_Template] ([Id], [Name], [Active], [Sort], [EntityId]) VALUES (3, N'StandardReport_100.frx', 1, 3, 2)
GO
INSERT [dbo].[REP_FAST_Template] ([Id], [Name], [Active], [Sort], [EntityId]) VALUES (4, N'Simplified_StandardReport_Final.frx', 1, 4, 2)
GO
INSERT [dbo].[REP_FAST_Template] ([Id], [Name], [Active], [Sort], [EntityId]) VALUES (5, N'Simplified_StandardReport_Inline.frx', 1, 5, 2)
GO
INSERT [dbo].[REP_FAST_Template] ([Id], [Name], [Active], [Sort], [EntityId]) VALUES (6, N'Simplified_StandardReport_Final.frx', 1, 6, 2)
GO
SET IDENTITY_INSERT [dbo].[REP_FAST_Template] OFF
GO





--2
CREATE TABLE [dbo].[REP_REF_ToolType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ToolName] [nvarchar](50) NULL,
	[Active] [bit] NULL,
	[Sort] [int] NULL,
 CONSTRAINT [PK_REP_REF_ToolType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[REP_REF_ToolType] ON 
GO
INSERT [dbo].[REP_REF_ToolType] ([Id], [ToolName], [Active], [Sort]) VALUES (1, N'FastReport', 1, 1)
GO
INSERT [dbo].[REP_REF_ToolType] ([Id], [ToolName], [Active], [Sort]) VALUES (2, N'Aspose', 1, 2)
GO
SET IDENTITY_INSERT [dbo].[REP_REF_ToolType] OFF
GO
ALTER TABLE [dbo].[REP_REF_ToolType] ADD  CONSTRAINT [DF_REP_REF_ToolType_Active]  DEFAULT ((1)) FOR [Active]
GO

--3 
CREATE TABLE [dbo].[REF_File_Extension](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ExtensionName] [nvarchar](50) NULL,
	[Active] [bit] NULL,
	[Sort] [int] NULL,
 CONSTRAINT [PK_REF_File_Extension] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[REF_File_Extension] ON 
GO
INSERT [dbo].[REF_File_Extension] ([Id], [ExtensionName], [Active], [Sort]) VALUES (1, N'pdf', 1, 1)
GO
INSERT [dbo].[REF_File_Extension] ([Id], [ExtensionName], [Active], [Sort]) VALUES (2, N'xlsx', 1, 2)
GO
INSERT [dbo].[REF_File_Extension] ([Id], [ExtensionName], [Active], [Sort]) VALUES (3, N'docx', 1, 3)
GO
INSERT [dbo].[REF_File_Extension] ([Id], [ExtensionName], [Active], [Sort]) VALUES (4, N'pptx', 1, 4)
GO
INSERT [dbo].[REF_File_Extension] ([Id], [ExtensionName], [Active], [Sort]) VALUES (5, N'xls', 1, 5)
GO
INSERT [dbo].[REF_File_Extension] ([Id], [ExtensionName], [Active], [Sort]) VALUES (6, N'xlsm', 1, 7)
GO
INSERT [dbo].[REF_File_Extension] ([Id], [ExtensionName], [Active], [Sort]) VALUES (7, N'docm', 1, 8)
GO
INSERT [dbo].[REF_File_Extension] ([Id], [ExtensionName], [Active], [Sort]) VALUES (8, N'doc', 1, 9)
GO
INSERT [dbo].[REF_File_Extension] ([Id], [ExtensionName], [Active], [Sort]) VALUES (9, N'ppt', 1, 10)
GO
INSERT [dbo].[REF_File_Extension] ([Id], [ExtensionName], [Active], [Sort]) VALUES (10, N'pptm', 1, 11)
GO
SET IDENTITY_INSERT [dbo].[REF_File_Extension] OFF
GO
ALTER TABLE [dbo].[REF_File_Extension] ADD  CONSTRAINT [DF_REF_File_Extension_Active]  DEFAULT ((1)) FOR [Active]
GO



--4 
CREATE TABLE [dbo].[REP_FAST_Template_Config](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[TemplateId] [int] NULL,
	[ServiceTypeId] [int] NULL,
	[ProductCategoryId] [int] NULL,
	[IsStandardTemplate] [bit] NULL,
	[ScheduleFromDate] [datetime] NULL,
	[ScheduleToDate] [datetime] NULL,
	[Active] [bit] NULL,
	[Sort] [int] NULL,
	[BrandId] [int] NULL,
	[DepartId] [int] NULL,
	[FileExtensionID] [int] NULL,
	[ReportToolTypeID] [int] NULL,
	[Entityid] [int] NULL,
 CONSTRAINT [PK_REP_FAST_Template_Config] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[REP_FAST_Template_Config] ON 
GO
INSERT [dbo].[REP_FAST_Template_Config] ([Id], [CustomerId], [TemplateId], [ServiceTypeId], [ProductCategoryId], [IsStandardTemplate], [ScheduleFromDate], [ScheduleToDate], [Active], [Sort], [BrandId], [DepartId], [FileExtensionID], [ReportToolTypeID], [Entityid]) VALUES (1, NULL, 1, 48, NULL, 1, CAST(N'2022-07-01T00:00:00.000' AS DateTime), CAST(N'2122-07-01T00:00:00.000' AS DateTime), 1, 1, NULL, NULL, 1, 1, 2)
GO
INSERT [dbo].[REP_FAST_Template_Config] ([Id], [CustomerId], [TemplateId], [ServiceTypeId], [ProductCategoryId], [IsStandardTemplate], [ScheduleFromDate], [ScheduleToDate], [Active], [Sort], [BrandId], [DepartId], [FileExtensionID], [ReportToolTypeID], [Entityid]) VALUES (2, NULL, 1, 51, NULL, 1, CAST(N'2022-07-01T00:00:00.000' AS DateTime), CAST(N'2122-07-01T00:00:00.000' AS DateTime), 1, 2, NULL, NULL, 1, 1, 2)
GO
INSERT [dbo].[REP_FAST_Template_Config] ([Id], [CustomerId], [TemplateId], [ServiceTypeId], [ProductCategoryId], [IsStandardTemplate], [ScheduleFromDate], [ScheduleToDate], [Active], [Sort], [BrandId], [DepartId], [FileExtensionID], [ReportToolTypeID], [Entityid]) VALUES (3, NULL, 2, 49, NULL, 1, CAST(N'2022-07-01T00:00:00.000' AS DateTime), CAST(N'2122-07-01T00:00:00.000' AS DateTime), 1, 3, NULL, NULL, 1, 1, 2)
GO
INSERT [dbo].[REP_FAST_Template_Config] ([Id], [CustomerId], [TemplateId], [ServiceTypeId], [ProductCategoryId], [IsStandardTemplate], [ScheduleFromDate], [ScheduleToDate], [Active], [Sort], [BrandId], [DepartId], [FileExtensionID], [ReportToolTypeID], [Entityid]) VALUES (4, NULL, 3, 50, NULL, 1, CAST(N'2022-07-01T00:00:00.000' AS DateTime), CAST(N'2122-07-01T00:00:00.000' AS DateTime), 1, 4, NULL, NULL, 1, 1, 2)
GO
INSERT [dbo].[REP_FAST_Template_Config] ([Id], [CustomerId], [TemplateId], [ServiceTypeId], [ProductCategoryId], [IsStandardTemplate], [ScheduleFromDate], [ScheduleToDate], [Active], [Sort], [BrandId], [DepartId], [FileExtensionID], [ReportToolTypeID], [Entityid]) VALUES (5, NULL, 7, NULL, 21, 1, CAST(N'2022-07-01T00:00:00.000' AS DateTime), CAST(N'2122-07-01T00:00:00.000' AS DateTime), 1, 5, NULL, NULL, 1, 1, 2)
GO
INSERT [dbo].[REP_FAST_Template_Config] ([Id], [CustomerId], [TemplateId], [ServiceTypeId], [ProductCategoryId], [IsStandardTemplate], [ScheduleFromDate], [ScheduleToDate], [Active], [Sort], [BrandId], [DepartId], [FileExtensionID], [ReportToolTypeID], [Entityid]) VALUES (6, 378, 4, 48, NULL, 0, CAST(N'2022-07-01T00:00:00.000' AS DateTime), CAST(N'2122-07-01T00:00:00.000' AS DateTime), 1, 6, NULL, NULL, 1, 1, 2)
GO
INSERT [dbo].[REP_FAST_Template_Config] ([Id], [CustomerId], [TemplateId], [ServiceTypeId], [ProductCategoryId], [IsStandardTemplate], [ScheduleFromDate], [ScheduleToDate], [Active], [Sort], [BrandId], [DepartId], [FileExtensionID], [ReportToolTypeID], [Entityid]) VALUES (7, 378, 4, 51, NULL, 0, CAST(N'2022-07-01T00:00:00.000' AS DateTime), CAST(N'2122-07-01T00:00:00.000' AS DateTime), 1, 7, NULL, NULL, 1, 1, 2)
GO
INSERT [dbo].[REP_FAST_Template_Config] ([Id], [CustomerId], [TemplateId], [ServiceTypeId], [ProductCategoryId], [IsStandardTemplate], [ScheduleFromDate], [ScheduleToDate], [Active], [Sort], [BrandId], [DepartId], [FileExtensionID], [ReportToolTypeID], [Entityid]) VALUES (8, 378, 5, 49, NULL, 0, CAST(N'2022-07-01T00:00:00.000' AS DateTime), CAST(N'2122-07-01T00:00:00.000' AS DateTime), 1, 8, NULL, NULL, 1, 1, 2)
GO
INSERT [dbo].[REP_FAST_Template_Config] ([Id], [CustomerId], [TemplateId], [ServiceTypeId], [ProductCategoryId], [IsStandardTemplate], [ScheduleFromDate], [ScheduleToDate], [Active], [Sort], [BrandId], [DepartId], [FileExtensionID], [ReportToolTypeID], [Entityid]) VALUES (9, 378, 6, 50, NULL, 0, CAST(N'2022-07-01T00:00:00.000' AS DateTime), CAST(N'2122-07-01T00:00:00.000' AS DateTime), 1, 9, NULL, NULL, 1, 1, 2)
GO
INSERT [dbo].[REP_FAST_Template_Config] ([Id], [CustomerId], [TemplateId], [ServiceTypeId], [ProductCategoryId], [IsStandardTemplate], [ScheduleFromDate], [ScheduleToDate], [Active], [Sort], [BrandId], [DepartId], [FileExtensionID], [ReportToolTypeID], [Entityid]) VALUES (10, 379, 4, 48, NULL, 0, CAST(N'2022-07-01T00:00:00.000' AS DateTime), CAST(N'2122-07-01T00:00:00.000' AS DateTime), 1, 10, NULL, NULL, 1, 1, 2)
GO
INSERT [dbo].[REP_FAST_Template_Config] ([Id], [CustomerId], [TemplateId], [ServiceTypeId], [ProductCategoryId], [IsStandardTemplate], [ScheduleFromDate], [ScheduleToDate], [Active], [Sort], [BrandId], [DepartId], [FileExtensionID], [ReportToolTypeID], [Entityid]) VALUES (11, 379, 4, 51, NULL, 0, CAST(N'2022-07-01T00:00:00.000' AS DateTime), CAST(N'2122-07-01T00:00:00.000' AS DateTime), 1, 11, NULL, NULL, 1, 1, 2)
GO
INSERT [dbo].[REP_FAST_Template_Config] ([Id], [CustomerId], [TemplateId], [ServiceTypeId], [ProductCategoryId], [IsStandardTemplate], [ScheduleFromDate], [ScheduleToDate], [Active], [Sort], [BrandId], [DepartId], [FileExtensionID], [ReportToolTypeID], [Entityid]) VALUES (12, 379, 5, 49, NULL, 0, CAST(N'2022-07-01T00:00:00.000' AS DateTime), CAST(N'2122-07-01T00:00:00.000' AS DateTime), 1, 12, NULL, NULL, 1, 1, 2)
GO
INSERT [dbo].[REP_FAST_Template_Config] ([Id], [CustomerId], [TemplateId], [ServiceTypeId], [ProductCategoryId], [IsStandardTemplate], [ScheduleFromDate], [ScheduleToDate], [Active], [Sort], [BrandId], [DepartId], [FileExtensionID], [ReportToolTypeID], [Entityid]) VALUES (13, 379, 6, 50, NULL, 0, CAST(N'2022-07-01T00:00:00.000' AS DateTime), CAST(N'2122-07-01T00:00:00.000' AS DateTime), 1, 13, NULL, NULL, 1, 1, 2)
GO
SET IDENTITY_INSERT [dbo].[REP_FAST_Template_Config] OFF
GO
ALTER TABLE [dbo].[REP_FAST_Template_Config] ADD  CONSTRAINT [DF_REP_FAST_Template_Config_IsStandardTemplate]  DEFAULT ((1)) FOR [IsStandardTemplate]
GO
ALTER TABLE [dbo].[REP_FAST_Template_Config] ADD  CONSTRAINT [DF_REP_FAST_Template_Config_Active]  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[REP_FAST_Template_Config] ADD  CONSTRAINT [DF_REP_FAST_Template_Config_ReportToolType]  DEFAULT ((1)) FOR [ReportToolTypeID]
GO
ALTER TABLE [dbo].[REP_FAST_Template_Config]  WITH CHECK ADD  CONSTRAINT [FK_REP_FAST_Template_Config_AP_Entity] FOREIGN KEY([Entityid])
REFERENCES [dbo].[AP_Entity] ([Id])
GO
ALTER TABLE [dbo].[REP_FAST_Template_Config] CHECK CONSTRAINT [FK_REP_FAST_Template_Config_AP_Entity]
GO
ALTER TABLE [dbo].[REP_FAST_Template_Config]  WITH CHECK ADD  CONSTRAINT [FK_REP_FAST_Template_Config_CU_Brand] FOREIGN KEY([BrandId])
REFERENCES [dbo].[CU_Brand] ([Id])
GO
ALTER TABLE [dbo].[REP_FAST_Template_Config] CHECK CONSTRAINT [FK_REP_FAST_Template_Config_CU_Brand]
GO
ALTER TABLE [dbo].[REP_FAST_Template_Config]  WITH CHECK ADD  CONSTRAINT [FK_REP_FAST_Template_Config_CU_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[CU_Customer] ([Id])
GO
ALTER TABLE [dbo].[REP_FAST_Template_Config] CHECK CONSTRAINT [FK_REP_FAST_Template_Config_CU_Customer]
GO
ALTER TABLE [dbo].[REP_FAST_Template_Config]  WITH CHECK ADD  CONSTRAINT [FK_REP_FAST_Template_Config_CU_Department] FOREIGN KEY([DepartId])
REFERENCES [dbo].[CU_Department] ([Id])
GO
ALTER TABLE [dbo].[REP_FAST_Template_Config] CHECK CONSTRAINT [FK_REP_FAST_Template_Config_CU_Department]
GO
ALTER TABLE [dbo].[REP_FAST_Template_Config]  WITH CHECK ADD  CONSTRAINT [FK_REP_FAST_Template_Config_REF_File_Extension] FOREIGN KEY([FileExtensionID])
REFERENCES [dbo].[REF_File_Extension] ([Id])
GO
ALTER TABLE [dbo].[REP_FAST_Template_Config] CHECK CONSTRAINT [FK_REP_FAST_Template_Config_REF_File_Extension]
GO
ALTER TABLE [dbo].[REP_FAST_Template_Config]  WITH CHECK ADD  CONSTRAINT [FK_REP_FAST_Template_Config_REF_ProductCategory] FOREIGN KEY([ProductCategoryId])
REFERENCES [dbo].[REF_ProductCategory] ([Id])
GO
ALTER TABLE [dbo].[REP_FAST_Template_Config] CHECK CONSTRAINT [FK_REP_FAST_Template_Config_REF_ProductCategory]
GO
ALTER TABLE [dbo].[REP_FAST_Template_Config]  WITH CHECK ADD  CONSTRAINT [FK_REP_FAST_Template_Config_REF_ServiceType] FOREIGN KEY([ServiceTypeId])
REFERENCES [dbo].[REF_ServiceType] ([Id])
GO
ALTER TABLE [dbo].[REP_FAST_Template_Config] CHECK CONSTRAINT [FK_REP_FAST_Template_Config_REF_ServiceType]
GO
ALTER TABLE [dbo].[REP_FAST_Template_Config]  WITH CHECK ADD  CONSTRAINT [FK_REP_FAST_Template_Config_REP_FAST_Template] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[REP_FAST_Template] ([Id])
GO
ALTER TABLE [dbo].[REP_FAST_Template_Config] CHECK CONSTRAINT [FK_REP_FAST_Template_Config_REP_FAST_Template]
GO
ALTER TABLE [dbo].[REP_FAST_Template_Config]  WITH CHECK ADD  CONSTRAINT [FK_REP_FAST_Template_Config_REP_REF_ToolType] FOREIGN KEY([ReportToolTypeID])
REFERENCES [dbo].[REP_REF_ToolType] ([Id])
GO
ALTER TABLE [dbo].[REP_FAST_Template_Config] CHECK CONSTRAINT [FK_REP_FAST_Template_Config_REP_REF_ToolType]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 pdf, 2 excel , 3 word ,4 ppt' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REP_FAST_Template_Config', @level2type=N'COLUMN',@level2name=N'FileExtensionID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 FastReport , 2 Aspose' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REP_FAST_Template_Config', @level2type=N'COLUMN',@level2name=N'ReportToolTypeID'
GO

--------V2_FastReport_100 Report Template end-----------------
------------------------- ALD-2246 Data Management not loading by Entity Ends -------------------------

-----------------------------ALD-2134 Add Credit Term in SU_Supplier_Customer end--------------------


-----------------------------ALD-2196 Enhancement in Booking Starts--------------------------------------

update ENT_Master_Config set Value='/Documents/Insp_Terms/API_ConfirmTermsEg28092021.pdf' where id=2

update ENT_Master_Config set Value='/Documents/Insp_Terms/API_ConfirmTermsCn28092021.pdf' where id=3

Insert into ENT_Master_Type(Name,Active,Sort) Values ('Inspection Confirm Booking Footer',1,14)

Insert into ENT_Master_Type(Name,Active,Sort) Values ('Inspection Reschedule Booking Footer',1,15)

Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (14,1,NULL,'<div class="div-style">Special Note as below:</div><br /><div class="div-style">1. Pls note this is API official confirmation to your booking. pls ready production on time for inspection.API will not specify separately but will allocate appropriate number of Quality Inspectors on each service day according to product and total items in booking, pls contact API if you have any particular request on this.</div><div class="div-style">2. Pls double check all booking details in this email and advise us if any question upon this confirm email received.</div><div class="div-style">3. If NO any feedback or disagreement received from supplier/factory within 12:00 noon time China time 1 working day before inspection date mentioned as above, it will be treated all info in this email are confirmed and agreed, API will move ahead to assign inspector as schedule accordingly.<a href="{docpath}">More</a></div>',1)

Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (14,2,NULL,'<div class="div-style">Special Note as below:</div><br /><div class="div-style">1. Pls note this is SGT official confirmation to your booking. pls ready production on time for inspection.SGT will not specify separately but will allocate appropriate number of Quality Inspectors on each service day according to product and total items in booking, pls contact SGT if you have any particular request on this.</div><div class="div-style">2. Pls double check all booking details in this email and advise us if any question upon this confirm email received.</div><div class="div-style">3. If NO any feedback or disagreement received from supplier/factory within 12:00 noon time China time 1 working day before inspection date mentioned as above, it will be treated all info in this email are confirmed and agreed, SGT will move ahead to assign inspector as schedule accordingly.<a href="{docpath}">More</a></div>',1)

Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (14,1,38,N'<div class="div-style"> 温馨提示与声明： </div><br /><br /><div class="div-style">1. 本邮件是对贵司预定申请的确认邮件，工厂应按预定日期备货检验。API 将依据申请上的产品类别和款数等安排每天到厂的检验员人数、不做另行通知，如有特殊要求请及时联系 API 。</div> <div class="div-style">2. 请贵司仔细地复核本邮件中的相关内容。</div> <div class="div-style">3. 若截止上述验货日期前1个工作日的12时（中国北京时间）未收到贵司邮件回复与异议即代表贵司确认本邮件中的相关内容正确无误，API 将相应安排人员验货。 若需修改相关内容，请按以下要求及规定执行。<a href="{docpath}">更多信息</a></div>',1)

Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (14,2,38,N'<div class="div-style"> 温馨提示与声明： </div><br /><br /><div class="div-style">1. 本邮件是对贵司预定申请的确认邮件，工厂应按预定日期备货检验。SGT 将依据申请上的产品类别和款数等安排每天到厂的检验员人数、不做另行通知，如有特殊要求请及时联系 SGT 。</div> <div class="div-style">2. 请贵司仔细地复核本邮件中的相关内容。</div> <div class="div-style">3. 若截止上述验货日期前1个工作日的12时（中国北京时间）未收到贵司邮件回复与异议即代表贵司确认本邮件中的相关内容正确无误，SGT 将相应安排人员验货。 若需修改相关内容，请按以下要求及规定执行。<a href="{docpath}">更多信息</a></div>',1)

Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (15,1,NULL,'<div class="div-style"><b>Special Note: -</b></div><div class="div-style">a) Pls note this is the official confirmation to your booking.<div class="div-style"> - No need supplier/factory to confirm API again if all info in this email are correct but shall ready production on time for inspection; </div><div class="div-style"> - Pls inform API following the process below mentioned if any modification needed;</div></div><div class="div-style">b) Factory has to ensure production with 100% finished and at least 80% packed at 9:00am before API QC arrived at factory on service date agreed, unless customer has special agreement on this;</div><div class="div-style">c) Factory has to prepay API the inspection cost (inspection fee and traveling expense) on 2nd inspection for same order, because 1st inspection has to be aborted and missing inspection due to factory good not ready when QC arrived (or supplier late advise on cancel inspection while inspector is on the way to factory);</div> <div class="div-style">d) Factory Support needed on inspection equipment and inspection cooperating:<div class="div-style"> - Ensure all necessary inspection equipment working well and calibrated in valid before API inspector arrived; </div><div class="div-style"> - Ensure a special/separate area for API inspection with sufficient lighting and enough factory worker for cartons moving, unpacking and packing during inspection. </div></div><div class="div-style">e) Any wrong info given on booking will affect our quotation and inspection report, as well as surcharge applied due to incorrect info. If any modification needed pls inform API team by written email before 3:00pm local time by 3 days prior to agreed service date. Surcharge as below on modification done due to supplier/factory reason:<div class="div-style"> - If booking was revised more than 2 times (counting from booking receive date), starting from 3rd time we will charge USD$25/RMB$175 each time for revision.</div><div class="div-style"> - If any revision made less than 3 working days and that changes affect the quotation, we will charge USD$50/RMB$350 for revision. </div></div><div class="div-style">f) In case of postponement or cancellation on inspection, pls inform API local office by written email before 3:00pm local time with 24 working hours prior to the confirmed inspection date (excluding regular rest days and public holidays), surcharges of affecting resources deployment(Man-day lost and traveling expense if occurred) will be applied to booking applicant - supplier/factory if late advise on cancellation or postponement;</div><div class="div-style">g) Prepayment needed for Non-program customer, 2nd inspection due to goods not ready or re-inspection:<div class="div-style"> - “Bill to party” shall settle the full payment no later than 16:00 local time on the working day before service date, or else inspection will be on hold until invoice amount received.</div><div class="div-style"> - Please send us copy of the bank receipt with “API Inspection Number” & “Invoice Number” marked by email ASAP, API will arrange inspection only once payment received.</div></div><div class="div-style">Thanks for your cooperation!</div>',1)

Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (15,2,NULL,'<div class="div-style"><b>Special Note: -</b></div><div class="div-style">a) Pls note this is the official confirmation to your booking.<div class="div-style"> - No need supplier/factory to confirm SGT again if all info in this email are correct but shall ready production on time for inspection; </div><div class="div-style"> - Pls inform SGT following the process below mentioned if any modification needed;</div></div><div class="div-style">b) Factory has to ensure production with 100% finished and at least 80% packed at 9:00am before SGT QC arrived at factory on service date agreed, unless customer has special agreement on this;</div><div class="div-style">c) Factory has to prepay SGT the inspection cost (inspection fee and traveling expense) on 2nd inspection for same order, because 1st inspection has to be aborted and missing inspection due to factory good not ready when QC arrived (or supplier late advise on cancel inspection while inspector is on the way to factory);</div> <div class="div-style">d) Factory Support needed on inspection equipment and inspection cooperating:<div class="div-style"> - Ensure all necessary inspection equipment working well and calibrated in valid before SGT inspector arrived; </div><div class="div-style"> - Ensure a special/separate area for SGT inspection with sufficient lighting and enough factory worker for cartons moving, unpacking and packing during inspection. </div></div><div class="div-style">e) Any wrong info given on booking will affect our quotation and inspection report, as well as surcharge applied due to incorrect info. If any modification needed pls inform SGT team by written email before 3:00pm local time by 3 days prior to agreed service date. Surcharge as below on modification done due to supplier/factory reason:<div class="div-style"> - If booking was revised more than 2 times (counting from booking receive date), starting from 3rd time we will charge USD$25/RMB$175 each time for revision.</div><div class="div-style"> - If any revision made less than 3 working days and that changes affect the quotation, we will charge USD$50/RMB$350 for revision. </div></div><div class="div-style">f) In case of postponement or cancellation on inspection, pls inform SGT local office by written email before 3:00pm local time with 24 working hours prior to the confirmed inspection date (excluding regular rest days and public holidays), surcharges of affecting resources deployment(Man-day lost and traveling expense if occurred) will be applied to booking applicant - supplier/factory if late advise on cancellation or postponement;</div><div class="div-style">g) Prepayment needed for Non-program customer, 2nd inspection due to goods not ready or re-inspection:<div class="div-style"> - “Bill to party” shall settle the full payment no later than 16:00 local time on the working day before service date, or else inspection will be on hold until invoice amount received.</div><div class="div-style"> - Please send us copy of the bank receipt with “SGT Inspection Number” & “Invoice Number” marked by email ASAP, SGT will arrange inspection only once payment received.</div></div><div class="div-style">Thanks for your cooperation!</div>',1)

Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (15,2,38,N'<div class="div-style"><b> 温馨提示与声明：</b></div><div class="div-style">a)	此邮件是 API 对您验货申请的正式验货确认。</div><div class="div-style">-	如果此邮件内所有信息正确，则供应商/工厂无需再确认回复、但应按下述要求提前做好检验准备；</div><div class="div-style">-	如需任何更改请参考以下提示；</div><div class="div-style">b)	除非买家有特定的要求，生产工厂必须确保货物在上述检验日期当天上午9点前：100%成品，至少80%完成装箱;</div><div class="div-style">c)	如 API 检验员到厂(或已在途中接到通知取消验货将继续前往工厂)，货物未按要求备好当天验货放空出missing报告，再次验货费用(=检验费+交通费)全额由工厂预付；</div><div class="div-style">d)	检测设备与检验过程协助：</div><div class="div-style">-	在 API 检验员到达之前，工厂应确保所有必要的检测设备是在校准有效期内且能被正常操作使用。</div><div class="div-style">-	请工厂为 API 检验员提供一个光线充足且宽敞的检验场地，并安排足够的人员协助检验，包括搬运/装卸外箱，拆封包装等。</div><div class="div-style">e)	任何检验信息的缺失或错误（如检验日期、检验类型、工厂地址、订单号码和订单数量等）将影响原订人员安排以及验货报告，并有可能产生额外费用。若申请人在此确认邮件中发现任何信息与实际情况不符，为避免产生额外费用请在上述确定的检验日期前(间隔)3个工作日的下午3点前发邮件通知 API 当地办事处。因申请人原因修改验货申请产生的费用如下：</div><div class="div-style">-	若修改信息超过2次（自检验申请收到之日起计算），从第3次修改起我们将会收取每次25美元/175元人民币的手续费。</div><div class="div-style">-	任何不足3个工作日通知的信息修改，且修改将影响检验报价的，我们将会收取每次50美元/350元人民币的手续费。</div><div class="div-style">f)	若需取消或改期检验，务必需提前提前1个工作日=间隔1个工作日（周六日和法定假日除外）的下午3点前发邮件通知 API 。 如不足通知期(=过迟)取消或改期验货，验货申请人需承担由此产生的人力损失和相关费用。</div><div class="div-style">g)	非合约客户或重验/再次服务的预付款要求：</div><div class="div-style">-	付款方务必在确定的检验日期前一个工作日(当地时间16:00前)将检验费用付至 API，否则检验将被暂时搁置，直到付清检验款项。</div><div class="div-style">-	请尽快邮件发送银行付款凭据（标注“ API 检验号码”和“发票号码”）给 API 联络人。我司确认收到款项后，将为您安排检验。</div><div class="div-style">感谢您的支持与合作！</div>',1)

Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (15,2,NULL,'<div class="div-style"><b> 温馨提示与声明：</b></div><div class="div-style">a)	此邮件是 SGT 对您验货申请的正式验货确认。</div><div class="div-style">-	如果此邮件内所有信息正确，则供应商/工厂无需再确认回复、但应按下述要求提前做好检验准备；</div><div class="div-style">-	如需任何更改请参考以下提示；</div><div class="div-style">b)	除非买家有特定的要求，生产工厂必须确保货物在上述检验日期当天上午9点前：100%成品，至少80%完成装箱;</div><div class="div-style">c)	如 SGT 检验员到厂(或已在途中接到通知取消验货将继续前往工厂)，货物未按要求备好当天验货放空出missing报告，再次验货费用(=检验费+交通费)全额由工厂预付；</div><div class="div-style">d)	检测设备与检验过程协助：</div><div class="div-style">-	在 SGT 检验员到达之前，工厂应确保所有必要的检测设备是在校准有效期内且能被正常操作使用。</div><div class="div-style">-	请工厂为 SGT 检验员提供一个光线充足且宽敞的检验场地，并安排足够的人员协助检验，包括搬运/装卸外箱，拆封包装等。</div><div class="div-style">e)	任何检验信息的缺失或错误（如检验日期、检验类型、工厂地址、订单号码和订单数量等）将影响原订人员安排以及验货报告，并有可能产生额外费用。若申请人在此确认邮件中发现任何信息与实际情况不符，为避免产生额外费用请在上述确定的检验日期前(间隔)3个工作日的下午3点前发邮件通知 SGT 当地办事处。因申请人原因修改验货申请产生的费用如下：</div><div class="div-style">-	若修改信息超过2次（自检验申请收到之日起计算），从第3次修改起我们将会收取每次25美元/175元人民币的手续费。</div><div class="div-style">-	任何不足3个工作日通知的信息修改，且修改将影响检验报价的，我们将会收取每次50美元/350元人民币的手续费。</div><div class="div-style">f)	若需取消或改期检验，务必需提前提前1个工作日=间隔1个工作日（周六日和法定假日除外）的下午3点前发邮件通知 SGT 。 如不足通知期(=过迟)取消或改期验货，验货申请人需承担由此产生的人力损失和相关费用。</div><div class="div-style">g)	非合约客户或重验/再次服务的预付款要求：</div><div class="div-style">-	付款方务必在确定的检验日期前一个工作日(当地时间16:00前)将检验费用付至 SGT，否则检验将被暂时搁置，直到付清检验款项。</div><div class="div-style">-	请尽快邮件发送银行付款凭据（标注“ SGT 检验号码”和“发票号码”）给 SGT 联络人。我司确认收到款项后，将为您安排检验。</div><div class="div-style">感谢您的支持与合作！</div>',1)



-----------------------------ALD-2196 Enhancement in Booking Ends--------------------------------------


-- Schedule job configuration start here ---



CREATE TABLE [dbo].[JOB_Schedule_Job_Type]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] [nvarchar](200) NOT NULL, 
    [Active] BIT NOT NULL, 
    [Sort] INT NULL
)

INSERT INTO JOB_Schedule_Job_Type (NAME,Active,Sort)VALUES('ScheduleQcEmail',1,1)
INSERT INTO JOB_Schedule_Job_Type (NAME,Active,Sort)VALUES('ScheduleFbReport',1,1)
INSERT INTO JOB_Schedule_Job_Type (NAME,Active,Sort)VALUES('ScheduleCulturaPackingInfo',1,1)
INSERT INTO JOB_Schedule_Job_Type (NAME,Active,Sort)VALUES('ScheduleTravelTariffEmail',1,1)
INSERT INTO JOB_Schedule_Job_Type (NAME,Active,Sort)VALUES('ScheduleAutoQcExpense',1,1)
INSERT INTO JOB_Schedule_Job_Type (NAME,Active,Sort)VALUES('ScheduleQcInspectionExpenseEmail',1,1)
INSERT INTO JOB_Schedule_Job_Type (NAME,Active,Sort)VALUES('ScheduleClaimReminderEmail',1,1)
INSERT INTO JOB_Schedule_Job_Type (NAME,Active,Sort)VALUES('ScheduleFastReport',1,1)
INSERT INTO JOB_Schedule_Job_Type (NAME,Active,Sort)VALUES('ScheduleCarrefourDailyResult',1,1)

CREATE TABLE [dbo].[JOB_Schedule_Configuration]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Name] NVARCHAR(1000),
	[Type] INT,
	[To] NVARCHAR(1500),
	[CC] NVARCHAR(1500),
	[StartDate] DATETIME NULL,
	[ScheduleInterval] INT,
	[FolderPath] NVARCHAR(1500),
	[FileName] NVARCHAR(1500),
	[EntityId] INT,
	[Active] BIT,
	CONSTRAINT FK_JOB_Schedule_Configuration_Type FOREIGN KEY([Type]) REFERENCES [dbo].[JOB_Schedule_Job_Type],
	CONSTRAINT FK_JOB_Schedule_Configuration_ENTITY_ID FOREIGN KEY([EntityId]) REFERENCES [dbo].[AP_ENTITY],
)

-- Schedule job configuration end here ---



----------------------------ALD-2235 Add INV_MAN_Tran_Tax start--------------------
CREATE TABLE INV_MAN_TRAN_TAX(
Id int PRIMARY KEY NOT NULL IDENTITY(1,1),
Man_InvoiceId INT,
TaxId INT,
CreatedOn DATETIME,
CreatedBy INT,
constraint FK_INV_MAN_TRAN_TAX_Man_Invoiceid FOREIGN KEY (Man_InvoiceId) REFERENCES INV_MAN_Transaction(Id),
constraint FK_INV_MAN_TRAN_TAX_TaxId FOREIGN KEY (TaxId) REFERENCES INV_TRAN_Bank_Tax(Id),
constraint FK_INV_MAN_TRAN_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES IT_UserMaster(Id)
);
----------------------------ALD-2235 Add INV_MAN_Tran_Tax end---------------------------------------


----------------------------ALD-2293 Alter statement for HR_Staff, SU_Supplier, CU_Customer start--------------------

ALTER TABLE HR_Staff ADD [CompanyId] [int] NULL
ALTER TABLE HR_Staff WITH CHECK ADD FOREIGN KEY([CompanyId]) REFERENCES [dbo].[AP_Entity] ([Id])

ALTER TABLE SU_Supplier ADD [CompanyId] [int] NULL
ALTER TABLE SU_Supplier WITH CHECK ADD FOREIGN KEY([CompanyId]) REFERENCES [dbo].[AP_Entity] ([Id])

ALTER TABLE CU_Customer ADD [CompanyId] [int] NULL
ALTER TABLE CU_Customer WITH CHECK ADD FOREIGN KEY([CompanyId]) REFERENCES [dbo].[AP_Entity] ([Id])

----------------------------ALD-2293 Alter statement for HR_Staff, SU_Supplier, CU_Customer end---------------------------------------


---------------------------ALD-2307 Email Rule for Invoice starts---------------------------------------
Insert Into ES_TYPE ([Name],[Active]) Values ('Invoice Send',1)

Insert Into ES_REF_Report_Send_Type (Name,Active,CreatedOn,EntityId) Values ('One Invoice per Email',1,GETDATE(),null)

Insert Into ES_Email_Report_Type_Map (EmailType,ReportType,Active) Values (3,6,1)

Insert Into ES_REF_RecipientType (Name,Active,Created_On) Values 
('Customer Contact',1,GETDATE()),
('Quotation Customer Contact',1,GETDATE()),
('Quotation Supplier Contact',1,GETDATE()),
('Quotation Factory Contact',1,GETDATE()),
('Quotation Internal Contact',1,GETDATE()),
('Invoice Contact',1,GETDATE())

--For the API Entity
Insert Into ES_SU_PreDefined_Fields (Field_Name,Field_Alias_Name,Active,CreatedOn,IsText,DataType,EntityId)
Values 
('Invoice','Invoice #',1,GetDate(),1,null,1),
('FactoryCountry','Factory Country',NULL,GetDate(),1,null,1),
('CustomerDecisionResult','Customer Decision Result',1,GetDate(),1,null,1),
('InvoiceDate','Invoice Date',1,GetDate(),Null,1,1),
('InvoicePostDate','Invoice Post Date',1,GetDate(),Null,1,1)

--For the SGT Entity
Insert Into ES_SU_PreDefined_Fields (Field_Name,Field_Alias_Name,Active,CreatedOn,IsText,DataType,EntityId)
Values 
('Invoice','Invoice #',1,GetDate(),1,null,2),
('FactoryCountry','Factory Country',1,GetDate(),NULL,null,2),
('CustomerDecisionResult','Customer Decision Result',1,GetDate(),1,null,2),
('InvoiceDate','Invoice Date',1,GetDate(),Null,1,2),
('InvoicePostDate','Invoice Post Date',1,GetDate(),Null,1,2)

Update ES_REF_RecipientType Set Name='Customer Booking Contact' Where Id=1
Update ES_REF_RecipientType Set Name='Supplier Booking Contact' Where Id=2
Update ES_REF_RecipientType Set Name='Factory Booking Contact' Where Id=3
Update ES_REF_RecipientType Set Name='Merchandiser Booking Contact' Where Id=5

Create Table ES_RULE_Recipient_EmailType_Map(
Id int PRIMARY KEY IDENTITY (1,1),
EmailTypeId int,
RecipientTypeId int,
Active bit,
CONSTRAINT FK_ES_RULE_Recipient_EmailType_Map_TypeId FOREIGN KEY (EmailTypeId) REFERENCES ES_Type(Id),
CONSTRAINT FK_ES_RULE_Recipient_EmailType_Map_RecipientTypeId FOREIGN KEY (RecipientTypeId) REFERENCES ES_REF_RecipientType(Id)
)

Insert Into ES_RULE_Recipient_EmailType_Map (EmailTypeId,RecipientTypeId,Active)
Values 
(1,1,1),
(1,2,1),
(1,3,1),
(1,4,1),
(1,5,1),
(1,6,1),
(2,1,1),
(2,2,1),
(2,3,1),
(2,4,1),
(2,5,1),
(2,6,1),
(3,1,1),
(3,2,1),
(3,3,1),
(3,4,1),
(3,5,1),
(3,6,1),
(3,7,1),
(3,8,1),
(3,9,1),
(3,10,1),
(3,11,1)
---------------------------ALD-2307 Email Rule for Invoice ends---------------------------------------

---------------------------ALD-2292 Supplier / factory need to access the statistics dashboard Starts -------------------------

ALTER TABLE SU_Supplier_Customer ADD [IsStatisticsVisibility] BIT

---------------------------ALD-2292 Supplier / factory need to access the statistics dashboard ends -------------------------
---------------------------ALD-2307 Email Rule for Invoice ends---------------------------------------


----------------------------ALD-2356 ALTER TABLE REF_KPI_Teamplate DROP COLUMN CustomerId start--------------------

ALTER TABLE REF_KPI_Teamplate DROP COLUMN CustomerId

----------------------------ALD-2356 ALTER TABLE REF_KPI_Teamplate DROP COLUMN CustomerId end--------------------


----------------------------ALD-2356 CREATE TABLE REF_KPI_Teamplate_Customer start--------------------

CREATE TABLE [dbo].[REF_KPI_Teamplate_Customer]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[TeamplateId] [int] NULL,
	[CustomerId] [int] NULL,
	[Active] BIT NOT NULL, 
	[UserTypeId] [int] NULL,
	FOREIGN KEY([TeamplateId]) REFERENCES REF_KPI_Teamplate(Id),
	FOREIGN KEY([CustomerId]) REFERENCES CU_Customer(Id),
	FOREIGN KEY([UserTypeId]) REFERENCES IT_UserType(Id)
)


----------------------------ALD-2356 CREATE TABLE REF_KPI_Teamplate_Customer end---------------------------------------


---------------------------ALD-2347 Email Rule Enhancement starts-------------------------------------
CREATE TABLE ES_REF_Recipient(
Id int not null PRIMARY KEY IDENTITY(1,1),
Sort int,
Name varchar(255),
Active bit
)

Insert into ES_REF_Recipient (Name, Sort,  Active) values ('To', 1, 1);
Insert into ES_REF_Recipient (Name, Sort,  Active) values ('Cc', 2, 1);
Insert into ES_REF_Recipient (Name, Sort,  Active) values ('Bcc', 3, 1);

--ES_Additional Recipients table
Create table ES_AdditionalRecipients(
Id INT  NOT NULL PRIMARY KEY IDENTITY(1,1),
EmailDetailId INT,
AdditionalEmail varchar(255),
Recipient int,
CreatedOn datetime not null,
CreatedBy int,
CONSTRAINT FK_ES_AdditionalRecipients_EmailDetailId FOREIGN KEY (EmailDetailId) REFERENCES ES_Details(Id),
CONSTRAINT FK_ES_AdditionalRecipients_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES IT_UserMaster(Id),
CONSTRAINT FK_ES_AdditionalRecipients_Recipient FOREIGN KEY (Recipient) REFERENCES ES_REF_Recipient(Id)
)

---------------------------ALD-2347 Email Rule Enhancement ends-------------------------------------

---------------ALD-2355 Add email customization in Booking upload documents starts----------

ALTER TABLE INSP_TRAN_File_Attachment ADD [IsbookingEmailNotification] BIT

ALTER TABLE INSP_TRAN_File_Attachment ADD [IsReportSendToFB] BIT 

Update INSP_TRAN_File_Attachment Set [IsFBReport]=1

---------------ALD-2355 Add email customization in Booking upload documents ends----------

---------------ALD-2383 Create Schedule Job for Missing MS chart starts----------

SET IDENTITY_INSERT JOB_Schedule_Job_Type ON 
GO
INSERT INTO JOB_Schedule_Job_Type (Id,name,active,sort) VALUES (11,'ScheduleMissedMSchart',1,1);
GO
SET IDENTITY_INSERT JOB_Schedule_Job_Type OFF

INSERT INTO JOB_Schedule_Configuration ([Name],[Type],[To],[CC],[StartDate],[ScheduleInterval],[EntityId],[Active])
VALUES('Schedule Missed MSchart',11,'sabarimalai_alagar@api-hk.com;nixon.antony@sgtgroup.net','sabarimalai_alagar@api-hk.com;nixon.antony@sgtgroup.net',GETDATE(),1,1,1)


---------------ALD-2383 Create Schedule Job for Missing MS chart ends----------



---------------------------ALD-2441 REF_KPI_Teamplate Starts -------------------------

SET IDENTITY_INSERT REF_KPI_Teamplate ON 
GO
Insert Into REF_KPI_Teamplate (ID,Name,Active,TypeId) values (39,'ECI',1,1)
GO
SET IDENTITY_INSERT REF_KPI_Teamplate OFF
GO

---------------------------ALD-2441 REF_KPI_Teamplate ends -------------------------


---------------------------ALD-2440 REF_KPI_Teamplate Starts -------------------------

SET IDENTITY_INSERT REF_KPI_Teamplate ON 
GO
Insert Into REF_KPI_Teamplate (ID,Name,Active,TypeId) values (40,'Cultura',1,1)
GO
SET IDENTITY_INSERT REF_KPI_Teamplate OFF
GO

---------------------------ALD-2440 REF_KPI_Teamplate ends -------------------------

---------------------------ALD-2492 payroll company Starts -------------------------

CREATE TABLE HR_PayrollCompany(
[Id] INT NOT NULL PRIMARY KEY Identity(1,1),
[CompanyName] NVARCHAR(200),
[Active] BIT NOT NULL,
[Sort] INT,
[AccountEmail] NVARCHAR(1000),
[Entity] INT NOT NULL,
CONSTRAINT FK_HR_Payrol_Entity FOREIGN KEY([Entity]) REFERENCES [dbo].[AP_Entity](Id)
)

ALTER TABLE HR_Staff
ADD PayrollCompany INT

ALTER TABLE HR_Staff
ADD FOREIGN KEY (PayrollCompany) REFERENCES HR_PayrollCompany(Id)

---------------------------ALD-2492 payroll company ends -------------------------


-----------ALD-2532 invoice edit page enhancement start ---------------

INSERT INTO [dbo].[ENT_REF_Features] (Name,Active) VALUES('Add Invoice Bill To in Monthly  invoice number ',1)
INSERT INTO [dbo].[ENT_REF_Features] (Name,Active) VALUES('Add Factory Country in Monthly Invoice number',1)

INSERT INTO [dbo].[ENT_Feature_Details] (FeatureId,EntityId,Active) VALUES(6,2,1)
INSERT INTO [dbo].[ENT_Feature_Details] (FeatureId,EntityId,Active) VALUES(7,2,1)

-----------ALD-2532 invoice edit page enhancement end ---------------

-- start ALD-2463 -- 
ALTER TABLE INV_AUT_TRAN_Details ADD [Additional_BD_Tax] FLOAT NULL
-- end ALD-2463 -- 



--------------------------ALD-2606 Add Color Code and Color name predefined fields starts-------------
--data type 2 is list
Insert Into ES_SU_Predefined_Fields
(Field_Name,Field_Alias_Name,Active,CreatedOn,EntityId,DataType)
Values
('ColorCode','Color Code', 1,GETDATE(), 2,2),
('ColorName','Color Name', 1,GETDATE(), 2,2) 
--------------------------ALD-2606 Add Color Code and Color name predefined fields ends-------------
----------------------- ALD-2563 Fetch the season based on customer Starts-----------------------------------------------

ALTER TABLE INSP_Transaction DROP CONSTRAINT FK__INSP_Tran__Seaso__6D449F23

ALTER TABLE INSP_Transaction DROP COLUMN Season_Id

ALTER TABLE INSP_Transaction ADD Season_Id INT

ALTER TABLE INSP_Transaction ADD CONSTRAINT FK_INSP_SEASON_CONFIG FOREIGN KEY(Season_Id) REFERENCES [dbo].[CU_Season_Config](Id)

----------------------- ALD-2563 Fetch the season based on customer Ends-----------------------------------------------
-----------ALD-2613 Add the piece rate logic in Quotation start ---------------

ALTER TABLE QU_Quotation_Insp ADD Quantity Int NULL
ALTER TABLE QU_Quotation_Insp ADD BilledQtyType Int NULL 
ALTER TABLE QU_Quotation_Insp ADD FOREIGN KEY (BilledQtyType) REFERENCES INSP_REF_QuantityType(Id)

-----------ALD-2613 Add the piece rate logic in Quotation end ---------------


--------------------------ALD-2603 starts-------------
create table INV_AUT_TRAN_Communications
(Id int not null identity(1,1) primary key,
Invoice_Number nvarchar(1000),
Comment nvarchar(2000),
CreatedBy int,
CreatedOn datetime null default getdate(),
Active bit,
EntityId int
CONSTRAINT FK_INV_AUT_TRAN_Communications_Created_By FOREIGN KEY(CreatedBy) REFERENCES [dbo].[it_usermaster](Id),
CONSTRAINT FK_INV_AUT_TRAN_Communications_Entity_Id FOREIGN KEY(EntityId) REFERENCES [dbo].[ap_entity](Id)
)

ALTER TABLE [INSP_Product_Transaction] ALTER COLUMN [Remarks] nvarchar(max) null;

--------------------------ALD-2603 ends-------------


--------------------------ALD-2680 ends-------------
alter table CU_PR_TRAN_Subcategory ALTER COLUMN AQL_QTY_8 float
alter table CU_PR_TRAN_Subcategory ALTER COLUMN AQL_QTY_13 float
alter table CU_PR_TRAN_Subcategory ALTER COLUMN AQL_QTY_20 float
alter table CU_PR_TRAN_Subcategory ALTER COLUMN AQL_QTY_32 float
alter table CU_PR_TRAN_Subcategory ALTER COLUMN AQL_QTY_50 float
alter table CU_PR_TRAN_Subcategory ALTER COLUMN AQL_QTY_80 float
alter table CU_PR_TRAN_Subcategory ALTER COLUMN AQL_QTY_125 float
alter table CU_PR_TRAN_Subcategory ALTER COLUMN AQL_QTY_200 float
alter table CU_PR_TRAN_Subcategory ALTER COLUMN AQL_QTY_315 float
alter table CU_PR_TRAN_Subcategory ALTER COLUMN AQL_QTY_500 float
alter table CU_PR_TRAN_Subcategory ALTER COLUMN AQL_QTY_800 float
alter table CU_PR_TRAN_Subcategory ALTER COLUMN AQL_QTY_1250 float
--------------------------ALD-2680 ends-------------

-------------------- ENT Master Starts ------------------------------------

insert into ENT_Master_Type(Name,Active,Sort) Values
('Inspection Booking Order Upload',1,20)

insert into ENT_Master_Config(EntityId,Type,Value,Active)
Values (1,20,'/Documents/Purchase_Order/PurchaseOrderUpload.xlsx',1)

insert into ENT_Master_Config(EntityId,Type,Value,Active)
Values (1,20,'/Documents/Purchase_Order/Import-PurchaseOrder-Date-Format.docx',1)

-------------------- ENT Master Ends ------------------------------------
--------------------------ALD-2603 ends-------------


---------------------------ALD-2559 Extra Fee Enhancement starts-------------------------
ALTER TABLE INV_EXF_Transaction add ExchangeRate float;
ALTER TABLE INV_EXF_Transaction add InvoiceCurrencyId int;

ALTER TABLE INV_EXF_Transaction 
add Constraint FK_INV_EXF_Transaction_InvoiceCurrencyId FOREIGN KEY(InvoiceCurrencyId) REFERENCES REF_Currency(Id);
---------------------------ALD-2559 Extra Fee Enhancement ends -------------------------


----------------------ALD-2695 Send Booking Email To Customer------------------------
--send booking email to customer checkpoint
Insert Into CU_CheckPointType (Name,Active,Entity_Id) Values ('Send Booking Email to Customer',1,1)

Create Table INSP_BookingEmailConfiguration(
Id int NOT NULL PRIMARY KEY IDENTITY (1,1),
CustomerId int NOT NULL,
Email nvarchar(max) NOT NULL,
FactoryCountryId int NOT NULL,
BookingStatusId int NOT NULL,
Active BIT NOT NULL
CONSTRAINT FK_INSP_BookingEmailConfiguration_CustomerId FOREIGN KEY (CustomerId) REFERENCES CU_Customer(Id),
CONSTRAINT FK_INSP_BookingEmailConfiguration_BookingStatusId FOREIGN KEY (BookingStatusId) REFERENCES INSP_Status(Id),
CONSTRAINT FK_INSP_BookingEmailConfiguration_FactoryCountryId FOREIGN KEY (FactoryCountryId) REFERENCES REF_Country(Id)
)

----------------------ALD-2695 Send Booking Email To Customer Ends------------------------
---------------------------ALD-2292 Supplier / factory need to access the statistics dashboard ends -------------------------



-----------------------------------ALD-2521 Start--------------------------


Alter table CU_ProductCategory
Add LinkProductSubCategory int,
FOREIGN KEY (LinkProductSubCategory) REFERENCES REF_ProductCategory_Sub(Id)


create table CU_ProductType
(
	Id int IDENTITY(1,1) PRIMARY KEY,
	Name nvarchar(50),
	Code nvarchar(50),
	CustomerId int FOREIGN KEY REFERENCES Cu_Customer(Id),
	Active bit,
	Sort int,
	EntityId int FOREIGN KEY REFERENCES Ap_Entity(Id),
	LinkProductType int FOREIGN KEY REFERENCES REF_ProductCategory_Sub2
)



-----------------------------------ALD-2521 End--------------------------

-----------------------------------ALD-2759 Start--------------------------

ALTER TABLE REF_ProductCategory
ADD BusinessLineId int;

ALTER TABLE REF_ProductCategory
ADD FOREIGN KEY (BusinessLineId) REFERENCES REF_BUSINESS_LINE(Id);

UPDATE [dbo].[REF_ProductCategory] SET BusinessLineId = 2 WHERE EntityId = 1
UPDATE [dbo].[REF_ProductCategory] SET BusinessLineId = 1 WHERE EntityId = 2


-----------------------------------ALD-2759 End--------------------------

-----------------------------------ALD-2785 Start--------------------------

ALTER TABLE CU_Contact
ADD LastName nvarchar(500);

-----------------------------------ALD-2785 End--------------------------

--------------------------ALD-2652 starts-------------

Alter table SCH_Schedule_QC add IsReportFilledQC Bit 
Alter table SCH_Schedule_CS add IsReportReviewCS Bit

--------------------------ALD-2652 ends-------------



    ---------------------- ALD-2740 Custom KPI Starts ------------------------

ALTER TABLE REF_KPI_Teamplate ADD [IsDefault] [bit]

---------------------- ALD-2740 Custom KPI Ends ------------------------


----------------------ALD-2751 FactoryCountry is not mandatory starts -----------------

Alter Table INSP_BookingEmailConfiguration Alter Column FactoryCountryId int 

Alter Table INSP_BookingEmailConfiguration Add EntityId int 
Alter Table INSP_BookingEmailConfiguration Add Constraint FK_INSP_BookingEmailConfiguration_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
----------------------ALD-2751 FactoryCountry is not mandatory ends -----------------

---------------------- ALD-2706 Booking Confirm Email  starts ------------------------

 update ENT_Master_Config set Value = '<div class="div-style">SPECIAL NOTE:</div><br />
 <div class="div-style">1. Please note this is API official confirmation to your booking.</div>
 <div class="div-style">2. Please double check all booking details in this email and revert to us immediately if any 
question or update needed. Attached hyperlink pls find 
<a href="{docpath}">API Booking Terms</a> for your reference.</div>
 <div class="div-style">3. If no feedback or disagreement received from supplier/factory within 12:00PM noon time 
local time 1 working day before inspection date mentioned as above, it will be treated as all 
information in this email is confirmed and agreed,  </div>
 <div class="div-style">4. Please ensure necessary equipment for inspection is available and calibrated.</div>
 <div class="div-style">5. Please provide an inspection area with sufficient light and space. Factory support is 
needed for cartons handling, opening of the packages and repacking of the products</div>' where id = 50 and entityid  = 1 and type =14


 update ENT_Master_Config set Value = N' <div class="div-style">温馨提示与声明：</div><br />
 <div class="div-style">1. 本邮件是对贵司预约检验的正式确认。 </div>
 <div class="div-style">2. 请贵司仔细地复核本邮件中的相关内容，如有任何疑问或任何更改请立即与我们联系。附
上链接 <a href="{docpath}">API 检验预约说明</a>供参阅.</div>
 <div class="div-style">3. 截止上述验货日期前 1 个工作日的 12 时（中国北京时间）如未收到贵司邮件反馈与异议即
代表贵司确认本邮件中的相关内容正确无误，API 将相应安排人员验货。</div>
 <div class="div-style">4. 请协助提供检验时需要用到的仪器设备，并确保这些仪器设备经过校准。</div>
 <div class="div-style">5. 请协助提供充足灯照和合适空间的检验区域，并安排人员协助检验如搬箱、开箱和重新包
装等事宜</div>' where id = 55 and entityid  = 1 and type =14

update ENT_Master_Config set Value = '\\Documents\\Insp_Terms\\API_Insp_Book_Terms12112019.pdf' where id = 2 and entityid  = 1 and type =2

---------------------- ALD-2706 Booking Confirm Email  Ends ------------------------

ALTER TABLE [CU_Products] ALTER COLUMN Barcode NVARCHAR(100)
---------------------------ALD-2559 Extra Fee Enhancement ends -------------------------

---------------ALD-2381 Schedule Analysis starts----------

INSERT INTO REF_KPI_Teamplate (Name,Active,TypeId,CustomerId) VALUES ('Schedule Analysis',1,1,556)

---------------ALD-2381 Schedule Analysis ends----------

----------------ALD-2564 Add IT-Right Table Rejection Rate start----------------

declare @IdItem as int

INSERT INTO IT_Right (ParentId, TitleName, MenuName,Path,IsHeading,Active,ShowMenu,RightType) 
	VALUES (1,'Rejection Rate','Rejection Rate','rejectrate/rejection-rate',1,1,1,1)

SELECT @IdItem = SCOPE_IDENTITY()

INSERT INTO [dbo].[IT_Right_Entity](RightId,EntityId,Active) 
	values(@IdItem,1,1),(@IdItem,2,1)

----------------ALD-2564 Add IT-Right Table Rejection Rate end----------------

----------------ALD-2565 Add IT-Right Table Defect Pareto start----------------

declare @IdItem as int

INSERT INTO IT_Right (ParentId, TitleName, MenuName,Path,IsHeading,Active,ShowMenu,RightType) 
VALUES (1,'Defect Pareto','Defect Pareto','defectpareto/defectpareto',0,1,1,1)

SELECT @IdItem = SCOPE_IDENTITY()

INSERT INTO [dbo].[IT_Right_Entity](RightId,EntityId,Active) 
	values(@IdItem,1,1),(@IdItem,1,1)

----------------ALD-2565 Add IT-Right Table Defect Pareto end----------------

----------------------ALD-2682 Factory is not mandatory when the booking status requested or new booking Starts-----

ALTER TABLE [dbo].[INSP_Transaction] ALTER COLUMN [Factory_Id] [int] NULL

INSERT INTO ENT_Master_Type(Name,Active,Sort) Values
('DefaultEntityBasedOffice',1,22)

INSERT INTO ENT_Master_Config(EntityId,Type,Value,Active) Values
(1,22,1,{0})

INSERT INTO ENT_Master_Config(EntityId,Type,Value,Active) Values
(2,22,1,{0})

----------------------ALD-2682 Factory is not mandatory when the booking status requested or new booking Ends-----


--- start ALD-2855 -- 
ALTER TABLE [dbo].[EC_ExpencesClaims] ADD [IsAutoExpense] BIT NULL

--- end ALD-2855 -- 

---------------------- ALD-2912 Change Quantity data type from INT to Float for support fabric inspection Starts------------------------
ALTER TABLE FB_Report_Quantity_Details ALTER COLUMN ShipmentQuantity FLOAT;
ALTER TABLE FB_Report_Quantity_Details ALTER COLUMN InspectedQuantity FLOAT;
ALTER TABLE FB_Report_Quantity_Details ALTER COLUMN PresentedQuantity FLOAT;
ALTER TABLE FB_Report_Quantity_Details ALTER COLUMN OrderQuantity FLOAT;

ALTER TABLE FB_Report_Details ALTER COLUMN OrderQty FLOAT;
ALTER TABLE FB_Report_Details ALTER COLUMN InspectedQty FLOAT;
ALTER TABLE FB_Report_Details ALTER COLUMN PresentedQty FLOAT;
---------------------- ALD-2912 Change Quantity data type from INT to Float for support fabric inspection ends----


----------------ALD-2989 Fb template table starts----------------
ALTER TABLE FB_Report_Template DROP CONSTRAINT FK_FB_Report_Template_Company_Id;

ALTER TABLE FB_Report_Template DROP COLUMN Company_Id;

ALTER TABLE FB_Report_Template ADD Entity_Id [int] NULL
ALTER TABLE FB_Report_Template ADD CONSTRAINT FK_FB_Report_Template_Entity_Id FOREIGN KEY(Entity_Id) REFERENCES [dbo].[AP_Entity] ([Id])

----------------ALD-2989 Fb template table end----------------

---------------------------ALD-2719 Hide Monthly invoice for operation team starts-----------------
Insert Into ENT_REF_Features (Name,CreatedOn,Active) Values ('Hide Monthly Invoice to Operation in invoice status page',GETDATE(),1)
Insert Into ENT_Feature_Details (FeatureId,EntityId,CreatedOn,Active) Values (8,2,GETDATE(),1)
---------------------------ALD-2719 Hide Monthly invoice for operation team ends-------------------

---------------------------ALD-2761 Add factory country in Checkpoint starts-------------------
CREATE TABLE CU_CheckPoints_Country(
	Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
	CheckpointId int NOT NULL,
	CountryId int NOT NULL,
	Active bit NOT NULL,
	EntityId int NULL,
	CreatedBy int NULL,
	CreatedOn datetime NULL,
	DeletedBy int NULL,
	DeletedOn datetime NULL,
	FOREIGN KEY(CheckpointId) REFERENCES CU_CheckPoints (Id),
	FOREIGN KEY(CountryId) REFERENCES Ref_country(Id),
	FOREIGN KEY(EntityId) REFERENCES AP_Entity (Id)
)
---------------------------ALD-2761 Add factory country in Checkpoint ends-------------------

--------------------------ALD-2711 Add DefectInfo in defect table & Report version & revision details Starts----------------------
Alter Table FB_Report_InspDefects Add DefectInfo nvarchar(2000) NULL
Alter Table FB_Report_Details Add ReportVersion int NULL, ReportRevision Int NULL
--------------------------ALD-2711 Add DefectInfo in defect table & Report version & revision details ends------------------------


--------------------------ALD-2763 Add Sister company in customer and customer contact page Starts-----------------
CREATE TABLE [dbo].[CU_SisterCompany](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[CustomerId] [int] NOT NULL,
	[Active] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[EntityId] [int] NOT NULL,
	[SisterCompanyId] [int] NOT NULL,
	CONSTRAINT FK_CU_SisterCompany_CustomerId FOREIGN KEY (CustomerId) REFERENCES CU_Customer(Id),
	CONSTRAINT FK_CU_SisterCompany_SisterCompanyId FOREIGN KEY (SisterCompanyId) REFERENCES CU_Customer(Id),
	CONSTRAINT FK_CU_SisterCompany_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id),
	CONSTRAINT FK_CU_SisterCompany_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES IT_UserMaster(Id),
	CONSTRAINT FK_CU_SisterCompany_DeletedBy FOREIGN KEY (DeletedBy) REFERENCES IT_UserMaster(Id)
)


CREATE TABLE [dbo].[CU_Contact_SisterCompany](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ContactId] [int] NOT NULL,
	[Active] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[EntityId] [int] NOT NULL,
	[SisterCompanyId] [int] NOT NULL,
	CONSTRAINT FK_CU_Contact_SisterCompany_ContactId FOREIGN KEY (ContactId) REFERENCES CU_Contact(Id),
	CONSTRAINT FK_CU_Contact_SisterCompany_SisterCompanyId FOREIGN KEY (SisterCompanyId) REFERENCES CU_Customer(Id),
	CONSTRAINT FK_CU_Contact_SisterCompany_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id),
	CONSTRAINT FK_CU_Contact_SisterCompany_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES IT_UserMaster(Id),
	CONSTRAINT FK_CU_Contact_SisterCompany_DeletedBy FOREIGN KEY (DeletedBy) REFERENCES IT_UserMaster(Id)
)

--------------------------ALD-2763 Add Sister company in customer and customer contact page Ends-------------------

-- ALD-2814 Gap Enhancement in Booking page Starts-----------

CREATE TABLE [dbo].[INSP_REF_BookingType]
(
 Id INT PRIMARY KEY IDENTITY(1,1),
 Name NVARCHAR(100),
 Active BIT,
 Sort INT
)

INSERT INTO [dbo].[INSP_REF_BookingType] (Name,Active,Sort) 
Values ('Announced',1,1)

INSERT INTO [dbo].[INSP_REF_BookingType] (Name,Active,Sort) 
Values ('Un Announced',1,2)

CREATE TABLE [dbo].[INSP_REF_PaymentOption](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,
	[Name] [nvarchar](100),
	[CustomerId] [int],
	[Active] [bit],
	[Sort] [int],
	CONSTRAINT [FK_INSP_REF_PAYOPTION_CUSTOMERID] FOREIGN KEY([CustomerId]) REFERENCES [dbo].[CU_Customer] ([Id])
	)

	insert into [INSP_REF_PaymentOption](Name,CustomerId,Active,Sort)
	Values('A',396,1,1)

	insert into [INSP_REF_PaymentOption](Name,CustomerId,Active,Sort)
	Values('B',396,1,1)

	ALTER TABLE INSP_TRANSACTION ADD BookingType INT

	ALTER TABLE INSP_TRANSACTION ADD PaymentOptions INT

	ALTER TABLE INSP_TRANSACTION ADD CONSTRAINT FK_INSP_BOOKING_TYPE FOREIGN KEY(BookingType) REFERENCES dbo.INSP_REF_BookingType (Id)

	ALTER TABLE INSP_TRANSACTION ADD CONSTRAINT FK_INSP_PAYMENT_OPTIONS FOREIGN KEY(PaymentOptions) REFERENCES dbo.INSP_REF_PaymentOption (Id)

	insert into INSP_REF_FileAttachment_Category(Name,Sort,Active) Values ('GAP',2,1)

-- ALD-2814 Gap Enhancement in Booking page Ends-----------

--ALD-2715 start 

ALTER  TABLE [FB_Report_Details] ADD [RequestedReportRevision] INT NULL

--ALD-2715 end 

---------ALD-2789 Create a check point to restrict the supplier or factory creation Ends------
--------------------------ALD-2711 Add DefectInfo in defect table & Report version & revision details ends------------------------

---------------------------ALD-ALD-2777 Auto customer decision by report result starts-------------------

SET IDENTITY_INSERT CU_CheckPointType ON 
GO
Insert Into CU_CheckPointType (Id,Name,Active,Entity_Id) values (9,'Auto Customer Decision For Pass Report Result',1,1)
GO
SET IDENTITY_INSERT CU_CheckPointType OFF
GO

ALTER TABLE INSP_REP_CUS_Decision ADD IsAutoCustomerDecision bit 

---------------------------ALD-ALD-2777 Auto customer decision by report result ends-------------------

---------ALD-2789 Create a check point to restrict the supplier or factory creation Starts------

INSERT INTO CU_CheckPointType(Name,Active,Entity_Id)
Values ('Supplier Creation Not Allowed By Customer',1,1)

INSERT INTO CU_CheckPointType(Name,Active,Entity_Id)
Values ('Factory Creation Not Allowed By Customer',1,1)

INSERT INTO CU_CheckPointType(Name,Active,Entity_Id)
Values ('Factory Creation Not Allowed By Supplier',1,1)

---------ALD-2789 Create a check point to restrict the supplier or factory creation Ends------

--------------------------ALD-2807 Grade on Supplier / Factory Module Starts -----------------------------------------------------
CREATE TABLE [dbo].[SU_Level_Custom](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[LevelId] [int] NULL,
	[CustomerId] [int] NULL,
	[IsDefault] [bit] NOT NULL,
	[CustomName] [nvarchar](500) NULL,
	CONSTRAINT FK_SU_Level_Custom_CustomerId FOREIGN KEY(CustomerId) REFERENCES CU_Customer(Id),
	CONSTRAINT FK_SU_Level_Custom_LevelId FOREIGN KEY(LevelId) REFERENCES SU_Level(Id)
)

INSERT INTO [SU_Level_Custom] (LevelId,CustomerId,IsDefault,CustomName) Values (1,NULL,1,NULL)
INSERT INTO [SU_Level_Custom] (LevelId,CustomerId,IsDefault,CustomName) Values (2,NULL,1,NULL)
INSERT INTO [SU_Level_Custom] (LevelId,CustomerId,IsDefault,CustomName) Values (3,NULL,1,NULL)
INSERT INTO [SU_Level_Custom] (LevelId,CustomerId,IsDefault,CustomName) Values (4,NULL,1,NULL)

CREATE TABLE [dbo].[SU_Grade](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[SupplierId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Level_Id] [int] NOT NULL,
	[PeriodFrom] [datetime] NOT NULL,
	[PeriodTo] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	CONSTRAINT FK_SU_Grade_CustomerId FOREIGN KEY(CustomerId) REFERENCES CU_Customer(Id),
	CONSTRAINT FK_SU_Grade_SupplierId FOREIGN KEY(SupplierId) REFERENCES SU_Supplier(Id),
	CONSTRAINT FK_SU_Grade_Level_Id FOREIGN KEY(Level_Id) REFERENCES SU_Level_Custom(Id),
	CONSTRAINT FK_SU_Grade_CreatedBy FOREIGN KEY(CreatedBy) REFERENCES IT_UserMaster(Id),
	CONSTRAINT FK_SU_Grade_UpdatedBy FOREIGN KEY(UpdatedBy) REFERENCES IT_UserMaster(Id),
	CONSTRAINT FK_SU_Grade_DeletedBy FOREIGN KEY(DeletedBy) REFERENCES IT_UserMaster(Id)
)

--------------------------ALD-2807 Grade on Supplier / Factory Module Ends-----------------------------------------------------

---------------------ALD-2882 Extra Fee Enhnacement starts-----------------------------------
ALTER TABLE INV_EXF_Transaction ADD 
BilledName nvarchar(500),
BilledAddress nvarchar(1000),
PaymentTerms nvarchar(500),
PaymentDuration int;
----------------------ALD-2882 Extra Fee Enhnacement ends---------------------------------


--------------------------ALD-2911 add re-inspection type foregin key starts-----------------------------------------------------

ALTER TABLE [INSP_Transaction] ADD CONSTRAINT FK_INSP_TRANSACTION_RE_INSPECTION_TYPE FOREIGN KEY ([ReInspectionType]) REFERENCES REF_ReInspectionType(Id);

--------------------------ALD-2911 add re-inspection type foregin key Ends-----------------------------------------------------

--- ALD-2881 Sync Fabric data with Link start---
ALTER TABLE FB_Report_Details ADD 
Fabric_NoOfRollsPresented FLOAT NULL,
Fabric_NoOfLotsPresented FLOAT NULL,
Fabric_ProducedQtyRoll FLOAT NULL,
Fabric_PresentedQtyRoll FLOAT NULL,
Fabric_InspectedQtyRoll FLOAT NULL,
Fabric_AcceptedQtyRoll FLOAT NULL,
Fabric_RejectedQtyRoll FLOAT NULL,
Fabric_MachineSpeed Nvarchar(200) NULL,
Fabric_Type nvarchar(200) NULL,
Fabric_TypeCheck  nvarchar(200) NULL,
Fabric_factoryType nvarchar(200) NULL;


ALTER TABLE FB_Report_Quantity_Details ADD 
Fabric_Points100Sqy FLOAT NULL,
Fabric_AcceptanceCriteria FLOAT NULL,
ProducedQuantity FLOAT NULL,
Fabric_OverLessProducedQty nvarchar(100) NULL,
Fabric_RejectedQuantity FLOAT NULL,
Fabric_RejectedRolls FLOAT NULL,
Fabric_DemeritPts FLOAT NULL,
Fabric_Tolerance FLOAT NULL,
Fabric_Rating nvarchar(100) NULL;

CREATE TABLE FB_Report_FabricControlmadeWith(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	ReportDetailsId INT NOT NULL,
	Name nvarchar(500) NULL,
	Active BIT,
	CreatedOn DATETIME,
	DeletedOn DATETIME,
	CONSTRAINT FB_Report_FabricControlmadeWith_ReportDetailsId FOREIGN KEY(ReportDetailsId) REFERENCES FB_Report_Details(Id)
);

CREATE TABLE FB_Report_FabricDefects(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	FbreportdetailsId INT NULL,
	InspPoTransactionId INT NULL,
	InspColorTransactionId INT NULL,
	LengthUnit nvarchar(100) NULL,
	DyeLot Float NULL,
	RollNumber INT NULL,
	Result nvarchar(100) NULL,
	AcceptanceCriteria FLOAT NULL,
	Points100Sqy FLOAT NULL,
	LengthOriginal FLOAT NULL,
	LengthActual FLOAT NULL,
	WeightOriginal FLOAT NULL,
	WeightActual FLOAT NULL,
	WidthOriginal FLOAT NULL,
	WidthActual FLOAT NULL,
	Code nvarchar(100) NULL,
	Description nvarchar(200) NULL,
	Location FLOAT NULL,
	Point FLOAT NULL,
	Active BIT,
	CreatedOn DATETIME,
	DeletedOn DATETIME,
	CONSTRAINT FB_Report_FabricDefects_FbreportdetailsId FOREIGN KEY(FbreportdetailsId) REFERENCES FB_Report_Details(Id),
	CONSTRAINT FB_Report_FabricDefects_InspPoTransactionId FOREIGN KEY(InspPoTransactionId) REFERENCES INSP_PurchaseOrder_Transaction(Id),
	CONSTRAINT FB_Report_FabricDefects_InspColorTransactionId FOREIGN KEY(InspColorTransactionId) REFERENCES INSP_PurchaseOrder_Color_Transaction(Id)
);

--- ALD-2881 Sync Fabric data with Link ends---
---------------------- ALD-2912 Change Quantity data type from INT to Float for support fabric inspection ends------------------------


-------------------------ALD-2816 Create EAQF Booking API - ENT_Master_Type -----------------------------------

INSERT INTO [dbo].ENT_Master_Type( Name, Active, Sort) VALUES ('DefaultOfficeForEAQF', 1, 23 );

INSERT INTO [dbo].ENT_Master_Config( EntityId, CountryId, Type, Value, Active) VALUES (3, NULL, 23, 1, 1 );

-------------------------ALD-2816 ENT_Master_Type -----------------------------------


-------------------------ALD-2921 Load the Menu by Right Type starts-----------------------------------
Update IT_Right set ParentId = NULL where TitleName = 'Customer'
Update IT_Right set ParentId = NULL where TitleName = 'Supplier/Factory'
Update IT_Right set ParentId = NULL where TitleName = 'Human Resource'
Update IT_Right set ParentId = NULL where TitleName = 'Expense Claim'
Update IT_Right set ParentId = NULL where TitleName = 'User Management'
Update IT_Right set ParentId = NULL where TitleName = 'Area Management'

Update IT_Right set MenuName = 'Register' where MenuName = 'Staff Register'
Update IT_Right set MenuName = 'Summary' where MenuName = 'Staff Summary'

declare @LeaveId as int
Insert Into IT_Right (TitleName,MenuName,IsHeading,Active,Glyphicons,ShowMenu) values ('Leave','Leave',1,1,'fa fa-sign-in',1)
SELECT @LeaveId = SCOPE_IDENTITY()
Insert Into IT_Right_Entity (RightId,EntityId,Active) Values (@LeaveId,1,1)
Insert Into IT_Right_Entity (RightId,EntityId,Active) Values (@LeaveId,2,1)
Insert Into IT_Right_Entity (RightId,EntityId,Active) Values (@LeaveId,3,1)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (1, @LeaveId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (2, @LeaveId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (3, @LeaveId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (4, @LeaveId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (5, @LeaveId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (6, @LeaveId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (8, @LeaveId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (9, @LeaveId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (11, @LeaveId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (12, @LeaveId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (13, @LeaveId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (14, @LeaveId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (15, @LeaveId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (16, @LeaveId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (28, @LeaveId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (43, @LeaveId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (52, @LeaveId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (54, @LeaveId)
Update IT_Right set ParentId = @LeaveId, MenuName = 'Application' where MenuName = 'Leave Application'
Update IT_Right set ParentId = @LeaveId, MenuName = 'Summary' where MenuName = 'Leave Summary'
Update IT_Right set ParentId = @LeaveId, MenuName = 'Approve' where MenuName = 'Leave Approve'

declare @HolidayId as int
Insert Into IT_Right (TitleName,MenuName,IsHeading,Active,Glyphicons,ShowMenu) values ('Holiday','Holiday',1,1,'fa fa-sign-in',1)
SELECT @HolidayId = SCOPE_IDENTITY()
Insert Into IT_Right_Entity (RightId,EntityId,Active) Values (@HolidayId,1,1)
Insert Into IT_Right_Entity (RightId,EntityId,Active) Values (@HolidayId,2,1)
Insert Into IT_Right_Entity (RightId,EntityId,Active) Values (@HolidayId,3,1)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (1, @HolidayId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (2, @HolidayId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (3, @HolidayId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (4, @HolidayId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (6, @HolidayId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (8, @HolidayId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (9, @HolidayId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (11, @HolidayId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (12, @HolidayId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (13, @HolidayId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (14, @HolidayId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (15, @HolidayId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (16, @HolidayId)
INSERT [dbo].[IT_Role_Right] ([RoleId], [RightId]) VALUES (52, @HolidayId)
Update IT_Right set ParentId = @HolidayId, MenuName = 'Register' where MenuName = 'Holiday Register'
Update IT_Right set ParentId = @HolidayId, MenuName = 'Calendar' where MenuName = 'Holiday Calendar'

--move exchange rate under invoice menu 
Update IT_Right set ParentId = 12 where TitleName = 'Exchange Rate'

--move product management , Lab under Support
Update IT_Right set ParentId = 13 where TitleName = 'Product Management'
Update IT_Right set ParentId = 13 where TitleName = 'Lab'

--remove master menu
Update IT_Right set Active = 0 where TitleName = 'Master'

CREATE TABLE [IT_Right_Map] (
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [RightId] int NOT NULL,
    [RightTypeId] int NOT NULL,
	[Active] bit NOT NULL,
    FOREIGN KEY ([RightId]) REFERENCES [IT_Right]([Id]),
	FOREIGN KEY ([RightTypeId]) REFERENCES [IT_Right_Type]([Id])
)

ALTER TABLE [It_Right_Type] ADD [Sort] INT
ALTER TABLE [It_Right_Type] ADD [Service] INT FOREIGN KEY ([Service]) REFERENCES [REF_Service]([Id])

Update IT_Right_Type Set Service = 1 where Id = 1
Update IT_Right_Type Set Service = 2 where Id = 2
Update IT_Right_Type Set Service = 3 where Id = 3
Insert Into IT_Right_Type (Name,Active) Values ('Lab Testing',1)
Insert Into IT_Right_Type (Name,Active) Values ('Customer',1)
Insert Into IT_Right_Type (Name,Active) Values ('Supplier / Factory',1)
Insert Into IT_Right_Type (Name,Active) Values ('Human Resource',1)
Insert Into IT_Right_Type (Name,Active) Values ('Document',1)
Insert Into IT_Right_Type (Name,Active) Values ('Admin',1)

--manage order menu
Update IT_Right set Ranking =	1	where Id =	1
Update IT_Right set Ranking =	2	where Id =	2
Update IT_Right set Ranking =	1	where Id =	3
Update IT_Right set Ranking =	1	where Id =	4
Update IT_Right set Ranking =	2	where Id =	5
Update IT_Right set Ranking =	2	where Id =	6
Update IT_Right set Ranking =	1	where Id =	7
Update IT_Right set Ranking =	2	where Id =	8
Update IT_Right set Ranking =	3	where Id =	9
Update IT_Right set Ranking =	4	where Id =	10
Update IT_Right set Ranking =	5	where Id =	11
Update IT_Right set Ranking =	6	where Id =	12
Update IT_Right set Ranking =	7	where Id =	13
Update IT_Right set Ranking =	12	where Id =	14
Update IT_Right set Ranking =	1	where Id =	15
Update IT_Right set Ranking =	2	where Id =	16
Update IT_Right set Ranking =	1	where Id =	17
Update IT_Right set Ranking =	2	where Id =	18
Update IT_Right set Ranking =	3	where Id =	19
Update IT_Right set Ranking =	1	where Id =	20
Update IT_Right set Ranking =	2	where Id =	21
Update IT_Right set Ranking =	13	where Id =	22
Update IT_Right set Ranking =	1	where Id =	23
Update IT_Right set Ranking =	2	where Id =	24
Update IT_Right set Ranking =	3	where Id =	25
Update IT_Right set Ranking =	27	where Id =	26
Update IT_Right set Ranking =	18	where Id =	27
Update IT_Right set Ranking =	1	where Id =	28
Update IT_Right set Ranking =	2	where Id =	29
Update IT_Right set Ranking =	3	where Id =	30
Update IT_Right set Ranking =	4	where Id =	31
Update IT_Right set Ranking =	5	where Id =	32
Update IT_Right set Ranking =	11	where Id =	33
Update IT_Right set Ranking =	1	where Id =	34
Update IT_Right set Ranking =	2	where Id =	35
Update IT_Right set Ranking =	10	where Id =	36
Update IT_Right set Ranking =	1	where Id =	37
Update IT_Right set Ranking =	2	where Id =	38
Update IT_Right set Ranking =	3	where Id =	39
Update IT_Right set Ranking =	1	where Id =	40
Update IT_Right set Ranking =	1	where Id =	41
Update IT_Right set Ranking =	2	where Id =	42
Update IT_Right set Ranking =	1	where Id =	43
Update IT_Right set Ranking =	1	where Id =	44
Update IT_Right set Ranking =	2	where Id =	45
Update IT_Right set Ranking =	3	where Id =	46
Update IT_Right set Ranking =	17	where Id =	47
Update IT_Right set Ranking =	1	where Id =	48
Update IT_Right set Ranking =	2	where Id =	49
Update IT_Right set Ranking =	2	where Id =	50
Update IT_Right set Ranking =	1	where Id =	51
Update IT_Right set Ranking =	2	where Id =	52
Update IT_Right set Ranking =	4	where Id =	53
Update IT_Right set Ranking =	5	where Id =	54
Update IT_Right set Ranking =	6	where Id =	55
Update IT_Right set Ranking =	3	where Id =	56
Update IT_Right set Ranking =	17	where Id =	57
Update IT_Right set Ranking =	19	where Id =	58
Update IT_Right set Ranking =	6	where Id =	59
Update IT_Right set Ranking =	7	where Id =	60
Update IT_Right set Ranking =	3	where Id =	61
Update IT_Right set Ranking =	1	where Id =	62
Update IT_Right set Ranking =	2	where Id =	63
Update IT_Right set Ranking =	3	where Id =	64
Update IT_Right set Ranking =	4	where Id =	65
Update IT_Right set Ranking =	5	where Id =	66
Update IT_Right set Ranking =	6	where Id =	67
Update IT_Right set Ranking =	7	where Id =	68
Update IT_Right set Ranking =	8	where Id =	69
Update IT_Right set Ranking =	7	where Id =	70
Update IT_Right set Ranking =	8	where Id =	71
Update IT_Right set Ranking =	1	where Id =	72
Update IT_Right set Ranking =	2	where Id =	73
Update IT_Right set Ranking =	1	where Id =	74
Update IT_Right set Ranking =	2	where Id =	75
Update IT_Right set Ranking =	3	where Id =	76
Update IT_Right set Ranking =	1	where Id =	77
Update IT_Right set Ranking =	2	where Id =	78
Update IT_Right set Ranking =	4	where Id =	79
Update IT_Right set Ranking =	3	where Id =	80
Update IT_Right set Ranking =	5	where Id =	81
Update IT_Right set Ranking =	4	where Id =	82
Update IT_Right set Ranking =	5	where Id =	83
Update IT_Right set Ranking =	6	where Id =	84
Update IT_Right set Ranking =	3	where Id =	85
Update IT_Right set Ranking =	2	where Id =	86
Update IT_Right set Ranking =	7	where Id =	87
Update IT_Right set Ranking =	1	where Id =	88
Update IT_Right set Ranking =	2	where Id =	89
Update IT_Right set Ranking =	1	where Id =	90
Update IT_Right set Ranking =	2	where Id =	91
Update IT_Right set Ranking =	3	where Id =	92
Update IT_Right set Ranking =	3	where Id =	93
Update IT_Right set Ranking =	4	where Id =	94
Update IT_Right set Ranking =	5	where Id =	95
Update IT_Right set Ranking =	6	where Id =	96
Update IT_Right set Ranking =	4	where Id =	97
Update IT_Right set Ranking =	9	where Id =	98
Update IT_Right set Ranking =	1	where Id =	99
Update IT_Right set Ranking =	2	where Id =	100
Update IT_Right set Ranking =	4	where Id =	101
Update IT_Right set Ranking =	5	where Id =	102
Update IT_Right set Ranking =	6	where Id =	103
Update IT_Right set Ranking =	7	where Id =	104
Update IT_Right set Ranking =	8	where Id =	105
Update IT_Right set Ranking =	8	where Id =	106
Update IT_Right set Ranking =	9	where Id =	107
Update IT_Right set Ranking =	1	where Id =	108
Update IT_Right set Ranking =	2	where Id =	109
Update IT_Right set Ranking =	3	where Id =	110
Update IT_Right set Ranking =	10	where Id =	111
Update IT_Right set Ranking =	4	where Id =	112
Update IT_Right set Ranking =	1	where Id =	113
Update IT_Right set Ranking =	7	where Id =	114
Update IT_Right set Ranking =	9	where Id =	115
Update IT_Right set Ranking =	4	where Id =	116
Update IT_Right set Ranking =	8	where Id =	117
Update IT_Right set Ranking =	5	where Id =	118
Update IT_Right set Ranking =	16	where Id =	119
Update IT_Right set Ranking =	1	where Id =	120
Update IT_Right set Ranking =	2	where Id =	121
Update IT_Right set Ranking =	1	where Id =	122
Update IT_Right set Ranking =	9	where Id =	123
Update IT_Right set Ranking =	1	where Id =	124
Update IT_Right set Ranking =	2	where Id =	125
Update IT_Right set Ranking =	9	where Id =	126
Update IT_Right set Ranking =	6	where Id =	127
Update IT_Right set Ranking =	2	where Id =	128
Update IT_Right set Ranking =	1	where Id =	129
Update IT_Right set Ranking =	4	where Id =	130
Update IT_Right set Ranking =	5	where Id =	131
Update IT_Right set Ranking =	6	where Id =	132
Update IT_Right set Ranking =	7	where Id =	133
Update IT_Right set Ranking =	9	where Id =	134
Update IT_Right set Ranking =	2	where Id =	135
Update IT_Right set Ranking =	9	where Id =	136
Update IT_Right set Ranking =	14	where Id =	137
Update IT_Right set Ranking =	15	where Id =	138

SET IDENTITY_INSERT [dbo].[IT_Right_Map] ON 

INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (1, 1, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (2, 91, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (3, 92, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (4, 101, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (5, 102, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (6, 104, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (7, 106, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (8, 2, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (9, 6, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (10, 7, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (11, 8, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (12, 9, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (13, 53, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (14, 54, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (15, 55, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (16, 70, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (17, 71, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (18, 61, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (19, 62, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (20, 63, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (21, 64, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (22, 65, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (23, 66, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (24, 68, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (25, 69, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (26, 115, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (27, 10, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (28, 72, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (29, 73, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (30, 85, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (31, 97, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (32, 11, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (33, 74, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (34, 75, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (35, 76, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (36, 81, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (37, 84, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (38, 87, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (39, 105, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (40, 107, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (41, 111, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (42, 77, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (43, 78, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (44, 80, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (45, 88, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (46, 89, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (47, 108, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (48, 109, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (49, 110, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (50, 12, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (51, 86, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (52, 93, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (53, 94, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (54, 95, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (55, 96, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (56, 114, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (57, 117, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (58, 123, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (59, 126, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (60, 134, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (61, 136, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (62, 124, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (63, 125, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (64, 129, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (65, 135, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (66, 14, 7, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (67, 22, 7, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (68, 137, 7, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (69, 138, 7, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (70, 15, 7, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (71, 16, 7, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (72, 23, 7, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (73, 24, 7, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (74, 17, 7, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (75, 18, 7, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (76, 19, 7, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (77, 20, 7, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (78, 21, 7, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (79, 47, 9, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (80, 48, 9, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (81, 49, 9, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (82, 13, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (83, 112, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (84, 118, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (85, 113, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (86, 128, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (87, 33, 6, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (88, 34, 6, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (89, 35, 6, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (90, 36, 5, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (91, 37, 5, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (92, 38, 5, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (93, 39, 5, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (94, 82, 5, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (95, 83, 5, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (96, 27, 9, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (97, 28, 9, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (98, 29, 9, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (99, 30, 9, 1)
GO
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (100, 31, 9, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (101, 32, 9, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (102, 59, 9, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (103, 60, 9, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (104, 40, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (105, 41, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (106, 42, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (107, 43, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (108, 56, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (109, 44, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (110, 45, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (111, 46, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (112, 116, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (113, 57, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (114, 58, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (115, 119, 8, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (116, 120, 8, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (117, 121, 8, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (118, 122, 8, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (119, 2, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (120, 3, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (121, 4, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (122, 5, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (125, 61, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (126, 62, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (127, 63, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (128, 64, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (129, 65, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (130, 66, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (131, 68, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (132, 69, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (133, 11, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (134, 79, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (135, 12, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (136, 86, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (137, 93, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (138, 94, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (139, 95, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (140, 96, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (141, 136, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (142, 40, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (143, 41, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (144, 42, 2, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (145, 98, 3, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (146, 99, 3, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (147, 100, 3, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (148, 25, 7, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (149, 130, 7, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (150, 131, 7, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (151, 132, 7, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (152, 133, 7, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (153, 50, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (154, 51, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (155, 52, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (156, 103, 1, 1)
INSERT [dbo].[IT_Right_Map] ([Id], [RightId], [RightTypeId], [Active]) VALUES (157, 114, 2, 1)
SET IDENTITY_INSERT [dbo].[IT_Right_Map] OFF
GO

-------------------------ALD-2921 Load the Menu by Right Type end-----------------------------------

-------------------------ALD-2949 New fields for EAQF booking (ADL-2816 story) -----------------------------------

CREATE TABLE INSP_REF_ReportRequest (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	Name NVARCHAR(200),
    Active BIT,
    Sort INT,
);

INSERT INTO INSP_REF_ReportRequest (Name, Active, Sort)
VALUES ('Individual', 1, 1),
		('Multi products', 1, 2),
		('Batch', 1, 3)

CREATE TABLE INSP_REF_PackingStatus (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    Name NVARCHAR(200),
    Active BIT,
    Sort INT
);

INSERT INTO INSP_REF_PackingStatus (Name, Active, Sort)
VALUES	(0, 1, 1),
		(10, 1, 2),
		(20, 1, 3),
		(30, 1, 4),
		(40, 1, 5),
		(50, 1, 6),
		(60, 1, 7),
		(70, 1, 8),
		(80, 1, 9),
		(90, 1, 10),
		(100, 1, 11)

CREATE TABLE INSP_REF_ProductionStatus (
    Id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    Name NVARCHAR(200),
    Active BIT,
    Sort INT
);

INSERT INTO INSP_REF_ProductionStatus (Name, Active, Sort)
VALUES (80, 1, 1), (90, 1, 2), (100, 1, 3)

ALTER TABLE INSP_Product_Transaction 
	ADD ProductionStatus INT,
	PackingStatus INT,
	IsGoldenSampleAvailable  BIT,
	GoldenSampleComments NVARCHAR(500),
	IsSampleCollectionBit BIT,
	SampleCollectionComments NVARCHAR(500)

---- renamed FK name ----

ALTER TABLE [INSP_Product_Transaction] DROP  FK__INSP_Prod__Produ__0547DC94;
ALTER TABLE [INSP_Product_Transaction] DROP  FK__INSP_Prod__Packi__063C00CD;

 ALTER TABLE [INSP_Product_Transaction]
ADD CONSTRAINT FK__INSP_Prod__Production__Status
	FOREIGN KEY (ProductionStatus) REFERENCES [dbo].[INSP_REF_ProductionStatus](Id),
	CONSTRAINT FK__INSP_Prod__Packing__Status
	FOREIGN KEY (PackingStatus) REFERENCES [dbo].[INSP_REF_PackingStatus](Id);
---- renamed FK name ----

ALTER TABLE INSP_Transaction 
ADD ReportRequest INT,
	IsSameDayReport  BIT,
	IsInspectionCertificate  BIT,
	IsEAQF BIT

-- renamed FK name ----
ALTER TABLE [INSP_Transaction] DROP  [FK__INSP_Tran__Repor__07302506];

ALTER TABLE [INSP_Transaction] ADD CONSTRAINT FK__INSP_Tran__Report_Request
FOREIGN KEY (ReportRequest) REFERENCES [dbo].[INSP_REF_ReportRequest](Id);
--- renamed FK name ---

ALTER TABLE Cu_Customer ADD IsEAQF BIT

ALTER TABLE Su_Supplier ADD IsEAQF BIT

-------------------------ALD-2949 New fields for EAQF booking (ADL-2816 story) -----------------------------------


---------------------- ALD-2912 Change Quantity data type from INT to Float for support fabric inspection ends------------------------

--- ALD-2881 Sync Fabric data with Link ends---

--- ALD-3066 Roll Number should be string starts --------
Alter Table FB_Report_FabricDefects Alter Column RollNumber nvarchar(100)
Alter Table FB_Report_FabricDefects Alter Column DyeLot nvarchar(200)
--- ALD-3066 Roll Number should be string ends-----
-------------------------ALD-2816 ENT_Master_Type -----------------------------------
---ALD-2479 task start -- 
ALTER TABLE INV_REF_FileType ADD  [IsUpload] BIT 
ALTER TABLE INV_TRAN_Files ADD  [InvoiceNo] nvarchar(1000) 
---ALD-2479 task end -- 


insert into [ES_REF_Report_Send_Type](name,Active) values('All Invoice',1)


------------------------- ALD-2922 Add additional FB result start -----------------------------------

Insert Into FB_Report_Result (ResultName,Active) Values ('Conformed',1)
Insert Into FB_Report_Result (ResultName,Active) Values ('Not_Conformed',1)
Insert Into FB_Report_Result (ResultName,Active) Values ('Delay',1)
Insert Into FB_Report_Result (ResultName,Active) Values ('Note',1)

------------------------- ALD-2922 Add additional FB result end -----------------------------------


------------------------------ ALD-3051 Change Manday Type Numeric to Decimal Starts --------------------------
ALTER TABLE QU_Quotation_Audit
ALTER COLUMN NoOfManDay float

ALTER TABLE QU_Quotation_Audit
ALTER COLUMN NoOfTravelManDay float

ALTER TABLE QU_Quotation_Insp
ALTER COLUMN NoOfManDay float

ALTER TABLE QU_Quotation_Insp
ALTER COLUMN NoOfTravelManDay float

ALTER TABLE QU_Quotation_Aud_Manday
ALTER COLUMN NoOfManDay float

ALTER TABLE QU_Quotation_Insp_Manday
ALTER COLUMN NoOfManDay float

------------------------------ ALD-3051 Change Manday Type Numeric to Decimal Ends ----------------------------


----------ALD-2309 Invoice send by Email for Pre-invoice start here ----------------

INSERT INTO INV_Status (NAME,Active) values('Sent',1)

ALTER TABLE INV_AUT_TRAN_Details ADD TemplateId INT Null;

CREATE TABLE [dbo].[INV_REF_FileType]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] [nvarchar](200) NOT NULL, 
    [Active] BIT NOT NULL, 
    [Sort] INT NULL
)

INSERT INTO [INV_REF_FileType] (NAME,Active,Sort)VALUES('Invoice',1,1)
INSERT INTO [INV_REF_FileType] (NAME,Active,Sort)VALUES('Manual Invoice',1,1)
INSERT INTO [INV_REF_FileType] (NAME,Active,Sort)VALUES('Credit Note',1,1)

CREATE TABLE [dbo].[INV_TRAN_Files]
(
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[InvoiceId] INT,
	[FileName] NVARCHAR(1000),
	[FileType] INT,
	[UniqueId] NVARCHAR(MAX),
	[FilePath] NVARCHAR(1000),
	[CreatedBy] INT,	
	[CreatedOn] DATETIME NULL,
	[DeletedBy] INT,
	[DeletedOn] DATETIME NULL,
	[Active] BIT,	
	CONSTRAINT FK_INV_TRAN_Files_InvoiceId FOREIGN KEY([InvoiceId]) REFERENCES [dbo].[INV_AUT_TRAN_Details],
	CONSTRAINT FK_INV_TRAN_Files_FileType FOREIGN KEY([FileType]) REFERENCES [dbo].[INV_REF_FileType],
	CONSTRAINT FK_INV_TRAN_Files_CreatedBy FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster],
	CONSTRAINT FK_INV_TRAN_Files_DeletedBy FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster]
)

INSERT INTO [ENT_Master_Type] (NAME,Active,Sort)VALUES('Pre-Invoice Email content 1',1,18)
INSERT INTO [ENT_Master_Type] (NAME,Active,Sort)VALUES('Pre-Invoice Email content 2',1,19)

INSERT INTO ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (18,2,NULL,'The attached invoice is issued by SgT. Please preview and arrange payment to SgT accordingly. Any overdue payment is subjected to an interest of 2% per month from the due date until the outstanding amount has been paid in full',1)
INSERT INTO ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (18,1,NULL,'The attached invoice is issued by API. Please preview and arrange payment to API accordingly. Any overdue payment is subjected to an interest of 2% per month from the due date until the outstanding amount has been paid in full',1)
INSERT INTO ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (19,1,NULL,'<div class="div-style">All overdue payment will be subjected to 2% monthly interest.</div> <div class="div-style">All Bank charges should be borne by the payer</div> <div class="div-style">Thank you to fax or post us copy of the bank transfer.</div>',1)
INSERT INTO ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (19,2,NULL,'<div class="div-style">All overdue payment will be subjected to 2% monthly interest.</div> <div class="div-style">All Bank charges should be borne by the payer</div> <div class="div-style">Thank you to fax or post us copy of the bank transfer.</div>',1)

---------- ALD-2309 Invoice send by Email for Pre-invoice end here ----------------

---------- ALD-3104 Register Quotation | Factory grade is incorrectly mapped starts-------
Alter Table SU_Grade Add EntityId int 
Alter Table SU_Grade Add Constraint FK_SU_Grade_EntityId FOREIGN KEY (EntityId) REFERENCES AP_Entity(Id)
---------- ALD-3104 Register Quotation | Factory grade is incorrectly mapped ends-------

------------------------------ALD-3125 Move Supplier and factory to Purchase order level Starts --------------------

CREATE TABLE [dbo].[CU_PO_Supplier]
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	PoId INT,
	SupplierId INT,
	Active BIT,
	CreatedBy INT,
	CreatedOn DateTime,
	DeletedBy INT,
	DeletedOn DateTime,
	CONSTRAINT FK_CU_PO_Supplier_PO_Id FOREIGN KEY(PoId) REFERENCES Cu_PurchaseOrder(Id),
	CONSTRAINT FK_CU_PO_Supplier_Supplier_Id FOREIGN KEY(SupplierId) REFERENCES SU_Supplier(Id),
	CONSTRAINT FK_CU_PO_Supplier_Created_By FOREIGN KEY(CreatedBy) REFERENCES IT_UserMaster(Id),
	CONSTRAINT FK_CU_PO_Supplier_Deleted_By FOREIGN KEY(DeletedBy) REFERENCES IT_UserMaster(Id)
)

CREATE TABLE [dbo].[CU_PO_Factory]
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	PoId INT,
	FactoryId INT,
	Active BIT,
	CreatedBy INT,
	CreatedOn DateTime,
	DeletedBy INT,
	DeletedOn DateTime,
	CONSTRAINT FK_CU_PO_Factory_PO_Id FOREIGN KEY(PoId) REFERENCES Cu_PurchaseOrder(Id),
	CONSTRAINT FK_CU_PO_Factory_FactoryId FOREIGN KEY(FactoryId) REFERENCES SU_Supplier(Id),
	CONSTRAINT FK_CU_PO_Factory_Created_By FOREIGN KEY(CreatedBy) REFERENCES IT_UserMaster(Id),
	CONSTRAINT FK_CU_PO_Factory_Deleted_By FOREIGN KEY(DeletedBy) REFERENCES IT_UserMaster(Id)
)

insert into [dbo].[CU_PO_Supplier](PoId,SupplierId,Active,CreatedBy,CreatedOn)
select distinct
po.Id,
poDetail.Supplier_Id,
1,
1,
getDate()
from CU_PurchaseOrder po
join
CU_PurchaseOrder_Details poDetail
on po.Id=poDetail.PO_Id
Order by po.Id asc

insert into [dbo].[CU_PO_Factory](PoId,FactoryId,Active,CreatedBy,CreatedOn)
select distinct
po.Id,
poDetail.Factory_Id,
1,
1,
getDate()
from CU_PurchaseOrder po
join
CU_PurchaseOrder_Details poDetail
on po.Id=poDetail.PO_Id
Order by po.Id 

 UPDATE pd1 
SET pd1.Active = 0
FROM CU_PurchaseOrder_Details pd1
INNER JOIN 

( 
   select 
   min(Id) as MinId,
   PO_Id,Product_Id
   from CU_PurchaseOrder_Details 
   group by PO_Id,Product_Id HAVING count(*)> 1
   ) pd2 

   ON pd1.PO_Id = pd2.PO_Id AND pd2.Product_Id = pd2.Product_Id AND pd1.Id > pd2.MinId

   ------------------------------ALD-3125 Move Supplier and factory to Purchase order level Ends --------------------

   ------------------------------ALD-3124 Load PO & Product in Booking page Starts --------------------

   insert into CU_CheckPointType(Name,Active,Entity_Id) Values ('Po Product By Supplier',1,1)

   ------------------------------ALD-3124 Load PO & Product in Booking page Ends --------------------

-- ALD-3287 start 
CREATE NONCLUSTERED INDEX IX_FB_Report_Additional_Photos_FbReportDetailId
ON FB_Report_Additional_Photos ([FbReportDetailId] ASC)
-- ALD-3287 end 



-------------ALD-3293 IC Enhancement add Color and Color Code in IC starts ------------------

Alter Table INSP_IC_TRAN_Products Add PoColorId int 
Alter Table INSP_IC_TRAN_Products Add Constraint FK_INSP_IC_TRAN_Products_PoColorId FOREIGN KEY (PoColorId) REFERENCES INSP_PurchaseOrder_Color_Transaction(Id)

insert into ENT_Master_Type(Name,Active,Sort) values('IC_Remark',1,24)
insert into ENT_Master_Type(Name,Active,Sort) values('IC_Footer',1,25)

Insert Into ENT_Master_Config (EntityId,Type,Active,Value)
Values 
(1,24,1,'Remarks: The inspection has been carried out at random and generally applies only to part of the total quantity released for
shipment. The inspection has been executed by us conscientiously with our best knowledge, but does not imply any
responsibility on our part. The issue of this Certificate has no effect on the rights of the buyer to claim any deviations
in the goods released for shipment when compared with the conditions contained in the order. This particularly
applies to the quantity, quality, marketing and packing of the released goods compared with the order details'),
(2,24,1,'NB: The Provision of SgT Inspection Certificate does not release the beneficiary from their contractual liabilities and
responsibilities for their product withregard to quality nor does it prejudice the applicant''s right to claim againts
beneficiary for compasation for any apparent and/or hidden defect not detected during random inspection or occuring
thereafter.'),
(2,25,1,'Sgt is ISO 9001 and ISO 17020 Certified SGT-QR-16-IC-01A')

-------------ALD-3293 IC Enhancement add Color and Color Code in IC ends ------------------
-- ALD-3287 end 
--ALD-3329 start 
INSERT INTO [INV_REF_FileType] (NAME,Active,Sort,IsUpload)VALUES('Other Documents',1,1,1)

update INV_REF_FileType set IsUpload=null where id=1
--ALD-3329 end 
---------- ALD-3104 Register Quotation | Factory grade is incorrectly mapped ends-------
---------- ALD-2309 Invoice send by Email for Pre-invoice end here ----------------

---------- ALD-3044 Create a custom KPI for Inspection summary for QC start here ----------------

SET IDENTITY_INSERT REF_KPI_Teamplate ON 
GO
Insert Into REF_KPI_Teamplate (ID,Name,Active,TypeId,IsDefault) values (41,'Inspection Summary - QC',1,1,1)
GO
SET IDENTITY_INSERT REF_KPI_Teamplate OFF
GO

---------- ALD-3044 Create a custom KPI for Inspection summary for QC start here ----------------

---ALD-2969 table added task starts-- 
CREATE TABLE [dbo].[CU_PR_City]
(
[Id] INT NOT NULL PRIMARY KEY identity(1,1),
[CU_PR_Id] int not null,
[Factory_City_Id] int not null,
[Active] bit,
[Created_By] INT NULL, 
[Deleted_By] INT NULL, 
[Updated_By] INT NULL, 
[Updated_On] DATETIME, 
[Created_On] DATETIME NOT NULL DEFAULT GETDATE(), 
[Deleted_On] DATETIME NULL,
[Entity_Id] INT NULL,
CONSTRAINT FK_CU_PR_City_Entity_Id  FOREIGN KEY(Entity_Id) REFERENCES [dbo].[AP_Entity](Id),
CONSTRAINT FK_CU_PR_City_CU_PR_Id FOREIGN KEY ([CU_PR_Id]) REFERENCES [dbo].[CU_PR_Details](Id),	
CONSTRAINT FK_CU_PR_City_Factory_City_Id FOREIGN KEY ([Factory_City_Id]) REFERENCES [dbo].[REF_City](Id),	
CONSTRAINT FK_CU_PR_City_Created_By FOREIGN KEY ([Created_By]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_CU_PR_City_Deleted_By FOREIGN KEY ([Deleted_By]) REFERENCES [dbo].[IT_UserMaster](Id),
CONSTRAINT FK_CU_PR_City_Updated_By FOREIGN KEY ([Updated_By]) REFERENCES [dbo].[IT_UserMaster](Id)
)
---ALD-2969 table added task end -- 


---------- ALD-3117 Add more columns in Email send table start here ----------------

ALTER TABLE ES_Details ADD UpdatedBy int null
ALTER TABLE ES_Details ADD UpdatedOn datetime null
ALTER TABLE ES_Details ADD DeletedBy int null
ALTER TABLE ES_Details ADD DeletedOn datetime null
ALTER TABLE ES_Details ADD CONSTRAINT FK_ES_Details_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES IT_UserMaster (Id)
ALTER TABLE ES_Details ADD CONSTRAINT FK_ES_Details_DeletedBy FOREIGN KEY (DeletedBy) REFERENCES IT_UserMaster (Id)

---------- ALD-3117 Add more columns in Email send table end here ----------------

---------- ALD_3119 Customer Man day in Custom KPI starts ---------------------

Insert Into REF_KPI_Teamplate (Name,Active,TypeId,IsDefault)
Values ('Customer Manday', 1, 1, 1)

---------- ALD_3119 Customer Man day in Custom KPI ends---------------------


---------- ALD-3430 Show the Suggested MD in Quotation Booking info starts ---------------------

ALTER TABLE QU_Quotation_Insp ADD SuggestedManday Float Null

---------- ALD-3430 Show the Suggested MD in Quotation Booking info ends ---------------------


----------ALD-3143 Add more sample type in Booking page Starts--------------

insert into REF_SampleType(SampleType,Active) Values ('10%',1)

insert into REF_SampleType(SampleType,Active) Values ('20%',1)

insert into REF_SampleType(SampleType,Active) Values ('30%',1)

insert into REF_SampleType(SampleType,Active) Values ('50%',1)

----------ALD-3143 Add more sample type in Booking page Ends--------------
---- ALD-3219 start 
ALTER TABLE CU_Contact ALTER COLUMN Office INT NULL
---- ALD-3219 end


-----------------------ALD-3228 Manual invoice start ----------------------------

Insert Into REF_Billing_Entity (Name,Active,EntityId) Values ('EAQF Billing Entity',1,3)
Insert Into INV_REF_Bank (AccountName,AccountCurrency,Active,CreatedOn,EntityId,BillingEntity) Values ('EAQF Bank',156,1,GETDATE(),3,11)

INSERT INTO ENT_Master_Type (Name,Active,Sort) Values ('Default EAQF Bank',	1, 26)
INSERT INTO ENT_Master_Type (Name,Active,Sort) Values ('DefaultEAQFBillingEntity', 1, 27)

INSERT INTO ENT_Master_Config (EntityId,CountryId,Type,Value,Active) Values (3,null,26,'1',1)
INSERT INTO ENT_Master_Config (EntityId,CountryId,Type,Value,Active) Values (3,null,27,'28',1)

ALTER TABLE INV_MAN_Transaction ADD BookingNo int null
ALTER TABLE INV_MAN_Transaction 
ADD CONSTRAINT FK_INV_MAN_Transaction_BookingNo FOREIGN KEY(BookingNo) REFERENCES INSP_Transaction(Id)

ALTER TABLE INV_MAN_TRAN_Details ADD Discount FLOAT null,UnitPrice FLOAT null, Manday FLOAT null

CREATE TABLE INV_REF_PaymentMode(
Id int not null primary key identity(1,1),
Name nvarchar(200),
Active bit NOT NULL,
Sort int NOT NULL
);

ALTER TABLE INV_MAN_Transaction 
ADD 
PaymentMode int,
PaymentRef nvarchar(200),
CONSTRAINT INV_MAN_Transaction_PaymentMode FOREIGN KEY(PaymentMode) REFERENCES INV_REF_PaymentMode(Id)


-----------------------ALD-3228 Manual invoice end ----------------------------


--------------------ALD-2970 AUD_Transaction table starts----------------------------
ALTER TABLE AUD_Transaction ADD
FBMissionId INT,
FBReportId INT,
FillingStatus INT,
FBMissionTitle NVARCHAR(3000),
FBReportTitle NVARCHAR(3000),
FBFillingStatus INT, 
FBReviewStatus INT, 
FBReportStatus INT,  
CONSTRAINT AUD_Transaction_FBFillingStatus FOREIGN KEY(FBFillingStatus) REFERENCES FB_Status(Id),
CONSTRAINT AUD_Transaction_FBReviewStatus FOREIGN KEY(FBReviewStatus) REFERENCES FB_Status(Id),
CONSTRAINT AUD_Transaction_FBReportStatus FOREIGN KEY(FBReportStatus) REFERENCES FB_Status(Id),
CONSTRAINT AUD_Transaction_FillingStatus FOREIGN KEY (FillingStatus) REFERENCES FB_Status(Id)


CREATE TABLE LOG_Booking_FB_Queue(
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[BookingId] [int] NOT NULL,
	[FbBookingSyncType] [int] NOT NULL,
	[TryCount] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[IsMissionUpdated] [bit] NULL,
	[Status] [int] NOT NULL,
	[EntityId] [int] NOT NULL,
	CONSTRAINT FK_LOG_Booking_FB_Queue_CreatedBy FOREIGN KEY(CreatedBy) REFERENCES IT_UserMaster(Id),
	CONSTRAINT FK_LOG_Booking_FB_Queue_EntityId FOREIGN KEY(EntityId) REFERENCES AP_Entity(Id),
)

Alter Table FB_Booking_RequestLog Add ServiceId int;

-------------------ALD-2970 AUD_Transaction table ends----------------------------

-----------------------ALD-3446 Sale invoice summary page start ----------------------------
declare @InvoiceRightId as int
select top 1 @InvoiceRightId = Id from IT_Right where TitleName = 'Invoice'
declare @RightId as int
INSERT INTO IT_Right (ParentId, TitleName, MenuName, Path, IsHeading, Active, Ranking, ShowMenu) VALUES (@InvoiceRightId, 'Sale Invoice Summary', 'Sale Invoice Summary','saleinvoicesummary',0,1,9,1)
SELECT @RightId = SCOPE_IDENTITY()
INSERT INTO IT_Right_Entity (RightId, EntityId,Active) VALUES (@RightId,1,1)
INSERT INTO IT_Right_Entity (RightId, EntityId,Active) VALUES (@RightId,2,1)
INSERT INTO IT_Right_Map (RightId, RightTypeId,Active) VALUES (@RightId,1,1)
INSERT INTO IT_Right_Map (RightId, RightTypeId,Active) VALUES (@RightId,2,1)
-----------------------ALD-3446 Sale invoice summary page end ----------------------------

-- ALD-3490 start--

ALTER  TABLE [LOG_Booking_Report_Email_Queue] ADD [ReportRevision] INT NULL
ALTER  TABLE [LOG_Booking_Report_Email_Queue] ADD [ReportVersion] INT NULL

-- ALD-3490 end--

-------------------------ALD-3228 Manual invoice start--------------------------------------
INSERT INTO ENT_Master_Type (Name,Active,Sort) Values ('DefaultEAQFInvoiceOffice', 1, 28)
INSERT INTO ENT_Master_Config (EntityId,CountryId,Type,Value,Active) Values (3,null,28,'17',1)
-------------------------ALD-3228 Manual invoice ends--------------------------------------

-----------------------------ALD-3647 Update Fabric value from Float to nvarchar(200) starts-----------------------------------------------
Alter Table FB_Report_FabricDefects Alter Column AcceptanceCriteria nvarchar(200)
Alter Table FB_Report_FabricDefects Alter Column Points100Sqy nvarchar(200)
Alter Table FB_Report_FabricDefects Alter Column LengthOriginal nvarchar(200)
Alter Table FB_Report_FabricDefects Alter Column LengthActual nvarchar(200)
Alter Table FB_Report_FabricDefects Alter Column WeightOriginal nvarchar(200)
Alter Table FB_Report_FabricDefects Alter Column WeightActual nvarchar(200)
Alter Table FB_Report_FabricDefects Alter Column WidthOriginal nvarchar(200)
Alter Table FB_Report_FabricDefects Alter Column WidthActual nvarchar(200)
Alter Table FB_Report_FabricDefects Alter Column Location nvarchar(200)
Alter Table FB_Report_FabricDefects Alter Column Point nvarchar(200)
-----------------------------ALD-3647 Update Fabric value from Float to nvarchar(200)  ends-----------------------------------------------

----------------------------ALD-3593 File uploaded in 'Technical Document' tab is not updating to FB starts-------------------------------
Alter Table AUD_TRAN_File_Attachment Add FbMissionUrlId int 

--when query executed at that time, please pick the constraint name from table
--Table Name:- AUD_TRAN_Auditors
Alter Table AUD_TRAN_Auditors Drop Constraint FK__AUD_TRAN___Creat__4CECDE2A  --CreatedBy column 
Alter Table AUD_TRAN_Auditors Drop Constraint FK__AUD_TRAN___Delet__4DE10263  --DeletedBy column
--Table Name:- AUD_TRAN_Cs
Alter Table AUD_TRAN_Cs Drop Constraint FK__AUD_TRAN___Creat__5399DBB9  --CreatedBy column 
Alter Table AUD_TRAN_Cs Drop Constraint FK__AUD_TRAN___Delet__548DFFF2  --DeletedBy column

Alter Table AUD_Tran_Auditors Add Constraint FK_AUD_TRAN_Auditors_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES IT_UserMaster(Id)
Alter Table AUD_Tran_Auditors Add Constraint FK_AUD_TRAN_Auditors_DeletedBy FOREIGN KEY (DeletedBy) REFERENCES IT_UserMaster(Id)

Alter Table AUD_TRAN_Cs Add Constraint FK_AUD_TRAN_Cs_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES IT_UserMaster(Id)
Alter Table AUD_TRAN_Cs Add Constraint FK_AUD_TRAN_Cs_DeletedBy FOREIGN KEY (DeletedBy) REFERENCES IT_UserMaster(Id)
----------------------------ALD-3593 File uploaded in 'Technical Document' tab is not updating to FB ends-------------------------------


-----------------------ALD-3446 Sale invoice summary page end ----------------------------

-----------------------ALD-3442 Fetch More inspection related Info from FB start----------------------------
ALTER TABLE FB_Report_Details
ADD InspectionStartedDate date NULL,
InspectionSubmittedDate date NULL,
QtyInspected int NULL,
ProductCategory nvarchar(500) NULL,
KeyStyleHighRisk nvarchar(500) NULL,
MasterCartonPackedQuantityCtns nvarchar(500) NULL,
Region nvarchar(500) NULL,
InspectionDurationMins nvarchar(500) NULL,
NumberPOMMeasured int NULL,
DACorrelation_Enabled bit NULL,
DACorrelationEmail nvarchar(500) NULL,
DACorrelationInspectionSampling int NULL,
DACorrelationRate nvarchar(500) NULL,
DACorrelationResult nvarchar(500) NULL,
FactoryTourEnabled bit NULL,
FactoryTourBottleneckProductionStage nvarchar(500) NULL,
FactoryTourNotConductedReason nvarchar(500) NULL,
FactoryTourIrregularitiesIdentified nvarchar(500) NULL

ALTER TABLE FB_Report_Details ALTER COLUMN InspectionStartTime nvarchar(100) NULL
ALTER TABLE FB_REPORT_Details ALTER COLUMN InspectionEndTime nvarchar(100) NULL

CREATE TABLE FB_Report_RDNumbers(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ReportdetailsId] [int] NULL,
	[ProductId] [int] NULL,
	[RDNumber] [nvarchar](500) NULL,
	[CreatedOn] [datetime] NULL,
	[PoId] [int] NULL,
	[PoColorId] [int] NULL,	
	CONSTRAINT FK_FB_Report_RDNumbers_ReportdetailsId FOREIGN KEY (ReportDetailsId) REFERENCES FB_Report_Details(Id),
	CONSTRAINT FK_FB_Report_RDNumbers_ProductId FOREIGN KEY (ProductId) REFERENCES INSP_Product_Transaction(Id),
	CONSTRAINT FK_FB_Report_RDNumbers_PoColorId FOREIGN KEY (PoColorId) REFERENCES INSP_PurchaseOrder_Color_Transaction(Id),
	CONSTRAINT FK_FB_Report_RDNumbers_PoId FOREIGN KEY (PoId) REFERENCES INSP_PurchaseOrder_Transaction(Id)
)

CREATE TABLE FB_Report_PackingPackagingLabelling_Product(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	FbReportdetailsId [int] NULL,
	PackingType int NULL,
	SampleSizeCtns int NULL,
	Critical int NULL,
	Major int NULL,
	Minor int NULL,
	TotalDefectiveUnits int NULL,
	CreatedOn datetime,
	CONSTRAINT FK_FB_Report_PackingPackagingLabelling_Product_FbReportdetailsId FOREIGN KEY (FbReportdetailsId) REFERENCES FB_Report_Details(Id)
)

CREATE TABLE FB_Report_PackingPackagingLabelling_Product_Defect(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	PackingPackagingLabelling_Id [int] NULL,		
	[Code] [nvarchar](500) NULL,
	[RDNumber] [nvarchar](500) NULL,
	PackingType int NULL,
	[Description] [nvarchar](500) NULL,
	[Severity] [nvarchar](500) NULL,
	[Quantity] [int] NULL,
	[CreatedOn] [datetime] NULL,	
	CONSTRAINT FK_FB_Report_PackingPackagingLabelling_Product_Defect_PackingPackagingLabelling_Id FOREIGN KEY (PackingPackagingLabelling_Id) REFERENCES FB_Report_PackingPackagingLabelling_Product(Id)
)


CREATE TABLE [dbo].[FB_Report_QualityPlan](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY ,
	[FbReportDetailsId] [int] NULL,
	[Title] [nvarchar](500) NULL,
	[TotalDefectiveUnits] [int] NULL,
	[Result] [nvarchar](500) NULL,
	[TotalQtyMeasurmentDefects] [int] NULL,
	[CreatedOn] [datetime] NULL,
	CONSTRAINT [FK_FB_Report_QualityPlan_FbReportDetailsId] FOREIGN KEY([FbReportDetailsId]) REFERENCES [dbo].[FB_Report_Details] ([Id])
)

CREATE TABLE [dbo].[FB_Report_QualityPlan_MeasurementDefectsPOM](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[QualityPlanId] [int] NULL,
	[CodePOM] [nvarchar](500) NULL,
	[POM] [nvarchar](500) NULL,
	[CriticalPOM] [nvarchar](500) NULL,
	[Quantity] [int] NULL,
	[SpecZone] [nvarchar](500) NULL,
	CONSTRAINT [FB_Report_QualityPlan_MeasurementDefectsPOM_QualityPlanId] FOREIGN KEY([QualityPlanId]) REFERENCES [dbo].[FB_Report_QualityPlan] ([Id])
)

CREATE TABLE [dbo].[FB_Report_QualityPlan_MeasurementDefectsSize](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[QualityPlanId] [int] NULL,
	[Size] [nvarchar](500) NULL,
	[Quantity] [int] NULL,
	CONSTRAINT [FB_Report_QualityPlan_MeasurementDefectsSize_QualityPlanId] FOREIGN KEY([QualityPlanId]) REFERENCES [dbo].[FB_Report_QualityPlan] ([Id])
)
-----------------------ALD-3442 Fetch More inspection related Info from FB end----------------------------

---------------------------ALD-2967 Picking Enhancement Starts---------------------------------

ALTER TABLE INSP_TRAN_Picking ADD BookingId INT

ALTER TABLE INSP_TRAN_Picking ADD CONSTRAINT FK_INSP_PICKING_BOOKING_ID FOREIGN KEY(BookingId) 
REFERENCES INSP_TRANSACTION(Id)


--Update the picking history records Starts-----------

update picking set BookingId=purchaseOrder.Inspection_Id
from INSP_TRAN_Picking picking
join INSP_PurchaseOrder_Transaction purchaseOrder
on picking.PO_Tran_Id=purchaseOrder.Id

ALTER TABLE INSP_TRAN_Picking_Contacts ADD CONSTRAINT FK_INSP_PICKING_LAB_CONTACT FOREIGN KEY(Lab_Contact_Id)
REFERENCES INSP_LAB_Contact(Id)

ALTER TABLE INSP_TRAN_Picking ADD CONSTRAINT FK_INSP_PICKING_LAB_ADDRESS_VALUE FOREIGN KEY(Lab_Address_Id)
REFERENCES INSP_LAB_Address(Id)

ALTER TABLE INSP_TRAN_Picking ADD CONSTRAINT FK_INSP_PICKING_CUST_ADDRESS FOREIGN KEY(Cus_Address_Id)
REFERENCES Cu_Address(Id)

Update INSP_TRAN_Picking set Lab_Id=null where Lab_Address_Id is null
Update INSP_TRAN_Picking set Customer_Id=null where Cus_Address_Id is null

--Update the picking history records Ends-------------

---------------------------ALD-2967 Picking Enhancement Ends---------------------------------

---------- ALD_3180 Add payment Terms in Quotation -SL20-419 starts---------------------

ALTER TABLE [dbo].[QU_QUOTATION]
ADD [PaymentTermsValue] nvarchar(200),
	[PaymentTermsCount] int;

---------- ALD_3180 Add payment Terms in Quotation -SL20-419 ends---------------------

---------------------------ALD-3468 Schedule Email to CS Starts---------------------------------
SET IDENTITY_INSERT JOB_Schedule_Job_Type ON 
GO
INSERT INTO JOB_Schedule_Job_Type (Id,[Name],Active,Sort) VALUES (13,'SchedulePlanningForCS',1,1)
GO
SET IDENTITY_INSERT JOB_Schedule_Job_Type OFF
GO

INSERT INTO JOB_Schedule_Configuration ([Name],[Type],[To],[CC],StartDate,ScheduleInterval,EntityId,Active) 
VALUES ('Schedule Planning For CS',13,'sabarimalai_alagar@api-hk.com;nixon.antony@sgtgroup.net;sreejin_a1_s@api-hk.com','sabarimalai_alagar@api-hk.com;nixon.antony@sgtgroup.net',GETDATE(),3,1,1)
---------------------------ALD-3468 Schedule Email to CS Ends---------------------------------


------------------------ ALD - 3173 Data Management Enhancement Start -----------------------------------

CREATE TABLE DM_Role(
Id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
RoleId int,
StaffId int,
EditRight bit NOT NULL,
DownloadRight bit NOT NULL,
DeleteRight bit NOT NULL,
UploadRight bit NOT NULL,  
EntityId int,
CreatedBy int NULL,
CreatedOn datetime NULL,
UpdatedBy int NULL,
UpdatedOn datetime NULL,
Active bit NULL,
DeletedBy int NULL,
DeletedOn datetime NULL,
CONSTRAINT FK_DM_Role_EntityId FOREIGN KEY(EntityId) REFERENCES AP_Entity(Id),
CONSTRAINT FK_DM_Role_RoleId FOREIGN KEY(RoleId) REFERENCES IT_Role(Id),
CONSTRAINT FK_DM_Role_StaffId FOREIGN KEY(StaffId) REFERENCES HR_Staff(Id),
CONSTRAINT FK_DM_Role_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES IT_UserMaster(Id),
CONSTRAINT FK_DM_Role_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES IT_UserMaster(Id),
CONSTRAINT FK_DM_Role_DeletedyBy FOREIGN KEY (DeletedBy) REFERENCES IT_UserMaster(Id)
)

ALTER TABLE DM_RIGHT ADD DM_RoleId int,
CONSTRAINT FK_DM_RIGHT_DM_RoleId FOREIGN KEY(DM_RoleId) REFERENCES DM_Role(Id)

CREATE TABLE DM_Brand(
Id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
BrandId INT NOT NULL,
DMFileId INT,
CONSTRAINT FK_DM_Brand_BrandId FOREIGN KEY(BrandId) REFERENCES CU_Brand(Id),
CONSTRAINT FK_DM_Brand_DMFileId FOREIGN KEY(DMFileId) REFERENCES DM_File(Id)
)

CREATE TABLE DM_Department(
Id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
DepartmentId INT NOT NULL,  
DMFileId INT,
CONSTRAINT FK_DM_Department_DepartmentId FOREIGN KEY(DepartmentId) REFERENCES CU_Department(Id),
CONSTRAINT FK_DM_Department_DMFileId FOREIGN KEY(DMFileId) REFERENCES DM_File(Id)
)

Insert Into IT_Right_Map (RightId,RightTypeId,Active) Values (140,8,1) --based on the production map the right id

Insert Into IT_Right_Entity (RightId,EntityId,CustomerId,Active) Values (140,1,null,1), (140,2,null,1) --based on the production map the right id
------------------------ ALD - 3173 Data Management Enhancement End -------------------------------------


--based on the production select the id
Update IT_Right Set MenuName = 'Document Access Right', Path='datamanagement/dmusermanagersummary' Where Id=122
------------------------ ALD - 3173 Data Management Enhancement End -------------------------------------

-------------------------ALD-3869 'Factory Country' not displaying correctly in email subject field - LINK 950 starts -----------------------------
Update ES_SU_PreDefined_Fields Set IsText=null Where Field_Name ='FactoryCountry'
-------------------------ALD-3869 'Factory Country' not displaying correctly in email subject field - LINK 950 ends -----------------------------


------------------------ALD-3897 Map the Gap defects to Defect table starts------------------------------------------------------------------
Alter Table FB_Report_InspDefects Add DefectCheckPoint int 

EXEC sp_addextendedproperty 
    @name = N'MS_Description', @value = 'Defect Check point type value:- 1. Workmanship, 2. Packing',
    @level0type = N'Schema',   @level0name = 'dbo',
    @level1type = N'Table',    @level1name = 'FB_Report_InspDefects',
    @level2type = N'Column',   @level2name = 'DefectCheckPoint';
GO
------------------------ALD-3897 Map the Gap defects to Defect table ends--------------------------------------------------------------------


--------------------------------ALD-3991 Sync Gap Sample size & Validated time between FB & Link starts------------------------------------------------------------
Alter Table FB_Report_Details Add 
FillingValidatedFirstTime nvarchar (100), 
ReviewValidatedFirstTime nvarchar (100), 
DACorrelationDone nvarchar (100),
FactoryTourDone nvarchar (100),
FactoryTourResult  nvarchar (100),
ExternalReportNumber nvarchar(200)

Alter Table FB_Report_QualityPlan Add
TotalPiecesMeasurmentDefects nvarchar(100),
SampleInspected nvarchar(100),
ActualMeasuredSampleSize nvarchar(100)

--------------------------------ALD-3991 Sync Gap Sample size & Validated time between FB & Link ends------------------------------------------------------------

--------------------------------ALD-4003 Create a new custom KPI template for GAP starts------------------------------------------------------------------------

Insert Into REF_KPI_Teamplate (Name,Active,TypeId,IsDefault) Values ('Gap KPI',1,1,NULL)

Insert Into REF_KPI_Teamplate_Customer (TeamplateId,CustomerId,Active)
Values (43,568,1)

--------------------------------ALD-4003 Create a new custom KPI template for GAP ends------------------------------------------------------------------------
------------------------ALD-3897 Map the Gap defects to Defect table ends--------------------------------------------------------------------
-------------------------ALD-3869 'Factory Country' not displaying correctly in email subject field - LINK 950 ends -----------------------------
Insert Into IT_Right_Entity (RightId,EntityId,CustomerId,Active) Values (140,1,null,1), (140,2,null,1) --based on the production map the right id
------------------------ ALD - 3173 Data Management Enhancement End -------------------------------------

------------------------ ALD-3440 Fetch Audit Information from FB to Link start -------------------------------------
ALTER TABLE AUD_Transaction ADD 
ScoreValue nvarchar(100),
Scorepercentage nvarchar(100),
Grade nvarchar(100),
ReportRemarks nvarchar(4000),
FinalReportPath nvarchar(500),
PictureReportPath nvarchar(500);

CREATE TABLE Aud_FB_Report_Checkpoints(
	Id int PRIMARY KEY IDENTITY(1,1) NOT NULL,
	AuditId int,
	ChekPointName nvarchar(1000),
	ScoreValue nvarchar(100),
	ScorePercentage nvarchar(100),
	Grade nvarchar(100),
	Remarks nvarchar(4000),
	Major nvarchar(500),
	Minor nvarchar(500),
	ZeroTolerance nvarchar(500),
	MaxPoint nvarchar(500),
	CreatedOn datetime,
	CreatedBy int,
	Active bit,
	DeletedOn datetime,
	DeletedBy int,
	CONSTRAINT FK_Aud_FB_Report_Checkpoints_AuditId FOREIGN KEY(AuditId) REFERENCES AUD_Transaction (Id),
	CONSTRAINT FK_Aud_FB_Report_Checkpoints_CreatedBy FOREIGN KEY(CreatedBy) REFERENCES IT_UserMaster (Id),
	CONSTRAINT FK_Aud_FB_Report_Checkpoints_DeletedBy FOREIGN KEY(DeletedBy) REFERENCES IT_UserMaster (Id)
);
------------------------ ALD-3440 Fetch Audit Information from FB to Link end -------------------------------------

------------------------ ALD-3698 OCR MS chart Integration with Link start -------------------------------------
Update CU_Product_FileType set Name = 'MS Chart Excel' where Name = 'MS chart'
Update CU_Product_FileType set Name = 'Product Ref. Pictures' where Name = 'Pictures'
INSERT INTO CU_Product_FileType (Name, Sort, Active) VALUES ('MS Chart Pdf',1,1)

CREATE TABLE CU_Product_MSChart_OCR_MAP (
	Id int PRIMARY KEY IDENTITY(1,1) NOT NULL,
	CustomerId int NOT NULL,
	OCR_CustomerName nvarchar(500) NULL,
	OCR_FileFormat nvarchar(500) NULL,
	Active bit NOT NULL,
	CreatedBy int NOT NULL,
	CreatedOn datetime NOT NULL,
	DeletedBy int NULL,
	DeletedOn datetime NULL,
	CONSTRAINT FK_CU_Product_MSChart_OCR_MAP_CustomerId FOREIGN KEY(CustomerId) REFERENCES CU_Customer (Id),
	CONSTRAINT FK_CU_Product_MSChart_OCR_MAP_CreatedBy FOREIGN KEY(CreatedBy) REFERENCES IT_UserMaster (Id),
	CONSTRAINT FK_CU_Product_MSChart_OCR_MAP_DeletedBy FOREIGN KEY(DeletedBy) REFERENCES IT_UserMaster (Id)
)
------------------------ ALD-3698 OCR MS chart Integration with Link end -------------------------------------
------------------------ ALD-3809 start -------------------------------------

insert into INSP_Cancel_Reasons(Reason,IsDefault,IsAPI,Active,EntityId)values('Request by E-AQF',1,0,1,3)
insert into dbo.INSP_Reschedule_Reasons(Reason,IsDefault,IsAPI,Active,EntityId) values('Request By Eaqf',1,1,1,3)

------------------------ ALD-3809 end -------------------------------------


--------------------ALD-3815 GAP Enhancement in booking starts -----------------------

ALTER TABLE INSP_Transaction ADD GAPDACorrelation BIT NULL

ALTER TABLE INSP_Transaction ADD GAPDAName NVARCHAR(500) NULL

ALTER TABLE INSP_Transaction ADD GAPDAEmail NVARCHAR(500) NULL

--------------------ALD-3815 GAP Enhancement in booking ends -----------------------


---------------ALD-3874 Enable the Man day column for "Man day cost" type in Expense claim Starts----

ALTER TABLE EC_ExpensesTypes ADD IsOutsource BIT

ALTER TABLE EC_ExpensesTypes ADD IsPermanent BIT

Update EC_ExpensesTypes set IsPermanent=1

---------------ALD-3874 Enable the Man day column for "Man day cost" type in Expense claim Ends----

---------------ALD-3996 Critical'column is missing in 'Aud_FB_Report_Checkpoints' db table starts----
ALTER TABLE Aud_FB_Report_Checkpoints ADD Critical nvarchar(500)
---------------ALD-3996 Critical'column is missing in 'Aud_FB_Report_Checkpoints' db table end----
--------------------ALD-3815 GAP Enhancement in booking ends -----------------------


------------------------ ALD-3802 Add the product category based on Service type and customer in Audit booking start -------------------------------------
CREATE TABLE [dbo].[AUD_CU_ProductCategory](
	[Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NULL,
	[ServiceType] [int] NULL,
	[CustomerId] [int] NULL,
	[Active] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[DeletedBy] [int] NULL,
	[DeletedOn] [datetime] NULL,
	[EntityId] [int] NULL,
	[FB_Name] [nvarchar](200) NULL,
	CONSTRAINT [FK_AUD_CU_ProductCategory_ServiceType] FOREIGN KEY([ServiceType]) REFERENCES [dbo].[REF_ServiceType] ([Id]),
	CONSTRAINT [FK_AUD_CU_ProductCategory_CustomerId] FOREIGN KEY([CustomerId]) REFERENCES [dbo].[CU_Customer] ([Id]),
	CONSTRAINT [FK_AUD_CU_ProductCategory_CreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_AUD_CU_ProductCategory_DeletedBy] FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_AUD_CU_ProductCategory_EntityId] FOREIGN KEY([EntityId]) REFERENCES [dbo].[AP_Entity] ([Id])
)

ALTER TABLE [dbo].[AUD_Transaction] ADD [CU_ProductCategory] [int] NULL
ALTER TABLE [dbo].[AUD_Transaction] ADD CONSTRAINT [FK_AUD_Transaction_CU_ProductCategory] FOREIGN KEY([CU_ProductCategory])
REFERENCES [dbo].[AUD_CU_ProductCategory] ([Id])

SET IDENTITY_INSERT REF_ServiceType ON GO
INSERT REF_ServiceType ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation], [ServiceId], [BusinessLineId], [ShowServiceDateTo], [IsAutoQCExpenseClaim], [Sort]) VALUES (127, N'Process Audit', 1, 2, 0, 925, NULL, 2, NULL, 1, 1, 9)
INSERT REF_ServiceType ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation], [ServiceId], [BusinessLineId], [ShowServiceDateTo], [IsAutoQCExpenseClaim], [Sort]) VALUES (128, N'Flash Process Audit', 1, 2, 0, 924, NULL, 2, NULL, 1, 1, 9)
INSERT REF_ServiceType ([Id], [Name], [Active], [EntityId], [IsReInspectedService], [Fb_ServiceType_Id], [Abbreviation], [ServiceId], [BusinessLineId], [ShowServiceDateTo], [IsAutoQCExpenseClaim], [Sort]) VALUES (129, N'Automation Index Audit', 1, 2, 0, 922, NULL, 2, NULL, 1, 1, 9)
SET IDENTITY_INSERT REF_ServiceType OFF GO

INSERT CU_ServiceType ([CustomerId], [ServiceId], [ServiceTypeId], [Active], [PickType], [LevelPick1], [LevelPick2], [CriticalPick1], [CriticalPick2], [MajorTolerancePick1], [MajorTolerancePick2], [MinorTolerancePick1], [MinorTolerancePick2], [AllowAQLModification], [DefectClassification], [CheckMeasurementPoints], [ReportUnit], [ProductCategoryId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [DeletedBy], [DeletedOn], [CustomServiceTypeName], [EntityId], [CustomerRequirementIndex], [DP_Point], [IgnoreAcceptanceLevel]) VALUES (17, 2, 129, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, 5, GETDATE(), NULL, NULL, NULL, NULL, NULL, 2, 1, NULL, NULL)
INSERT CU_ServiceType ([CustomerId], [ServiceId], [ServiceTypeId], [Active], [PickType], [LevelPick1], [LevelPick2], [CriticalPick1], [CriticalPick2], [MajorTolerancePick1], [MajorTolerancePick2], [MinorTolerancePick1], [MinorTolerancePick2], [AllowAQLModification], [DefectClassification], [CheckMeasurementPoints], [ReportUnit], [ProductCategoryId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [DeletedBy], [DeletedOn], [CustomServiceTypeName], [EntityId], [CustomerRequirementIndex], [DP_Point], [IgnoreAcceptanceLevel]) VALUES (17, 2, 128, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, 5, GETDATE(), NULL, NULL, NULL, NULL, NULL, 2, 1, NULL, NULL)
INSERT CU_ServiceType ([CustomerId], [ServiceId], [ServiceTypeId], [Active], [PickType], [LevelPick1], [LevelPick2], [CriticalPick1], [CriticalPick2], [MajorTolerancePick1], [MajorTolerancePick2], [MinorTolerancePick1], [MinorTolerancePick2], [AllowAQLModification], [DefectClassification], [CheckMeasurementPoints], [ReportUnit], [ProductCategoryId], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [DeletedBy], [DeletedOn], [CustomServiceTypeName], [EntityId], [CustomerRequirementIndex], [DP_Point], [IgnoreAcceptanceLevel]) VALUES (59, 2, 127, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, 5, GETDATE(), NULL, NULL, NULL, NULL, NULL, 2, 1, NULL, NULL)

INSERT AUD_CU_ProductCategory ([Name], [ServiceType], [CustomerId], [Active], [CreatedBy], [CreatedOn], [DeletedBy], [DeletedOn], [EntityId], [FB_Name]) VALUES (N'Denim', 129, 17, 1, 556, CAST(N'2023-02-14T01:16:51.863' AS DateTime), NULL, NULL, 2, N'denim')
INSERT AUD_CU_ProductCategory ([Name], [ServiceType], [CustomerId], [Active], [CreatedBy], [CreatedOn], [DeletedBy], [DeletedOn], [EntityId], [FB_Name]) VALUES (N'Knit', 129, 17, 1, 556, CAST(N'2023-02-14T01:16:51.867' AS DateTime), NULL, NULL, 2, N'knit')
INSERT AUD_CU_ProductCategory ([Name], [ServiceType], [CustomerId], [Active], [CreatedBy], [CreatedOn], [DeletedBy], [DeletedOn], [EntityId], [FB_Name]) VALUES (N'Accessory Hardlines', 128, 17, 1, 556, CAST(N'2023-02-14T01:36:24.120' AS DateTime), NULL, NULL, 2, N'accessory_hardlines')
INSERT AUD_CU_ProductCategory ([Name], [ServiceType], [CustomerId], [Active], [CreatedBy], [CreatedOn], [DeletedBy], [DeletedOn], [EntityId], [FB_Name]) VALUES (N'Cut N Sewn', 128, 17, 1, 556, CAST(N'2023-02-14T01:36:24.123' AS DateTime), NULL, NULL, 2, N'cut_n_sewn')
INSERT AUD_CU_ProductCategory ([Name], [ServiceType], [CustomerId], [Active], [CreatedBy], [CreatedOn], [DeletedBy], [DeletedOn], [EntityId], [FB_Name]) VALUES (N'Footwear', 128, 17, 1, 556, CAST(N'2023-02-14T01:36:24.123' AS DateTime), NULL, NULL, 2, N'footwear')
INSERT AUD_CU_ProductCategory ([Name], [ServiceType], [CustomerId], [Active], [CreatedBy], [CreatedOn], [DeletedBy], [DeletedOn], [EntityId], [FB_Name]) VALUES (N'Sweater', 128, 17, 1, 556, CAST(N'2023-02-14T01:36:24.127' AS DateTime), NULL, NULL, 2, N'sweater')
INSERT AUD_CU_ProductCategory ([Name], [ServiceType], [CustomerId], [Active], [CreatedBy], [CreatedOn], [DeletedBy], [DeletedOn], [EntityId], [FB_Name]) VALUES (N'Footwear', 127, 59, 1, 556, CAST(N'2023-02-14T01:23:54.827' AS DateTime), NULL, NULL, 2, N'footwear')
INSERT AUD_CU_ProductCategory ([Name], [ServiceType], [CustomerId], [Active], [CreatedBy], [CreatedOn], [DeletedBy], [DeletedOn], [EntityId], [FB_Name]) VALUES (N'Sweater', 127, 59, 1, 556, CAST(N'2023-02-14T01:27:01.353' AS DateTime), NULL, NULL, 2, N'sweater')
------------------------ ALD-3802 Add the product category based on Service type and customer in Audit booking end -------------------------------------

--ALD-3463 start --- 

CREATE TABLE [dbo].[Ref_ServiceType_Xero]
    (
		[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, 
		[TrackingOptionName] [nvarchar](500) NULL,
		[XERO_Account] [nvarchar](500) NULL,
		[Inspection_Type_consolidate] [nvarchar](500) NULL,
		[Inspection_ServiceType_Id] [int] NULL,
		[Inspection_Type] [nvarchar](500) NULL,
		[Entity_Id] [INT] NULL,
		[Active] [bit] NULL,
		[TrackingOptionName_Travel] [nvarchar](500) NULL,
		CONSTRAINT FK_Ref_ServiceType_Xero_Entity_Id FOREIGN KEY(Entity_Id) REFERENCES [dbo].[AP_Entity],
		CONSTRAINT FK_Ref_ServiceType_Xero_Inspection_ServiceType_Id 
		FOREIGN KEY(Inspection_ServiceType_Id) REFERENCES [dbo].[REF_ServiceType]
	)

	Insert into [dbo].[Ref_ServiceType_Xero](TrackingOptionName,XERO_Account,Inspection_Type_consolidate,
	Inspection_ServiceType_Id,Inspection_Type,Entity_Id,Active,TrackingOptionName_Travel)
	Values('Inspection-Income','700000','Inspection',5,'Indus',1,1,'Inspection Other - Income')

	Insert into [dbo].[Ref_ServiceType_Xero](TrackingOptionName,XERO_Account,Inspection_Type_consolidate,
	Inspection_ServiceType_Id,Inspection_Type,Entity_Id,Active,TrackingOptionName_Travel)
	Values('Inspection-Income','700000','Inspection',9,'Indus',1,1,'Inspection Other - Income')

	Insert into [dbo].[Ref_ServiceType_Xero](TrackingOptionName,XERO_Account,Inspection_Type_consolidate,
	Inspection_ServiceType_Id,Inspection_Type,Entity_Id,Active,TrackingOptionName_Travel)
	Values('Inspection-Income','700000','Inspection',15,'Indus',1,1,'Inspection Other - Income')

--ALD-3463 end --- 

insert into REF_KPI_Teamplate values('Xero Invoice', 1,1,1)
------------------------ ALD-3802 Add the product category based on Service type and customer in Audit booking end -------------------------------------

------------------------ ALD-3949 Add GAP inspection platform in filling & review page start -------------------------------------

declare @DF_DDL_SourceTypeId as int
Insert Into DF_DDL_SourceType (Name,Active) values ('GAP-Inspection Platform',1)
SELECT @DF_DDL_SourceTypeId = SCOPE_IDENTITY()

INSERT Into DF_DDL_Source (Name,Type,Active) VALUES ('PIVOT 88', @DF_DDL_SourceTypeId, 1)
INSERT Into DF_DDL_Source (Name,Type,Active) VALUES ('SMEE', @DF_DDL_SourceTypeId, 1)
INSERT Into DF_DDL_Source (Name,Type,Active) VALUES ('N/A', @DF_DDL_SourceTypeId, 1)

declare @CU_CustomerId as int
select top 1 @CU_CustomerId = Id from CU_Customer where Customer_Name Like '%Gap%' and Active = 1

Insert Into DF_CU_DDL_SourceType (CustomerId, TypeId, Active) values (@CU_CustomerId, @DF_DDL_SourceTypeId, 1)

declare @DF_CU_ConfigurationId as int
INSERT Into DF_CU_Configuration (CustomerId, ModuleId, ControlTypeId, Label, Type, DataSourceType, DisplayOrder, Active, CreatedBy, CreatedOn, EntityId) VALUES 
(@CU_CustomerId, 1, 3, 'Inspection platform', NULL, @DF_DDL_SourceTypeId, 2, 1, 2, GETDATE(), 2)
SELECT @DF_CU_ConfigurationId = SCOPE_IDENTITY()

Insert Into DF_Control_Attributes (ControlAttributeId, Value, ControlConfigurationID, Active) values (9, '-- Select --', @DF_CU_ConfigurationId, 1)
Insert Into DF_Control_Attributes (ControlAttributeId, Value, ControlConfigurationID, Active) values (10, 'false', @DF_CU_ConfigurationId, 1)
Insert Into DF_Control_Attributes (ControlAttributeId, Value, ControlConfigurationID, Active) values (11, 'false', @DF_CU_ConfigurationId, 1)
Insert Into DF_Control_Attributes (ControlAttributeId, Value, ControlConfigurationID, Active) values (12, 'false', @DF_CU_ConfigurationId, 1)
Insert Into DF_Control_Attributes (ControlAttributeId, Value, ControlConfigurationID, Active) values (13, 'false', @DF_CU_ConfigurationId, 1)
Insert Into DF_Control_Attributes (ControlAttributeId, Value, ControlConfigurationID, Active) values (14, 'false', @DF_CU_ConfigurationId, 1)

------------------------ ALD-3949 Add GAP inspection platform in filling & review page end -------------------------------------

-------------------------- ALD-4087 AR Follow Up report Starts -----------------------------

insert into REF_KPI_Teamplate(Name,Active,TypeId,IsDefault) Values('AR Follow Up Report',1,1,1)

-------------------------- ALD-4087 AR Follow Up report Ends -----------------------------
------------------------ ALD-3949 Add GAP inspection platform in filling & review page end -------------------------------------


------------------------ -----------------------ALD-4050 Enhancement for Pre- Invoice Send starts--------------------------------------------------
Alter Table ES_Details Add InvoiceTypeId int
Alter Table ES_Details ADD CONSTRAINT FK_ES_Details_REF_InvoiceType_InvoiceTypeId FOREIGN KEY (InvoiceTypeId) REFERENCES REF_InvoiceType(Id)
------------------------ -----------------------ALD-4050 Enhancement for Pre- Invoice Send ends----------------------------------------------------


-------------------------ALD-2816 Allow to reschedule the Inspection by Entity without cancel the quotation start -----------------------------------

INSERT INTO [dbo].ENT_Master_Type( Name, Active, Sort) VALUES ('Reschedule the booking without cancel the quotation', 1, 29 );

INSERT INTO [dbo].ENT_Master_Config( EntityId, CountryId, Type, Value, Active) VALUES (1, NULL, 29, NULL, 1 );

-------------------------ALD-2816 Allow to reschedule the Inspection by Entity without cancel the quotation end -----------------------------------

------------------------ALD-4036 Add additional data Sync for GAP starts-----------------------------------------------------
ALTER TABLE FB_Report_Details ADD 
ReportType nvarchar(200), 
Origin nvarchar(100), 
ShipMode nvarchar(100), 
Othercategory nvarchar(200),
Market  nvarchar(200),
TotalScore nvarchar(100),
Grade nvarchar(100), 
LastAuditScore nvarchar(100)

ALTER TABLE FB_Report_InspSummary ADD
ScoreValue nvarchar(100),
ScorePercentage nvarchar(100) 

ALTER TABLE FB_Report_PackingPackagingLabelling_Product ADD
CartonQty INT

ALTER TABLE AUD_Transaction ADD
ExternalReportNo nvarchar(100), 
AuditStartTime nvarchar(50),AuditEndTime nvarchar(50),
MainCategory nvarchar(100), 
OtherCategory nvarchar(100),
Market nvarchar(200),
FillingValidatedFirstTime nvarchar(100),
ReviewValidatedFirstTime nvarchar(100),
LastAuditScore nvarchar(100)

ALTER TABLE AUD_TRAN_CS ADD IsReport bit

------------------------ALD-4036 Add additional data Sync for GAP ends-----------------------------------------------------

------------------------ALD-4086 GAP KPI starts-----------------------------------------------------
Insert Into REF_KPI_Teamplate (Name,Active,TypeId,IsDefault) Values ('Flash Process Audit',1,1,NULL)
Insert Into REF_KPI_Teamplate_Customer (TeamplateId,CustomerId,Active)
Values (47,568,1)

Insert Into REF_KPI_Teamplate (Name,Active,TypeId,IsDefault) Values ('Gap Audit',1,1,NULL)
Insert Into REF_KPI_Teamplate_Customer (TeamplateId,CustomerId,Active)
Values (48,568,1)
------------------------ALD-4086 GAP KPI ends-----------------------------------------------------
------------------------ ALD-3949 Add GAP inspection platform in filling & review page end -------------------------------------

---ALD-3965 start-----------------

CREATE TABLE [dbo].[Hr_Staff_XeroDept]
(
	[Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[DeptName] [nvarchar](250) NULL,
	[Active] [int] NULL
)

ALTER TABLE HR_Staff ADD [Xero_DeptId] INT NULL

ALTER TABLE HR_Staff ADD CONSTRAINT HR_Staff_Xero_DeptId  FOREIGN KEY(Xero_DeptId) REFERENCES [Hr_Staff_XeroDept](Id)

ALTER TABLE EC_ExpensesTypes ADD [Xero_AccountCode] nvarchar(2000) NULL

ALTER TABLE EC_ExpensesTypes ADD [Xero_OutSource_AccountCode] nvarchar(2000) NULL

update EC_ExpensesTypes set Xero_AccountCode='800700' where id in (1,2,3,4,5,6,7,8,9,10)
update EC_ExpensesTypes set Xero_AccountCode='804700' where id in (11,12,13,14,15,16,17)
update EC_ExpensesTypes set Xero_AccountCode='803100' where id in (18,19)
update EC_ExpensesTypes set Xero_AccountCode='801720' where id=20
update EC_ExpensesTypes set Xero_AccountCode='801720' where id=21
update EC_ExpensesTypes set Xero_OutSource_AccountCode='801720' where id=21
update EC_ExpensesTypes set Xero_AccountCode='803200' where id=22

update EC_ExpensesTypes set Xero_AccountCode='801500' where id=23

update EC_ExpensesTypes set Xero_AccountCode='801500' where id=24
update EC_ExpensesTypes set Xero_AccountCode='801500' where id=25

update EC_ExpensesTypes set Xero_AccountCode='801820' where id=26
update EC_ExpensesTypes set Xero_OutSource_AccountCode='801820' where id=26

ALTER TABLE  INV_REF_Bank ADD TaxNameInXero nvarchar(2000) NULL
update inv_ref_bank set TaxNameInXero='Tax Exempt (0%)'

---ALD-3965 end-----------------

-----------------------------------ALD-4209 On Email subject fileds, 'Invoice #' field not showing Invoice number when try to send email starts-------------------------

--Update Invoice field data for all three entities
Update ES_SU_PreDefined_Fields Set DataType=1,IsText=Null Where Id IN (48,53,82)

-----------------------------------ALD-4209 On Email subject fileds, 'Invoice #' field not showing Invoice number when try to send email ends-------------------------

------------------------- ALD-4088 Schedule a KPI Email for BBG start -----------------------------------
Insert Into JOB_Schedule_Job_Type (Name,Active,Sort) values ('BBGInitialBookingExtract',1,1)
ALTER TABLE JOB_Schedule_Configuration ADD CustomerId nvarchar(1500)
Insert Into JOB_Schedule_Configuration (Type,[To],StartDate,ScheduleInterval,FileName,EntityId,Active,CustomerId) values (14,'nixon.antony@sgtgroup.net',GETDATE(),7,'Initial booking extract',1,1,'318')
------------------------- ALD-4088 Schedule a KPI Email for BBG end -----------------------------------

------------------------- ALD-4196 CWF Customer decision enhancement start -----------------------------------

SET IDENTITY_INSERT CU_CheckPointType ON 
GO
Insert Into CU_CheckPointType (Id,Name,Active,Entity_Id) values (15,'Hide multi-select customer decision',1,1)
GO
SET IDENTITY_INSERT CU_CheckPointType OFF
GO

CREATE TABLE CU_Report_CustomerDecisionComment (
	Id [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	CustomerId [int] NOT NULL,
	ReportResult [nvarchar](1000) NULL,
	Comments [nvarchar](max) NULL,
	Active [bit] NOT NULL,
	CONSTRAINT [FK_CU_Customer_CU_Report_CustomerDecisionComment_CustomerId] FOREIGN KEY(CustomerId) REFERENCES CU_Customer (Id)
)

INSERT CU_Report_CustomerDecisionComment ([Id], [CustomerId], [ReportResult], [Comments], [Active]) VALUES (1, 59, N'Pass', N'We accept this consignment, for shipment. This approval doesn''t relieve you from responsibility as per CWF GTCP', 1)
INSERT CU_Report_CustomerDecisionComment ([Id], [CustomerId], [ReportResult], [Comments], [Active]) VALUES (2, 59, N'Quantity', N'Consignment is rejected for Quantity defects. Supplier must sort it out and make necessary reparations. When completed, supplier must call for re-inspection by SGT at 100%', 1)
INSERT CU_Report_CustomerDecisionComment ([Id], [CustomerId], [ReportResult], [Comments], [Active]) VALUES (3, 59, N'Workmanship', N'Consignment is rejected for Workmanship defects. Supplier must sort it out and make necessary reparations. When completed, supplier must call for re-inspection by SGT at 10%', 1)
INSERT CU_Report_CustomerDecisionComment ([Id], [CustomerId], [ReportResult], [Comments], [Active]) VALUES (4, 59, N'Product specifications', N'Consignment is rejected for Product specifications defects. Supplier must sort it out and make necessary reparations. When completed, supplier must call for re-inspection by SGT at 10%', 1)
INSERT CU_Report_CustomerDecisionComment ([Id], [CustomerId], [ReportResult], [Comments], [Active]) VALUES (5, 59, N'Packing', N'Consignment is rejected for Packing defects. Supplier must sort it out and make necessary reparations. When completed, supplier must call for re-inspection by SGT at 10%', 1)
INSERT CU_Report_CustomerDecisionComment ([Id], [CustomerId], [ReportResult], [Comments], [Active]) VALUES (6, 59, N'Onsite tests', N'Consignment is rejected for Onsite tests defects. Supplier must sort it out and make necessary reparations. When completed, supplier must call for re-inspection by SGT at 10%', 1)
INSERT CU_Report_CustomerDecisionComment ([Id], [CustomerId], [ReportResult], [Comments], [Active]) VALUES (7, 59, N'Other information', N'Consignment is rejected for Other information defects. Supplier must sort it out and make necessary reparations. When completed, supplier must call for re-inspection by SGT at 10%', 1)
INSERT CU_Report_CustomerDecisionComment ([Id], [CustomerId], [ReportResult], [Comments], [Active]) VALUES (8, 59, N'Pending', N'', 1)
------------------------- ALD-4088 CWF Customer decision enhancement start -----------------------------------

------------------------- ALD-4198 Map 100% to service type under AQL start -----------------------------------
ALTER TABLE REF_ServiceType ADD Is100Inspection BIT
------------------------- ALD-4198 Map 100% to service type under AQL end -----------------------------------

------------------------- ALD-4232 derogated customer decision start -----------------------------------
Update REF_INSP_CUS_Decision_Config set CustomDecisionName = 'Derogated', [Default] = 1, CusDecId = 4, CustomerId = null where Id = 4
------------------------- ALD-4232 derogated customer decision end -----------------------------------

------------------------- ALD-4188 Show Default KPI templates to customer start -----------------------------------
ALTER TABLE REF_KPI_Teamplate ADD IsDefaultCustomer BIT
------------------------- ALD-4188 Show Default KPI templates to customer end -----------------------------------
-----------------------------------ALD-4209 On Email subject fileds, 'Invoice #' field not showing Invoice number when try to send email ends-------------------------

---------------------------ALD-4231 Pre-Invoice send enhancement starts ---------------------------------
INSERT INTO ENT_Master_Type (Name,Active,Sort) Values ('Pre-invoice contact Mandatory in Quotation',1,30)
INSERT INTO ENT_Master_Config (Type,EntityId,Value,Active) Values (30,1,'true',1)
INSERT INTO ENT_Master_Config (Type,EntityId,Value,Active) Values (30,2,'true',1)
INSERT INTO ENT_Master_Config (Type,EntityId,Value,Active) Values (30,3,'true',1)

ALTER TABLE QU_Quotation_CustomerContact Add InvoiceEmail bit DEFAULT 0

ALTER TABLE QU_Quotation_SupplierContact Add InvoiceEmail bit DEFAULT 0

ALTER TABLE QU_Quotation_FactoryContact Add InvoiceEmail bit DEFAULT 0
---------------------------ALD-4231 Pre-Invoice send enhancement ends------------------------------------



INSERT INTO ENT_Master_Type (Name,Active,Sort) Values ('Audit Confirmed English Footer',1,31)
INSERT INTO ENT_Master_Type (Name,Active,Sort) Values ('Audit Confirmed Chinese Footer',1,32)


Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (31,1,NULL,'<div class="div-style">Special Note as below:</div><br /><div class="div-style">1. Pls note this is API official confirmation to your booking. pls ready production on time for inspection.API will not specify separately but will allocate appropriate number of Quality Inspectors on each service day according to product and total items in booking, pls contact API if you have any particular request on this.</div><div class="div-style">2. Pls double check all booking details in this email and advise us if any question upon this confirm email received.</div><div class="div-style">3. If NO any feedback or disagreement received from supplier/factory within 12:00 noon time China time 1 working day before inspection date mentioned as above, it will be treated all info in this email are confirmed and agreed, API will move ahead to assign inspector as schedule accordingly.<a href="{docpath}">More</a></div>',1)

Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (31,2,NULL,'<div class="div-style">Special Note as below:</div><br /><div class="div-style">1. Pls note this is SGT official confirmation to your booking. pls ready production on time for inspection.SGT will not specify separately but will allocate appropriate number of Quality Inspectors on each service day according to product and total items in booking, pls contact SGT if you have any particular request on this.</div><div class="div-style">2. Pls double check all booking details in this email and advise us if any question upon this confirm email received.</div><div class="div-style">3. If NO any feedback or disagreement received from supplier/factory within 12:00 noon time China time 1 working day before inspection date mentioned as above, it will be treated all info in this email are confirmed and agreed, SGT will move ahead to assign inspector as schedule accordingly.<a href="{docpath}">More</a></div>',1)

Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (31,1,38,N'<div class="div-style"> 温馨提示与声明： </div><br /><br /><div class="div-style">1. 本邮件是对贵司预定申请的确认邮件，工厂应按预定日期备货检验。API 将依据申请上的产品类别和款数等安排每天到厂的检验员人数、不做另行通知，如有特殊要求请及时联系 API 。</div> <div class="div-style">2. 请贵司仔细地复核本邮件中的相关内容。</div> <div class="div-style">3. 若截止上述验货日期前1个工作日的12时（中国北京时间）未收到贵司邮件回复与异议即代表贵司确认本邮件中的相关内容正确无误，API 将相应安排人员验货。 若需修改相关内容，请按以下要求及规定执行。<a href="{docpath}">更多信息</a></div>',1)

Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (31,2,38,N'<div class="div-style"> 温馨提示与声明： </div><br /><br /><div class="div-style">1. 本邮件是对贵司预定申请的确认邮件，工厂应按预定日期备货检验。SGT 将依据申请上的产品类别和款数等安排每天到厂的检验员人数、不做另行通知，如有特殊要求请及时联系 SGT 。</div> <div class="div-style">2. 请贵司仔细地复核本邮件中的相关内容。</div> <div class="div-style">3. 若截止上述验货日期前1个工作日的12时（中国北京时间）未收到贵司邮件回复与异议即代表贵司确认本邮件中的相关内容正确无误，SGT 将相应安排人员验货。 若需修改相关内容，请按以下要求及规定执行。<a href="{docpath}">更多信息</a></div>',1)

Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (32,1,NULL,'<div class="div-style"><b>Special Note: -</b></div><div class="div-style">a) Pls note this is the official confirmation to your booking.<div class="div-style"> - No need supplier/factory to confirm API again if all info in this email are correct but shall ready production on time for inspection; </div><div class="div-style"> - Pls inform API following the process below mentioned if any modification needed;</div></div><div class="div-style">b) Factory has to ensure production with 100% finished and at least 80% packed at 9:00am before API QC arrived at factory on service date agreed, unless customer has special agreement on this;</div><div class="div-style">c) Factory has to prepay API the inspection cost (inspection fee and traveling expense) on 2nd inspection for same order, because 1st inspection has to be aborted and missing inspection due to factory good not ready when QC arrived (or supplier late advise on cancel inspection while inspector is on the way to factory);</div> <div class="div-style">d) Factory Support needed on inspection equipment and inspection cooperating:<div class="div-style"> - Ensure all necessary inspection equipment working well and calibrated in valid before API inspector arrived; </div><div class="div-style"> - Ensure a special/separate area for API inspection with sufficient lighting and enough factory worker for cartons moving, unpacking and packing during inspection. </div></div><div class="div-style">e) Any wrong info given on booking will affect our quotation and inspection report, as well as surcharge applied due to incorrect info. If any modification needed pls inform API team by written email before 3:00pm local time by 3 days prior to agreed service date. Surcharge as below on modification done due to supplier/factory reason:<div class="div-style"> - If booking was revised more than 2 times (counting from booking receive date), starting from 3rd time we will charge USD$25/RMB$175 each time for revision.</div><div class="div-style"> - If any revision made less than 3 working days and that changes affect the quotation, we will charge USD$50/RMB$350 for revision. </div></div><div class="div-style">f) In case of postponement or cancellation on inspection, pls inform API local office by written email before 3:00pm local time with 24 working hours prior to the confirmed inspection date (excluding regular rest days and public holidays), surcharges of affecting resources deployment(Man-day lost and traveling expense if occurred) will be applied to booking applicant - supplier/factory if late advise on cancellation or postponement;</div><div class="div-style">g) Prepayment needed for Non-program customer, 2nd inspection due to goods not ready or re-inspection:<div class="div-style"> - “Bill to party” shall settle the full payment no later than 16:00 local time on the working day before service date, or else inspection will be on hold until invoice amount received.</div><div class="div-style"> - Please send us copy of the bank receipt with “API Inspection Number” & “Invoice Number” marked by email ASAP, API will arrange inspection only once payment received.</div></div><div class="div-style">Thanks for your cooperation!</div>',1)

Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (32,2,NULL,'<div class="div-style"><b>Special Note: -</b></div><div class="div-style">a) Pls note this is the official confirmation to your booking.<div class="div-style"> - No need supplier/factory to confirm SGT again if all info in this email are correct but shall ready production on time for inspection; </div><div class="div-style"> - Pls inform SGT following the process below mentioned if any modification needed;</div></div><div class="div-style">b) Factory has to ensure production with 100% finished and at least 80% packed at 9:00am before SGT QC arrived at factory on service date agreed, unless customer has special agreement on this;</div><div class="div-style">c) Factory has to prepay SGT the inspection cost (inspection fee and traveling expense) on 2nd inspection for same order, because 1st inspection has to be aborted and missing inspection due to factory good not ready when QC arrived (or supplier late advise on cancel inspection while inspector is on the way to factory);</div> <div class="div-style">d) Factory Support needed on inspection equipment and inspection cooperating:<div class="div-style"> - Ensure all necessary inspection equipment working well and calibrated in valid before SGT inspector arrived; </div><div class="div-style"> - Ensure a special/separate area for SGT inspection with sufficient lighting and enough factory worker for cartons moving, unpacking and packing during inspection. </div></div><div class="div-style">e) Any wrong info given on booking will affect our quotation and inspection report, as well as surcharge applied due to incorrect info. If any modification needed pls inform SGT team by written email before 3:00pm local time by 3 days prior to agreed service date. Surcharge as below on modification done due to supplier/factory reason:<div class="div-style"> - If booking was revised more than 2 times (counting from booking receive date), starting from 3rd time we will charge USD$25/RMB$175 each time for revision.</div><div class="div-style"> - If any revision made less than 3 working days and that changes affect the quotation, we will charge USD$50/RMB$350 for revision. </div></div><div class="div-style">f) In case of postponement or cancellation on inspection, pls inform SGT local office by written email before 3:00pm local time with 24 working hours prior to the confirmed inspection date (excluding regular rest days and public holidays), surcharges of affecting resources deployment(Man-day lost and traveling expense if occurred) will be applied to booking applicant - supplier/factory if late advise on cancellation or postponement;</div><div class="div-style">g) Prepayment needed for Non-program customer, 2nd inspection due to goods not ready or re-inspection:<div class="div-style"> - “Bill to party” shall settle the full payment no later than 16:00 local time on the working day before service date, or else inspection will be on hold until invoice amount received.</div><div class="div-style"> - Please send us copy of the bank receipt with “SGT Inspection Number” & “Invoice Number” marked by email ASAP, SGT will arrange inspection only once payment received.</div></div><div class="div-style">Thanks for your cooperation!</div>',1)

Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (32,2,38,N'<div class="div-style"><b> 温馨提示与声明：</b></div><div class="div-style">a)	此邮件是 API 对您验货申请的正式验货确认。</div><div class="div-style">-	如果此邮件内所有信息正确，则供应商/工厂无需再确认回复、但应按下述要求提前做好检验准备；</div><div class="div-style">-	如需任何更改请参考以下提示；</div><div class="div-style">b)	除非买家有特定的要求，生产工厂必须确保货物在上述检验日期当天上午9点前：100%成品，至少80%完成装箱;</div><div class="div-style">c)	如 API 检验员到厂(或已在途中接到通知取消验货将继续前往工厂)，货物未按要求备好当天验货放空出missing报告，再次验货费用(=检验费+交通费)全额由工厂预付；</div><div class="div-style">d)	检测设备与检验过程协助：</div><div class="div-style">-	在 API 检验员到达之前，工厂应确保所有必要的检测设备是在校准有效期内且能被正常操作使用。</div><div class="div-style">-	请工厂为 API 检验员提供一个光线充足且宽敞的检验场地，并安排足够的人员协助检验，包括搬运/装卸外箱，拆封包装等。</div><div class="div-style">e)	任何检验信息的缺失或错误（如检验日期、检验类型、工厂地址、订单号码和订单数量等）将影响原订人员安排以及验货报告，并有可能产生额外费用。若申请人在此确认邮件中发现任何信息与实际情况不符，为避免产生额外费用请在上述确定的检验日期前(间隔)3个工作日的下午3点前发邮件通知 API 当地办事处。因申请人原因修改验货申请产生的费用如下：</div><div class="div-style">-	若修改信息超过2次（自检验申请收到之日起计算），从第3次修改起我们将会收取每次25美元/175元人民币的手续费。</div><div class="div-style">-	任何不足3个工作日通知的信息修改，且修改将影响检验报价的，我们将会收取每次50美元/350元人民币的手续费。</div><div class="div-style">f)	若需取消或改期检验，务必需提前提前1个工作日=间隔1个工作日（周六日和法定假日除外）的下午3点前发邮件通知 API 。 如不足通知期(=过迟)取消或改期验货，验货申请人需承担由此产生的人力损失和相关费用。</div><div class="div-style">g)	非合约客户或重验/再次服务的预付款要求：</div><div class="div-style">-	付款方务必在确定的检验日期前一个工作日(当地时间16:00前)将检验费用付至 API，否则检验将被暂时搁置，直到付清检验款项。</div><div class="div-style">-	请尽快邮件发送银行付款凭据（标注“ API 检验号码”和“发票号码”）给 API 联络人。我司确认收到款项后，将为您安排检验。</div><div class="div-style">感谢您的支持与合作！</div>',1)

Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (32,2,NULL,'<div class="div-style"><b> 温馨提示与声明：</b></div><div class="div-style">a)	此邮件是 SGT 对您验货申请的正式验货确认。</div><div class="div-style">-	如果此邮件内所有信息正确，则供应商/工厂无需再确认回复、但应按下述要求提前做好检验准备；</div><div class="div-style">-	如需任何更改请参考以下提示；</div><div class="div-style">b)	除非买家有特定的要求，生产工厂必须确保货物在上述检验日期当天上午9点前：100%成品，至少80%完成装箱;</div><div class="div-style">c)	如 SGT 检验员到厂(或已在途中接到通知取消验货将继续前往工厂)，货物未按要求备好当天验货放空出missing报告，再次验货费用(=检验费+交通费)全额由工厂预付；</div><div class="div-style">d)	检测设备与检验过程协助：</div><div class="div-style">-	在 SGT 检验员到达之前，工厂应确保所有必要的检测设备是在校准有效期内且能被正常操作使用。</div><div class="div-style">-	请工厂为 SGT 检验员提供一个光线充足且宽敞的检验场地，并安排足够的人员协助检验，包括搬运/装卸外箱，拆封包装等。</div><div class="div-style">e)	任何检验信息的缺失或错误（如检验日期、检验类型、工厂地址、订单号码和订单数量等）将影响原订人员安排以及验货报告，并有可能产生额外费用。若申请人在此确认邮件中发现任何信息与实际情况不符，为避免产生额外费用请在上述确定的检验日期前(间隔)3个工作日的下午3点前发邮件通知 SGT 当地办事处。因申请人原因修改验货申请产生的费用如下：</div><div class="div-style">-	若修改信息超过2次（自检验申请收到之日起计算），从第3次修改起我们将会收取每次25美元/175元人民币的手续费。</div><div class="div-style">-	任何不足3个工作日通知的信息修改，且修改将影响检验报价的，我们将会收取每次50美元/350元人民币的手续费。</div><div class="div-style">f)	若需取消或改期检验，务必需提前提前1个工作日=间隔1个工作日（周六日和法定假日除外）的下午3点前发邮件通知 SGT 。 如不足通知期(=过迟)取消或改期验货，验货申请人需承担由此产生的人力损失和相关费用。</div><div class="div-style">g)	非合约客户或重验/再次服务的预付款要求：</div><div class="div-style">-	付款方务必在确定的检验日期前一个工作日(当地时间16:00前)将检验费用付至 SGT，否则检验将被暂时搁置，直到付清检验款项。</div><div class="div-style">-	请尽快邮件发送银行付款凭据（标注“ SGT 检验号码”和“发票号码”）给 SGT 联络人。我司确认收到款项后，将为您安排检验。</div><div class="div-style">感谢您的支持与合作！</div>',1)



INSERT INTO ENT_Master_Type (Name,Active,Sort) Values ('Audit Reschedule English Footer',1,33)
INSERT INTO ENT_Master_Type (Name,Active,Sort) Values ('Audit Reschedule Chinese Footer',1,34)



Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (33,1,NULL,'<div class="div-style">Special Note as below:</div><br /><div class="div-style">1. Pls note this is API official confirmation to your booking. pls ready production on time for inspection.API will not specify separately but will allocate appropriate number of Quality Inspectors on each service day according to product and total items in booking, pls contact API if you have any particular request on this.</div><div class="div-style">2. Pls double check all booking details in this email and advise us if any question upon this confirm email received.</div><div class="div-style">3. If NO any feedback or disagreement received from supplier/factory within 12:00 noon time China time 1 working day before inspection date mentioned as above, it will be treated all info in this email are confirmed and agreed, API will move ahead to assign inspector as schedule accordingly.<a href="{docpath}">More</a></div>',1)

Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (33,2,NULL,'<div class="div-style">Special Note as below:</div><br /><div class="div-style">1. Pls note this is SGT official confirmation to your booking. pls ready production on time for inspection.SGT will not specify separately but will allocate appropriate number of Quality Inspectors on each service day according to product and total items in booking, pls contact SGT if you have any particular request on this.</div><div class="div-style">2. Pls double check all booking details in this email and advise us if any question upon this confirm email received.</div><div class="div-style">3. If NO any feedback or disagreement received from supplier/factory within 12:00 noon time China time 1 working day before inspection date mentioned as above, it will be treated all info in this email are confirmed and agreed, SGT will move ahead to assign inspector as schedule accordingly.<a href="{docpath}">More</a></div>',1)

Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (33,1,38,N'<div class="div-style"> 温馨提示与声明： </div><br /><br /><div class="div-style">1. 本邮件是对贵司预定申请的确认邮件，工厂应按预定日期备货检验。API 将依据申请上的产品类别和款数等安排每天到厂的检验员人数、不做另行通知，如有特殊要求请及时联系 API 。</div> <div class="div-style">2. 请贵司仔细地复核本邮件中的相关内容。</div> <div class="div-style">3. 若截止上述验货日期前1个工作日的12时（中国北京时间）未收到贵司邮件回复与异议即代表贵司确认本邮件中的相关内容正确无误，API 将相应安排人员验货。 若需修改相关内容，请按以下要求及规定执行。<a href="{docpath}">更多信息</a></div>',1)

Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (33,2,38,N'<div class="div-style"> 温馨提示与声明： </div><br /><br /><div class="div-style">1. 本邮件是对贵司预定申请的确认邮件，工厂应按预定日期备货检验。SGT 将依据申请上的产品类别和款数等安排每天到厂的检验员人数、不做另行通知，如有特殊要求请及时联系 SGT 。</div> <div class="div-style">2. 请贵司仔细地复核本邮件中的相关内容。</div> <div class="div-style">3. 若截止上述验货日期前1个工作日的12时（中国北京时间）未收到贵司邮件回复与异议即代表贵司确认本邮件中的相关内容正确无误，SGT 将相应安排人员验货。 若需修改相关内容，请按以下要求及规定执行。<a href="{docpath}">更多信息</a></div>',1)

Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (34,1,NULL,'<div class="div-style"><b>Special Note: -</b></div><div class="div-style">a) Pls note this is the official confirmation to your booking.<div class="div-style"> - No need supplier/factory to confirm API again if all info in this email are correct but shall ready production on time for inspection; </div><div class="div-style"> - Pls inform API following the process below mentioned if any modification needed;</div></div><div class="div-style">b) Factory has to ensure production with 100% finished and at least 80% packed at 9:00am before API QC arrived at factory on service date agreed, unless customer has special agreement on this;</div><div class="div-style">c) Factory has to prepay API the inspection cost (inspection fee and traveling expense) on 2nd inspection for same order, because 1st inspection has to be aborted and missing inspection due to factory good not ready when QC arrived (or supplier late advise on cancel inspection while inspector is on the way to factory);</div> <div class="div-style">d) Factory Support needed on inspection equipment and inspection cooperating:<div class="div-style"> - Ensure all necessary inspection equipment working well and calibrated in valid before API inspector arrived; </div><div class="div-style"> - Ensure a special/separate area for API inspection with sufficient lighting and enough factory worker for cartons moving, unpacking and packing during inspection. </div></div><div class="div-style">e) Any wrong info given on booking will affect our quotation and inspection report, as well as surcharge applied due to incorrect info. If any modification needed pls inform API team by written email before 3:00pm local time by 3 days prior to agreed service date. Surcharge as below on modification done due to supplier/factory reason:<div class="div-style"> - If booking was revised more than 2 times (counting from booking receive date), starting from 3rd time we will charge USD$25/RMB$175 each time for revision.</div><div class="div-style"> - If any revision made less than 3 working days and that changes affect the quotation, we will charge USD$50/RMB$350 for revision. </div></div><div class="div-style">f) In case of postponement or cancellation on inspection, pls inform API local office by written email before 3:00pm local time with 24 working hours prior to the confirmed inspection date (excluding regular rest days and public holidays), surcharges of affecting resources deployment(Man-day lost and traveling expense if occurred) will be applied to booking applicant - supplier/factory if late advise on cancellation or postponement;</div><div class="div-style">g) Prepayment needed for Non-program customer, 2nd inspection due to goods not ready or re-inspection:<div class="div-style"> - “Bill to party” shall settle the full payment no later than 16:00 local time on the working day before service date, or else inspection will be on hold until invoice amount received.</div><div class="div-style"> - Please send us copy of the bank receipt with “API Inspection Number” & “Invoice Number” marked by email ASAP, API will arrange inspection only once payment received.</div></div><div class="div-style">Thanks for your cooperation!</div>',1)

Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (34,2,NULL,'<div class="div-style"><b>Special Note: -</b></div><div class="div-style">a) Pls note this is the official confirmation to your booking.<div class="div-style"> - No need supplier/factory to confirm SGT again if all info in this email are correct but shall ready production on time for inspection; </div><div class="div-style"> - Pls inform SGT following the process below mentioned if any modification needed;</div></div><div class="div-style">b) Factory has to ensure production with 100% finished and at least 80% packed at 9:00am before SGT QC arrived at factory on service date agreed, unless customer has special agreement on this;</div><div class="div-style">c) Factory has to prepay SGT the inspection cost (inspection fee and traveling expense) on 2nd inspection for same order, because 1st inspection has to be aborted and missing inspection due to factory good not ready when QC arrived (or supplier late advise on cancel inspection while inspector is on the way to factory);</div> <div class="div-style">d) Factory Support needed on inspection equipment and inspection cooperating:<div class="div-style"> - Ensure all necessary inspection equipment working well and calibrated in valid before SGT inspector arrived; </div><div class="div-style"> - Ensure a special/separate area for SGT inspection with sufficient lighting and enough factory worker for cartons moving, unpacking and packing during inspection. </div></div><div class="div-style">e) Any wrong info given on booking will affect our quotation and inspection report, as well as surcharge applied due to incorrect info. If any modification needed pls inform SGT team by written email before 3:00pm local time by 3 days prior to agreed service date. Surcharge as below on modification done due to supplier/factory reason:<div class="div-style"> - If booking was revised more than 2 times (counting from booking receive date), starting from 3rd time we will charge USD$25/RMB$175 each time for revision.</div><div class="div-style"> - If any revision made less than 3 working days and that changes affect the quotation, we will charge USD$50/RMB$350 for revision. </div></div><div class="div-style">f) In case of postponement or cancellation on inspection, pls inform SGT local office by written email before 3:00pm local time with 24 working hours prior to the confirmed inspection date (excluding regular rest days and public holidays), surcharges of affecting resources deployment(Man-day lost and traveling expense if occurred) will be applied to booking applicant - supplier/factory if late advise on cancellation or postponement;</div><div class="div-style">g) Prepayment needed for Non-program customer, 2nd inspection due to goods not ready or re-inspection:<div class="div-style"> - “Bill to party” shall settle the full payment no later than 16:00 local time on the working day before service date, or else inspection will be on hold until invoice amount received.</div><div class="div-style"> - Please send us copy of the bank receipt with “SGT Inspection Number” & “Invoice Number” marked by email ASAP, SGT will arrange inspection only once payment received.</div></div><div class="div-style">Thanks for your cooperation!</div>',1)

Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (34,2,38,N'<div class="div-style"><b> 温馨提示与声明：</b></div><div class="div-style">a)	此邮件是 API 对您验货申请的正式验货确认。</div><div class="div-style">-	如果此邮件内所有信息正确，则供应商/工厂无需再确认回复、但应按下述要求提前做好检验准备；</div><div class="div-style">-	如需任何更改请参考以下提示；</div><div class="div-style">b)	除非买家有特定的要求，生产工厂必须确保货物在上述检验日期当天上午9点前：100%成品，至少80%完成装箱;</div><div class="div-style">c)	如 API 检验员到厂(或已在途中接到通知取消验货将继续前往工厂)，货物未按要求备好当天验货放空出missing报告，再次验货费用(=检验费+交通费)全额由工厂预付；</div><div class="div-style">d)	检测设备与检验过程协助：</div><div class="div-style">-	在 API 检验员到达之前，工厂应确保所有必要的检测设备是在校准有效期内且能被正常操作使用。</div><div class="div-style">-	请工厂为 API 检验员提供一个光线充足且宽敞的检验场地，并安排足够的人员协助检验，包括搬运/装卸外箱，拆封包装等。</div><div class="div-style">e)	任何检验信息的缺失或错误（如检验日期、检验类型、工厂地址、订单号码和订单数量等）将影响原订人员安排以及验货报告，并有可能产生额外费用。若申请人在此确认邮件中发现任何信息与实际情况不符，为避免产生额外费用请在上述确定的检验日期前(间隔)3个工作日的下午3点前发邮件通知 API 当地办事处。因申请人原因修改验货申请产生的费用如下：</div><div class="div-style">-	若修改信息超过2次（自检验申请收到之日起计算），从第3次修改起我们将会收取每次25美元/175元人民币的手续费。</div><div class="div-style">-	任何不足3个工作日通知的信息修改，且修改将影响检验报价的，我们将会收取每次50美元/350元人民币的手续费。</div><div class="div-style">f)	若需取消或改期检验，务必需提前提前1个工作日=间隔1个工作日（周六日和法定假日除外）的下午3点前发邮件通知 API 。 如不足通知期(=过迟)取消或改期验货，验货申请人需承担由此产生的人力损失和相关费用。</div><div class="div-style">g)	非合约客户或重验/再次服务的预付款要求：</div><div class="div-style">-	付款方务必在确定的检验日期前一个工作日(当地时间16:00前)将检验费用付至 API，否则检验将被暂时搁置，直到付清检验款项。</div><div class="div-style">-	请尽快邮件发送银行付款凭据（标注“ API 检验号码”和“发票号码”）给 API 联络人。我司确认收到款项后，将为您安排检验。</div><div class="div-style">感谢您的支持与合作！</div>',1)

Insert into ENT_Master_Config(Type,EntityId,CountryId,Value,Active) Values (34,2,NULL,'<div class="div-style"><b> 温馨提示与声明：</b></div><div class="div-style">a)	此邮件是 SGT 对您验货申请的正式验货确认。</div><div class="div-style">-	如果此邮件内所有信息正确，则供应商/工厂无需再确认回复、但应按下述要求提前做好检验准备；</div><div class="div-style">-	如需任何更改请参考以下提示；</div><div class="div-style">b)	除非买家有特定的要求，生产工厂必须确保货物在上述检验日期当天上午9点前：100%成品，至少80%完成装箱;</div><div class="div-style">c)	如 SGT 检验员到厂(或已在途中接到通知取消验货将继续前往工厂)，货物未按要求备好当天验货放空出missing报告，再次验货费用(=检验费+交通费)全额由工厂预付；</div><div class="div-style">d)	检测设备与检验过程协助：</div><div class="div-style">-	在 SGT 检验员到达之前，工厂应确保所有必要的检测设备是在校准有效期内且能被正常操作使用。</div><div class="div-style">-	请工厂为 SGT 检验员提供一个光线充足且宽敞的检验场地，并安排足够的人员协助检验，包括搬运/装卸外箱，拆封包装等。</div><div class="div-style">e)	任何检验信息的缺失或错误（如检验日期、检验类型、工厂地址、订单号码和订单数量等）将影响原订人员安排以及验货报告，并有可能产生额外费用。若申请人在此确认邮件中发现任何信息与实际情况不符，为避免产生额外费用请在上述确定的检验日期前(间隔)3个工作日的下午3点前发邮件通知 SGT 当地办事处。因申请人原因修改验货申请产生的费用如下：</div><div class="div-style">-	若修改信息超过2次（自检验申请收到之日起计算），从第3次修改起我们将会收取每次25美元/175元人民币的手续费。</div><div class="div-style">-	任何不足3个工作日通知的信息修改，且修改将影响检验报价的，我们将会收取每次50美元/350元人民币的手续费。</div><div class="div-style">f)	若需取消或改期检验，务必需提前提前1个工作日=间隔1个工作日（周六日和法定假日除外）的下午3点前发邮件通知 SGT 。 如不足通知期(=过迟)取消或改期验货，验货申请人需承担由此产生的人力损失和相关费用。</div><div class="div-style">g)	非合约客户或重验/再次服务的预付款要求：</div><div class="div-style">-	付款方务必在确定的检验日期前一个工作日(当地时间16:00前)将检验费用付至 SGT，否则检验将被暂时搁置，直到付清检验款项。</div><div class="div-style">-	请尽快邮件发送银行付款凭据（标注“ SGT 检验号码”和“发票号码”）给 SGT 联络人。我司确认收到款项后，将为您安排检验。</div><div class="div-style">感谢您的支持与合作！</div>',1)



Alter table AUD_TRAN_Cancel_Reschedule add [CreatedOn] DATETIME NULL default GETDATE()

Alter table AUD_TRAN_Cancel_Reschedule add  [CreatedBy] INT NULL

Alter table AUD_TRAN_Cancel_Reschedule add  FOREIGN KEY(CreatedBy) REFERENCES [IT_UserMaster](Id)


------------------------------------------ALD-4302 starts---------------------------------------------------
Update ES_SU_PreDefined_Fields Set IsText=0,DataType=1 Where Field_Name='CustomerDecisionResult'
------------------------------------------ALD-4302 ends---------------------------------------------------

--- ALD-4190 start ---

ALTER TABLE AUD_Transaction ADD IsEAQF BIT

--- ALD-4190 end ---



--------------------------------ALD-3806 CS Allocation enhancement starts-----------------------------------------------------------------------
CREATE TABLE [dbo].[DA_UserByFactoryCountry](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[DaUserCustomerId] [int] NOT NULL,
	[FactoryCountryId] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[EntityId] [int] NOT NULL,
	CONSTRAINT [FK_DA_UserByFactoryCountry_AP_Entity_EntityId] FOREIGN KEY([EntityId]) REFERENCES [dbo].[AP_Entity] ([Id]),
	CONSTRAINT [FK_DA_UserByFactoryCountry_DA_UserCustomer_DaUserCustomerId] FOREIGN KEY([DaUserCustomerId]) REFERENCES [dbo].[DA_UserCustomer] ([Id]),
	CONSTRAINT [FK_DA_UserByFactoryCountry_IT_UserMaster_CreatedBy] FOREIGN KEY([CreatedBy]) REFERENCES [dbo].[IT_UserMaster] ([Id]),
	CONSTRAINT [FK_DA_UserByFactoryCountry_REF_Country_FactoryCountryId] FOREIGN KEY([FactoryCountryId]) REFERENCES [dbo].[REF_Country] ([Id])
)
--------------------------------ALD-3806 CS Allocation enhancement ends-----------------------------------------------------------------------
ALTER TABLE INV_MAN_Transaction ADD AuditId int null
ALTER TABLE INV_MAN_Transaction 
ADD CONSTRAINT FK_INV_MAN_Transaction_AuditId FOREIGN KEY(AuditId) REFERENCES AUD_Transaction(Id)

--- ALD-4190 end ---


---------------------------------------- ALD-4449 starts ------------------------------
Insert Into REF_KPI_Teamplate Values ('Inspection Summary-Expense',1,1,1,NULL)
---------------------------------------- ALD-4449 ends------------------------------