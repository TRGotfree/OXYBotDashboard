USE [telegramBotOXY]
GO
/****** Object:  StoredProcedure [dbo].[insertNewMessage]    Script Date: 23.01.2019 22:42:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- =============================================
ALTER PROCEDURE [dbo].[insertNewMessage]
   @chatId bigint,
   @userid bigint,
   @username nvarchar(100),
   @userfirstname nvarchar(100), 
   @userlastname nvarchar(100), 
   @messagetext nvarchar(100),
   @commandtext nvarchar(100),
   @messagedatetime smalldatetime
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO MessagesInfo
	 (ChatId, UserId, UserName, UserFirstName, UserLastName, MessageText, MessageDateTime, CommandText) 
	VALUES 
	 (@chatid, @userid, @username, @userfirstname, @userlastname, @messagetext, @messagedatetime, @commandtext);
 
    EXEC dbo.insertNewTelegramUser @chatId, @username, @userfirstname, @userlastname;

END
