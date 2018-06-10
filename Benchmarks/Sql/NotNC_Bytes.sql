﻿CREATE PROCEDURE [dbo].[NotNC_Bytes]
    @EntityBId smallint,
    @EntityCId bigint
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Id [int];

    MERGE INTO [dbo].[EntityA] AS [target]
    USING (VALUES (@EntityBId, @EntityCId))
        AS [source] (EntityBId, EntityCId)
        ON [target].[EntityBId] = [source].[EntityBId]
        AND [target].[EntityCId] = [source].[EntityCId]
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (EntityBId, EntityCId)
        VALUES (EntityBId, EntityCId)
    WHEN MATCHED THEN
        UPDATE SET @Id = [target].[Id];

    IF @Id IS NULL SELECT CONVERT(BINARY(4), SCOPE_IDENTITY());
    ELSE SELECT CONVERT(BINARY(4), @Id);
END