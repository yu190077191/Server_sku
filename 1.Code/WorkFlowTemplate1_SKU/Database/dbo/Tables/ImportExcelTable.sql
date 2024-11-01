CREATE TABLE [dbo].[ImportExcelTable] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (128) NOT NULL,
    [ColumnNames] NVARCHAR (MAX) NULL,
    [Priority]    FLOAT (53)     NOT NULL,
    [Template]    NVARCHAR (128) NULL
);

