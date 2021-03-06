USE [telegramBotOXY]
GO
/****** Object:  StoredProcedure [dbo].[updateAction]    Script Date: 13.10.2018 19:17:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[updateAction]
  @advertId int,
  @advertText nvarchar(2000),
  @nameOfAction nvarchar(50),
  @dateBegin smalldatetime,
  @dateEnd smalldatetime,
  @commandText varchar(30),
  @advertState bit
AS
BEGIN
  UPDATE dbo.Advertising 
  SET AdvertisingText = @advertText, 
      NameOfAction = @nameOfAction,
	  DateBegin = @dateBegin,
	  DateEnd = @dateEnd,
 	  CommandText = @commandText,
	  AdvState = @advertState
  WHERE AdvertisingId = @advertId;
END
