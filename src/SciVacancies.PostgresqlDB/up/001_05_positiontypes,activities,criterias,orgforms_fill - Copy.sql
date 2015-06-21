SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

SET search_path = public, pg_catalog;

--Руководители научной организации
INSERT INTO "PositionTypes" VALUES('01ce3840-2607-51ce-863f-30ffb837979b',1,'Заместитель директора (начальника) организации по научной работе');
INSERT INTO "PositionTypes" VALUES('0dd433cc-e8ea-a747-32e1-d9b3c8522ee3',2,'Главный (генеральный) конструктор');
INSERT INTO "PositionTypes" VALUES('12367b62-c919-a9b5-c95d-d70fec1c72f0',3,'Директор (начальник) отделения (института, центра) в составе организации');
INSERT INTO "PositionTypes" VALUES('3a3a8553-4ff3-60fd-28f8-8693a1b2137e',4,'Руководитель научного и (или) научно-технического проекта');

--Руководители структурных подразделений организации, осуществляющих научную, научно-техническую деятельность, а также деятельность по использованию научных и (или) научно-технических результатов
INSERT INTO "PositionTypes" VALUES('584612f5-3db5-a63a-e29d-ad1504e32fb2',5,'Заведующий (начальник) научно-исследовательского отдела (лаборатории)');
INSERT INTO "PositionTypes" VALUES('6f9000ee-6912-6440-6423-dac701a854f5',6,'Заведующий (начальник) конструкторского отдела (лаборатории)');
INSERT INTO "PositionTypes" VALUES('71a82e92-bd95-d832-b7df-9b8c5b6ef70e',7,'Заведующий (начальник) отдела (центра) патентования');
INSERT INTO "PositionTypes" VALUES('757426f4-20d4-c8bd-b97f-81e1cccbe415',8,'Заведующий (начальник) отдела (центра) научной и (или) научно-технической информации');
INSERT INTO "PositionTypes" VALUES('7b14a396-c5fe-231c-1763-8215db2f44cb',9,'Заведующий (начальник) отдела (центра) научной и (или) научно-технической библиотеки');
INSERT INTO "PositionTypes" VALUES('8c5ec81e-7ec6-e9bc-fa19-49d853effcdb',10,'Заведующий (начальник) отдела (центра) коммерциализации результатов научной и (или) научно-технической деятельности');

--Сотрудники подразделений, осуществляющих научную, научно-техническую деятельность, в том числе конструирование и экспериментальное производство
INSERT INTO "PositionTypes" VALUES('8d21cfb4-cf4d-3706-7718-26f3ac82bf02',11,'Главный научный сотрудник');
INSERT INTO "PositionTypes" VALUES('9f19d672-6f2f-bfbd-200e-c8a0d4147881',12,'Ведущий научный сотрудник');
INSERT INTO "PositionTypes" VALUES('ab0f07af-7e1a-abc5-9729-09c946447e90',13,'Старший научный сотрудник');
INSERT INTO "PositionTypes" VALUES('b15133cd-12ee-2da6-cb38-7d80db86a4b0',14,'Научный сотрудник');
INSERT INTO "PositionTypes" VALUES('b7280ace-d237-c007-42fe-ec4aed8f52d4',15,'Младший научный сотрудник/инженер-исследователь');

--Stub activities
INSERT INTO "Activities" VALUES('b7b752b1-486b-d5d4-1bc2-8dc173a92b4e','Разведение лазерных акул',1);
INSERT INTO "Activities" VALUES('c3073466-a452-6e77-7f78-1d461ffa8b3e','Уменьшение энтропии чёрных дыр',2);
INSERT INTO "Activities" VALUES('c9cbeca4-e04f-a722-cf20-86a168ef70d0','Головология',3);
INSERT INTO "Activities" VALUES('cacbae66-7cd3-ab78-915a-cf5f6671338d','Исследование всего',4);
INSERT INTO "Activities" VALUES('d4e1bcc5-0b2e-5310-eb91-37a12b625305','Системный анализ',5);

--Stub Criterias
INSERT INTO "Criterias" VALUES('d703d0f9-de7b-068b-24da-b4d9ff7fe8e2',NULL,1,'Количество лазерных акул разведено','Laser');
INSERT INTO "Criterias" VALUES('eadd07fc-8330-f1cf-5e7f-93a2d3a2e0dd',1,2,'Количество публикаций','Publ');
INSERT INTO "Criterias" VALUES('21d94536-ca48-f863-d10e-3d686702850c',NULL,3,'Количество использованных результатов интеллектуальной деятельности','Isp');
INSERT INTO "Criterias" VALUES('43a82624-2882-a5db-2e25-5f6bb8571ee1',NULL,4,'Число статей про чёрные дыры','Blho');
INSERT INTO "Criterias" VALUES('45503e8b-5218-808a-807a-bdbc1f3af2df',4,5,'Количество дыр в кармане','PcH');

--Stub  OrgForms
INSERT INTO "OrgForms" VALUES('7178a8be-10cf-5ae6-81f8-1f6b8a263d59',1,'Корпорация зла');
INSERT INTO "OrgForms" VALUES('9dbc1a3a-0c7a-e9ba-6527-0717f11da768',2,'Коллаборация');
INSERT INTO "OrgForms" VALUES('9ea4bd8a-1765-3511-2882-de8f32337e3a',3,'Институт');
INSERT INTO "OrgForms" VALUES('bd10df51-f369-8dd6-5b57-7bbbc75efbbb',4,'Кластер');
INSERT INTO "OrgForms" VALUES('f2de2fa2-b621-afa7-97da-7ec4123084d3',5,'Лаборатория "Umbrella"');