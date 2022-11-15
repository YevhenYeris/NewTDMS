using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewTDBMS.API.Hateoas.Models;
using NewTDBMS.API.Hateoas.Services;
using NewTDBMS.Domain.Entities;
using NewTDBMS.Service;
using NewTDBMS.Service.Validation;

namespace NewTDBMS.API.Controllers;

[Route("api/db/{dBName}/tables/{tableName}/[controller]")]
[ApiController]
public class RowsController : ControllerBase
{
	private ITDBMSService _service;
	private readonly RowLinkGetter _rowLinkGetter;

	public RowsController(
		ITDBMSService service,
		RowLinkGetter rowLinkGetter)
	{
		_service = service;
		_rowLinkGetter = rowLinkGetter;
	}

	[HttpGet]
	public IActionResult Get(string dBName, string tableName)
	{
		var rows = _service.GetRows(dBName, tableName);

		if (rows is null) return NotFound();

		var result = rows.Select(row => new HateoasModelWrapper<Row>(row, _rowLinkGetter.GetLinks(dBName, tableName, row.Id)));

		return Ok(result);
	}

	[HttpPost]
	public IActionResult Post(string dBName, string tableName, [FromBody] Row row)
	{
		if (!_service.TableExists(dBName, tableName)) return NotFound();

		try
		{
			_service.AddRow(dBName, tableName, row);
		}
		catch(ValidationException ex)
		{
			return BadRequest($"Validation error: {ex.Message}");
		}

		return Ok();
	}

	[HttpPut]
	public IActionResult Put(string dBName, string tableName, [FromBody] Row row)
	{
		if (!_service.RowExists(dBName, tableName, row.Id)) return NotFound();

		_service.UpdateRow(dBName, tableName, row);

		return Ok();
	}

	[HttpDelete("{rowId}")]
	public IActionResult Delete(string dBName, string tableName, int rowId)
	{
		if (!_service.RowExists(dBName, tableName, rowId)) return NotFound();

		_service.DeleteRow(dBName, tableName, rowId);

		return Ok();
	}
}
