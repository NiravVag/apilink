create table INV_TM_Type(
Id int not null primary key identity(1,1),
Name nvarchar(200),
Active bit, 
CreatedOn datetime default getdate()
)