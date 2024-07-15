CREATE TABLE [dbo].[AuditTrail] (
    [Id]                     BIGINT             IDENTITY (1, 1) NOT NULL,
    [Timestamp]              DATETIMEOFFSET (7) NOT NULL,
    [User]                   NVARCHAR (255)     NOT NULL,
    [Area]                   NVARCHAR (450)     NOT NULL,
    [PrimaryKey]             NVARCHAR (255)     NULL,
    [Category]               NVARCHAR (450)     NOT NULL,
    [Action]                 NVARCHAR (255)     NOT NULL,
    [DataSource]             NVARCHAR (450)     NULL,
    [OriginalContent]        NVARCHAR (MAX)     NULL,
    [CurrentContent]         NVARCHAR (MAX)     NULL,
    [Signature]              NVARCHAR (MAX)     NULL,
    [SignaturePublicKeyInfo] NVARCHAR (MAX)     NULL,
    [State]                  INT                NOT NULL,
    CONSTRAINT [PK_AuditTrail] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE NONCLUSTERED INDEX [IX_AuditTrail_Action]
    ON [dbo].[AuditTrail]([Action] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_AuditTrail_Area]
    ON [dbo].[AuditTrail]([Area] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_AuditTrail_Category]
    ON [dbo].[AuditTrail]([Category] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_AuditTrail_DataSource]
    ON [dbo].[AuditTrail]([DataSource] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_AuditTrail_PrimaryKey]
    ON [dbo].[AuditTrail]([PrimaryKey] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_AuditTrail_State]
    ON [dbo].[AuditTrail]([State] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_AuditTrail_Timestamp]
    ON [dbo].[AuditTrail]([Timestamp] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_AuditTrail_User]
    ON [dbo].[AuditTrail]([User] ASC);
GO
