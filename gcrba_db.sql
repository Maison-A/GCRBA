IF OBJECT_ID('tblSpecialLocation')		IS NOT NULL DROP TABLE tblSpecialLocation 
IF OBJECT_ID('tblCompanyAward')			IS NOT NULL DROP TABLE tblCompanyAward
IF OBJECT_ID('tblLocationHours')		IS NOT NULL DROP TABLE tblLocationHours
IF OBJECT_ID('tblCompanyMember')		IS NOT NULL DROP TABLE tblCompanyMember
IF OBJECT_ID('tblCategoryLocation')		IS NOT NULL DROP TABLE tblCategoryLocation 
IF OBJECT_ID('tblEventLocation')		IS NOT NULL DROP TABLE tblEventLocation 
IF OBJECT_ID('tblSpecial')				IS NOT NULL DROP TABLE tblSpecial 
IF OBJECT_ID('tblCategory')				IS NOT NULL DROP TABLE tblCategory
IF OBJECT_ID('tblEvent')				IS NOT NULL DROP TABLE tblEvent
IF OBJECT_ID('tblLocation')				IS NOT NULL DROP TABLE tblLocation 
IF OBJECT_ID('tblContactPerson')		IS NOT NULL DROP TABLE tblContactPerson
IF OBJECT_ID('tblAdminRequest')			IS NOT NULL DROP TABLE tblAdminRequest  
IF OBJECT_ID('tblCompanySocialMedia')	IS NOT NULL DROP TABLE tblCompanySocialMedia
IF OBJECT_ID('tblWebsite')				IS NOT NULL DROP TABLE tblWebsite 
IF OBJECT_ID('tblWebsiteType')			IS NOT NULL DROP TABLE tblWebsiteType
IF OBJECT_ID('tblCompany')				IS NOT NULL DROP TABLE tblCompany 
IF OBJECT_ID('tblSocialMedia')			IS NOT NULL DROP TABLE tblSocialMedia
IF OBJECT_ID('tblMember')				IS NOT NULL DROP TABLE tblMember 
IF OBJECT_ID('tblMemberLevel')			IS NOT NULL DROP TABLE tblMemberLevel 
IF OBJECT_ID('tblPaymentType')			IS NOT NULL DROP TABLE tblPaymentType 
IF OBJECT_ID('tblApprovalStatus')		IS NOT NULL DROP TABLE tblApprovalStatus 
IF OBJECT_ID('tblUser')					IS NOT NULL DROP TABLE tblUser 
IF OBJECT_ID('tblState')				IS NOT NULL DROP TABLE tblState 
IF OBJECT_ID('tblDay')					IS NOT NULL DROP TABLE tblDay
IF OBJECT_ID('tblMainBanner')			IS NOT NULL DROP TABLE tblMainBanner
IF OBJECT_ID('tblAboutGCRBA')			IS NOT NULL DROP TABLE tblAboutGCRBA
IF OBJECT_ID('LOGIN')					IS NOT NULL DROP PROCEDURE LOGIN 
IF OBJECT_ID('VERIFY_MEMBER')			IS NOT NULL DROP PROCEDURE VERIFY_MEMBER


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
	strAbout			NVARCHAR(2000),
	strWebAdminName		NVARCHAR(100),
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
	intContactPersonID	BIGINT				NOT NULL,
	CONSTRAINT tblLocation_PK PRIMARY KEY (intLocationID)
)

