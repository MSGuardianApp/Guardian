CREATE PROCEDURE [dbo].[RemoveLiveLocation](
	@ProfileID AS BIGINT,
	@SessionID AS NVARCHAR(50) = NULL,
	@ClientTimeStamp AS BIGINT
)
AS
	BEGIN TRY
		DELETE  FROM  dbo.LiveLocation 
		WHERE ProfileID = @ProfileID
			--(ProfileID = @ProfileID AND @SessionID IS NOT NULL AND SessionID = @SessionID) 
			--OR (ProfileID = @ProfileID AND @SessionID IS NULL) 
			--OR (@SessionID IS NOT NULL AND SessionID = @SessionID)
	END TRY
	BEGIN CATCH
	END CATCH

	BEGIN TRY
		UPDATE dbo.LiveSession
			SET Command			= 'STOP'
			, SessionEndTime	= CASE WHEN @ClientTimeStamp = 0 THEN LastCapturedDate ELSE dbo.TicksToDateTime(@ClientTimeStamp) END
			, ClientTimeStamp	= CASE WHEN @SessionID IS NULL OR @ClientTimeStamp = 0 THEN ClientTimeStamp ELSE @ClientTimeStamp END
			, LastModifiedDate	= GETUTCDATE()
		WHERE
			(ProfileID = @ProfileID AND @SessionID IS NOT NULL AND SessionID = @SessionID) 
			OR (ProfileID = @ProfileID AND @SessionID IS NULL)
			OR (@SessionID IS NOT NULL AND SessionID = @SessionID)
	END TRY
	BEGIN CATCH
	END CATCH

	BEGIN TRY
		DELETE  FROM  dbo.GroupMembership 
		WHERE ParentGrpID IS NOT NULL AND
			  ProfileID = @ProfileID 
	END TRY
	BEGIN CATCH
	END CATCH
GO