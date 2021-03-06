USE [telegramBotOXY]
GO
/****** Object:  StoredProcedure [dbo].[updateIsActiveUserState]    Script Date: 15.09.2019 19:52:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[updateIsActiveUserState]
   @userId bigint,
   @isActive bit
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
	 BEGIN TRANSACTION;
	  UPDATE dbo.TelegramUsers SET IsActive = @isActive WHERE ChatId = @userId;
     COMMIT TRANSACTION;
	 END TRY
	 BEGIN CATCH
	  IF @@TRANCOUNT > 0
	    BEGIN
	     ROLLBACK TRANSACTION;
	    END
	    DECLARE @error NVARCHAR(MAX) = ERROR_MESSAGE();
	    RAISERROR(@error, 16, 1);
	 END CATCH 

END
