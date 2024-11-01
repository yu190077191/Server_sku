CREATE TABLE [dbo].[SAPEmailTo] (
    [Id]                      INT              IDENTITY (1, 1) NOT NULL,
    [SAPEmailId]              INT              NOT NULL,
    [StepNumber]              INT              NOT NULL,
    [BU]                      NVARCHAR (128)   NOT NULL,
    [Plant]                   NVARCHAR (128)   NOT NULL,
    [EmployeeId]              UNIQUEIDENTIFIER NULL,
    [Email]                   VARCHAR (MAX)    NULL,
    [NeedAllMemebersApproval] BIT              NULL,
    [RecordStatus]            INT              NOT NULL,
    [CreatedBy]               UNIQUEIDENTIFIER NULL,
    [CreatedTime]             DATETIME         NULL,
    [ModifiedBy]              UNIQUEIDENTIFIER NULL,
    [ModifiedOn]              DATETIME         NULL,
    CONSTRAINT [PK_SAPEmailTo] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

