using AutoFixture.Xunit2;
using Moq;
using NewTDBMS.gRPC.Services;
using NewTDBMS.Service;

namespace NewTDBMS.Tests.Unit;

public class ColumnsUnitTests
{
    [Fact]
    public async Task ColumnExists_ServiceReturnsTrue_ReturnTrue()
    {
        // Arrange
        var mockService = new Mock<ITDBMSService>();
        mockService.Setup(
            m => m.ColumnExists(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);
        var service = new ColumnService(mockService.Object);

        // Act
        var response = await service.ColumnExists(
            new gRPC.ColumnExistsRequest(),
            TestServerCallContext.Create());

        // Assert
        Assert.True(response.ColumnExists);
    }

    [Fact]
    public async Task ColumnExists_ServiceReturnsFalse_ReturnFalse()
    {
        // Arrange
        var mockService = new Mock<ITDBMSService>();
        mockService.Setup(
            m => m.ColumnExists(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false);
        var service = new ColumnService(mockService.Object);

        // Act
        var response = await service.ColumnExists(
            new gRPC.ColumnExistsRequest(),
            TestServerCallContext.Create());

        // Assert
        Assert.False(response.ColumnExists);
    }

    [Theory]
    [AutoData]
    public void RenameColumn_ExistsCalledNeverRenameCalledOnce(
        string dBName,
        string tableName,
        string oldColumnName,
        string newColumnName)
    {
        // Arrange
        var mockService = new Mock<ITDBMSService>();
        var service = new ColumnService(mockService.Object);

        // Act
        var response = service.RenameColumn(
            new gRPC.RenameColumnRequest()
            {
                DBName = dBName,
                TableName = tableName,
                ColumnName = oldColumnName,
                NewName = newColumnName,
            },
            TestServerCallContext.Create());

        // Assert
        mockService.Verify(m => m.ColumnExists(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        mockService.Verify(m => m.RenameColumn(dBName, tableName, oldColumnName, newColumnName), Times.Once);
    }
}