CREATE TABLE tblContactPerson
(
	intContactPersonID		BIGINT IDENTITY(1,1) NOT NULL,
	strContactName		NVARCHAR(50)		NOT NULL,
	strContactPhone			NVARCHAR(20)		,
	strContactEmail			NVARCHAR(50)		,
	CONSTRAINT tblContactPerson_PK PRIMARY KEY (intContactPersonID)
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
	intWebsiteID			SMALLINT IDENTITY(1,1)		NOT NULL, 
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

-- CHILD				PARENT				COLUMN(s)
-- -----				-----				------
-- tblUser				tblState			intStateID
-- tblMember				tblUser				intUserID
-- tblMember				tblMemberLevel			intMemberLevelID
-- tblMember				tblPaymentType			intPaymentTypeID
-- tblCompanyMember			tblCompany			intCompanyID
-- tblCompanyMember			tblMember			intMemberID
-- tblLocation				tblCompany			intCompanyID
-- tblLocation				tblState			intStateID
-- tblCategoryLocation			tblCategory			intCategoryID
-- tblCategoryLocation			tblLocation			intLocationID
-- tblEventLocation			tblEvent			intEventID
-- tblEventLocation			tblLocation			intLocationID
-- tblAdminRequest			tblMember			intMemberID
-- tblAdminRequest			tblApprovalStatus		intApprovalStatusID
-- tblSpecialLocation			tblSpecial			intSpecialID
-- tblSpecialLocation			tblLocation			intLocationID
-- tblLocationHours			tblLocation			intLocationID
-- tblLocationHours			tblDay				intDayID
-- tblCompanyAward			tblCompany			intCompanyID
-- tblLocation				tblContactPerson	intContactPersonID
-- tblCompanySocialMedia	tblCompany			intCompanyID
-- tblCompanySocialMedia	tblSocialMedia		intSocialMedia

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

ALTER TABLE tblLocation ADD CONSTRAINT tblLocation_tblContactPerson_FK
FOREIGN KEY (intContactPersonID) REFERENCES tblContactPerson (intContactPersonID)

ALTER TABLE tblCompanySocialMedia ADD CONSTRAINT tblCompanySocialMedia_tblCompany_FK
FOREIGN KEY (intCompanyID) REFERENCES tblCompany (intCompanyID)

ALTER TABLE tblCompanySocialMedia ADD CONSTRAINT tblCompanySocialMedia_tblSocialMedia_FK
FOREIGN KEY (intSocialMediaID) REFERENCES tblSocialMedia (intSocialMediaID)

ALTER TABLE tblWebsite ADD CONSTRAINT tblWebsite_tblWebsiteType_FK
FOREIGN KEY (intWebsiteTypeID) REFERENCES tblWebsiteType (intWebsiteTypeID)

ALTER TABLE tblWebsite ADD CONSTRAINT tblWebsite_tblCompany_FK
FOREIGN KEY (intCompanyID) REFERENCES tblCompany (intCompanyID)

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

-- -----------------------------------------------------------------------------------------
-- ADD TEST DATA
-- -----------------------------------------------------------------------------------------

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
INSERT INTO tblCompany (strCompanyName, strAbout)
VALUES	('The Bonbonerie', 'In Business Since 1983<br /><br />At BonBonerie, our rule is that everything we make must be two things: beautiful and delicious. That means using quality ingredients like sweet cream butter, cane sugar, fresh lemon juice and zest, Belgian chocolate, and real vanilla from Madagascar. We create everything by hand in the BonBonerie kitchens, including all our doughs, icings, syrups, batters, and fillings.<br /><br />Each of our original recipes have been reworked and refined over years to create pastry that is unique, perfected, and delicious.<br /><br />Our extremely talented staff of bakers and decorators can customize almost anything for your special event. From astonishing cake centerpieces to hand-cut cookies, everything is crafted with the utmost care by true artists in their field.<br /><br />We are proud to be your award-winning choice for all pastries Beautiful and Delicious since 1983.'),
	('Wyoming Pastry Shop', 'In Business Since 1934<br /><br />Welcome to Wyoming Pastry Shop<br /><br />Our bakery has been serving the Village of Wyoming and surrounding areas since 1934. Phillip and Kimberly Reschke are the fifth owners of this hometown bakery. Phillip is a second generation Master Baker, working with his father, a Master Baker from Germany, he has learned all aspects of the bakery and pastry trade and takes pride in all of the products he crafts. Kimberly has been decorating cakes for 30 years, spending time in Cincinnati and Las Vegas perfecting her skills and creativity in a variety of pastry and cake design. We strive for complete customer satisfaction. We want you to think of us whenever you have a craving for something sweet, a special cake or cookie, or one of our many products we offer. We are a small business and will keep it small so we can control our quality to provide the best product to you.')

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

INSERT INTO tblContactPerson (strContactName, strContactPhone, strContactEmail)
VALUES					('Briggs, Randall', '5555555555', 'briggs.r@gmail.com')

INSERT INTO tblLocation (intCompanyID, strAddress, strCity, intStateID, strZip, strPhone, intContactPersonID)
VALUES	(1, '2030 Madison Rd', 'Cincinnati', 3, '45208-3289', '513-321-3399', 1),
	(2, '505 Wyoming Ave', 'Wyoming', 3, '45215-4578', '513-821-0742', 1)

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
	(2, 7, 'Closed', null)

INSERT INTO tblWebsiteType(strWebsiteType)
VALUES				('Main')
					,('Ordering')
					,('Kettle')

INSERT INTO tblWebsite (intCompanyID, strURL, intWebsiteTypeID)
VALUES	(1, 'https://twitter.com/bonbonerie', 1),
	(1, 'https://www.facebook.com/bonbonerie?ref=m', 1),
	(1, 'https://www.instagram.com/bonboneriecincy/', 2),
	(1, 'https://www.yelp.ca/biz/the-bonbonerie-cincinnati-2', 2),
	(2, 'https://mobile.twitter.com/wyomingpastry', 3),
	(2, 'https://www.facebook.com/Wyoming-Pastry-Shop-104461259599883/', 2),
	(2, 'https://www.instagram.com/wyomingpastryshop/?hl=en', 1),
	(2, 'https://www.yelp.com/biz/wyoming-pastry-shop-cincinnati', 1)

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
VALUES	('This is an example of the main banner. This will hold information relevant to the GCRBA.')

INSERT INTO tblSocialMedia (strPlatform)
VALUES		('Facebook')
			,('Instagram')
			,('Snapchat')
			,('TikTok')
			,('Twitter')
			,('Yelp')

