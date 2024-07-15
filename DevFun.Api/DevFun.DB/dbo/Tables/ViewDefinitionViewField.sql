CREATE TABLE [dbo].[ViewDefinitionViewField] (
    [Id]                    UNIQUEIDENTIFIER NOT NULL,
    [PropertyExpression]    NVARCHAR (255)   NOT NULL,
    [TitleLabelId]          NVARCHAR (255)   NULL,
    [CellType]              INT              NOT NULL,
    [Position]              INT              NOT NULL,
    [IsSortable]            BIT              NOT NULL,
    [IsSearchable]          BIT              NOT NULL,
    [IsReadonly]            BIT              NOT NULL,
    [IsDisplayedExpression] NVARCHAR (MAX)   NULL,
    [IsEnabledExpression]   NVARCHAR (MAX)   NULL,
    [CssClass]              NVARCHAR (255)   NULL,
    [HideEnumValues]        NVARCHAR (255)   NULL,
    [Metadata]              NVARCHAR (MAX)   NULL, 
    [ViewFieldGroupId]      UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_ViewDefinitionViewField] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ViewDefinitionViewField_ViewDefinitionViewFieldGroup_ViewFieldGroupId] FOREIGN KEY ([ViewFieldGroupId]) REFERENCES [dbo].[ViewDefinitionViewFieldGroup] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ViewDefinitionViewField_ViewFieldGroupId]
    ON [dbo].[ViewDefinitionViewField]([ViewFieldGroupId] ASC);

