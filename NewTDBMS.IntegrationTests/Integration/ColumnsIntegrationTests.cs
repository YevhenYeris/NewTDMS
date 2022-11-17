using Xunit.Abstractions;
using Grpc.AspNetCore.ClientFactory;
using System.Diagnostics.Metrics;
using NewTDBMS.API;

namespace NewTDBMS.Tests.Integration;

public class ColumnsIntegrationTests : IntegrationTestBase
{
    public ColumnsIntegrationTests(
        GrpcTestFixture<gRPC.Startup> fixture,
        ITestOutputHelper outputHelper)
        : base(fixture, outputHelper)
    {
    }

    [Fact]
    public void ColumnExists_ExistingColumn_ReturnsTrue()
    {
        // Arrange
        var client = new Columns.ColumnsClient(Channel);

        // Act
        var response = client.ColumnExists(new ColumnExistsRequest()
        {
            DBName = "TestDB",
            TableName = "TestTable",
            ColumnName = "TestColumn",
        });

        // Assert
        Assert.True(response.ColumnExists);
    }

    [Fact]
    public void ColumnExists_NotExistingColumn_ReturnsFalse()
    {
        // Arrange
        var client = new Columns.ColumnsClient(Channel);

        // Act
        var response = client.ColumnExists(new ColumnExistsRequest()
        {
            DBName = "NewDB",
            TableName = "NewTable",
            ColumnName = "PhantomColumn",
        });

        // Assert
        Assert.False(response.ColumnExists);
    }
}