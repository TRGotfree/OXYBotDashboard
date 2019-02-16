USE [telegramBotOXY]
GO
/****** Object:  StoredProcedure [dbo].[getDrugStores]    Script Date: 16.02.2019 22:22:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[getDrugStores]
   @previousPage int,
   @nextPage int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @drugStoreCount INT = (SELECT COUNT(*) FROM dbo.DrugStore WHERE DrugStoreStatus = 1);	
	SELECT *
    FROM (SELECT ROW_NUMBER() OVER ( ORDER BY ds.Id DESC ) AS RowNum,
	 ds.Id as id, 
	 ISNULL(ds.DrugStoreId, 0) as drugStoreId, 
	 ISNULL(ds.Name, '') as name, 
	 ISNULL(ds.[Address], '') as [address], 
	 ISNULL(ds.DrugStoreStatus, 0) as [status], 
	 ISNULL(ds.PhoneNumber, '') as phone, 
	 ISNULL(ds.DrugStoreWorkTime, '') as workTime,
	 ISNULL(ds.DrugStoreOrientir, '') as orientir,
	 ISNULL(ds.DrugStoreDistrict, '') as district,
	 ISNULL(ds.DrugStoreShortName, '') as shortname,
	 @drugStoreCount as dsCount
	FROM dbo.DrugStore ds
	GROUP BY 
	 ds.Id, 
	 ds.DrugStoreId, 
	 ds.Name, 
	 ds.[Address], 
	 ds.DrugStoreStatus, 
	 ds.PhoneNumber, 
	 ds.DrugStoreWorkTime,
	 ds.DrugStoreOrientir,
	 ds.DrugStoreDistrict,
	 ds.DrugStoreShortName
	) AS RowConstrainedResult
    WHERE RowNum >= @previousPage
    AND RowNum < @nextPage
    ORDER BY RowNum
END
