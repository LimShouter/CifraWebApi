using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using SharedLibrary;

namespace WebServer.Controllers;

public class GoogleSheetDataImportService
{
	private string[] Scopes = {SheetsService.Scope.Spreadsheets};
	private string _applicationName = "cifra_app";
	private string _spreadsheetId = "111C4oaX-fKKPEOZs7x6HJw6GfWeEFCLoEsxtD3uwPf0";
	private string _dataSheet = "";
	private string _agentSheet = "";
	private string _environmentSheet = "";
	private SheetsService _service;

	public void RefreshData()
	{
		GoogleCredential credential;
		using (var stream = new FileStream("cifra-347712-7455051e4b9a", FileMode.Open, FileAccess.Read))
		{
			credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
		}

		_service = new SheetsService(new BaseClientService.Initializer()
		{
			HttpClientInitializer = credential,
			ApplicationName = _applicationName
		});
	}

	void ReadAgents()
	{
		int i = 1;
		bool isAgentsDone = false;
		do
		{
			i++;
			var agentRange = $"{_agentSheet}!a{i}:o{i}";
			var dataRequest = _service.Spreadsheets.Values.Get(_spreadsheetId, agentRange);
			var dataResponse = dataRequest.Execute();
			var rawAgentData = dataResponse.Values[0];
			var agent = new Agent()
			{
				Id = (int) rawAgentData[3]
			};
		} while (isAgentsDone);
	}
	
	void ReadEnvironment()
	{
		int i = 1;
		bool isAgentsDone = false;
		do
		{
			i++;
			var environmentRange = $"{_environmentSheet}!a{i}:o{i}";
			var environmentRequest = _service.Spreadsheets.Values.Get(_spreadsheetId, environmentRange);
			var environmentResponse = environmentRequest.Execute();
			var rawEnvironmentData = environmentResponse.Values[0];
			var agent = new EnvironmentIndicators()
			{
				Pressure = (int) rawEnvironmentData[0]
			};
		} while (isAgentsDone);
	}

	void ReadSessions()
	{
		int i = 1;
		bool isDataDone = false;
		do
		{
			i++;
			var dataRange = $"{_dataSheet}!a{i}:ah{i}";
			var dataRequest = _service.Spreadsheets.Values.Get(_spreadsheetId, dataRange);
			var dataResponse = dataRequest.Execute();
			var rawSessionData = dataResponse.Values[0];
			var sessionData = new SessionData()
			{
				Id = i,
				SessionNumber = (int) rawSessionData[0],
				Organization = rawSessionData[1].ToString(),
				ObjectID = new []
				{
					rawSessionData[2].ToString(),
					rawSessionData[3].ToString(),
					rawSessionData[4].ToString(),
					rawSessionData[5].ToString(),
				},
				TrackDetectorsAverage = (float) rawSessionData[7],
				TrackDetectors = new[]
				{
					(float) rawSessionData[8],
					(float) rawSessionData[9],
					(float) rawSessionData[10],
					(float) rawSessionData[11],
					(float) rawSessionData[12],
					(float) rawSessionData[13],
					(float) rawSessionData[14],
					(float) rawSessionData[15],
					(float) rawSessionData[16],
				},
				ODAverage = (float) rawSessionData[17],
				OnlineDetectors = new[]
				{
					(float) rawSessionData[18],
					(float) rawSessionData[19],
					(float) rawSessionData[20],
					(float) rawSessionData[21],
				},
				SessionFlowIntensity = (float) rawSessionData[22],
				Angle = (float) rawSessionData[22],
				SessionTemperature = (float) rawSessionData[0],
				Heterogeneity = (float) rawSessionData[0],
				LeftHeterogeneity = (float) rawSessionData[0],
				RightHeterogeneity = (float) rawSessionData[0],
				Error = (float) rawSessionData[0],
				K = (float) rawSessionData[0],
			};
			isDataDone = rawSessionData[0].ToString() == string.Empty;
		} while (!isDataDone);
	}

	void AddNewSession()
	{
		var dataRequest = _service.Spreadsheets.Values.Append(new ValueRange()
		{
			Values = new List<IList<object>>()
			{
				new List<object>()
			}
		},_spreadsheetId,($"{_dataSheet}"));
	}
}