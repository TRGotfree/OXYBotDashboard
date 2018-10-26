SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE dbo.getDistricts	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT ID, COMMAND, ISNULL(NAME, '') FROM dbo.District WHERE IsDeleted = 0;
END
GO
