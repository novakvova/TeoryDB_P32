IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'tbl_products'))
EXEC dbo.sp_executesql @statement = N'
CREATE TABLE tbl_products (
    Id INT IDENTITY(1,1) PRIMARY KEY,  -- Автоінкрементний первинний ключ
	CategoryId INT NOT NULL FOREIGN KEY REFERENCES tbl_categories(Id),
    Name NVARCHAR(100) NOT NULL,  -- Назва
    Description NVARCHAR(4000) NOT NULL, -- Опис
	Price DECIMAL(18,2),
	CreatedDate DATETIME NOT NULL 
);'