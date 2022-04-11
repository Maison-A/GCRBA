-- DROP TABLES
IF OBJECT_ID('tblSpecialLocation')		IS NOT NULL DROP TABLE tblSpecialLocation 
IF OBJECT_ID('tblCompanyAward')			IS NOT NULL DROP TABLE tblCompanyAward
IF OBJECT_ID('tblLocationHours')		IS NOT NULL DROP TABLE tblLocationHours
IF OBJECT_ID('tblCompanyMember')		IS NOT NULL DROP TABLE tblCompanyMember
IF OBJECT_ID('tblCategoryLocation')		IS NOT NULL DROP TABLE tblCategoryLocation 
IF OBJECT_ID('tblEventLocation')		IS NOT NULL DROP TABLE tblEventLocation 
IF OBJECT_ID('tblSpecial')				IS NOT NULL DROP TABLE tblSpecial 
IF OBJECT_ID('tblCategory')				IS NOT NULL DROP TABLE tblCategory
IF OBJECT_ID('tblEvent')				IS NOT NULL DROP TABLE tblEvent
IF OBJECT_ID('tblContactPerson')		IS NOT NULL DROP TABLE tblContactPerson
IF OBJECT_ID('tblLocation')				IS NOT NULL DROP TABLE tblLocation 
IF OBJECT_ID('tblAdminRequest')			IS NOT NULL DROP TABLE tblAdminRequest  
IF OBJECT_ID('tblCompanySocialMedia')	IS NOT NULL DROP TABLE tblCompanySocialMedia
IF OBJECT_ID('tblWebsite')				IS NOT NULL DROP TABLE tblWebsite 
IF OBJECT_ID('tblWebsiteType')			IS NOT NULL DROP TABLE tblWebsiteType
IF OBJECT_ID('tblCompany')				IS NOT NULL DROP TABLE tblCompany 
IF OBJECT_ID('tblContactPersonType')	IS NOT NULL DROP TABLE tblContactPersonType
IF OBJECT_ID('tblSocialMedia')			IS NOT NULL DROP TABLE tblSocialMedia
IF OBJECT_ID('tblMember')				IS NOT NULL DROP TABLE tblMember 
IF OBJECT_ID('tblMemberLevel')			IS NOT NULL DROP TABLE tblMemberLevel 
IF OBJECT_ID('tblPaymentType')			IS NOT NULL DROP TABLE tblPaymentType 
IF OBJECT_ID('tblApprovalStatus')		IS NOT NULL DROP TABLE tblApprovalStatus 
IF OBJECT_ID('tblDay')					IS NOT NULL DROP TABLE tblDay
IF OBJECT_ID('tblMainBanner')			IS NOT NULL DROP TABLE tblMainBanner
IF OBJECT_ID('tblAboutGCRBA')			IS NOT NULL DROP TABLE tblAboutGCRBA
IF OBJECT_ID('tblUser')					IS NOT NULL DROP TABLE tblUser 
IF OBJECT_ID('tblState')				IS NOT NULL DROP TABLE tblState 

--DROP STORED PROCEDURES
IF OBJECT_ID('LOGIN')					IS NOT NULL DROP PROCEDURE LOGIN 
IF OBJECT_ID('VERIFY_MEMBER')			IS NOT NULL DROP PROCEDURE VERIFY_MEMBER
IF OBJECT_ID('INSERT_WEBSITE')			IS NOT NULL DROP PROCEDURE INSERT_WEBSITE
IF OBJECT_ID('INSERT_CONTACTPERSON')	IS NOT NULL DROP PROCEDURE INSERT_CONTACTPERSON
IF OBJECT_ID('INSERT_SOCIALMEDIA')		IS NOT NULL DROP PROCEDURE INSERT_SOCIALMEDIA
IF OBJECT_ID('INSERT_LOCATION')			IS NOT NULL DROP PROCEDURE INSERT_LOCATION
IF OBJECT_ID('INSERT_COMPANY')			IS NOT NULL DROP PROCEDURE INSERT_COMPANY
IF OBJECT_ID('INSERT_CATEGORYLOCATION') IS NOT NULL DROP PROCEDURE INSERT_CATEGORYLOCATION
IF OBJECT_ID('INSERT_LOCATIONHOURS')	IS NOT NULL DROP PROCEDURE INSERT_LOCATIONHOURS
IF OBJECT_ID('GET_MAIN_BANNER')			IS NOT NULL DROP PROCEDURE GET_MAIN_BANNER
IF OBJECT_ID('GET_COMPANY_INFO')		IS NOT NULL DROP PROCEDURE GET_COMPANY_INFO
IF OBJECT_ID('GET_ALL_MAIN_BANNERS')	IS NOT NULL DROP PROCEDURE GET_ALL_MAIN_BANNERS
IF OBJECT_ID('SELECT_LOCATION')			IS NOT NULL DROP PROCEDURE SELECT_LOCATION
IF OBJECT_ID('SELECT_CATEGORYLOCATION') IS NOT NULL DROP PROCEDURE SELECT_CATEGORYLOCATION
IF OBJECT_ID('INSERT_NEW_USER')			IS NOT NULL	DROP PROCEDURE INSERT_NEW_USER
IF OBJECT_ID('SELECT_STATES')			IS NOT NULL	DROP PROCEDURE SELECT_STATES
IF OBJECT_ID('DELETE_LOCATION')			IS NOT NULL	DROP PROCEDURE DELETE_LOCATION
IF OBJECT_ID('INSERT_NEW_MAIN_BANNER')	IS NOT NULL	DROP PROCEDURE INSERT_NEW_MAIN_BANNER
IF OBJECT_ID('REUSE_MAIN_BANNER')		IS NOT NULL	DROP PROCEDURE REUSE_MAIN_BANNER
IF OBJECT_ID('DELETE_COMPANY')			IS NOT NULL	DROP PROCEDURE DELETE_COMPANY
IF OBJECT_ID('GET_LOCATIONS')			IS NOT NULL	DROP PROCEDURE GET_LOCATIONS
IF OBJECT_ID('GET_SPECIFIC_COMPANY')	IS NOT NULL DROP PROCEDURE GET_SPECIFIC_COMPANY

CREATE TABLE tblState
(
	intStateID		SMALLINT IDENTITY(1,1)	NOT NULL,
	strState		NVARCHAR(20)		NOT NULL, 
	CONSTRAINT tblState_PK PRIMARY KEY (intStateID)
)

CREATE TABLE tblAboutGCRBA
(
	intAboutGCRBAID		SMALLINT IDENTITY (1,1) NOT NULL,
	strAbout			NVARCHAR(1000) NOT NULL,
	CONSTRAINT tblAboutGCRBA_PK PRIMARY KEY (intAboutGCRBAID)
)

