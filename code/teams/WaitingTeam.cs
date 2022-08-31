using Sandbox;

[Team("waiting")]
public class WaitingTeam : Team {
	public override Color Color => Color.White;
	public override bool CheckWin(Player player) => false;
}