CREATE TABLE [dbo].[ViewDefinition] (
    [Id]                           UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Route]                        NVARCHAR (255)   NOT NULL,
    [ViewType]                     NVARCHAR (255)   NOT NULL,
    [DtoType]                      NVARCHAR (255)   NOT NULL,
    [PrimaryKeyType]               NVARCHAR (255)   NOT NULL,
    [PrimaryKeyPropertyExpression] NVARCHAR (255)   NOT NULL,
    [ServiceClientType]            NVARCHAR (255)   NOT NULL,
    [GetExpression]                NVARCHAR (MAX)   NULL,
    [GetByIdExpression]            NVARCHAR (MAX)   NULL,
    [DeleteExpression]             NVARCHAR (MAX)   NULL,
    [CreateExpression]             NVARCHAR (MAX)   NULL,
    [UpdateExpression]             NVARCHAR (MAX)   NULL,
    [TitleLabelId]                 NVARCHAR (255)   NULL,
    [CssClass]                     NVARCHAR (255)   NULL,
    [IsAuthorizedView]             BIT              NOT NULL,
    [Position]                     INT              NOT NULL,
    [Metadata]                     NVARCHAR (MAX)   NULL,
    [ParentId]                     UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_ViewDefinition] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ViewDefinition_ViewDefinition_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[ViewDefinition] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_ViewDefinition_ParentId]
    ON [dbo].[ViewDefinition]([ParentId] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ViewDefinition_Route]
    ON [dbo].[ViewDefinition]([Route] ASC);

