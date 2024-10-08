USE [TestDB]
GO
/****** Object:  UserDefinedFunction [dbo].[GetAverageSalesAmount]    Script Date: 10.09.2024 20:02:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetAverageSalesAmount]()
RETURNS DECIMAL(10, 2)
AS
BEGIN
    DECLARE @AverageSales DECIMAL(10, 2);

    SELECT @AverageSales = AVG(SalesAmount)
    FROM SalesData;

    RETURN @AverageSales;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[GetTotalSalesAmount]    Script Date: 10.09.2024 20:02:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetTotalSalesAmount]()
RETURNS DECIMAL(10, 2)
AS
BEGIN
    DECLARE @TotalSales DECIMAL(10, 2);

    SELECT @TotalSales = SUM(SalesAmount) 
    FROM SalesData;

    RETURN @TotalSales;
END;
GO
/****** Object:  Table [dbo].[SalesData]    Script Date: 10.09.2024 20:02:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesData](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](255) NULL,
	[SalesAmount] [decimal](10, 2) NULL,
	[SaleDate] [date] NULL,
	[ProductId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[SalesDataView]    Script Date: 10.09.2024 20:02:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SalesDataView] AS
SELECT ProductName, SUM(SalesAmount) AS TotalSales, SaleDate
FROM SalesData
GROUP BY ProductName, SaleDate;
GO
/****** Object:  View [dbo].[TopSellingProductsView]    Script Date: 10.09.2024 20:02:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[TopSellingProductsView] AS
SELECT 
    ProductName,
    SUM(SalesAmount) AS TotalSales
FROM 
    SalesData
GROUP BY 
    ProductName;
GO
/****** Object:  View [dbo].[SalesDetailsView]    Script Date: 10.09.2024 20:02:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SalesDetailsView] AS
SELECT 
    ProductName,
    SalesAmount,
    SaleDate
FROM 
    SalesData;
GO
/****** Object:  UserDefinedFunction [dbo].[GetSalesByProduct]    Script Date: 10.09.2024 20:02:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetSalesByProduct](@ProductId INT)
RETURNS TABLE
AS
RETURN 
(
    SELECT ProductName, SalesAmount, SaleDate
    FROM SalesData
    WHERE ProductId = @ProductId
);
GO
/****** Object:  UserDefinedFunction [dbo].[GetSalesWithinDateRange]    Script Date: 10.09.2024 20:02:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetSalesWithinDateRange](@StartDate DATE, @EndDate DATE)
RETURNS TABLE
AS
RETURN 
(
    SELECT ProductName, SalesAmount, SaleDate
    FROM SalesData
    WHERE SaleDate BETWEEN @StartDate AND @EndDate
);
GO
SET IDENTITY_INSERT [dbo].[SalesData] ON 

INSERT [dbo].[SalesData] ([Id], [ProductName], [SalesAmount], [SaleDate], [ProductId]) VALUES (1, N'Product A', CAST(100.00 AS Decimal(10, 2)), CAST(N'2024-09-01' AS Date), 101)
INSERT [dbo].[SalesData] ([Id], [ProductName], [SalesAmount], [SaleDate], [ProductId]) VALUES (2, N'Product B', CAST(200.00 AS Decimal(10, 2)), CAST(N'2024-09-02' AS Date), 101)
INSERT [dbo].[SalesData] ([Id], [ProductName], [SalesAmount], [SaleDate], [ProductId]) VALUES (3, N'Product A', CAST(150.00 AS Decimal(10, 2)), CAST(N'2024-09-03' AS Date), 102)
INSERT [dbo].[SalesData] ([Id], [ProductName], [SalesAmount], [SaleDate], [ProductId]) VALUES (4, N'Product C', CAST(300.00 AS Decimal(10, 2)), CAST(N'2024-09-04' AS Date), 104)
INSERT [dbo].[SalesData] ([Id], [ProductName], [SalesAmount], [SaleDate], [ProductId]) VALUES (5, N'Product B', CAST(250.00 AS Decimal(10, 2)), CAST(N'2024-09-05' AS Date), 103)
SET IDENTITY_INSERT [dbo].[SalesData] OFF
GO
/****** Object:  StoredProcedure [dbo].[GetAllSalesData]    Script Date: 10.09.2024 20:02:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllSalesData]
AS
BEGIN
    SELECT ProductName, SalesAmount, SaleDate
    FROM SalesData;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetSalesAboveAmount]    Script Date: 10.09.2024 20:02:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetSalesAboveAmount]
    @Amount DECIMAL(10, 2)
AS
BEGIN
    SELECT ProductName, SalesAmount, SaleDate
    FROM SalesData
    WHERE SalesAmount > @Amount;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetSalesByProductId]    Script Date: 10.09.2024 20:02:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetSalesByProductId]
    @ProductId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ProductName, SalesAmount, SaleDate
    FROM SalesData
    WHERE ProductId = @ProductId;

END;
GO
/****** Object:  StoredProcedure [dbo].[GetSalesCountByDateRange]    Script Date: 10.09.2024 20:02:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetSalesCountByDateRange]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT COUNT(*) AS SalesCount
    FROM SalesData
    WHERE SaleDate BETWEEN @StartDate AND @EndDate;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetSalesDataByDateRange]    Script Date: 10.09.2024 20:02:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetSalesDataByDateRange]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT ProductName, SUM(SalesAmount) AS TotalSales, SaleDate
    FROM SalesData
    WHERE SaleDate BETWEEN @StartDate AND @EndDate
    GROUP BY ProductName, SaleDate;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetTopSellingProduct]    Script Date: 10.09.2024 20:02:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTopSellingProduct]
AS
BEGIN
    SET NOCOUNT ON;

    -- En çok satış yapılan ürünü getiriyoruz
    SELECT TOP 1 ProductId, ProductName, SUM(SalesAmount) AS TotalSales
    FROM SalesData
    GROUP BY ProductId, ProductName
    ORDER BY TotalSales DESC;
END;
GO
