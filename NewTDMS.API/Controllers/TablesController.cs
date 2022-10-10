using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewTDBMS.Domain.Entities;
using NewTDBMS.Service;
using NewTDBMS.Service.Validation;

namespace NewTDBMS.API.Controllers;

[Route("api/db/{dBName}/[controller]")]
[ApiController]
public class TablesController : ControllerBase
{
	private ITDBMSService _service;

	public TablesController(ITDBMSService service)
	{
		_service = service;
	}

	[HttpGet()]
	public IActionResult Get(string dBName)
	{
		var tables = _service.GetTableNames(dBName);

		if (!tables.Any()) return NotFound();

		return Ok(tables);
	}

	[HttpGet("{tableName}")]
	public IActionResult GetTable(string dBName, string tableName)
	{
		try
		{
			var table = _service.GetTable(dBName, tableName);

			if (table is null) return NotFound();
	
			return Ok(table);
		}
		catch(ArgumentException)
		{
			return BadRequest();
		}
	}

	[HttpPost]
	public IActionResult Post(string dBName, [FromBody] Table table)
	{
		if (!_service.DBExists(dBName)) return NotFound();

		try
		{
		_service.CreateTable(dBName, table);
		}
		catch(ValidationException ex)
		{
			return BadRequest(ex.Message);
		}

		return Ok();
	}

	[HttpDelete("{tableName}")]
	public IActionResult Delete(string dBName, string tableName)
	{
		if (!_service.TableExists(dBName, tableName)) return NotFound();

		_service.DeleteTable(dBName, tableName);

		return Ok();
	}
}
