using Sandbox;

partial class BasePlayer : Player
{
	public Team team = new Spectator();

	public override void Respawn(){
		if(MyGame.deathrun.gameState.id is not 1 && team.id is not 1){
			base.Respawn();
			team = new Spectator();
			Client.Pawn = new SpectatorPawn();
		}
	}
}