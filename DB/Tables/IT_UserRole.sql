CREATE TABLE [dbo].[IT_UserRole]
(
	UserId INT NOT NULL,
	RoleId INT NOT NULL, 
	EntityId INT not null,
	PRIMARY KEY(UserId, RoleId,EntityId),
	FOREIGN KEY(UserId) REFERENCES IT_UserMaster(Id),
	FOREIGN KEY(RoleId) REFERENCES IT_Role(Id),
	CONSTRAINT FK_IT_UserRole_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id)
)
