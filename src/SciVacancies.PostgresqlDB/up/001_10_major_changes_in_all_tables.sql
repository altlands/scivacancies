SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

SET search_path = public, pg_catalog;

DROP TABLE "Activities";
DROP TABLE "Positions";
DROP TABLE "Notifications";

ALTER TABLE "Foivs" DROP COLUMN "Guid";
ALTER TABLE "Foivs" RENAME COLUMN "Title" TO title;
ALTER TABLE "Foivs" RENAME COLUMN "ShortTitle" TO shorttitle;
ALTER TABLE "Foivs" RENAME COLUMN "Id" TO id;
ALTER TABLE "Foivs" RENAME COLUMN "ParentId" TO parent_id;
ALTER TABLE "Foivs" ADD CONSTRAINT "foiv_pkey" PRIMARY KEY ("id");
ALTER TABLE "Foivs" RENAME TO d_foivs;


ALTER TABLE "OrgForms" DROP COLUMN "Guid";
ALTER TABLE "OrgForms" RENAME COLUMN "Title" TO title;
ALTER TABLE "OrgForms" RENAME COLUMN "Id" TO id;
ALTER TABLE "OrgForms" ADD CONSTRAINT "orgform_pkey" PRIMARY KEY ("id");
ALTER TABLE "OrgForms" RENAME TO d_orgforms;


ALTER TABLE "ResearchDirections" DROP COLUMN "Guid";
ALTER TABLE "ResearchDirections" RENAME COLUMN "Id" TO "id";
ALTER TABLE "ResearchDirections" RENAME COLUMN "ParentId" TO parent_id;
ALTER TABLE "ResearchDirections" RENAME COLUMN "Title" TO "title";
ALTER TABLE "ResearchDirections" RENAME COLUMN "TitleEng" TO title_eng;
ALTER TABLE "ResearchDirections" RENAME COLUMN "Lft" TO lft;
ALTER TABLE "ResearchDirections" RENAME COLUMN "Rgt" TO rgt;
ALTER TABLE "ResearchDirections" RENAME COLUMN "Lvl" TO lvl;
ALTER TABLE "ResearchDirections" RENAME COLUMN "OecdCode" TO oecd_code;
ALTER TABLE "ResearchDirections" RENAME COLUMN "WosCode" TO wos_code;
ALTER TABLE "ResearchDirections" RENAME COLUMN "Root" TO root_id;
ALTER TABLE "ResearchDirections" ADD CONSTRAINT "researchdirection_pkey" PRIMARY KEY ("id");
ALTER TABLE "ResearchDirections" RENAME TO d_researchdirections;


ALTER TABLE "Regions" DROP COLUMN "Guid";
ALTER TABLE "Regions" RENAME COLUMN "Id" TO "id";
ALTER TABLE "Regions" RENAME COLUMN "FedDistrictId" TO feddistrict_id;
ALTER TABLE "Regions" RENAME COLUMN "Title" TO "title";
ALTER TABLE "Regions" RENAME COLUMN "OsmId" TO osm_id;
ALTER TABLE "Regions" RENAME COLUMN "Okato" TO okato;
ALTER TABLE "Regions" RENAME COLUMN "Slug" TO slug;
ALTER TABLE "Regions" RENAME COLUMN "Code" TO code;
ALTER TABLE "Regions" ADD CONSTRAINT "region_pkey" PRIMARY KEY ("id");
ALTER TABLE "Regions" RENAME TO d_regions;


ALTER TABLE "PositionTypes" DROP COLUMN "Guid";
ALTER TABLE "PositionTypes" RENAME COLUMN "Id" TO id;
ALTER TABLE "PositionTypes" RENAME COLUMN "Title" TO title;
ALTER TABLE "PositionTypes" ADD CONSTRAINT "positiontype_pkey" PRIMARY KEY ("id");
ALTER TABLE "PositionTypes" RENAME TO d_positiontypes;


ALTER TABLE "Criterias" DROP COLUMN "Guid";
ALTER TABLE "Criterias" RENAME COLUMN "Id" TO id;
ALTER TABLE "Criterias" RENAME COLUMN "Title" TO title;
ALTER TABLE "Criterias" RENAME COLUMN "ParentId" TO parent_id;
ALTER TABLE "Criterias" RENAME COLUMN "Code" TO code;
ALTER TABLE "Criterias" ADD CONSTRAINT "criteria_pkey" PRIMARY KEY ("id");
ALTER TABLE "Criterias" RENAME TO d_criterias;


