namespace NewTDBMS.gRPC.Services;
using Grpc.Core;
using NewTDBMS.gRPC;
using NewTDBMS.Service;

public class ColumnService : Columns.ColumnsBase
{
    private readonly ITDBMSService _tDBMSService;

    public ColumnService(ITDBMSService tDBMSService)
    {
        _tDBMSService = tDBMSService;
    }

    public override Task<ColumnExistsReply> ColumnExists(ColumnExistsRequest request, ServerCallContext context)
    {
        return Task.FromResult(new ColumnExistsReply
        {
            ColumnExists = _tDBMSService.ColumnExists(request.DBName, request.TableName, request.ColumnName)
        });
    }

    public override Task<RenameColumnReply> RenameColumn(RenameColumnRequest request, ServerCallContext context)
    {
        _tDBMSService.RenameColumn(request.DBName, request.TableName, request.ColumnName, request.NewName);
        return Task.FromResult(new RenameColumnReply());
    }
}
