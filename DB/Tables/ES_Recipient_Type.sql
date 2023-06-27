create table ES_Recipient_Type 
(
Id int primary key identity(1,1),
Es_Details_Id int null,
Recipient_Type_Id int null,
IsTo bit,
IsCC bit,
Active bit,
Created_By int null,
Created_On DATETIME NOT NULL DEFAULT GETDATE()
CONSTRAINT ES_Recipient_Type_Created_By FOREIGN KEY (Created_By) REFERENCES [dbo].[IT_UserMaster](Id),
CONSTRAINT ES_Recipient_Type_Es_Details_Id FOREIGN KEY (Es_Details_Id) REFERENCES [dbo].[ES_Details](Id),
CONSTRAINT ES_Recipient_Type_Recipient_Type_Id FOREIGN KEY (Recipient_Type_Id) REFERENCES [dbo].[ES_REF_RecipientType](Id)
)