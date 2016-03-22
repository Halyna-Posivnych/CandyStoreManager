-- CREATE DATABASE CandyStore;

USE CandyStore;

GO
CREATE TABLE tblUser
(
	Id INT NOT NULL IDENTITY(1,1),
	FirstName NVARCHAR(50) NOT NULL,
	Surname NVARCHAR(50) NOT NULL,
	[Login] VARCHAR(50) NOT NULL,
	[Password] VARCHAR(50) NOT NULL,
	[Disabled] BIT NOT NULL,
	CONSTRAINT PK_tblUser_Id PRIMARY KEY (Id),
	CONSTRAINT UQ_tblUser_Login UNIQUE ([Login])
);

GO

CREATE TABLE tblCustomer
(
	Id INT NOT NULL IDENTITY(1, 1),
	FirstName NVARCHAR(50) NOT NULL,
	Surname NVARCHAR(50) NOT NULL,
	Discount NUMERIC(18, 4) NOT NULL,

	CONSTRAINT PK_tblCustomer_Id PRIMARY KEY (Id)
);

GO

CREATE TABLE tblCandy
(
	Id INT NOT NULL IDENTITY(1, 1),
	Name NVARCHAR(50) NOT NULL,
	Manufacturer NVARCHAR(50) NOT NULL,
	Price NUMERIC(18, 4) NOT NULL,
	SupplyQty NUMERIC(18, 4) NOT NULL,
	Promotion NUMERIC(18, 4) NOT NULL,
	Deleted BIT NOT NULL,

	CONSTRAINT PF_tblCandy_Id PRIMARY KEY (Id),
	CONSTRAINT AK_tblCandy_Name_Manufacture UNIQUE (Name, Manufacturer),
	CONSTRAINT CHK_tblCandy_Price CHECK (Price > 0)
);

GO

CREATE TABLE tblPurchase
(
	Id INT NOT NULL IDENTITY(1, 1),
	CustomerId INT,
	Cost NUMERIC(18, 4) NOT NULL,
	PurchaseTime DATETIME NOT NULL,	
	UserId INT,

	CONSTRAINT PK_tblPurchase_Id PRIMARY KEY (Id),
	CONSTRAINT FK_tblPurchase_CustomerId_tblCustomer FOREIGN KEY (CustomerId) REFERENCES tblCustomer(Id),
	CONSTRAINT FK_tblPurchase_UserId_tblUser FOREIGN KEY (UserId) REFERENCES tblUser(Id)
)

GO

CREATE TABLE tblPurchaseItem
(
	Id INT NOT NULL IDENTITY(1, 1),
	PurchaseId INT NOT NULL,
	CandyId INT NOT NULL,
	Amount NUMERIC(18, 4) NOT NULL,
	Cost NUMERIC(18, 4) NOT NULL,

	CONSTRAINT PK_tblPurchaseItem_Id PRIMARY KEY (Id),
	CONSTRAINT FK_tblPurchaseItem_CandyId_tblCandy FOREIGN KEY (CandyId) REFERENCES tblCandy(Id),
	CONSTRAINT FK_tblPurchaseItem_PurchaseId_tblPurchase FOREIGN KEY (PurchaseId) REFERENCES tblPurchase(Id),
	CONSTRAINT CHK_tblPurchaseItem_Amount CHECK (Amount > 0)
)

GO