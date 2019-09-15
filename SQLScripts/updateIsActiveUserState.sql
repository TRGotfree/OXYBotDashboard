SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.updateIsActiveUserState
   @userId bigint,
   @isActive bit
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.TelegramUsers SET IsActive = @isActive WHERE ChatId = @userId;
END
GO
