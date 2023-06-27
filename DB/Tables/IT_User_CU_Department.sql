CREATE TABLE [dbo].[IT_User_CU_Department]
(
	[User_Id] INT NOT NULL,
[Department_Id] INT NOT NULL,
PRIMARY KEY([User_Id] ,[Department_Id]),
FOREIGN KEY([User_Id]) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY([Department_Id]) REFERENCES [CU_Department](Id)
)
