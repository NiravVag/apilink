CREATE TABLE [dbo].[HR_Holiday](
	[Id] [int] IDENTITY(1,1)  NOT NULL PRIMARY KEY,
	[Country_Id] [int] NOT NULL,
	[Holiday_Name] [NVARCHAR](200) NOT NULL,
	[location_id] [int] NULL,
	[recurrence_type] INT NOT NULL DEFAULT(0),  -- 0 : manually - 1 : EVRY YEAR, 2: EVRYMONTH, 3 EVRY WEEK 
	[start_date] DATETIME NULL,
	[end_date] DATETIME NULL, 
	[EntityId] INT NULL, 
	[HolidayId] INT NULL,
	[StartDateType] INT NULL,
	[EndDateType] INT NULL,
	FOREIGN KEY([Country_Id]) REFERENCES [dbo].[REF_Country]([Id]),
	FOREIGN KEY([location_id]) REFERENCES [dbo].[REF_Location]([Id]),
	FOREIGN KEY([EntityId]) REFERENCES [dbo].[AP_Entity](Id),
	FOREIGN KEY([StartDateType]) REFERENCES [dbo].[HR_HolidayDayType](Id),
	FOREIGN KEY([EndDateType]) REFERENCES [dbo].[HR_HolidayDayType](Id)
	)