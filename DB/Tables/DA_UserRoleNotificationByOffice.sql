CREATE TABLE [dbo].[DA_UserRoleNotificationByOffice]
(
[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),	
[DAUserCustomerId] INT NOT NULL,
[EntityId] INT NULL,
[OfficeId]  INT NULL,
[CreatedBy] INT NULL,
[CreatedOn] DATETIME NOT NULL DEFAULT GETDATE()
CONSTRAINT FK_DAUserRoleNotificationByOffice_CreatedBy FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[IT_UserMaster](Id),	
CONSTRAINT FK_DAUserRoleNotificationByOffice_OfficeId FOREIGN KEY ([OfficeId]) REFERENCES [dbo].[REF_Location](Id),
CONSTRAINT FK_DAUserRoleNotificationByOffice_DAUserCustomerId FOREIGN KEY ([DAUserCustomerId]) REFERENCES [dbo].[DA_UserCustomer](Id),
CONSTRAINT FK_DAUserRoleNotificationByOffice_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)