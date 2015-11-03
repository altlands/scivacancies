SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

SET search_path = public, pg_catalog;

UPDATE "d_positiontypes" SET title = 'Заместитель директора (заведующего, начальника) по научной работе' WHERE id = 1;
UPDATE "d_positiontypes" SET title = 'Директор (заведующий, начальник) отделения (института, центра), находящегося в структуре организации' WHERE id = 3;
UPDATE "d_positiontypes" SET title = 'Руководитель научного и (или) научно-технического проекта' WHERE id = 4;
UPDATE "d_positiontypes" SET title = 'Заведующий (начальник) центра (отдела) (патентования, научной и (или) научно-технической информации, коллективного пользования научным оборудованием. коммерциализации результатов научной и (или) научно-технической деятельности)' WHERE id = 7;