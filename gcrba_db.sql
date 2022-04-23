-- DROP TABLES
IF OBJECT_ID('tblSpecialLocation')			IS NOT NULL DROP TABLE tblSpecialLocation 
IF OBJECT_ID('tblPaymentStatus')			IS NOT NULL DROP TABLE tblPaymentStatus 
IF OBJECT_ID('tblMembershipRequest')		IS NOT NULL DROP TABLE tblMembershipRequest 
IF OBJECT_ID('tblCompanyAward')				IS NOT NULL DROP TABLE tblCompanyAward
IF OBJECT_ID('tblLocationHours')			IS NOT NULL DROP TABLE tblLocationHours
IF OBJECT_ID('tblTempLocationHours')		IS NOT NULL DROP TABLE tblTempLocationHours   
IF OBJECT_ID('tblCompanyMember')			IS NOT NULL DROP TABLE tblCompanyMember
IF OBJECT_ID('tblCategoryLocation')			IS NOT NULL DROP TABLE tblCategoryLocation 
IF OBJECT_ID('tblTempContactLocation')		IS NOT NULL DROP TABLE tblTempContactLocation
IF OBJECT_ID('tblTempCategoryLocation')		IS NOT NULL DROP TABLE tblTempCategoryLocation
IF OBJECT_ID('tblEventLocation')			IS NOT NULL DROP TABLE tblEventLocation 
IF OBJECT_ID('tblSpecial')					IS NOT NULL DROP TABLE tblSpecial 
IF OBJECT_ID('tblCategory')					IS NOT NULL DROP TABLE tblCategory
IF OBJECT_ID('tblTempCategory')				IS NOT NULL DROP TABLE tblTempCategory
IF OBJECT_ID('tblEvent')					IS NOT NULL DROP TABLE tblEvent
IF OBJECT_ID('tblContactLocation')			IS NOT NULL DROP TABLE tblContactLocation
IF OBJECT_ID('tblContactPerson')			IS NOT NULL DROP TABLE tblContactPerson
IF OBJECT_ID('tblTempContactPerson')		IS NOT NULL DROP TABLE tblTempContactPerson
IF OBJECT_ID('tblLocation')					IS NOT NULL DROP TABLE tblLocation 
IF OBJECT_ID('tblTempLocation')				IS NOT NULL DROP TABLE tblTempLocation 
IF OBJECT_ID('tblAdminRequest')				IS NOT NULL DROP TABLE tblAdminRequest  
IF OBJECT_ID('tblCompanySocialMedia')		IS NOT NULL DROP TABLE tblCompanySocialMedia
IF OBJECT_ID('tblTempCompanySocialMedia')	IS NOT NULL DROP TABLE tblTempCompanySocialMedia
IF OBJECT_ID('tblWebsite')					IS NOT NULL DROP TABLE tblWebsite 
IF OBJECT_ID('tblTempWebsite')				IS NOT NULL DROP TABLE tblTempWebsite 
IF OBJECT_ID('tblWebsiteType')				IS NOT NULL DROP TABLE tblWebsiteType
IF OBJECT_ID('tblTempWebsiteType')			IS NOT NULL DROP TABLE tblTempWebsiteType
IF OBJECT_ID('tblCompany')					IS NOT NULL DROP TABLE tblCompany 
IF OBJECT_ID('tblTempCompany')				IS NOT NULL DROP TABLE tblTempCompany     
IF OBJECT_ID('tblContactPersonType')		IS NOT NULL DROP TABLE tblContactPersonType
IF OBJECT_ID('tblTempContactPersonType')	IS NOT NULL DROP TABLE tblTempContactPersonType
IF OBJECT_ID('tblSocialMedia')				IS NOT NULL DROP TABLE tblSocialMedia
IF OBJECT_ID('tblTempSocialMedia')			IS NOT NULL DROP TABLE tblTempSocialMedia
IF OBJECT_ID('tblMember')					IS NOT NULL DROP TABLE tblMember 
IF OBJECT_ID('tblMemberLevel')				IS NOT NULL DROP TABLE tblMemberLevel 
IF OBJECT_ID('tblPaymentType')				IS NOT NULL DROP TABLE tblPaymentType 
IF OBJECT_ID('tblApprovalStatus')			IS NOT NULL DROP TABLE tblApprovalStatus 
IF OBJECT_ID('tblDay')						IS NOT NULL DROP TABLE tblDay
IF OBJECT_ID('tblTempDay')					IS NOT NULL DROP TABLE tblTempDay
IF OBJECT_ID('tblMainBanner')				IS NOT NULL DROP TABLE tblMainBanner
IF OBJECT_ID('tblAboutGCRBA')				IS NOT NULL DROP TABLE tblAboutGCRBA
IF OBJECT_ID('tblUser')						IS NOT NULL DROP TABLE tblUser 
IF OBJECT_ID('tblState')					IS NOT NULL DROP TABLE tblState 
IF OBJECT_ID('tblTempState')				IS NOT NULL DROP TABLE tblTempState 

