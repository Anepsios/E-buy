CREATE TRIGGER DeletedUsers
ON dbo.AspNetUsers
AFTER DELETE 
AS
    INSERT INTO dbo.OldUsers(UserName, FirstName, LastName, Email, DeletedOn)
        SELECT d.UserName, d.FirstName, d.LastName,d.Email,GETDATE()
        FROM Deleted d