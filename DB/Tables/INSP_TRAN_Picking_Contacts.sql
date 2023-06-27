CREATE TABLE [dbo].[INSP_TRAN_Picking_Contacts]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Picking_Tran_Id] INT NOT NULL, 
    [Lab_Contact_Id] INT NULL, 
    [Cus_Contact_Id] INT NULL, 
    [Active] BIT NOT NULL,
	[CreatedBy] INT NULL, 
    [CreatedOn] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletedOn] DATETIME NULL, 
    FOREIGN KEY(Cus_Contact_Id) REFERENCES [CU_Contact](Id),
	FOREIGN KEY(Picking_Tran_Id) REFERENCES [INSP_TRAN_Picking](Id),
	FOREIGN KEY(CreatedBy) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY(DeletedBy) REFERENCES [IT_UserMaster](Id)
)
