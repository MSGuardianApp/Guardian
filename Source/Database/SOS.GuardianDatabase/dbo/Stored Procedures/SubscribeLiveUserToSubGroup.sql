--TEST: EXEC [dbo].[SubscribeLiveUserToSubGroup] 17,2,'VR','48060B78-2BB3-4DEC-B5B7-1B901BEFB02F'
CREATE PROCEDURE [dbo].[SubscribeLiveUserToSubGroup]
(@SubGroupId int,@ProfileID bigint,@UserName nvarchar(100),@LiveSessionID nvarchar(50),@ParentGrpID int=NULL)
AS
BEGIN

BEGIN TRY
BEGIN TRANSACTION;

	 DELETE FROM [dbo].[GroupMembership] WHERE ProfileID = @ProfileID AND ParentGrpID IS NOT NULL AND ParentGrpID = @ParentGrpID

	 INSERT INTO [dbo].[GroupMembership]([GroupID],[ProfileID],[UserName],[EnrollmentKeyValue],[IsValidated],[ParentGrpID],[CreatedDate],[CreatedBy],[LastModifiedDate],[LastModifiedBy])
			VALUES(@SubGroupId,@ProfileID,@UserName,NULL,1,@ParentGrpID,GETUTCDATE(),NULL,GETUTCDATE(),NULL)

	 UPDATE dbo.LiveSession SET LastSubGroupID = @SubGroupId where SessionID = @LiveSessionID and ProfileID = @ProfileID 	  
	
	 COMMIT TRANSACTION;
END TRY
BEGIN CATCH
   IF(@@TRANCOUNT>0)
		ROLLBACK TRANSACTION;
   THROW;
END CATCH
END
 

