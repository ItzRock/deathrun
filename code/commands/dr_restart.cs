using Sandbox;

public partial class Deathrun : Sandbox.Game {
	[ConCmd.Server( "dr_restart" )]
	internal void dr_restart(){
		RestartGame();
	}
}