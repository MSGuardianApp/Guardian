CREATE PROCEDURE [dbo].[UpdatePrimeBuddy]
(@BuddyID AS bigint,
@ProfileID AS bigint)
AS
BEGIN
	BEGIN TRY
	BEGIN TRAN
		UPDATE [dbo].[Buddy] SET [IsPrimeBuddy]=0 WHERE [ProfileID]=@ProfileID AND [IsPrimeBuddy]=1
		UPDATE [dbo].[Buddy] SET [IsPrimeBuddy]=1 WHERE [BuddyID]=@BuddyID AND [ProfileID]=@ProfileID
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT>0
		ROLLBACK;
	END CATCH

RETURN; 
END