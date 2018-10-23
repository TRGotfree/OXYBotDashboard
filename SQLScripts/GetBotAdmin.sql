
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.getBotAdmin
  @login nvarchar(100),
  @pass nvarchar(64)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TOP 1 AdminId, [Login], [Password], [State] FROM dbo.BotAdmin WHERE [Login] = @login AND [Password] = @pass;
END

