CREATE TABLE HR_StaffPhoto
(
	[GuidId] [uniqueidentifier] ROWGUIDCOL NOT NULL PRIMARY KEY, 
	[Photo] VARBINARY(MAX) FILESTREAM NULL,
	[StaffId] INT NOT NULL,
	[Photo_mType]  VARCHAR(100) NULL,
	FOREIGN KEY ([StaffId]) REFERENCES [dbo].[HR_Staff](id) 
)