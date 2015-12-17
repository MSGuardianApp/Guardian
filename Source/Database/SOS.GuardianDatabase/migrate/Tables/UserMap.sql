Create TABLE [migrate].[UserMap](
	[StorageUserID] [uniqueidentifier] Not Null,
	[UserID] [bigint] NULL,
	CONSTRAINT [PK_UserMap] PRIMARY KEY CLUSTERED
	([StorageUserID] ASC)
	)