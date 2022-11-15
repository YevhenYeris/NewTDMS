using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewTDBMS.API.Hateoas.Services;
using NewTDBMS.Domain.Entities;
using NewTDBMS.API;
using NewTDBMS.Service;

namespace NewTDBMS.API.Controllers;

[Route("api/db/{dBName}/tables/{tableName}/[controller]")]
[ApiController]
public class ColumnsController : ControllerBase
{
	private ITDBMSService _serice;
	private ColumnLinkGetter _colLinkGetter;
	private readonly string _gRPCAddress = "https://localhost:7159";
	
	public ColumnsController(
		ITDBMSService service,
		ColumnLinkGetter columnLinkGetter)
	{
		_serice = service;
		_colLinkGetter = columnLinkGetter;
	}

	[HttpPut("columnName")]
	public async Task<IActionResult> PutAsync(string dBName, string tableName, string columnName, [FromBody] string newName)
	{
		using (var channel = GrpcChannel.ForAddress(_gRPCAddress))
		{
			var client = new Columns.ColumnsClient(channel);
			var columnExistsReply = await client.ColumnExistsAsync(new ColumnExistsRequest()
			{
				DBName = dBName,
				TableName = tableName,
				ColumnName = columnName,
			});

			if (!columnExistsReply.ColumnExists)
			{
				return NotFound();
			}

			await client.RenameColumnAsync(new RenameColumnRequest()
			{
				DBName = dBName,
				TableName = tableName,
				ColumnName = columnName,
				NewName = newName,
			});

			return Ok();
		}

		//if (!_serice.ColumnExists(dBName, tableName, columnName)) return NotFound();

		//_serice.RenameColumn(dBName, tableName, columnName, newName);

		//return Ok();
	}

	[HttpPost]
	public IActionResult Post(string dBName, string tableName, Column column)
	{
		if (!_serice.TableExists(dBName, tableName)) return NotFound();

		_serice.CreateColumn(dBName, tableName, column);

		return Ok();
	}
}