USE [telegramBotOXY]
GO
/****** Object:  StoredProcedure [dbo].[insertDrugStore]    Script Date: 16.10.2018 23:29:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[insertDrugStore]
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
   INSERT INTO dbo.DrugStore 
    (Name, [Address], DrugStoreStatus, PhoneNumber, DrugStoreWorkTime, DrugStoreOrientir, DrugStoreDistrict, DrugStoreShortName)
   VALUES
    (@name, @address, @status, @phone, @workTime, @orientir, @district, @shortName);
END
 
