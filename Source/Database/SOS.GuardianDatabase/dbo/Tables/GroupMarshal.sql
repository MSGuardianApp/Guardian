CREATE TABLE [dbo].[GroupMarshal] (
    [GroupID]          INT           NOT NULL,
    [ProfileID]        BIGINT        NOT NULL,
    [IsValidated]      BIT           NOT NULL,
    [CreatedDate]      DATETIME      NULL,
    [CreatedBy]        NVARCHAR (50) NULL,
    [LastModifiedDate] DATETIME      NULL,
    [LastModifiedBy]   NVARCHAR (50) NULL,
    CONSTRAINT [PK_dbo.GroupMarshal] PRIMARY KEY CLUSTERED ([GroupID] ASC, [ProfileID] ASC),
    CONSTRAINT [FK_dbo.GroupMarshal_dbo.Profile_ProfileID] FOREIGN KEY ([ProfileID]) REFERENCES [dbo].[Profile] ([ProfileID])
);


GO
CREATE NONCLUSTERED INDEX [IX_ProfileID]
    ON [dbo].[GroupMarshal]([ProfileID] ASC);