-- all users listed here
-- if admin, isAdmin = 1, else isAdmin = 0
-- phone number is optional, so is allowed null value 
-- brackets around table name to make explicit b/c user is reserved keyword
CREATE TABLE tblUser
(
	intUserID		SMALLINT IDENTITY(1,1)	NOT NULL, 
	strFirstName		NVARCHAR(25)		NOT NULL, 
	strLastName		NVARCHAR(25)		NOT NULL, 
	strAddress		NVARCHAR(100), 
	strCity			NVARCHAR(20), 
	intStateID		SMALLINT, 
	strZip			NVARCHAR(15), 
	strPhone		NVARCHAR(20), 
	strEmail		NVARCHAR(50)		NOT NULL, 
	strUsername		NVARCHAR(15)		NOT NULL, 
	strPassword		NVARCHAR(15)		NOT NULL, 
	isAdmin			BIT			NOT NULL, 
	CONSTRAINT tblUser_PK PRIMARY KEY (intUserID)
)

CREATE TABLE tblMemberLevel
(
	intMemberLevelID		SMALLINT IDENTITY(1,1)	NOT NULL, 
	strMemberLevel			NVARCHAR(15)		NOT NULL, 
	CONSTRAINT tblMemberLevel_PK PRIMARY KEY (intMemberLevelID)
)

CREATE TABLE tblPaymentType
(
	intPaymentTypeID		SMALLINT IDENTITY(1,1)	NOT NULL, 
	strPaymentType			NVARCHAR(15)		NOT NULL, 
	CONSTRAINT tblPaymentType_PK PRIMARY KEY (intPaymentTypeID)
)

CREATE TABLE tblMember 
(
	intMemberID			SMALLINT IDENTITY(1,1)	NOT NULL, 
	intUserID			SMALLINT		NOT NULL, 
	intMemberLevelID		SMALLINT		NOT NULL, 
	intPaymentTypeID		SMALLINT		NOT NULL,
	CONSTRAINT tblMember_PK PRIMARY KEY (intMemberID)
)	

CREATE TABLE tblCompany
(
	intCompanyID			BIGINT IDENTITY(1,1)	NOT NULL, 
	strCompanyName			NVARCHAR(50)		NOT NULL, 
	strAbout			NVARCHAR(2000)			NOT NULL, 
	strBizYear			NVARCHAR(10),
	CONSTRAINT tblCompany_PK PRIMARY KEY (intCompanyID)
)

CREATE TABLE tblSocialMedia
(
	intSocialMediaID		SMALLINT IDENTITY (1,1) NOT NULL,
	strPlatform				NVARCHAR(50)			NOT NULL,
	CONSTRAINT tblSocialMedia_PK PRIMARY KEY (intSocialMediaID)
)

CREATE TABLE tblCompanySocialMedia
(
	intCompanySocialMediaID BIGINT IDENTITY NOT NULL,
	strSocialMediaLink		NVARCHAR(100)	NOT NULL,
	intCompanyID			BIGINT			NOT NULL,
	intSocialMediaID		SMALLINT		NOT NULL,
	CONSTRAINT tblCompanySocialMedia_PK PRIMARY KEY (intCompanySocialMediaID)
)

CREATE TABLE tblCompanyAward
(
	intCompanyAwardID		SMALLINT IDENTITY(1, 1)	NOT NULL, 
	intCompanyID			BIGINT			NOT NULL, 
	strFrom				VARCHAR(100)		NOT NULL, 
	strAward			NVARCHAR(200)		NOT NULL, 
	CONSTRAINT tblCompanyAward_PK PRIMARY KEY (intCompanyAwardID)
)

CREATE TABLE tblCompanyMember
(
	intCompanyMemberID		SMALLINT IDENTITY(1,1)	NOT NULL, 
	intCompanyID			BIGINT			NOT NULL,
	intMemberID			SMALLINT		NOT NULL, 
	CONSTRAINT tblCompanyMember_PK PRIMARY KEY (intCompanyMemberID)
)

CREATE TABLE tblCategory
(
	intCategoryID			SMALLINT IDENTITY(1,1)	NOT NULL, 
	strCategory			NVARCHAR(50)		NOT NULL, 
	CONSTRAINT tblCategory_PK PRIMARY KEY (intCategoryID)
)

-- brackets around table name to make explicit b/c location is reserved keyword
CREATE TABLE tblLocation
(
	intLocationID			BIGINT IDENTITY(1,1)	NOT NULL, 
	intCompanyID			BIGINT			NOT NULL, 
	strAddress			NVARCHAR(100)		NOT NULL, 
	strCity				NVARCHAR(20)		NOT NULL, 
	intStateID			SMALLINT			NOT NULL, 
	strZip				NVARCHAR(15)		NOT NULL,
	strPhone			NVARCHAR(20)		NOT NULL,
	strEmail			NVARCHAR(50),		
	CONSTRAINT tblLocation_PK PRIMARY KEY (intLocationID)
)

CREATE TABLE tblContactPerson
(
	intContactPersonID		BIGINT IDENTITY(1,1) NOT NULL,
	strContactName		NVARCHAR(50)		NOT NULL,
	strContactPhone			NVARCHAR(20)		,
	strContactEmail			NVARCHAR(50)		,
	intLocationID			BIGINT				,
	intCompanyID			BIGINT				NOT NULL,
	intContactPersonTypeID	SMALLINT			NOT NULL,
	CONSTRAINT tblContactPerson_PK PRIMARY KEY (intContactPersonID)
)

CREATE TABLE tblContactPersonType
(
	intContactPersonTypeID		SMALLINT IDENTITY(1,1)	NOT NULL,
	strContactPersonType		NVARCHAR(50)			NOT NULL,
	CONSTRAINT tblContactPersonType_PK PRIMARY KEY (intContactPersonTypeID)
)

CREATE TABLE tblCategoryLocation
(
	intCategoryLocationID		BIGINT IDENTITY(1,1)	NOT NULL,
	intCategoryID			SMALLINT		NOT NULL,
	intLocationID			BIGINT			NOT NULL,
	blnAvailable			BIT			NOT NULL,
	CONSTRAINT tblCategoryLocation_PK PRIMARY KEY (intCategoryLocationID)
)

CREATE TABLE tblApprovalStatus
(
	intApprovalStatusID		SMALLINT IDENTITY(1,1)	NOT NULL,
	strApprovalStatus		NVARCHAR(20)		NOT NULL, 
	CONSTRAINT tblApprovalStatus_PK PRIMARY KEY (intApprovalStatusID)
)

