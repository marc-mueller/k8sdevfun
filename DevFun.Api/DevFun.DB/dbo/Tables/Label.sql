CREATE TABLE [dbo].[Label] (
    [ContentPath]  NVARCHAR (450) NOT NULL,
    [Content]      NVARCHAR (MAX) NOT NULL,
    [LanguageName] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Label] PRIMARY KEY CLUSTERED ([ContentPath] ASC)
);



