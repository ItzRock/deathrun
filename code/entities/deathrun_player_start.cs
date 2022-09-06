using Sandbox;
using SandboxEditor;

public enum SpawnPointType{
	Waiting = 0,
	Runner = 1,
	Controller = 2,
}

[Library( "deathrun_player_start" ), HammerEntity]
[EditorModel( "models/editor/playerstart.vmdl" )]
[Title( "Player Spawnpoint Deathrun" ), Category( "Player" ), Icon( "place" )]
public class DeathrunSpawnPoint : SpawnPoint
{
	[Property( Title = "Spawn Point Type " ) ]
	public SpawnPointType SpawnType { get; set; } = SpawnPointType.Waiting;
}