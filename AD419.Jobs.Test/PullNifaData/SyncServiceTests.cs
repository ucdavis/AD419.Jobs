using System.Data;
using System.Reflection;
using AD419.Jobs.Configuration;
using AD419.Jobs.Core.Configuration;
using AD419.Jobs.Core.Services;
using AD419.Jobs.PullNifaData.Services;
using Microsoft.Extensions.Options;
using Moq;
using Serilog;
using Shouldly;
using Xunit.Abstractions;

namespace AD419.Jobs.Test.PullNifaData;

public class SyncServiceTests
{
    private ITestOutputHelper _output;
    private Mock<IOptions<ConnectionStrings>> _mockConnectionStrings;
    private Mock<IOptions<SyncOptions>> _mockSyncOptions;
    private Mock<ISshService> _mockSshService;
    private Mock<ISqlDataContext> _mockSqlDataContext;


    // The following are state that can be checked in test assertions
    // Cleanup not necessary, because xUnit instantiates this class for each test
    private List<string> _sentQueries = new();

    public SyncServiceTests(ITestOutputHelper output)
    {
        _output = output;
        Log.Logger = new LoggerConfiguration()
            .WriteTo.TestOutput(output)
            .CreateLogger();
        _mockConnectionStrings = new Mock<IOptions<ConnectionStrings>>();
        _mockConnectionStrings.Setup(cs => cs.Value).Returns(new ConnectionStrings { DefaultConnection = "" });

        _mockSyncOptions = new Mock<IOptions<SyncOptions>>();
        _mockSyncOptions.Setup(so => so.Value).Returns(new SyncOptions());

        _mockSshService = new Mock<ISshService>();
        _mockSshService.Setup(ss => ss.DownloadFile(It.IsAny<string>())).Returns((string filename) =>
        {
            // Test data files are stored as embedded resources
            var assembly = Assembly.GetAssembly(typeof(SyncServiceTests))!;
            var stream = assembly.GetManifestResourceStream($"AD419.Jobs.Test.Data.{filename}")!;
            return stream;
        });

        _mockSqlDataContext = new Mock<ISqlDataContext>();
        _mockSqlDataContext
            .Setup(sdc => sdc.BeginTransaction())
            .Returns(Task.CompletedTask);
        _mockSqlDataContext
            .Setup(sdc => sdc.CommitTransaction())
            .Returns(Task.CompletedTask);
        _mockSqlDataContext
            .Setup(sdc => sdc.RollbackTransaction())
            .Returns(Task.CompletedTask);
        _mockSqlDataContext
            .Setup(sdc => sdc.ExecuteNonQuery(It.IsAny<string>()))
            .Callback<string>(_sentQueries.Add);
        _mockSqlDataContext
            .Setup(sdc => sdc.BulkCopy(It.IsAny<DataTable>(), It.IsAny<string>(), It.IsAny<int>()));
    }


    [Theory]
    [InlineData("NIFA_PGM_AWARD")]
    [InlineData("NIFA_PGM_EMPLOYEE")]
    [InlineData("NIFA_PGM_EXPENDITURE")]
    [InlineData("NIFA_PGM_PROJECT")]
    public async Task PgmExtractsAreSuccessfullyProcessed(string extractName)
    {
        // Arrange
        _mockSshService
            .Setup(ss => ss.ListFiles(It.IsAny<string>()))
            .Returns([$"{extractName}_Incremental_00000000_000000.csv"]);
        var syncService = new SyncService(_mockSqlDataContext.Object, _mockSyncOptions.Object, _mockSshService.Object);
        
        // Act
        await syncService.Run();

        // Assert
        // Each extract should generate three queries (ensure temp table, merge data, and truncate temp table)
        _sentQueries.Count(q => q.Contains(extractName)).ShouldBe(3);
        _mockSqlDataContext.Verify(sdc => sdc.BulkCopy(It.IsAny<DataTable>(), It.IsAny<string>(), It.IsAny<int>()));
    }
}