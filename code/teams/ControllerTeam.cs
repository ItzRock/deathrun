namespace Teams;

[Team("controller")]
public class Controller : Team {
	public override Color Color => Color.Red;
	public override SpawnPointType Spawn => SpawnPointType.Controller;
}