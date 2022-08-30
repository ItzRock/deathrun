using Sandbox;
namespace Sandbox;
public partial class Waiting : Sandbox.State {
	public Waiting(){
		this.id = 1;
	}

	public void Countdown(float seconds){

	}
	public override void ManageClient(Client client){
		var player = new SpectatorPawn();
		client.Pawn = player;
	}
}
