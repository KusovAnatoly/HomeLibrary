-- Вставка автора
CREATE PROCEDURE InsertAuthor
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @BirthYear INT = NULL
AS
BEGIN
    INSERT INTO Author (FirstName, LastName, BirthYear)
    VALUES (@FirstName, @LastName, @BirthYear);
END;
GO

-- Обновление автора
CREATE PROCEDURE UpdateAuthor
    @AuthorID INT,
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @BirthYear INT = NULL
AS
BEGIN
    UPDATE Author
    SET FirstName = @FirstName, LastName = @LastName, BirthYear = @BirthYear
    WHERE AuthorID = @AuthorID;
END;
GO

-- Удаление автора
CREATE PROCEDURE DeleteAuthor
    @AuthorID INT
AS
BEGIN
    DELETE FROM Author WHERE AuthorID = @AuthorID;
END;
GO

-- Получение авторов
CREATE PROCEDURE GetAuthors
AS
BEGIN
    SELECT * FROM Author;
END;
GO
