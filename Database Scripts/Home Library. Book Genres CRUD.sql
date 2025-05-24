CREATE PROCEDURE InsertBookGenre
    @BookID INT,
    @GenreID INT
AS
BEGIN
    INSERT INTO BookGenres (BookID, GenreID)
    VALUES (@BookID, @GenreID);
END;
GO

CREATE PROCEDURE DeleteBookGenre
    @BookID INT,
    @GenreID INT
AS
BEGIN
    DELETE FROM BookGenres
    WHERE BookID = @BookID AND GenreID = @GenreID;
END;
GO

CREATE PROCEDURE GetGenresByBook
    @BookID INT
AS
BEGIN
    SELECT g.GenreID, g.GenreName
    FROM BookGenres bg
    INNER JOIN Genre g ON bg.GenreID = g.GenreID
    WHERE bg.BookID = @BookID;
END;
GO
