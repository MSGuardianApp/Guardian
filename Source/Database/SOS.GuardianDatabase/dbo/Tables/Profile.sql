CREATE TABLE [dbo].[Profile] (
    [ProfileID]        BIGINT          IDENTITY (1, 1) NOT NULL,
    [UserID]           BIGINT          NOT NULL,
    [MobileNumber]     NVARCHAR (4000) NULL,
    [RegionCode]       NVARCHAR (10)   NULL,
    [DeviceID]         NVARCHAR (100)  NULL,
	[DeviceType]	   INT			   NULL,
    [FBGroup]          NVARCHAR (100)  NULL,
    [FBGroupID]        NVARCHAR (100)  NULL,
    [CanPost]          BIT             NOT NULL,
    [CanSMS]           BIT             NOT NULL,
    [CanEmail]         BIT             NOT NULL,
    [SecurityToken]    NVARCHAR (10)   NULL,
	[EnterpriseSecurityToken] NVARCHAR (10) NULL,
	[EnterpriseEmailID] NVARCHAR (50)  NULL,
    [LocationConsent]  BIT             NOT NULL,
    [IsValid]          BIT             NOT NULL,
	[NotificationUri]  NVARCHAR (2000) NULL,
    [CreatedDate]      DATETIME        NULL,
    [CreatedBy]        NVARCHAR (50)   NULL,
    [LastModifiedDate] DATETIME        NULL,
    [LastModifiedBy]   NVARCHAR (50)   NULL,
    CONSTRAINT [PK_dbo.Profile] PRIMARY KEY CLUSTERED ([ProfileID] ASC),
    CONSTRAINT [FK_dbo.Profile_dbo.User_UserID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([UserID])
);


GO
CREATE NONCLUSTERED INDEX [IX_UserID]
    ON [dbo].[Profile]([UserID] ASC);

