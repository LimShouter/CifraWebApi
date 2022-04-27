using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedLibrary;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.CommandsManager.Commands;
using TelegramBot.Controllers;
using TelegramBot.Services;
using WebServer.Db;

namespace TelegramBot.CommandsManager;

public class CommandExecutorService
{
	private readonly IConfiguration _configuration;
	private readonly SessionsDbContext _context;
	private readonly EmailReportService _reportService;
	public static TelegramBotClient _botClient;
	private Dictionary<ChatId, CommandNames> _lastCommands= new Dictionary<ChatId, CommandNames>();

	public Dictionary<CommandNames, BaseCommand> CommandStore;

	public CommandExecutorService(IConfiguration configuration,SessionsDbContext context,EmailReportService reportService)
	{
		
		_configuration = configuration;
		_context = context;
		_reportService = reportService;
		CommandStore = new Dictionary<CommandNames, BaseCommand>()
		{
			// {CommandNames.StartCommand, new StartCommand(_botClient)},
			{CommandNames.MainMenuCommand, new MainMenuCommand(_botClient)},
			{CommandNames.SessionMenuCommand, new SessionMenuCommand(_botClient,_context)},
			{CommandNames.TrackDetectorsMenuCommand, new TrackDetectorsMenuCommand()},
			{CommandNames.OnlineDetectorsMenuCommand, new OnlineDetectorsMenuCommand()},
			{CommandNames.ProtocolsMenuCommand, new ProtocolsMenuCommand(_botClient)},
			{CommandNames.SessionDetailsMenuCommand,new SessionDetailsMenuCommand(_botClient,_context)},
			{CommandNames.EnvironmentMenuCommand ,new EnvironmentIndicatorsMenuCommand(_botClient,_context)}
		};
		_botClient = BotProviderService.Bot;
	}

	public async Task Execute(Update update)
	{
		

		if (update.Type == UpdateType.Message && _lastCommands.ContainsKey(update.Message.Chat))
		{
			if (_lastCommands[update.Message.Chat] == CommandNames.ProtocolsMenuCommand)
			{
				var sessionDatas = _context.Sessions.Include(op=> op.Agent).Include(ok=> ok.Timing).Include(om => om.Indicators);
				var upd = update.Message.Text.Split(" ");
				var emailRaw = upd[0];
				var i = Convert.ToInt32(Convert.ToInt32(upd[1]));
				List<SessionData> values = null;
				do
				{
					i++;
					values.Add(sessionDatas.First(o=>o.SessionNumber == i));
				} while (i<Convert.ToInt32(upd[2]));
				_reportService.SendNonStandartReports(values, emailRaw);
			}
		}
		
		if (update.Message != null)
		{
			if (update.Message.Text == "/start")
			{
				await CommandStore[CommandNames.MainMenuCommand].ExecuteAsync(update);
			}
		}

		if (update.Type == UpdateType.CallbackQuery)
		{
			await CommandStore[(CommandNames)Convert.ToInt32(update.CallbackQuery.Data.Split("_")[0])].ExecuteAsync(update);
		}
		
		
	}

	public void Run()
	{
		
	}
}