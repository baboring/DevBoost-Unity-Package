

namespace DevBoost.ActionScript
{
	public class LoadingScreenActionTrigger : ActionTrigger {

		// Use this for initialization
		void Start () {
			action = (v) => v.Initialize(gameObject);
		}
	
	}

}
