CREATE TABLE HR_Renew
(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[staff_id] [int] NOT NULL,
	[Number] INT NOT NULL,
	StartDate DATETIME NOT NULL,
	EndDate DATETIME NOT NULL,
	[EntityId] INT NULL, 
	FOREIGN KEY([EntityId]) REFERENCES [AP_Entity](Id),
	FOREIGN KEY([staff_id]) REFERENCES [dbo].[HR_Staff](Id)
)