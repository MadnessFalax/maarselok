create table if not exists "School" (
	"Id" INTEGER primary key autoincrement,
	"Name" TEXT not null,
	"Address" TEXT,
	"Created" TEXT not null default CURRENT_TIME,
	"LastUpdated" TEXT not null default CURRENT_TIME 
);

create table if not exists "Program" (
	"Id" INTEGER primary key autoincrement,
	"SchoolId" INTEGER not null references "School"("Id"),
	"Name" TEXT not null,
	"Description" text,
	"Capacity" INTEGER not null,
	"ApplicationCount" INTEGER not null default 0,
	"Created" TEXT not null default CURRENT_TIME,
	"LastUpdated" TEXT not null default CURRENT_TIME,
	check (ApplicationCount < Capacity)
);

create table if not exists "Student" (
	"Id" INTEGER primary key autoincrement,
	"Name" TEXT not null,
	"Address" TEXT not null,
	"Email" TEXT not null,
	"ApplicationCount" INTEGER not null default 0,
	"Created" TEXT not null default CURRENT_TIME,
	"LastUpdated" TEXT not null default CURRENT_TIME, 
	check (ApplicationCount < 3)
);

create table if not exists "Application" (
	"Id" INTEGER primary key autoincrement,
	"StudentId" INTEGER not null references "Student"("Id"), 
	"ProgramId" INTEGER not null references "Program"("Id"),
	"Created" TEXT not null default CURRENT_TIME,
	"LastUpdated" TEXT not null default CURRENT_TIME 
);

create trigger if not exists "SchoolUpdated" after update on School
for each row
begin
	update School 
	set LastUpdated = CURRENT_TIME
	where Id = OLD.Id;
end;

create trigger if not exists "ProgramUpdated" after update on Program
for each row
begin
	update Program 
	set LastUpdated = CURRENT_TIME
	where Id = OLD.Id;
end;

create trigger if not exists "ApplicationUpdated" after update on Application
for each row
begin
	update application 
	set LastUpdated = CURRENT_TIME
	where Id = OLD.Id;
end;

create trigger if not exists "StudentUpdated" after update on Student
for each row
begin
	update Student 
	set LastUpdated = CURRENT_TIME
	where Id = OLD.Id;
end;