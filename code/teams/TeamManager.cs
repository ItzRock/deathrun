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
	public static void SwitchTeam(Player player, string team){
		
	}
}
public abstract class Team{
	public readonly string Name;
	public abstract Color Color { get; }
	public List<Player> Members { get; set; } = new();
	public Team(){
		Name = GetType().ToString().ToLower();
		if(!TeamManager.Teams.ContainsKey(Name)){
			TeamManager.Teams[Name] = this;
		}
	}
	public virtual bool CheckWin(Player player) => true;
}