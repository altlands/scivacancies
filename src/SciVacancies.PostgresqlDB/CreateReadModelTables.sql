
--DICTIONARIES

CREATE TABLE "Activities"
(
  "Guid" uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
  "Title" text,
  "Id" integer,
  CONSTRAINT "Activity_pkey" PRIMARY KEY ("Guid")
);

CREATE TABLE "Criterias"
(
  "Guid" uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
  "ParentId" integer,
  "Id" integer,
  "Title" text,
  "Code" text,
  CONSTRAINT "Criteria_pkey" PRIMARY KEY ("Guid")
);
CREATE TABLE "Foivs"
(
  "Guid" uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
  "Id" integer,
  "ParentId" integer,
  "Title" text,
  "ShortTitle" text,
  CONSTRAINT "Foiv_pkey" PRIMARY KEY ("Guid")
);
CREATE TABLE "OrgForms"
(
  "Guid" uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
  "Id" integer,
  "Title" text,
  CONSTRAINT "OrgForm_pkey" PRIMARY KEY ("Guid")
);
CREATE TABLE "PositionTypes"
(
  "Guid" uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
  "Id" integer,
  "Title" text,
  CONSTRAINT "PositionType_pkey" PRIMARY KEY ("Guid")
);
CREATE TABLE "Regions"
(
  "Guid" uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
  "Id" integer,
  "FedDistrictId" integer,
  "Title" text,
  "OsmId" integer,
  "Okato" text,
  "Slug" text,
  "Code" integer,
  CONSTRAINT "Region_pkey" PRIMARY KEY ("Guid")
);
CREATE TABLE "ResearchDirections"
(
  "Guid" uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
  "Id" integer,
  "ParentId" integer,
  "Title" text,
  "TitleEng" text,
  "Lft" integer,
  "Rgt" integer,
  "Lvl" integer,
  "OecdCode" text,
  "WosCode" text,
  "Root" integer,
  CONSTRAINT "ResearchDirection_pkey" PRIMARY KEY ("Guid")
);

--CORE

CREATE TABLE "Attachments"
(
"Guid" uuid NOT NULL,

CONSTRAINT "Attachment_pkey" PRIMARY KEY ("Guid")
);

CREATE TABLE "FavoriteVacancies"
(
"VacancyGuid" uuid NOT NULL,
"ResearcherGuid" uuid NOT NULL
);
CREATE TABLE "Notifications"
(
"Guid" uuid NOT NULL,
CONSTRAINT "Notification_pkey" PRIMARY KEY ("Guid")
);
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
CONSTRAINT "Organization_pkey" PRIMARY KEY ("Guid")
);
CREATE TABLE "Positions"
(

"Guid" uuid NOT NULL,
"Name" text,
"FullName" text,
"ResearchDirection" text,
"ResearchDirectionId" integer,
"ResearchTheme" text,
"Tasks" text,
"SalaryFrom" integer,
"SalaryTo" integer,
"Bonuses" text,
"ContractType" integer,
"ContractTime" integer,
"SocialPackage" bit,
"Rent" bit,
"OfficeAccomodation" bit,
"TransportCompensation" bit,
"RegionId" integer,
"CityName" text,
"Details" text,

CONSTRAINT "Position_pkey" PRIMARY KEY ("Guid")
);


