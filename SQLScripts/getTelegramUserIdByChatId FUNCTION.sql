
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION dbo.getTelegramUserIdByChatId 
(@chatId bigint)
RETURNS INT
AS
BEGIN
	DECLARE @tgUserId INT = (SELECT TOP 1 tg.Id FROM dbo.TelegramUsers tg WHERE tg.ChatId = @chatId);
	RETURN @tgUserId;
END
