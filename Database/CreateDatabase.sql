-- Veritabanını oluştur
CREATE DATABASE StudentDB;
GO

USE StudentDB;
GO

-- Öğrenci tablosu
CREATE TABLE Students (
    StudentId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    DateOfBirth DATE,
    EnrollmentDate DATETIME2 DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1
);

-- Kurs tablosu
CREATE TABLE Courses (
    CourseId INT IDENTITY(1,1) PRIMARY KEY,
    CourseName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    Credits INT DEFAULT 3,
    IsActive BIT DEFAULT 1
);

-- Öğrenci-Kurs ilişki tablosu
CREATE TABLE StudentCourses (
    StudentCourseId INT IDENTITY(1,1) PRIMARY KEY,
    StudentId INT FOREIGN KEY REFERENCES Students(StudentId),
    CourseId INT FOREIGN KEY REFERENCES Courses(CourseId),
    EnrollmentDate DATETIME2 DEFAULT GETDATE(),
    Grade DECIMAL(5,2),
    UNIQUE(StudentId, CourseId)
);

-- Örnek veriler
INSERT INTO Students (FirstName, LastName, Email, DateOfBirth) VALUES
('Ahmet', 'Yilmaz', 'ahmet.yilmaz@email.com', '2000-05-15'),
('Ayse', 'Demir', 'ayse.demir@email.com', '1999-08-22'),
('Mehmet', 'Kaya', 'mehmet.kaya@email.com', '2001-03-10'),
('Fatma', 'Özkan', 'fatma.ozkan@email.com', '2000-11-30');

INSERT INTO Courses (CourseName, Description, Credits) VALUES
('Matematik', 'Temel matematik dersi', 4),
('Fizik', 'Temel fizik dersi', 3),
('Kimya', 'Temel kimya dersi', 3),
('Biyoloji', 'Temel biyoloji dersi', 3),
('Türkçe', 'Türkçe dil bilgisi', 2);

INSERT INTO StudentCourses (StudentId, CourseId, Grade) VALUES
(1, 1, 85.5),
(1, 2, 78.0),
(2, 1, 92.0),
(2, 3, 88.5),
(3, 2, 75.0),
(3, 4, 82.0),
(4, 1, 90.0),
(4, 5, 95.0); 