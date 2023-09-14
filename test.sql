CREATE TABLE [User] (
  [UserID] int PRIMARY KEY IDENTITY(1, 1),
  [FullName] nvarchar(50),
  [Email] nvarchar(50),
  [PhoneNumber] nvarchar(50),
  [UserName] nvarchar(50),
  [Password] nvarchar(50),
  [UserRole] nvarchar(10)
)
GO

CREATE TABLE [Category] (
  [CategoryID] int PRIMARY KEY IDENTITY(1, 1),
  [CategoryName] nvarchar(50)
)
GO

CREATE TABLE [Product] (
  [ProductID] int PRIMARY KEY IDENTITY(1, 1),
  [ProductName] nvarchar(100),
  [Price] float,
  [Image] nvarchar(500),
  [Description] nvarchar(500),
  [CategoryID] int,
  [Author] nvarchar(100)
)
GO

CREATE TABLE [Orders] (
  [OrderID] int PRIMARY KEY IDENTITY(1, 1),
  [OrderDate] date,
  [UserID] int
)
GO

CREATE TABLE [OrderDetails] (
  [ID] int PRIMARY KEY IDENTITY(1, 1),
  [OrderID] int,
  [ProductID] int,
  [UnitPrice] float,
  [Quantity] int
)
GO

CREATE TABLE [Comments] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [Content] nvarchar(500),
  [CreatedAt] datetime,
  [UserId] int,
  [ProductId] int
)
GO

ALTER TABLE [Comments] ADD FOREIGN KEY ([UserId]) REFERENCES [User] ([UserID])
GO

ALTER TABLE [Comments] ADD FOREIGN KEY ([ProductId]) REFERENCES [Product] ([ProductID])
GO

ALTER TABLE [Orders] ADD FOREIGN KEY ([UserID]) REFERENCES [User] ([UserID])
GO

ALTER TABLE [Product] ADD FOREIGN KEY ([CategoryID]) REFERENCES [Category] ([CategoryID])
GO

ALTER TABLE [OrderDetails] ADD FOREIGN KEY ([OrderID]) REFERENCES [Orders] ([OrderID])
GO

ALTER TABLE [OrderDetails] ADD FOREIGN KEY ([ProductID]) REFERENCES [Product] ([ProductID])
GO
