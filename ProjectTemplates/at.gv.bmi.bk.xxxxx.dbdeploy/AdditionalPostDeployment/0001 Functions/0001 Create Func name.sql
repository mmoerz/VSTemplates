CREATE FUNCTION [dbo].[emptyFunc] ( @Id Int  )
  RETURNS DATE
  AS
  BEGIN
	  DECLARE @today DATE
      SELECT @today = GETDATE()
      RETURN(@today)
  END
GO
