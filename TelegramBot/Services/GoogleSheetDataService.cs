using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using SharedLibrary;
using WebServer.Db;

namespace WebServer.Controllers;

public class GoogleSheetDataService
{
	private string[] Scopes = {SheetsService.Scope.Spreadsheets};
	private string _applicationName = "cifra_app";
	private string _spreadsheetId = "111C4oaX-fKKPEOZs7x6HJw6GfWeEFCLoEsxtD3uwPf0";
	private string _dataSheet = "Data";
	private string _agentSheet = "ION";
	private string _environmentSheet = "Service";
	private string _timingSheet = "Timing";
	private string _authSheet = "Auth";
	private SheetsService _service;
	private SessionsDbContext _context;

	public GoogleSheetDataService()
	{
		GoogleCredential credential;
		using (var stream = new FileStream($"./cifra-347712-7455051e4b9a.json", FileMode.Open, FileAccess.Read))
		{
			credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
		}

		_service = new SheetsService(new BaseClientService.Initializer()
		{
			HttpClientInitializer = credential,
			ApplicationName = _applicationName
		});
	}
	
	public async Task RefreshData(SessionsDbContext context)
	{
		_context = context;
		

		await ReadAgents();
		await ReadEnvironment();
		await ReadSessions();
	}

	async Task ReadAgents()
	{
		var agents = new List<Agent>();
		int i = 1;
		while (true)
		{
			i++;
			var agentRange = $"{_agentSheet}!a{i}:o{i}";
			var dataRequest = _service.Spreadsheets.Values.Get(_spreadsheetId, agentRange);
			var dataResponse = await dataRequest.ExecuteAsync();
			if (dataResponse.Values == null)
			{
				break;
			}

			var rawAgentData = dataResponse.Values[0];
			var agent = new Agent()
			{
				Ion = rawAgentData[0].ToString(),
				Isotope = rawAgentData[1].ToString(),
				IsotopeEnvironment = rawAgentData[5].ToString(),
				ObjectSurfaceEnergy = (float) Convert.ToDouble(rawAgentData[6]),
				ObjectEnergySetupError = (float) Convert.ToDouble(rawAgentData[7]),
				Mileage = (float) Convert.ToDouble(rawAgentData[8]),
				MileageSetupError = (float) Convert.ToDouble(rawAgentData[9]),
				LPP = (float) Convert.ToDouble(rawAgentData[10]),
				LPPSetupError = (float) Convert.ToDouble(rawAgentData[11]),
				WireEnergy = (float) Convert.ToDouble(rawAgentData[12]),
				SessionId = Convert.ToInt32(rawAgentData[13].ToString()),
				EnvironmentId = rawAgentData[14].ToString(),
			};

			Console.WriteLine(JsonConvert.SerializeObject(rawAgentData));
			agents.Add(agent);
		}

		await _context.Agents.AddRangeAsync(agents);
		await _context.SaveChangesAsync(true);
	}

	async Task ReadEnvironment()
	{
		int i = 1;
		while(true)
		{
			i++;
			var environmentRange = $"{_environmentSheet}!a{i}:o{i}";
			var environmentRequest = _service.Spreadsheets.Values.Get(_spreadsheetId, environmentRange);
			var environmentResponse = await environmentRequest.ExecuteAsync();
			if (environmentResponse.Values == null)
			{
				break;
			}
			var rawEnvironmentData = environmentResponse.Values[0];
			var indicators = new EnvironmentIndicators()
			{
				Pressure = Convert.ToInt32(rawEnvironmentData[0]),
				Humidity = Convert.ToInt32(rawEnvironmentData[1]),
				Temperature = Convert.ToInt32(rawEnvironmentData[2]),
				ReadTime = rawEnvironmentData[3].ToString()
			};
			await _context.EnvironmentIndicatorsList.AddAsync(indicators);
		} 
		_context.SaveChanges();
	}

