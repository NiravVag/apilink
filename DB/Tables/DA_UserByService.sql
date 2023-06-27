CREATE TABLE [dbo].[DA_UserByService]
(
[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),	
[DAUserCustomerId] INT NOT NULL,
[EntityId] INT NULL,
[ServiceId] INT NULL,
[CreatedBy] INT NULL,
[CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(),
CONSTRAINT FK_DAUserByService_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_DAUserByService_ServiceId FOREIGN KEY ([ServiceId]) REFERENCES [dbo].[REF_Service](Id),
CONSTRAINT FK_DAUserByService_DAUserCustomerId FOREIGN KEY ([DAUserCustomerId]) REFERENCES [dbo].[DA_UserCustomer](Id),
CONSTRAINT FK_DAUserByService_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)