using Sandbox;

public partial class Deathrun : Sandbox.Game {
	[ConCmd.Server( "dr_setprogress" )]
	public static void setProgress(bool progress){
		inProgress = progress;
	}
	public bool getProgress(){
		return inProgress;
	}
}