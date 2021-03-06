USE [telegramBotOXY]
GO
/****** Object:  StoredProcedure [dbo].[getUserCardDataByChatId]    Script Date: 27.02.2019 17:40:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[getUserCardDataByChatId]
     @chatId bigint
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
	 c.ID, 
	 c.CARD_ID, 
	 c.USER_FIO, 
	 c.BIRTH_DATE, 
	 c.PHONE, 
	 ISNULL(c.EMAIL, ''), 
	 c.SEX, 
	 c.IS_USER_WANT_GET_NEWS, 
	 c.IS_REGISTERED
	FROM dbo.Cards c 
	WHERE c.TELEGRAM_USER_ID = dbo.getTelegramUserIdByChatId(@chatId);

END

