USE CandyStore
GO

CREATE PROC spAddCustomer
	@name NVARCHAR(50),
	@surname NVARCHAR(50),
	@newId INT OUTPUT

AS

BEGIN
	INSERT INTO tblCustomer
			(FirstName,
			Surname,
			Discount)
	VALUES (@name, @surname, 0);
	SET @newId = SCOPE_IDENTITY();
END;
GO

CREATE PROC spAddNewCandy
	@name NVARCHAR(50),
	@manufacturer NVARCHAR(50),
	@price NUMERIC(18, 4),
	@supply NUMERIC(18, 4), 
	@newId INT OUTPUT

AS

BEGIN

	INSERT INTO tblCandy
			(Name, Manufacturer, Price, SupplyQty ,Promotion, Deleted)
	VALUES
			(@name, @manufacturer, @price, @supply, 0, 0);
			
	SET @newId = SCOPE_IDENTITY();
END
GO


CREATE PROC spAddPurchase
	@customerId INT = NULL,
	@userId INT,
	@time DATETIME,
	@newId INT OUTPUT

AS

BEGIN
	INSERT INTO tblPurchase
			(CustomerId, Cost, PurchaseTime, UserId)
	VALUES (@customerId, 0, @time, @userId);

	SET @newId = SCOPE_IDENTITY();

	SELECT Id, CustomerId, Cost, PurchaseTime, UserId
	FROM tblPurchase
	WHERE Id = @newId;
END
GO


CREATE PROC spAddPurchaseItem
	@customerId INT = NULL,
	@purchaseId INT,
	@candyId INT,
	@amount NUMERIC(18, 4),

	@cost NUMERIC(18, 4) OUTPUT,
	@newId INT OUTPUT

AS

BEGIN
	IF  @amount > (SELECT SupplyQty FROM tblCandy WHERE Id = @candyId)
	THROW 60000, 'There are not enough candies in stock', 1;

	IF  1 = (SELECT Deleted FROM tblCandy WHERE Id = @candyId) OR
		NOT EXISTS (SELECT 1 FROM tblCandy WHERE Id = @candyId)
	THROW 61000, 'This candy can''t be bought', 1;

	-- Calculate cost
	DECLARE @candyPrice NUMERIC(18, 4) = 0;
	SELECT @candyPrice = Price FROM tblCandy WHERE Id = @candyId;
	
	DECLARE @candyPromo NUMERIC(18, 4) = 0;
	SELECT @candyPromo = Promotion FROM tblCandy WHERE Id = @candyId;
	
	DECLARE @discount NUMERIC(18, 4) = 0;
	SELECT @discount = Discount FROM tblCustomer WHERE Id = @customerId;
	
	SET @cost = @candyPrice * (1 - @candyPromo / 100) * @amount * ( 1 - @discount / 100);

	-- Update candies supply quantities
	UPDATE tblCandy SET SupplyQty = SupplyQty - @amount WHERE Id = @candyId;

	-- Insert new purchase item
	INSERT INTO tblPurchaseItem (PurchaseId, CandyId, Amount, Cost)
		VALUES (@purchaseId, @candyId, @amount, @cost);
	SET @newId = SCOPE_IDENTITY();

	-- Update purchase cost
	UPDATE tblPurchase SET Cost = Cost + @cost WHERE Id = @purchaseId; 	
	
END
GO


CREATE PROC spChangePromotion
	@candyId INT,
	@promotion NUMERIC(18, 4)

AS

BEGIN
	UPDATE tblCandy
	SET Promotion = @promotion
	WHERE Id = @candyId AND Deleted <> 1;
END
GO


CREATE PROC spConfirmPurchase
	@purchaseId INT,
	@customerId INT = NULL,
	@discount NUMERIC(18, 4) OUTPUT

AS

BEGIN
	IF @customerId IS NOT NULL
	BEGIN
		SET @discount = 0;
		
		-- Calculate customers spending
		DECLARE @totalCost NUMERIC(18, 4) = 0;
		SELECT  @totalCost = SUM(Cost) FROM tblPurchase WHERE CustomerId = @customerId;

		
		DECLARE @cost NUMERIC(18, 4) = 0;
		SELECT  @cost = Cost FROM tblPurchase WHERE Id = @purchaseId;

		-- Give new discount to customer
		IF @cost > 500 AND @discount < 5 
		BEGIN
			UPDATE tblCustomer SET Discount = 5 WHERE Id = @customerId;
		END
		ELSE IF @totalCost > 1500 AND @discount < 4 
		BEGIN
			UPDATE tblCustomer SET Discount = 4 WHERE Id = @customerId;
		END
		SELECT @discount = Discount FROM tblCustomer WHERE Id = @customerId;
	END

	SELECT Id, CustomerId, Cost, PurchaseTime, UserId
	FROM tblPurchase
	WHERE Id = @purchaseId;
END
GO


CREATE PROC spDeleteCandyById
	@candyId INT 

AS

BEGIN

	UPDATE tblCandy
	SET Deleted = 1
	WHERE Id = @candyId;
END
GO


CREATE PROC spGetAllCandies

AS

BEGIN
	SELECT 
		Id, 
		Name, 
		Manufacturer, 
		Price, 
		SupplyQty, 
		Promotion,
		Deleted
	FROM tblCandy
	WHERE Deleted <> 1;
END;
GO


CREATE PROC spGetAllCustomers

AS

BEGIN
	SELECT Id,
	       FirstName,
	       Surname,
	       Discount
	FROM tblCustomer
	ORDER BY FirstName;
	
END;
GO


CREATE PROC spGetAllPurchases

AS

BEGIN
	SELECT Id, 
		CustomerId, 
		Cost, 
		PurchaseTime, 
		UserId
	FROM tblPurchase
	ORDER BY PurchaseTime DESC;
	
END;
GO


CREATE PROC spGetCandyById
	@candyId INT

AS

BEGIN
	SELECT  Id, 
			Name, 
			Manufacturer, 
			Price, 
			SupplyQty, 
			Promotion,
			Deleted
	FROM tblCandy
	WHERE Id = @candyId;
	
END;
GO


CREATE PROC spGetCurrentPurchaseItems
	@purchaseId INT

AS

BEGIN
	SELECT Id, 
		PurchaseId, 
		CandyId, 
		Amount, 
		Cost
	FROM tblPurchaseItem 
	WHERE PurchaseId = @purchaseId
	
END;
GO


CREATE PROC spGetCustomerById
	@customerId INT

AS

BEGIN
	SELECT Id,
	       FirstName,
	       Surname,
	       Discount
	FROM tblCustomer
	WHERE Id = @customerId;
	
END;
GO


CREATE PROC spGetPurchaseItemById
	@purchaseItemId INT

AS

BEGIN
	SELECT Id, 
		PurchaseId, 
		CandyId, 
		Amount, 
		Cost
	FROM tblPurchaseItem 
	WHERE Id = @purchaseItemId
END;
GO


CREATE PROC spGetUserByLogin
	@Login VARCHAR(50),
	@Password VARCHAR(50)

AS

BEGIN
	SELECT 
		Id, 
		FirstName, 
		Surname, 
		[Login],
		[Disabled] 
	FROM tblUser
	WHERE [Login] = @Login and [Password] = @Password and [Disabled] <> 1;
END;
GO


CREATE PROC spReplenishSupplies
	@candyId INT,
	@supply NUMERIC(18, 4)

AS

BEGIN
	UPDATE tblCandy
	SET SupplyQty = SupplyQty + @supply
	WHERE Id = @candyId AND Deleted <> 1;
END
GO