DROP TABLE "Organizations";
CREATE TABLE org_organizations
(
  guid uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
  name text NOT NULL,
  shortname text,
  address text NOT NULL,
  email text,
  inn text NOT NULL,
  ogrn text NOT NULL,
  head_firstname text,
  head_secondname text,
  head_patronymic text,
  foiv_id integer,
  orgform_id integer,
  status smallint NOT NULL DEFAULT 0,
  creation_date timestamp without time zone NOT NULL,
  update_date timestamp without time zone,
  CONSTRAINT organization_pkey PRIMARY KEY (guid)
);


DROP TABLE "Researchers";
CREATE TABLE res_researchers
(
  guid uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
  firstname text NOT NULL,
  firstname_eng text,
  secondname text NOT NULL,
  secondname_eng text,
  patronymic text NOT NULL,
  patronymic_eng text,
  previous_secondname text,
  previous_secondname_eng text,
  birthdate date NOT NULL,
  email text,
  extraemail text,
  phone text,
  extraphone text,
  nationality text,
  research_activity text,
  teaching_activity text,
  other_activity text,
  science_degree text,
  science_rank text,
  rewards text,
  memberships text,
  conferences text,
  status smallint NOT NULL DEFAULT 0,
  creation_date timestamp without time zone NOT NULL,
  update_date timestamp without time zone,
  CONSTRAINT researcher_pkey PRIMARY KEY (guid)
);


DROP TABLE "Publications";
CREATE TABLE res_publications
(
  guid uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
  title text NOT NULL,
  researcher_guid uuid NOT NULL REFERENCES res_researchers(guid) ON DELETE CASCADE,
  CONSTRAINT publication_pkey PRIMARY KEY (guid)
);


DROP TABLE "Educations";
CREATE TABLE res_educations
(
  guid uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
  city text,
  university_shortname text NOT NULL,
  faculty_shortname text NOT NULL,
  graduation_date date,
  degree text NOT NULL,
  researcher_guid uuid NOT NULL REFERENCES res_researchers(guid) ON DELETE CASCADE,
  CONSTRAINT education_pkey PRIMARY KEY (guid)
);


DROP TABLE "SearchSubscriptions";
CREATE TABLE res_searchsubscriptions
(
  guid uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),

  title text NOT NULL,
  query text NOT NULL,
  foiv_ids text,
  positiontype_ids text,
  region_ids text,
  researchdirection_ids text,
  salary_from integer,
  salary_to integer,
  vacancy_statuses text,
  researcher_guid uuid NOT NULL REFERENCES res_researchers(guid) ON DELETE CASCADE,
  status smallint NOT NULL DEFAULT 0,
  creation_date timestamp without time zone NOT NULL,
  update_date timestamp without time zone,
  CONSTRAINT searchsubscription_pkey PRIMARY KEY (guid)
);


DROP TABLE "Vacancies";
CREATE TABLE org_vacancies
(
  guid uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
  name text NOT NULL,
  fullname text NOT NULL,
  tasks text NOT NULL,
  researchtheme text,
  cityname text,
  details text,
  contact_name text NOT NULL,
  contact_email text NOT NULL,
  contact_phone text NOT NULL,
  contact_details text,
  salary_from integer,
  salary_to integer,
  bonuses text,
  contract_type smallint NOT NULL,
  contract_time integer,
  employment_type smallint NOT NULL,
  operatingschedule_type smallint NOT NULL,
  socialpackage boolean NOT NULL,
  rent boolean NOT NULL,
  officeaccomodation boolean NOT NULL,
  transportcompensation boolean NOT NULL,
  positiontype_id integer NOT NULL REFERENCES d_positiontypes(id),
  region_id integer NOT NULL REFERENCES d_regions(id),
  researchdirection_id integer NOT NULL REFERENCES d_researchdirections(id),
  winner_researcher_guid uuid,
  winner_vacancyapplication_guid uuid,
  winner_request_date timestamp without time zone,
  winner_response_date timestamp without time zone,
  is_winner_accept boolean,
  pretender_researcher_guid uuid,
  pretender_vacancyapplication_guid uuid,
  pretender_request_date timestamp without time zone,
  pretender_response_date timestamp without time zone,
  is_pretender_accept boolean,
  publish_date timestamp without time zone,
  committee_date timestamp without time zone,
  announcement_date timestamp without time zone,
  organization_guid uuid NOT NULL REFERENCES org_organizations(guid) ON DELETE CASCADE,
  status smallint NOT NULL DEFAULT 0,
  creation_date timestamp without time zone NOT NULL,
  update_date timestamp without time zone,
  CONSTRAINT vacancy_pkey PRIMARY KEY (guid)
);


