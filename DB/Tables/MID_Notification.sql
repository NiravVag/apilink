CREATE TABLE MID_Notification (
 Id UNIQUEIDENTIFIER  NOT NULL PRIMARY KEY, 
 NotifTypeId INT NOT NULL,
 UserId INT NOT NULL, 
 LinkId INT NOT NULL,
 IsRead BIT NOT NULL DEFAULT(0),
 [CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
 [UpdatedOn] DATETIME NULL,
 [EntityId] INT NULL,
 [MessageId] int,
 [NotificationMessage] nvarchar(1000)
 CONSTRAINT FK_MID_Notification_EntityId FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id),
 CONSTRAINT FK_MID_Notification_MessageId FOREIGN KEY (MessageId) REFERENCES MID_Notification_Message(Id),
 FOREIGN KEY (NotifTypeId)  REFERENCES  [dbo].[MID_NotificationType](Id),
 FOREIGN KEY (UserId)  REFERENCES  [dbo].[IT_UserMaster](Id) 
)