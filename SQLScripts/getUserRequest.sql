USE [telegramBotOXY]
GO
/****** Object:  StoredProcedure [dbo].[getUserRequest]    Script Date: 11.03.2019 12:36:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
ALTER PROCEDURE [dbo].[getUserRequest]
   @previousPage int,
   @nextPage int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @today DATE = GETDATE();
	DECLARE @tomorrow DATE = DATEADD(DAY, 1, @today);

	DECLARE @totalCountOfRequests INT = (SELECT COUNT(*) FROM dbo.MessagesInfo msg WHERE msg.MessageText IS NOT NULL);
	DECLARE @todayRequestsCount INT = (SELECT COUNT(*) FROM dbo.MessagesInfo msg WHERE msg.MessageText IS NOT NULL AND msg.MessageDateTime BETWEEN @today AND @tomorrow);

    SELECT  *
    FROM (SELECT ROW_NUMBER() OVER ( ORDER BY COUNT(msg.MessageId) DESC ) AS RowNum, 
	                msg.MessageId, msg.MessageText, msg.ChatId, ISNULL(msg.UserName, '') as NickName, ISNULL(msg.UserFirstName, '') as FirstName, ISNULL(msg.UserLastName, '') as[LastName], 
					msg.MessageDateTime as RequestDateTime,
					@totalCountOfRequests as TotalRequests,
					@todayRequestsCount as TodayRequestCount
	           FROM dbo.MessagesInfo msg 
			   WHERE msg.MessageText IS NOT NULL 
			   GROUP BY msg.MessageId, msg.MessageText, msg.ChatId, msg.UserName, msg.UserFirstName, msg.UserLastName, msg.MessageDateTime      
            ) AS RowConstrainedResult
    WHERE RowNum >= @previousPage
    AND RowNum < @nextPage
	ORDER BY RowNum

END
