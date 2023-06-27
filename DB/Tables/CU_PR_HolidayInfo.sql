CREATE TABLE [dbo].[CU_PR_HolidayInfo]
(
[Id] int identity(1,1) primary key,
[Name] NVARCHAR(50),
[Active] BIT,
[EntityId] INT NULL,
CONSTRAINT CU_PR_HolidayInfo_EntityId  FOREIGN KEY(EntityId) REFERENCES [dbo].[AP_Entity](Id),
)