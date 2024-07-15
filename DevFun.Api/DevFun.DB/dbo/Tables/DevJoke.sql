CREATE TABLE [dbo].[DevJoke] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Text]       NVARCHAR (MAX) NOT NULL,
    [Author]     NVARCHAR (100) NULL,
    [ImageUrl]   NVARCHAR (255) NULL,
    [Tags]       NVARCHAR (500) NULL,
    [LikeCount]  INT            DEFAULT ((0)) NOT NULL,
    [CategoryId] INT            NOT NULL,
    CONSTRAINT [PK_DevJoke] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_DevJoke_Category_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([Id]) ON DELETE CASCADE
);






GO
CREATE NONCLUSTERED INDEX [IX_DevJoke_CategoryId]
    ON [dbo].[DevJoke]([CategoryId] ASC);

