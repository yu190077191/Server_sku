CREATE TABLE [dbo].[Field_BAK_20180508] (
    [Id]            INT              IDENTITY (1, 1) NOT NULL,
    [Type]          NVARCHAR (64)    NOT NULL,
    [Category]      NVARCHAR (128)   NOT NULL,
    [Name]          NVARCHAR (128)   NOT NULL,
    [InputType]     NVARCHAR (128)   NOT NULL,
    [Specification] NVARCHAR (MAX)   NULL,
    [Values]        NVARCHAR (MAX)   NULL,
    [TypeCode]      NVARCHAR (64)    NOT NULL,
    [Colspan]       INT              NOT NULL,
    [Priority]      FLOAT (53)       NOT NULL,
    [Description]   NVARCHAR (MAX)   NULL,
    [RecordStatus]  INT              NOT NULL,
    [CreatedBy]     UNIQUEIDENTIFIER NULL,
    [CreatedTime]   DATETIME         NULL,
    [ModifiedBy]    UNIQUEIDENTIFIER NULL,
    [ModifiedOn]    DATETIME         NULL
);

