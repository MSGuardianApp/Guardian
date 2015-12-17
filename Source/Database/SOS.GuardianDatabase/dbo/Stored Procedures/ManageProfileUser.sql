CREATE PROCEDURE [dbo].[ManageProfileUser]
   (@Name nvarchar(50),
	@Email nvarchar(50),
	@MobileNumber varchar(4000),
	@FBAuthID varchar(4000),
	@FBID varchar(100),
	@LiveID varchar(100))
AS
BEGIN
	DECLARE @UserID bigint=0,
	@InvalidEmail nvarchar(50)='invalid@invalid.com',
	@InvalidateMobileNo varchar(4000)='EdXVanIL+svZSBakfswv1dlIFqR+zC/V2UgWpH7ML9XZSBakfswv1dlIFqR+zC/VR2Ld+/VFy18=' 
	
	SELECT @UserID=[UserID] FROM [dbo].[User] WHERE [Email]=@Email and [Email]!=@InvalidEmail
	IF (@UserID=0) -- Different Email
	BEGIN
		SELECT @UserID=[UserID] FROM [dbo].[User] WHERE [MobileNumber]=@MobileNumber and [MobileNumber]!=@InvalidateMobileNo
		IF (@UserID>0) --Same Mobile
		BEGIN 
			--Some user with a new email is claiming the MobileNumber. So, existing User and Profile needs to be invalidated.
			UPDATE [dbo].[Profile] SET [IsValid]=0,[MobileNumber]=@InvalidateMobileNo,[LastModifiedDate]=getutcdate() WHERE [UserID]=@UserID
			UPDATE [dbo].[User] SET [MobileNumber]=@InvalidateMobileNo,[LastModifiedDate]=getutcdate() WHERE [UserID]=@UserID			
		END
		INSERT INTO [dbo].[User]
		([Name],[Email],[MobileNumber],[FBAuthID],[FBID],[LiveID],[CreatedDate],[LastModifiedDate])
		VALUES (@Name,@Email,@MobileNumber,@FBAuthID,@FBID,@LiveID,getutcdate(),getutcdate())
		SELECT @UserID=@@IDENTITY
	END
	ELSE
	BEGIN 
	    IF EXISTS (SELECT [UserID] FROM [dbo].[User] WHERE [MobileNumber]=@MobileNumber and [MobileNumber]!=@InvalidateMobileNo)
		BEGIN
			UPDATE [dbo].[User]
			SET [Name]=@Name,[FBAuthID]=@FBAuthID,[FBID]=@FBID,[LastModifiedDate]=getutcdate()
				WHERE [UserID]=@UserID
		END
		ELSE
		BEGIN
			UPDATE [dbo].[Profile] SET [IsValid]=0,[MobileNumber]=@InvalidateMobileNo,[LastModifiedDate]=getutcdate() WHERE [UserID]=@UserID
			UPDATE [dbo].[User]	SET [Email]=@InvalidEmail, LiveID=NULL,[LastModifiedDate]=getutcdate() 
			WHERE [UserID]=@UserID --If actual user is registering after added as buddy, then to avoid duplicate mail -invalidating mail, and also to avoid mistypos by buddyuser
			
			INSERT INTO [dbo].[User]
			([Name],[Email],[MobileNumber],[FBAuthID],[FBID],[LiveID],[CreatedDate],[LastModifiedDate])
			VALUES (@Name,@Email,@MobileNumber,@FBAuthID,@FBID,@LiveID,getutcdate(),getutcdate())
			SELECT @UserID=@@IDENTITY
		END
	END
	SELECT @UserID
END
GO