CREATE TABLE dbo.[SavedSearch] (
    [Id]					        uniqueidentifier	NOT NULL,
    [UserRef]  		    	        uniqueidentifier    NOT NULL,
    [DateCreated]                   datetime            NOT NULL,
    [LastRunDate]                   datetime            NULL,
    [EmailLastSendDate]             datetime            NULL,
    [SearchParameters]  		    nvarchar(max)       NOT NULL    
    CONSTRAINT [PK_SavedSearch] PRIMARY KEY (Id),    
    INDEX [IX_SavedSearch_UserRef] NONCLUSTERED(UserRef)
	)