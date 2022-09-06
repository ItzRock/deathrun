using Sandbox;
using System;
using System.Linq;
using System.Collections.Generic;
using Sandbox.UI;

public partial class Deathrun : Sandbox.Game {
	public override void PostLevelLoaded(){
		base.PostLevelLoaded();
		playersToStart = playersToStart < 2 ? 2 : playersToStart;
		// Setup spawnpoints
		if(IsServer){
			var spawnpoints = Entity.All.OfType<DeathrunSpawnPoint>();
			foreach(var spawnpoint in spawnpoints){
				SpawnPointType type = spawnpoint.SpawnType;
				if(SpawnPoints[type] == null ) SpawnPoints[type] = new List<DeathrunSpawnPoint>();
				SpawnPoints[type].Add(spawnpoint);
			}
		}
	}
}