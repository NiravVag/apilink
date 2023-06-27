
/*
--------------------------------------------------------------------------------------
   [SU_Level]		
--------------------------------------------------------------------------------------
*/

INSERT INTO  [dbo].[SU_Level]([Level])
VALUES ('A'), ('B'), ('C'),('D')
GO 

/*
--------------------------------------------------------------------------------------
[SU_Type]
--------------------------------------------------------------------------------------
*/

DECLARE  @IdTran INT

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Usine', NULL,12)
SELECT @IdTran = SCOPE_IDENTITY()
INSERT INTO [dbo].[SU_Type] ([Type], TypeTransId) VALUES('Factory', @IdTran)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N' Préstataire - agent', NULL,12)
SELECT @IdTran = SCOPE_IDENTITY()
INSERT INTO [dbo].[SU_Type] ([Type], TypeTransId) VALUES('Supplier - Agent', @IdTran)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Préstataire - Traideur', NULL,12)
SELECT @IdTran = SCOPE_IDENTITY()
INSERT INTO [dbo].[SU_Type] ([Type], TypeTransId) VALUES('Supplier - Trading', @IdTran)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N' Préstataire - Import / Export', NULL,12)
SELECT @IdTran = SCOPE_IDENTITY()
INSERT INTO [dbo].[SU_Type] ([Type], TypeTransId) VALUES('Supplier - Import / Export', @IdTran)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Préstataire / Vente Local', NULL,12)
SELECT @IdTran = SCOPE_IDENTITY()
INSERT INTO [dbo].[SU_Type] ([Type], TypeTransId) VALUES('Supplier - Buying House', @IdTran)

GO

/*
--------------------------------------------------------------------------------------
[SU_OwnlerShip]
--------------------------------------------------------------------------------------
*/

DECLARE  @IdTran INT

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Rien', NULL,13)
SELECT @IdTran = SCOPE_IDENTITY()
INSERT INTO [dbo].[SU_OwnlerShip]([Name], Name_TranId) Values ('None', @IdTran)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Majeur Partiel', NULL,13)
SELECT @IdTran = SCOPE_IDENTITY()
INSERT INTO [dbo].[SU_OwnlerShip]([Name], Name_TranId) Values ('Partial Major', @IdTran)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Mineur partiel', NULL,13)
SELECT @IdTran = SCOPE_IDENTITY()
INSERT INTO [dbo].[SU_OwnlerShip]([Name], Name_TranId) Values ('Partial Minor', @IdTran)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Tout', NULL,13)
SELECT @IdTran = SCOPE_IDENTITY()
INSERT INTO [dbo].[SU_OwnlerShip]([Name], Name_TranId) Values ('Full', @IdTran)
GO


/*
--------------------------------------------------------------------------------------
[SU_AddressType]
--------------------------------------------------------------------------------------
*/


DECLARE  @IdTran INT

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Tête Office', NULL,14)
SELECT @IdTran = SCOPE_IDENTITY()
INSERT INTO [dbo].[SU_AddressType]([Address_Type], [Address_Type_Flag],[TranslationId])
VALUES('Head Office', 'H', @IdTran)

INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Facturation', NULL,14)
SELECT @IdTran = SCOPE_IDENTITY()
INSERT INTO [dbo].[SU_AddressType]([Address_Type], [Address_Type_Flag],[TranslationId])
VALUES('Accounting', 'B', @IdTran)


INSERT [dbo].[REF_Translation] ([Text_FR], [Text_CH], [TranslationGroupId]) VALUES (N'Plateforme', NULL,14)
SELECT @IdTran = SCOPE_IDENTITY()
INSERT INTO [dbo].[SU_AddressType]([Address_Type], [Address_Type_Flag],[TranslationId])
VALUES('Regional Office', 'R', @IdTran)
GO