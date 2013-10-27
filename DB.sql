CREATE DATABASE SpamBase
GO

USE SpamBase
GO

CREATE TABLE Users(
	Id int NOT NULL IDENTITY PRIMARY KEY,
	Username nvarchar(50) NOT NULL,
	AuthCode varchar(40) NOT NULL,
	SessionKey varchar(50),
	Email nvarchar(256) NOT NULL,
);
GO

CREATE TABLE SendEmails(
	Id int NOT NULL IDENTITY PRIMARY KEY,
	UserId int NOT NULL FOREIGN KEY REFERENCES Users(Id),
	Subject ntext NOT NULL,
	Content ntext NOT NULL,
	SendDate datetime NOT NULL,
	Recipients ntext NOT NULL
);
GO