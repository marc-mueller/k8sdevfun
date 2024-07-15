CREATE TABLE [dbo].[ViewDefinitionViewFilterField] (
    [Id]                    UNIQUEIDENTIFIER NOT NULL,
    [FieldTitleLabelId]     NVARCHAR (255)   NULL,
    [FilterTitleLabelId]    NVARCHAR (255)   NULL,
    [FilterType]            INT              NOT NULL,
    [IsSetToRouteParameter] BIT              NOT NULL,
    [DataExpression]        NVARCHAR (MAX)   NULL,
    [ViewDefinitionId]      UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_ViewDefinitionViewFilterField] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ViewDefinitionViewFilterField_ViewDefinition_ViewDefinitionId] FOREIGN KEY ([ViewDefinitionId]) REFERENCES [dbo].[ViewDefinition] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ViewDefinitionViewFilterField_ViewDefinitionId]
    ON [dbo].[ViewDefinitionViewFilterField]([ViewDefinitionId] ASC);

