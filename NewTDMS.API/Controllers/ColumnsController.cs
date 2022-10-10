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

	[HttpGet("test")]
	public IActionResult GetTest([FromQuery] Model model)
	{
		return Ok();
	}
}

public class Model
{
	public string UserId { get; set; } = string.Empty;

    public int GroupId { get; set; }

    public DateTime StartOfWeek { get; set; }
}
