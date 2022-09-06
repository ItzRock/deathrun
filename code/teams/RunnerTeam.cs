namespace Teams;
[Team("runners")]
public class Runners : Team {
	public override Color Color => Color.Blue;
	public override SpawnPointType Spawn => SpawnPointType.Runner;
}