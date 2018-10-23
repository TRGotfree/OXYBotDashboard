
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.updateDrugStore  
    @id int,  
	@drugStoreId int,
	@name nvarchar(75),
	@address nvarchar(100),
	@status bit,
	@phone nvarchar(50),
	@workTime nvarchar(100),
	@orientir nvarchar(100),
	@district nvarchar(50),
	@shortName nvarchar(15)
AS
BEGIN
   UPDATE dbo.DrugStore 
   SET  DrugStoreId = @id, 
	    Name = @name, 
		[Address] = @address, 
		DrugStoreStatus = @status, 
		PhoneNumber = @phone, 
		DrugStoreWorkTime = @workTime, 
		DrugStoreOrientir = @orientir, 
		DrugStoreDistrict = @district,
		DrugStoreShortName = @shortName
   WHERE Id = @id;  
END
 
