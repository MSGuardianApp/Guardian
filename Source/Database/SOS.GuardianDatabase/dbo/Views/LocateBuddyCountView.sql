CREATE VIEW LocateBuddyCountView AS
	SELECT B.UserID,
		SUM(CASE WHEN LS.IsSOS =1 THEN 1 ELSE 0 END) AS SosCount,
		SUM(CASE WHEN LS.IsSOS =0 THEN 1 ELSE 0 END) AS TrackCount     
	FROM dbo.Buddy AS B 
		 INNER JOIN dbo.[LiveSession] as LS 
			ON LS.ProfileID = B.ProfileID
	WHERE LS.Command != 'STOP'
	GROUP BY B.UserID;