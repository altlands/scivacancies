SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

SET search_path = public, pg_catalog;

ALTER TABLE org_vacancies ADD COLUMN contract_time_new  real;
UPDATE org_vacancies t2 SET contract_time_new = t1.contract_time FROM org_vacancies t1 WHERE t2.guid = t1.guid;
ALTER TABLE org_vacancies DROP COLUMN contract_time;
ALTER TABLE org_vacancies RENAME COLUMN contract_time_new TO contract_time;