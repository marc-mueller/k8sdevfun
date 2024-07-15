CREATE TABLE [dbo].[ViewDefinitionViewAction] (
    [Id]                   UNIQUEIDENTIFIER NOT NULL,
    [ActionFieldType]      INT              NOT NULL,
    [ActionType]           INT              NOT NULL,
    [Position]             INT              NOT NULL,
    [CommandExpression]    NVARCHAR (MAX)   NULL,
    [CanExecuteExpression] NVARCHAR (MAX)   NULL,
    [IsReloadAfterCommand] BIT              NOT NULL,
    [IconClass]            NVARCHAR (255)   NULL,
    [TooltipTextLabelId]   NVARCHAR (255)   NULL,
    [Metadata]             NVARCHAR (MAX)   NULL, 
    [ViewDefinitionId]     UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_ViewDefinitionViewAction] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ViewDefinitionViewAction_ViewDefinition_ViewDefinitionId] FOREIGN KEY ([ViewDefinitionId]) REFERENCES [dbo].[ViewDefinition] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ViewDefinitionViewAction_ViewDefinitionId]
    ON [dbo].[ViewDefinitionViewAction]([ViewDefinitionId] ASC);

