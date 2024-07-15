CREATE TABLE [dbo].[ViewDefinitionViewFieldGroup] (
    [Id]                    UNIQUEIDENTIFIER NOT NULL,
    [TitleLabelId]          NVARCHAR (255)   NULL,
    [Position]              INT              NOT NULL,
    [IsDefaultGroup]        BIT              NOT NULL,
    [IsDisplayedExpression] NVARCHAR (MAX)   NULL,
    [IsEnabledExpression]   NVARCHAR (MAX)   NULL,
    [CssClass]              NVARCHAR (255)   NULL,
    [ViewDefinitionId]      UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_ViewDefinitionViewFieldGroup] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ViewDefinitionViewFieldGroup_ViewDefinition_ViewDefinitionId] FOREIGN KEY ([ViewDefinitionId]) REFERENCES [dbo].[ViewDefinition] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ViewDefinitionViewFieldGroup_ViewDefinitionId]
    ON [dbo].[ViewDefinitionViewFieldGroup]([ViewDefinitionId] ASC);

