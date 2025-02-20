IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'tbl_orders'))
EXEC dbo.sp_executesql @statement = N'
CREATE TABLE tbl_orders (
    Id INT IDENTITY(1,1) PRIMARY KEY,  -- Автоінкрементний первинний ключ
	UserId INT NOT NULL FOREIGN KEY REFERENCES tbl_users(Id),
	OrderStatusId INT NOT NULL FOREIGN KEY REFERENCES tbl_order_statuses(Id),
	CreatedDate DATETIME NOT NULL 
);'