using Sandbox;
using SandboxEditor;
using System;
using System.Linq;
using System.Collections.Generic;


[Library( "deathrun_music" ), HammerEntity]
[EditorModel( "models/editor/spot_cone.vmdl" )]
[Title( "Deathrun Music Player" ), Category( "Deathrun" ), Icon( "place" )]
public class DeathrunMusic : Entity {
	[Property( Title = "Waiting Music" ) ]
	public TagList WaitingMusic { get; set; }

	[Property( Title = "Game Music" ) ]
	public TagList GameMusic { get; set; }
}