USE [telegramBotOXY]
GO
/****** Object:  StoredProcedure [dbo].[getGoodAnnotation]    Script Date: 27.01.2019 13:47:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[updateAnnotation]
   @id int,
   @usingWay nvarchar(max),
   @forWhatIsUse nvarchar(max),
   @specialInstructions nvarchar(max),
   @contraindicators nvarchar(max),
   @sideEffects nvarchar(max)
AS
BEGIN
   UPDATE dbo.DrugAnnotation
   SET
    UsingWay = @usingWay,
	ForWhatIsUse = @forWhatIsUse,
	SpecialInstructions = @specialInstructions,
	Contraindications = @contraindicators,
	SideEffects = @sideEffects
   WHERE AnnotationDrugId = @id; 
END