CREATE TABLE [dbo].[INSP_REF_InspectionLocation]
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name NVARCHAR(500),
	Active BIT,
	Sort INT
)