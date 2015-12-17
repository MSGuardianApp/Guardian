CREATE PROCEDURE [migrate].[InsertProfileToSQLGroupMembershipTable]( @GroupID int,@ProfileID bigint,@UserName nvarchar(50),@EnrollmentKeyValue nvarchar(50),@IsValidated bit,@CreatedDate DateTime)
AS
BEGIN
IF NOT EXISTS(SELECT [GroupID],[ProfileID] FROM [dbo].[GroupMembership] WHERE [ProfileID] = @ProfileID AND [GroupID] = @GroupID)
    BEGIN
	INSERT INTO [dbo].[GroupMembership]
           ([GroupID]
           ,[ProfileID]
           ,[UserName]
           ,[EnrollmentKeyValue]
           ,[IsValidated],[CreatedDate],[LastModifiedDate])
     VALUES( @GroupID,@ProfileID,@UserName,@EnrollmentKeyValue,@IsValidated,@CreatedDate,GETUTCDATE())
	 END
END


GO