--DROP STORED PROCEDURES
IF OBJECT_ID('LOGIN')								IS NOT NULL DROP PROCEDURE LOGIN 
IF OBJECT_ID('VERIFY_MEMBER')						IS NOT NULL DROP PROCEDURE VERIFY_MEMBER
IF OBJECT_ID('INSERT_WEBSITE')						IS NOT NULL DROP PROCEDURE INSERT_WEBSITE
IF OBJECT_ID('INSERT_CONTACTPERSON')				IS NOT NULL DROP PROCEDURE INSERT_CONTACTPERSON
IF OBJECT_ID('INSERT_SOCIALMEDIA')					IS NOT NULL DROP PROCEDURE INSERT_SOCIALMEDIA
IF OBJECT_ID('INSERT_LOCATION')						IS NOT NULL DROP PROCEDURE INSERT_LOCATION
IF OBJECT_ID('INSERT_COMPANY')						IS NOT NULL DROP PROCEDURE INSERT_COMPANY
IF OBJECT_ID('INSERT_CATEGORYLOCATION')				IS NOT NULL DROP PROCEDURE INSERT_CATEGORYLOCATION
IF OBJECT_ID('INSERT_LOCATIONHOURS')				IS NOT NULL DROP PROCEDURE INSERT_LOCATIONHOURS
IF OBJECT_ID('GET_MAIN_BANNER')						IS NOT NULL DROP PROCEDURE GET_MAIN_BANNER
IF OBJECT_ID('GET_COMPANY_INFO')					IS NOT NULL DROP PROCEDURE GET_COMPANY_INFO
IF OBJECT_ID('GET_ALL_MAIN_BANNERS')				IS NOT NULL DROP PROCEDURE GET_ALL_MAIN_BANNERS
IF OBJECT_ID('SELECT_LOCATION')						IS NOT NULL DROP PROCEDURE SELECT_LOCATION
IF OBJECT_ID('INSERT_NEW_USER')						IS NOT NULL	DROP PROCEDURE INSERT_NEW_USER
IF OBJECT_ID('SELECT_STATES')						IS NOT NULL	DROP PROCEDURE SELECT_STATES
IF OBJECT_ID('DELETE_LOCATION')						IS NOT NULL	DROP PROCEDURE DELETE_LOCATION
IF OBJECT_ID('INSERT_NEW_MAIN_BANNER')				IS NOT NULL	DROP PROCEDURE INSERT_NEW_MAIN_BANNER
IF OBJECT_ID('REUSE_MAIN_BANNER')					IS NOT NULL	DROP PROCEDURE REUSE_MAIN_BANNER
IF OBJECT_ID('DELETE_COMPANY')						IS NOT NULL	DROP PROCEDURE DELETE_COMPANY
IF OBJECT_ID('GET_LOCATIONS')						IS NOT NULL	DROP PROCEDURE GET_LOCATIONS
IF OBJECT_ID('GET_SPECIFIC_COMPANY')				IS NOT NULL DROP PROCEDURE GET_SPECIFIC_COMPANY
IF OBJECT_ID('SELECT_ALLCATEGORY_FORLOCATION') 		IS NOT NULL DROP PROCEDURE SELECT_ALLCATEGORY_FORLOCATION
IF OBJECT_ID('SELECT_LOCATION_BYCATEGORY')			IS NOT NULL DROP PROCEDURE SELECT_LOCATION_BYCATEGORY
IF OBJECT_ID ('SELECT_LOCATION_SPECIALS')			IS NOT NULL DROP PROCEDURE SELECT_LOCATION_SPECIALS
IF OBJECT_ID('SELECT_LOCATION_CONTACTS') 			IS NOT NULL DROP PROCEDURE SELECT_LOCATION_CONTACTS
IF OBJECT_ID('SELECT_LOCATION_SOCIALMEDIA') 		IS NOT NULL DROP PROCEDURE SELECT_LOCATION_SOCIALMEDIA
IF OBJECT_ID('SELECT_LOCATION_WEBSITE') 			IS NOT NULL DROP PROCEDURE SELECT_LOCATION_WEBSITE
IF OBJECT_ID('SELECT_USERLOCATION_ASSOCIATION') 	IS NOT NULL DROP PROCEDURE SELECT_USERLOCATION_ASSOCIATION
IF OBJECT_ID('GET_LOCATIONS_NOT_CONTACT') 			IS NOT NULL DROP PROCEDURE GET_LOCATIONS_NOT_CONTACT
IF OBJECT_ID('GET_CONTACTS_BY_COMPANY') 			IS NOT NULL DROP PROCEDURE GET_CONTACTS_BY_COMPANY
IF OBJECT_ID('GET_NOT_CATEGORIES') 					IS NOT NULL DROP PROCEDURE GET_NOT_CATEGORIES
IF OBJECT_ID('GET_CURRENT_CATEGORIES') 				IS NOT NULL DROP PROCEDURE GET_CURRENT_CATEGORIES
IF OBJECT_ID('DELETE_CATEGORY_FROM_LOCATION') 		IS NOT NULL DROP PROCEDURE DELETE_CATEGORY_FROM_LOCATION
IF OBJECT_ID('INSERT_SPECIALLOCATION') 				IS NOT NULL DROP PROCEDURE INSERT_SPECIALLOCATION
IF OBJECT_ID('INSERT_SPECIAL') 						IS NOT NULL DROP PROCEDURE INSERT_SPECIAL
IF OBJECT_ID('DELETE_SPECIALLOCATION') 				IS NOT NULL DROP PROCEDURE DELETE_SPECIALLOCATION
IF OBJECT_ID('GET_TOTAL_REQUESTS') 					IS NOT NULL DROP PROCEDURE GET_TOTAL_REQUESTS
IF OBJECT_ID('UPDATE_USER') 						IS NOT NULL DROP PROCEDURE UPDATE_USER 
IF OBJECT_ID('INSERT_TEMP_WEBSITE')					IS NOT NULL DROP PROCEDURE INSERT_TEMP_WEBSITE
IF OBJECT_ID('INSERT_TEMP_CONTACTPERSON')			IS NOT NULL DROP PROCEDURE INSERT_TEMP_CONTACTPERSON
IF OBJECT_ID('INSERT_TEMP_SOCIALMEDIA')				IS NOT NULL DROP PROCEDURE INSERT_TEMP_SOCIALMEDIA
IF OBJECT_ID('INSERT_TEMP_LOCATION')				IS NOT NULL DROP PROCEDURE INSERT_TEMP_LOCATION
IF OBJECT_ID('INSERT_TEMP_COMPANY')					IS NOT NULL DROP PROCEDURE INSERT_TEMP_COMPANY   
IF OBJECT_ID('INSERT_TEMP_CATEGORYLOCATION')		IS NOT NULL DROP PROCEDURE INSERT_TEMP_CATEGORYLOCATION
IF OBJECT_ID('INSERT_TEMP_LOCATIONHOURS')			IS NOT NULL DROP PROCEDURE INSERT_TEMP_LOCATIONHOURS
IF OBJECT_ID('INSERT_ADMIN_REQUEST')				IS NOT NULL DROP PROCEDURE INSERT_ADMIN_REQUEST
IF OBJECT_ID('INSERT_CONTACTLOCATION_RELATIONSHIP') IS NOT NULL DROP PROCEDURE INSERT_CONTACTLOCATION_RELATIONSHIP
IF OBJECT_ID('INSERT_TEMP_CONTACTLOCATION')			IS NOT NULL DROP PROCEDURE INSERT_TEMP_CONTACTLOCATION
IF OBJECT_ID('SELECT_ADMINREQUESTS')				IS NOT NULL DROP PROCEDURE SELECT_ADMINREQUESTS
IF OBJECT_ID('SELECT_TEMP_LOCATION')				IS NOT NULL DROP PROCEDURE SELECT_TEMP_LOCATION
IF OBJECT_ID('SELECT_ALLCATEGORY_FORTEMPLOCATION')	IS NOT NULL DROP PROCEDURE SELECT_ALLCATEGORY_FORTEMPLOCATION
IF OBJECT_ID('SELECT_LOCATIONHOURS')				IS NOT NULL DROP PROCEDURE SELECT_LOCATIONHOURS
IF OBJECT_ID('SELECT_TEMP_LOCATIONHOURS')			IS NOT NULL DROP PROCEDURE SELECT_TEMP_LOCATIONHOURS
IF OBJECT_ID('SELECT_TEMP_LOCATION_CONTACTS')		IS NOT NULL DROP PROCEDURE SELECT_TEMP_LOCATION_CONTACTS
IF OBJECT_ID('SELECT_TEMP_LOCATION_SOCIALMEDIA')	IS NOT NULL DROP PROCEDURE SELECT_TEMP_LOCATION_SOCIALMEDIA
IF OBJECT_ID('SELECT_TEMP_LOCATION_WEBSITE')		IS NOT NULL DROP PROCEDURE SELECT_TEMP_LOCATION_WEBSITE
IF OBJECT_ID('CHECK_IF_MEMBERLOCATION')				IS NOT NULL DROP PROC CHECK_IF_MEMBERLOCATION
IF OBJECT_ID ('SELECT_LOCATION_AWARDS')				IS NOT NULL DROP PROCEDURE SELECT_LOCATION_AWARDS
IF OBJECT_ID ('SELECT_SINGLE_ADMINREQUEST')			IS NOT NULL DROP PROCEDURE SELECT_SINGLE_ADMINREQUEST
IF OBJECT_ID ('INSERT_COMPANYMEMBER_RELATIONSHIP')	IS NOT NULL DROP PROCEDURE INSERT_COMPANYMEMBER_RELATIONSHIP
IF OBJECT_ID ('DELETE_TEMP_LOCATION')				IS NOT NULL DROP PROCEDURE DELETE_TEMP_LOCATION
IF OBJECT_ID ('DELETE_ADMIN_REQUEST')				IS NOT NULL DROP PROCEDURE DELETE_ADMIN_REQUEST
IF OBJECT_ID ('GET_MEMBERSHIP_REQUESTS')			IS NOT NULL DROP PROCEDURE GET_MEMBERSHIP_REQUESTS
IF OBJECT_ID ('INSERT_CONTACTLOCATION')				IS NOT NULL DROP PROCEDURE INSERT_CONTACTLOCATION
IF OBJECT_ID('INSERT_TEMP_CONTACTLOCATION_RELATIONSHIP') IS NOT NULL DROP PROCEDURE INSERT_TEMP_CONTACTLOCATION_RELATIONSHIP

CREATE TABLE tblTempCompany   
(
	intCompanyID		BIGINT IDENTITY(1,1)	NOT NULL, 
	strCompanyName		NVARCHAR(50)			NOT NULL, 
	strAbout			NVARCHAR(2000)				NOT NULL, 
	strBizYear			NVARCHAR(10),
	CONSTRAINT tblTempCompany_PK PRIMARY KEY (intCompanyID)
)

CREATE TABLE tblTempLocationHours   
(
	intLocationHoursID	BIGINT IDENTITY(1,1)	NOT NULL, 
	intLocationID		BIGINT					NOT NULL, 
	intDayID			SMALLINT				NOT NULL, 
	strOpen				NVARCHAR(100), 
	strClose			NVARCHAR(100), 
	CONSTRAINT tblTempLocationHours_PK PRIMARY KEY (intLocationHoursID)
)

CREATE TABLE tblTempCompanySocialMedia   
(
	intCompanySocialMediaID BIGINT IDENTITY NOT NULL,
	strSocialMediaLink		NVARCHAR(100)	NOT NULL,
	intCompanyID			BIGINT			NOT NULL,
	intSocialMediaID		SMALLINT		NOT NULL,
	CONSTRAINT tblTempCompanySocialMedia_PK PRIMARY KEY (intCompanySocialMediaID)
)

CREATE TABLE tblTempLocation
(
	intLocationID		BIGINT IDENTITY(1,1)	NOT NULL, 
	intCompanyID		BIGINT					NOT NULL, 
	strAddress			NVARCHAR(100)			NOT NULL, 
	strCity				NVARCHAR(20)			NOT NULL, 
	intStateID			SMALLINT				NOT NULL, 
	strZip				NVARCHAR(15)			NOT NULL,
	strPhone			NVARCHAR(20)			NOT NULL,
	strEmail			NVARCHAR(50),
	intAdminRequestID	SMALLINT				NOT NULL,
	CONSTRAINT tblTempLocation_PK PRIMARY KEY (intLocationID)
)

CREATE TABLE tblTempContactLocation
(
	intContactLocationID BIGINT IDENTITY(1,1)	NOT NULL,
	intLocationID		BIGINT					NOT NULL,
	intContactPersonID		BIGINT 				NOT NULL,
	CONSTRAINT tblTempContactLocation_PK PRIMARY KEY (intContactLocationID)
)

CREATE TABLE tblTempContactPerson
(
	intContactPersonID		BIGINT IDENTITY(1,1)	NOT NULL,
	strContactName		NVARCHAR(50)				NOT NULL,
	strContactPhone			NVARCHAR(20)		,
	strContactEmail			NVARCHAR(50)		,
	intCompanyID			BIGINT				NOT NULL,
	intContactPersonTypeID	SMALLINT			NOT NULL,
	CONSTRAINT tblTempContactPerson_PK PRIMARY KEY (intContactPersonID)
)

CREATE TABLE tblTempCategoryLocation
(
	intCategoryLocationID		BIGINT IDENTITY(1,1)	NOT NULL,
	intCategoryID			SMALLINT		NOT NULL,
	intLocationID			BIGINT			NOT NULL,
	CONSTRAINT tblTempCategoryLocation_PK PRIMARY KEY (intCategoryLocationID)
)

