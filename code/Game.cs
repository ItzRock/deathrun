using Sandbox;
using System;
using System.Linq;
using System.Collections.Generic;
using Sandbox.UI;

namespace Sandbox;

public partial class Deathrun : Sandbox.Game{
	[Net] public static bool inProgress { get; set; } = false;
	[Net] public static int playersToStart { get; } = 3;
	[Net] public static int players { get; set; } = 0;
	[Net] public bool enoughPlayers { get; set; } = false;

	[Net] private Dictionary<long, DeathrunPlayer> playerControllers { get; set;} = new();

	List<SpawnPoint> WaitingPoints = new();
	List<SpawnPoint> RunnerPoints = new();
	List<SpawnPoint> ControllerPoint = new();
	public static Deathrun current;

	public Deathrun(){
		current = this;
		if(IsServer){
			_ = new DeathrunHud();
		};
	}
	public override void PostLevelLoaded(){
		base.PostLevelLoaded();
		var spawnpoints = Entity.All.OfType<SpawnPoint>();
		foreach(var spawnPoint in spawnpoints){
			if(spawnPoint.Tags.Has("waiting")) WaitingPoints.Add(spawnPoint);
			if(spawnPoint.Tags.Has("runner")) RunnerPoints.Add(spawnPoint);
			if(spawnPoint.Tags.Has("controller")) ControllerPoint.Add(spawnPoint);
		}
	}
	public override void ClientJoined( Client client ){
		base.ClientJoined( client );
		DeathrunPlayer player = new DeathrunPlayer(client);
		player.Respawn();
		client.Pawn = player;
		playerControllers.Add(client.PlayerId, player);

		// Set spawnpoint to a waiting room spawn.
		if(IsServer){
			ChatBox.AddChatEntry( To.Everyone, "Debugger", $"Waiting Spawns: {WaitingPoints.Count} | Runner Spawns: {RunnerPoints.Count} | Controller Spawn: {ControllerPoint.Count}");
		}
		if(enoughPlayers == false && IsServer || inProgress == false && IsServer) {
			player.Position = WaitingPoints[Sandbox.Rand.Int(0, WaitingPoints.Count -1)].Position;
		}
		

		players++;
		if(!enoughPlayers && IsServer){
			ChatBox.AddChatEntry( To.Everyone, "Deathrun", $"Players {players}/{playersToStart}");
			if(players >= playersToStart) {
				StartGame();
			}
		}
	}
	public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
	{
		base.ClientDisconnect( cl, reason );
		players--;
		enoughPlayers = players >= playersToStart;
	}
	[ConCmd.Server( "setprogress" )]
	public static void setProgress(bool progress){
		inProgress = progress;
	}
	public bool getProgress(){
		return inProgress;
	}
	bool startIsRunning = false;
	public async void StartGame(){
		if(IsServer && !startIsRunning){
			startIsRunning = true;
			enoughPlayers = true;
			ChatBox.AddChatEntry( To.Everyone, "Deathrun", "Enough players have joined! Starting in 15 seconds.");
			for(int i = 14; i > 0; i--){
				enoughPlayers = players >= playersToStart;
				if(enoughPlayers)
					ChatBox.AddChatEntry( To.Everyone, "Deathrun", $"Starting Deathrun in {i} seconds.");
				else {
					ChatBox.AddChatEntry( To.Everyone, "Deathrun", "Game is no longer ready to start. Waiting for Players");
					break;
				}
				await GameTask.Delay(1000);
			}
			if(enoughPlayers){
				Client controllerClient = Rand.FromList<Client>((List<Client>)Client.All);
				DeathrunPlayer controllerPlayer = playerControllers[controllerClient.PlayerId];
				ChatBox.AddChatEntry( To.Everyone, "Deathrun", $"Watch out! {controllerClient.Name} is the controlling the traps!");
				foreach(Client client in Client.All){
					if(playerControllers.ContainsKey(client.PlayerId)){
						DeathrunPlayer player = playerControllers[client.PlayerId];
						player.Respawn();
						player.Position = RunnerPoints[Sandbox.Rand.Int(0, RunnerPoints.Count -1)].Position;
					}
				}
				inProgress = true;
			} else startIsRunning = false;
		}
	}
}
