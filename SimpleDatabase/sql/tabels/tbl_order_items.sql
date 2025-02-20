IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'tbl_order_items'))
EXEC dbo.sp_executesql @statement = N'
CREATE TABLE tbl_order_items (
    Id INT IDENTITY(1,1) PRIMARY KEY,  -- Автоінкрементний первинний ключ
	OrderId INT NOT NULL FOREIGN KEY REFERENCES tbl_orders(Id),
	ProductId INT NOT NULL FOREIGN KEY REFERENCES tbl_products(Id),
	Count INT NOT NULL,
	CurrentPrice DECIMAL(18,2) 
);'