﻿SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

SET search_path = public, pg_catalog;

ALTER TABLE "Positions" ADD COLUMN "EmploymentType" integer;
ALTER TABLE "Positions" ADD COLUMN "OperatingScheduleType" integer;
ALTER TABLE "Vacancies" ADD COLUMN "EmploymentType" integer;
ALTER TABLE "Vacancies" ADD COLUMN "OperatingScheduleType" integer;