/*
TEST:

	DECLARE @RoleID			VARCHAR(250)='SOS.Worker.Broadcaster_IN_0',
		@ProcessKey			UNIQUEIDENTIFIER='e6632f33-af66-4e87-9ca7-149e0d3546c8',
		@UpdatedSessionsXML XML ='<ArrayOfLiveSessionLite>
								  <LiveSessionLite>
									<ProfileID>2</ProfileID>
									<SessionID>32bff552-af77-437d-b0c0-d49bffc54fa8</SessionID>
									<LastSMSPostTime>2015-04-01T15:08:03.597</LastSMSPostTime>
									<LastEmailPostTime p3:nil="true" xmlns:p3="http://www.w3.org/2001/XMLSchema-instance" />
									<LastFacebookPostTime p3:nil="true" xmlns:p3="http://www.w3.org/2001/XMLSchema-instance" />
									<SMSRecipientsList>DM+919949091097,+918688393989,+917799012383</SMSRecipientsList>
									<TinyUri>http://tinyurl.com/mb9q5lo</TinyUri>
									<NoOfSMSSent>1</NoOfSMSSent>
									<NoOfEmailsSent>0</NoOfEmailsSent>
									<NoOfFBPostsSent>0</NoOfFBPostsSent>
								  </LiveSessionLite>
								</ArrayOfLiveSessionLite>'

EXEC UpdateNotificationComplete @RoleID, @ProcessKey, @UpdatedSessionsXML

*/
CREATE PROCEDURE [dbo].[UpdateNotificationComplete]
(
	@RoleID				VARCHAR(250),
	@ProcessKey			UNIQUEIDENTIFIER,
	@UpdatedSessionsXML XML
)
AS
BEGIN	
	
	UPDATE [dbo].[LiveSession] 
		SET ProcessKey=NULL,
			ProcessingInstanceId=NULL,
			LastSMSPostTime = ULS.LastSMSPostTime,
			LastEmailPostTime=ULS.LastEmailPostTime,
			LastFacebookPostTime=ULS.LastFacebookPostTime,
			SMSRecipientsList = ULS.SMSRecipientsList,
			TinyUri=ULS.TinyUri,
			NoOfEmailsSent=ULS.NoOfEmailsSent,
			NoOfSMSSent=ULS.NoOfSMSSent,
			NoOfFBPostsSent=ULS.NoOfFBPostsSent
			--,LastModifiedDate=GETUTCDATE()
	FROM LiveSession LS
		INNER JOIN 
		(
			SELECT    
				c.value('(ProfileID/text())[1]', 'BIGINT') ProfileID,
				c.value('(SessionID/text())[1]', 'VARCHAR(50)') SessionID,
				c.value('(LastSMSPostTime/text())[1]', 'DATETIME') LastSMSPostTime,
				c.value('(LastEmailPostTime/text())[1]', 'DATETIME') LastEmailPostTime,
				c.value('(LastFacebookPostTime/text())[1]', 'DATETIME') LastFacebookPostTime,
				c.value('(SMSRecipientsList/text())[1]', 'VARCHAR(4000)') SMSRecipientsList,
				c.value('(TinyUri/text())[1]', 'VARCHAR(250)') TinyUri,
				c.value('(NoOfSMSSent/text())[1]', 'SMALLINT') NoOfSMSSent,
				c.value('(NoOfEmailsSent/text())[1]', 'SMALLINT') NoOfEmailsSent,
				c.value('(NoOfFBPostsSent/text())[1]', 'SMALLINT') NoOfFBPostsSent
			FROM @UpdatedSessionsXML.nodes('//LiveSessionLite') as r(c)
		) ULS
			ON LS.ProfileID = ULS.ProfileID
				AND  LS.SessionID = ULS.SessionID
	WHERE ProcessingInstanceId=@RoleID
		  AND ProcessKey=@ProcessKey;

END



