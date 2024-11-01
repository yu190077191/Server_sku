create PROCEDURE [dbo].[InsertMaterialGroup1Proc]
   @Id INT
  ,@Name NVARCHAR(128)
AS
BEGIN
 INSERT [MaterialGroup1]
(
  [Name]
)
VALUES
(
  @Name
)
END