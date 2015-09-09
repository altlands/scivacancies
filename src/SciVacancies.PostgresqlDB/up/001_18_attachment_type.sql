SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

SET search_path = public, pg_catalog;

CREATE TABLE "d_attachmenttypes"
(
	"id" integer NOT NULL, 
	"title" text,

CONSTRAINT "attachmenttype_pkey" PRIMARY KEY ("id")
);

INSERT INTO "d_attachmenttypes" VALUES ('1','Решение комиссии');
INSERT INTO "d_attachmenttypes" VALUES ('2','Резюме');
INSERT INTO "d_attachmenttypes" VALUES ('3','Прочее');

ALTER TABLE "org_vacancy_attachments" ADD COLUMN "type_id" integer NOT NULL REFERENCES d_attachmenttypes(id);

ALTER TABLE "res_vacancyapplication_attachments" ADD COLUMN "type_id" integer NOT NULL REFERENCES d_attachmenttypes(id);