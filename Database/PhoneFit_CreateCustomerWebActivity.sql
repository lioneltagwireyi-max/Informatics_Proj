USE PhoneFitDB;
GO

/*
  Minimal customer web-activity log for Manager statistics.
  The PhoneFit web app writes here when connection string PhoneFitDB is configured.
  It also mirrors events to App_Data for local fallback (see ActivityTracker.cs).
*/

IF OBJECT_ID('dbo.CustomerWebActivity', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.CustomerWebActivity (
        ActivityID INT IDENTITY(1,1) NOT NULL,
        UserID INT NULL,
        RoleName VARCHAR(50) NULL,
        ActionType VARCHAR(50) NOT NULL,   -- Login, PageView, AddToCart, Checkout
        PagePath VARCHAR(250) NOT NULL,
        Detail VARCHAR(500) NULL,
        OccurredAt DATETIME NOT NULL CONSTRAINT DF_CustomerWebActivity_OccurredAt DEFAULT GETDATE(),
        PRIMARY KEY (ActivityID),
        FOREIGN KEY (UserID) REFERENCES dbo.UserAccount(UserID)
    );

    CREATE INDEX IX_CustomerWebActivity_OccurredAt
        ON dbo.CustomerWebActivity (OccurredAt DESC);

    CREATE INDEX IX_CustomerWebActivity_ActionType
        ON dbo.CustomerWebActivity (ActionType, OccurredAt DESC);

    CREATE INDEX IX_CustomerWebActivity_UserID
        ON dbo.CustomerWebActivity (UserID, OccurredAt DESC);
END
GO
