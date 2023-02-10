using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.Mecani {

	public class TweenPosition : TweenVector {

		#region Data

		/// <summary>
		/// The transform to tween.
		/// </summary>
		[SerializeField, Header("----- Tween Position -----")]
		private Transform operatingTransform = null;

		/// <summary>
		/// Flag to tween using local position or world position.
		/// </summary>
		[SerializeField]
		private bool useWorldMode = false;

		/// <summary>
		/// The positional value.
		/// </summary>
		/// <returns></returns>
		protected override Vector3 Value {
			get {
				return (this.useWorldMode) ? this.operatingTransform.position : this.operatingTransform.localPosition;
			}
			set {
				if (this.useWorldMode) {
					this.operatingTransform.position = value;
				} else {
					this.operatingTransform.localPosition = value;
				}
			}
		}

		#endregion

		#region MonoBehaviour

		/// <summary>
		/// Attach the transform on reset.
		/// </summary>
		protected override void Reset() {
			this.operatingTransform = this.GetComponent<Transform>();
			// Get the transform before calling base so that SetToCurrentValue functions work correctly.
			base.Reset();
		}

		#endregion
	}
}
