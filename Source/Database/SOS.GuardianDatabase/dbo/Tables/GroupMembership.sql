CREATE TABLE [dbo].[GroupMembership] (
	[GroupID]            INT           NOT NULL,
	[ProfileID]          BIGINT        NOT NULL,
	[UserName]           NVARCHAR (50) NULL,
	[EnrollmentKeyValue] NVARCHAR (50) NULL,
	[IsValidated]        BIT           NOT NULL,
	[ParentGrpID]		 INT		   NULL,
	[CreatedDate]        DATETIME      NULL,
	[CreatedBy]          NVARCHAR (50) NULL,
	[LastModifiedDate]   DATETIME      NULL,
	[LastModifiedBy]     NVARCHAR (50) NULL,	
	CONSTRAINT [PK_dbo.GroupMembership] PRIMARY KEY CLUSTERED ([GroupID] ASC, [ProfileID] ASC),
	CONSTRAINT [FK_dbo.GroupMembership_dbo.Profile_ProfileID] FOREIGN KEY ([ProfileID]) REFERENCES [dbo].[Profile] ([ProfileID])
);


GO
CREATE NONCLUSTERED INDEX [IX_ProfileID]
	ON [dbo].[GroupMembership]([ProfileID] ASC);

