USE [telegramBotOXY]
GO
/****** Object:  StoredProcedure [dbo].[getTelegramUsers]    Script Date: 11.03.2019 10:59:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[getTelegramUsers]
   @previousPage int,
   @nextPage int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @countOfUsers INT = (SELECT COUNT(ChatId) FROM dbo.TelegramUsers);

	SELECT  *
    FROM    (SELECT ROW_NUMBER() OVER ( ORDER BY COUNT(mi.MessageId) DESC ) AS RowNum, 
	                tu.ChatId, ISNULL(tu.NickName, '') [NickName], tu.UserFirstName, ISNULL(tu.UserLastName, '') [LastName], 
					MAX(mi.MessageDateTime) AS LastVisit, COUNT(mi.MessageId) AS MsgCount, 
					@countOfUsers AS UserCount
	           FROM dbo.TelegramUsers tu
	            LEFT JOIN dbo.MessagesInfo mi 
	           ON mi.ChatId = tu.ChatId
	           GROUP BY tu.ChatId, tu.NickName, tu.UserFirstName, tu.UserLastName	          
            ) AS RowConstrainedResult
    WHERE   RowNum >= @previousPage
    AND RowNum < @nextPage
ORDER BY RowNum


   
END