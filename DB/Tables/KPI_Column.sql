﻿	CREATE TABLE [KPI_Column] (
		[Id] INT IDENTITY(1,1) PRIMARY KEY, 
		[FieldLabel] NVARCHAR(300) NOT NULL,
		[FieldName] NVARCHAR(300) NOT NULL, 
		[FieldType] VARCHAR(20) NULL,
		[IdSubModule] INT NULL,
		[IdModule] INT NULL,
		[CanFilter] BIT NOT NULL,
		[CanShowInResult] BIT NOT NULL,
		[FilterIsMultiple] BIT NULL, 
		[FilterDataSourceName] VARCHAR(200) NULL,
		[FilterDataSourceTypeId] INT NULL, 
		[FilterDataSourceFieldValue] VARCHAR(100) NULL,
		[FilterDataSourceFieldName] VARCHAR(100) NULL,
		[FilterDataSourceFieldCondition] VARCHAR(100) NULL,
		[FilterDataSourceFieldConditionValue] VARCHAR(100) NULL,
		[FilterRequired] BIT NOT NULL, 
		[FilterSignEqualityId]  INT NULL,
		[Active] BIT NOT NULL,
		[IsLocationId] BIT NOT NULL, 
		[IsCustomerId] BIT NOT NULL,
		[IsKey] BIT NOT NULL,
		FOREIGN  KEY ([IdSubModule]) REFERENCES [AP_SubModule](Id),
		FOREIGN  KEY ([IdModule]) REFERENCES [AP_Module](Id),
		FOREIGN KEY ([FilterDataSourceTypeId]) REFERENCES [REF_DataSourceType](Id),
		FOREIGN KEY ([FilterSignEqualityId]) REFERENCES [REF_SignEquality](Id)
	)
