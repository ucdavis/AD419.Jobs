MERGE INTO ErpActivityValues AS target
USING #ErpActivityValues AS source
ON (target.Code = source.Code)
WHEN MATCHED THEN
  UPDATE SET
    target.Id = source.Id,
    target.Name = source.Name,
    target.EligibleForUse = source.EligibleForUse,
    target.LastUpdated = GETDATE(),
    target.IsActive = 1
WHEN NOT MATCHED BY target THEN
  INSERT (Id, Code, Name, EligibleForUse, LastUpdated, IsActive)
  VALUES (source.Id, source.Code, source.Name, source.EligibleForUse, GETDATE(), 1)
WHEN NOT MATCHED BY source THEN
  UPDATE SET
    target.IsActive = 0;

go