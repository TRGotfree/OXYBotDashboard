SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.insertOrUpdateAnnotation
   @annotationId int,
   @drugName nvarchar(255),
   @producer nvarchar(255),
   @usingWay nvarchar(max),
   @forWhatIsUse nvarchar(max),
   @specialInstructions nvarchar(max),
   @contraIndicators nvarchar(max),
   @sideEffects nvarchar(max)
AS
BEGIN
	
	IF EXISTS (SELECT 1 FROM dbo.DrugAnnotation da WHERE da.AnnotationDrugId = @annotationId)
	  BEGIN
	   UPDATE dbo.DrugAnnotation 
	   SET 
	    DrugName = @drugName, 
		Producer = @producer,
		ForWhatIsUse = @forWhatIsUse,
		SpecialInstructions = @specialInstructions,
		UsingWay = @usingWay,
		Contraindications = @contraIndicators,
		SideEffects = @sideEffects
       WHERE AnnotationDrugId = @annotationId;
	  END
	ELSE
	  BEGIN
	   INSERT INTO dbo.DrugAnnotation
	    (AnnotationDrugId, DrugName, Producer, ForWhatIsUse, SpecialInstructions, UsingWay, Contraindications, SideEffects)
	   VALUES
	    (@annotationId, @drugName, @producer, @forWhatIsUse, @specialInstructions, @usingWay, @contraIndicators, @sideEffects)	
	  END

END
