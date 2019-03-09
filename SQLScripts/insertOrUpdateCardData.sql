-- ================================================

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.insertOrUpdateCardData
  @cardId int,
  @chatId bigint,
  @userFIO nvarchar(200),
  @birthDate date,
  @phone nvarchar(50),
  @email nvarchar(50),
  @sex nvarchar(6),
  @isUserWantToGetNews bit,
  @isRegistered bit = 0

AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @tgUserId INT = dbo.getTelegramUserIdByChatId(@chatId);

	IF EXISTS (SELECT 1 FROM dbo.Cards WHERE CARD_ID = @cardId AND TELEGRAM_USER_ID = @tgUserId)
	  BEGIN
		UPDATE dbo.Cards SET 
		 CARD_ID = @cardId, 
		 USER_FIO = @userFIO, 
		 BIRTH_DATE = @birthDate, 
		 PHONE = @phone,
		 EMAIL = @email,
		 SEX = @sex,
		 IS_USER_WANT_GET_NEWS = @isUserWantToGetNews,
		 IS_REGISTERED = @isRegistered 
		WHERE CARD_ID = @cardId AND TELEGRAM_USER_ID = @tgUserId 
	  END
    ELSE
	  BEGIN
	   INSERT INTO dbo.Cards 
	    (CARD_ID, USER_FIO, BIRTH_DATE, PHONE, EMAIL, SEX, IS_USER_WANT_GET_NEWS, IS_REGISTERED) 
	   VALUES
	    (@cardId, @userFIO, @birthDate, @phone, @email, @sex, @isUserWantToGetNews, @isRegistered)
	  END

END
GO
