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

public class SessionDetailsMenuCommand : BaseCommand
{
	private readonly TelegramBotClient _client;
	private readonly SessionsDbContext _dbContext;

	public SessionDetailsMenuCommand(TelegramBotClient client, SessionsDbContext dbContext)
	{
		_client = client;
		_dbContext = dbContext;
	}

	public override async Task ExecuteAsync(Update update)
	{
		var split = update.CallbackQuery.Data.Split($"_");
		var sessionData = _dbContext.Sessions.Include(o=>o.Agent).Include(o=>o.Indicators).Include(o=>o.Timing).First(o=> o.Id == Convert.ToInt32(split[1]));

		var text = new StringBuilder($"Номер сеанса : {sessionData.SessionNumber}" +
		                             $"Наименовани$е организации : {sessionData.Organization} \n" +
		                             $"OД1 : {sessionData.OD1:E2} \n" +
		                             $"OД2 : {sessionData.OD2:E2} \n" +
		                             $"OД3 : {sessionData.OD3:E2} \n" +
		                             $"OД4 : {sessionData.OD4:E2} \n" +
		                             $"*** Средне$е значение ОД : {sessionData.ODAverage:E2} *** \n" +
		                             $"ТД1 : {sessionData.TD1:E2} \n" +
		                             $"ТД2 : {sessionData.TD2:E2} \n" +
		                             $"ТД3 : {sessionData.TD3:E2} \n" +
		                             $"ТД4 : {sessionData.TD4:E2} \n" +
		                             $"ТД5 : {sessionData.TD5:E2} \n" +
		                             $"ТД6 : {sessionData.TD6:E2} \n" +
		                             $"ТД7 : {sessionData.TD7:E2} \n" +
		                             $"ТД8 : {sessionData.TD8:E2} \n" +
		                             $"ТД9 : {sessionData.TD9:E2} \n" +
		                             $"***Средне$е значение ТД : {sessionData.TDAverage:E2} *** \n" +
		                             $"Влажность : {sessionData.Indicators.Humidity} \n" +
		                             $"Температура : {sessionData.Indicators.Pressure} \n" +
		                             $"Давление : {sessionData.Indicators.Temperature} \n" +
		                             $"Неоднородность : {sessionData.Heterogeneity} \n" +
		                             $"Код среды : {sessionData.Agent.EnvironmentId} \n" +
		                             $"Неоднор$одность левая : {sessionData.LeftHeterogeneity} \n" +
		                             $"Неоднор$одность правая : {sessionData.RightHeterogeneity} \n" +
		                             $"k : {sessionData.K} \n" +
		                             $"+ - : {sessionData.Error}\n"
		);

		var markup = new InlineKeyboardMarkup(new[]
		{
			new[]
			{
				new InlineKeyboardButton("Назад") {CallbackData = ((int) CommandNames.MainMenuCommand).ToString()}
			},
		});
		await _client.EditMessageTextAsync(update.CallbackQuery.Message.Chat, update.CallbackQuery.Message.MessageId,
			text.ToString(), ParseMode.Markdown);
		await _client.EditMessageReplyMarkupAsync(update.CallbackQuery.Message.Chat, update.CallbackQuery.Message.MessageId,
			markup);
	}

	private async Task WriteInfo(SessionData sessionData)
	{
	}
}