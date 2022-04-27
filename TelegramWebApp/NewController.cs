using Microsoft.AspNetCore.Mvc;

namespace TelegramWebApp;

[ApiController]
[Route("new")]
public class NewController : ControllerBase
{
	[HttpGet]
	public IActionResult Get()
	{
		return Ok("1qaqwe");
	}
}