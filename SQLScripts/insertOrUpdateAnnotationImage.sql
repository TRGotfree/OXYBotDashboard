
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.insertOrUpdateAnnotationImage
  @annotationId int,
  @image varbinary(max),
  @fileName nvarchar(300)
AS
BEGIN

IF EXISTS (SELECT 1 FROM dbo.DrugImages WHERE DrugAnnotationId = @annotationId)
  BEGIN
   UPDATE dbo.DrugImages SET [Image] = @image, FileNameWithExtension = @fileName WHERE DrugAnnotationId = @annotationId;
  END
ELSE
  BEGIN
   INSERT dbo.DrugImages 
    (DrugAnnotationId, [Image], FileNameWithExtension)
   VALUES
    (@annotationId, @image, @fileName)
  END
END
GO
