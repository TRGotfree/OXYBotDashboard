USE [telegramBotOXY]
GO
/****** Object:  StoredProcedure [dbo].[insertNewTelegramUser]    Script Date: 23.04.2019 12:18:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[getBotAminsMessageGetters]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT ChatId FROM dbo.BotAdmin WHERE IsMessageGetter = 1 AND ChatId IS NOT NULL AND ChatId > 0;
END

