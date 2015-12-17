CREATE PROCEDURE [migrate].[InsertProfileToSQLGroupMarshalTable]( @GroupID int,@ProfileID bigint,@IsValidated bit,@CreatedDate DateTime)
AS
BEGIN
IF NOT EXISTS(SELECT [GroupID],[ProfileID] FROM [dbo].[GroupMarshal] WHERE [ProfileID] = @ProfileID AND [GroupID] = @GroupID)
    BEGIN
	INSERT INTO [dbo].[GroupMarshal]
           ([GroupID]
           ,[ProfileID]
           ,[IsValidated],[CreatedDate],[LastModifiedDate])
     VALUES(  @GroupID,@ProfileID,@IsValidated,@CreatedDate,GETUTCDATE())
	 END
END


GO