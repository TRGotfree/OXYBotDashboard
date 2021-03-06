USE [telegramBotOXY]
GO
/****** Object:  StoredProcedure [dbo].[getGoodAnnotationById]    Script Date: 27.01.2019 18:56:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[getGoodAnnotationById]
   @id int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
	       da.AnnotationDrugId, 
	       ISNULL(da.DrugName, ''), 
	       ISNULL(da.Producer, ''), 
	       ISNULL(da.UsingWay, '') [Способ применения], 
		   ISNULL(da.ForWhatIsUse, '') [Для чего], 
		   ISNULL(da.SpecialInstructions, '') [Специальные указания], 
	       RTRIM(LTRIM(ISNULL(da.Contraindications, ''))) [Противопоказания], 
	       RTRIM(LTRIM(ISNULL(da.SideEffects, ''))) [Побочные эффекты],
		   (CASE WHEN di.Id > 0 THEN 1 ELSE 0 END)
	FROM dbo.DrugAnnotation da
	 LEFT JOIN dbo.DrugImages di ON di.DrugAnnotationId = da.AnnotationDrugId
	WHERE da.AnnotationDrugId = @id;

END