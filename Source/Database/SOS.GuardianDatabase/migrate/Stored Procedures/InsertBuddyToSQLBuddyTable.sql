CREATE PROCEDURE [migrate].[InsertBuddyToSQLBuddyTable](@ProfileID bigint,@UserID bigint,@BuddyName nvarchar(50),@MobileNumber nvarchar(4000),@Email nvarchar(50),@IsPrimeBuddy bit,@State bit,@CreatedDate DateTime)
AS
BEGIN

IF NOT EXISTS(SELECT [ProfileID],[UserID] FROM [dbo].[Buddy] WHERE [ProfileID] = @ProfileID AND [UserID] = @UserID)
    BEGIN
        INSERT INTO [dbo].[Buddy]
           ([ProfileID],
		   [UserID]
           ,[BuddyName]
           ,[MobileNumber]
           ,[Email]
           ,[IsPrimeBuddy]
           ,[State],[CreatedDate],[LastModifiedDate])
     VALUES(@ProfileID,@UserID,@BuddyName,@MobileNumber,@Email,@IsPrimeBuddy,@State,@CreatedDate,GETUTCDATE())
    END	
END


GO