SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

SET search_path = public, pg_catalog;

CREATE TABLE undispatched_commits
(
  bucketid character varying(40) NOT NULL,
  streamid character(40) NOT NULL,
  streamidoriginal character varying(1000) NOT NULL,
  streamrevision integer NOT NULL,
  items smallint NOT NULL,
  commitid uuid NOT NULL,
  commitsequence integer NOT NULL,
  commitstamp timestamp without time zone NOT NULL,
  checkpointnumber serial NOT NULL,
  dispatched boolean NOT NULL DEFAULT false,
  headers bytea,
  payload bytea NOT NULL
)