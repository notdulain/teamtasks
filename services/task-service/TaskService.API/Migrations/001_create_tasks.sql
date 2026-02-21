CREATE TABLE tasks (
    id               INT IDENTITY(1,1) PRIMARY KEY,
    title            NVARCHAR(200) NOT NULL,
    assigned_user_id INT NOT NULL,
    status           NVARCHAR(50) NOT NULL DEFAULT 'open'
);
