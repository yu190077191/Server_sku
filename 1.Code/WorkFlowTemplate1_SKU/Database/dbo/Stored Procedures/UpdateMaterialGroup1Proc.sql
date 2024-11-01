
CREATE PROCEDURE [dbo].[UpdateMaterialGroup1Proc]
   @Id INT
  ,@Name NVARCHAR(128)
AS
BEGIN
 UPDATE [MaterialGroup1]
SET
   [Name] = @Name
WHERE [Id] = @Id
END