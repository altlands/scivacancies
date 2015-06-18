
--DICTIONARIES

CREATE TABLE "Activities"
(
"Guid" uuid NOT NULL,
"Id" integer NOT NULL,
"Title" text,
CONSTRAINT "PrimaryKey" PRIMARY KEY ("Guid")
)
CREATE TABLE "Criterias"
(
"Guid" uuid NOT NULL,
"Parentid" integer NOT NULL,
"Title" text,
"Code" text,
CONSTRAINT "PrimaryKey" PRIMARY KEY ("Guid")
)
CREATE TABLE "Foivs"
(
"Guid" uuid NOT NULL,
"Id" integer NOT NULL,
"Parentid" integer,
"Title" text,
"Code" text,
CONSTRAINT "PrimaryKey" PRIMARY KEY ("Guid")
)
CREATE TABLE "OrgForms"
(
"Guid" uuid NOT NULL,
"Id" integer NOT NULL,
"Title" text,
CONSTRAINT "PrimaryKey" PRIMARY KEY ("Guid")
)
CREATE TABLE "Regions"
(
"Guid" uuid NOT NULL,
"Id" integer NOT NULL,
"FedDistrictId" integer NOT NULL,
"Title" text,
"OsmId" integer,
"Okato" text,
"Slug" text,
"Code" integer
CONSTRAINT "PrimaryKey" PRIMARY KEY ("Guid")
)
CREATE TABLE "ResearchDirections"
(
"Guid" uuid NOT NULL,
"Id" integer NOT NULL,
"ParentId" integer,
"Title" text,
"TitleEng" text,
"Lft" integer,
"Rgt" integer,
"Lvl" integer,
"OecdCode" text,
"WosCode" text
CONSTRAINT "PrimaryKey" PRIMARY KEY ("Guid")
)

--CORE

CREATE TABLE "Attachments"
(
"Guid" uuid NOT NULL

CONSTRAINT "PrimaryKey" PRIMARY KEY ("Guid")
)
CREATE TABLE "FavoriteVacancies"
(
"VacancyGuid" uuid NOT NULL,
"ResearcherGuid" uuid NOT NULL
)
CREATE TABLE "Notifications"
(
"Guid" uuid NOT NULL,
CONSTRAINT "PrimaryKey" PRIMARY KEY ("Guid")
)
CREATE TABLE "Organizations"
(
"Guid" uuid NOT NULL,
"Name" text,
"NameEng" text,
"ShortName" text,
"ShortNameEng" text,
"CityName" text,
"Address" text,
"Website" text,
"Email" text,
"INN" text,
"OGRN" text,
"OrgForm" text,
"OrgFormId" integer,
"PublishedVacancies" integer,
"Foiv" text,
"FoivId" integer,
"Activity" text,
"ActivityId" integer,
"HeadFirstName" text,
"HeadLastName" text,
"HeadPatronymic" text,
CONSTRAINT "PrimaryKey" PRIMARY KEY ("Guid")
)
CREATE TABLE "Positions"
(

"Guid" uuid NOT NULL,
"Name" text,
"FullName" text,
"FieldOfScience" text,
"FieldOfScienceId" integer,
"ResearchTheme" text,
"ResearchThemeId" integer,
"Tasks" text,
"SalaryFrom" integer,
"SalaryTo" integer,
"Bonuses" text,
"ContractType" integer,
"ContractTime" 
"SocialPackage" 
"Rent"
"OfficeAccomodation"
"TransportCompensation"
"RegionId"
"CityName"
"Details"

)


