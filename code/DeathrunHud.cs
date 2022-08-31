using Sandbox;
using Sandbox.UI;

[Library]
public partial class DeathrunHud : HudEntity<RootPanel>
{
	public DeathrunHud()
	{
		if ( !IsClient )
			return;

		//RootPanel.StyleSheet.Load( "/ui/SandboxHud.scss" );

		RootPanel.AddChild<ChatBox>();
		RootPanel.AddChild<VoiceList>();
		RootPanel.AddChild<VoiceSpeaker>();
		RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();
	}
}