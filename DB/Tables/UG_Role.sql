CREATE TABLE [dbo].[UG_Role](
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[RoleId] [int] NULL,
	[UserGuideId] [int] NULL,
	[Active] BIT NULL
	CONSTRAINT FK_UG_USER_GUIDE FOREIGN KEY(UserGuideId) REFERENCES [dbo].[UG_UserGuide_Details](Id),
	CONSTRAINT FK_UG_ROLE_ID FOREIGN KEY(RoleId) REFERENCES [dbo].[IT_Role](Id)
)