	async Task ReadSessions()
	{
		int i = 1;
		bool isDataDone = false;
		while (true)
		{
			i++;
			await Task.Delay(1500);
			var dataRange = $"{_dataSheet}!a{i}:ah{i}";
			var dataRequest = _service.Spreadsheets.Values.Get(_spreadsheetId, dataRange);
			var dataResponse = await dataRequest.ExecuteAsync();
			if (dataResponse.Values == null || dataResponse.Values[0] == null)
			{
				break;
			}
			var rawSessionData = dataResponse.Values[0];
			

			if (!int.TryParse(rawSessionData[0].ToString(), out int k))
			{
				continue;
			}

			var timingRange = $"{_timingSheet}!a{i}:l{i}";
			var timingRequest = _service.Spreadsheets.Values.Get(_spreadsheetId, timingRange);
			var timingResponse = await timingRequest.ExecuteAsync();
			if (timingResponse.Values == null)
			{
				break;
			}
			
			Console.WriteLine(JsonConvert.SerializeObject(rawSessionData));

			var rawTimingData = timingResponse.Values[0];
			var timing = new SessionTiming()
			{
				StartTime = rawTimingData[3].ToString(),
				EndTime = rawTimingData[4].ToString(),
				IrradiationDuration = rawTimingData[5].ToString(),
				HasTechnicalBreak = rawTimingData[9].ToString() != String.Empty,
			};
			if (timing.HasTechnicalBreak)
			{
				timing.BreakStartTime = rawTimingData[9].ToString();
				timing.BreakEndTime = rawTimingData[10].ToString();
			}


			var sessionData = new SessionData() { };

			sessionData.SessionNumber = Convert.ToInt32(rawSessionData[0].ToString());
			sessionData.Organization = rawSessionData[1].ToString();
			sessionData.Objects = new string(rawSessionData[2] + " " + rawSessionData[3] + " " + rawSessionData[4] +
			                                 " " + rawSessionData[5]);
			sessionData.Agent = _context.Agents.First(o => o.EnvironmentId == rawSessionData[6].ToString());

			var sessionIndicators = rawSessionData[25].ToString();
			if (sessionIndicators != String.Empty && _context.EnvironmentIndicatorsList.Any(o => o.Pressure - Convert.ToDouble(rawSessionData[25]) < 000.1f))
			{
				sessionData.Indicators = _context.EnvironmentIndicatorsList
					.Where(o => o.Pressure - Convert.ToDouble(rawSessionData[25]) < 000.1f).First();
			}
			sessionData.Timing = timing;
			sessionData.TDAverage = (float) Convert.ToDouble(rawSessionData[7]);
			sessionData.TD1 = (float) Convert.ToDouble(rawSessionData[8]);
			sessionData.TD2 = (float) Convert.ToDouble(rawSessionData[9]);
			sessionData.TD3 = rawSessionData[10].ToString() != string.Empty? (float) Convert.ToDouble(rawSessionData[10].ToString()):null;
			sessionData.TD4 = rawSessionData[11].ToString() != string.Empty?(float) Convert.ToDouble(rawSessionData[11]):null;
			sessionData.TD5 = rawSessionData[12].ToString() != string.Empty?(float) Convert.ToDouble(rawSessionData[12]):null;
			sessionData.TD6 = rawSessionData[13].ToString() != string.Empty?(float) Convert.ToDouble(rawSessionData[13]):null;
			sessionData.TD7 = rawSessionData[14].ToString() != string.Empty?(float) Convert.ToDouble(rawSessionData[14]):null;
			sessionData.TD8 = rawSessionData[15].ToString() != string.Empty?(float) Convert.ToDouble(rawSessionData[15]):null;
			sessionData.TD9 = rawSessionData[16].ToString() != string.Empty?(float) Convert.ToDouble(rawSessionData[16]):null;
			sessionData.ODAverage = (float) Convert.ToDouble(rawSessionData[17]);
			sessionData.OD1 = rawSessionData[18].ToString() != string.Empty?(float) Convert.ToDouble(rawSessionData[18]):null;
			sessionData.OD2 = rawSessionData[19].ToString() != string.Empty?(float) Convert.ToDouble(rawSessionData[19]):null;
			sessionData.OD3 = rawSessionData[20].ToString() != string.Empty?(float) Convert.ToDouble(rawSessionData[20]):null;
			sessionData.OD4 = rawSessionData[21].ToString() != string.Empty?(float) Convert.ToDouble(rawSessionData[21]):null;
			sessionData.SessionFlowIntensity = rawSessionData[22].ToString() != string.Empty?(float) Convert.ToDouble(rawSessionData[22]):null;
			sessionData.AdmissionReportNumber = rawSessionData[23].ToString();
			sessionData.Angle = rawSessionData[24].ToString() != string.Empty?(float) Convert.ToDouble(rawSessionData[24]):null;
			sessionData.SessionTemperature = rawSessionData[28].ToString() != string.Empty?(float) Convert.ToDouble(rawSessionData[28]):null;
			sessionData.Heterogeneity = rawSessionData[29].ToString() != string.Empty?(float) Convert.ToDouble(rawSessionData[29]):null;

			if (rawSessionData.Count > 32)
			{
				sessionData.LeftHeterogeneity = (float) Convert.ToDouble(rawSessionData[32]);
				sessionData.RightHeterogeneity = (float) Convert.ToDouble(rawSessionData[33]);
			}
			


			sessionData.Error = rawSessionData[31].ToString() != string.Empty?(float) Convert.ToDouble(rawSessionData[31]):null;
			sessionData.K = rawSessionData[30].ToString() != string.Empty?(float) Convert.ToDouble(rawSessionData[30]):null;

			await _context.Sessions.AddAsync(sessionData);
			await _context.SaveChangesAsync();
		}
	}

