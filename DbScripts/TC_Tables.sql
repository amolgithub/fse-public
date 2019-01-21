USE TweetCloneDB
/* Person table */
DROP TABLE Tweet;
GO
DROP TABLE Following;
GO
DROP TABLE Person;

GO
CREATE TABLE Person
(
	UserId VARCHAR(25) NOT NULL,
	Password VARCHAR(50) NOT NULL,
	FullName VARCHAR(30) NOT NULL,
	Email VARCHAR(50) NOT NULL,
	Joined DATETIME NOT NULL,
	Active BIT NOT NULL
);
GO

ALTER TABLE person
ADD CONSTRAINT pk_user_id PRIMARY KEY(UserId);
GO


/* Following table */
CREATE TABLE Following
(
	followerId INT IDENTITY(1,1) NOT NULL,
	UserId VARCHAR(25) NOT NULL,
	FollowingId VARCHAR(25) NOT NULL
);
GO

ALTER TABLE Following
ADD CONSTRAINT pk_following_id PRIMARY KEY(followerId)
ALTER TABLE Following
ADD CONSTRAINT fk_following_user_id FOREIGN KEY (UserId) REFERENCES Person(UserId);
ALTER TABLE Following
ADD CONSTRAINT fk_following_following_id FOREIGN KEY (FollowingId) REFERENCES Person(UserId);
GO

/* Tweet table */
CREATE TABLE Tweet
(
	TweetId INT IDENTITY(1,1) NOT NULL,
	UserId VARCHAR(25) NOT NULL,
	Message VARCHAR(140) NOT NULL,
	Created DATETIME
);
GO

ALTER TABLE Tweet
ADD CONSTRAINT pk_tweet_id PRIMARY KEY(TweetId);
ALTER TABLE Tweet
ADD CONSTRAINT fk_tweet_user_id FOREIGN KEY(UserId) REFERENCES person(UserId);
GO

