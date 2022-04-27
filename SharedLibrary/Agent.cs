namespace SharedLibrary;

public class Agent
{
	public int Id { get; set; }
	public string Ion { get; set; }
	public string Isotope { get; set; }
	public string IsotopeEnvironment { get; set; }
	public float ObjectSurfaceEnergy { get; set; }
	public float ObjectEnergySetupError { get; set; }
	public float Mileage { get; set; }
	public float MileageSetupError { get; set; }
	public float LPP { get; set; }
	public float LPPSetupError { get; set; }
	public float WireEnergy { get; set; }
	public int SessionId { get; set; }
	public string EnvironmentId { get; set; }
}