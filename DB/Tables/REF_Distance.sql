CREATE TABLE [dbo].[REF_Distance] (
    [Id]     INT            IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR (100) NULL,
    [Active] BIT            NULL,
    CONSTRAINT [PK_REF_Distance] PRIMARY KEY CLUSTERED ([Id] ASC)
);

