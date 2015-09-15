SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

SET search_path = public, pg_catalog;

ALTER TABLE "org_vacancies" ADD COLUMN "committee_end_date" timestamp without time zone;
ALTER TABLE "org_vacancies" RENAME COLUMN "committee_date" TO "committee_start_date";
ALTER TABLE "org_vacancies" ADD COLUMN "prolonging_reason" text;
ALTER TABLE "org_vacancies" RENAME COLUMN "close_reason" TO "committee_resolution";

