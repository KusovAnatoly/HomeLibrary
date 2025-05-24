CREATE PROCEDURE InsertGenre
    @GenreName NVARCHAR(100)
AS
BEGIN
    INSERT INTO Genre (GenreName)
    VALUES (@GenreName);
END;
GO

CREATE PROCEDURE UpdateGenre
    @GenreID INT,
    @GenreName NVARCHAR(100)
AS
BEGIN
    UPDATE Genre
    SET GenreName = @GenreName
    WHERE GenreID = @GenreID;
END;
GO

CREATE PROCEDURE DeleteGenre
    @GenreID INT
AS
BEGIN
    DELETE FROM Genre WHERE GenreID = @GenreID;
END;
GO

CREATE PROCEDURE GetGenres
AS
BEGIN
    SELECT * FROM Genre;
END;
GO
