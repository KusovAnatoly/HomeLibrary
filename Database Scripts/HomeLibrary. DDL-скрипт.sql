-- Создание базы данных
CREATE DATABASE HomeLibrary;
GO

-- Предположим, база уже создана
USE HomeLibrary;
GO

-- Таблица авторов
CREATE TABLE Author (
    AuthorID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    BirthYear INT NULL
);
GO

-- Таблица издательств
CREATE TABLE Publisher (
    PublisherID INT IDENTITY(1,1) PRIMARY KEY,
    PublisherName NVARCHAR(255) NOT NULL UNIQUE
);
GO

-- Таблица жанров
CREATE TABLE Genre (
    GenreID INT IDENTITY(1,1) PRIMARY KEY,
    GenreName NVARCHAR(100) NOT NULL UNIQUE
);
GO

-- Таблица книг (без GenreID)
CREATE TABLE Book (
    BookID INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    AuthorID INT NOT NULL,
    PublisherID INT NULL,
    PublishYear INT CHECK (PublishYear >= 1440 AND PublishYear <= YEAR(GETDATE())),
    ISBN VARCHAR(20) NULL UNIQUE,
    TableOfContents XML NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_Books_Authors FOREIGN KEY (AuthorID) REFERENCES Author(AuthorID),
    CONSTRAINT FK_Books_Publishers FOREIGN KEY (PublisherID) REFERENCES Publisher(PublisherID)
);
GO

-- Таблица для связи многие-ко-многим: Книги <-> Жанры
CREATE TABLE BookGenres (
    BookID INT NOT NULL,
    GenreID INT NOT NULL,
    PRIMARY KEY (BookID, GenreID),
    CONSTRAINT FK_BookGenres_Books FOREIGN KEY (BookID) REFERENCES Book(BookID) ON DELETE CASCADE,
    CONSTRAINT FK_BookGenres_Genres FOREIGN KEY (GenreID) REFERENCES Genre(GenreID) ON DELETE CASCADE
);
GO

-- Индексы
CREATE INDEX IX_Books_Title ON Book (Title);
CREATE INDEX IX_Books_PublishYear ON Book (PublishYear);
