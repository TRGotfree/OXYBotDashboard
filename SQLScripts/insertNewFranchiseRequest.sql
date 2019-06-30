
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.insertNewFranchiseRequest
   @chatId bigint,
   @nameAndPhone nvarchar(100)
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO dbo.FranchiseRequest 
	 (TelegramUserId, PhoneAndName)
	VALUES
     (dbo.getTelegramUserIdByChatId(@chatId), @nameAndPhone);
END

