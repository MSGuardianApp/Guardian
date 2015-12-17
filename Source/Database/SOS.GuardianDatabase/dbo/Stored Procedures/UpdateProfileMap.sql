CREATE PROCEDURE [dbo].[UpdateProfileMap]
(@UserID AS bigint)
AS
BEGIN
	UPDATE [migrate].[ProfileMap] SET [IsMigrated]=1 WHERE [ProfileID] IN (SELECT [ProfileID] FROM [dbo].[Profile] WHERE [UserID]=@UserID) AND ISNULL([IsMigrated],0)!=1
END
GO