CREATE TABLE tblTempWebsite
(
	intWebsiteID			BIGINT IDENTITY(1,1)		NOT NULL, 
	intCompanyID			BIGINT				NOT NULL,
	strURL				NVARCHAR(100)			NOT NULL, 
	intWebsiteTypeID	SMALLINT				NOT NULL,
	CONSTRAINT tblTempWebsite_PK PRIMARY KEY (intWebsiteID)
)

CREATE TABLE tblTempState
(
	intStateID		SMALLINT IDENTITY(1,1)	NOT NULL,
	strState		NVARCHAR(20)		NOT NULL, 
	CONSTRAINT tblTempState_PK PRIMARY KEY (intStateID)
)


CREATE TABLE tblTempCategory
(
	intCategoryID		SMALLINT IDENTITY(1,1)	NOT NULL, 
	strCategory			NVARCHAR(50)		NOT NULL, 
	CONSTRAINT tblTempCategory_PK PRIMARY KEY (intCategoryID)
)

CREATE TABLE tblTempDay
(
	intDayID		SMALLINT IDENTITY(1,1)	NOT NULL, 
	strDay			NVARCHAR(20)		NOT NULL, 
	CONSTRAINT tblTempDay_PK PRIMARY KEY (intDayID)
)

CREATE TABLE tblTempContactPersonType
(
	intContactPersonTypeID		SMALLINT IDENTITY(1,1)	NOT NULL,
	strContactPersonType		NVARCHAR(50)			NOT NULL,
	CONSTRAINT tblTempContactPersonType_PK PRIMARY KEY (intContactPersonTypeID)
)

CREATE TABLE tblTempSocialMedia
(
	intSocialMediaID		SMALLINT IDENTITY (1,1) NOT NULL,
	strPlatform				NVARCHAR(50)			NOT NULL,
	CONSTRAINT tblTempSocialMedia_PK PRIMARY KEY (intSocialMediaID)
)

CREATE TABLE tblTempWebsiteType
(
	intWebsiteTypeID  SMALLINT IDENTITY(1,1) NOT NULL,
	strWebsiteType		VARCHAR(20)			NOT NULL,
	CONSTRAINT tblTempWebsiteType_PK PRIMARY KEY (intWebsiteTypeID)
)

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

CREATE TABLE tblPaymentStatus
(
	intPaymentStatusID	SMALLINT IDENTITY(1,1) NOT NULL,
	strStatus			NVARCHAR(20)			NOT NULL, 
	CONSTRAINT tblPaymentStatus_PK PRIMARY KEY (intPaymentStatusID)
)

CREATE TABLE tblMember 
(
	intMemberID			SMALLINT IDENTITY(1,1)	NOT NULL, 
	intUserID			SMALLINT		NOT NULL, 
	intMemberLevelID		SMALLINT		NOT NULL, 
	intPaymentTypeID		SMALLINT		NOT NULL,
	intPaymentStatusID		SMALLINT		NOT NULL,
	intApprovalStatusID		SMALLINT		NOT NULL,
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

CREATE TABLE tblContactLocation
(
	intContactLocationID	BIGINT IDENTITY(1,1) NOT NULL,
	intLocationID			BIGINT				NOT NULL,
	intContactPersonID		BIGINT				NOT NULL,
	CONSTRAINT tblContactLocation_PK PRIMARY KEY (intContactLocationID)
)

CREATE TABLE tblContactPerson
(
	intContactPersonID		BIGINT IDENTITY(1,1) NOT NULL,
	strContactName			NVARCHAR(50)		NOT NULL,
	strContactPhone			NVARCHAR(20)		,
	strContactEmail			NVARCHAR(50)		,
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
	CONSTRAINT tblCategoryLocation_PK PRIMARY KEY (intCategoryLocationID)
)

--CREATE TABLE tblAdminRequest
--(
--	intAdminRequestID		SMALLINT IDENTITY(1,1)	NOT NULL,
--	intUserID				SMALLINT			NOT NULL,
--	intTypeID				SMALLINT			NOT NULL,
--	intEditedTableID		SMALLINT			NOT NULL, 
--	intEditedColumnID		SMALLINT, 
--	intLocationInTable		SMALLINT, 
--	strRequestedChange      NVARCHAR(2000), 
--	CONSTRAINT tblAdminRequest_PK PRIMARY KEY (intAdminRequestID)
--)

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
	monPrice			MONEY, 
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

CREATE TABLE tblAdminRequest
(
	intAdminRequestID		SMALLINT IDENTITY(1,1)		NOT NULL,
	intMemberID				SMALLINT			NOT NULL,
	strRequestType			NVARCHAR(50)			NOT NULL, 
	strRequestedChange		NVARCHAR(500)			NOT NULL,
	intApprovalStatusID		SMALLINT			NOT NULL,
	CONSTRAINT tblAdminRequest_PK PRIMARY KEY (intAdminRequestID)
)

CREATE TABLE tblMembershipRequest 
(
	intMembershipRequestID	SMALLINT IDENTITY(1,1) NOT NULL, 
	intMemberID				SMALLINT				NOT NULL, 
	CONSTRAINT tblMembershipRequest_PK PRIMARY KEY (intMembershipRequestID)
)

CREATE TABLE tblApprovalStatus
(
	intApprovalStatusID		SMALLINT IDENTITY(1,1)	NOT NULL,
	strApprovalStatus		NVARCHAR(20)		NOT NULL, 
	CONSTRAINT tblApprovalStatus_PK PRIMARY KEY (intApprovalStatusID)
)

-------------------------------------------------------------------------------------------------------------------------------
-- FOREIGN KEYS 
-------------------------------------------------------------------------------------------------------------------------------

-- CHILD					PARENT					COLUMN(s)
-- -----					-----					------
-- tblUser						tblState				intStateID
-- tblMember					tblUser					intUserID
-- tblMember					tblMemberLevel			intMemberLevelID
-- tblMember					tblPaymentType			intPaymentTypeID
-- tblCompanyMember				tblCompany				intCompanyID
-- tblCompanyMember				tblMember				intMemberID
-- tblLocation					tblCompany				intCompanyID
-- tblLocation					tblState				intStateID
-- tblCategoryLocation			tblCategory				intCategoryID
-- tblCategoryLocation			tblLocation				intLocationID
-- tblEventLocation				tblEvent				intEventID
-- tblEventLocation				tblLocation				intLocationID
-- tblAdminRequest				tblMember				intMemberID
-- tblAdminRequest				tblRequestTable			intRequestTableID
-- tblSpecialLocation			tblSpecial				intSpecialID
-- tblSpecialLocation			tblLocation				intLocationID
-- tblLocationHours				tblLocation				intLocationID
-- tblLocationHours				tblDay					intDayID
-- tblCompanyAward				tblCompany				intCompanyID
-- tblCompanySocialMedia		tblCompany				intCompanyID
-- tblCompanySocialMedia		tblSocialMedia			intSocialMedia
-- tblContactLocation			tblContactPerson		intContactPersonID
-- tblContactLocation			tblLocation				intLocationID
-- tblContactPerson				tblCompanyID			intCompanyID
-- tblContactPerson				tblContactPersonType	intContactPersonTypeID
-- tblTempMember				tblUser					intUserID
-- tblTempMember				tblMemberLevel			intMemberLevelID
-- tblTempMember				tblPaymentType			intPaymentTypeID
-- tblTempLocation				tblTempCompany			intCompanyID
-- tblTempLocation				tblTempState			intStateID
-- tblTempCategoryLocation		tblTempCategory			intCategoryID
-- tblTempCategoryLocation		tblTempLocation			intLocationID
-- tblTempLocationHours			tblTempLocation			intLocationID
-- tblTempLocationHours			tblTempDay				intDayID
-- tblTempCompanySocialMedia	tblTempCompany			intCompanyID
-- tblTempCompanySocialMedia	tblTempSocialMedia		intSocialMedia
-- tblTempContactPerson			tblTempCompanyID		intCompanyID
-- tblTempContactLocation		tblTempContactPerson	intContactPersonID
-- tblTempContactLocation		tblTempLocation			intLocationID
-- tblTempLocation				tblAdminRequest			intAdminRequestID
-- tblTempContactPerson			tblContactPersonType	intContactPersonTypeID

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

ALTER TABLE tblContactPerson ADD CONSTRAINT tblContactPerson_tblCompany_FK
FOREIGN KEY (intCompanyID) REFERENCES tblCompany (intCompanyID)

ALTER TABLE tblContactPerson ADD CONSTRAINT tblContactPerson_tblContactPersonType_FK
FOREIGN KEY (intContactPersonTypeID) REFERENCES tblContactPersonType (intContactPersonTypeID)

ALTER TABLE tblTempLocation ADD CONSTRAINT tblTempLocation_tblTempCompany_FK
FOREIGN KEY (intCompanyID) REFERENCES tblTempCompany (intCompanyID)

ALTER TABLE tblTempLocation ADD CONSTRAINT tblTempLocation_tblState_FK
FOREIGN KEY (intStateID) REFERENCES tblTempState (intStateID)

ALTER TABLE tblTempCategoryLocation ADD CONSTRAINT tblTempCategoryLocation_tblTempCategory_FK
FOREIGN KEY (intCategoryID) REFERENCES tblTempCategory (intCategoryID)

ALTER TABLE tblTempCategoryLocation ADD CONSTRAINT tblTempCategoryLocation_tblTempLocation_FK
FOREIGN KEY (intLocationID) REFERENCES tblTempLocation (intLocationID)

ALTER TABLE tblTempLocationHours ADD CONSTRAINT tblTempLocationHours_tblTempLocation_FK
FOREIGN KEY (intLocationID) REFERENCES tblTempLocation (intLocationID)

ALTER TABLE tblTempLocationHours ADD CONSTRAINT tblTempLocationHours_tblTempDay_FK
FOREIGN KEY (intDayID) REFERENCES tblTempDay (intDayID)

ALTER TABLE tblTempCompanySocialMedia ADD CONSTRAINT tblTempCompanySocialMedia_tblTempCompany_FK
FOREIGN KEY (intCompanyID) REFERENCES tblTempCompany (intCompanyID)

ALTER TABLE tblTempCompanySocialMedia ADD CONSTRAINT tblTempCompanySocialMedia_tblTempSocialMedia_FK
FOREIGN KEY (intSocialMediaID) REFERENCES tblTempSocialMedia (intSocialMediaID)

ALTER TABLE tblTempWebsite ADD CONSTRAINT tblTempWebsite_tblTempWebsiteType_FK
FOREIGN KEY (intWebsiteTypeID) REFERENCES tblTempWebsiteType (intWebsiteTypeID)

ALTER TABLE tblTempWebsite ADD CONSTRAINT tblTempWebsite_tblTempCompany_FK
FOREIGN KEY (intCompanyID) REFERENCES tblTempCompany (intCompanyID)

ALTER TABLE tblTempContactPerson ADD CONSTRAINT tblTempContactPerson_tblTempCompany_FK
FOREIGN KEY (intCompanyID) REFERENCES tblTempCompany (intCompanyID)

ALTER TABLE tblTempContactLocation ADD CONSTRAINT tblTempContactLocation_tblTempContactPerson_FK
FOREIGN KEY (intContactPersonID) REFERENCES tblTempContactPerson (intContactPersonID)

ALTER TABLE tblTempContactLocation ADD CONSTRAINT tblTempContactLocation_tblTempLocation_FK
FOREIGN KEY (intLocationID) REFERENCES tblTempLocation (intLocationID)

ALTER TABLE tblTempLocation ADD CONSTRAINT tblTempLocation_tblAdminRequest_FK
FOREIGN KEY (intAdminRequestID) REFERENCES tblAdminRequest (intAdminRequestID)

ALTER TABLE tblTempContactPerson ADD CONSTRAINT tblTempContactPerson_tblTempContactPersonType_FK
FOREIGN KEY (intContactPersonTypeID) REFERENCES tblTempContactPersonType (intContactPersonTypeID)

ALTER TABLE tblMembershipRequest ADD CONSTRAINT tblMembershipRequest_tblMember_FK
FOREIGN KEY (intMemberID) REFERENCES tblMember (intMemberID)

ALTER TABLE tblMember ADD CONSTRAINT tblMember_tblApprovalStatus_FK
FOREIGN KEY (intApprovalStatusID) REFERENCES tblApprovalStatus (intApprovalStatusID)

ALTER TABLE tblContactLocation ADD CONSTRAINT tblTContactLocation_tblContactPerson_FK
FOREIGN KEY (intContactPersonID) REFERENCES tblContactPerson (intContactPersonID)

ALTER TABLE tblContactLocation ADD CONSTRAINT tblContactLocation_tblTempLocation_FK
FOREIGN KEY (intLocationID) REFERENCES tblLocation (intLocationID)

-- -----------------------------------------------------------------------------------------
-- STORED PROCEDURES 
-- -----------------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DELETE_ADMIN_REQUEST]
@intAdminRequestID AS SMALLINT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM db_owner.tblAdminRequest WHERE intAdminRequestID = @intAdminRequestID
	RETURN @@ROWCOUNT
