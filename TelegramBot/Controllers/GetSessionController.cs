using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary;
using WebServer.Db;

namespace WebServer.Controllers;

[Route("Session")]
[ApiController]
public class GetSessionController : ControllerBase
{
	private readonly SessionsDbContext _dataContext;
	private readonly GoogleSheetDataService _dataService;

	public GetSessionController(SessionsDbContext dataContext,GoogleSheetDataService dataService)
	{
		_dataContext = dataContext;
		_dataService = dataService;
	}
	[HttpGet]
	[Route("Single")]
	public IActionResult GetSession(int sessionId)
	{
		return Ok(_dataContext.Sessions.Include(ol => ol.Agent).Include(op => op.Indicators).Include(ok=>ok.Timing).Where(om=> om.SessionNumber == sessionId).First());
	}

	[HttpGet]
	[Route("All")]
	public async Task<IActionResult> GetAllSessions()
	{
		return Ok(_dataContext.Sessions.Include(o => o.Agent).Include(op => op.Indicators).Include(ok=>ok.Timing));
	}
}