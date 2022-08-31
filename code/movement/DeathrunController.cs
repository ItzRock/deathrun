using Sandbox;

public partial class DeathrunPlayer : Player
{
	[Net]
	public LifeState life {get; set;}

	[Net] public Team team {get; set;} = TeamManager.Get("Waiting");

	private TimeSince timeSinceJumpReleased;

	public ClothingContainer Clothing = new();
	public DeathrunPlayer(){}
	public DeathrunPlayer( Client cl ) : this(){
		Clothing.LoadFromClient( cl );
	}

	public override void Respawn(){
		if(Deathrun.current.getProgress() || life == LifeState.Dead){
			// Create a Spectator Camera.
			if(IsServer){
				life = LifeState.Dead;
			}
			TeamManager.SwitchTeam(this, "WaitingTeam");

			SetModel( "models/citizen/citizen.vmdl" );

			Controller = new NoclipController();

			EnableAllCollisions = false;
			EnableDrawing = false;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = false;

			CameraMode = new FirstPersonCamera();
		} else {
			if(IsServer){
				life = LifeState.Alive;
			}
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
		if(Deathrun.current.getProgress()){
			if(IsServer){
			Sandbox.UI.ChatBox.AddChatEntry( To.Everyone ,"Deathrun", $"{this.Client.Name} has died! (lmao)");
			}
			Particles.Create( "particles/explosion/barrel_explosion/explosion_fire_ring.vpcf", this.Position );
			PlaySound( "sounds/explo_gas_can_01.sound" );
			life = LifeState.Dead;
		}

		Controller = null;

		EnableAllCollisions = false;
		EnableDrawing = false;

		foreach ( var child in Children ){
			child.EnableDrawing = false;
		}
	}

}