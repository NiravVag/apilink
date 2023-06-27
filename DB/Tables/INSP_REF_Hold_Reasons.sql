CREATE TABLE [dbo].[INSP_REF_Hold_Reasons]
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Reason NVARCHAR(500),
	Active BIT,
	Sort INT,
	EntityId INT
)