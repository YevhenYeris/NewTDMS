using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewTDBMS.Domain.Entities;
using NewTDBMS.Service;

namespace NewTDBMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DBsController : ControllerBase
{
	private ITDBMSService _service;

	public DBsController(ITDBMSService service)
	{
		_service = service;
	}

	[HttpGet]
	public IActionResult Get()
	{
		var dBs = _service.GetDBNames();

		if (!dBs.Any()) return NotFound();

		return Ok(dBs);
	}

	[HttpPost("{dBName}")]
	public IActionResult Post(string dBName)
	{
		_service.CreateDB(dBName);

		return Ok();
	}

	[HttpDelete("{dBName}")]
	public IActionResult Delete(string dBName)
	{
		if (!_service.DBExists(dBName)) return NotFound();

		_service.DeleteDB(dBName);

		return Ok();
	}
}
