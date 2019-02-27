
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE dbo.updatedIsCardRegisteredState 
	@chatId bigint,
	@isRegistered bit
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.Cards SET IS_REGISTERED = @isRegistered WHERE TELEGRAM_USER_ID = dbo.getTelegramUserIdByChatId(@chatId);
END
