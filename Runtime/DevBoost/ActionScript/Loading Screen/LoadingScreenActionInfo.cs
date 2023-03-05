using UnityEngine;

namespace DevBoost.ActionScript
{

	[CreateAssetMenu(menuName = "ActionObject/LoadingScreenActionInfo")]
	public class LoadingScreenActionInfo : ActionObject
	{

		[SerializeField]
		protected GameObject prefab;
		public override void Initialize(GameObject obj)
		{

		}
		public override void TriggerEvent(object arg = null)
		{

		}
	}
}
