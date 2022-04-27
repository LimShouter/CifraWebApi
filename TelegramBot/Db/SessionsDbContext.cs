using Microsoft.EntityFrameworkCore;
using SharedLibrary;

namespace WebServer.Db;

public class SessionsDbContext : DbContext
{
	public SessionsDbContext(DbContextOptions<SessionsDbContext> options) : base(options)
	{
	}
	public DbSet<SessionData> Sessions { get; set; }
	public DbSet<EnvironmentIndicators> EnvironmentIndicatorsList { get; set; }
	public DbSet<Agent> Agents { get; set; }
}