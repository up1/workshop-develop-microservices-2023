CREATE DATABASE catalog_api;
GO
USE catalog_api;
GO
CREATE TABLE Product (
    ID int, 
    Name nvarchar(max),
    Description nvarchar(max),
    Price int,
    Stock int);
GO
INSERT INTO Product VALUES(1, "Name 01", "Desc 01", 0,0);
INSERT INTO Product VALUES(2, "Name 02", "Desc 02", 0,0);
GO
SELECT * FROM Product;
GO