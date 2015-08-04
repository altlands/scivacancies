SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

SET search_path = public, pg_catalog;

ALTER TABLE org_organizations ADD COLUMN image_name text;
ALTER TABLE org_organizations ADD COLUMN image_size bigint;
ALTER TABLE org_organizations ADD COLUMN image_extension text;
ALTER TABLE org_organizations ADD COLUMN image_url text;

ALTER TABLE res_researchers ADD COLUMN image_name text;
ALTER TABLE res_researchers ADD COLUMN image_size bigint;
ALTER TABLE res_researchers ADD COLUMN image_extension text;
ALTER TABLE res_researchers ADD COLUMN image_url text;

ALTER TABLE org_vacancycriterias RENAME COLUMN from TO count;
ALTER TABLE org_vacancycriterias DROP COLUMN to;