CREATE TABLE tblAdminRequest
(
	intAdminRequestID		SMALLINT IDENTITY(1,1)		NOT NULL,
	intMemberID			SMALLINT			NOT NULL,
	strRequestType			NVARCHAR(50)			NOT NULL, 
	strRequestedChange		NVARCHAR(500)			NOT NULL,
	intApprovalStatusID		SMALLINT			NOT NULL,
	CONSTRAINT tblAdminRequest_PK PRIMARY KEY (intAdminRequestID)
)

CREATE TABLE tblEvent
(
	intEventID				SMALLINT IDENTITY(1,1)		NOT NULL,
	dtmStart				DATE				NOT NULL, 
	dtmEnd					DATE				NOT NULL, 
	CONSTRAINT tblEvent_PK PRIMARY KEY (intEventID)
)	

CREATE TABLE tblEventLocation
(
	intEventLocationID		SMALLINT IDENTITY(1,1)		NOT NULL, 
	intEventID			SMALLINT			NOT NULL,
	intLocationID			BIGINT				NOT NULL,
	CONSTRAINT tblEventLocation_PK PRIMARY KEY (intEventLocationID)
)

-- price can be null b/c special may not necessarily be part of special 
CREATE TABLE tblSpecial
(
	intSpecialID			SMALLINT IDENTITY(1,1)		NOT NULL,
	strDescription			NVARCHAR(500)			NOT NULL, 
	strPrice			NVARCHAR(10), 
	dtmStart			DATE				NOT NULL, 
	dtmEnd				DATE				NOT NULL, 
	CONSTRAINT tblSpecial_PK PRIMARY KEY (intSpecialID)
)

CREATE TABLE tblSpecialLocation
(
	intSpecialLocationID	SMALLINT IDENTITY(1,1)		NOT NULL,
	intSpecialID		SMALLINT			NOT NULL,
	intLocationID		BIGINT				NOT NULL,
	CONSTRAINT tblSpecialLocation_PK PRIMARY KEY (intSpecialLocationID)
)

CREATE TABLE tblWebsiteType
(
	intWebsiteTypeID  SMALLINT IDENTITY(1,1) NOT NULL,
	strWebsiteType		VARCHAR(20)			NOT NULL,
	CONSTRAINT tblWebsiteType_PK PRIMARY KEY (intWebsiteTypeID)
)

CREATE TABLE tblWebsite
(
	intWebsiteID			BIGINT IDENTITY(1,1)		NOT NULL, 
	intCompanyID			BIGINT				NOT NULL,
	strURL				NVARCHAR(100)			NOT NULL, 
	intWebsiteTypeID	SMALLINT				NOT NULL,
	CONSTRAINT tblWebsite_PK PRIMARY KEY (intWebsiteID)
)

CREATE TABLE tblDay
(
	intDayID		SMALLINT IDENTITY(1,1)	NOT NULL, 
	strDay			NVARCHAR(20)		NOT NULL, 
	CONSTRAINT tblDay_PK PRIMARY KEY (intDayID)
)

CREATE TABLE tblLocationHours
(
	intLocationHoursID		BIGINT IDENTITY(1,1)		NOT NULL, 
	intLocationID			BIGINT				NOT NULL, 
	intDayID			SMALLINT			NOT NULL, 
	strOpen				NVARCHAR(100), 
	strClose			NVARCHAR(100), 
	CONSTRAINT tblLocationHours_PK PRIMARY KEY (intLocationHoursID)
)

CREATE TABLE tblMainBanner
(
	intMainBannerID		SMALLINT IDENTITY(1, 1)	NOT NULL, 
	strBanner		NVARCHAR(1000)		NOT NULL,
	CONSTRAINT tblMainBanner_PK PRIMARY KEY (intMainBannerID)
)

-------------------------------------------------------------------------------------------------------------------------------
-- FOREIGN KEYS 
-------------------------------------------------------------------------------------------------------------------------------

-- CHILD					PARENT					COLUMN(s)
-- -----					-----					------
-- tblUser					tblState				intStateID
-- tblMember				tblUser					intUserID
-- tblMember				tblMemberLevel			intMemberLevelID
-- tblMember				tblPaymentType			intPaymentTypeID
-- tblCompanyMember			tblCompany				intCompanyID
-- tblCompanyMember			tblMember				intMemberID
-- tblLocation				tblCompany				intCompanyID
-- tblLocation				tblState				intStateID
-- tblCategoryLocation		tblCategory				intCategoryID
-- tblCategoryLocation		tblLocation				intLocationID
-- tblEventLocation			tblEvent				intEventID
-- tblEventLocation			tblLocation				intLocationID
-- tblAdminRequest			tblMember				intMemberID
-- tblAdminRequest			tblApprovalStatus		intApprovalStatusID
-- tblSpecialLocation		tblSpecial				intSpecialID
-- tblSpecialLocation		tblLocation				intLocationID
-- tblLocationHours			tblLocation				intLocationID
-- tblLocationHours			tblDay					intDayID
-- tblCompanyAward			tblCompany				intCompanyID
-- tblCompanySocialMedia	tblCompany				intCompanyID
-- tblCompanySocialMedia	tblSocialMedia			intSocialMedia
-- tblContactPerson			tblLocation				intLocationID
-- tblContactPerson			tblCompanyID			intCompanyID
-- tblContactPerson			tblContactPersonType	intContactPersonTypeID

ALTER TABLE tblUser ADD CONSTRAINT tblUser_tblState_FK
FOREIGN KEY (intStateID) REFERENCES tblState (intStateID)

ALTER TABLE tblMember ADD CONSTRAINT tblMember_tblUser_FK
FOREIGN KEY (intUserID) REFERENCES tblUser (intUserID)

ALTER TABLE tblMember ADD CONSTRAINT tblMember_tblMemberLevel_FK
FOREIGN KEY (intMemberLevelID) REFERENCES tblMemberLevel (intMemberLevelID)

ALTER TABLE tblMember ADD CONSTRAINT tblMember_tblPaymentType_FK
FOREIGN KEY (intPaymentTypeID) REFERENCES tblPaymentType (intPaymentTypeID)

ALTER TABLE tblCompanyMember ADD CONSTRAINT tblCompanyMember_tblCompany_FK
FOREIGN KEY (intCompanyID) REFERENCES tblCompany (intCompanyID)

ALTER TABLE tblCompanyMember ADD CONSTRAINT tblCompanyMember_tblMember_FK
FOREIGN KEY (intMemberID) REFERENCES tblMember (intMemberID)