END
GO

CREATE PROCEDURE [db_owner].[LOGIN]
@strUsername NVARCHAR(20)
,@strPassword NVARCHAR(20)
AS
BEGIN	
	SET NOCOUNT ON;

	SELECT		u.intUserID, u.strFirstName, u.strLastName, u.strAddress, u.strCity, u.intStateID, s.strState, u.strZip, u.strPhone, u.strEmail, u.strUsername, u.strPassword, u.isAdmin
	FROM		tblState as s FULL OUTER JOIN tblUser as u 
				ON s.intStateID = u.intStateID
	WHERE		u.strUsername = @strUsername and u.strPassword = @strPassword 
END
GO

CREATE PROCEDURE [dbo].[SELECT_ADMINREQUESTS]
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN
		SELECT * FROM db_owner.tblAdminRequest
		WHERE intApprovalStatusID = 1
	END
END
GO

CREATE PROCEDURE [dbo].INSERT_COMPANYMEMBER_RELATIONSHIP
@intMemberID SMALLINT
,@intCompanyID BIGINT
AS 
BEGIN
	SET NOCOUNT ON;
	DECLARE @COUNT AS TINYINT

	--DONT ALLOW MORE THAN ONE COMPANY NAME IN THIS TABLE
	IF @intMemberID IS NOT NULL AND @intCompanyID IS NOT NULL
	SELECT @COUNT=COUNT(*) FROM db_owner.tblCompanyMember  WHERE intCompanyID = @intCompanyID AND intMemberID = @intMemberID
	
	IF @COUNT = 0
		BEGIN
			INSERT INTO db_owner.tblCompanyMember WITH (TABLOCKX)
				([intMemberID]
				,[intCompanyID])
			VALUES
				(@intMemberID
				,@intCompanyID)
			PRINT 'Good'
			RETURN 1
		END
	ELSE
		PRINT 'BAD'
		RETURN -1
END
GO

CREATE PROCEDURE [dbo].[DELETE_TEMP_LOCATION]
@lngLocationID AS BIGINT
,@lngCompanyID AS BIGINT
AS
BEGIN
	SET NOCOUNT ON;

		DELETE FROM db_owner.tblTempCategoryLocation WHERE intLocationID = @lngLocationID
		DELETE FROM db_owner.tblTempLocationHours WHERE intLocationID = @lngLocationID
		DELETE FROM db_owner.tblTempCompanySocialMedia WHERE intCompanyID = @lngCompanyID
		DELETE FROM db_owner.tblTempWebsite WHERE intCompanyID = @lngCompanyID
		DELETE from db_owner.tblTempContactLocation Where intLocationID = @lngLocationID
		DELETE FROM db_owner.tblTempContactPerson WHERE intCompanyID = @lngCompanyID
		DELETE FROM db_owner.tblTempLocation WHERE intLocationID = @lngLocationID
		DELETE FROM db_owner.tblTempCompany WHERE intCompanyID = @lngCompanyID

	RETURN @@ROWCOUNT
END
GO

CREATE PROCEDURE [dbo].[SELECT_ALLCATEGORY_FORTEMPLOCATION]
@intLocationID BIGINT = NULL
AS 
BEGIN
	SET NOCOUNT ON;
	
	IF @intLocationID IS NOT NULL
	BEGIN
		SELECT *
		FROM db_owner.tblTempCategoryLocation AS CatLoc
		JOIN db_owner.tblTempCategory AS Cat
		ON CatLoc.intCategoryID = Cat.intCategoryID
		WHERE [intLocationID] = @intLocationID
		ORDER BY Cat.strCategory
	END
END
GO

CREATE PROCEDURE [db_owner].[GET_NOT_CATEGORIES]
@intLocationID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	*
	FROM	tblCategory 
	WHERE	intCategoryID NOT IN (SELECT intCategoryID FROM tblCategoryLocation WHERE intLocationID = @intLocationID)
END 
GO

CREATE PROCEDURE [db_owner].[UPDATE_USER]
@intUserID SMALLINT, 
@strFirstName NVARCHAR(25),
@strLastName NVARCHAR(25),
@strAddress NVARCHAR(100),
@strCity NVARCHAR(20),
@intStateID SMALLINT,
@strZip NVARCHAR(15),
@strPhone NVARCHAR(20),
@strEmail NVARCHAR(50),
@strPassword NVARCHAR(15)
AS
BEGIN 
	SET NOCOUNT ON;

	UPDATE	tblUser
	SET		strFirstName = @strFirstName, strLastName = @strLastName, strAddress = @strAddress, strCity = @strCity, intStateID = @intStateID, strZip =				@strZip, strPhone = @strPhone, strEmail = @strEmail, strPassword = @strPassword
	WHERE	intUserID = @intUserID
	RETURN 1

END
GO

CREATE PROCEDURE [dbo].[SELECT_LOCATION_WEBSITE]
@intLocationID BIGINT = NULL
AS 
BEGIN
	SET NOCOUNT ON;
	
	IF @intLocationID IS NOT NULL
	BEGIN
		SELECT * FROM db_owner.tblWebsite AS Website
		JOIN db_owner.tblWebsiteType AS WebType
		ON Website.intWebsiteTypeID = WebType.intWebsiteTypeID
		JOIN db_owner.tblLocation AS Loc
		ON Loc.intCompanyID = Website.intCompanyID
		WHERE intLocationID = @intLocationID
	END
END
GO


CREATE PROCEDURE [dbo].[SELECT_ALLCATEGORY_FORLOCATION]
@intLocationID BIGINT = NULL
AS 
BEGIN
	SET NOCOUNT ON;
	
	IF @intLocationID IS NOT NULL
	BEGIN
		SELECT *
		FROM db_owner.tblCategoryLocation AS CatLoc
		JOIN db_owner.tblCategory AS Cat
		ON CatLoc.intCategoryID = Cat.intCategoryID
		WHERE [intLocationID] = @intLocationID
		ORDER BY Cat.strCategory
	END
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

