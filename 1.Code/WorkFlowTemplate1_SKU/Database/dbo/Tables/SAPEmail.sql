CREATE TABLE [dbo].[SAPEmail] (
    [Id]           INT              IDENTITY (1, 1) NOT NULL,
    [Type]         NVARCHAR (64)    NOT NULL,
    [StepNumber]   INT              NOT NULL,
    [StepId]       INT              NOT NULL,
    [EmailSubject] NVARCHAR (128)   NOT NULL,
    [EmailBody]    NVARCHAR (MAX)   NOT NULL,
    [RecordStatus] INT              NOT NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [CreatedTime]  DATETIME         NULL,
    [ModifiedBy]   UNIQUEIDENTIFIER NULL,
    [ModifiedOn]   DATETIME         NULL,
    CONSTRAINT [PK_SAPEmail] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

