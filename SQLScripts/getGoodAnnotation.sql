USE [telegramBotOXY]
GO
/****** Object:  StoredProcedure [dbo].[getGoodAnnotation]    Script Date: 27.01.2019 19:06:12 ******/
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

	SELECT 
	       da.AnnotationDrugId, 
	       ISNULL(da.DrugName, ''), 
	       ISNULL(da.Producer, ''), 
	       ISNULL(da.UsingWay, '') [Способ применения], 
		   ISNULL(da.ForWhatIsUse, '') [Для чего], 
		   ISNULL(da.SpecialInstructions, '') [Специальные указания], 
	       RTRIM(LTRIM(ISNULL(da.Contraindications, ''))) [Противопоказания], 
	       RTRIM(LTRIM(ISNULL(da.SideEffects, ''))) [Побочные эффекты],
		   (CASE WHEN di.Id > 0 THEN 1 ELSE 0 END),
		   @totalCountOfAnnot [TotalCount]
	FROM dbo.DrugAnnotation da
	 LEFT JOIN dbo.DrugImages di ON di.DrugAnnotationId = da.AnnotationDrugId
	ORDER BY da.AnnotationDrugId OFFSET @previousPage 
	ROWS FETCH NEXT @nextPage ROWS ONLY;

END