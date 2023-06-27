CREATE TABLE [dbo].[IT_Role_Right]
(
	[RoleId] INT NOT NULL,
	[RightId] INT NOT NULL,
	PRIMARY KEY([RoleId],[RightId]),
	FOREIGN KEY([RoleId]) REFERENCES IT_Role(Id),
	FOREIGN KEY([RightId]) REFERENCES IT_Right(Id)
)
