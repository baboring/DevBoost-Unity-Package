using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.Core.Animation {

	/// <summary>
	/// Really simple class for artists to add 2D rotation to objects.
	/// </summary>
	public class Rotate2D : MonoBehaviour {

		/// <summary>
		/// Speed of the rotation around the z axis in degrees per second.
		/// </summary>
		[SerializeField]
		private float rotationSpeed = 0f;

		/// <summary>
		/// Flag indicating we should rotate using unscaled time.
		/// </summary>
		[SerializeField]
		private bool useUnscaledTime = false;

		/// <summary>
		/// Cached rotation vector so we aren't making garbage every frame.
		/// </summary>
		private Vector3 rotationVector = Vector3.zero;

		/// <summary>
		/// Just rotate the thing the specific amount.
		/// </summary>
		private void Update() {
			this.rotationVector.z = this.rotationSpeed * ((this.useUnscaledTime) ? Time.unscaledDeltaTime : Time.deltaTime);
			// Just do the dang thing.
			this.transform.Rotate(rotationVector);
		}
	}
}
