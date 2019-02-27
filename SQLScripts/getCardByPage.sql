USE [telegramBotOXY]
GO
/****** Object:  StoredProcedure [dbo].[getTelegramUsers]    Script Date: 27.02.2019 17:43:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[getCardByPage]
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
					@countOfCards AS CardCount
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
					tgu.ChatId       
            ) AS RowConstrainedResult
    WHERE RowNum >= @previousPage
    AND RowNum < @nextPage
ORDER BY RowNum


   
END