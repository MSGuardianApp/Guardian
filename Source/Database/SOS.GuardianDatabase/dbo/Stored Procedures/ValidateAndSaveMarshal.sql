CREATE PROCEDURE [dbo].[ValidateAndSaveMarshal]
(@GroupID int,@Email nvarchar(50),@MobileNumber nvarchar(4000),@IsValidated bit)
AS
BEGIN
    DECLARE @UserID bigint,@ProfileID bigint
	SET @UserID=0
	SET @ProfileID=0
	SELECT @UserID=[UserID] FROM [dbo].[User] WITH (NOLOCK) WHERE [Email]=@Email
	IF(@UserID!=0)
	BEGIN
	 SELECT @ProfileID=[ProfileID] FROM [dbo].[Profile] WITH (NOLOCK) WHERE [UserID]=@UserID AND [MobileNumber]=@MobileNumber
		IF(@ProfileID!=0)
		BEGIN
			 IF NOT EXISTS(SELECT * FROM [dbo].[GroupMarshal] WITH (NOLOCK) WHERE [GroupID]=1 AND [ProfileID]=@ProfileID)
			 BEGIN
				INSERT INTO [dbo].[GroupMarshal]([GroupID],[ProfileID],[IsValidated],[CreatedDate],[LastModifiedDate])
					   VALUES(@GroupID,@ProfileID,@IsValidated,getutcdate(),getutcdate())
				SELECT @ProfileID as ProfileID,@UserID as UserID,1 as Code,'Marshal Saved' as MessageInfo
			 END
			 ELSE
				SELECT @ProfileID as ProfileID,@UserID as UserID,0 as Code,'Marshal already present for the group' as MessageInfo
		END
		ELSE
			SELECT @ProfileID as ProfileID,@UserID as UserID,0 as Code,'User profile not present with given phone number' as MessageInfo
	END
	ELSE
		SELECT @ProfileID as ProfileID,@UserID as UserID,0 as Code,'User Not Present with given LiveMail' as MessageInfo
END