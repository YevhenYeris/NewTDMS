using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewTDBMS.API.Hateoas.Models;
using NewTDBMS.API.Hateoas.Services;
using NewTDBMS.Domain.Entities;
using NewTDBMS.Service;

namespace NewTDBMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DBsController : ControllerBase
{
	private readonly ITDBMSService _service;
	private readonly DBLinkGetter _dBLinkGetter;

	public DBsController(
		ITDBMSService service,
		DBLinkGetter dBLinkGetter)
	{
		_service = service;
		_dBLinkGetter = dBLinkGetter;
	}

	[HttpGet]
	public IActionResult Get()
	{
		var dBs = _service.GetDBNames();

		if (!dBs.Any()) return NotFound();
		
		var result = dBs.Select(db => 
			new HateoasModelWrapper<string>(db, _dBLinkGetter.GetLinks(db)));

		return Ok(result);
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
