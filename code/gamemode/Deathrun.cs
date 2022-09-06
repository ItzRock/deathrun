using Sandbox;
using System;
using System.Linq;
using System.Collections.Generic;
using Sandbox.UI;

public partial class Deathrun : Sandbox.Game {
	[Net] public static bool inProgress { get; set; } = false;
	[ConVar.Replicated("players_required"), Net] public static int playersToStart { get; set; } = 2;
	[Net] public static int players { get; set; } = 0;
	[Net] public bool enoughPlayers { get; set; } = false;
	Dictionary<SpawnPointType, int> pointsUsed = new(){
		{SpawnPointType.Waiting, 0},
		{SpawnPointType.Runner, 0},
		{SpawnPointType.Controller, 0},
	};		
	[Net] private Dictionary<long, DeathrunPlayer> playerControllers { get; set;} = new();
	public List<Client> Controllers { get; set; } = new();
	[Net] public int TotalControllers { get; private set; } = 1;

	[Net] public static int AlivePlayers { get; set; }

	[Net] Dictionary<SpawnPointType, List<DeathrunSpawnPoint>> SpawnPoints { get; set; } = new(){
		{SpawnPointType.Waiting, new()},
		{SpawnPointType.Runner, new()},
		{SpawnPointType.Controller, new()},
	};
	
	public static Deathrun current;
	public Deathrun(){
		current = this;
		if(IsServer){
			_ = new DeathrunHud();
			new Teams.Waiting();
			new Teams.Runners();
			new Teams.Controller();
		};
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );
		if(IsServer && inProgress && AlivePlayers <= 0){
			ChatBox.AddChatEntry( To.Everyone, "Deathrun", $"The Controller has succesfully killed all the runners! Returning to lobby.");
			RestartGame();
		}
		if(IsServer && enoughPlayers && !inProgress){
			StartGame();
		}
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
				// Set everybody to runner.
				foreach(Client client in Client.All){
					TeamManager.SwitchTeam(playerControllers[client.PlayerId], "runners");
				}
				// For each controller we can have
				for(int i = 1; i <= TotalControllers; i++){
					int randomIndex = Rand.Int(0, TeamManager.Get("runners").Members.Count);
					Client controllerClient = TeamManager.Get("runners").Members[randomIndex].Client;
					Controllers.Add(controllerClient);
					Log.Info($"Picked {controllerClient.Name} as a Controller");
					TeamManager.SwitchTeam(playerControllers[controllerClient.PlayerId], "controller");
				}
				AlivePlayers = TeamManager.Get("runners").Members.Count;
				string message = TotalControllers == 1 ? $"Watch out! {Controllers[0].Name} is the controlling the traps!" : $"Watch out! {Controllers[0].Name} and {Controllers[1].Name} is the controlling the traps!";
				ChatBox.AddChatEntry( To.Everyone, "Deathrun", message);
				Respawn();
				inProgress = true;
			} else startIsRunning = false;
		}
	}
	[ConCmd.Server("dr_respawn_all")]
	public void Respawn(){
		pointsUsed = new(){
			{SpawnPointType.Waiting, 0},
			{SpawnPointType.Runner, 0},
			{SpawnPointType.Controller, 0},
		};		
		Respawn(To.Everyone);
	}
	public void Respawn( To who ){
		foreach(Client client in who){
			Respawn(client);
		}
	}

	public void Respawn( Client client ){
		if(IsServer){
			if(playerControllers.ContainsKey(client.PlayerId)){
				DeathrunPlayer player = playerControllers[client.PlayerId];
				player.Respawn();
				Team team = player.team;
				if(team == null) TeamManager.SwitchTeam(player, "waiting");
				Log.Info($"Respawning {client.Name} as a {team.Name}");
				List<DeathrunSpawnPoint> possiblePonts = SpawnPoints[team.Spawn];
				Vector3 spawn = possiblePonts[pointsUsed[team.Spawn]].Position;
				pointsUsed[team.Spawn]++;
				
				player.Position = spawn;
			}
		}
	}
		
	[ConCmd.Server("dr_restart")]
	public void RestartGame(){
		pointsUsed = new(){
			{SpawnPointType.Waiting, 0},
			{SpawnPointType.Runner, 0},
			{SpawnPointType.Controller, 0},
		};		
		inProgress = false;
		Controllers = new();
		startIsRunning = false;
		foreach(Client client in Client.All){
			TeamManager.SwitchTeam(playerControllers[client.PlayerId], "waiting");
			DeathrunPlayer drPlayer = playerControllers[client.PlayerId];
			drPlayer.life = LifeState.Alive;
		}
		Respawn();
	}
}
