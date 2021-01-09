IF OBJECT_ID('BlogAuthor', 'U') IS NOT NULL
BEGIN
	DROP TABLE BlogAuthor
END
GO

IF OBJECT_ID('BlogConfiguration', 'U') IS NOT NULL
BEGIN
	DROP TABLE BlogConfiguration
END
GO

IF OBJECT_ID('BlogPostCategory', 'U') IS NOT NULL
BEGIN
	DROP TABLE BlogPostCategory
END
GO

IF OBJECT_ID('BlogPost', 'U') IS NOT NULL
BEGIN
	DROP TABLE BlogPost
END
GO

IF OBJECT_ID('BlogCategory', 'U') IS NOT NULL
BEGIN
	DROP TABLE BlogCategory
END
GO

CREATE TABLE BlogCategory(
	Id varchar(100) NOT NULL,
	[Name] nvarchar(500) NOT NULL,
	[Description] nvarchar(1000) NULL,
	UtcCreatedOn datetime NOT NULL,
	UtcUpdatedOn datetime NOT NULL,
	CreatedByUserId varchar(100) NOT NULL,
	UpdatedByUserId varchar(100) NOT NULL,

	CONSTRAINT PK_BlogCategory
		PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_BlogCategory
		UNIQUE ([Name])
)

CREATE TABLE BlogPost(
	Id varchar(100) NOT NULL,
	Title nvarchar(500) NOT NULL,
	Slug nvarchar(500) NOT NULL,
	Tags nvarchar(500) NULL,
	UtcPublishDate datetime NOT NULL,
	Content nvarchar(MAX) NOT NULL,
	UtcCreatedOn datetime NOT NULL,
	UtcUpdatedOn datetime NOT NULL,
	CreatedByUserId varchar(100) NOT NULL,
	UpdatedByUserId varchar(100) NOT NULL,

	CONSTRAINT PK_BlogPost
		PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_BlogPost
		UNIQUE (Slug)
)

CREATE TABLE BlogPostCategory(
	Id varchar(100) NOT NULL,
	BlogPostId varchar(100) NOT NULL,
	BlogCategoryId varchar(100) NOT NULL,
	UtcCreatedOn datetime NOT NULL,
	UtcUpdatedOn datetime NOT NULL,
	CreatedByUserId varchar(100) NOT NULL,
	UpdatedByUserId varchar(100) NOT NULL,

	CONSTRAINT PK_BlogPostCategory
		PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_BlogPostCategory_BlogPost FOREIGN KEY (BlogPostId)
		REFERENCES BlogPost(Id),
	CONSTRAINT FK_BlogPostCategory_BlogCategory FOREIGN KEY (BlogPostId)
		REFERENCES BlogCategory(Id),
)

CREATE TABLE BlogConfiguration(
	Id varchar(100) NOT NULL,
	Title nvarchar(200) NOT NULL,
	[Language] nvarchar(200) NOT NULL,
	[TimeZone] nvarchar(200) NOT NULL,
	[DateFormat] nvarchar(200) NOT NULL,
	[Permalink] nvarchar(200) NOT NULL,
	[PostHeader] nvarchar(MAX) NULL,
	[PostFooter] nvarchar(MAX) NULL,
	UtcCreatedOn datetime NOT NULL,
	UtcUpdatedOn datetime NOT NULL,
	CreatedByUserId varchar(100) NOT NULL,
	UpdatedByUserId varchar(100) NOT NULL,

	CONSTRAINT PK_BlogConfiguration
		PRIMARY KEY CLUSTERED (Id),
)

CREATE TABLE BlogAuthor(
	Id varchar(100) NOT NULL,
	ProfilePicture varchar(200) NOT NULL,
	Website varchar(200) NOT NULL,
	Bio nvarchar(2000) NOT NULL,
	UtcCreatedOn datetime NOT NULL,
	UtcUpdatedOn datetime NOT NULL,
	CreatedByUserId varchar(100) NOT NULL,
	UpdatedByUserId varchar(100) NOT NULL,

	CONSTRAINT PK_BlogAuthor
		PRIMARY KEY CLUSTERED (Id)
)
