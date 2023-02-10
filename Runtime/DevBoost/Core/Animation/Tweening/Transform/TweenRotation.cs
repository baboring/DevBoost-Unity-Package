using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.Mecani {

	public class TweenRotation : TweenVector {

		#region Data

		/// <summary>
		/// The transform that is being tweened.
		/// </summary>
		[SerializeField, Header("----- Tween Rotation -----")]
		private Transform operatingTransform = null;

		/// <summary>
		/// The value for the rotation is the localEulerAngles value from the operating rotation.
		/// </summary>
		protected override Vector3 Value {
			get {
				return this.operatingTransform.localEulerAngles;
			}
			set {
				this.operatingTransform.localEulerAngles = value;
			}
		}

		#endregion

		#region MonoBehaviour

		/// <summary>
		/// Get the transform on component addition.
		/// </summary>
		protected override void Reset() {
			this.operatingTransform = this.GetComponent<Transform>();
			base.Reset();
		}

		#endregion

	}
}
