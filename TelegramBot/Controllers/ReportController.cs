using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedLibrary;
using TelegramBot.Services;
using WebServer.Db;

namespace TelegramBot.Controllers;


[ApiController]
public class ReportController : ControllerBase
{
	private readonly EmailReportService _service;
	private readonly SessionsDbContext _context;

	public ReportController(EmailReportService service,SessionsDbContext context)
	{
		_service = service;
		_context = context;
	}
	
	[Route("Report")]
	[HttpPost]
	public IActionResult Post([FromBody]ReportHelper data)
	{
		List<SessionData> values = new List<SessionData>();
		var sessionDatas = _context.Sessions.Include(op=> op.Agent).Include(ok=> ok.Timing).Include(om => om.Indicators);
		var i = Convert.ToInt32(data.From);
		do
		{
			i++;
			values.Add(sessionDatas.First(o=>o.SessionNumber == i));
		} while (i<Convert.ToInt32(data.To));
		_service.SendNonStandartReports(values, data.Email);
		return Ok();
	}
}