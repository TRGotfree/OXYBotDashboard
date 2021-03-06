USE [telegramBotOXY]
GO
/****** Object:  StoredProcedure [dbo].[getGoodAnnotation]    Script Date: 13.03.2019 12:45:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[getGoodAnnotation]
   @previousPage int,
   @nextPage int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @totalCountOfAnnot INT = (SELECT COUNT(*) FROM dbo.DrugAnnotation da);
	DECLARE @goodsWithImages INT = (SELECT COUNT(da.AnnotationDrugId) FROM dbo.DrugAnnotation da 
									 LEFT JOIN dbo.DrugImages di 
									ON di.DrugAnnotationId = da.AnnotationDrugId
									WHERE di.Id IS NOT NULL);

	DECLARE @goodsWithoutImages INT = @totalCountOfAnnot - @goodsWithImages;

	SELECT *
    FROM (SELECT ROW_NUMBER() OVER ( ORDER BY da.AnnotationDrugId ASC ) AS RowNum,
	 da.AnnotationDrugId [AnnotationId], 
	 ISNULL(da.DrugName, '') [DrugName], 
	 ISNULL(da.Producer, '') [Producer], 
	 ISNULL(da.UsingWay, '') [UsingWay], 
	 ISNULL(da.ForWhatIsUse, '') [ForWhatIsUse], 
	 ISNULL(da.SpecialInstructions, '') [SpecialInstructions], 
	 RTRIM(LTRIM(ISNULL(da.Contraindications, ''))) [ContraIndicators], 
	 RTRIM(LTRIM(ISNULL(da.SideEffects, ''))) [SideEffects],
	 (CASE WHEN di.Id > 0 THEN 1 ELSE 0 END) [IsImageExists],
	 @totalCountOfAnnot [TotalCountOfAnnotation],
	 @goodsWithImages [GoodsWithImages],
	 @goodsWithoutImages [GoodsWithoutImages]
	FROM dbo.DrugAnnotation da 
	 LEFT JOIN dbo.DrugImages di ON di.DrugAnnotationId = da.AnnotationDrugId
	GROUP BY 
	 da.AnnotationDrugId, 
	 da.DrugName, 
	 da.Producer, 
	 da.UsingWay, 
	 da.ForWhatIsUse, 
	 da.SpecialInstructions, 
	 da.Contraindications, 
	 da.SideEffects,
	 di.Id
	) AS RowConstrainedResult
    WHERE RowNum >= @previousPage
    AND RowNum < @nextPage
    ORDER BY RowNum


END