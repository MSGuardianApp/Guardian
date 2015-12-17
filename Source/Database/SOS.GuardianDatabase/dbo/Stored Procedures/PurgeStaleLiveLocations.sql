

CREATE PROCEDURE [dbo].[PurgeStaleLiveLocations]
AS
BEGIN	
	
	DELETE FROM [dbo].[LiveLocation] 
	WHERE CreatedDate < DATEADD(hour, -4, GETUTCDATE()) ;
	
END



