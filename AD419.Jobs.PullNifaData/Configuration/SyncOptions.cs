namespace AD419.Jobs.Configuration;

public class SyncOptions
{
    public int BulkCopyBatchSize { get; set; } = 1000;
    public string ProcessedFileLocation { get; set; } = "/Already Uploaded";
    public string FailedFileLocation{ get; set; } = "/Failed to Upload";
}
