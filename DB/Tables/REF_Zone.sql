CREATE TABLE [dbo].[REF_Zone] (
    [Id]     INT            IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR (500) NULL,
    [Active] BIT            NULL,
    CONSTRAINT [PK_REF_Zone] PRIMARY KEY CLUSTERED ([Id] ASC)
);

