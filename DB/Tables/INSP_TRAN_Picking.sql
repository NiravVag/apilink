CREATE TABLE [dbo].[INSP_TRAN_Picking]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Lab_Id] INT NULL,  
    [Customer_Id] INT NULL, 
    [Lab_Address_Id] INT NULL, 
    [Cus_Address_Id] INT NULL, 
    [PO_Tran_Id] INT NOT NULL, 
    [Picking_Qty] INT NOT NULL, 
    [Remarks] NVARCHAR(2000) NULL, 
    [Active] BIT NOT NULL, 
    [CreatedBy] INT NULL, 
    [CreationDate] DATETIME NULL, 
    [UpdatedBy] INT NULL, 
    [UpdationDate] DATETIME NULL, 
    [DeletedBy] INT NULL, 
    [DeletionDate] DATETIME NULL, 
    FOREIGN KEY(Customer_Id) REFERENCES [CU_Customer](Id),
	FOREIGN KEY(PO_Tran_Id) REFERENCES [INSP_PurchaseOrder_Transaction](Id),
	FOREIGN KEY(CreatedBy) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY(UpdatedBy) REFERENCES [IT_UserMaster](Id),
	FOREIGN KEY(DeletedBy) REFERENCES [IT_UserMaster](Id),
	CONSTRAINT FK_Lab_Id FOREIGN KEY([Lab_Id]) REFERENCES INSP_LAB_Details(Id)
)
