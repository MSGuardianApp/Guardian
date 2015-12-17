CREATE PROCEDURE [dbo].[UpdateDispatchInfo]
(@ProfileID bigint,
@SessionID nvarchar(50),
@DispatchInfo nvarchar(250))
AS
BEGIN
	UPDATE [dbo].[LiveSession] SET [DispatchInfo]=@DispatchInfo WHERE [ProfileID]=@ProfileID AND [SessionID]=@SessionID
END
