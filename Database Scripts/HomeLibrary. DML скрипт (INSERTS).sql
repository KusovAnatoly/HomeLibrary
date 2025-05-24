USE HomeLibrary;
GO

-- Авторы
INSERT INTO Author (FirstName, LastName, BirthYear)
VALUES 
    (N'Лев', N'Толстой', 1828),
    (N'Фёдор', N'Достоевский', 1821);

-- Издательства
INSERT INTO Publisher (PublisherName)
VALUES 
    (N'Русский вестник');

-- Жанры
INSERT INTO Genre (GenreName)
VALUES 
    (N'Роман'), (N'История'), (N'Философия');

-- Книги
INSERT INTO Book (Title, AuthorID, PublisherID, PublishYear, ISBN, TableOfContents)
VALUES 
(
    N'Война и мир',
    (SELECT AuthorID FROM Author WHERE LastName = N'Толстой'),
    (SELECT PublisherID FROM Publisher WHERE PublisherName = N'Русский вестник'),
    1869,
    '978-5-389-07403-6',
    '<Contents><Part>Часть 1</Part><Part>Часть 2</Part><Part>Часть 3</Part><Part>Часть 4</Part></Contents>'
),
(
    N'Преступление и наказание',
    (SELECT AuthorID FROM Author WHERE LastName = N'Достоевский'),
    (SELECT PublisherID FROM Publisher WHERE PublisherName = N'Русский вестник'),
    1866,
    '978-5-389-07410-4',
    '<Contents><Part>Часть 1</Part><Part>Часть 2</Part><Part>Часть 3</Part><Part>Часть 4</Part><Part>Эпилог</Part></Contents>'
);

-- Привязка книг к жанрам (многие ко многим)
INSERT INTO BookGenres (BookID, GenreID)
SELECT b.BookID, g.GenreID
FROM Book b
JOIN Genre g ON g.GenreName = N'Роман'
WHERE b.Title = N'Война и мир';

INSERT INTO BookGenres (BookID, GenreID)
SELECT b.BookID, g.GenreID
FROM Book b
JOIN Genre g ON g.GenreName = N'История'
WHERE b.Title = N'Война и мир';

INSERT INTO BookGenres (BookID, GenreID)
SELECT b.BookID, g.GenreID
FROM Book b
JOIN Genre g ON g.GenreName = N'Роман'
WHERE b.Title = N'Преступление и наказание';

INSERT INTO BookGenres (BookID, GenreID)
SELECT b.BookID, g.GenreID
FROM Book b
JOIN Genre g ON g.GenreName = N'Философия'
WHERE b.Title = N'Преступление и наказание';
