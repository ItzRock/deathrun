using Sandbox;
namespace Teams;
[Team("waiting")]
public class Waiting : Team {
	public override Color Color => Color.White;
	public override bool CheckWin(Player player) => false;
	public override SpawnPointType Spawn => SpawnPointType.Waiting;
}