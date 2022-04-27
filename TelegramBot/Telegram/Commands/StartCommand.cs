using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.CommandsManager.Commands;

public class StartCommand : BaseCommand
{
	private readonly TelegramBotClient _client;
	
	public StartCommand(TelegramBotClient client)
	{
		_client = client;
	}

	public override async Task ExecuteAsync(Update update)
	{
		var menuMessage = await _client.SendTextMessageAsync(update.Message.Chat.Id,"/menu");
		_client.PinChatMessageAsync(update.Message.Chat.Id, menuMessage.MessageId, true);
	}
	
}