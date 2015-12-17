
CREATE PROCEDURE [dbo].[ClearInProcessLiveSessions]
(@RoleID VARCHAR(250))
AS
BEGIN	
	
	UPDATE [dbo].[LiveSession] 
		SET ProcessKey = NULL,
			ProcessingInstanceId = NULL
	WHERE Command != 'STOP' AND ProcessingInstanceId IS NOT NULL
		AND ProcessingInstanceId = CASE WHEN @RoleID != '' THEN @RoleID ELSE ProcessingInstanceId END;
	
END



