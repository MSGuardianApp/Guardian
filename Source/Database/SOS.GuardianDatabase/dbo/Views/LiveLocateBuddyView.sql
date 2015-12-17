CREATE VIEW LiveLocateBuddyView AS
	SELECT B.UserID,B.ProfileID,U.Name,LS.Lat,LS.Long,LS.IsSOS,ISNULL(LS.ClientTimeStamp,0) AS ClientTimeStamp,U.MobileNumber,U.Email
	FROM    dbo.Buddy AS B 
		 INNER JOIN [Profile] P
			ON B.ProfileID = P.ProfileID 
		INNER JOIN dbo.[User] AS U 
			ON P.UserID = U.UserID 
		LEFT OUTER JOIN	dbo.[LiveSession] as LS 
			ON LS.ProfileID = B.ProfileID AND LS.Command != 'STOP' 