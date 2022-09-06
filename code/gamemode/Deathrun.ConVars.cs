using Sandbox;
public partial class Deathrun : Sandbox.Game {
    [ConVar.Replicated] public static string end_weapon { get; set; } = "weapon_dr_bat";
}