DROP TABLE "VacancyApplications";
CREATE TABLE res_vacancyapplications
(
  guid uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),

  researcher_fullname text,
  position_name text,
  email text,
  extraemail text,
  phone text,
  extraphone text,
  research_activity text,
  teaching_activity text,
  other_activity text,
  science_degree text,
  science_rank text,
  rewards text,
  memberships text,
  conferences text,
  educations text,
  publications text,
  vacancy_guid uuid NOT NULL REFERENCES org_vacancies(guid) ON DELETE CASCADE,
  researcher_guid uuid NOT NULL REFERENCES res_researchers(guid) ON DELETE CASCADE,
  status smallint NOT NULL DEFAULT 0,
  creation_date timestamp without time zone NOT NULL,
  update_date timestamp without time zone,
  apply_date timestamp without time zone,
  CONSTRAINT vacancyapplication_pkey PRIMARY KEY (guid)
);


DROP TABLE "FavoriteVacancies";
CREATE TABLE res_favoritevacancies
(
  guid uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
  vacancy_guid uuid NOT NULL REFERENCES org_vacancies(guid) ON DELETE CASCADE,
  researcher_guid uuid NOT NULL REFERENCES res_researchers(guid) ON DELETE CASCADE,
  CONSTRAINT favoritevacancy_pkey PRIMARY KEY (guid)
);


DROP TABLE "Attachments";
CREATE TABLE res_attachments
(
  guid uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
  name text NOT NULL,
  size bigint NOT NULL,
  extension text NOT NULL,
  url text NOT NULL,
  vacancyapplication_guid uuid NOT NULL REFERENCES res_vacancyapplications(guid) ON DELETE CASCADE,
  upload_date timestamp without time zone NOT NULL,

  CONSTRAINT attachment_pkey PRIMARY KEY (guid)
);


CREATE TABLE "org_vacancycriterias"
(
  "guid" uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
  "criteria_id" integer REFERENCES d_criterias(id),
  "vacancy_guid" uuid NOT NULL REFERENCES org_vacancies(guid) ON DELETE CASCADE,
  "from" bigint,
  "to" bigint,
  CONSTRAINT "vacancycriteria_pkey" PRIMARY KEY ("guid")
);


CREATE TABLE "org_researchdirections"
(
  "guid" uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
  "researchdirection_id" integer NOT NULL REFERENCES d_researchdirections(id),
  "organization_guid" uuid NOT NULL REFERENCES org_organizations(guid) ON DELETE CASCADE,
  CONSTRAINT "org_researchdirection_pkey" PRIMARY KEY ("guid")
);


CREATE TABLE "org_notifications"
(
  "guid" uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
  "title" text NOT NULL,
  "vacancyapplication_guid" uuid NOT NULL REFERENCES res_vacancyapplications(guid) ON DELETE CASCADE,
  "organization_guid" uuid NOT NULL REFERENCES org_organizations(guid) ON DELETE CASCADE,
  "status" smallint NOT NULL DEFAULT 0,
  "creation_date" timestamp without time zone NOT NULL,
  "update_date" timestamp without time zone,
  CONSTRAINT "org_notification_pkey" PRIMARY KEY ("guid")
);


CREATE TABLE "res_notifications"
(
  "guid" uuid NOT NULL DEFAULT uuid_in((md5(((random())::text || (now())::text)))::cstring),
  "title" text NOT NULL,
  "vacancy_guid" uuid NOT NULL REFERENCES org_vacancies(guid) ON DELETE CASCADE,
  "researcher_guid" uuid NOT NULL REFERENCES res_researchers(guid) ON DELETE CASCADE,
  "status" smallint NOT NULL DEFAULT 0,
  "creation_date" timestamp without time zone NOT NULL,
  "update_date" timestamp without time zone,
  CONSTRAINT "res_notification_pkey" PRIMARY KEY ("guid")
);