USE [telegramBotOXY]
GO
/****** Object:  StoredProcedure [dbo].[insertDrugStore]    Script Date: 16.02.2019 21:56:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[insertOrUpdateDrugStore]
    @id int,
	@name nvarchar(75),
	@address nvarchar(100),
	@status bit,
	@phone nvarchar(50),
	@workTime nvarchar(100),
	@orientir nvarchar(100),
	@district nvarchar(100),
	@shortName nvarchar(15)
AS
BEGIN	
   DECLARE @districtCommand NVARCHAR(100) = (SELECT TOP 1 COMMAND FROM dbo.District WHERE [NAME] = @district);

   IF @districtCommand IS NULL
     RAISERROR('Не удалось найти команду для этого района!', 16, 1);

   IF NOT EXISTS (SELECT 1 FROM dbo.DrugStore WHERE DrugStoreId = @id)
    BEGIN
     INSERT INTO dbo.DrugStore 
      (DrugStoreId,  Name, [Address], DrugStoreStatus, PhoneNumber, DrugStoreWorkTime, DrugStoreOrientir, DrugStoreDistrict, DrugStoreShortName)
     VALUES
      (@id, @name, @address, @status, @phone, @workTime, @orientir, @districtCommand, @shortName);
	END
   ELSE
    BEGIN
	 UPDATE dbo.DrugStore 
      SET  
	    DrugStoreId = @id, 
	    Name = @name, 
		[Address] = @address, 
		DrugStoreStatus = @status, 
		PhoneNumber = @phone, 
		DrugStoreWorkTime = @workTime, 
		DrugStoreOrientir = @orientir, 
		DrugStoreDistrict = @districtCommand,
		DrugStoreShortName = @shortName
      WHERE Id = @id;  	
	END  
END
 
