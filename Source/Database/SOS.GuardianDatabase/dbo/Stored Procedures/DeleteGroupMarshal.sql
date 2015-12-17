CREATE PROCEDURE [dbo].[DeleteGroupMarshal]
	@GroupID int,
	@ProfileID bigint
AS
BEGIN
	DELETE FROM [dbo].[GroupMarshal] WHERE [GroupID]=@GroupID AND [ProfileID]=@ProfileID
END
