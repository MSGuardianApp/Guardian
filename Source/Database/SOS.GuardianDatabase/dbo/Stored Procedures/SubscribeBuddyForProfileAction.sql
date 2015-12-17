CREATE PROCEDURE [dbo].[SubscribeBuddyForProfileAction]
(@ProfileID bigint,@UserID bigint,@State int,@SubscribtionID varchar(50))
AS
BEGIN
	IF EXISTS(SELECT * FROM [dbo].[Buddy] WHERE [SubscribtionId]=Convert(UniqueIdentifier,@SubscribtionID))
	BEGIN
		UPDATE [dbo].[Buddy] SET [State]=@State WHERE [ProfileID]=@ProfileID AND [UserID]=@UserID AND [SubscribtionId]=Convert(UniqueIdentifier,@SubscribtionID)

		SELECT usr.[Name],prf.[MobileNumber] FROM [dbo].[User] usr WITH (NOLOCK) JOIN [dbo].[Profile] prf WITH (NOLOCK)
		ON usr.[UserID]=prf.[UserID] WHERE usr.UserID = @UserID AND prf.ProfileID = @ProfileID
	END
END



