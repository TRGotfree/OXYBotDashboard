USE [telegramBotOXY]
GO
/****** Object:  StoredProcedure [dbo].[getActionsInfo]    Script Date: 05.10.2018 22:05:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[getActionsInfo]
   @previousPage int,
   @nextPage int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @totalCountOfAdvertActions INT = (SELECT COUNT(*) FROM dbo.Advertising);

	SELECT  *
    FROM    (SELECT ROW_NUMBER() OVER ( ORDER BY COUNT(ad.AdvertisingId) DESC ) AS RowNum, 
	                ad.AdvertisingId as Id,
	                ISNULL(ad.AdvertisingText, '') AS advertText, ISNULL(ad.NameOfAction, '') AS name, ad.DateBegin as beginDate, ad.DateEnd as endDate, 
				    ISNULL(ad.CommandText, '') as commandText, ad.AdvState as [state], @totalCountOfAdvertActions as TotalCountOfAdvert
	           FROM dbo.Advertising ad 
			   GROUP BY ad.AdvertisingId, ad.AdvertisingText, ad.NameOfAction, ad.DateBegin, ad.DateEnd, ad.CommandText, ad.AdvState           	          
            ) AS RowConstrainedResult
    WHERE   RowNum >= @previousPage
    AND RowNum < @nextPage
ORDER BY RowNum
END