CREATE PROCEDURE [db_owner].[GET_CURRENT_CATEGORIES]
@intLocationID BIGINT 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	*
	FROM	tblCategory
	WHERE	intCategoryID IN (SELECT intCategoryID FROM tblCategoryLocation WHERE intLocationID = @intLocationID)
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
@strAddress NVARCHAR(50),
@strCity NVARCHAR(50),
@strPhone NVARCHAR(10),
@intStateID SMALLINT,
@strZip NVARCHAR(10),
@isAdmin BIT
AS
SET NOCOUNT ON
SET XACT_ABORT ON 
BEGIN
	DECLARE @COUNT AS TINYINT 

	IF @intStateID = 0 SET @intStateID = NULL

	SELECT @COUNT=COUNT(*) FROM db_owner.tblUser WHERE strUsername = @strUsername
	IF @COUNT > 0 RETURN -2 -- user with this username already exists 
	
	SELECT @COUNT=COUNT(*)FROM db_owner.tblUser WHERE strEmail = @strEmail
	IF @COUNT > 0 RETURN -1 -- user with this email already exists 

	INSERT INTO [db_owner].[tblUser]
			([strFirstName],
			 [strLastName],
			 [strAddress],
			 [strCity],
			 [intStateID],
			 [strZip],
			 [strPhone],
			 [strEmail],
			 [strUsername],
			 [strPassword],
			 [isAdmin])
		VALUES
			(@strFirstName, 
			 @strLastName, 
			 @strAddress,
			 @strCity,
			 @intStateID,
			 @strZip,
			 @strPhone,
			 @strEmail,
			 @strUsername, 
			 @strPassword,
			 @isAdmin)
	SELECT @intNewUserID=@@IDENTITY
	RETURN 1
END 
GO

CREATE PROCEDURE [db_owner].[INSERT_SPECIAL]
@intSpecialID SMALLINT OUTPUT, 
@strDescription NVARCHAR(100), 
@monPrice MONEY, 
@dtmStart DATE, 
@dtmEnd DATE
AS 
SET NOCOUNT ON 
SET XACT_ABORT ON
BEGIN
	INSERT INTO [db_owner].[tblSpecial] WITH (TABLOCKX)
				([strDescription]	
				,[monPrice]
				,[dtmStart]
				,[dtmEnd])
			VALUES
				(@strDescription
				,@monPrice
				,@dtmStart
				,@dtmEnd)
			SELECT @intSpecialID=@@IDENTITY
			RETURN 1
END
GO

CREATE PROCEDURE [dbo].[INSERT_CONTACTPERSON]
@intContactPersonID AS BIGINT OUTPUT
,@PROC_TEST AS INT OUTPUT
,@strContactName AS NVARCHAR(50)
,@strContactPhone AS NVARCHAR(20)
,@strContactEmail AS NVARCHAR(50)
,@intCompanyID AS BIGINT
,@intContactPersonTypeID AS SMALLINT
AS
SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN

	DECLARE @COUNT AS TINYINT

	SELECT @COUNT=COUNT(*) FROM db_owner.tblContactPerson  WHERE intCompanyID = @intCompanyID AND intContactPersonTypeID = @intContactPersonTypeID AND strContactName = @strContactName
	IF @COUNT >0 RETURN -1 --CONTACT PERSON RECORD EXISTS

	INSERT INTO [db_owner].[tblContactPerson] WITH (TABLOCKX)
				([strContactName]
				,[strContactPhone]
				,[strContactEmail]
				,[intCompanyID]
				,[intContactPersonTypeID])
			VALUES
				(@strContactName
				,@strContactPhone
				,@strContactEmail
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
	SELECT @COUNT=COUNT(*) FROM db_owner.tblLocation  WHERE strAddress = @strAddress AND strZip = @strZip
	IF @COUNT >0 RETURN -1 --Location already exists

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

CREATE PROCEDURE [dbo].[SELECT_LOCATION_CONTACTS]
@intLocationID BIGINT = NULL
AS 
BEGIN
	SET NOCOUNT ON;
	
	IF @intLocationID IS NOT NULL
	BEGIN
		SELECT * FROM db_owner.tblContactPerson AS person
		JOIN db_owner.tblContactPersonType AS type
		ON type.intContactPersonTypeID = person.intContactPersonTypeID
		JOIN db_owner.tblContactLocation AS ConLoc
		ON ConLoc.intContactPersonID = person.intContactPersonID
		WHERE intLocationID = @intLocationID
	END
END
GO

CREATE PROCEDURE [dbo].[SELECT_LOCATION_SOCIALMEDIA]
@intLocationID BIGINT = NULL
AS 
BEGIN
	SET NOCOUNT ON;
	
	IF @intLocationID IS NOT NULL
	BEGIN
		SELECT * FROM db_owner.tblCompanySocialMedia AS compSocMed
		JOIN db_owner.tblLocation AS Loc
		ON Loc.intCompanyID = compSocMed.intCompanyID
		JOIN db_owner.tblSocialMedia AS SocMed
		ON SocMed.intSocialMediaID = compSocMed.intSocialMediaID
		WHERE Loc.intLocationID = @intLocationID
	END
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

	SELECT @COUNT=COUNT(*) FROM db_owner.tblCompanySocialMedia WHERE intSocialMediaID = @intSocialMediaID AND intCompanyID = @intCompanyID
	IF @COUNT >0 RETURN -1 --SOCIAL MEDIA ACCOUNT ALREADY EXISTS FOR COMPANY

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
				,[intLocationID])
			VALUES
				(@intCategoryID
				,@intLocationID)
	SELECT @intCategoryLocationID=@@IDENTITY
	RETURN 1

END
GO

CREATE PROCEDURE [db_owner].[INSERT_SPECIALLOCATION]
@intSpecialLocationID BIGINT OUT,
@intSpecialID SMALLINT, 
@intLocationID BIGINT
AS
SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN
	DECLARE @COUNT AS TINYINT

	SELECT @COUNT=COUNT(*) FROM db_owner.tblSpecialLocation WHERE intSpecialID = @intSpecialID and intLocationID = @intLocationID
	IF @COUNT > 0 RETURN -1 --SPECIAL ALREADY EXISTS FOR LOCATION 

	INSERT INTO [db_owner].[tblSpecialLocation] WITH (TABLOCKX)
				([intSpecialID],				
				 [intLocationID]) 
			VALUES
				(@intSpecialID,
				 @intLocationID)
			SELECT @intSpecialLocationID=@@IDENTITY
			RETURN 1
END
GO

CREATE PROCEDURE [dbo].[SELECT_LOCATION_SPECIALS]
@intLocationID BIGINT = NULL
AS 
BEGIN
	SET NOCOUNT ON;
	
	IF @intLocationID IS NOT NULL
	BEGIN
		SELECT *
		FROM db_owner.tblSpecial as special
		FULL OUTER JOIN db_owner.tblSpecialLocation as specLoc
		ON special.intSpecialID = specLoc.intSpecialID
		WHERE [intLocationID] = @intLocationID
	END
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

CREATE PROCEDURE [db_owner].[GET_LOCATIONS_NOT_CONTACT]
@intContactPersonID BIGINT,
@intCompanyID		BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	DISTINCT l.intLocationID, l.strAddress, l.strCity, s.strState, l.strZip
	FROM	tblLocation as l JOIN tblContactLocation as conloc 
			ON l.intLocationID = conloc.intLocationID
			JOIN tblContactPerson as person
			ON person.intContactPersonID = conloc.intContactPersonID
			JOIN tblState as s 
			ON s.intStateID = l.intStateID
	WHERE	person.intContactPersonID != @intContactPersonID and person.intCompanyID = @intCompanyID and l.intLocationID NOT IN (SELECT intLocationID FROM tblContactLocation WHERE intContactPersonID = @intContactPersonID)
END 
GO

CREATE PROCEDURE [db_owner].[GET_CONTACTS_BY_COMPANY]
@intCompanyID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT c.intContactPersonID, c.strContactName, c.strContactPhone, c.strContactEmail, conloc.intLocationID, ct.intContactPersonTypeID, ct.strContactPersonType
	FROM	tblContactPerson as c FULL OUTER JOIN tblContactPersonType as ct
			ON ct.intContactPersonTypeID = c.intContactPersonTypeID
			JOIN tblContactLocation as conloc
			ON conloc.intContactPersonID = c.intContactPersonID
	WHERE	c.intCompanyID = @intCompanyID
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

CREATE PROCEDURE [dbo].[SELECT_LOCATION_BYCATEGORY]
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
@lngLocationID AS BIGINT
,@lngCompanyID AS BIGINT
AS
BEGIN
	SET NOCOUNT ON;

		DELETE FROM db_owner.tblTempCategoryLocation WHERE intLocationID = @lngLocationID
		DELETE FROM db_owner.tblTempLocationHours WHERE intLocationID = @lngLocationID
		DELETE FROM db_owner.tblTempCompanySocialMedia WHERE intCompanyID = @lngCompanyID
		DELETE FROM db_owner.tblTempWebsite WHERE intCompanyID = @lngCompanyID
		DELETE from db_owner.tblTempContactLocation Where intLocationID = @lngLocationID
		DELETE FROM db_owner.tblTempContactPerson WHERE intCompanyID = @lngCompanyID
		DELETE FROM db_owner.tblTempLocation WHERE intLocationID = @lngLocationID
		DELETE FROM db_owner.tblTempCompany WHERE intCompanyID = @lngCompanyID

	RETURN @@ROWCOUNT
