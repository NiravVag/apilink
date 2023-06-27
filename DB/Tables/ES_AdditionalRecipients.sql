Create table ES_AdditionalRecipients(
Id INT  NOT NULL PRIMARY KEY IDENTITY(1,1),
EmailDetailId INT,
AdditionalEmail varchar(255),
Recipient int,
CreatedOn datetime not null,
CreatedBy int,
CONSTRAINT FK_ES_AdditionalRecipients_EmailDetailId FOREIGN KEY (EmailDetailId) REFERENCES ES_Details(Id),
CONSTRAINT FK_ES_AdditionalRecipients_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES IT_UserMaster(Id),
CONSTRAINT FK_ES_AdditionalRecipients_Recipient FOREIGN KEY (Recipient) REFERENCES ES_REF_Recipient(Id)
)