
CREATE TABLE "Student" (
  "StudentId" INT IDENTITY NOT NULL,
  "UserId" NVARCHAR(450) NOT NULL UNIQUE,
  "University" NVARCHAR(50) NOT NULL,
  "Faculty" NVARCHAR(50) NOT NULL,
  "StudyYear" INT NOT NULL,
  "Skills" NVARCHAR(500) NOT NULL,
  "HasAccess" BIT DEFAULT 0,
  CONSTRAINT [pk_Student] PRIMARY KEY ("StudentId"),
  CONSTRAINT "fk_Student_AspNetUsers" FOREIGN KEY ("UserId") 
  REFERENCES "AspNetUsers" ("Id"))


CREATE TABLE "Course" (
  "CourseId" INT IDENTITY NOT NULL,
  "TeacherId" NVARCHAR(450) NOT NULL,
  "CreatedBy" NVARCHAR(450) NULL,
  "CreatedAt" DATE NULL DEFAULT CURRENT_TIMESTAMP,
  "StartDate" DATE NOT NULL,
  "EndDate" DATE NOT NULL
  CONSTRAINT [pk_Course] PRIMARY KEY ("CourseId"),
  CONSTRAINT "fk_Course_Teacher_AspNetUsers" FOREIGN KEY ("TeacherId") 
  REFERENCES "AspNetUsers" ("Id"),
  CONSTRAINT "fk_Course_CreatedBy_AspNetUsers" FOREIGN KEY ("CreatedBy") 
  REFERENCES "AspNetUsers" ("Id"))


CREATE TABLE "Subject" (
  "SubjectId" INT IDENTITY NOT NULL,
  "Name" NVARCHAR(64) NOT NULL,
  "Description" NVARCHAR(256) NULL
  CONSTRAINT [pk_Subject] PRIMARY KEY ("SubjectId"))



CREATE TABLE "Module" (
  "ModuleId" INT IDENTITY NOT NULL,
  "Name" NVARCHAR(64) NULL,
  "Description" NVARCHAR(256) NULL,
  "HasTest" BIT NULL,
  "HasLab" BIT NULL,
  "MinTestMark" INT NULL,
  "MaxTestMark" INT NULL,
  "MinLabMark" INT NULL,
  "MaxLabMark" INT NULL,
  "SubjectId" INT NULL
  CONSTRAINT "pk_Module" PRIMARY KEY ("ModuleId"),
  CONSTRAINT "fk_Module_Subject" FOREIGN KEY ("SubjectId") 
  REFERENCES "Subject" ("SubjectId"))



CREATE TABLE "CourseStudent" (
  "CourseStudentId" INT IDENTITY NOT NULL,
  "StudentId" INT NULL,
  "CourseId" INT NULL,
  CONSTRAINT "pk_CourseStudent" PRIMARY KEY ("CourseStudentId"),
    CONSTRAINT "fk_CourseStudent_Student" FOREIGN KEY ("StudentId")
    REFERENCES "Student" ("StudentId"),
    CONSTRAINT "fk_CourseStudent_Course" FOREIGN KEY ("CourseId")
    REFERENCES "Course" ("CourseId"))


CREATE TABLE "CourseModule" (
  "CourseModuleId" INT IDENTITY NOT NULL,
  "CourseId" INT NOT NULL,
  "ModuleId" INT NOT NULL,
  CONSTRAINT "pk_CourseModule" PRIMARY KEY ("CourseModuleId"),
    CONSTRAINT "fk_CourseModule_Course" FOREIGN KEY ("CourseId")
    REFERENCES "Course" ("CourseId"),
    CONSTRAINT "fk_CourseModule_Module" FOREIGN KEY ("ModuleId")
    REFERENCES "Module" ("ModuleId"))

CREATE TABLE "Mark" (
  "MarkId" INT IDENTITY NOT NULL,
  "TestMark" INT NULL,
  "LabMark" INT NULL,
  "StudentId" INT NOT NULL,
  "CourseModuleId" INT NOT NULL
  CONSTRAINT "pk_Mark" PRIMARY KEY ("MarkId")
  CONSTRAINT "fk_Mark_Student"
    FOREIGN KEY ("StudentId")
    REFERENCES "Student" ("StudentId"),
  CONSTRAINT "fk_Mark_CourseModule"
    FOREIGN KEY ("CourseModuleId")
    REFERENCES "CourseModule" ("CourseModuleId"))