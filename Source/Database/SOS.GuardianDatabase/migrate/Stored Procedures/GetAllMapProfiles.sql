CREATE PROCEDURE [migrate].[GetAllMapProfiles]
	AS
BEGIN
	SELECT prfMap.[StorageProfileID],prfMap.[ProfileID] from [migrate].[ProfileMap] prfMap;
END
