CREATE PROCEDURE [dbo].[StopSOSOnly](
	@ProfileID AS BIGINT,
	@SessionID AS NVARCHAR(50)='0'
)
AS
   	UPDATE dbo.LiveSession 
		SET IsSOS=0,
			LastModifiedDate=GETUTCDATE()
	WHERE Command != 'STOP' AND 
		  ((@SessionID = '0' AND ProfileID = @ProfileID) OR
		   (@SessionID !='0' AND ProfileID = @ProfileID AND SessionID = @SessionID) 
		  )
	
GO