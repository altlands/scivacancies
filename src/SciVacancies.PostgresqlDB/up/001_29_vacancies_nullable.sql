SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

SET search_path = public, pg_catalog;


ALTER TABLE org_vacancies ADD COLUMN intreserve integer;
UPDATE  org_vacancies ov SET intreserve = ovs.positiontype_id FROM org_vacancies ovs WHERE ov.guid = ovs.guid;
ALTER TABLE org_vacancies DROP COLUMN positiontype_id;
ALTER TABLE org_vacancies ADD COLUMN positiontype_id integer;
UPDATE  org_vacancies ov SET positiontype_id = ovs.intreserve FROM org_vacancies ovs WHERE ov.guid = ovs.guid;
ALTER TABLE org_vacancies DROP COLUMN intreserve;


ALTER TABLE org_vacancies ADD COLUMN textreserve text;
UPDATE  org_vacancies ov SET textreserve = ovs.name FROM org_vacancies ovs WHERE ov.guid = ovs.guid;
ALTER TABLE org_vacancies DROP COLUMN name;
ALTER TABLE org_vacancies ADD COLUMN name text;
UPDATE  org_vacancies ov SET name = ovs.textreserve FROM org_vacancies ovs WHERE ov.guid = ovs.guid;
ALTER TABLE org_vacancies DROP COLUMN textreserve;


ALTER TABLE org_vacancies ADD COLUMN intreserve integer;
UPDATE  org_vacancies ov SET intreserve = ovs.researchdirection_id FROM org_vacancies ovs WHERE ov.guid = ovs.guid;
ALTER TABLE org_vacancies DROP COLUMN researchdirection_id;
ALTER TABLE org_vacancies ADD COLUMN researchdirection_id integer;
UPDATE  org_vacancies ov SET researchdirection_id = ovs.intreserve FROM org_vacancies ovs WHERE ov.guid = ovs.guid;
ALTER TABLE org_vacancies DROP COLUMN intreserve;


ALTER TABLE org_vacancies ADD COLUMN intreserve integer;
UPDATE  org_vacancies ov SET intreserve = ovs.region_id FROM org_vacancies ovs WHERE ov.guid = ovs.guid;
ALTER TABLE org_vacancies DROP COLUMN region_id;
ALTER TABLE org_vacancies ADD COLUMN region_id integer;
UPDATE  org_vacancies ov SET region_id = ovs.intreserve FROM org_vacancies ovs WHERE ov.guid = ovs.guid;
ALTER TABLE org_vacancies DROP COLUMN intreserve;


ALTER TABLE org_vacancies ADD COLUMN textreserve text;
UPDATE  org_vacancies ov SET textreserve = ovs.tasks FROM org_vacancies ovs WHERE ov.guid = ovs.guid;
ALTER TABLE org_vacancies DROP COLUMN tasks;
ALTER TABLE org_vacancies ADD COLUMN tasks text;
UPDATE  org_vacancies ov SET tasks = ovs.textreserve FROM org_vacancies ovs WHERE ov.guid = ovs.guid;
ALTER TABLE org_vacancies DROP COLUMN textreserve;


ALTER TABLE org_vacancies ADD COLUMN intreserve integer;
UPDATE  org_vacancies ov SET intreserve = ovs.salary_from FROM org_vacancies ovs WHERE ov.guid = ovs.guid;
ALTER TABLE org_vacancies DROP COLUMN salary_from;
ALTER TABLE org_vacancies ADD COLUMN salary_from integer;
UPDATE  org_vacancies ov SET salary_from = ovs.intreserve FROM org_vacancies ovs WHERE ov.guid = ovs.guid;
ALTER TABLE org_vacancies DROP COLUMN intreserve;


ALTER TABLE org_vacancies ADD COLUMN intreserve integer;
UPDATE  org_vacancies ov SET intreserve = ovs.salary_to FROM org_vacancies ovs WHERE ov.guid = ovs.guid;
ALTER TABLE org_vacancies DROP COLUMN salary_to;
ALTER TABLE org_vacancies ADD COLUMN salary_to integer;
UPDATE  org_vacancies ov SET salary_to = ovs.intreserve FROM org_vacancies ovs WHERE ov.guid = ovs.guid;
ALTER TABLE org_vacancies DROP COLUMN intreserve;


ALTER TABLE org_vacancies ADD COLUMN textreserve text;
UPDATE  org_vacancies ov SET textreserve = ovs.contact_name FROM org_vacancies ovs WHERE ov.guid = ovs.guid;
ALTER TABLE org_vacancies DROP COLUMN contact_name;
ALTER TABLE org_vacancies ADD COLUMN contact_name text;
UPDATE  org_vacancies ov SET contact_name = ovs.textreserve FROM org_vacancies ovs WHERE ov.guid = ovs.guid;
ALTER TABLE org_vacancies DROP COLUMN textreserve;


ALTER TABLE org_vacancies ADD COLUMN textreserve text;
UPDATE  org_vacancies ov SET textreserve = ovs.contact_email FROM org_vacancies ovs WHERE ov.guid = ovs.guid;
ALTER TABLE org_vacancies DROP COLUMN contact_email;
ALTER TABLE org_vacancies ADD COLUMN contact_email text;
UPDATE  org_vacancies ov SET contact_email = ovs.textreserve FROM org_vacancies ovs WHERE ov.guid = ovs.guid;
ALTER TABLE org_vacancies DROP COLUMN textreserve;


ALTER TABLE org_vacancies ADD COLUMN textreserve text;
UPDATE  org_vacancies ov SET textreserve = ovs.contact_phone FROM org_vacancies ovs WHERE ov.guid = ovs.guid;
ALTER TABLE org_vacancies DROP COLUMN contact_phone;
ALTER TABLE org_vacancies ADD COLUMN contact_phone text;
UPDATE  org_vacancies ov SET contact_phone = ovs.textreserve FROM org_vacancies ovs WHERE ov.guid = ovs.guid;
ALTER TABLE org_vacancies DROP COLUMN textreserve;