END
GO

CREATE PROCEDURE [db_owner].[DELETE_CATEGORY_FROM_LOCATION]
@intLocationID BIGINT,
@intCategoryID BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM tblCategoryLocation WHERE intLocationID = @intLocationID and intCategoryID = @intCategoryID

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
	DELETE FROM tblSpecialLocation WHERE intLocationID IN (SELECT intLocationID FROM tblLocation WHERE intCompanyID = @intCompanyID)
	DELETE FROM tblContactPerson WHERE intCompanyID = @intCompanyID
	DELETE FROM tblLocation WHERE intCompanyID = @intCompanyID
	DELETE FROM tblCompanyAward WHERE intCompanyID = @intCompanyID
	DELETE FROM tblCompanySocialMedia WHERE intCompanyID = @intCompanyID
	DELETE FROM tblContactPerson WHERE intCompanyID = @intCompanyID
	DELETE FROM tblWebsite WHERE intCompanyID = @intCompanyID
	DELETE FROM tblCompany WHERE intCompanyID = @intCompanyID
	RETURN @@rowcount
END
GO

CREATE PROCEDURE [db_owner].[DELETE_SPECIALLOCATION]
@intSpecialID SMALLINT, 
@intLocationID BIGINT
AS
SET NOCOUNT ON
SET XACT_ABORT ON 
BEGIN
	DELETE FROM tblSpecialLocation WHERE intSpecialID = @intSpecialID and intLocationID = @intLocationID 
	RETURN @@ROWCOUNT
END
GO

CREATE PROCEDURE [dbo].[SELECT_USERLOCATION_ASSOCIATION]
@intUserID AS SMALLINT
AS
BEGIN
	SET NOCOUNT ON;
	IF @intUserID IS NOT NULL
	BEGIN
		SELECT * FROM db_owner.tblLocation AS loc
		JOIN db_owner.tblCompany AS comp
		ON loc.intCompanyID = comp.intCompanyID
		JOIN db_owner.tblCompanyMember AS compMem
		ON compMem.intCompanyID = comp.intCompanyID
		JOIN db_owner.tblMember AS mem
		ON mem.intMemberID = compMem.intMemberID
		JOIN db_owner.tblUser AS users
		ON mem.intUserID = users.intUserID
		WHERE users.intUserID = @intUserID
	END
END
GO

CREATE PROCEDURE [dbo].[SELECT_TEMP_LOCATION_WEBSITE]
@intLocationID BIGINT = NULL
AS 
BEGIN
	SET NOCOUNT ON;
	
	IF @intLocationID IS NOT NULL
	BEGIN
		SELECT * FROM db_owner.tblTempWebsite AS Website
		JOIN db_owner.tblTempWebsiteType AS WebType
		ON Website.intWebsiteTypeID = WebType.intWebsiteTypeID
		JOIN db_owner.tblTempLocation AS Loc
		ON Loc.intCompanyID = Website.intCompanyID
		WHERE intLocationID = @intLocationID
	END
END
GO

CREATE PROCEDURE [dbo].[SELECT_LOCATION_AWARDS]
@intLocationID BIGINT = NULL
AS 
BEGIN
	SET NOCOUNT ON;
	
	IF @intLocationID IS NOT NULL
	BEGIN
		SELECT *
		FROM db_owner.tblCompanyAward AS compAwards
		JOIN db_owner.tblCompany AS comp
		ON comp.intCompanyID = compAwards.intCompanyID
		JOIN db_owner.tblLocation AS loc
		ON loc.intCompanyID = comp.intCompanyID
		WHERE [intLocationID] = @intLocationID;
	END
END
GO

CREATE PROCEDURE [dbo].[SELECT_TEMP_LOCATION_CONTACTS]
@intLocationID BIGINT = NULL
AS 
BEGIN
	SET NOCOUNT ON;
	
	IF @intLocationID IS NOT NULL
	BEGIN
		SELECT * FROM db_owner.tblTempContactPerson AS person
		JOIN db_owner.tblTempContactPersonType AS type
		ON type.intContactPersonTypeID = person.intContactPersonTypeID
		JOIN db_owner.tblTempContactLocation as LocCon
		ON LocCon.intContactPersonID = person.intContactPersonID
		WHERE intLocationID = @intLocationID
	END
END
GO

CREATE PROCEDURE [dbo].[SELECT_TEMP_LOCATION_SOCIALMEDIA]
@intLocationID BIGINT = NULL
AS 
BEGIN
	SET NOCOUNT ON;
	
	IF @intLocationID IS NOT NULL
	BEGIN
		SELECT * FROM db_owner.tblTempCompanySocialMedia AS compSocMed
		JOIN db_owner.tblTempLocation AS Loc
		ON Loc.intCompanyID = compSocMed.intCompanyID
		JOIN db_owner.tblTempSocialMedia AS SocMed
		ON SocMed.intSocialMediaID = compSocMed.intSocialMediaID
		WHERE Loc.intLocationID = @intLocationID
	END
END
GO

CREATE PROCEDURE [dbo].[INSERT_TEMP_COMPANY]
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
	SELECT @COUNT=COUNT(*) FROM db_owner.tblTempCompany  WHERE strCompanyName = @strCompanyName
	IF @COUNT >0 RETURN -1 --COMPANY NAME EXISTS

	INSERT INTO [db_owner].[tblTempCompany] WITH (TABLOCKX)
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

CREATE PROCEDURE [dbo].[SELECT_TEMP_LOCATION]
@intAdminRequestKey SMALLINT = NULL
AS 
BEGIN
	SET NOCOUNT ON;
	
	IF @intAdminRequestKey IS NOT NULL
	BEGIN
		SELECT *
		FROM db_owner.tblTempLocation as Loc
		JOIN db_owner.tblTempCompany as Comp
		ON Comp.intCompanyID = Loc.intCompanyID
		JOIN db_owner.tblTempState as St
		ON St.intStateID = Loc.intStateID
		WHERE [intAdminRequestID] = @intAdminRequestKey
	END
END
GO

CREATE PROCEDURE [db_owner].[GET_MEMBERSHIP_REQUESTS]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	u.strFirstName, u.strLastName, u.strEmail, u.strPhone, m.intMemberID, ml.strMemberLevel, pt.strPaymentType
	FROM	tblUser as u FULL OUTER JOIN tblMember as m
			ON u.intUserID = m.intUserID
			FULL OUTER JOIN tblMemberLevel as ml
			ON ml.intMemberLevelID = m.intMemberLevelID
			FULL OUTER JOIN tblPaymentType as pt
			ON pt.intPaymentTypeID = m.intPaymentTypeID
			FULL OUTER JOIN tblPaymentStatus as ps
			ON ps.intPaymentStatusID = m.intPaymentStatusID
	WHERE	m.intApprovalStatusID = 1
END
GO

CREATE PROCEDURE [dbo].[INSERT_TEMP_WEBSITE]
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
	SELECT @COUNT=COUNT(*) FROM db_owner.tblTempWebsite  WHERE intCompanyID = @intCompanyID AND strURL = @strURL 
	IF @COUNT >0 RETURN -1 --COMPANY WEBPAGE CONNECTION ALREADY EXISTS

	INSERT INTO [db_owner].[tblTempWebsite] WITH (TABLOCKX)
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

CREATE PROCEDURE [dbo].[INSERT_TEMP_CONTACTPERSON]
@intContactPersonID AS BIGINT OUTPUT
,@PROC_TEST AS INT OUTPUT
,@strContactName AS NVARCHAR(50)
,@strContactPhone AS NVARCHAR(20)
,@strContactEmail AS NVARCHAR(50)
,@intCompanyID AS BIGINT
,@intContactPersonTypeID AS SMALLINT
AS
SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN
	
	DECLARE @COUNT AS TINYINT

	SELECT @COUNT=COUNT(*) FROM db_owner.tblTempContactPerson  WHERE strContactName = @strContactName AND intCompanyID = @intCompanyID AND strContactEmail = @strContactEmail
	IF @COUNT >0 
		BEGIN
			SET @intContactPersonID = -1
			SET @PROC_TEST = -1 
			RETURN
		END
	ELSE
		BEGIN
			INSERT INTO [db_owner].[tblTempContactPerson] WITH (TABLOCKX)
						([strContactName]
						,[strContactPhone]
						,[strContactEmail]
						,[intCompanyID]
						,[intContactPersonTypeID])
					VALUES
						(@strContactName
						,@strContactPhone
						,@strContactEmail
						,@intCompanyID
						,@intContactPersonTypeID)
			SELECT @intContactPersonID=@@IDENTITY
			SET @PROC_TEST = 1
		END
END
GO	

