CREATE TABLE [dbo].[Buddy] (
    [BuddyID]          BIGINT          IDENTITY (1, 1) NOT NULL,
    [ProfileID]        BIGINT          NOT NULL,
    [UserID]           BIGINT          NOT NULL,
    [BuddyName]        NVARCHAR (50)   NULL,
    [MobileNumber]     NVARCHAR (4000) NULL,
    [Email]            NVARCHAR (50)   NULL,
    [IsPrimeBuddy]     BIT             NOT NULL,
    [State]            INT             NOT NULL,
	[SubscribtionId]   UniqueIdentifier NULL,
    [CreatedDate]      DATETIME        NULL,
    [CreatedBy]        NVARCHAR (50)   NULL,
    [LastModifiedDate] DATETIME        NULL,
    [LastModifiedBy]   NVARCHAR (50)   NULL,
    CONSTRAINT [PK_dbo.Buddy] PRIMARY KEY CLUSTERED ([BuddyID] ASC),
    CONSTRAINT [FK_dbo.Buddy_dbo.Profile_ProfileID] FOREIGN KEY ([ProfileID]) REFERENCES [dbo].[Profile] ([ProfileID]),
    CONSTRAINT [FK_dbo.Buddy_dbo.User_UserID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
);


GO
CREATE NONCLUSTERED INDEX [IX_ProfileID]
    ON [dbo].[Buddy]([ProfileID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserID]
    ON [dbo].[Buddy]([UserID] ASC);

