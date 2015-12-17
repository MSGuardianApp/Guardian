CREATE TABLE [dbo].[User] (
    [UserID]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (50)   NULL,
    [Email]            NVARCHAR (50)   NULL,
    [MobileNumber]     VARCHAR (4000) NULL,
    [FBAuthID]         VARCHAR (4000) NULL,
    [FBID]             VARCHAR (100) NULL,
    [LiveID]           VARCHAR (100) NULL,
    [CreatedDate]      DATETIME        NULL,
    [CreatedBy]        NVARCHAR (50)   NULL,
    [LastModifiedDate] DATETIME        NULL,
    [LastModifiedBy]   NVARCHAR (50)   NULL,
    CONSTRAINT [PK_dbo.User] PRIMARY KEY CLUSTERED ([UserID] ASC)
);
GO

CREATE NONCLUSTERED INDEX [IX_User_LiveID] ON [dbo].[User]
	( [LiveID] ASC )
GO
