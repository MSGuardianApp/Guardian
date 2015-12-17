CREATE VIEW LocateBuddyView AS
	SELECT B.UserID,B.ProfileID,U.Name,U.MobileNumber,U.Email
	FROM    dbo.Buddy AS B 
		 INNER JOIN [Profile] P
			ON B.ProfileID = P.ProfileID 
		INNER JOIN dbo.[User] AS U 
			ON P.UserID = U.UserID