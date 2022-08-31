using Sandbox;

public class DeathrunBot : Bot{
	internal static System.Collections.Generic.List<DeathrunBot> bots = new();

	[ConCmd.Server("deathrun_bot_add")]
	internal static void SpawnCustomBot()
	{
		Host.AssertServer();

		// Create an instance of your custom bot.
		var bot = new DeathrunBot();
		bots.Add(bot);
	}

	[ConCmd.Server("deathrun_bot_remove")]
	internal static void Remove(){
		bots[0].Client.Kick();
		bots.RemoveAt(0);
	}

	public override void BuildInput( InputBuilder builder )
	{
		// Here we can choose / modify the bot's input each tick.
		// We'll make them constantly attack by holding down the Attack1 button.
		//builder.SetButton( InputButton.Attack1, true );
	}
	
	public override void Tick()
	{
		// Here we can do something with the bot each tick.
		// Here we'll print our bot's name every tick.
		//Log.Info( Client.Name );
	}
}