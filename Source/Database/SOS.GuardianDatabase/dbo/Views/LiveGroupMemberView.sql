CREATE VIEW [dbo].[LiveGroupMemberView]
AS 
SELECT GM.GroupID, GM.ProfileID, U.Name, LS.IsSOS 
FROM GroupMembership GM
	INNER JOIN LiveSession LS
		ON GM.ProfileID = LS.ProfileID
	INNER JOIN [Profile] P
		ON P.ProfileID = LS.ProfileID
	INNER JOIN [User] U
		ON U.UserID = P.UserID
WHERE LS.Command != 'STOP'
