namespace Sandbox;
public class State {
	public int id = 0;
	public void Transition(State state, float seconds){

	}
	public virtual void ManageClient(Client client){
		Log.Info("hi");
	}
}
