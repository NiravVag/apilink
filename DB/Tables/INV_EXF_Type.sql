create table  [dbo].[INV_EXF_Type] ([Id] int not null primary key identity(1,1),
[Name] nvarchar(max)
,[Active] bit,
[Sort] Bit, 
[CreatedOn] datetime not null default getdate()
)

