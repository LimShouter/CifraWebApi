namespace PDF_Report;

public abstract class MonitoringReport
{
	public Dictionary<string, string> ParamsReplace { get; set; }

	public abstract string GetHtml();
}