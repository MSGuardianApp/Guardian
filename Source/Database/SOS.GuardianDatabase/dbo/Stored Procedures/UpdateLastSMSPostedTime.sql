CREATE PROCEDURE [dbo].[UpdateLastSMSPostedTime]
	(@ProfileID BIGINT,
	 @SessionID NVARCHAR(50),
	 @SMSPostedTime DATETIME)
AS
BEGIN	
		UPDATE [dbo].[LiveSession] SET [LastSMSPostTime] = @SMSPostedTime
			 WHERE [ProfileID] = @ProfileID AND [SessionID] = @SessionID		
END
