CREATE TABLE [dbo].[LiveLocation] (
    [ProfileID]       BIGINT           NOT NULL,
    [SessionID]      NVARCHAR (50)    NOT NULL,
	[ClientTimeStamp]  BIGINT           NOT NULL,
    [IsSOS]           BIT              NULL,
    [Lat]             NVARCHAR (50)    NULL,
    [Long]            NVARCHAR (50)    NULL,
    [Alt]             NVARCHAR (10)    NULL,
    [Speed]           INT              NOT NULL,
    [ClientDateTime] DATETIME         NOT NULL,
    [MediaUri]        NVARCHAR (250)   NULL,
	[Accuracy] VARCHAR(50) NULL,
	[CreatedDate]	  DATETIME         NOT NULL,
	[CreatedBy]		  NVARCHAR (50)    NULL,
    CONSTRAINT [FK_dbo.LiveLocation_dbo.Profile_ProfileID] FOREIGN KEY ([ProfileID]) REFERENCES [dbo].[Profile] ([ProfileID]), 
    CONSTRAINT [PK_LiveLocation] PRIMARY KEY ([ProfileID], [SessionID], [ClientTimeStamp])
);


GO
CREATE NONCLUSTERED INDEX [IX_ProfileID]
    ON [dbo].[LiveLocation]([ProfileID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_LiveLocation_ClientTimeStamp] ON [dbo].[LiveLocation]
	( [ClientTimeStamp] ASC )
GO

