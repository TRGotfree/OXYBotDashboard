SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
ALTER PROCEDURE dbo.getUserRequest
   @previousPage int,
   @nextPage int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @totalCountOfRequests INT = (SELECT COUNT(*) FROM dbo.MessagesInfo msg WHERE msg.MessageText IS NOT NULL);

	SELECT 
	    msg.MessageId as Id,
	    msg.MessageText AS MessageText, 
		msg.ChatId AS chatId,
		ISNULL(msg.UserName, '') as NickName, 
		ISNULL(msg.UserFirstName, '') as FirstName,
		ISNULL(msg.UserLastName, '') as LastName, 
		msg.MessageDateTime as RequestDateTime,
		@totalCountOfRequests as TotalRequests 
	FROM dbo.MessagesInfo msg 
	WHERE msg.MessageText IS NOT NULL 
	ORDER BY msg.MessageId DESC OFFSET @previousPage 
	ROWS FETCH NEXT @nextPage ROWS ONLY;

END
GO
