/* DELETE FROM StudentExercise;
DELETE FROM Student;
DELETE FROM Exercise;
DELETE FROM Instructor;
DELETE FROM Cohort;


ALTER TABLE StudentExercise DROP CONSTRAINT [FK_Exercise];
ALTER TABLE StudentExercise DROP CONSTRAINT [FK_Student];
ALTER TABLE StudentExercise DROP CONSTRAINT [FK_Instructor];
ALTER TABLE Student DROP CONSTRAINT [FK_StudentCohort];
ALTER TABLE Instructor DROP CONSTRAINT [FK_Cohort];

DROP TABLE IF EXISTS Cohort;
DROP TABLE IF EXISTS Instructor;
DROP TABLE IF EXISTS Exercise;
DROP TABLE IF EXISTS Student;
DROP TABLE IF EXISTS StudentExercise; */


CREATE TABLE Cohort (
    Id	INTEGER NOT NULL PRIMARY KEY IDENTITY,
    Name	varchar(80) NOT NULL UNIQUE
);

CREATE TABLE Instructor (
    Id	INTEGER NOT NULL PRIMARY KEY IDENTITY,
    FirstName	varchar(80) NOT NULL,
    LastName	varchar(80) NOT NULL,
    SlackHandle	varchar(80) NOT NULL,
    Specialty varchar(80),
    CohortId	INTEGER NOT NULL,
    CONSTRAINT FK_Cohort FOREIGN KEY(CohortId) REFERENCES Cohort(Id)
);

CREATE TABLE Exercise (
    Id	INTEGER NOT NULL PRIMARY KEY IDENTITY,
    Name	VARCHAR(80) NOT NULL,
    Language VARCHAR(80) NOT NULL
);


CREATE TABLE Student (
    Id	integer NOT NULL PRIMARY KEY IDENTITY,
    FirstName	varchar(80) NOT NULL,
    LastName	varchar(80) NOT NULL,
    SlackHandle	varchar(80) NOT NULL,
    CohortId	integer NOT NULL,
    CONSTRAINT FK_StudentCohort FOREIGN KEY(CohortId) REFERENCES Cohort(Id)
);

CREATE TABLE StudentExercise (
    Id	        INTEGER NOT NULL PRIMARY KEY IDENTITY,
    ExerciseId	INTEGER NOT NULL,
    StudentId 	INTEGER NOT NULL,
    InstructorId 	INTEGER NOT NULL,
    CONSTRAINT FK_Exercise FOREIGN KEY(ExerciseId) REFERENCES Exercise(Id),
    CONSTRAINT FK_Student FOREIGN KEY(StudentId) REFERENCES Student(Id),
    CONSTRAINT FK_Instructor FOREIGN KEY(InstructorId) REFERENCES Instructor(Id)
);


INSERT INTO Cohort VALUES ('Evening Cohort 1');
INSERT INTO Cohort VALUES ('Day Cohort 10');
INSERT INTO Cohort VALUES ('Day Cohort 11');
INSERT INTO Cohort VALUES ('Day Cohort 12');
INSERT INTO Cohort VALUES ('Day Cohort 13');
INSERT INTO Cohort VALUES ('Day Cohort 21');


INSERT INTO Exercise
(Name, Language)
VALUES
('ChickenMonkey', 'JavaScript');

INSERT INTO Exercise
(Name, Language)
VALUES
('Overly Excited', 'JavaScript');

INSERT INTO Exercise
(Name, Language)
VALUES
('Boy Bands & Vegetables', 'JavaScript');



INSERT INTO Instructor
(FirstName, LastName, SlackHandle, Specialty, CohortId)
    SELECT 'Steve',
            'Brownlee',
            '@coach',
            'Dad jokes',
            c.Id
    FROM Cohort c WHERE c.Name = 'Evening Cohort 1'
;

INSERT INTO Instructor
(FirstName, LastName, SlackHandle, Specialty, CohortId)
    SELECT 'Joe',
            'Shepherd',
            '@joes',
            'Analogies',
            c.Id
    FROM Cohort c WHERE c.Name = 'Day Cohort 13'
;

INSERT INTO Instructor
(FirstName, LastName, SlackHandle, Specialty, CohortId)
    SELECT 'Jisie',
            'David',
            '@jisie',
            'Student success',
            c.Id
    FROM Cohort c WHERE c.Name = 'Day Cohort 21'
;


INSERT INTO Student
(FirstName, LastName, SlackHandle, CohortId)
    SELECT 'Kate',
            'Williams',
            '@katerebekah',
            c.Id
    FROM Cohort c WHERE c.Name = 'Evening Cohort 1'
;

INSERT INTO Student
(FirstName, LastName, SlackHandle, CohortId)
    SELECT 'Ryan',
            'Tanay',
            '@ryan.tanay',
            c.Id
    FROM Cohort c WHERE c.Name = 'Day Cohort 10'
;

INSERT INTO Student
(FirstName, LastName, SlackHandle, CohortId)
    SELECT 'Juan',
            'Rodriguez',
            '@juanrod',
            c.Id
    FROM Cohort c WHERE c.Name = 'Day Cohort 11'
;




INSERT INTO StudentExercise
(ExerciseId, StudentId, InstructorId)
    SELECT  e.Id, s.Id, i.Id FROM Student s, Exercise e, Instructor i
    WHERE e.Name = 'Overly Excited'
    AND s.SlackHandle = '@ryan.tanay'
    AND i.SlackHandle = '@coach'
;


INSERT INTO StudentExercise
(ExerciseId, StudentId, InstructorId)
    SELECT  e.Id, s.Id, i.Id FROM Student s, Exercise e, Instructor i
    WHERE e.Name = 'Overly Excited'
    AND s.SlackHandle = '@katerebekah'
    AND i.SlackHandle = '@coach'
;


INSERT INTO StudentExercise
(ExerciseId, StudentId, InstructorId)
    SELECT  e.Id, s.Id, i.Id FROM Student s, Exercise e, Instructor i
    WHERE e.Name = 'ChickenMonkey'
    AND s.SlackHandle = '@juanrod'
    AND i.SlackHandle = '@joes'
;