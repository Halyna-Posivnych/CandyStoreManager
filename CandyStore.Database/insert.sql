USE CandyStore
GO

-- Candy insert
SET IDENTITY_INSERT tblCandy ON

INSERT INTO tblCandy
           (Id, Name, Manufacturer, Price, SupplyQty ,Promotion, Deleted)
VALUES
		(1, 'Romashka', 'Svitoch', 96, 10, 0, 0),
		(2, 'Korivka', 'Roshen', 54, 10, 0, 0),
		(3, 'Sharm', 'AVK', 97, 10, 0, 0),
		(4, 'Bim-bom', 'Roshen', 58, 10, 0, 0),
		(5, 'Zoriane Siaivo', 'Svitoch', 105, 10, 0, 0),
		(6, 'Sheridan', 'AVK', 78, 10, 0, 0),
		(7, 'Jon Krocker', 'Roshen', 85, 10, 0, 0),
		(8, 'Rachky', 'Roshen', 62, 10, 0, 0),
		(9, 'Mak', 'Svitoch', 120, 10, 0, 0),
		(10, 'Rafaello', 'Fererro', 200, 10, 0, 0)

SET IDENTITY_INSERT tblCandy OFF
GO


-- Customer insert
SET IDENTITY_INSERT tblCustomer ON

INSERT INTO tblCustomer
			(Id, FirstName, Surname, Discount)
VALUES
		(1, 'Peter', 'Zoller', 0),
		(2, 'Rosalyn', 'Yalow', 0),
		(3, 'Emil', 'Wolf', 0),
		(4, 'Max', 'Volmer', 0),
		(5, 'Stanislaw', 'Ulam', 0),
		(6, 'Charles', 'Thorn', 0),
		(7, 'Lev', 'Shubnikov', 0),
		(8, 'Sidney', 'Redner', 0),
		(9, 'Leonid', 'Mandelshtam', 0),
		(10, 'Ivan', 'Pulyuy', 0)

SET IDENTITY_INSERT tblCustomer OFF
GO

-- User insert
SET IDENTITY_INSERT tblUser ON
GO

INSERT INTO tblUser 
		(Id, FirstName, Surname, [Login], [Password], [Disabled])
VALUES
		(1,'Candy','Seller','candy_seller','b59c67bf196a4758191e42f76670ceba',0),
		(2,'Bob','Trix','Bobx','e10adc3949ba59abbe56e057f20f883e',0),
        (3,'John','Cena','JCena','c33367701511b4f6020ec61ded352059',0),
		(4, 'Mr.', 'Bean', 'BeanKing', '4f351e69c91975f5532533db26492bd7', 0),
		(5, 'Ben', 'Gann', 'Archon1', 'c3981fa8d26e95d911fe8eaeb6570f2f', 0);

SET IDENTITY_INSERT tblUser OFF
GO

-- Purchase insert

SET IDENTITY_INSERT tblPurchase ON
GO

INSERT INTO tblPurchase
			(Id, CustomerId, Cost, PurchaseTime, UserId)
VALUES
		(1, 2, 48, '2016-03-18 10:15:00', 4), 
		(2, 2, 27, '2016-03-18 13:05:00', 4), 
		(3, 3, 97, '2016-03-18 17:15:00', 4), 
		(4, 4, 29, '2016-03-18 20:10:00', 4), 
		(5, 5, 21, '2016-03-19 11:15:00', 4), 
		(6, 6, 39, '2016-03-19 15:20:00', 4), 
		(7, 7, 42.5, '2016-03-19 16:15:00', 3), 
		(8, 8, 31, '2016-03-19 18:55:00', 3), 
		(9, 9, 60, '2016-03-19 19:15:00', 3), 
		(10, 9, 158, '2016-03-20 13:20:00', 2)

SET IDENTITY_INSERT tblPurchase OFF
GO


-- PurchaseItem insert
SET IDENTITY_INSERT tblPurchaseItem ON

INSERT INTO tblPurchaseItem
			(Id, PurchaseId, CandyId, Amount, Cost)
VALUES
		(1, 1, 1, 0.5, 48), 
		(2, 2, 2, 0.5, 27), 
		(3, 3, 3, 1.0, 97), 
		(4, 4, 4, 0.5, 29), 
		(5, 5, 5, 0.2, 21), 
		(6, 6, 6, 0.5, 39), 
		(7, 7, 7, 0.5, 42.5), 
		(8, 8, 8, 0.5, 31), 
		(9, 9, 9, 0.5, 60), 
		(10, 10, 10, 0.4, 80), 
		(11, 10, 9, 0.2, 24), 
		(12, 10, 2, 1.0, 54)

SET IDENTITY_INSERT tblPurchaseItem OFF
GO

