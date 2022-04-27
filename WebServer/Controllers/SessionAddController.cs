using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebServer.Controllers;

[Route("add")]
public class SessionAddController : ControllerBase
{
	[HttpPost]
	public IActionResult Post([FromBody] object obj)
	{
		var session = JsonConvert.DeserializeObject(obj.ToString());
		return Ok();
	}
}