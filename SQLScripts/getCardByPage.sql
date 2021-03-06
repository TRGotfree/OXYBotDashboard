USE [telegramBotOXY]
GO
/****** Object:  StoredProcedure [dbo].[getCardByPage]    Script Date: 10.03.2019 20:36:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[getCardByPage]
   @previousPage int,
   @nextPage int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @countOfCards INT = (SELECT COUNT(Id) FROM dbo.Cards);

	SELECT  *
    FROM (SELECT ROW_NUMBER() OVER ( ORDER BY COUNT(c.ID) DESC ) AS RowNum, 
	                c.ID, 
					c.CARD_ID, 
					c.USER_FIO, 
					c.BIRTH_DATE, 
					c.PHONE, 
					c.EMAIL, 
					c.SEX, 
					c.IS_USER_WANT_GET_NEWS, 
					c.IS_REGISTERED, 
					tgu.ChatId, 
					@countOfCards AS CardCount,
					c.DATETIME_ENTERED
	           FROM dbo.Cards c
	            LEFT JOIN dbo.TelegramUsers tgu 
	           ON c.TELEGRAM_USER_ID = tgu.Id
			   GROUP BY  
			        c.ID, 
					c.CARD_ID, 
					c.USER_FIO, 
					c.BIRTH_DATE, 
					c.PHONE, 
					c.EMAIL, 
					c.SEX, 
					c.IS_USER_WANT_GET_NEWS, 
					c.IS_REGISTERED, 
					tgu.ChatId,
					c.DATETIME_ENTERED       
            ) AS RowConstrainedResult
    WHERE RowNum >= @previousPage
    AND RowNum < @nextPage
ORDER BY RowNum


   
END