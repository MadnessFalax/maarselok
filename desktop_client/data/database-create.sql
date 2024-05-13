BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "SchoolTable" (
	"Id"	INTEGER,
	"Name"	TEXT NOT NULL,
	"Address"	TEXT,
	"Created"	TEXT NOT NULL DEFAULT CURRENT_TIME,
	"LastUpdated"	TEXT NOT NULL DEFAULT CURRENT_TIME,
	PRIMARY KEY("Id" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "ProgramTable" (
	"Id"	INTEGER,
	"SchoolId"	INTEGER NOT NULL,
	"Name"	TEXT NOT NULL,
	"Description"	text,
	"Capacity"	INTEGER NOT NULL,
	"ApplicationCount"	INTEGER NOT NULL DEFAULT 0,
	"Created"	TEXT NOT NULL DEFAULT CURRENT_TIME,
	"LastUpdated"	TEXT NOT NULL DEFAULT CURRENT_TIME,
	CHECK("ApplicationCount" < "Capacity" + 1),
	FOREIGN KEY("SchoolId") REFERENCES "SchoolTable"("Id"),
	PRIMARY KEY("Id" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "StudentTable" (
	"Id"	INTEGER,
	"Name"	TEXT NOT NULL,
	"Address"	TEXT NOT NULL,
	"Email"	TEXT NOT NULL,
	"ApplicationCount"	INTEGER NOT NULL DEFAULT 0,
	"Created"	TIME NOT NULL DEFAULT CURRENT_TIME,
	"LastUpdated"	TEXT NOT NULL DEFAULT CURRENT_TIME,
	"Password"	TEXT NOT NULL DEFAULT 'TEST',
	CHECK("ApplicationCount" < 4),
	PRIMARY KEY("Id" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "ApplicationTable" (
	"Id"	INTEGER,
	"StudentId"	INTEGER NOT NULL,
	"ProgramId"	INTEGER NOT NULL,
	"Created"	TIME NOT NULL DEFAULT CURRENT_TIME,
	"LastUpdated"	TEXT NOT NULL DEFAULT CURRENT_TIME,
	FOREIGN KEY("StudentId") REFERENCES "StudentTable"("Id"),
	FOREIGN KEY("ProgramId") REFERENCES "ProgramTable"("Id"),
	PRIMARY KEY("Id" AUTOINCREMENT)
);
CREATE TRIGGER "SchoolUpdated" after update on "SchoolTable"
for each row
begin
	update "SchoolTable" 
	set LastUpdated = CURRENT_TIME
	where Id = OLD.Id;
end;
CREATE TRIGGER "ProgramUpdated" after update on "ProgramTable"
for each row
begin
	update "ProgramTable" 
	set LastUpdated = CURRENT_TIME
	where Id = OLD.Id;
end;
CREATE TRIGGER "ApplicationUpdated" after update on "ApplicationTable"
for each row
begin
	update "ApplicationTable" 
	set LastUpdated = CURRENT_TIME
	where Id = OLD.Id;
end;
CREATE TRIGGER "StudentUpdated" after update on "StudentTable"
for each row
begin
	update "StudentTable" 
	set LastUpdated = CURRENT_TIME
	where Id = OLD.Id;
end;
CREATE TRIGGER IncreaseApplicationCount
BEFORE INSERT ON ApplicationTable
BEGIN
    -- Check if SchoolTable.ApplicationCount < SchoolTable.Capacity - 1 for the SchoolId being inserted
    -- If not, raise a rollback exception
    SELECT RAISE(ROLLBACK, 'School is full') 
    FROM ProgramTable 
    WHERE ID = NEW.ProgramId AND ApplicationCount >= Capacity;

    -- Check if StudentTable.ApplicationCount < 2 for the StudentId being inserted
    -- If not, raise a rollback exception
    SELECT RAISE(ROLLBACK, 'Student already has 3 applications') 
    FROM StudentTable 
    WHERE ID = NEW.StudentId AND ApplicationCount >= 3;

    -- If all conditions are met, increment SchoolTable.ApplicationCount and StudentTable.ApplicationCount
    UPDATE ProgramTable 
    SET ApplicationCount = ApplicationCount + 1 
    WHERE ID = NEW.ProgramId;

    UPDATE StudentTable 
    SET ApplicationCount = ApplicationCount + 1 
    WHERE ID = NEW.StudentId;
END;
CREATE TRIGGER DecreaseApplicationCount
AFTER DELETE ON ApplicationTable
BEGIN
    -- Decrement ProgramTable.ApplicationCount for the ProgramId of the deleted row
    UPDATE ProgramTable 
    SET ApplicationCount = ApplicationCount - 1 
    WHERE ID = OLD.ProgramId;

    -- Decrement StudentTable.ApplicationCount for the StudentId of the deleted row
    UPDATE StudentTable 
    SET ApplicationCount = ApplicationCount - 1 
    WHERE ID = OLD.StudentId;
END;
COMMIT;
