SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

SET search_path = public, pg_catalog;

ALTER TABLE res_publications RENAME COLUMN "title" TO name;
ALTER TABLE res_publications ADD COLUMN "authors" text;
ALTER TABLE res_publications ADD COLUMN "type" text;
ALTER TABLE res_publications ADD COLUMN "ext_id" text;
ALTER TABLE res_publications ADD COLUMN "updated"  timestamp without time zone;
