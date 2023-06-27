CREATE TABLE [dbo].[HR_StaffTraining](
	[Id]  INT  IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Staff_Id] [int] NULL,
	[Training_Topic] [nvarchar](200) NULL,
	[DateStart] [datetime] NULL,
	[DateEnd] [datetime] NULL,
	[Trainer] [nvarchar](200) NULL,
	[Comment] [nvarchar](500) NULL,
	FOREIGN KEY([Staff_Id]) REFERENCES [dbo].[HR_Staff](Id))
