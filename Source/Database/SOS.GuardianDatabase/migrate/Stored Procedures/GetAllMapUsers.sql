CREATE PROCEDURE [migrate].[GetAllMapUsers]
	
AS
BEGIN
	SELECT usrMap.[StorageUserID],usrMap.[UserID] from [migrate].[UserMap] usrMap;
END