ALTER TABLE tblLocation ADD CONSTRAINT tblLocation_tblCompany_FK
FOREIGN KEY (intCompanyID) REFERENCES tblCompany (intCompanyID)

ALTER TABLE tblLocation ADD CONSTRAINT tblLocation_tblState_FK
FOREIGN KEY (intStateID) REFERENCES tblState (intStateID)

ALTER TABLE tblCategoryLocation ADD CONSTRAINT tblCategoryLocation_tblCategory_FK
FOREIGN KEY (intCategoryID) REFERENCES tblCategory (intCategoryID)

ALTER TABLE tblCategoryLocation ADD CONSTRAINT tblCategoryLocation_tblLocation_FK
FOREIGN KEY (intLocationID) REFERENCES tblLocation (intLocationID)

ALTER TABLE tblEventLocation ADD CONSTRAINT tblEventLocation_tblEvent_FK
FOREIGN KEY (intEventID) REFERENCES tblEvent (intEventID)

ALTER TABLE tblEventLocation ADD CONSTRAINT tblEventLocation_tblLocation_FK
FOREIGN KEY (intLocationID) REFERENCES tblLocation (intLocationID)

ALTER TABLE tblAdminRequest ADD CONSTRAINT tblAdminRequest_tblMember_FK
FOREIGN KEY (intMemberID) REFERENCES tblMember (intMemberID)

ALTER TABLE tblAdminRequest ADD CONSTRAINT tblAdminRequest_tblApprovalStatus_FK
FOREIGN KEY (intApprovalStatusID) REFERENCES tblApprovalStatus (intApprovalStatusID)

ALTER TABLE tblSpecialLocation ADD CONSTRAINT tblSpecialLocation_tblSpecial_FK
FOREIGN KEY (intSpecialID) REFERENCES tblSpecial (intSpecialID)

ALTER TABLE tblSpecialLocation ADD CONSTRAINT tblSpecialLocation_tblLocation_FK 
FOREIGN KEY (intLocationID) REFERENCES tblLocation (intLocationID)

ALTER TABLE tblLocationHours ADD CONSTRAINT tblLocationHours_tblLocation_FK
FOREIGN KEY (intLocationID) REFERENCES tblLocation (intLocationID)

ALTER TABLE tblLocationHours ADD CONSTRAINT tblLocationHours_tblDay_FK
FOREIGN KEY (intDayID) REFERENCES tblDay (intDayID)

ALTER TABLE tblCompanyAward ADD CONSTRAINT tblCompanyAward_tblCompany_FK
FOREIGN KEY (intCompanyID) REFERENCES tblCompany (intCompanyID)

ALTER TABLE tblCompanySocialMedia ADD CONSTRAINT tblCompanySocialMedia_tblCompany_FK
FOREIGN KEY (intCompanyID) REFERENCES tblCompany (intCompanyID)

ALTER TABLE tblCompanySocialMedia ADD CONSTRAINT tblCompanySocialMedia_tblSocialMedia_FK
FOREIGN KEY (intSocialMediaID) REFERENCES tblSocialMedia (intSocialMediaID)

ALTER TABLE tblWebsite ADD CONSTRAINT tblWebsite_tblWebsiteType_FK
FOREIGN KEY (intWebsiteTypeID) REFERENCES tblWebsiteType (intWebsiteTypeID)

ALTER TABLE tblWebsite ADD CONSTRAINT tblWebsite_tblCompany_FK
FOREIGN KEY (intCompanyID) REFERENCES tblCompany (intCompanyID)

ALTER TABLE tblContactPerson ADD CONSTRAINT tblContactPerson_tblLocation_FK
FOREIGN KEY (intLocationID) REFERENCES tblLocation (intLocationID)

ALTER TABLE tblContactPerson ADD CONSTRAINT tblContactPerson_tblCompany_FK
FOREIGN KEY (intCompanyID) REFERENCES tblCompany (intCompanyID)

ALTER TABLE tblContactPerson ADD CONSTRAINT tblContactPerson_tblContactPersonType_FK
FOREIGN KEY (intContactPersonTypeID) REFERENCES tblContactPersonType (intContactPersonTypeID)


-- -----------------------------------------------------------------------------------------
-- STORED PROCEDURES 
-- -----------------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [db_owner].[LOGIN]
@strUsername NVARCHAR(20)
,@strPassword NVARCHAR(20)
AS
BEGIN	
	SET NOCOUNT ON;

	SELECT		u.intUserID, u.strFirstName, u.strLastName, u.strAddress, u.strCity, s.strState, u.strZip, u.strPhone, u.strEmail, u.strUsername, u.strPassword, u.isAdmin
	FROM		tblState as s JOIN tblUser as u 
				ON s.intStateID = u.intStateID
	WHERE		u.strUsername = @strUsername and u.strPassword = @strPassword
END
GO


CREATE PROCEDURE [db_owner].[VERIFY_MEMBER]
@intUserID SMALLINT
AS 
BEGIN
	SET NOCOUNT ON;

	SELECT		intMemberID 
	FROM		tblMember
	WHERE		intUserID = @intUserID
END
GO

CREATE PROCEDURE [dbo].[INSERT_WEBSITE]
@intWebsiteID AS BIGINT OUTPUT
,@intCompanyID AS BIGINT
,@strURL AS NVARCHAR(100)
,@intWebsiteTypeID AS SMALLINT
AS
SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN

	DECLARE @COUNT AS TINYINT

	--DONT ALLOW MORE THAN ONE COMPANY NAME IN THIS TABLE
	SELECT @COUNT=COUNT(*) FROM db_owner.tblWebsite  WHERE intCompanyID = @intCompanyID AND strURL = @strURL 
	IF @COUNT >0 RETURN -1 --COMPANY WEBPAGE CONNECTION ALREADY EXISTS

	INSERT INTO [db_owner].[tblWebsite] WITH (TABLOCKX)
				([intCompanyID]
				,[strURL]
				,[intWebsiteTypeID])
			VALUES
				(@intCompanyID
				,@strURL
				,@intWebsiteTypeID)
	SELECT @intWebsiteID=@@IDENTITY
	RETURN 1

END
GO

