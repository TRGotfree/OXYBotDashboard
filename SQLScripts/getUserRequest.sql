SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
CREATE PROCEDURE dbo.getUserRequest
   @previousPage int,
   @nextPage int
AS
BEGIN
	SET NOCOUNT ON;

	SET NOCOUNT ON;

	DECLARE @totalCountOfAdvertActions INT = (SELECT COUNT(*) FROM dbo.Advertising);

	SELECT *
    FROM (SELECT ROW_NUMBER() OVER ( ORDER BY COUNT(msg.MessageId) DESC ) AS RowNum, 
	                msg.MessageId as Id,
	                msg.MessageText AS MessageText, 
				    msg.ChatId AS chatId,
					ISNULL(msg.UserName, '') as NickName, 
					ISNULL(msg.UserFirstName, '') as FirstName,
					ISNULL(msg.UserLastName, '') as LastName, 
				    msg.MessageDateTime as RequestDateTime
	           FROM dbo.MessagesInfo msg
			   WHERE msg.MessageText IS NOT NULL
			   GROUP BY msg.MessageId, msg.MessageText, msg.ChatId, 
			   msg.UserName, msg.UserFirstName, msg.UserLastName, msg.MessageDateTime           	          
            ) AS RowConstrainedResult
    WHERE RowNum >= @previousPage
    AND RowNum < @nextPage
    ORDER BY RowNum

END
GO
