CREATE TABLE [migrate].[ProfileMap](
	[StorageProfileID] [uniqueidentifier] NOT NULL,
	[ProfileID] [bigint] NULL,
	[IsMigrated] [BIT] NULL,
	CONSTRAINT [PK_ProfileMap] PRIMARY KEY CLUSTERED
	([StorageProfileID] ASC)
) 