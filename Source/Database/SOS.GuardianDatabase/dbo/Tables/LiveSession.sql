CREATE TABLE [dbo].[LiveSession]
(
	[ProfileID] [bigint] NOT NULL,
	[SessionID] [nvarchar](50) NOT NULL,
	[IsSOS] [bit] NOT NULL,
	[Lat] VARCHAR(50) NULL,
	[Long] VARCHAR(50) NULL,
	[LastCapturedDate] [datetime] NULL,
	[ClientTimeStamp] [bigint] NULL,
	[Command] NVARCHAR(20) NULL,
	[ExtendedCommand] NVARCHAR(50) NULL,
	[SessionStartTime] [datetime] NOT NULL,
	[SessionEndTime] [datetime] NULL,
	[InSOS] BIT NULL,
	[ProcessingInstanceId] VARCHAR(100) NULL,
	[ProcessKey] uniqueidentifier NULL,
	[Name] NVARCHAR(250) NULL,
	[MobileNumber] NVARCHAR(2000) NULL,
	[LastSubGroupID] int NULL,
	[LastSMSPostTime] [datetime] NULL,
	[LastEmailPostTime] [datetime] NULL,
	[LastFacebookPostTime] [datetime] NULL,
	[SMSRecipientsList] [nvarchar](2000) NULL,
	[EmailRecipientsList] [nvarchar](1000) NULL,
	[FBGroupID]	[nvarchar](100) NULL,
	[FBAuthID] [nvarchar](4000) NULL,
	[NoOfSMSRecipients] SMALLINT NULL,
	[NoOfEmailRecipients] SMALLINT NULL,
	[NoOfSMSSent] SMALLINT NULL,
	[NoOfEmailsSent] SMALLINT NULL,
	[NoOfFBPostsSent] SMALLINT NULL,
	[TinyUri] [nvarchar](250) NULL,
	[DispatchInfo] NVARCHAR(250) NULL,
	[IsEvidenceAvailable] [bit] NULL,
	[LastModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.LiveSession] PRIMARY KEY CLUSTERED 
(
	[ProfileID] ASC,
	[SessionID] ASC
)
);
GO

CREATE NONCLUSTERED INDEX [IX_LiveSession_Command] ON [dbo].[LiveSession]
	( [Command] ASC )
GO

