using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Web;
using TelegramBot.CommandsManager;
using TelegramBot.Services;
using WebServer.Controllers;
using WebServer.Db;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));

builder.Services.AddControllers();
builder.Services.AddDbContext<SessionsDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Db")));
builder.Services.AddScoped<CommandExecutorService>();
builder.Services.AddScoped<GoogleSheetDataService>();
builder.Services.AddScoped<DataHandleService>();
builder.Services.AddScoped<EmailReportService>();
builder.Services.AddScoped<ObserverCommandService>();
BotProviderService.Main = new BotProviderService(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
  
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
