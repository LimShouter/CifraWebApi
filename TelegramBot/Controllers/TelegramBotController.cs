using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.CommandsManager;
using TelegramBot.Services;

namespace TelegramBot.Controllers;

[ApiController]
[Route("api/message/update")]
public class TelegramBotController : ControllerBase
{
	private readonly CommandExecutorService _commandExecutor;
 
	public TelegramBotController(CommandExecutorService commandExecutor)
	{
		_commandExecutor = commandExecutor;
	}
	
	
	[HttpPost]
	public async Task<IActionResult> Post([FromBody]object update)
	{
	     var upd = JsonConvert.DeserializeObject<Update>(update.ToString());
		await _commandExecutor.Execute(upd);
		return Ok();
	}
}