CREATE PROCEDURE [dbo].[INSERT_TEMP_CONTACTLOCATION]
@intContactLocationID AS BIGINT OUTPUT
,@PROC_TEST AS INT OUTPUT
,@intLocationID AS BIGINT
,@intContactPersonID AS BIGINT
AS
SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN

	DECLARE @COUNT AS TINYINT

	SELECT @COUNT=COUNT(*) FROM db_owner.tblTempContactLocation  WHERE intLocationID = @intLocationID AND intContactPersonID = @intContactPersonID
	IF @COUNT >0 
		BEGIN
			SET @intContactPersonID = -1
			SET @PROC_TEST = -1 
			RETURN
		END
	ELSE
		BEGIN
			INSERT INTO [db_owner].[tblTempContactLocation] WITH (TABLOCKX)
				([intLocationID]
				,[intContactPersonID])
			VALUES
				(@intLocationID
				,@intContactPersonID)
			SELECT @intContactLocationID=@@IDENTITY
			SET @PROC_TEST = 1
		END
END
GO

CREATE PROCEDURE[dbo].[INSERT_TEMP_CONTACTLOCATION_RELATIONSHIP]
@strContactName AS NVARCHAR(50)
,@strContactPhone AS NVARCHAR(20)
,@strContactEmail AS NVARCHAR(50)
,@intCompanyID AS BIGINT
,@intContactPersonTypeID AS SMALLINT
,@intLocationID AS BIGINT
,@intContactPersonID AS BIGINT OUTPUT
,@intContactLocationID AS BIGINT OUTPUT
AS
SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN
	DECLARE @PROC_TEST AS INT
	EXECUTE dbo.INSERT_TEMP_CONTACTPERSON @intContactPersonID OUTPUT, @PROC_TEST OUTPUT, @strContactName, @strContactPhone, @strContactEmail, @intCompanyID, @intContactPersonTypeID
END
BEGIN
	EXECUTE dbo.INSERT_TEMP_CONTACTLOCATION @intContactLocationID OUTPUT, @PROC_TEST OUTPUT, @intLocationID, @intContactPersonID
END
IF @PROC_TEST = -1
	RETURN -1
ELSE
	RETURN 1
GO

CREATE PROCEDURE [dbo].[INSERT_CONTACTLOCATION]
@intContactLocationID AS BIGINT OUTPUT
,@PROC_TEST AS INT OUTPUT
,@intLocationID AS BIGINT
,@intContactPersonID AS BIGINT
AS
SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN

	DECLARE @COUNT AS TINYINT

	SELECT @COUNT=COUNT(*) FROM db_owner.tblTempContactLocation  WHERE intLocationID = @intLocationID AND intContactPersonID = @intContactPersonID
	IF @COUNT >0 
		BEGIN
			SET @intContactPersonID = -1
			SET @PROC_TEST = -1 
			RETURN
		END
	ELSE
		BEGIN
			INSERT INTO [db_owner].[tblTempContactLocation] WITH (TABLOCKX)
				([intLocationID]
				,[intContactPersonID])
			VALUES
				(@intLocationID
				,@intContactPersonID)
			SELECT @intContactLocationID=@@IDENTITY
			SET @PROC_TEST = 1
		END
END
GO

CREATE PROCEDURE[dbo].[INSERT_CONTACTLOCATION_RELATIONSHIP]
@strContactName AS NVARCHAR(50)
,@strContactPhone AS NVARCHAR(20)
,@strContactEmail AS NVARCHAR(50)
,@intCompanyID AS BIGINT
,@intContactPersonTypeID AS SMALLINT
,@intLocationID AS BIGINT
,@intContactPersonID AS BIGINT OUTPUT
,@intContactLocationID AS BIGINT OUTPUT
AS
SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN
	DECLARE @PROC_TEST AS INT
	EXECUTE dbo.INSERT_CONTACTPERSON @intContactPersonID OUTPUT, @PROC_TEST OUTPUT, @strContactName, @strContactPhone, @strContactEmail, @intCompanyID, @intContactPersonTypeID
END
BEGIN
	EXECUTE dbo.INSERT_CONTACTLOCATION @intContactLocationID OUTPUT, @PROC_TEST OUTPUT, @intLocationID, @intContactPersonID
END
IF @PROC_TEST = -1
	RETURN -1
ELSE
	RETURN 1
GO

CREATE PROCEDURE [dbo].[INSERT_TEMP_LOCATION]
@intLocationID AS BIGINT OUTPUT
,@intAdminRequestID AS SMALLINT
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
	SELECT @COUNT=COUNT(*) FROM db_owner.tblTempLocation  WHERE strAddress = @strAddress AND strZip = @strZip
	IF @COUNT >0 RETURN -1 --Location already exists

	INSERT INTO [db_owner].[tblTempLocation] WITH (TABLOCKX)
				([intCompanyID]
				,[strAddress]
				,[strCity]
				,[intStateID]
				,[strZip]
				,[strPhone]
				,[strEmail]
				,[intAdminRequestID])
			VALUES
				(@intCompanyID
				,@strAddress
				,@strCity
				,@intStateID
				,@strZip
				,@strPhone
				,@strEmail
				,@intAdminRequestID)
	SELECT @intLocationID=@@IDENTITY
	RETURN 1

END
GO

CREATE PROCEDURE [dbo].[INSERT_ADMIN_REQUEST]
@intAdminRequestID AS SMALLINT OUTPUT
,@intMemberID AS SMALLINT
,@strRequestType AS NVARCHAR(100)
,@strRequestedChange AS NVARCHAR(500)
,@intApprovalStatusID AS SMALLINT
AS
SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN

	--DONT ALLOW MORE THAN ONE EXACT ADDRESS IN THIS TABLE

	INSERT INTO [db_owner].[tblAdminRequest] WITH (TABLOCKX)
				([intMemberID]
				,[strRequestType]
				,[strRequestedChange]
				,[intApprovalStatusID])
			VALUES
				(@intMemberID
				,@strRequestType
				,@strRequestedChange
				,@intApprovalStatusID)
	SELECT @intAdminRequestID=@@IDENTITY
	RETURN 1

END
GO

CREATE PROCEDURE [dbo].[INSERT_TEMP_SOCIALMEDIA]
@intCompanySocialMediaID AS BIGINT OUTPUT
,@strSocialMediaLink AS NVARCHAR(100)
,@intCompanyID AS BIGINT
,@intSocialMediaID AS SMALLINT
AS
SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN

	DECLARE @COUNT AS TINYINT

	SELECT @COUNT=COUNT(*) FROM db_owner.tblTempCompanySocialMedia WHERE intSocialMediaID = @intSocialMediaID AND intCompanyID = @intCompanyID
	IF @COUNT >0 RETURN -1 --SOCIAL MEDIA ACCOUNT ALREADY EXISTS FOR COMPANY

	INSERT INTO [db_owner].[tblTempCompanySocialMedia] WITH (TABLOCKX)
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

CREATE PROCEDURE [dbo].[INSERT_TEMP_CATEGORYLOCATION]
@intCategoryLocationID AS BIGINT OUTPUT
,@intCategoryID AS SMALLINT
,@intLocationID AS BIGINT
AS
SET NOCOUNT ON
SET XACT_ABORT ON
BEGIN

	DECLARE @COUNT AS TINYINT

	--DONT ALLOW MORE THAN ONE EXACT ADDRESS IN THIS TABLE
	SELECT @COUNT=COUNT(*) FROM db_owner.tblTempCategoryLocation  WHERE intCategoryID = @intCategoryID AND intLocationID = @intLocationID
	IF @COUNT >0 RETURN -1 --CATEGORY NAME ALREADY EXISTS FOR LOCATION

	INSERT INTO [db_owner].[tblTempCategoryLocation] WITH (TABLOCKX)
				([intCategoryID]
				,[intLocationID])
			VALUES
				(@intCategoryID
				,@intLocationID)
	SELECT @intCategoryLocationID=@@IDENTITY
	RETURN 1

END
GO

CREATE PROCEDURE [dbo].[INSERT_TEMP_LOCATIONHOURS]
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
	SELECT @COUNT=COUNT(*) FROM db_owner.tblTempLocationHours  WHERE intLocationID = @intLocationID AND @intDayID = intDayID
	IF @COUNT >0 RETURN -1 --LOCATION HOURS ALREADY EXIST

	INSERT INTO [db_owner].[tblTempLocationHours] WITH (TABLOCKX)
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

CREATE PROCEDURE [dbo].[SELECT_LOCATIONHOURS]
@intLocationID BIGINT = NULL
AS 
BEGIN
	SET NOCOUNT ON;
	
	IF @intLocationID IS NOT NULL
	BEGIN
		SELECT * FROM db_owner.tblLocationHours AS LocHours
		JOIN db_owner.tblDay AS Days
		ON Days.intDayID = LocHours.intDayID
		WHERE [intLocationID] = @intLocationID
	END
END
GO

CREATE PROCEDURE [dbo].[SELECT_TEMP_LOCATIONHOURS]
@intLocationID BIGINT = NULL
AS 
BEGIN
	SET NOCOUNT ON;
	
	IF @intLocationID IS NOT NULL
	BEGIN
		SELECT * FROM db_owner.tblTempLocationHours AS LocHours
		JOIN db_owner.tblTempDay AS Days
		ON Days.intDayID = LocHours.intDayID
		WHERE [intLocationID] = @intLocationID
	END
END
GO

CREATE PROC [dbo].CHECK_IF_MEMBERLOCATION
@intLocationID AS BIGINT = NULL
AS 
BEGIN
	SET NOCOUNT ON
	BEGIN
		IF @intLocationID IS NOT NULL

		SELECT DISTINCT mem.intMemberLevelID
		FROM db_owner.tblMember AS mem
		JOIN db_owner.tblCompanyMember AS compMem
		ON compMem.intMemberID = mem.intMemberID
		JOIN db_owner.tblCompany AS comp
		ON comp.intCompanyID = compMem.intCompanyID
		JOIN db_owner.tblLocation AS loc
		ON loc.intCompanyID = comp.intCompanyID
		WHERE loc.intLocationID = @intLocationID;
	END
