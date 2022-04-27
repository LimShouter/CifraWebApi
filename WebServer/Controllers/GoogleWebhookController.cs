using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebServer.Controllers;

[Route("google/onadd")]
public class GoogleWebhookController : ControllerBase
{
	[HttpPost]
	public IActionResult Post([FromBody]string obj)
	{
		var mainNode = JsonConvert.DeserializeObject<Dictionary<string, object>>(obj);
		Console.WriteLine(obj);
		

		return BadRequest();
	}

	private IActionResult PostSession()
	{
		return Ok();
	}
	private IActionResult PostAgent()
	{
		return Ok();
	}
	private IActionResult PostEnvironmentIndicators()
	{
		return Ok();
	}
	
	private IActionResult PostAuth()
	{
		return Ok();
	}
}