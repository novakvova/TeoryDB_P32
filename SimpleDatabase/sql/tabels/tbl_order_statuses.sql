IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'tbl_order_statuses'))
EXEC dbo.sp_executesql @statement = N'
CREATE TABLE tbl_order_statuses (
    Id INT IDENTITY(1,1) PRIMARY KEY,  -- Автоінкрементний первинний ключ
    Name NVARCHAR(100) NOT NULL,  --Назва статуса
);'