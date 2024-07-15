CREATE TABLE [dbo].[ModuleMetadata] (
    [Name]  NVARCHAR (450) NOT NULL,
    [Value] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ModuleMetadata] PRIMARY KEY CLUSTERED ([Name] ASC)
);