CREATE PROCEDURE [dbo].[INSERT_NEW_USER]
@intNewUserID SMALLINT  = null OUTPUT, 
@strFirstName NVARCHAR(25), 
@strLastName NVARCHAR(25),
@strEmail NVARCHAR(50),
@strUsername NVARCHAR (15),
@strPassword NVARCHAR(15),
@isAdmin BIT
AS
SET NOCOUNT ON
SET XACT_ABORT ON 
BEGIN
	DECLARE @COUNT AS TINYINT 

	SELECT @COUNT=COUNT(*) FROM db_owner.tblUser WHERE strUsername = @strUsername
	IF @COUNT > 0 RETURN -2 -- user with this username already exists 
	
	SELECT @COUNT=COUNT(*)FROM db_owner.tblUser WHERE strEmail = @strEmail
	IF @COUNT > 0 RETURN -1 -- user with this email already exists 

	INSERT INTO [db_owner].[tblUser]
			([strFirstName],
			 [strLastName],
			 [strEmail],
			 [strUsername],
			 [strPassword],
			 [isAdmin])
		VALUES
			(@strFirstName, 
			 @strLastName, 
			 @strEmail,
			 @strUsername, 
			 @strPassword,
			 @isAdmin)
	SELECT @intNewUserID=@@IDENTITY
	RETURN 1
END 
GO


CREATE PROCEDURE [dbo].[INSERT_CONTACTPERSON]
@intContactPersonID AS BIGINT OUTPUT
,@strContactName AS NVARCHAR(50)
,@strContactPhone AS NVARCHAR(20)
,@strContactEmail AS NVARCHAR(50)
,@intLocationID	AS BIGINT
,@intCompanyID AS BIGINT
,@intContactPersonTypeID AS SMALLINT
AS
SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN

	DECLARE @COUNT AS TINYINT

	INSERT INTO [db_owner].[tblContactPerson] WITH (TABLOCKX)
				([strContactName]
				,[strContactPhone]
				,[strContactEmail]
				,[intLocationID]
				,[intCompanyID]
				,[intContactPersonTypeID])
			VALUES
				(@strContactName
				,@strContactPhone
				,@strContactEmail
				,@intLocationID
				,@intCompanyID
				,@intContactPersonTypeID)
	SELECT @intContactPersonID=@@IDENTITY
	RETURN 1

END
GO	

CREATE PROCEDURE [dbo].[INSERT_LOCATION]
@intLocationID AS BIGINT OUTPUT
,@intCompanyID AS BIGINT
,@strAddress AS NVARCHAR(100)
,@strCity AS NVARCHAR(20)
,@intStateID AS SMALLINT
,@strZip NVARCHAR(15)
,@strPhone NVARCHAR(20)
,@strEmail NVARCHAR(50)
AS
SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN

	DECLARE @COUNT AS TINYINT

	--DONT ALLOW MORE THAN ONE EXACT ADDRESS IN THIS TABLE
	SELECT @COUNT=COUNT(*) FROM db_owner.tblLocation  WHERE strAddress = @strAddress
	IF @COUNT >0 RETURN -1 --COMPANY NAME EXISTS

	INSERT INTO [db_owner].[tblLocation] WITH (TABLOCKX)
				([intCompanyID]
				,[strAddress]
				,[strCity]
				,[intStateID]
				,[strZip]
				,[strPhone]
				,[strEmail])
			VALUES
				(@intCompanyID
				,@strAddress
				,@strCity
				,@intStateID
				,@strZip
				,@strPhone
				,@strEmail)
	SELECT @intLocationID=@@IDENTITY
	RETURN 1

END
GO

CREATE PROCEDURE [dbo].[INSERT_SOCIALMEDIA]
@intCompanySocialMediaID AS BIGINT OUTPUT
,@strSocialMediaLink AS NVARCHAR(100)
,@intCompanyID AS BIGINT
,@intSocialMediaID AS SMALLINT
AS
SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN

	DECLARE @COUNT AS TINYINT

	INSERT INTO [db_owner].[tblCompanySocialMedia] WITH (TABLOCKX)
				([strSocialMediaLink]
				,[intCompanyID]
				,[intSocialMediaID])
			VALUES
				(@strSocialMediaLink
				,@intCompanyID
				,@intSocialMediaID)
	SELECT @intCompanySocialMediaID=@@IDENTITY
	RETURN 1

END
GO

CREATE PROCEDURE [dbo].[INSERT_COMPANY]
@intCompanyID AS BIGINT OUTPUT
,@strCompanyName AS NVARCHAR(50)
,@strAbout AS NVARCHAR(2000)
,@strBizYear NVARCHAR(10)
AS
SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN

	DECLARE @COUNT AS TINYINT

	--DONT ALLOW MORE THAN ONE COMPANY NAME IN THIS TABLE
	SELECT @COUNT=COUNT(*) FROM db_owner.tblCompany  WHERE strCompanyName = @strCompanyName
	IF @COUNT >0 RETURN -1 --COMPANY NAME EXISTS

	INSERT INTO [db_owner].[tblCompany] WITH (TABLOCKX)
				([strCompanyName]
				,[strAbout]
				,[strBizYear])
			VALUES
				(@strCompanyName
				,@strAbout
				,@strBizYear)
	SELECT @intCompanyID=@@IDENTITY
	RETURN 1

END
GO

CREATE PROCEDURE [dbo].[INSERT_CATEGORYLOCATION]
@intCategoryLocationID AS BIGINT OUTPUT
,@intCategoryID AS SMALLINT
,@intLocationID AS BIGINT
,@blnAvailable AS BIT
AS
SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN

	DECLARE @COUNT AS TINYINT

	--DONT ALLOW MORE THAN ONE EXACT ADDRESS IN THIS TABLE
	SELECT @COUNT=COUNT(*) FROM db_owner.tblCategoryLocation  WHERE intCategoryID = @intCategoryID AND intLocationID = @intLocationID
	IF @COUNT >0 RETURN -1 --CATEGORY NAME ALREADY EXISTS FOR LOCATION

	INSERT INTO [db_owner].[tblCategoryLocation] WITH (TABLOCKX)
				([intCategoryID]
				,[intLocationID]
				,[blnAvailable])
			VALUES
				(@intCategoryID
				,@intLocationID
				,@blnAvailable)
	SELECT @intCategoryLocationID=@@IDENTITY
	RETURN 1

END
GO

CREATE PROCEDURE [dbo].[INSERT_LOCATIONHOURS]
@intLocationHoursID AS BIGINT OUTPUT
,@intLocationID AS SMALLINT
,@intDayID AS BIGINT
,@strOpen AS NVARCHAR(100)
,@strClose AS NVARCHAR(100)
AS
SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN

	DECLARE @COUNT AS TINYINT

	--DONT ALLOW MORE THAN ONE EXACT ADDRESS IN THIS TABLE
	SELECT @COUNT=COUNT(*) FROM db_owner.tblLocationHours  WHERE intLocationID = @intLocationID AND @intDayID = intDayID
	IF @COUNT >0 RETURN -1 --LOCATION HOURS ALREADY EXIST

	INSERT INTO [db_owner].[tblLocationHours] WITH (TABLOCKX)
				([intLocationID]
				,[intDayID]
				,[strOpen]
				,[strClose])
			VALUES
				(@intLocationID
				,@intDayID
				,@strOpen
				,@strClose)
	SELECT @intLocationHoursID=@@IDENTITY
	RETURN 1

