using Sandbox;
using System;
using System.Linq;
using System.Collections.Generic;
using Sandbox.UI;

public partial class Deathrun : Sandbox.Game{
	[Net] public static bool inProgress { get; set; } = false;
	[ConVar.Replicated("players_required"), Net] public static int playersToStart { get; set; } = 2;
	[Net] public static int players { get; set; } = 0;
	[Net] public bool enoughPlayers { get; set; } = false;

	[Net] private Dictionary<long, DeathrunPlayer> playerControllers { get; set;} = new();

	Dictionary<SpawnPointType, List<DeathrunSpawnPoint>> SpawnPoints = new(){
		{SpawnPointType.Waiting, new()},
		{SpawnPointType.Runner, new()},
		{SpawnPointType.Controller, new()},
	};
	
	public static Deathrun current;
	public Deathrun(){
		current = this;
		if(IsServer){
			_ = new DeathrunHud();
		};
	}
	public override void PostLevelLoaded(){
		base.PostLevelLoaded();
		playersToStart = playersToStart < 2 ? 2 : playersToStart;
		// Setup spawnpoints
		var spawnpoints = Entity.All.OfType<DeathrunSpawnPoint>();
		foreach(var spawnpoint in spawnpoints){
			SpawnPointType type = spawnpoint.SpawnType;
			if(SpawnPoints[type] == null ) SpawnPoints[type] = new List<DeathrunSpawnPoint>();
			SpawnPoints[type].Add(spawnpoint);
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
			Log.Info($"Waiting Spawns: {SpawnPoints[SpawnPointType.Waiting].Count} | Runner Spawns: {SpawnPoints[SpawnPointType.Runner].Count} | Controller Spawn: {SpawnPoints[SpawnPointType.Controller].Count}");
		}
		if(enoughPlayers == false && IsServer || inProgress == false && IsServer) {
			player.Position = SpawnPoints[SpawnPointType.Waiting][Sandbox.Rand.Int(0, SpawnPoints[SpawnPointType.Waiting].Count -1)].Position;
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
		TeamManager.RemovePlayer(cl);
		enoughPlayers = players >= playersToStart;
		if(inProgress && !enoughPlayers || TeamManager.Get("Controller").Members.Count <= 0) {
			ChatBox.AddChatEntry( To.Everyone, "Deathrun", $"Current Deathrun Match is no longer playable, returning to waiting room.");
		}
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
			ChatBox.AddChatEntry( To.Everyone, "Deathrun", "Deathrun is Ready! Starting in 5 seconds.");
			for(int i = 4; i > 0; i--){
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
				Client controllerClient = Rand.FromList<Client>(Client.All.ToList());
				DeathrunPlayer controllerPlayer = playerControllers[controllerClient.PlayerId];
				ChatBox.AddChatEntry( To.Everyone, "Deathrun", $"Watch out! {controllerClient.Name} is the controlling the traps!");
				foreach(Client client in Client.All){
					if(playerControllers.ContainsKey(client.PlayerId)){
						DeathrunPlayer player = playerControllers[client.PlayerId];
						player.Respawn();
						if(controllerClient.PlayerId == client.PlayerId){
							player.Position = SpawnPoints[SpawnPointType.Controller][Sandbox.Rand.Int(0, SpawnPoints[SpawnPointType.Controller].Count -1)].Position;
						} else player.Position = SpawnPoints[SpawnPointType.Runner][Sandbox.Rand.Int(0, SpawnPoints[SpawnPointType.Runner].Count -1)].Position;
					}
				}
				inProgress = true;
			} else startIsRunning = false;
		}
	}
}
