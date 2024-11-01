
CREATE PROCEDURE [dbo].[UpdateDictionaryProc]
   @Id INT
  ,@TypeCode NVARCHAR(64)
  ,@Name NVARCHAR(128)
  ,@Value INT
  ,@ValueString NVARCHAR(MAX)
  ,@Priority FLOAT
  ,@Description NVARCHAR(MAX)
  ,@RecordStatus INT
  ,@CreatedBy UNIQUEIDENTIFIER
  ,@CreatedTime DATETIME
  ,@ModifiedBy UNIQUEIDENTIFIER
  ,@ModifiedOn DATETIME
AS
BEGIN
 UPDATE [Dictionary]
SET
   [TypeCode] = @TypeCode
  ,[Name] = @Name
  ,[Value] = @Value
  ,[ValueString] = @ValueString
  ,[Priority] = @Priority
  ,[Description] = @Description
  ,[RecordStatus] = @RecordStatus
  --,[CreatedBy] = @CreatedBy
  --,[CreatedTime] = @CreatedTime
  ,[ModifiedBy] = @ModifiedBy
  ,[ModifiedOn] = GETDATE()
WHERE [Id] = @Id
END