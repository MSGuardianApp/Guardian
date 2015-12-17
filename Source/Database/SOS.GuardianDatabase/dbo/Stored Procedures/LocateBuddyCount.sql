CREATE PROCEDURE [dbo].[LocateBuddyCount](@UserID BIGINT) AS
-- TEST: EXEC [LocateBuddyCount]
BEGIN
	SELECT 
		SUM(CASE WHEN LS.IsSOS =1 THEN 1 END) AS SosCount,
		SUM(CASE WHEN LS.IsSOS =0 THEN 1 END) AS TrackCount     
	FROM dbo.Buddy AS B WITH (NOLOCK)
		 INNER JOIN dbo.[LiveSession] as LS WITH (NOLOCK)
			ON LS.ProfileID = B.ProfileID
	WHERE LS.Command != 'STOP'
		AND B.UserID = @UserID

END
