CREATE PROCEDURE [dbo].[Is_NC_AsInt]
    @EntityBId SMALLINT
   ,@EntityCId BIGINT
WITH NATIVE_COMPILATION, SCHEMABINDING
AS BEGIN ATOMIC WITH (TRANSACTION ISOLATION LEVEL = SNAPSHOT, LANGUAGE = N'us_english')
    DECLARE @Id INT;
	SELECT TOP 1 @Id = Id FROM [dbo].[MemoryOptimizedEntityA] WHERE EntityBId = @EntityBId AND EntityCId = @EntityCId;
    IF @Id IS NULL
    BEGIN
        INSERT INTO [dbo].[MemoryOptimizedEntityA] (EntityBId, EntityCId)
			VALUES (@EntityBId, @EntityCId);
        SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
    END
    ELSE SELECT @Id AS Id;
END
