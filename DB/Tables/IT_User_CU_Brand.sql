CREATE TABLE [dbo].[IT_User_CU_Brand]
(
	[User_Id] INT NOT NULL,
[Brand_Id] INT NOT NULL,
PRIMARY KEY([User_Id] ,[Brand_Id]),
FOREIGN KEY([User_Id]) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY([Brand_Id]) REFERENCES [CU_Brand](Id)
)
