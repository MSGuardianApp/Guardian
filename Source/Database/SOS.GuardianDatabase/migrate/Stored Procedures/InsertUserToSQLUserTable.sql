CREATE PROCEDURE [migrate].[InsertUserToSQLUserTable](@UserID uniqueidentifier, @Name nvarchar(50),@Email nvarchar(50),@MobileNumber nvarchar(4000),@FBAuthID nvarchar(4000),@FBID nvarchar(4000),@LiveID nvarchar(4000),@CreatedDate DateTime)
AS
BEGIN
	INSERT INTO [dbo].[User]
           (
		   [Name]
           ,[Email]
           ,[MobileNumber]
           ,[FBAuthID]
           ,[FBID]
           ,[LiveID],[CreatedDate],[LastModifiedDate])
     VALUES(@Name,@Email,@MobileNumber,@FBAuthID,@FBID,@LiveID,@CreatedDate,GETUTCDATE());

	 INSERT INTO [migrate].[UserMap]
           ([StorageUserID],[UserID]) Values(@UserID,SCOPE_IDENTITY())
END


GO