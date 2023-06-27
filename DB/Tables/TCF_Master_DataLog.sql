CREATE TABLE [dbo].TCF_Master_DataLog
(
 ID INT IDENTITY(1,1) PRIMARY KEY,
 AccountId INT,
 DataType INT,
 RequestUrl NVARCHAR(500),
 LogInformation NVARCHAR(MAX),
 ResponseMessage NVARCHAR(MAX),
 CreatedOn DATETIME,
 CreatedBy INT
)