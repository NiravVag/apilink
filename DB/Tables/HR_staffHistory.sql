CREATE TABLE [dbo].[HR_staffHistory](
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Staff_Id] [int] NULL,
	[Company] [nvarchar](100) NULL,
	[Sgt_Location_ID] [int] NULL,
	[Salary] [float] NULL,
	[Currency_Id] [int] NULL,
	[Position] [nvarchar](100) NULL,
	[Datebegin] [datetime] NULL,
	[DateEnd] [datetime] NULL,
	[Comments] [nvarchar](100) NULL,
	FOREIGN KEY([Staff_Id]) REFERENCES [dbo].[HR_Staff](Id),
	FOREIGN KEY([Currency_Id]) REFERENCES [dbo].[REF_Currency](Id))