using Sandbox;
using System;
using System.Linq;
using System.Threading.Tasks;
using Sandbox.UI;

namespace Sandbox;

public partial class Deathrun : Sandbox.Game{
	public static bool inProgress = false;

	public static Deathrun current;
	public Deathrun(){
		current = this;
		if(IsServer){
			_ = new DeathrunHud();
		};
	}
	public override void ClientJoined( Client client )
	{
		base.ClientJoined( client );
		
		Player player = new DeathrunPlayer(client);
		player.Respawn();
		client.Pawn = player;
	}
	[ConCmd.Server( "setprogress" )]
	public static void setProgress(bool progress){
		inProgress = progress;
	}
	public bool getProgress(){
		return inProgress;
	}
}
