IF NOT EXISTS (SELECT * FROM sys.tables WHERE object_id = OBJECT_ID(N'tbl_users'))
EXEC dbo.sp_executesql @statement = N'
CREATE TABLE tbl_users (
    Id              int identity primary key,
    FirstName       nvarchar(50) not null,
    LastName        nvarchar(50) not null,
    Email           nvarchar(50),
    PhoneNumber     nvarchar(50),
    Password        nvarchar(150)
);'