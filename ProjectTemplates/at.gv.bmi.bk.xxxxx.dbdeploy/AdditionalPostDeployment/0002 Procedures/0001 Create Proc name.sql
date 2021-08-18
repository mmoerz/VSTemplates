CREATE Procedure [dbo].[emptyProc]
(
	@Id INT 
)
AS BEGIN
	SELECT GetDate(), @ID
END