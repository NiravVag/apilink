﻿CREATE TABLE HR_HolidayDayType (
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Label NVARCHAR(100) NOT NULL,
	[TypeTransId] INT NULL
)