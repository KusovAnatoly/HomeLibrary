CREATE PROCEDURE InsertBook
    @Title NVARCHAR(255),
    @AuthorID INT,
    @PublisherID INT = NULL,
    @PublishYear INT,
    @ISBN VARCHAR(20) = NULL,
    @TableOfContents XML = NULL
AS
BEGIN
    INSERT INTO Book (Title, AuthorID, PublisherID, PublishYear, ISBN, TableOfContents)
    VALUES (@Title, @AuthorID, @PublisherID, @PublishYear, @ISBN, @TableOfContents);
END;
GO

CREATE PROCEDURE UpdateBook
    @BookID INT,
    @Title NVARCHAR(255),
    @AuthorID INT,
    @PublisherID INT = NULL,
    @PublishYear INT,
    @ISBN VARCHAR(20) = NULL,
    @TableOfContents XML = NULL
AS
BEGIN
    UPDATE Book
    SET Title = @Title,
        AuthorID = @AuthorID,
        PublisherID = @PublisherID,
        PublishYear = @PublishYear,
        ISBN = @ISBN,
        TableOfContents = @TableOfContents
    WHERE BookID = @BookID;
END;
GO

CREATE PROCEDURE DeleteBook
    @BookID INT
AS
BEGIN
    DELETE FROM Book WHERE BookID = @BookID;
END;
GO

CREATE PROCEDURE GetBooks
AS
BEGIN
    SELECT 
        b.BookID,
        b.Title,
        b.PublishYear,
        b.ISBN,
        b.TableOfContents,
        b.CreatedAt,
        a.FirstName + ' ' + a.LastName AS AuthorName,
        p.PublisherName
    FROM Book b
    INNER JOIN Author a ON b.AuthorID = a.AuthorID
    LEFT JOIN Publisher p ON b.PublisherID = p.PublisherID;
END;
GO