	public async void AddNewSession(SessionData data)
	{
		var values = new List<object?>();
		values.Add(data.SessionNumber);
		values.Add(data.Organization);
		if (data.Objects != string.Empty)
		{
			values.Add((data.Objects.Split(" ")[0] == null)?data.Objects.Split(" ")[3]:null);
			values.Add((data.Objects.Split(" ")[0] == null)?data.Objects.Split(" ")[3]:null);
			values.Add((data.Objects.Split(" ")[0] == null)?data.Objects.Split(" ")[3]:null);
			values.Add((data.Objects.Split(" ")[0] == null)?data.Objects.Split(" ")[3]:null);
		}
		else
		{
			values.Add(null);
			values.Add(null);
			values.Add(null);
			values.Add(null);
		}
		values.Add(data.Agent.EnvironmentId);
		values.Add(data.TDAverage);
		values.Add(data.TD1);
		values.Add(data.TD2);
		values.Add(data.TD3);
		values.Add(data.TD4);
		values.Add(data.TD5);
		values.Add(data.TD6);
		values.Add(data.TD7);
		values.Add(data.TD8);
		values.Add(data.TD9);
		values.Add(data.ODAverage);
		values.Add(data.OD1);
		values.Add(data.OD2);
		values.Add(data.OD3);
		values.Add(data.OD4);
		values.Add(data.SessionFlowIntensity);
		values.Add(data.AdmissionReportNumber);
		values.Add(data.Angle);
		values.Add(data.Indicators?.Pressure);
		values.Add(data.Indicators?.Humidity);
		values.Add(data.Indicators?.Temperature);
		values.Add(data.SessionTemperature);
		values.Add(data.Heterogeneity);
		values.Add(data.K);
		values.Add(data.Error);
		values.Add(data.LeftHeterogeneity);
		values.Add(data.RightHeterogeneity);
		

		var dataRequest = _service.Spreadsheets.Values.Append(new ValueRange() {Values = new List<IList<object>>() {values}}, _spreadsheetId, ($"{_dataSheet}!A:AH"));
		dataRequest.ValueInputOption =
			SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;
		dataRequest.InsertDataOption =
			SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum.INSERTROWS;
		var appendResponse = await dataRequest.ExecuteAsync();
	}

	public async void UpdateSession(SessionData data)
	{
		int id = 2;
		var searchRequest = _service.Spreadsheets.Values.Get(_spreadsheetId, $"{_dataSheet}!A2:A500");
		var searchResponse = await searchRequest.ExecuteAsync();
		for (int i = 2; i < searchResponse.Values.Count; i++)
		{
			string value = searchResponse.Values[i][0].ToString();
			if (value != String.Empty)
			{
				if (Convert.ToInt32(value) == data.SessionNumber)
				{
					id = i;
					break;
				}
			}
			else
			{
				continue;
			}
		}

		var values = new List<object?>();
		values.Add(data.SessionNumber);
		values.Add(data.Organization);
		if (data.Objects != string.Empty)
		{
			values.Add((data.Objects.Split(" ")[0] == null)?data.Objects.Split(" ")[3]:null);
			values.Add((data.Objects.Split(" ")[0] == null)?data.Objects.Split(" ")[3]:null);
			values.Add((data.Objects.Split(" ")[0] == null)?data.Objects.Split(" ")[3]:null);
			values.Add((data.Objects.Split(" ")[0] == null)?data.Objects.Split(" ")[3]:null);
		}
		else
		{
			values.Add(null);
			values.Add(null);
			values.Add(null);
			values.Add(null);
		}
		values.Add(data.Agent.EnvironmentId);
		values.Add(data.TDAverage);
		values.Add(data.TD1);
		values.Add(data.TD2);
		values.Add(data.TD3);
		values.Add(data.TD4);
		values.Add(data.TD5);
		values.Add(data.TD6);
		values.Add(data.TD7);
		values.Add(data.TD8);
		values.Add(data.TD9);
		values.Add(data.ODAverage);
		values.Add(data.OD1);
		values.Add(data.OD2);
		values.Add(data.OD3);
		values.Add(data.OD4);
		values.Add(data.SessionFlowIntensity);
		values.Add(data.AdmissionReportNumber);
		values.Add(data.Angle);
		values.Add(data.Indicators?.Pressure);
		values.Add(data.Indicators?.Humidity);
		values.Add(data.Indicators?.Temperature);
		values.Add(data.SessionTemperature);
		values.Add(data.Heterogeneity);
		values.Add(data.K);
		values.Add(data.Error);
		values.Add(data.LeftHeterogeneity);
		values.Add(data.RightHeterogeneity);
		var changeRequest = _service.Spreadsheets.Values.Update(new ValueRange()
		{
			Values = new List<IList<object>>()
			{
				values
			}
		}, _spreadsheetId, $"{_dataSheet}!a{id}:ah{id}");
	}
}