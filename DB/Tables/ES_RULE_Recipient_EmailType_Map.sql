CREATE TABLE [dbo].[ES_RULE_Recipient_EmailType_Map]
(
	Id int PRIMARY KEY IDENTITY (1,1),
	TypeId int,
	RecipientTypeId int,
	Active bit,
	CONSTRAINT FK_ES_RULE_Recipient_EmailType_Map_TypeId FOREIGN KEY (TypeId) REFERENCES ES_Type(Id),
	CONSTRAINT FK_ES_RULE_Recipient_EmailType_Map_RecipientTypeId FOREIGN KEY (RecipientTypeId) REFERENCES ES_REF_RecipientType(Id)

)
