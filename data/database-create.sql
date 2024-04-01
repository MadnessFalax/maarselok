create table "school" (
	"id" INTEGER primary key autoincrement,
	"name" TEXT not null,
	"address" TEXT,
	"created" TIME not null default CURRENT_TIME
);

create table "program" (
	"id" INTEGER primary key autoincrement,
	"school_id" INTEGER not null references "school"("id"),
	"name" TEXT not null,
	"description" text,
	"capacity" INTEGER not null,
	"application_count" INTEGER not null default 0,
	"created" TIME not null default CURRENT_TIME,
	check (application_count < capacity)
);

create table "student" (
	"id" INTEGER primary key autoincrement,
	"name" TEXT not null,
	"address" TEXT not null,
	"email" TEXT not null,
	"application_count" INTEGER not null default 0,
	"created" TIME not null default CURRENT_TIME,
	check (application_count < 3)
);

create table "application" (
	"id" INTEGER primary key autoincrement,
	"student_id" INTEGER not null references "student"("id"), 
	"program_id" INTEGER not null references "program"("id"),
	"created" TIME not null default CURRENT_TIME
);