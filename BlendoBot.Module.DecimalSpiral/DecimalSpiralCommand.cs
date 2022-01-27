using BlendoBot.Core.Command;
using BlendoBot.Core.Entities;
using BlendoBot.Core.Module;
using BlendoBot.Core.Utility;
using DSharpPlus.EventArgs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlendoBot.Module.DecimalSpiral;

internal class DecimalSpiralCommand : ICommand {
	public DecimalSpiralCommand(DecimalSpiral module) {
		this.module = module;
	}

	private readonly DecimalSpiral module;
	public IModule Module => module;

	public string Guid => "decimalspiral.command";
	public string DesiredTerm => "ds";
	public string Description => "Makes a pretty spiral";
	public Dictionary<string, string> Usage => new() {
		{ "[size]", "Makes a spiral of a given size (size must be an odd number between 5 and 43 inclusive)." }
	};
		
	public async Task OnMessage(MessageCreateEventArgs e, string[] tokenizedInput) {
		if (tokenizedInput.Length != 1) {
			await module.DiscordInteractor.Send(this, new SendEventArgs {
				Message = $"You must specify two arguments to {module.ModuleManager.GetCommandTermWithPrefix(this)}",
				Channel = e.Channel,
				Tag = "DecimalSpiralErrorIncorrectNumArgs"
			});
			return;
		}

		if (!int.TryParse(tokenizedInput[0], out int size)) {
			await module.DiscordInteractor.Send(this, new SendEventArgs {
				Message = $"The argument is not a number!",
				Channel = e.Channel,
				Tag = "DecimalSpiralErrorNonNumericValue"
			});
			return;
		}

		if (size < 5 || size > 43 || size % 2 == 0) {
			await module.DiscordInteractor.Send(this, new SendEventArgs {
				Message = $"The argument must be between 5 and 43 inclusive and be odd!",
				Channel = e.Channel,
				Tag = "DecimalSpiralErrorIncorrectValue"
			});
			return;
		}

		await module.DiscordInteractor.Send(this, new SendEventArgs {
			Message = $"{DecimalSpiral.CreateSpiral(size)}".CodeBlock(),
			Channel = e.Channel,
			Tag = "DecimalSpiralSuccess"
		});
	}
}
