CREATE PROCEDURE [migrate].[InsertGeoLocationToSQLLiveLocationTable](@ProfileID bigint,@SessionID nvarchar(20),@ClientDateTime bigint ,@IsSOS bit,@Lat  nvarchar(20),@Long  nvarchar(20),@Alt  nvarchar(10),@Speed bit,@ClientTimeStamp datetime,@MediaUri  nvarchar(150))
AS 
BEGIN
	INSERT INTO [dbo].[LiveLocation]
           ([ProfileID]
           ,[SessionID]
           ,[ClientDateTime]
           ,[IsSOS]
           ,[Lat]
           ,[Long]
           ,[Alt]
           ,[Speed]
           ,[ClientTimeStamp]
           ,[MediaUri])
     VALUES( @ProfileID,@SessionID,@ClientTimeStamp,@IsSOS,@Lat,@Long,@Alt,@Speed,@ClientDateTime,@MediaUri)
END
