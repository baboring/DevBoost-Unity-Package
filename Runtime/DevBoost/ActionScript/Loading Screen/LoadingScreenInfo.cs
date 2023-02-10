using UnityEngine;
using DevBoost.ActionScript;

namespace DevBoost
{

	[CreateAssetMenu(menuName = "ActionObject/LoadingScreenInfo")]
	public class LoadingScreenInfo : ActionObject
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
