CREATE PROCEDURE InsertPublisher
    @PublisherName NVARCHAR(255)
AS
BEGIN
    INSERT INTO Publisher (PublisherName)
    VALUES (@PublisherName);
END;
GO

CREATE PROCEDURE UpdatePublisher
    @PublisherID INT,
    @PublisherName NVARCHAR(255)
AS
BEGIN
    UPDATE Publisher
    SET PublisherName = @PublisherName
    WHERE PublisherID = @PublisherID;
END;
GO

CREATE PROCEDURE DeletePublisher
    @PublisherID INT
AS
BEGIN
    DELETE FROM Publisher WHERE PublisherID = @PublisherID;
END;
GO

CREATE PROCEDURE GetPublishers
AS
BEGIN
    SELECT * FROM Publisher;
END;
GO
