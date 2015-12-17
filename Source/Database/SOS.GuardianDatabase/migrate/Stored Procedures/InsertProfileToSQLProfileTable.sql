CREATE PROCEDURE [migrate].[InsertProfileToSQLProfileTable](@ProfileID uniqueidentifier,@UserID bigint,@MobileNumber  nvarchar(4000),@RegionCode nvarchar(10),@CanPost bit,@CanSMS bit,@CanEmail bit,@SecurityToken nvarchar(10),@LocationConsent bit,@IsValid bit,@CreatedDate DateTime)
AS
BEGIN
	INSERT INTO [dbo].[Profile]
           (
		   [UserID]
           ,[MobileNumber]
           ,[RegionCode]                      
           ,[CanPost]
           ,[CanSMS]
           ,[CanEmail]
           ,[SecurityToken]
           ,[LocationConsent]
           ,[IsValid],[CreatedDate],[LastModifiedDate])
     VALUES( @UserID,@MobileNumber,@RegionCode,@CanPost,@CanSMS,@CanEmail,@SecurityToken,@LocationConsent,@IsValid,@CreatedDate,GETUTCDATE());
	 INSERT INTO [migrate].[ProfileMap]
           ([StorageProfileID],[ProfileID]) Values(@ProfileID,SCOPE_IDENTITY())

END


GO