CREATE PROCEDURE [dbo].[UpdateGroupMembership]
(@GroupID AS int,
 @ProfileID AS bigint)
AS
BEGIN
	UPDATE [dbo].[GroupMembership] 
		SET [IsValidated]=1,
			[LastModifiedDate]=getutcdate()
	WHERE [GroupID]=@GroupID AND [ProfileID]=@ProfileID;
	
	RETURN; 
END
