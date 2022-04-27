using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.CommandsManager.Commands;

public class SendAdmissionProtocol : BaseCommand
{
	private readonly TelegramBotClient _client;

	public SendAdmissionProtocol(TelegramBotClient client)
	{
		_client = client;
	}

	public override async Task ExecuteAsync(Update update)
	{
		var markup = new InlineKeyboardMarkup(new[]
		{
			new[] {new InlineKeyboardButton("Назад")}
		});
		
	}
}