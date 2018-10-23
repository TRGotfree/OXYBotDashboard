
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.getAllTelegramUsers
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
	 tg.Id,
	 tg.ChatId,
	 ISNULL(tg.NickName, ''),
	 ISNULL(tg.UserFirstName, ''),
	 ISNULL(tg.UserLastName, '')
	FROM dbo.TelegramUsers tg WHERE tg.ChatId IS NOT NULL AND tg.ChatId > 0
END

