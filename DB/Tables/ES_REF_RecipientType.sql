create table ES_REF_RecipientType 
(
Id int primary key identity(1,1),
Name nvarchar(300),
Active bit,
Created_By int null,
Created_On DATETIME NOT NULL DEFAULT GETDATE()
CONSTRAINT ES_REF_RecipientType_Created_By FOREIGN KEY (Created_By) REFERENCES [dbo].[IT_UserMaster](Id)
)