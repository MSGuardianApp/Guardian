
CREATE PROCEDURE [dbo].[GetSessionsForNotifications]
(
	@RoleID			VARCHAR(250),
	@ProcessKey		UNIQUEIDENTIFIER, 
	@SendSMS		BIT,
	@SMSInterval	INT=15,
	@EmailInterval	INT=15,
	@FBInterval		INT=15
)
AS
BEGIN	
	
	UPDATE TOP (50) [dbo].[LiveSession] 
		SET ProcessKey = @ProcessKey,
			ProcessingInstanceId=@RoleID
			--,LastModifiedDate=GETUTCDATE()
	WHERE   Command != 'STOP' 
			AND IsSOS = 1
			AND ProcessKey IS NULL 
			AND
			  ((@SendSMS=1 AND (DATEADD(minute,@SMSInterval,LastSMSPostTime) <= GETUTCDATE() OR (LastSMSPostTime IS NULL AND SMSRecipientsList IS NOT NULL))) OR
			   DATEADD(minute,@EmailInterval,LastEmailPostTime) <= GETUTCDATE()  OR (LastEmailPostTime IS NULL AND EmailRecipientsList  IS NOT NULL) OR
			   DATEADD(minute,@FBInterval,LastFacebookPostTime) <= GETUTCDATE() OR (LastFacebookPostTime IS NULL AND FBGroupID IS NOT NULL) 
			  );
	
	SELECT * FROM LiveSession WITH (NOLOCK)
	WHERE ProcessingInstanceId=@RoleID
		AND ProcessKey=@ProcessKey;
END



