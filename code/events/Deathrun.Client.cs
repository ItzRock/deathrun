using Sandbox;
using System;
using System.Linq;
using System.Collections.Generic;
using Sandbox.UI;

public partial class Deathrun : Sandbox.Game {
	public void ClientsChanged(Client client) {
		players = Client.All.Count;
		Log.Info($"Total Players: {players}");
		enoughPlayers = players >= playersToStart;
		
		if(players >= 12) TotalControllers = 2;
		else TotalControllers = 1;
		
		if(inProgress && !enoughPlayers || inProgress && TeamManager.Get("Controller").Members.Count <= 0) {
			ChatBox.AddChatEntry( To.Everyone, "Deathrun", $"Current Deathrun Match is no longer playable, returning to waiting room.");
		}
	}
	public override void ClientJoined( Client client ){
		base.ClientJoined( client );
		ClientsChanged( client );
		// Setup their player
		DeathrunPlayer player = new DeathrunPlayer(client);
		player.Respawn();
		client.Pawn = player;
		playerControllers.Add(client.PlayerId, player);

		// Set spawnpoint to a waiting room spawn.
		if(enoughPlayers == false && IsServer || inProgress == false && IsServer)
			Respawn(client);
		
		if(!enoughPlayers && IsServer){
			ChatBox.AddChatEntry( To.Everyone, "Deathrun", $"Players {players}/{playersToStart}");
			if(players >= playersToStart) {
				StartGame();
			}
		}
		if(IsServer && enoughPlayers && !inProgress){
			StartGame();
		}
	}
	public override void ClientDisconnect( Client client, NetworkDisconnectionReason reason )
	{
		base.ClientDisconnect( client, reason );
		ClientsChanged( client );
		TeamManager.RemovePlayer(client);
	}
}