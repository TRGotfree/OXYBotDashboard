-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE dbo.insertOrUpdateCardOwnerInfo
    @chatId bigint,
	@cardId nvarchar(50),
	@fio nvarchar(200),
	@birthDate date,
	@phone nvarchar(50),
	@email nvarchar(50),
	@sex nvarchar(6),
	@isUserWantsToGetNews bit,
	@isUserRegistered bit
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @tgUserId INT = dbo.getTelegramUserIdByChatId(@chatId);

	IF EXISTS (SELECT 1 FROM dbo.Cards c WHERE c.TELEGRAM_USER_ID = @tgUserId)
	  BEGIN
	   UPDATE dbo.Cards 
	   SET CARD_ID = @cardId, USER_FIO = @fio, BIRTH_DATE = @birthDate, PHONE = @phone, 
	       EMAIL = @email, SEX = @sex, IS_USER_WANT_GET_NEWS = @isUserWantsToGetNews, 
		   IS_REGISTERED = @isUserRegistered 
       WHERE TELEGRAM_USER_ID = @tgUserId; 	   
	  END
	ELSE
	  BEGIN
		INSERT INTO dbo.Cards 
		 (CARD_ID, USER_FIO, BIRTH_DATE, PHONE, EMAIL, SEX, IS_USER_WANT_GET_NEWS, IS_REGISTERED, TELEGRAM_USER_ID)
        VALUES
		 (@cardId, @fio, @birthDate, @phone, @email, @sex, @isUserWantsToGetNews, @isUserRegistered, @tgUserId)
	  END    
	
END
GO