END
GO

CREATE PROCEDURE [db_owner].[GET_MAIN_BANNER]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	strBanner 
	FROM	tblMainBanner 
	WHERE	intMainBannerID IN (SELECT MAX(intMainBannerID) FROM tblMainBanner)
END
GO

CREATE PROCEDURE [db_owner].[GET_ALL_MAIN_BANNERS]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	intMainBannerID, strBanner 
	FROM	tblMainBanner 
END
GO

CREATE PROCEDURE [db_owner].[GET_LOCATIONS]
@intCompanyID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT l.intLocationID, l.strAddress, l.strCity, s.strState, l.strZip, l.strPhone, l.strEmail
	FROM tblLocation AS l JOIN tblState AS s ON s.intStateID = l.intStateID
	WHERE l.intCompanyID = @intCompanyID
END
GO

CREATE PROCEDURE [db_owner].[GET_COMPANY_INFO]
AS 
BEGIN
	SET NOCOUNT ON;

	SELECT	intCompanyID, strCompanyName
	FROM	tblCompany 
END 
GO

CREATE PROCEDURE [db_owner].[GET_SPECIFIC_COMPANY]
@intCompanyID BIGINT
AS 
BEGIN
	SET NOCOUNT ON;

	SELECT	intCompanyID, strCompanyName, strAbout, strBizYear 
	FROM	tblCompany 
	WHERE	intCompanyID = @intCompanyID
END
GO


CREATE PROCEDURE [dbo].[SELECT_LOCATION]
@intLocationID BIGINT = NULL
AS 
BEGIN
	SET NOCOUNT ON;
	
	IF @intLocationID IS NOT NULL
	BEGIN
		SELECT *
		FROM db_owner.tblLocation as Loc
		JOIN db_owner.tblCompany as Comp
		ON Comp.intCompanyID = Loc.intCompanyID
		JOIN db_owner.tblState as St
		ON St.intStateID = Loc.intStateID
		WHERE [intLocationID] = @intLocationID
	END
END
GO

CREATE PROCEDURE [dbo].[SELECT_CATEGORYLOCATION]
@intCategoryID SMALLINT = NULL
AS 
BEGIN
	SET NOCOUNT ON;
	
	IF @intCategoryID IS NOT NULL
	BEGIN
		SELECT Loc.intLocationID,
		Comp.intCompanyID,
		Comp.strCompanyName,
		Loc.strAddress,
		Loc.strCity,
		St.strState,
		Loc.strZip,
		Kind.strCategory
		FROM db_owner.tblCategoryLocation As Catloc
		JOIN db_owner.tblLocation AS Loc
		ON Catloc.intLocationID = Loc.intLocationID
		JOIN db_owner.tblState As St
		ON St.intStateID = Loc.intStateID
		JOIN db_owner.tblCategory AS Kind
		ON Kind.intCategoryID = Catloc.intCategoryID
		JOIN db_owner.tblCompany AS Comp
		ON Comp.intCompanyID = Loc.intCompanyID
		WHERE Catloc.intCategoryID = @intCategoryID;
	END
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [db_owner].[SELECT_STATES]
AS
BEGIN
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM tblState
END
GO

CREATE PROCEDURE [dbo].[DELETE_LOCATION]
@lngLocationID AS BIGINT = 1
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM db_owner.tblCategoryLocation WHERE intLocationID = @lngLocationID
	DELETE FROM db_owner.tblEventLocation WHERE intLocationID = @lngLocationID
	DELETE FROM db_owner.tblLocationHours WHERE intLocationID = @lngLocationID
	DELETE FROM db_owner.tblContactPerson WHERE intLocationID = @lngLocationID
	DELETE FROM db_owner.tblSpecialLocation WHERE intLocationID = @lngLocationID
	DELETE FROM db_owner.tblLocation WHERE intLocationID = @lngLocationID

	RETURN @@ROWCOUNT
END
GO

CREATE PROCEDURE [dbo].[INSERT_NEW_MAIN_BANNER]
@intNewBannerID SMALLINT = null OUTPUT, 
@strBanner NVARCHAR(2000)
AS
SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN
	INSERT INTO [db_owner].[tblMainBanner]
		([strBanner])
	VALUES	
		(@strBanner)
	SELECT @intNewBannerID=@@IDENTITY
	RETURN 1
END
GO


CREATE PROCEDURE [dbo].[REUSE_MAIN_BANNER]
@intMainBannerID SMALLINT,
@intNewBannerID	SMALLINT = null OUTPUT,
@strBanner NVARCHAR(2000)
AS
SET NOCOUNT ON 
SET XACT_ABORT ON 
BEGIN
	SELECT @strBanner=strBanner FROM db_owner.tblMainBanner WHERE intMainBannerID = @intMainBannerID

	INSERT INTO [db_owner].[tblMainBanner] WITH (TABLOCKX)
					([strBanner])	
	VALUES			(@strBanner)

	SELECT @intNewBannerID=@@IDENTITY
	RETURN 1
END
GO

CREATE PROCEDURE [db_owner].[DELETE_COMPANY]
@intCompanyID BIGINT
AS
SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN
	
	DELETE FROM tblCompanyMember WHERE intCompanyID = @intCompanyID 
	DELETE FROM tblCategoryLocation WHERE intLocationID IN (SELECT intLocationID FROM tblLocation WHERE intCompanyID = @intCompanyID)
	DELETE FROM tblLocationHours WHERE intLocationID IN (SELECT intLocationID FROM  tblLocation WHERE intCompanyID = @intCompanyID)
	DELETE FROM tblLocation WHERE intCompanyID = @intCompanyID
	DELETE FROM tblCompanyAward WHERE intCompanyID = @intCompanyID
	DELETE FROM tblCompanySocialMedia WHERE intCompanyID = @intCompanyID
	DELETE FROM tblContactPerson WHERE intCompanyID = @intCompanyID
	DELETE FROM tblWebsite WHERE intCompanyID = @intCompanyID
	DELETE FROM tblCompany WHERE intCompanyID = @intCompanyID
	RETURN @@rowcount
END
GO

-- -----------------------------------------------------------------------------------------
-- ADD TEST DATA
-- -----------------------------------------------------------------------------------------

INSERT INTO tblContactPersonType (strContactPersonType)
VALUES			('Location Contact')
				,('Web Admin')
				,('Customer Service Representative')

