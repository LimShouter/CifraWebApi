namespace SharedLibrary;

public class SessionData
{
	public int Id { get; set; }
	public int SessionNumber { get; set; }
	public string? Organization { get; set; }
	public SessionTiming Timing { get; set; }
	public string Objects { get; set; }
	public Agent Agent { get; set; }
	public float? Angle { get; set; }
	public float? SessionFlowIntensity { get; set; }
	public EnvironmentIndicators? Indicators { get; set; }
	public float TDAverage { get; set; }
	public float? TD1 { get; set; }
	public float? TD2 { get; set; }
	public float? TD3 { get; set; }
	public float? TD4 { get; set; }
	public float? TD5 { get; set; }
	public float? TD6 { get; set; }
	public float? TD7 { get; set; }
	public float? TD8 { get; set; }
	public float? TD9 { get; set; }
	public float ODAverage { get; set; }
	public float? OD1 { get; set; }
	public float? OD2 { get; set; }
	public float? OD3 { get; set; }
	public float? OD4 { get; set; }
	public float? SessionTemperature { get; set; }
	public float? Heterogeneity { get; set; }
	
	public string? AdmissionReportNumber { get; set; }
	public float? LeftHeterogeneity { get; set; }
	public float? RightHeterogeneity { get; set; }
	public float? Error { get; set; }
	public float? K { get; set; }
}