CREATE PROCEDURE [dbo].[DeleteGroupMembership]
	@GroupID int,
	@ProfileID bigint
AS
BEGIN
	DELETE FROM [dbo].[GroupMembership] WHERE [GroupID]=@GroupID AND [ProfileID]=@ProfileID
END
