USE catalog-api;
GO
CREATE TABLE Products (
    ID int, 
    Name nvarchar(max),
    Description nvarchar(max),
    Price int,
    Stock int);
GO
INSERT INTO PRODUCTS VALUES(1, "Name 01", "Desc 01", 0,0);
GO
INSERT INTO PRODUCTS VALUES(2, "Name 02", "Desc 02", 0,0);
GO