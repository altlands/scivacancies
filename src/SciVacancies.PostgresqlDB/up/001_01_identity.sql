CREATE SCHEMA IF NOT EXISTS public
;

CREATE TABLE "public"."AspNetRoles"("Id" varchar(128) NOT NULL DEFAULT '',"Name" text NOT NULL DEFAULT '',CONSTRAINT "PK_public.AspNetRoles" PRIMARY KEY ("Id"))
;

CREATE TABLE "public"."AspNetUserRoles"("UserId" varchar(128) NOT NULL DEFAULT '',"RoleId" varchar(128) NOT NULL DEFAULT '',CONSTRAINT "PK_public.AspNetUserRoles" PRIMARY KEY ("UserId","RoleId"))
;

CREATE INDEX "AspNetUserRoles_IX_UserId" ON "public"."AspNetUserRoles" ("UserId")
;

CREATE INDEX "AspNetUserRoles_IX_RoleId" ON "public"."AspNetUserRoles" ("RoleId")
;

CREATE TABLE "public"."AspNetUsers"("Id" varchar(128) NOT NULL DEFAULT '',"Email" text,"EmailConfirmed" boolean NOT NULL DEFAULT FALSE,"PasswordHash" text,"SecurityStamp" text,"PhoneNumber" text,"PhoneNumberConfirmed" boolean NOT NULL DEFAULT FALSE,"TwoFactorEnabled" boolean NOT NULL DEFAULT FALSE,"LockoutEndDateUtc" timestamp,"LockoutEnabled" boolean NOT NULL DEFAULT FALSE,"AccessFailedCount" int4 NOT NULL DEFAULT 0,"UserName" text NOT NULL DEFAULT '',CONSTRAINT "PK_public.AspNetUsers" PRIMARY KEY ("Id"))
;

CREATE TABLE "public"."AspNetUserClaims"("Id" serial4 NOT NULL,"UserId" varchar(128) NOT NULL DEFAULT '',"ClaimType" text,"ClaimValue" text,CONSTRAINT "PK_public.AspNetUserClaims" PRIMARY KEY ("Id"))
;

CREATE INDEX "AspNetUserClaims_IX_UserId" ON "public"."AspNetUserClaims" ("UserId")
;

CREATE TABLE "public"."AspNetUserLogins"("LoginProvider" varchar(128) NOT NULL DEFAULT '',"ProviderKey" varchar(128) NOT NULL DEFAULT '',"UserId" varchar(128) NOT NULL DEFAULT '',CONSTRAINT "PK_public.AspNetUserLogins" PRIMARY KEY ("LoginProvider","ProviderKey","UserId"))
;

CREATE INDEX "AspNetUserLogins_IX_UserId" ON "public"."AspNetUserLogins" ("UserId")
;

ALTER TABLE "public"."AspNetUserRoles" ADD CONSTRAINT "FK_public.AspNetUserRoles_public.AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "public"."AspNetRoles" ("Id") ON DELETE CASCADE
;

ALTER TABLE "public"."AspNetUserRoles" ADD CONSTRAINT "FK_public.AspNetUserRoles_public.AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "public"."AspNetUsers" ("Id") ON DELETE CASCADE
;

ALTER TABLE "public"."AspNetUserClaims" ADD CONSTRAINT "FK_public.AspNetUserClaims_public.AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "public"."AspNetUsers" ("Id") ON DELETE CASCADE
;

ALTER TABLE "public"."AspNetUserLogins" ADD CONSTRAINT "FK_public.AspNetUserLogins_public.AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "public"."AspNetUsers" ("Id") ON DELETE CASCADE
;