END
GO

CREATE PROCEDURE [dbo].[SELECT_SINGLE_ADMINREQUEST]
@intAdminRequestID SMALLINT = NULL
AS 
BEGIN
	SET NOCOUNT ON;
	
	IF @intAdminRequestID IS NOT NULL
	BEGIN
		SELECT *
		FROM db_owner.tblAdminRequest
		WHERE intAdminRequestID = @intAdminRequestID
	END
END
GO

-- -----------------------------------------------------------------------------------------
-- ADD TEST DATA
-- -----------------------------------------------------------------------------------------

INSERT INTO tblApprovalStatus (strApprovalStatus)
VALUES						('Not Approved')
							,('Approved')

INSERT INTO tblContactPersonType (strContactPersonType)
VALUES			('Location Contact')
				,('Customer Service Representative')
				,('Web Admin')

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

INSERT INTO tblPaymentStatus (strStatus)
VALUES		('Paid'),
			('Not Paid')

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

INSERT INTO tblLocation (intCompanyID, strAddress, strCity, intStateID, strZip, strPhone, strEmail)
VALUES	(1, '2030 Madison Rd', 'Cincinnati', 3, '45208-3289', '513-321-3399', 'customerservice@bonbonerie.com'),
	(2, '505 Wyoming Ave', 'Wyoming', 3, '45215-4578', '513-821-0742', 'reschke@wyomingpastryshop.com'),
	(3, '3824 Paxton Ave', 'Cincinnati', 3, '45209-2399', '513-871-3244', 'servatiipastryshop@gmail.com'),
	(3, '2010 Anderson Ferry Rd', 'Cincinnati', 3, '45238-3398', '513-922-0033', 'servatiipastryshop@gmail.com')

INSERT INTO tblContactPerson (strContactName, strContactPhone, strContactEmail, intCompanyID, intContactPersonTypeID)
VALUES					('Briggs, Randall', '5555555555', 'briggs.r@gmail.com', 1, 1)
						,('Hall, Ben', '5555555555', 'hall.b@gmail.com', 1, 2)
						,('Cowen, Candice', '5555555555', 'cowen.c@gmail.com', 1, 3)
						,('Smith, Bob', '5555555555', 'bob.smith@gmail.com', 3, 1)
						,('Brown, Erica', '5555555555', 'erica.b@gmail.com', 3, 1)
						,('Lopez, Maria', '5555555555', 'maria_lopez@gmail.com', 3, 2)
						,('Frank, Joseph', '5555555555', 'joe_frank@gmail.com', 2, 3)

INSERT INTO tblContactLocation (intContactPersonID, intLocationID)
VALUES						(1, 1)
							,(2, 1)
							,(3, 1)
							,(4, 2)
							,(5, 2)
							,(6, 2)
							,(7, 3)

INSERT INTO tblLocationHours (intLocationID, intDayID, strOpen, strClose)
VALUES	(1, 1, '10:00am', '4:00pm'),
	(1, 2, '10:00am', '4:00pm'),
	(1, 3, '10:00am', '4:00pm'),
	(1, 4, '10:00am', '4:00pm'),
	(1, 5, '10:00am', '4:00pm'),
	(1, 6, '10:00am', '4:00pm'),
	(1, 7, 'Closed (except for private parties - call for details)', 'Closed'),
	(2, 1, '6:00am', '6:00pm'),
	(2, 2, '6:00am', '6:00pm'),
	(2, 3, '6:00am', '6:00pm'),
	(2, 4, '6:00am', '6:00pm'),
	(2, 5, '6:00am', '6:00pm'),
	(2, 6, '6:00am', '4:00pm'),
	(2, 7, 'Closed', 'Closed'),
	(3, 1, 'Closed', 'Closed'),
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
		('Random', 'User', '1234 Main St', 'Lawrenceburg', 1, '41010', '5135555555', 'random_user@gmail.com', 'test3', 'test3', 0),
		('Shane', 'Winslow', '26 Glenway', 'Ft. Thomas', 1, '5555555555', 'winzlizle@gmail.com', 'winslows1@gmail.com', 'winslows1', 'password', 0),
		('Grace', 'Gottenbusch', '123 Elm St', 'Covington', 2, '41212', '5135555555', 'grace@gmail.com', 'grace', 'grace', 1),
		('Bob', 'Smith', NULL, NULL, NULL, NULL, NULL, 'bob@gmail.com', 'bob', 'bob', 0)


-- ADD USER TO MEMBER TABLE 
INSERT INTO  tblMember (intUserID, intMemberLevelID, intPaymentTypeID, intApprovalStatusID, intPaymentStatusID)
VALUES	 (1, 1, 2, 2, 1)
		,(3, 2, 1, 2, 1)
		,(4, 2, 1, 2, 1)
		,(5, 1, 1, 1, 2)

-- ADD CONNECTION BETWEEN COMPANY AND MEMBER
INSERT INTO tblCompanyMember (intCompanyID, intMemberID)
VALUES				 (3,2) -- SHANE AS MEMBER FOR SERVATII
					,(3,3) -- GRACE AS MEMBER FOR SERVATII 

INSERT INTO tblMainBanner (strBanner)
VALUES	('This is an example of the main banner. This will hold information relevant to the GCRBA.'),
		('This is an example of the most up-to-date banner in this database. This will hold information relevant to the GCRBA')

INSERT INTO tblSpecial (strDescription, monPrice, dtmStart, dtmEnd)
VALUES			('Celebrate National Apple Pie Day! $5.85 for an 8" Dutch Apple at all Servatii Locations! Call ahead to ensure availablity', 5.85, '05/13/2022', '05/14/2022')

INSERT INTO tblSpecialLocation (intSpecialID, intLocationID)
VALUES				(1, 3)
					,(1, 4)

INSERT INTO tblCategoryLocation (intCategoryID, intLocationID)
VALUES		(6, 1),
			(8, 1),
			(9, 1),
			(10, 1), 
			(11, 1),
			(13,  1),
			(15, 1),
			(1, 2),
			(6, 2),
			(7, 2),
			(8, 2),
			(9, 2),
			(10, 2),
			(11, 2),
			(12, 2),
			(1, 3),
			(2, 3),
			(3, 3),
			(6, 3),
			(7, 3),
			(8, 3),
			(9, 3),
			(10, 3),
			(11, 3),
			(12, 3),
			(13, 3),
			(15, 3),
			(16, 3),
			(1, 4),
			(2, 4),
			(3, 4),
			(6, 4),
			(7, 4),
			(8, 4),
			(9, 4),
			(10, 4),
			(11, 4),
			(12, 4),
			(13, 4),
			(15, 4),
			(16, 4)

INSERT INTO tblTempContactPersonType (strContactPersonType)
VALUES			('Location Contact')
				,('Customer Service Representative')
				,('Web Admin')

INSERT INTO tblTempState (strState)
VALUES			('Indiana'),
				('Kentucky'),
				('Ohio')

INSERT INTO tblTempCategory (strCategory)
VALUES			('Donuts'), 
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

INSERT INTO tblTempDay (strDay)
VALUES			('Monday'),
				('Tuesday'),
				('Wednesday'),
				('Thursday'),
				('Friday'),
				('Saturday'),
				('Sunday')

INSERT INTO tblTempWebsiteType(strWebsiteType)
VALUES				('Main')
					,('Ordering')
					,('Kettle')

INSERT INTO tblAdminRequest(intMemberID, strRequestType, strRequestedChange, intApprovalStatusID)
VALUES						(3, 'Add', 'Add Location', 1)

INSERT INTO tblTempCompany(strCompanyName, strAbout, strBizYear)
VALUES						('BIZ1', 'WE ARE BIZ', '2050')

INSERT INTO tblTempLocation (intCompanyID, strAddress, strCity, intStateID, strZip, strPhone, strEmail, intAdminRequestID)
VALUES							(1, 'MyStreet', 'MyCity', 1, 'MyZip', '5555555555', 'MyEmail@gmail.com', 1)

INSERT INTO tblTempContactPerson(strContactName, strContactPhone, strContactEmail, intCompanyID, intContactPersonTypeID)
VALUES								('Winslow, Shane', '5555555555', 'winslow.shane2@gmail.com', 1, 1)

INSERT INTO tblTempCategoryLocation (intCategoryID, intLocationID)
VALUES								(1, 1)

INSERT INTO tblTempLocationHours (intDayID, intLocationID, strOpen, strClose)
VALUES								(1, 1, '9:00 AM', '10:00 PM')

INSERT INTO tblTempWebsite (intCompanyID, intWebsiteTypeID, strURL)
VALUES						(1, 1, 'https://www.mymain.com')

INSERT INTO tblTempSocialMedia (strPlatform)
VALUES					('Facebook')
						,('Instagram')
						,('Snapchat')
						,('TikTok')
						,('Twitter')
						,('Yelp')

INSERT INTO tblTempContactLocation (intContactPersonID, intLocationID)
VALUES									(1, 1)

INSERT INTO tblTempCompanySocialMedia (intCompanyID, strSocialMediaLink, intSocialMediaID)
VALUES					(1, 'https://www.facebook.com', 1)
