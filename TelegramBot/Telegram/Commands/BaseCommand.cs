using Telegram.Bot.Types;

namespace TelegramBot.CommandsManager.Commands;

public abstract class BaseCommand
{
	public const string NextCommandNameKey = "NextCommandName";
	public abstract Task ExecuteAsync(Update update);
}