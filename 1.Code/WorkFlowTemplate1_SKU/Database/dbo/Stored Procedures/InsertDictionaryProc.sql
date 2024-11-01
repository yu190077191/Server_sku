
CREATE PROCEDURE [dbo].[InsertDictionaryProc]
   @id INT OUTPUT
  ,@TypeCode NVARCHAR(64)
  ,@Name NVARCHAR(128)
  ,@Value INT
  ,@ValueString NVARCHAR(MAX)
  ,@Priority FLOAT
  ,@Description NVARCHAR(MAX)
  ,@RecordStatus INT
  ,@CreatedBy UNIQUEIDENTIFIER
AS
BEGIN
 INSERT [Dictionary]
(
   [TypeCode]
  ,[Name]
  ,[Value]
  ,[ValueString]
  ,[Priority]
  ,[Description]
  ,[RecordStatus]
  ,[CreatedBy]
  ,[CreatedTime]
  --,[ModifiedBy]
  --,[ModifiedOn]
)
VALUES
(
   @TypeCode
  ,@Name
  ,@Value
  ,@ValueString
  ,@Priority
  ,@Description
  ,@RecordStatus
  ,@CreatedBy
  ,GETDATE()
  --,@ModifiedBy
  --,@ModifiedOn
)
SELECT @id = SCOPE_IDENTITY()
END