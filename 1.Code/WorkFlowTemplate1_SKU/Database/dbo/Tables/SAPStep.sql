CREATE TABLE [dbo].[SAPStep] (
    [Id]           INT              IDENTITY (1, 1) NOT NULL,
    [Priority]     FLOAT (53)       NOT NULL,
    [StepNumber]   INT              NOT NULL,
    [StepName]     NVARCHAR (256)   NOT NULL,
    [RecordStatus] INT              NOT NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [CreatedTime]  DATETIME         NULL,
    [ModifiedBy]   UNIQUEIDENTIFIER NULL,
    [ModifiedOn]   DATETIME         NULL,
    CONSTRAINT [PK_SAPStep] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

