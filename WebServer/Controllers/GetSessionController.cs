using Microsoft.AspNetCore.Mvc;
using SharedLibrary;
using WebServer.Db;

namespace WebServer.Controllers;

[Route("Session")]
[ApiController]
public class GetSessionController : ControllerBase
{
	private readonly SessionsDbContext _dataContext;

	public GetSessionController(SessionsDbContext dataContext)
	{
		_dataContext = dataContext;
	}
	[HttpGet]
	[Route("Single")]
	public IActionResult GetSession(int sessionId)
	{
		return Ok(_dataContext.Sessions.Where(o => o.SessionNumber == sessionId));
	}

	[HttpGet]
	[Route("All")]
	public IActionResult GetAllSessions()
	{
		return Ok(_dataContext.Sessions.ToList());
	}
}