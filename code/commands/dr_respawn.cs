using Sandbox;

public partial class Deathrun : Sandbox.Game {
	[ConCmd.Server( "dr_respawn_all" )]
	internal void dr_respawn_all(){
		Respawn();
	}

	[ConCmd.Server( "dr_respawn" )]
	internal void dr_respawn(string user){
		Log.Info("Not implemented");
	}
}