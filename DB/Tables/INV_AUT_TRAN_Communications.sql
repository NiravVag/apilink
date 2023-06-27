create table INV_AUT_TRAN_Communications
(Id int not null identity(1,1) primary key,
Invoice_Number nvarchar(1000),
Comment nvarchar(2000),
CreatedBy int,
CreatedOn datetime null default getdate(),
Active bit,
EntityId int
CONSTRAINT FK_INV_AUT_TRAN_Communications_Created_By FOREIGN KEY(CreatedBy) REFERENCES [dbo].[it_usermaster](Id),
CONSTRAINT FK_INV_AUT_TRAN_Communications_Entity_Id FOREIGN KEY(EntityId) REFERENCES [dbo].[ap_entity](Id)
)