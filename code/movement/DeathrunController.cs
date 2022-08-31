using Sandbox;

partial class DeathrunPlayer : Player
{
	public LifeState life;

	private TimeSince timeSinceJumpReleased;

	public ClothingContainer Clothing = new();
	public DeathrunPlayer(){}
	public DeathrunPlayer( Client cl ) : this(){
		Clothing.LoadFromClient( cl );
	}

	public override void Respawn(){
		if(Deathrun.current.getProgress() || life == LifeState.Dead){
			// Create a Spectator Camera.
			life = LifeState.Dead;
			TeamManager.SwitchTeam(this, "WaitingTeam");

			SetModel( "models/citizen/citizen.vmdl" );

			Controller = new NoclipController();

			EnableAllCollisions = false;
			EnableDrawing = false;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = false;

			CameraMode = new FirstPersonCamera();
		} else {
			life = LifeState.Alive;
			SetModel( "models/citizen/citizen.vmdl" );

			Controller = new WalkController();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			Clothing.DressEntity( this );
			CameraMode = new FirstPersonCamera();

			
		}
		base.Respawn();
	}

	public override void OnKilled()
	{
		base.OnKilled();
		if(IsClient){
			Sandbox.UI.ChatBox.AddChatEntry("Deathrun", $"{this.Client.Name} has died! (lmao)");
		}
		Particles.Create( "particles/explosion/barrel_explosion/explosion_fire_ring.vpcf", this.Position );
		PlaySound( "sounds/explo_gas_can_01.sound" );

		Controller = null;

		EnableAllCollisions = false;
		EnableDrawing = false;

		foreach ( var child in Children ){
			child.EnableDrawing = false;
		}
		life = LifeState.Dead;
	}

}