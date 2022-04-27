using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedLibrary;
using TelegramBot.Services;

namespace WebServer.Controllers;


[ApiController]
public class SessionAddController : ControllerBase
{
	private readonly DataHandleService _handle;

	public SessionAddController(DataHandleService handle)
	{
		_handle = handle;
	}
	
	[HttpPost]
	[Route("Add")]
	public IActionResult Post([FromBody]SessionData obj)
	{
		_handle.AddSessionOrUpdate(obj);
		return Ok();
	}
}