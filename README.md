# SportsShop
Online Sport Shop

---------------
ToDo-List
---------------

DataBase Tools
	- MSSQL Server
	- Tables
	- Normalizations
Backend Tools
	- DotNet Core
	- Asp.net Core WebAPI
	- ExceptionHandling
	- MSUnitTest
	- EF
	- Log4Net
	- Swagger Integrations
	
ForntendTools
	- Angular
	- Jasmine and Karma UnitTest

Other Tools
	- Azure Cloud
	- Git/TFS Source Version Control


USE [master]
GO
/****** Object:  Database [ShopDB]    Script Date: 25-09-2022 18:44:35 ******/
CREATE DATABASE [ShopDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ShopDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\ShopDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ShopDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\ShopDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [ShopDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ShopDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ShopDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ShopDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ShopDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ShopDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ShopDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [ShopDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ShopDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ShopDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ShopDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ShopDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ShopDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ShopDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ShopDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ShopDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ShopDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ShopDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ShopDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ShopDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ShopDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ShopDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ShopDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ShopDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ShopDB] SET RECOVERY FULL 
GO
ALTER DATABASE [ShopDB] SET  MULTI_USER 
GO
ALTER DATABASE [ShopDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ShopDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ShopDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ShopDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ShopDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ShopDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'ShopDB', N'ON'
GO
ALTER DATABASE [ShopDB] SET QUERY_STORE = OFF
GO
USE [ShopDB]
GO
/****** Object:  Table [dbo].[Tbl_Customer]    Script Date: 25-09-2022 18:44:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Customer](
	[CustomerId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerName] [nvarchar](50) NULL,
	[ContactNumber] [nvarchar](50) NULL,
	[CustomerEmailId] [nvarchar](50) NULL,
	[CustomerAddress] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_OrderedItems]    Script Date: 25-09-2022 18:44:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_OrderedItems](
	[OrderedItemsId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderedItemsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Orders]    Script Date: 25-09-2022 18:44:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Orders](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[OrderAddress] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Product]    Script Date: 25-09-2022 18:44:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Product](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](100) NULL,
	[ProductPrice] [decimal](18, 0) NULL,
	[ProductColor] [nvarchar](50) NULL,
	[ProductSize] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Tbl_Customer] ON 
GO
INSERT [dbo].[Tbl_Customer] ([CustomerId], [CustomerName], [ContactNumber], [CustomerEmailId], [CustomerAddress]) VALUES (1, N'Ramya', N'854697822', N'ramya@gmail.com', N'Old Hyth road, Road 2,HYD')
GO
INSERT [dbo].[Tbl_Customer] ([CustomerId], [CustomerName], [ContactNumber], [CustomerEmailId], [CustomerAddress]) VALUES (2, N'Soumya', N'789654123', N'soumya@gmail.com', N'Hythnagar, Road 2,RR Distirct')
GO
INSERT [dbo].[Tbl_Customer] ([CustomerId], [CustomerName], [ContactNumber], [CustomerEmailId], [CustomerAddress]) VALUES (5, N'Eshwar', N'8976543299', N'eshwar@gmail.com', N'vanasthalipuram')
GO
SET IDENTITY_INSERT [dbo].[Tbl_Customer] OFF
GO
SET IDENTITY_INSERT [dbo].[Tbl_OrderedItems] ON 
GO
INSERT [dbo].[Tbl_OrderedItems] ([OrderedItemsId], [OrderId], [ProductId]) VALUES (1, 1, 3)
GO
INSERT [dbo].[Tbl_OrderedItems] ([OrderedItemsId], [OrderId], [ProductId]) VALUES (2, 1, 4)
GO
INSERT [dbo].[Tbl_OrderedItems] ([OrderedItemsId], [OrderId], [ProductId]) VALUES (3, 2, 2)
GO
INSERT [dbo].[Tbl_OrderedItems] ([OrderedItemsId], [OrderId], [ProductId]) VALUES (4, 2, 1)
GO
INSERT [dbo].[Tbl_OrderedItems] ([OrderedItemsId], [OrderId], [ProductId]) VALUES (5, 3, 4)
GO
INSERT [dbo].[Tbl_OrderedItems] ([OrderedItemsId], [OrderId], [ProductId]) VALUES (6, 5, 3)
GO
INSERT [dbo].[Tbl_OrderedItems] ([OrderedItemsId], [OrderId], [ProductId]) VALUES (7, 5, 4)
GO
SET IDENTITY_INSERT [dbo].[Tbl_OrderedItems] OFF
GO
SET IDENTITY_INSERT [dbo].[Tbl_Orders] ON 
GO
INSERT [dbo].[Tbl_Orders] ([OrderId], [CustomerId], [OrderAddress]) VALUES (1, 2, N'Kuntlor,Hyt,Hyd-501505')
GO
INSERT [dbo].[Tbl_Orders] ([OrderId], [CustomerId], [OrderAddress]) VALUES (2, 2, N'VasanthNagar,KuntlRoad,HYT')
GO
INSERT [dbo].[Tbl_Orders] ([OrderId], [CustomerId], [OrderAddress]) VALUES (3, 2, N'saroornagar')
GO
INSERT [dbo].[Tbl_Orders] ([OrderId], [CustomerId], [OrderAddress]) VALUES (4, 1, N'vanasthalipuram')
GO
INSERT [dbo].[Tbl_Orders] ([OrderId], [CustomerId], [OrderAddress]) VALUES (5, 1, N'vanasthalipuram')
GO
SET IDENTITY_INSERT [dbo].[Tbl_Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[Tbl_Product] ON 
GO
INSERT [dbo].[Tbl_Product] ([ProductId], [ProductName], [ProductPrice], [ProductColor], [ProductSize]) VALUES (1, N'Criket Bat', CAST(2999 AS Decimal(18, 0)), N'Green', N'3 inch X 4.25 inch X 1.5inch')
GO
INSERT [dbo].[Tbl_Product] ([ProductId], [ProductName], [ProductPrice], [ProductColor], [ProductSize]) VALUES (2, N'Criket Ball', CAST(1599 AS Decimal(18, 0)), N'Red', N'Medium')
GO
INSERT [dbo].[Tbl_Product] ([ProductId], [ProductName], [ProductPrice], [ProductColor], [ProductSize]) VALUES (3, N'Badminton Racquet', CAST(1500 AS Decimal(18, 0)), N'Black', N'26 inches')
GO
INSERT [dbo].[Tbl_Product] ([ProductId], [ProductName], [ProductPrice], [ProductColor], [ProductSize]) VALUES (4, N'Badminton Shuttel', CAST(250 AS Decimal(18, 0)), N'White', N'5 inches')
GO
INSERT [dbo].[Tbl_Product] ([ProductId], [ProductName], [ProductPrice], [ProductColor], [ProductSize]) VALUES (5, N'Sports Shoes', CAST(3999 AS Decimal(18, 0)), N'white', N'8 inches')
GO
SET IDENTITY_INSERT [dbo].[Tbl_Product] OFF
GO
ALTER TABLE [dbo].[Tbl_OrderedItems]  WITH CHECK ADD FOREIGN KEY([OrderId])
REFERENCES [dbo].[Tbl_Orders] ([OrderId])
GO
ALTER TABLE [dbo].[Tbl_OrderedItems]  WITH CHECK ADD FOREIGN KEY([ProductId])
REFERENCES [dbo].[Tbl_Product] ([ProductId])
GO
ALTER TABLE [dbo].[Tbl_Orders]  WITH CHECK ADD FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Tbl_Customer] ([CustomerId])
GO
USE [master]
GO
ALTER DATABASE [ShopDB] SET  READ_WRITE 
GO
