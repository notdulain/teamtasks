CREATE TABLE notifications (
    id         INT IDENTITY(1,1) PRIMARY KEY,
    message    NVARCHAR(500) NOT NULL,
    created_at DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
