using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WebServer.Db;

namespace TelegramBot.CommandsManager.Commands;

public class EnvironmentIndicatorsMenuCommand : BaseCommand
{
	private readonly TelegramBotClient _client;
	private readonly SessionsDbContext _sessionsDbContext;

	public EnvironmentIndicatorsMenuCommand(TelegramBotClient client, SessionsDbContext sessionsDbContext)
	{
		_client = client;
		_sessionsDbContext = sessionsDbContext;
	}
	public override async Task ExecuteAsync(Update update)
	{
		
		var markup = new InlineKeyboardMarkup(new []
		{
			new []{new InlineKeyboardButton("Назад"){CallbackData = ((int) CommandNames.MainMenuCommand).ToString()}},
		});

		var indicator = _sessionsDbContext.EnvironmentIndicatorsList.First(o =>
			o.Id == _sessionsDbContext.EnvironmentIndicatorsList.Count() - 1);
		var text = new StringBuilder($"Дата последнего изсерения: {indicator.ReadTime} \n" +
		                             $"Влажность : {indicator.Humidity} \n" +
		                             $"Температура : {indicator.Temperature} \n" +
		                             $"Давление : {indicator.Pressure}");
		await _client.EditMessageTextAsync(update.CallbackQuery.Message.Chat,update.CallbackQuery.Message.MessageId,text.ToString());
		await _client.EditMessageReplyMarkupAsync(update.CallbackQuery.Message.Chat,update.CallbackQuery.Message.MessageId,markup);
	}
}