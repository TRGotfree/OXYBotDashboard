
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.getAllDiscountCards
	
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
	 c.CARD_ID,
	 c.USER_FIO,
	 c.BIRTH_DATE,
	 c.PHONE,
	 ISNULL(c.EMAIL, ''),
	 c.SEX,
	 c.IS_USER_WANT_GET_NEWS,
	 c.IS_REGISTERED,
	 tg.ChatId
	FROM dbo.Cards c
	LEFT JOIN dbo.TelegramUsers tg ON tg.Id = c.TELEGRAM_USER_ID;
END
GO