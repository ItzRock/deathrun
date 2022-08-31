// Partly borrowed from TTT https://github.com/TTTReborn/tttreborn/blob/master/code/teams/Team.cs
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;


[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class TeamAttribute : LibraryAttribute{
    public TeamAttribute(string name) : base("dr_team_" + name) { }
}
public static class TeamManager{
	public static Dictionary<string, Team> Teams { get; set;} = new();
	public static void SwitchTeam(DeathrunPlayer player, string team){
		if(Teams.ContainsKey(team)) {
			player.team.Members.Remove(player);
			Teams[team].Members.Add(player);
		}
	}
	public static Team Get(string team){
		if(Teams.ContainsKey(team)) return Teams[team];
		else return null;
	}
}
public abstract class Team{
	public readonly string Name;
	public abstract Color Color { get; }
	public List<DeathrunPlayer> Members { get; set; } = new();
	public Team(){
		Name = GetType().ToString().ToLower();
		if(!TeamManager.Teams.ContainsKey(Name)){
			TeamManager.Teams[Name] = this;
		}
	}
	public virtual bool CheckWin(Player player) => true;
}