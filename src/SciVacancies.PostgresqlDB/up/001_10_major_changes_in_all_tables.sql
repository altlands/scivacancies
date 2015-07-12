SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

SET search_path = public, pg_catalog;

DROP TABLE "Activities";


ALTER TABLE "Foivs" DROP COLUMN "Guid";
ALTER TABLE "Foivs" RENAME COLUMN "Title" TO title;
ALTER TABLE "Foivs" RENAME COLUMN "ShortTitle" TO shorttitle;
ALTER TABLE "Foivs" RENAME COLUMN "Id" TO id;
ALTER TABLE "Foivs" RENAME COLUMN "ParentId" TO parent_id;
ALTER TABLE "Foivs" ADD CONSTRAINT "foiv_pkey" PRIMARY KEY ("id");
ALTER TABLE "Foivs" RENAME TO foivs;


ALTER TABLE "OrgForms" DROP COLUMN "Guid";
ALTER TABLE "OrgForms" RENAME COLUMN "Title" TO title;
ALTER TABLE "OrgForms" RENAME COLUMN "Id" TO id;
ALTER TABLE "OrgForms" ADD CONSTRAINT "orgform_pkey" PRIMARY KEY ("id");
ALTER TABLE "OrgForms" RENAME TO orgforms;


ALTER TABLE "ResearchDirections" DROP COLUMN "Guid";
ALTER TABLE "ResearchDirections" RENAME COLUMN "ParentId" TO parent_id;
ALTER TABLE "ResearchDirections" RENAME COLUMN "TitleEng" TO title_eng;
ALTER TABLE "ResearchDirections" RENAME COLUMN "Lft" TO lft;
ALTER TABLE "ResearchDirections" RENAME COLUMN "Rgt" TO rgt;
ALTER TABLE "ResearchDirections" RENAME COLUMN "Lvl" TO lvl;
ALTER TABLE "ResearchDirections" RENAME COLUMN "OecdCode" TO oecd_code;
ALTER TABLE "ResearchDirections" RENAME COLUMN "WosCode" TO wos_code;
ALTER TABLE "ResearchDirections" RENAME COLUMN "Root" TO root_id;
ALTER TABLE "ResearchDirections" RENAME COLUMN "Title" TO "title";
ALTER TABLE "ResearchDirections" RENAME COLUMN "Id" TO "id";
ALTER TABLE "ResearchDirections" ADD CONSTRAINT "researchdirection_pkey" PRIMARY KEY ("id");
ALTER TABLE "ResearchDirections" RENAME TO researchdirections;


ALTER TABLE "Regions" DROP COLUMN "Guid";
ALTER TABLE "Regions" RENAME COLUMN "Title" TO "title";
ALTER TABLE "Regions" RENAME COLUMN "Id" TO "id";
ALTER TABLE "Regions" RENAME COLUMN "FedDistrictId" TO feddistrict_id;
ALTER TABLE "Regions" RENAME COLUMN "OsmId" TO osm_id;
ALTER TABLE "Regions" RENAME COLUMN "Okato" TO okato;
ALTER TABLE "Regions" RENAME COLUMN "Slug" TO slug;
ALTER TABLE "Regions" RENAME COLUMN "Code" TO code;
ALTER TABLE "Regions" ADD CONSTRAINT "region_pkey" PRIMARY KEY ("id");
ALTER TABLE "Regions" RENAME TO regions;


ALTER TABLE "PositionTypes" DROP COLUMN "Guid";
ALTER TABLE "PositionTypes" RENAME COLUMN "Title" TO title;
ALTER TABLE "PositionTypes" RENAME COLUMN "Id" TO id;
ALTER TABLE "PositionTypes" ADD CONSTRAINT "positiontype_pkey" PRIMARY KEY ("id");
ALTER TABLE "PositionTypes" RENAME TO positiontypes;


