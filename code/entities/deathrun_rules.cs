using Sandbox;
using SandboxEditor;

[Library( "deathrun_rules" ), HammerEntity]
//[EditorModel( "models/editor/playerstart.vmdl" )]
[Title( "Deathrun Rules" ), Category( "Deathrun" ), Icon( "place" )]
public class DeathrunRules : Entity
{
	[Property( Title = "Death Wall" ) ]
	public bool DeathWall { get; set; } = false;
	[Property( Title = "Round Length (In Seconds)" ) ]
	public int RoundLength { get; set; } = 300;
}