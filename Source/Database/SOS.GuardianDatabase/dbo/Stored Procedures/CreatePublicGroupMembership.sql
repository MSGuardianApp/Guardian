CREATE PROCEDURE [dbo].[CreatePublicGroupMembership]
(@GroupID int,@ProfileID bigint,@UserName nvarchar(50))
AS
BEGIN
    IF NOT EXISTS(SELECT * FROM [dbo].[GroupMembership] WHERE [GroupID]=@GroupID AND [ProfileID]=@ProfileID)
	BEGIN
		INSERT INTO [dbo].[GroupMembership]
		([GroupID],[ProfileID],[UserName],[IsValidated],[CreatedDate],[LastModifiedDate])
		VALUES(@GroupID,@ProfileID,@UserName,1,getutcdate(),getutcdate())
	END
END