INSERT INTO tblState (strState)
VALUES	('Indiana'),
	('Kentucky'),
	('Ohio')

INSERT INTO tblMemberLevel (strMemberLevel)
VALUES	('Associate'),
	('Business'),
	('Allied')

INSERT INTO tblPaymentType (strPaymentType)
VALUES	('Zelle'),
	('Check')

INSERT INTO tblCategory (strCategory)
VALUES	('Donuts'), 
	('Bagels'),
	('Muffins'),
	('Ice Cream'),
	('Fine Candies and Chocolates'),
	('Wedding Cakes'),
	('Breads'),
	('Decorated Cakes'),
	('Cupcakes'),
	('Cookies'),
	('Desserts/Tortes'),
	('Full-line Bakery'),
	('Deli/Catering'),
	('Other'),
	('Wholesale'),
	('Delivery (3rd Party)'),
	('Shipping'),
	('Online Ordering')

INSERT INTO tblDay (strDay)
VALUES	('Monday'),
	('Tuesday'),
	('Wednesday'),
	('Thursday'),
	('Friday'),
	('Saturday'),
	('Sunday')

-- COMPANY INFORMATION FOR THE BONBONERIE
INSERT INTO tblCompany (strCompanyName, strAbout, strBizYear)
VALUES	('The Bonbonerie', 'At BonBonerie, our rule is that everything we make must be two things: beautiful and delicious. That means using quality ingredients like sweet cream butter, cane sugar, fresh lemon juice and zest, Belgian chocolate, and real vanilla from Madagascar. We create everything by hand in the BonBonerie kitchens, including all our doughs, icings, syrups, batters, and fillings.<br /><br />Each of our original recipes have been reworked and refined over years to create pastry that is unique, perfected, and delicious.<br /><br />Our extremely talented staff of bakers and decorators can customize almost anything for your special event. From astonishing cake centerpieces to hand-cut cookies, everything is crafted with the utmost care by true artists in their field.<br /><br />We are proud to be your award-winning choice for all pastries Beautiful and Delicious since 1983.', '1963'),
	('Wyoming Pastry Shop', 'Welcome to Wyoming Pastry Shop<br /><br />Our bakery has been serving the Village of Wyoming and surrounding areas since 1934. Phillip and Kimberly Reschke are the fifth owners of this hometown bakery. Phillip is a second generation Master Baker, working with his father, a Master Baker from Germany, he has learned all aspects of the bakery and pastry trade and takes pride in all of the products he crafts. Kimberly has been decorating cakes for 30 years, spending time in Cincinnati and Las Vegas perfecting her skills and creativity in a variety of pastry and cake design. We strive for complete customer satisfaction. We want you to think of us whenever you have a craving for something sweet, a special cake or cookie, or one of our many products we offer. We are a small business and will keep it small so we can control our quality to provide the best product to you.', '1972'),
	('Servatii Pastry Shop', 'Who We Are and What We Do<br /><br />Wilhelm Gottenbusch, a German immigrant, came to the United States after traveling around the world on an international freighter. In 1963, Wilhelm opened his first bakery on Observatory Avenue in Hyde Park. In a one man shop, with a bakery in the back, he made a name for himself by focusing on one thing - quality. Focusong on the quality of his products, Wilhelm and his sons have expanded and grown over the last 50 years. Wilhelm brought the traditions of his father and grandfather over from Muenster, Germany. His grandfather, George, started out driving a horse drawn wagon door to door selling his fresh baked goods. Wilhelm''s father, George, attended Germany''s most recognizable baking school and received his "Konditor Meister" status - Master Pastry Chef. His father opened Cafe Servatii, on Servatii Platz, in the hear of Muenster, Germany. Wilhelm followed in his father''s footsteps by earning his "Master Baker" status and starting his own business in Cincinnati, Ohio.<br /><br />Wilhelm''s sons Gary and Greg have both apprenticed in Germany earning their journeyman status - Greg in pastry and Gary in baking. Gary continued in his father and grandfather''s footsteps by earning his Master Baker certification in 2001. In turn, the Gottenbusch''s have acquired some of the most talented bakers, decorators and pastry chefs in America. All together they work to uphold the values set forth by Wilhelm Gottenbusch 50 years ago.<br /><br />Today Gary runs a pretzel company from Germany called Pretzel Baron and Greg Gottenbusch runs the day to day business of Servatii. Mr. Gottenbusch still comes in and does his quality checks and keeps everyone in line. Paul, Gary''s son, is now the head night baker learning the family business.<br /><br />The future is bright and we appreciate all of our customers who have made Servatii their tradition.', '1963')

INSERT INTO tblCompanyAward (intCompanyID, strFrom, strAward)
VALUES	(1, 'Best of City Search', 'Best of City'),
	(1, 'CityBeat Best of Cincinnati', 'Best Wedding'),
	(1, 'CityBeat Best of Cincinnati', 'Best Desserts'),
	(1, 'CityBeat Best of Cincinnati', 'Best Bakery'),
	(1, 'CityBeat Best of Cincinnati', 'Staff Pick - Best Expansion of Business'),
	(1, 'CityBeat Best of Cincinnati', 'Best Neighborhood Bakery'),
	(1, 'Cincinnati Magazine Best of the City', 'Best Torte: Opera Cream'),
	(1, 'Cincinnati Magazine Best of the City', 'Hall of Fame Best Bakery & Best Scones'),
	(1, 'Cincinnati Magazine Best of the City', 'Best Layer Cake'),
	(1, 'Cincinnati Magazine Best of the City', 'Best Afternoon Escape'), 
	(1, 'Cincinnati Magazine Best of the City', 'Best Bakery, Reader''s Choice Best Local Flavor'), 
	(1, 'Cincinnati Magazine Best of the City', 'Hall of Fame'), 
	(1, 'Cincinnati Magazine Best of the City', 'Best Tea Time'),
	(1, 'The Knot', 'Best of Wedding'),
	(1, 'The Knot', 'Where the Locals Eat'), 
	(1, 'The Knot', 'Best Dessert'), 
	(1, 'Trip Advisor', 'Certificate of Excellence'), 
	(1, 'Cincinnati Chamber of Commerce', 'Small Business Award Winner')

INSERT INTO tblContactPerson (strContactName, strContactPhone, strContactEmail, intContactPersonTypeID, intCompanyID)
VALUES					('Briggs, Randall', '5555555555', 'briggs.r@gmail.com', 1, 1)

