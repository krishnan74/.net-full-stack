CREATE TABLE Categories (
  ID INT PRIMARY KEY,
  Name VARCHAR(100) NOT NULL,
  Status VARCHAR(20)
);

CREATE TABLE Country (
  ID INT PRIMARY KEY,
  Name VARCHAR(100) NOT NULL
);

CREATE TABLE State (
  ID INT PRIMARY KEY,
  Name VARCHAR(100) NOT NULL,
  Country_ID INT,
  FOREIGN KEY (Country_ID) REFERENCES Country(ID)
);

CREATE TABLE City (
  ID INT PRIMARY KEY,
  Name VARCHAR(100) NOT NULL,
  State_ID INT,
  FOREIGN KEY (State_ID) REFERENCES State(ID)
);

CREATE TABLE Area (
  Zipcode VARCHAR(10) PRIMARY KEY,
  Name VARCHAR(100) NOT NULL,
  City_ID INT,
  FOREIGN KEY (City_ID) REFERENCES City(ID)
);

CREATE TABLE Address (
  ID INT PRIMARY KEY,
  Door_Number VARCHAR(20),
  AddressLine1 VARCHAR(255),
  Zipcode VARCHAR(10),
  FOREIGN KEY (Zipcode) REFERENCES Area(Zipcode)
);

CREATE TABLE Supplier (
  ID INT PRIMARY KEY,
  Name VARCHAR(100) NOT NULL,
  Contact_Person VARCHAR(100),
  Phone VARCHAR(20),
  Email VARCHAR(100),
  Address_ID INT,
  Status VARCHAR(20),
  FOREIGN KEY (Address_ID) REFERENCES Address(ID)
);

CREATE TABLE Product (
  ID INT PRIMARY KEY,
  Name VARCHAR(100) NOT NULL,
  Unit_Price DECIMAL(10, 2) NOT NULL CHECK (Unit_Price >= 0),
  Quantity INT NOT NULL CHECK (Quantity > 0),
  Description TEXT,
  Image VARCHAR(255)
);

CREATE TABLE Product_Supplier (
  Transaction_ID INT PRIMARY KEY,
  Product_ID INT,
  Supplier_ID INT,
  Date_Of_Supply DATE,
  Quantity INT NOT NULL CHECK (Quantity > 0),
  FOREIGN KEY (Product_ID) REFERENCES Product(ID),
  FOREIGN KEY (Supplier_ID) REFERENCES Supplier(ID)
);

CREATE TABLE Customer (
  ID INT PRIMARY KEY,
  Name VARCHAR(100) NOT NULL,
  Phone VARCHAR(20),
  Age INT,
  Address_ID INT,
  FOREIGN KEY (Address_ID) REFERENCES Address(ID)
);

CREATE TABLE Orders (
  Order_Number INT PRIMARY KEY,
  Customer_ID INT,
  Date_Of_Order DATE NOT NULL CHECK (Date_Of_Order > GETDATE()),
  Amount DECIMAL(10, 2),
  Order_Status VARCHAR(50),
  FOREIGN KEY (Customer_ID) REFERENCES Customer(ID)
);

CREATE TABLE Order_Details (
  ID INT PRIMARY KEY,
  Order_Number INT,
  Product_ID INT,
  Quantity INT NOT NULL CHECK (Quantity > 0),
  Unit_Price DECIMAL(10, 2) NOT NULL CHECK (Unit_Price >= 0),
  FOREIGN KEY (Order_Number) REFERENCES Orders(Order_Number),
  FOREIGN KEY (Product_ID) REFERENCES Product(ID)
);