CREATE PROCEDURE [dbo].[PostLiveLocation]
	(@ProfileID BIGINT,
	 @SessionID NVARCHAR(50),
	 @ClientTimeStamp BIGINT,
	 @ClientDateTime DATETIME,
	 @Lat NVARCHAR(20),
	 @Long NVARCHAR(20),
	 @IsSOS BIT,
	 @Alt VARCHAR(10)=NULL,
	 @Speed INT,
	 @MediaUri VARCHAR(250)=NULL,
	 @ExtendedCommand NVARCHAR(50)=NULL,
	 @Accuracy varchar(50)=NULL)
AS
BEGIN
	
	BEGIN TRY
		DECLARE @UpdatedRecordsCount INT = 0,
		@Command NVARCHAR(50) = 'DEFAULT',
		@IsEvidenceAvailable BIT = 0

		IF (@MediaUri IS NOT NULL AND @MediaUri!='')
			SET @IsEvidenceAvailable = 1;

		--If received latest record, update the latest data 
		IF NOT EXISTS(SELECT (1) FROM [dbo].[LiveSession] WITH (NOLOCK) WHERE [ProfileID]=@ProfileID AND [SessionID]=@SessionID AND [ClientTimeStamp] > @ClientTimeStamp)
		BEGIN
			UPDATE [dbo].[LiveSession]
			   SET [IsSOS] = @IsSOS
				  ,[Lat] = @Lat
				  ,[Long] = @Long
				  ,[LastCapturedDate] = @ClientDateTime
				  ,[ClientTimeStamp] = @ClientTimeStamp
				  ,[InSOS] = @IsSOS | ISNULL([InSOS],0)
				  ,[Command] = @Command
				  ,[ExtendedCommand] = @ExtendedCommand
				  ,[IsEvidenceAvailable] = [IsEvidenceAvailable] | @IsEvidenceAvailable
				  ,[LastModifiedDate] = GETUTCDATE()
			 WHERE ProfileID = @ProfileID AND [SessionID] = @SessionID;

			 SET @UpdatedRecordsCount = @@ROWCOUNT;
		END

		IF(@UpdatedRecordsCount = 0) 
			 BEGIN
				IF NOT EXISTS(SELECT (1) FROM [dbo].[LiveSession] WITH (NOLOCK) WHERE [ProfileID]=@ProfileID AND [SessionID]=@SessionID)
				BEGIN
					DECLARE @SMSRecipientsList NVARCHAR(2000) = NULL
						   ,@EmailRecipientsList NVARCHAR(2000)=NULL
						   ,@FBGroupID NVARCHAR(100)=NULL
						   ,@FBAuthID NVARCHAR(4000)=NULL
						   ,@Name NVARCHAR(250)=NULL
						   ,@MobileNumber NVARCHAR(2000)=NULL
						   ,@NoOfSMSRecipients SMALLINT=0
						   ,@NoOfEmailRecipients SMALLINT=0;

					IF EXISTS (SELECT (1) FROM [Profile] WITH (NOLOCK) WHERE ProfileID = @ProfileID AND CanSMS=1)
					BEGIN
						SELECT @SMSRecipientsList= SUBSTRING(
							(SELECT ','+ MobileNumber FROM Buddy WITH (NOLOCK) WHERE ProfileID=@ProfileID AND ISNULL(MobileNumber,'')!=''
							 FOR XML PATH('')),2,2000) ;
			
						IF @SMSRecipientsList IS NOT NULL
							SELECT @NoOfSMSRecipients = LEN(@SMSRecipientsList) - LEN(REPLACE(@SMSRecipientsList,',','')) + 1;
					END
					IF EXISTS (SELECT (1) FROM [Profile] WITH (NOLOCK) WHERE ProfileID = @ProfileID AND CanEmail=1)
					BEGIN
						SELECT @EmailRecipientsList= SUBSTRING(
							(SELECT ','+Email FROM Buddy WITH (NOLOCK) WHERE ProfileID=@ProfileID AND ISNULL(Email,'')!=''
							 FOR XML PATH('')),2,1000) ;
						
						IF @EmailRecipientsList IS NOT NULL
							SELECT @NoOfEmailRecipients = LEN(@EmailRecipientsList) - LEN(REPLACE(@EmailRecipientsList,',','')) + 1;
					END
				
					--IF @NoOfSMSRecipients>0 AND @NoOfEmailRecipients>0 AND EXISTS (SELECT (1) FROM [Profile] WHERE ProfileID = @ProfileID AND CanPost=1 AND FBGroupID !=NULL AND RTRIM(LTRIM(FBGroupID))!='' )
					--BEGIN
					SELECT  @FBGroupID = P.FBGroupID,
							@FBAuthID= U.FBAuthID,
							@Name = U.Name,
							@MobileNumber=P.MobileNumber
					FROM [Profile] P WITH (NOLOCK)
						INNER JOIN [User] U WITH (NOLOCK)
							ON P.UserID = U.UserID
					WHERE ProfileID=@ProfileID ;
					--END

					--Update other live sessions to stop
					UPDATE [dbo].[LiveSession]
						SET Command = 'STOP',
							ExtendedCommand = 'NEW_SESSION_RECEIVED',
							LastModifiedDate=GETUTCDATE()
					WHERE ProfileID = @ProfileID AND
						  Command != 'STOP'
					
					DELETE  FROM  dbo.LiveLocation 
					WHERE ProfileID = @ProfileID
			
					--Insert a new record
					INSERT INTO [dbo].[LiveSession]
						   ([ProfileID]
						   ,[SessionID]
						   ,[IsSOS]
						   ,[Lat]
						   ,[Long]
						   ,[LastCapturedDate]
						   ,[ClientTimeStamp]
						   ,[Command]
						   ,[ExtendedCommand]
						   ,[SessionStartTime]
						   ,[SessionEndTime]
						   ,[InSOS]
						   ,[ProcessingInstanceId]
						   ,[ProcessKey]
						   ,[Name]
						   ,[MobileNumber]
						   ,[LastSMSPostTime]
						   ,[LastEmailPostTime]
						   ,[LastFacebookPostTime]
						   ,[SMSRecipientsList]
						   ,[EmailRecipientsList]
						   ,[FBGroupID]
						   ,[FBAuthID]
						   ,[NoOfSMSRecipients]
						   ,[NoOfEmailRecipients]
						   ,[NoOfSMSSent]
						   ,[NoOfEmailsSent]
						   ,[NoOfFBPostsSent]
						   ,[IsEvidenceAvailable]
						   ,[LastModifiedDate])
					 VALUES (
							@ProfileID 
						   ,@SessionID
						   ,@IsSOS
						   ,@Lat
						   ,@Long 
						   ,@ClientDateTime
						   ,@ClientTimeStamp 
						   ,@Command
						   ,@ExtendedCommand
						   ,@ClientDateTime --AS SessionStartTime
						   ,NULL --AS SessionEndTime
						   ,@IsSOS
						   ,NULL --AS ProcessInstanceId
						   ,NULL --AS ProcessKey
						   ,@Name
						   ,@MobileNumber
						   ,NULL --AS LastSMSPostTime
						   ,NULL --AS LastEmailPostTime
						   ,NULL --AS LastFacebookPostTime
						   ,@SMSRecipientsList
						   ,@EmailRecipientsList
						   ,@FBGroupID
						   ,@FBAuthID
						   ,@NoOfSMSRecipients
						   ,@NoOfEmailRecipients
						   ,0 --AS NoOfSMSSent
						   ,0 --AS NoOfEmailsSent
						   ,0 --AS NoOfFBPostsSent
						   ,@IsEvidenceAvailable
						   ,GETUTCDATE() --AS LastModifiedDate
						   );
				END
			 END
		--ELSE
		--BEGIN
		----IGNORE THE LOCATION	
		--END
	END TRY
	BEGIN CATCH
		--DO NOTHING
	END CATCH

	BEGIN TRY
		INSERT INTO [dbo].[LiveLocation]
           ([ProfileID]
           ,[SessionID]
		   ,[ClientTimeStamp]
           ,[ClientDateTime]
           ,[IsSOS]
           ,[Lat]
           ,[Long]
           ,[Alt]
           ,[Speed]
           ,[MediaUri]
		   ,[Accuracy]
           ,[CreatedDate]
           ,[CreatedBy])
		VALUES
           (@ProfileID
           ,@SessionID
		   ,@ClientTimeStamp
           ,@ClientDateTime
           ,@IsSOS
           ,@Lat
           ,@Long
           ,@Alt
           ,@Speed
           ,@MediaUri
		   ,@Accuracy
           ,GETUTCDATE()
           ,NULL)
	END TRY
	BEGIN CATCH
		--DO NOTHING
	END CATCH
END

GO