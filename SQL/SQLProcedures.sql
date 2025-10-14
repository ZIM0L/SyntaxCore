
use SyntaxCoreDB
GO

CREATE OR ALTER PROCEDURE DeleteLastAddedUser AS
DELETE FROM
	SyntaxCoreDB.dbo.Users 
WHERE Users.UserId = (
    SELECT TOP 1 
        Users.UserId
    FROM SyntaxCoreDB.dbo.Users as Users
    ORDER BY CreatedAt DESC
);
GO

EXEC DeleteLastAddedUser;
GO

SELECT * FROM sys.procedures;
GO