CREATE PROCEDURE [dbo].[CreateBuddy]
   (@ProfileID bigint,
	@Name nvarchar(50),
	@Email nvarchar(50),
	@MobileNumber varchar(4000),
	@IsPrimeBuddy bit,
	@State int)
AS
BEGIN
	DECLARE @UserID bigint=0,
		 @InvalidateMobileNo varchar(4000)='EdXVanIL+svZSBakfswv1dlIFqR+zC/V2UgWpH7ML9XZSBakfswv1dlIFqR+zC/VR2Ld+/VFy18='
	
	SELECT @UserID=[UserID] FROM [dbo].[User] WHERE [MobileNumber]=@MobileNumber and [MobileNumber]!=@InvalidateMobileNo
	IF (@UserID=0)		
	BEGIN
		INSERT INTO [dbo].[User]([Name],[Email],[MobileNumber],[CreatedDate],[LastModifiedDate])
					VALUES (@Name,@Email,@MobileNumber,getutcdate(),getutcdate())
		SELECT @UserID=@@IDENTITY
	END
	
	INSERT INTO [dbo].[Buddy]
	([ProfileID],[UserID],[BuddyName],[MobileNumber],[Email],[IsPrimeBuddy],[State],[SubscribtionId],[CreatedDate],[LastModifiedDate]) VALUES 
	(@ProfileID,@UserID,@Name,@MobileNumber,@Email,@IsPrimeBuddy,@State,NEWID(),getutcdate(),getutcdate())
END