using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedLibrary;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Controllers;
using WebServer.Db;

namespace TelegramBot.CommandsManager.Commands;

public class SessionMenuCommand : BaseCommand
{
	private readonly TelegramBotClient _client;
	private readonly SessionsDbContext _context;
	private int CurrentId = 2;

	public SessionMenuCommand(TelegramBotClient client,SessionsDbContext context)
	{
		_client = client;
		_context = context;
	}

	public override async Task ExecuteAsync(Update update)
	{
		
		string[]? s = update.CallbackQuery?.Data?.Split("_");

		if (s.Length < 3)
		{
			CurrentId = _context.Sessions.Count();
		}
		else
		{
			if (s[4] == "1")
			{
				CurrentId++;
			}
			else if (s[4] == "0")
			{
				CurrentId--;
			}

			if (CurrentId>_context.Sessions.Count()-2 || CurrentId<1)
			{
				CurrentId = _context.Sessions.Count() - 2;
			}
		}

		var sessionData = _context.Sessions.Include(o=>o.Agent).First(o =>o.Id ==CurrentId);

		var inlineKeyboard = new InlineKeyboardMarkup(new[]
		{
			new[]
			{
				new InlineKeyboardButton("◀️") {CallbackData = ((int)CommandNames.SessionMenuCommand).ToString()+$"_{sessionData.Id-1}"},
				new InlineKeyboardButton("📢") {CallbackData = ((int)CommandNames.SessionDetailsMenuCommand).ToString()+$"_{sessionData.Id}"},
				new InlineKeyboardButton("▶️") {CallbackData =((int)CommandNames.SessionMenuCommand).ToString()+$"_{sessionData.Id+1}" }, 
			},
			new[]
			{
				new InlineKeyboardButton("Назад") {CallbackData = ((int)CommandNames.MainMenuCommand).ToString()},
				new InlineKeyboardButton("Отчет") {CallbackData = ((int)CommandNames.ProtocolsMenuCommand).ToString()},
			},
		});

		var text = new StringBuilder($"Номер сеанса {sessionData.SessionNumber} \n" +
		                             $"Организаций {sessionData.Organization} \n" +
		                             $"Код Иона {sessionData.Agent.EnvironmentId} \n" +
		                             $"Поток в сеансе {sessionData.SessionFlowIntensity}");

		await _client.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id,update.CallbackQuery.Message.MessageId,text.ToString(),ParseMode.Markdown);
		await _client.EditMessageReplyMarkupAsync(update.CallbackQuery.Message.Chat.Id,update.CallbackQuery.Message.MessageId,inlineKeyboard);
	}
}