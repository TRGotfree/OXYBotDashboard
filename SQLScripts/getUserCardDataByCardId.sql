SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.getUserCardDataByCardId
     @cardId bigint
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
	WHERE c.ID = @cardId;
END

