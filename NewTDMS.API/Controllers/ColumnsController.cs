using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewTDBMS.Domain.Entities;
using NewTDBMS.Service;

namespace NewTDBMS.API.Controllers;

[Route("api/db/{dBName}/tables/{tableName}/[controller]")]
[ApiController]
public class ColumnsController : ControllerBase
{
	private ITDBMSService _serice;
	
	public ColumnsController(ITDBMSService service)
	{
		_serice = service;
	}

	[HttpPut("columnName")]
	public IActionResult Put(string dBName, string tableName, string columnName, [FromBody] string newName)
	{
		if (!_serice.ColumnExists(dBName, tableName, columnName)) return NotFound();

		_serice.RenameColumn(dBName, tableName, columnName, newName);

		return Ok();
	}

	[HttpPost]
	public IActionResult Post(string dBName, string tableName, Column column)
	{
		if (!_serice.TableExists(dBName, tableName)) return NotFound();

		_serice.CreateColumn(dBName, tableName, column);

		return Ok();
	}
}