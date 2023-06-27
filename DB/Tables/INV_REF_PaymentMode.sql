CREATE TABLE INV_REF_PaymentMode(
Id int not null primary key identity(1,1),
Name nvarchar(200),
Active bit NOT NULL,
Sort int NOT NULL
);