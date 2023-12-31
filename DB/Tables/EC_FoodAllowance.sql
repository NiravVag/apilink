﻿CREATE TABLE [dbo].[EC_FoodAllowance] (
 Id iNT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
 CountryId INT NOT NULL,
 StartDate DATETIME NOT NULL, 
 EndDate DATETIME NOT NULL,
 FoodAllowance DECIMAL(18,2) NOT NULL,
 CurrencyId INT NOT NULL,
 UserId INT NOT NULL,
 [EntityId] INT NULL,
[Active] BIT NULL,  
[CreatedOn] DATETIME NULL, 
[UpdatedBy] INT NULL, 
[UpdatedOn] DATETIME NULL, 
[DeletedBy] INT NULL, 
[DeletedOn] DATETIME NULL,
FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
FOREIGN KEY ([DeletedBy]) REFERENCES [dbo].[IT_UserMaster](Id),
 FOREIGN KEY(CountryId) REFERENCES [dbo].[REF_Country](Id),
 FOREIGN KEY(CurrencyId) REFERENCES [dbo].[REF_Currency](Id),
 FOREIGN KEY(UserId) REFERENCES [dbo].[IT_UserMaster](Id),
 FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id)
)