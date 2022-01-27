using BlendoBot.Core.Module;
using BlendoBot.Core.Services;
using System.Linq;
using System.Threading.Tasks;

namespace BlendoBot.Module.DecimalSpiral;

[Module(Guid = "com.biendeo.blendobot.module.decimalspiral", Name = "Decimal Spiral", Author = "Biendeo", Version = "2.0.0", Url = "https://github.com/BlendoBot/BlendoBot.Module.DecimalSpiral")]
public class DecimalSpiral : IModule {
	public DecimalSpiral(IDiscordInteractor discordInteractor, IModuleManager moduleManager) {
		DiscordInteractor = discordInteractor;
		ModuleManager = moduleManager;

		DecimalSpiralCommand = new(this);
	}

	internal ulong GuildId { get; private set; }

	internal readonly DecimalSpiralCommand DecimalSpiralCommand;

	internal readonly IDiscordInteractor DiscordInteractor;
	internal readonly IModuleManager ModuleManager;

	public Task<bool> Startup(ulong guildId) {
		GuildId = guildId;
		return Task.FromResult(ModuleManager.RegisterCommand(this, DecimalSpiralCommand, out _));
	}

	private enum Direction {
		Up,
		Right,
		Down,
		Left
	};

	internal static string CreateSpiral(int size) {
		char[] spiral = Enumerable.Repeat(' ', size * (size + 1)).ToArray();
		for (int row = 0; row < size; ++row) {
			spiral[(size + 1) * row + size] = '\n';
		}

		int x = size / 4 * 2;
		int y = ((size / 2) + 1) / 2 * 2;
		char num = '0';
		Direction direction = size / 2 % 2 == 0 ? Direction.Left : Direction.Right;
		int stride = 2;
		int currentStride = 0;

		while (x != -1 || y != 0) {
			spiral[(size + 1) * y + x] = num;
			++num;
			if (num > '9') {
				num = '0';
			}
			++currentStride;
			switch (direction) {
				case Direction.Right:
					++x;
					if (currentStride >= stride) {
						currentStride = 0;
						direction = Direction.Up;
					}
					break;
				case Direction.Up:
					--y;
					if (currentStride >= stride) {
						currentStride = 0;
						direction = Direction.Left;
						stride += 2;
					}
					break;
				case Direction.Left:
					--x;
					if (currentStride >= stride) {
						currentStride = 0;
						direction = Direction.Down;
					}
					break;
				case Direction.Down:
					++y;
					if (currentStride >= stride) {
						currentStride = 0;
						direction = Direction.Right;
						stride += 2;
					}
					break;
			}
		}

		return new string(spiral);
	}
}
