IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'tbl_categories'))
EXEC dbo.sp_executesql @statement = N'
CREATE TABLE tbl_categories (
    Id INT IDENTITY(1,1) PRIMARY KEY,  -- Автоінкрементний первинний ключ
    FName NVARCHAR(100) NOT NULL,  -- Імя категорії
    LName NVARCHAR(500) NOT NULL   -- Опис або додаткові дані
);'