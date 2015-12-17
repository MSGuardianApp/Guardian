

CREATE PROCEDURE [dbo].[DeleteWhileUnregisterUser](@ProfileID bigint)
AS
BEGIN	
	DELETE FROM [dbo].[LiveLocation] WHERE [ProfileID]=@ProfileID
	DELETE FROM [dbo].[LiveSession] WHERE [ProfileID]=@ProfileID
	DELETE FROM [dbo].[GroupMembership] WHERE [ProfileID]=@ProfileID
	DELETE FROM [dbo].[GroupMarshal] WHERE [ProfileID]=@ProfileID
	DELETE FROM [dbo].[Buddy] WHERE [ProfileID]=@ProfileID
	DELETE FROM [dbo].[Profile] WHERE [ProfileID]=@ProfileID
END



