using DevBoost.ActionScript;

namespace DevBoost.Utilities 
{
	public class LoadingScreenTrigger : ActionTrigger {

		// Use this for initialization
		void Start () {
			action = (v) => v.Initialize(gameObject);
		}
		
		// Update is called once per frame
		void Update () {

		}
	}

}
