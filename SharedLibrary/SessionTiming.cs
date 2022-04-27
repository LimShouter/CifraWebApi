namespace SharedLibrary;

public class SessionTiming
{
	public int Id { get; set; }
	public string StartTime{get;set; }
	public string EndTime{get;set;}
	public string IrradiationDuration{get;set;}
	public bool HasTechnicalBreak{get;set;}
	public string? BreakStartTime{get;set;}
	public string? BreakEndTime{get;set;}
}