namespace Sandbox;
public partial class DeathrunRound : Sandbox.State {
	public DeathrunRound(){
		this.id = 2;
	}

	public override void ManageClient(Client client){
		var player = new DeathrunPlayer(client);
		player.Respawn();
		client.Pawn = player;
	}
}
