using Telegram.Bot.Types;

namespace TelegramBot.Services;

public abstract class BaseObserver
{
	protected ChatId Id;
	public event Action<ChatId> Confirmed; 
	public abstract Task Check(Update update);
	protected abstract Task Execute(Update update);
}