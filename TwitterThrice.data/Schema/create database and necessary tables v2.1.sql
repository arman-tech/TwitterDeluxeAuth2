use master
GO

-- 0.  This script must be runned so database and tables can be created.
-- Create the database
CREATE DATABASE twitterThrice;
GO

USE twitterThrice;
GO

-- Create the Members table
CREATE TABLE Members (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
	Username NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    CreatedDate DATETIME2 DEFAULT GETDATE()
);
GO

-- Create the Tweets table
CREATE TABLE Tweets (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    MemberId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Members(Id),
    Message NVARCHAR(140) NOT NULL,
    PostedDate DATETIME2 NOT NULL
);
GO

-- introduce indexing to make the query process faster when grabbing top 10 out of 25M records.
CREATE INDEX IX_Tweets_PostedDate ON dbo.Tweets(PostedDate DESC);
GO

-- introduce indexing to make the query process faster when searching by email
CREATE INDEX IX_Members_Email ON dbo.Members(Email);
GO

