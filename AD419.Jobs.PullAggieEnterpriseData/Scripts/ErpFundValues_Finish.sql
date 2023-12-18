MERGE INTO ErpFundValues AS target
USING #ErpFundValues AS source
ON (target.Code = source.Code)
WHEN MATCHED THEN
  UPDATE SET
    target.Id = source.Id,
    target.Name = source.Name,
    target.ParentCode = source.ParentCode,
    target.LastUpdated = GETDATE(),
    target.IsActive = 1
WHEN NOT MATCHED BY target THEN
  INSERT (Id, Code, Name, ParentCode, LastUpdated, IsActive)
  VALUES (source.Id, source.Code, source.Name, source.ParentCode, GETDATE(), 1)
WHEN NOT MATCHED BY source THEN
  UPDATE SET
    target.IsActive = 0;

go