INSERT INTO tblLocation (intCompanyID, strAddress, strCity, intStateID, strZip, strPhone, strEmail)
VALUES	(1, '2030 Madison Rd', 'Cincinnati', 3, '45208-3289', '513-321-3399', 'customerservice@bonbonerie.com'),
	(2, '505 Wyoming Ave', 'Wyoming', 3, '45215-4578', '513-821-0742', 'reschke@wyomingpastryshop.com'),
	(3, '3824 Paxton Ave', 'Cincinnati', 3, '45209-2399', '513-871-3244', 'servatiipastryshop@gmail.com'),
	(3, '2010 Anderson Ferry Rd', 'Cincinnati', 3, '45238-3398', '513-922-0033', 'servatiipastryshop@gmail.com')

INSERT INTO tblLocationHours (intLocationID, intDayID, strOpen, strClose)
VALUES	(1, 1, '10:00am', '4:00pm'),
	(1, 2, '10:00am', '4:00pm'),
	(1, 3, '10:00am', '4:00pm'),
	(1, 4, '10:00am', '4:00pm'),
	(1, 5, '10:00am', '4:00pm'),
	(1, 6, '10:00am', '4:00pm'),
	(1, 7, 'Closed (except for private parties - call for details)', null),
	(2, 1, '6:00am', '6:00pm'),
	(2, 2, '6:00am', '6:00pm'),
	(2, 3, '6:00am', '6:00pm'),
	(2, 4, '6:00am', '6:00pm'),
	(2, 5, '6:00am', '6:00pm'),
	(2, 6, '6:00am', '4:00pm'),
	(2, 7, 'Closed', null),
	(3, 1, 'Closed', null),
	(3, 2, '7:00am', '3:00pm'),
	(3, 3, '7:00am', '3:00pm'),
	(3, 4, '7:00am', '3:00pm'),
	(3, 5, '7:00am', '3:00pm'),
	(3, 6, '7:00am', '3:00pm'),
	(3, 7, '7:30am', '2:00pm'),
	(4, 1, '6:30am', '6:30pm'),
	(4, 2, '6:30am', '6:30pm'),
	(4, 3, '6:30am', '6:30pm'),
	(4, 4, '6:30am', '6:30pm'),
	(4, 5, '6:30am', '6:30pm'),
	(4, 6, '6:30am', '5:30pm'),
	(4, 7, '7:30am', '2:00pm')

INSERT INTO tblWebsiteType(strWebsiteType)
VALUES				('Main')
					,('Ordering')
					,('Kettle')

INSERT INTO tblSocialMedia (strPlatform)
VALUES		('Facebook')
			,('Instagram')
			,('Snapchat')
			,('TikTok')
			,('Twitter')
			,('Yelp')

INSERT INTO tblWebsite (intCompanyID, strURL, intWebsiteTypeID)
VALUES		(3, 'https://give.salvationarmy.org/team/353650', 3),
			(3, 'https://www.servatii.com/online-orders', 2),
			(2, 'http://www.wyomingpastryshop.com/', 1),
			(1, 'https://www.bonbonerie.com/', 1),
			(1, 'https://cincyfavorites.com/shop/bonbonerie-bakery/', 2)

INSERT INTO tblCompanySocialMedia (strSocialMediaLink, intCompanyID, intSocialMediaID)
VALUES		('https://twitter.com/servatiipastry', 3, 5),
			('https://facebook.com/servatii', 3, 1),
			('https://www.instagram.com/servatiipastry/?hl=en', 3, 2),
			('https://mobile.twitter.com/wyomingpastry', 2, 5),
			('https://www.facebook.com/Wyoming-Pastry-Shop-104461259599883/', 2, 1),
			('https://www.instagram.com/wyomingpastryshop/?hl=en', 2, 2),
			('https://www.yelp.com/biz/wyoming-pastry-shop-cincinnati', 2, 6),
			('https://twitter.com/bonbonerie', 1, 5),
			('https://www.facebook.com/bonbonerie?ref=m', 1, 1),
			('https://www.instagram.com/bonboneriecincy/', 1, 2),
			('https://www.yelp.ca/biz/the-bonbonerie-cincinnati-2', 1, 6)

-- NON-ADMIN USER 
INSERT INTO tblUser (strFirstName, strLastName, strAddress, strCity, intStateID, strZip, strPhone, strEmail, strUsername, strPassword, isAdmin)
VALUES	('Katie', 'Schmidt', '6036 Flyer Drive', 'Cincinnati', 3, '45248', '5133103965', 'klschmidt16178@cincinnatistate.edu', 'test2', 'test2', 0),
		('Random', 'User', '1234 Main St', 'Lawrenceburg', 1, '41010', '5135555555', 'random_user@gmail.com', 'test3', 'test3', 0)

-- ADMIN USER 
INSERT INTO tblUser (strFirstName, strLastName, strAddress, strCity, intStateID, strZip, strPhone, strEmail, strUsername, strPassword, isAdmin)
VALUES	('Grace', 'Gottenbusch', '123 Elm St', 'Covington', 2, '41212', '5135555555', 'grace@gmail.com', 'grace', 'grace', 1)

-- ADD USER TO MEMBER TABLE 
INSERT INTO  tblMember (intUserID, intMemberLevelID, intPaymentTypeID)
VALUES	(1, 1, 2)

INSERT INTO tblMainBanner (strBanner)
VALUES	('This is an example of the main banner. This will hold information relevant to the GCRBA.'),
		('This is an example of the most up-to-date banner in this database. This will hold information relevant to the GCRBA')

INSERT INTO tblCategoryLocation (intCategoryID, intLocationID, blnAvailable)
VALUES		(6, 1, 1),
			(8, 1, 1),
			(9, 1, 1),
			(10, 1, 1), 
			(11, 1, 1),
			(13,  1, 1),
			(15, 1, 1),
			(1, 2, 1),
			(6, 2, 1),
			(7, 2, 1),
			(8, 2, 1),
			(9, 2, 1),
			(10, 2, 1),
			(11, 2, 1),
			(12, 2, 1),
			(1, 3, 1),
			(2, 3, 1),
			(3, 3, 1),
			(6, 3, 1),
			(7, 3, 1),
			(8, 3, 1),
			(9, 3, 1),
			(10, 3, 1),
			(11, 3, 1),
			(12, 3, 1),
			(13, 3, 1),
			(15, 3, 1),
			(16, 3, 1),
			(1, 4, 1),
			(2, 4, 1),
			(3, 4, 1),
			(6, 4, 1),
			(7, 4, 1),
			(8, 4, 1),
			(9, 4, 1),
			(10, 4, 1),
			(11, 4, 1),
			(12, 4, 1),
			(13, 4, 1),
			(15, 4, 1),
			(16, 4, 1)
