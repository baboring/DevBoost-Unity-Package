using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.Core {

	/// <summary>
	/// The UnityObjectUtility provides some helper functions for instantiation and parenting of unity objects.
	/// </summary>
	public static class UnityObjectUtility {

		#region Constants

		/// <summary>
		/// The default position for a new game object.
		/// </summary>
		private static readonly Vector3 DEFAULT_POSITION = Vector3.zero;

		/// <summary>
		/// The default rotation for a new game object.
		/// </summary>
		private static readonly Quaternion DEFAULT_ROTATION = Quaternion.identity;

		/// <summary>
		/// The default scale for a new game object.
		/// </summary>
		private static readonly Vector3 DEFAULT_SCALE = Vector3.one;

		#endregion

		#region Instantiation

		/// <summary>
		/// Instantiates a new game object 
		/// </summary>
		/// <param name="go">Game object to instantiate </param>
		/// <param name="parent"></param>
		/// <param name="keepPosition"></param>
		/// <param name="localPosition"></param>
		/// <param name="localRotation"></param>
		/// <param name="localScale"></param>
		/// <returns>Instantiated instance of the provided game object.</returns>
		public static GameObject Instantiate(GameObject go, Transform parent = null, Vector3? localPosition = null, Vector3? localRotation = null, Vector3? localScale = null) {
			GameObject instantedObject = GameObject.Instantiate(go);

			if (parent != null) {
				SetParent(instantedObject, parent, localPosition, localRotation, localScale);	
			}
			
			return instantedObject;
		}

		/// <summary>
		/// Instantiates a specifically typed object.
		/// </summary>
		/// <param name="prefab">The prefab of the </param>
		/// <param name="parent">Transform to parent the instanted game object to.</param>
		/// <param name="localPosition">The new local position of the prefab within it's parent object.</param>
		/// <param name="localRotation">The new local rotation of the object.</param>
		/// <param name="localScale">The new local scale for the prefab.</param>
		/// <typeparam name="T">The type of the instantiated prefab.</typeparam>
		/// <returns>The instantied instance of that prefab.</returns>
		public static T Instantiate<T>(T prefab, Transform parent = null, Vector3? localPosition = null, Vector3? localRotation = null, Vector3? localScale = null) where T : MonoBehaviour {
			T instantedObject = Object.Instantiate<T>(prefab);

			if (parent != null) {
				SetParent(instantedObject.transform, parent, localPosition, localRotation, localScale);	
			}

			return instantedObject;
		}

		#endregion

		#region Parenting

		/// <summary>
		/// Sets the parent of the provided child transform to the be provided parent transform.
		/// </summary>
		/// <param name="child">Child transform.</param>
		/// <param name="parent">The transform to parent the chil transform to.</param>
		/// <param name="localPosition">The new local position to set the transform to within it's parent.</param>
		/// <param name="localRotation">The new local rotation to set the transform to within it's parent.</param>
		/// <param name="localScale">The new local scale to the transform to within it's parent.</param>
		public static void SetParent(Transform child, Transform parent, Vector3? localPosition = null, Vector3? localRotation = null, Vector3? localScale = null) {
			child.SetParent(parent, false);

			// If there is a new position apply it.
			if (localPosition.HasValue) {
				child.localPosition = localPosition.Value;
			} else {
				child.localPosition = DEFAULT_POSITION;
			}

			// If there is a desired new rotation add it.
			if (localRotation.HasValue) {
				child.rotation = Quaternion.Euler(localRotation.Value);
			} else {
				child.rotation = DEFAULT_ROTATION;
			}

			// If there is a desired new scale apply it.
			if (localScale.HasValue) {
				child.localScale = localScale.Value;
			} else {
				child.localScale = DEFAULT_SCALE;
			}
		}

		/// <summary>
		/// Sets the parent of the provided game object to the indicated parent and will change the transform values accordingly if required.
		/// </summary>
		/// <param name="go">GameObject to add to the parent transform</param>
		/// <param name="parent">Parent Transform to add child to.</param>
		/// <param name="localPosition">The new local position of the transform.</param>
		/// <param name="localRotation">The new local rotation of the transform.</param>
		/// <param name="localScale">The new local scale of the transform.</param>
		public static void SetParent(GameObject go, Transform parent, Vector3? localPosition = null, Vector3? localRotation = null, Vector3? localScale = null) {
			SetParent(go.transform, parent, localPosition, localRotation, localScale);
		}

		/// <summary>
		/// Sets the parent object on a specific type implementing monobehaviour.
		/// </summary>
		/// <param name="prefab">MonoBehaviour to parent </param>
		/// <param name="parent"></param>
		/// <param name="localPosition"></param>
		/// <param name="localRotation"></param>
		/// <param name="localScale"></param>
		public static void SetParent(MonoBehaviour prefab, Transform parent, Vector3? localPosition = null, Vector3? localRotation = null, Vector3? localScale = null) {
			SetParent(prefab.gameObject.transform, parent, localPosition, localRotation, localScale);
		}

		#endregion
		
	}

}