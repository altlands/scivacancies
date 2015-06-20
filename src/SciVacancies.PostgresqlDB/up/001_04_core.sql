--RESEARCHER

CREATE TABLE "Researchers"
(
"Guid" uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),

"FirstName" text,
"FirstNameEng" text,
"SecondName" text,
"SecondNameEng" text,
"PreviouseSecondName" text,
"Patronymic" text,
"PatronymicEng" text,

"BirthDate" date,

"Email" text,
"ExtraEmail" text,
"Phone" text,
"ExtraPhone" text,

"Nationality" text,

"ResearchActivity" text,
"TeachingActivity" text,
"OtherActivity" text,

"ScienceDegree" text,
"AcademicStatus" text,
"Rewards" text,
"Memberships" text,
"Conferences" text,

"CreationDate" timestamp without time zone NOT NULL,
"UpdateDate" timestamp without time zone,

CONSTRAINT "Researcher_pkey" PRIMARY KEY ("Guid")
);

CREATE TABLE "Attachments"
(
"Guid" uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
"VacancyApplicationGuid" uuid NOT NULL,

"Name" text,
"Size" text,
"Type" text,
"Url" text,

"CreationDate" timestamp without time zone NOT NULL,

CONSTRAINT "Attachment_pkey" PRIMARY KEY ("Guid")
);

CREATE TABLE "Educations"
(
"Guid" uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
"ResearcherGuid" uuid NOT NULL,

"City" text,
"UniversityShortName" text,
"FacultyShortName" text,
"Degree" text,
"GraduationDate" date,

CONSTRAINT "Education_pkey" PRIMARY KEY ("Guid")
);

CREATE TABLE "FavoriteVacancies"
(
"Guid" uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
"VacancyGuid" uuid NOT NULL,
"ResearcherGuid" uuid NOT NULL,

"CreationDate"  timestamp without time zone NOT NULL,

CONSTRAINT "FavoriteVacancy_pkey" PRIMARY KEY ("Guid")
);

CREATE TABLE "Publications"
(
"Guid" uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
"ResearcherGuid" uuid NOT NULL,

"Name" text,

CONSTRAINT "Publication_pkey" PRIMARY KEY ("Guid")
);

CREATE TABLE "SearchSubscriptions"
(
"Guid" uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
"ResearcherGuid" uuid NOT NULL,

"Title" text,
"Status" integer,

"CreationDate" timestamp without time zone NOT NULL,
"UpdateDate" timestamp without time zone,

CONSTRAINT "SearchSubscription_pkey" PRIMARY KEY ("Guid")
);

CREATE TABLE "VacancyApplications"
(
"Guid" uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
"VacancyGuid" uuid NOT NULL,
"ResearcherGuid" uuid NOT NULL,

"PositionTypeId" integer,

"FullName" text,

"Email" text,
"ExtraEmail" text,
"Phone" text,
"ExtraPhone" text,

"ResearchActivity" text,
"TeachingActivity" text,
"OtherActivity" text,

"ScienceDegree" text,
"AcademicStatus" text,
"Rewards" text,
"Memberships" text,
"Conferences" text,

"Status" integer,

"CreationDate" timestamp without time zone NOT NULL,
"UpdateDate" timestamp without time zone,
"SentDate" timestamp without time zone,

CONSTRAINT "VacancyApplication_pkey" PRIMARY KEY ("Guid")
);

--ORGANIZATION

CREATE TABLE "Organizations"
(
"Guid" uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),

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
"OrgFormGuid" uuid,

"PublishedVacancies" integer,

"Foiv" text,
"FoivId" integer,
"FoivGuid" uuid,

"Activity" text,
"ActivityId" integer,
"ActivityGuid" uuid,

"HeadFirstName" text,
"HeadLastName" text,
"HeadPatronymic" text,

"CreationDate" timestamp without time zone NOT NULL,
"UpdateDate" timestamp without time zone,

CONSTRAINT "Organization_pkey" PRIMARY KEY ("Guid")
);

CREATE TABLE "Positions"
(
"Guid" uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
"OrganizationGuid" uuid,
"PositionTypeGuid" uuid,

"Name" text,
"FullName" text,

"ResearchDirection" text,
"ResearchDirectionId" integer,

"ResearchTheme" text,
"ResearchThemeId" integer,

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

"ContactName" text,
"ContactEmail" text,
"ContactPhone" text,
"ContactDetails" text,

"Status" integer,

"CreationDate" timestamp without time zone NOT NULL,
"UpdateDate" timestamp without time zone,

CONSTRAINT "Position_pkey" PRIMARY KEY ("Guid")
);

CREATE TABLE "Vacancies"
(
"Guid" uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
"OrganizationGuid" uuid,
"PositionGuid" uuid,
"PositionTypeGuid" uuid,
"WinnerGuid" uuid,
"PretenderGuid" uuid,

"Name" text,
"FullName" text,

"ResearchDirection" text,
"ResearchDirectionId" integer,

"ResearchTheme" text,
"ResearchThemeId" integer,

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

"Region" text,
"RegionId" integer,

"CityName" text,

"Details" text,

"ContactName" text,
"ContactEmail" text,
"ContactPhone" text,
"ContactDetails" text,

"Status" integer,

"FollowersCounter" integer,

"DateStart" date,
"DateStartAcceptance" date,
"DateFinishAcceptance" date,
"dateFinish" date,

"CreationDate" timestamp without time zone NOT NULL,

CONSTRAINT "Vacancy_pkey" PRIMARY KEY ("Guid")
);

--GENERAL

CREATE TABLE "Notifications"
(
"Guid" uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
"ResearcherGuid" uuid,
"OrganizationGuid" uuid,

"Title" text,

"Status" integer,

"CreationDate"  timestamp without time zone NOT NULL,
"UpdateDate" timestamp without time zone,

CONSTRAINT "Notification_pkey" PRIMARY KEY ("Guid")
);