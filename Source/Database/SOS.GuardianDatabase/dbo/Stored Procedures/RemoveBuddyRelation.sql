CREATE PROCEDURE [dbo].[RemoveBuddyRelation](@ProfileID bigint,@BuddyUserID bigint)
AS
BEGIN
	DELETE FROM [dbo].[Buddy] WHERE [ProfileID]=@ProfileID AND [UserID]=@BuddyUserID
END