CREATE TABLE [dbo].[SAPEmailToSetting] (
    [RequestId]    INT              NOT NULL,
    [SAPEmailToId] INT              NOT NULL,
    [Checked]      BIT              NOT NULL,
    [RecordStatus] INT              NOT NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [CreatedTime]  DATETIME         NULL,
    [ModifiedBy]   UNIQUEIDENTIFIER NULL,
    [ModifiedOn]   DATETIME         NULL,
    CONSTRAINT [PK_SAPEmailToSetting] PRIMARY KEY CLUSTERED ([RequestId] ASC, [SAPEmailToId] ASC) WITH (FILLFACTOR = 90)
);

