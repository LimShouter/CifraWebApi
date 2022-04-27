using Microsoft.EntityFrameworkCore;
using SharedLibrary;
using WebServer.Controllers;
using WebServer.Db;

namespace TelegramBot.Services;

public class DataHandleService
{
	private readonly SessionsDbContext _context;
	private readonly GoogleSheetDataService _sheets;

	public DataHandleService(SessionsDbContext context, GoogleSheetDataService sheets)
	{
		_context = context;
		_sheets = sheets;
	}

	public void AddSessionOrUpdate(SessionData sessionData)
	{
		if (!_context.Sessions.Any(o => o.SessionNumber == sessionData.SessionNumber))
		{
			_sheets.AddNewSession(sessionData);
			_context.Add(sessionData);
			_context.SaveChanges();
		}
		else
		{
			_sheets.UpdateSession(sessionData);

			_context.Agents.Add(sessionData.Agent);
			_context.EnvironmentIndicatorsList.Add(sessionData.Indicators);
			var session = _context.Sessions.First(o => o.SessionNumber == sessionData.SessionNumber);
			session.Agent = sessionData.Agent;
			session.Angle = sessionData.Angle;
			session.Error = sessionData.Error;
			session.Heterogeneity = sessionData.Heterogeneity;
			session.Indicators = sessionData.Indicators;
			session.K = sessionData.K;
			session.Objects = sessionData.Objects;
			session.Organization = sessionData.Organization;
			session.LeftHeterogeneity = sessionData.LeftHeterogeneity;
			session.OD1 = sessionData.OD1;
			session.OD2 = sessionData.OD2;
			session.OD3 = sessionData.OD3;
			session.OD4 = sessionData.OD4;
			session.RightHeterogeneity = sessionData.RightHeterogeneity;
			session.SessionNumber = sessionData.SessionNumber;
			session.SessionTemperature = sessionData.SessionTemperature;
			session.TD1 = sessionData.TD1;
			session.TD2 = sessionData.TD2;
			session.TD3 = sessionData.TD3;
			session.TD4 = sessionData.TD4;
			session.TD5 = sessionData.TD5;
			session.TD6 = sessionData.TD6;
			session.TD7 = sessionData.TD7;
			session.TD8 = sessionData.TD8;
			session.TD9 = sessionData.TD9;
			session.AdmissionReportNumber = sessionData.AdmissionReportNumber;
			session.ODAverage = sessionData.ODAverage;
			session.SessionFlowIntensity = sessionData.SessionFlowIntensity;
			session.TDAverage = sessionData.TDAverage;

			_context.SaveChanges();
		}
	}
}