ALTER TABLE "Criterias" DROP COLUMN "Guid";
ALTER TABLE "Criterias" RENAME COLUMN "Title" TO title;
ALTER TABLE "Criterias" RENAME COLUMN "Id" TO id;
ALTER TABLE "Criterias" RENAME COLUMN "ParentId" TO parent_id;
ALTER TABLE "Criterias" RENAME COLUMN "Code" TO code;
ALTER TABLE "Criterias" ADD CONSTRAINT "criteria_pkey" PRIMARY KEY ("id");
ALTER TABLE "Criterias" RENAME TO criterias;


ALTER TABLE "FavoriteVacancies" DROP CONSTRAINT "FavoriteVacancy_pkey";
ALTER TABLE "FavoriteVacancies" RENAME COLUMN "Guid" TO "guid";
ALTER TABLE "FavoriteVacancies" RENAME COLUMN "VacancyGuid" TO vacancy_guid;
ALTER TABLE "FavoriteVacancies" RENAME COLUMN "ResearcherGuid" TO researcher_guid;
ALTER TABLE "FavoriteVacancies" RENAME COLUMN "CreationDate" TO creation_date;
ALTER TABLE "FavoriteVacancies" ADD CONSTRAINT "favoritevacancy_pkey" PRIMARY KEY ("guid");
ALTER TABLE "FavoriteVacancies" RENAME TO favoritevacancies;


ALTER TABLE "Organizations" DROP CONSTRAINT "Organization_pkey";

ALTER TABLE "Organizations" RENAME COLUMN  "Guid" TO "guid";
ALTER TABLE "Organizations" RENAME COLUMN  "Name" TO name;
ALTER TABLE "Organizations" DROP COLUMN  "NameEng";
ALTER TABLE "Organizations" RENAME COLUMN  "ShortName" TO shortname;
ALTER TABLE "Organizations" DROP COLUMN  "ShortNameEng";
ALTER TABLE "Organizations" DROP COLUMN  "CityName";
ALTER TABLE "Organizations" RENAME COLUMN  "Address" TO "address";
ALTER TABLE "Organizations" DROP COLUMN  "Website";
ALTER TABLE "Organizations" RENAME COLUMN  "Email" TO email;
ALTER TABLE "Organizations" RENAME COLUMN  "INN" TO inn;
ALTER TABLE "Organizations" RENAME COLUMN  "OGRN" TO ogrn;
ALTER TABLE "Organizations" DROP COLUMN  "OrgForm";
ALTER TABLE "Organizations" RENAME COLUMN  "OrgFormId" TO orgform_id;
ALTER TABLE "Organizations" DROP COLUMN  "OrgFormGuid";
ALTER TABLE "Organizations" DROP COLUMN  "PublishedVacancies";
ALTER TABLE "Organizations" DROP COLUMN  "Foiv";
ALTER TABLE "Organizations" RENAME COLUMN  "FoivId" TO foiv_id;
ALTER TABLE "Organizations" DROP COLUMN  "FoivGuid";
ALTER TABLE "Organizations" DROP COLUMN  "Activity";
ALTER TABLE "Organizations" DROP COLUMN  "ActivityId";
ALTER TABLE "Organizations" DROP COLUMN "ActivityGuid";
ALTER TABLE "Organizations" RENAME COLUMN  "HeadFirstName" TO head_firstname;
ALTER TABLE "Organizations" RENAME COLUMN  "HeadLastName" TO head_secondname;
ALTER TABLE "Organizations" RENAME COLUMN  "HeadPatronymic" TO head_patronymic;
ALTER TABLE "Organizations" ADD COLUMN  "status" integer;
ALTER TABLE "Organizations" RENAME COLUMN  "CreationDate" TO creation_date;
ALTER TABLE "Organizations" RENAME COLUMN  "UpdateDate" TO update_date;

ALTER TABLE "Organizations" ADD CONSTRAINT "organization_pkey" PRIMARY KEY ("guid");
ALTER TABLE "Organizations" RENAME TO organizations;